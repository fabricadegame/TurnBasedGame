﻿using UnityEngine;
using UnityEngine.UI;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using SQLite4Unity3d;
using AdvancedCoroutines;
using Foundation.Tasks;

public class HaveDatabaseServerLoadDataUI : UIBehavior<HaveDatabaseServerLoadDataUI.UIData>
{

	#region UIData

	public class UIData :HaveDatabaseServerLoadUI.UIData.State
	{

		public VP<SQLiteConnection> connection;

		#region State

		public enum State
		{
			Start,
			Load
		}

		public VP<State> state;

		#endregion

		#region Constructor

		public enum Property
		{
			connection,
			state
		}

		public UIData() : base()
		{
			this.connection = new VP<SQLiteConnection>(this, (byte)Property.connection, null);
			this.state = new VP<State>(this, (byte)Property.state, State.Load);
		}

		#endregion

		public override Type getType ()
		{
			return Type.Data;
		}

	}

	#endregion

	#region Refresh

	public override void refresh ()
	{
		if (dirty) {
			dirty = false;
			if (this.data != null) {
				SQLiteConnection connection = this.data.connection.v;
				if (connection != null) {
					switch (this.data.state.v) {
					case UIData.State.Load:
						{
							destroyRoutine (loadRoutine);
							this.data.state.v = UIData.State.Start;
						}
						break;
					case UIData.State.Start:
						{
							startRoutine (ref this.loadRoutine, TaskLoad ());
						}
						break;
					default:
						Debug.LogError ("unknown state: " + this.data.state.v + "; " + this);
						break;
					}
				} else {
					Debug.LogError ("connection null: " + this);
					this.data.state.v = UIData.State.Load;
					destroyRoutine (loadRoutine);
				}
			} else {
				Debug.LogError ("data null: " + this);
			}
		}
	}

	public override bool isShouldDisableUpdate ()
	{
		return false;
	}

	#endregion

	#region Routine Load

	private Routine loadRoutine;

	public override List<Routine> getRoutineList ()
	{
		List<Routine> ret = new List<Routine> ();
		{
			ret.Add (loadRoutine);
		}
		return ret;
	}

	public IEnumerator TaskLoad()
	{
		if (this.data != null) {
			bool success = false;
			Server server = null;
			// Task
			{
				var mtask = UnityTask.Run (() => { 
					{
						try{
							if(this.data!=null){
								SQLiteConnection connection = this.data.connection.v;
								if(connection!=null){
									// get data dict
									Dictionary<int, List<SqliteData>> dataDict = new Dictionary<int, List<SqliteData>>();
									{
										TableQuery<SqliteObject> sqliteObjects = connection.Table<SqliteObject>();
										Debug.LogError("sqliteObjects: "+sqliteObjects.Count());
										foreach(SqliteObject sqliteObject in sqliteObjects){
											SqliteData sqliteData = new SqliteData(sqliteObject);
											if(sqliteData.data!=null){
												if(sqliteData.data is Server){
													server = sqliteData.data as Server;
												}else{
													// get
													List<SqliteData> dataList = null;
													{
														int idsCount = sqliteData.search.Count;
														if(!dataDict.TryGetValue(idsCount, out dataList)){
															dataList = new List<SqliteData>();
															dataDict.Add(idsCount, dataList);
														}
													}
													// add
													dataList.Add(sqliteData);
												}
											}else{
												Debug.LogError("sqliteData data null");
											}
										}
									}
									// add
									{
										// server
										{
											// server type
											Server.Type serverType = Server.Type.Offline;
											{
												if(this.data!=null){
													SqliteServerUI.UIData sqliteServerUIData = this.data.findDataInParent<SqliteServerUI.UIData>();
													if(sqliteServerUIData!=null){
														serverType = sqliteServerUIData.serverType;
													}else{
														Debug.LogError("sqliteServerUIData null");
													}
												}else{
													Debug.LogError("data null");
												}
											}// port
											int port = 7777;
											{
												// find edtPort
												InputField edtPort = null;
												{
													if(this.data!=null){
														HaveDatabaseServerUI.UIData haveDatabaseServerUIData = this.data.findDataInParent<HaveDatabaseServerUI.UIData>();
														if(haveDatabaseServerUIData!=null){
															HaveDatabaseServerUI haveDatabaseServerUI = haveDatabaseServerUIData.findCallBack<HaveDatabaseServerUI>();
															if(haveDatabaseServerUI!=null){
																edtPort = haveDatabaseServerUI.edtPort;
															}else{
																Debug.LogError("haveDatabaseServerUI null");
															}
														}else{
															Debug.LogError("haveDatabaseServerUIData null");
														}
													}else{
														Debug.LogError("data null");
													}
												}
												// get
												if (edtPort != null) {
													string strPort = edtPort.text;
													if (int.TryParse (strPort, out port)) {

													} else {
														Debug.LogError ("strPort error: " + strPort);
													}
												} else {
													Debug.LogError ("edtPort null: " + this);
												}
											}
											// set
											if(server!=null){
												server.init(serverType, port);
												{
													server.instanceId.v = Global.getRealTimeInMiliSeconds();
												}
												connection.Update(new SqliteObject(server));
											}else{
												Debug.LogError("server null");
												server = new Server();
												{
													server.init(serverType, port);
												}
												connection.Insert(new SqliteObject(server));
											}
										}
										List<History> histories = new List<History>();
										// other data
										{
											List<int> keyLists = new List<int>();
											{
												keyLists.AddRange(dataDict.Keys);
												keyLists.Sort((x,y)=> x.CompareTo(y));
											}
											foreach(int key in keyLists){
												// Debug.LogError("add sqliteDatas: idsCount: "+key);
												List<SqliteData> sqliteDatas = null;
												if(dataDict.TryGetValue(key, out sqliteDatas)){
													foreach(SqliteData sqliteData in sqliteDatas){
														// Debug.LogError("add sqlData: "+sqliteData.data);
														if(!SqliteData.AddToServer(server, sqliteData)){
															Debug.LogError("why not add add correct: "+sqliteData.search.Count+", "+sqliteData.data);
														}else{
															// Debug.LogError("add correct: "+sqliteData.search+", "+sqliteData.data);
															// add to histories
															if(sqliteData.data is History){
																histories.Add(sqliteData.data as History);
															}
														}
													}
												}else{
													Debug.LogError("key error: "+key);
												}
											}
										}
										// histories
										{
											foreach(History history in histories){
												history.humanConnections.clear();
											}
										}
										// user offline
										{
											foreach(User user in server.users.vs){
												if(user.role.v== User.Role.Normal){
													// Debug.LogError("user offline: "+user);
													if(user.human.v.state.v.state.v== UserState.State.Online){
														user.human.v.state.v.state.v = UserState.State.Disconnect;
														user.human.v.connection.v = null;
													}
												}
											}
										}
                                        // max user client count
                                        {
                                            int maxClientUserCount = LLAPITransport.DefaultMaxConnections;
                                            {
                                                // find edtMaxClientUserCount
                                                InputField edtMaxClientUserCount = null;
                                                {
                                                    if (this.data != null)
                                                    {
                                                        HaveDatabaseServerUI.UIData haveDatabaseServerUIData = this.data.findDataInParent<HaveDatabaseServerUI.UIData>();
                                                        if (haveDatabaseServerUIData != null)
                                                        {
                                                            HaveDatabaseServerUI haveDatabaseServerUI = haveDatabaseServerUIData.findCallBack<HaveDatabaseServerUI>();
                                                            if (haveDatabaseServerUI != null)
                                                            {
                                                                edtMaxClientUserCount = haveDatabaseServerUI.edtMaxClientUserCount;
                                                            }
                                                            else
                                                            {
                                                                Debug.LogError("haveDatabaseServerUI null");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Debug.LogError("haveDatabaseServerUIData null");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Debug.LogError("data null");
                                                    }
                                                }
                                                // get
                                                if (edtMaxClientUserCount != null)
                                                {
                                                    string strMaxClientUserCount = edtMaxClientUserCount.text;
                                                    if (int.TryParse(strMaxClientUserCount, out maxClientUserCount))
                                                    {

                                                    }
                                                    else
                                                    {
                                                        Debug.LogError("strMaxClientUserCount error: " + strMaxClientUserCount);
                                                    }
                                                }
                                                else
                                                {
                                                    Debug.LogError("edtMaxClientUserCount null: " + this);
                                                }
                                            }
                                            server.maxClientUserCount = maxClientUserCount;
                                        }
                                    }
									success = true;
								} else {
									Debug.LogError("connection null");
								}
							}else{
								Debug.LogError("data null");
							}
						}catch(System.Exception e){
							Debug.LogError ("Error: " + e);
						}
					}
				});
				// Wait
				{
					while (!mtask.IsCompleted) {
						yield return new Wait ();
					}
					// yield return mtask;
					if (mtask.IsFaulted) {
						Debug.LogException (mtask.Exception);
					}
				}
			}
			// Process
			if (this.data != null) {
				if (success) {
					if (server != null) {
						HaveDatabaseServerUI.UIData haveDatabaseServerUIData = this.data.findDataInParent<HaveDatabaseServerUI.UIData> ();
						if (haveDatabaseServerUIData != null) {
							HaveDatabaseServerUpdateUI.UIData updateUIData = new HaveDatabaseServerUpdateUI.UIData ();
							{
								updateUIData.uid = haveDatabaseServerUIData.sub.makeId ();
								// sqliteConnection
								updateUIData.sqliteConnection.v = this.data.connection.v;
								// // sqliteUpdate
								{
									updateUIData.sqliteUpdate.v.connection.v = this.data.connection.v;
									updateUIData.sqliteUpdate.v.server.v = new ReferenceData<Server> (server);
								}
								// serverManager
								{
									ServerManager.UIData severManagerUIData = new ServerManager.UIData ();
									{
										severManagerUIData.uid = updateUIData.serverManager.makeId ();
										severManagerUIData.server.v = new ReferenceData<Server> (server);
									}
									updateUIData.serverManager.v = severManagerUIData;
								}
							}
							haveDatabaseServerUIData.sub.v = updateUIData;
						} else {
							Debug.LogError ("haveDatabaseServerUIData null: " + this);
						}
					} else {
						Debug.LogError ("sever null: " + this);
						HaveDatabaseServerFailUI.UIData.changeToFail (this.data);
					}
				} else {
					HaveDatabaseServerFailUI.UIData.changeToFail (this.data);
				}
			} else {
				Debug.LogError ("data null");
			}
		} else {
			Debug.LogError ("inputData null");
		}
	}

	#endregion

	#region implement callBacks

	public override void onAddCallBack<T> (T data)
	{
		if (data is UIData) {
			dirty = true;
			return;
		}
		Debug.LogError ("Don't process: " + data + "; " + this);
	}

	public override void onRemoveCallBack<T> (T data, bool isHide)
	{
		if (data is UIData) {
			UIData uiData = data as UIData;
			// Child
			{

			}
			this.setDataNull (uiData);
			return;
		}
		Debug.LogError ("Don't process: " + data + "; " + this);
	}

	public override void onUpdateSync<T> (WrapProperty wrapProperty, List<Sync<T>> syncs)
	{
		if (WrapProperty.checkError (wrapProperty)) {
			return;
		}
		if (wrapProperty.p is UIData) {
			switch ((UIData.Property)wrapProperty.n) {
			case UIData.Property.connection:
				{
					// reset
					if (this.data != null) {
						this.data.state.v = UIData.State.Load;
					} else {
						Debug.LogError ("data null: " + this);
					}
				}
				break;
			case UIData.Property.state:
				dirty = true;
				break;
			default:
				Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
				break;
			}
			return;
		}
		Debug.LogError ("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
	}

	#endregion

}