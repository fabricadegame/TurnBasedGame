﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TimeControl;

public class GameFactoryUI : UIBehavior<GameFactoryUI.UIData>
{

	#region UIData

	public class UIData : Data
	{
		
		public VP<EditData<GameFactory>> editGameFactory;

		#region gameType

		public VP<RequestChangeEnumUI.UIData> gameDataFactoryType;

		public void makeRequestChangeGameDataFactoryType (RequestChangeUpdate<int>.UpdateData update, int newGameDataFactoryType)
		{
			// Find
			GameFactory gameFactory = null;
			{
				EditData<GameFactory> editGameFactory = this.editGameFactory.v;
				if (editGameFactory != null) {
					gameFactory = editGameFactory.show.v.data;
				} else {
					Debug.LogError ("editGameFactory null: " + this);
				}
			}
			// Process
			if (gameFactory != null) {
				// Find
				GameDataFactory.Type gameDataFactoryType = (GameDataFactory.Type)newGameDataFactoryType;
				gameFactory.requestChangeGameDataFactoryType (Server.getProfileUserId(gameFactory), gameDataFactoryType);
			} else {
				Debug.LogError ("gameFactory null: " + this);
			}
		}

		#endregion

		#region GameDataFactoryUI

		public abstract class GameDataFactoryUIData : Data
		{
			
			public abstract GameDataFactory.Type getType ();

			public abstract bool processEvent (Event e);

		}

		public VP<GameDataFactoryUIData> gameDataFactoryUIData;

		#endregion

		#region timeControl

		public VP<TimeControlUI.UIData> timeControl;

		#endregion

		#region Constructor

		public enum Property
		{
			editGameFactory,
			gameDataFactoryType,
			gameDataFactoryUIData,
			timeControl
		}

		public UIData() : base()
		{
			this.editGameFactory = new VP<EditData<GameFactory>>(this, (byte)Property.editGameFactory, new EditData<GameFactory>());
			// gameType
			{
				this.gameDataFactoryType = new VP<RequestChangeEnumUI.UIData>(this, (byte)Property.gameDataFactoryType, new RequestChangeEnumUI.UIData());
				// event
				this.gameDataFactoryType.v.updateData.v.request.v = makeRequestChangeGameDataFactoryType;
				{
					foreach (GameDataFactory.Type type in System.Enum.GetValues(typeof(GameDataFactory.Type))) {
						this.gameDataFactoryType.v.options.add(type.ToString());
					}
				}
			}
			this.gameDataFactoryUIData = new VP<GameDataFactoryUIData>(this, (byte)Property.gameDataFactoryUIData, null);
			this.timeControl = new VP<TimeControlUI.UIData>(this, (byte)Property.timeControl, new TimeControlUI.UIData());
		}

		#endregion

		public bool processEvent(Event e)
		{
			Debug.LogError ("processEvent: " + e + "; " + this);
			bool isProcess = false;
			{
				// gameDataFactoryUIData
				if (!isProcess) {
					GameDataFactoryUIData gameDataFactoryUIData = this.gameDataFactoryUIData.v;
					if (gameDataFactoryUIData != null) {
						isProcess = gameDataFactoryUIData.processEvent (e);
					} else {
						Debug.LogError ("gameDataFactoryUIData null: " + this);
					}
				}
				// timeControl
				if (!isProcess) {
					TimeControlUI.UIData timeControlUIData = this.timeControl.v;
					if (timeControlUIData != null) {
						isProcess = timeControlUIData.processEvent (e);
					} else {
						Debug.LogError ("timeControlUIData null: " + this);
					}
				}
			}
			return isProcess;
		}
	}

	#endregion

	#region Refresh

	#region txt

	public Text lbTitle;
	public static readonly TxtLanguage txtTitle = new TxtLanguage();

	public Text lbGameDataFactoryType;
	public static readonly TxtLanguage txtGameDataFactoryType = new TxtLanguage ();

	static GameFactoryUI()
	{
		txtTitle.add (Language.Type.vi, "Cách Tạo Game");
		txtGameDataFactoryType.add (Language.Type.vi, "Loại tạo dữ liệu game");
	}

	#endregion

	private bool needReset = true;
	public GameObject differentIndicator;

	public override void refresh ()
	{
		if (dirty) {
			dirty = false;
			if (this.data != null) {
				EditData<GameFactory> editGameFactory = this.data.editGameFactory.v;
				if (editGameFactory != null) {
					editGameFactory.update ();
					// get show
					GameFactory show = editGameFactory.show.v.data;
					GameFactory compare = editGameFactory.compare.v.data;
					if (show != null) {
						// differentIndicator
						if (differentIndicator != null) {
							bool isDifferent = false;
							{
								if (editGameFactory.compareOtherType.v.data != null) {
									if (editGameFactory.compareOtherType.v.data.GetType () != show.GetType ()) {
										isDifferent = true;
									}
								}
							}
							differentIndicator.SetActive (isDifferent);
						} else {
							Debug.LogError ("differentIndicator null: " + this);
						}
						// request
						{
							// get server state
							Server.State.Type serverState = Server.State.Type.Connect;
							{
								Server server = show.findDataInParent<Server> ();
								if (server != null) {
									if (server.state.v != null) {
										serverState = server.state.v.getType ();
									} else {
										Debug.LogError ("server state null: " + this);
									}
								} else {
									Debug.LogError ("server null: " + this);
								}
							}
							// set origin
							{
								// gameDataFactoryType
								{
									RequestChangeEnumUI.UIData gameDataFactoryType = this.data.gameDataFactoryType.v;
									if (gameDataFactoryType != null) {
										// update
										RequestChangeUpdate<int>.UpdateData updateData = gameDataFactoryType.updateData.v;
										if (updateData != null) {
											updateData.origin.v = (int)show.getGameDataFactoryType ();
											updateData.canRequestChange.v = editGameFactory.canEdit.v;
											updateData.serverState.v = serverState;
										} else {
											Debug.LogError ("updateData null: " + this);
										}
										// compare
										{
											if (compare != null) {
												gameDataFactoryType.showDifferent.v = true;
												gameDataFactoryType.compare.v = (int)compare.getGameDataFactoryType ();
											} else {
												gameDataFactoryType.showDifferent.v = false;
											}
										}
									} else {
										Debug.LogError ("useRule null: " + this);
									}
								}
								// gameDataFactoryUIData
								{
									GameDataFactory gameDataFactory = show.gameDataFactory.v;
									if (gameDataFactory != null) {
										// find origin 
										GameDataFactory originGameDataFactory = null;
										{
											GameFactory originGameFactory = editGameFactory.origin.v.data;
											if (originGameFactory != null) {
												originGameDataFactory = originGameFactory.gameDataFactory.v;
											} else {
												Debug.LogError ("origin null: " + this);
											}
										}
										// find compare
										GameDataFactory compareGameDataFactory = null;
										{
											if (compare != null) {
												compareGameDataFactory = compare.gameDataFactory.v;
											} else {
												// Debug.LogError ("compare null: " + this);
											}
										}
										switch (gameDataFactory.getType ()) {
										case GameDataFactory.Type.Default:
											{
												DefaultGameDataFactory defaultGameDataFactory = gameDataFactory as DefaultGameDataFactory;
												// UIData
												DefaultGameDataFactoryUI.UIData defaultGameDataFactoryUIData = this.data.gameDataFactoryUIData.newOrOld<DefaultGameDataFactoryUI.UIData> ();
												{
													EditData<DefaultGameDataFactory> editDefaultGameDataFactory = defaultGameDataFactoryUIData.editDefaultGameDataFactory.v;
													if (editDefaultGameDataFactory != null) {
														// origin
														editDefaultGameDataFactory.origin.v = new ReferenceData<DefaultGameDataFactory> ((DefaultGameDataFactory)originGameDataFactory);
														// show
														editDefaultGameDataFactory.show.v = new ReferenceData<DefaultGameDataFactory> (defaultGameDataFactory);
														// compare
														editDefaultGameDataFactory.compare.v = new ReferenceData<DefaultGameDataFactory> ((DefaultGameDataFactory)compareGameDataFactory);
														// compareOtherType
														editDefaultGameDataFactory.compareOtherType.v = new ReferenceData<Data> (compareGameDataFactory);
														// canEdit
														editDefaultGameDataFactory.canEdit.v = editGameFactory.canEdit.v;
														// editType
														editDefaultGameDataFactory.editType.v = editGameFactory.editType.v;
													} else {
														Debug.LogError ("editDefaultGameDataFactory null: " + this);
													}
												}
												this.data.gameDataFactoryUIData.v = defaultGameDataFactoryUIData;
											}
											break;
										case GameDataFactory.Type.Posture:
											{
												PostureGameDataFactory postureGameDataFactory = gameDataFactory as PostureGameDataFactory;
												// UIData
												PostureGameDataFactoryUI.UIData postureGameDataFactoryUIData = this.data.gameDataFactoryUIData.newOrOld<PostureGameDataFactoryUI.UIData> ();
												{
													EditData<PostureGameDataFactory> editPostureGameDataFactory = postureGameDataFactoryUIData.editPostureGameDataFactory.v;
													if (editPostureGameDataFactory != null) {
														// origin
														editPostureGameDataFactory.origin.v = new ReferenceData<PostureGameDataFactory> ((PostureGameDataFactory)originGameDataFactory);
														// show
														editPostureGameDataFactory.show.v = new ReferenceData<PostureGameDataFactory> (postureGameDataFactory);
														// compare
														editPostureGameDataFactory.compare.v = new ReferenceData<PostureGameDataFactory> ((PostureGameDataFactory)compareGameDataFactory);
														// compareOtherType
														editPostureGameDataFactory.compareOtherType.v = new ReferenceData<Data> (compareGameDataFactory);
														// canEdit
														editPostureGameDataFactory.canEdit.v = editGameFactory.canEdit.v;
														// editType
														editPostureGameDataFactory.editType.v = editGameFactory.editType.v;
													} else {
														Debug.LogError ("editPostureGameDataFactory null: " + this);
													}
												}
												this.data.gameDataFactoryUIData.v = postureGameDataFactoryUIData;
											}
											break;
										default:
											Debug.LogError ("unknown type: " + gameDataFactory.getType () + "; " + this);
											break;
										}
									} else {
										Debug.LogError ("show null: " + this);
									}
								}
								// timeControl
								{
									TimeControlUI.UIData timeControl = this.data.timeControl.v;
									if (timeControl != null) {
										EditData<TimeControl.TimeControl> editTimeControl = timeControl.editTimeControl.v;
										if (editTimeControl != null) {
											// origin
											{
												TimeControl.TimeControl originTimeControl = null;
												{
													GameFactory originGameFactory = editGameFactory.origin.v.data;
													if (originGameFactory != null) {
														originTimeControl = originGameFactory.timeControl.v;
													} else {
														Debug.LogError ("originGameFactory null: " + this);
													}
												}
												editTimeControl.origin.v = new ReferenceData<TimeControl.TimeControl> (originTimeControl);
											}
											// show
											{
												TimeControl.TimeControl showTimeControl = null;
												{
													GameFactory showGameFactory = editGameFactory.show.v.data;
													if (showGameFactory != null) {
														showTimeControl = showGameFactory.timeControl.v;
													} else {
														Debug.LogError ("showTimeControl null: " + this);
													}
												}
												editTimeControl.show.v = new ReferenceData<TimeControl.TimeControl> (showTimeControl);
											}
											// compare
											{
												TimeControl.TimeControl compareTimeControl = null;
												{
													GameFactory compareGameFactory = editGameFactory.compare.v.data;
													if (compareGameFactory != null) {
														compareTimeControl = compareGameFactory.timeControl.v;
													} else {
														Debug.LogError ("compareGameFactory null: " + this);
													}
												}
												editTimeControl.compare.v = new ReferenceData<TimeControl.TimeControl> (compareTimeControl);
											}
											// compare other type
											{
												TimeControl.TimeControl compareOtherTypeTimeControl = null;
												{
													GameFactory compareOtherTypeGameFactory = (GameFactory)editGameFactory.compareOtherType.v.data;
													if (compareOtherTypeGameFactory != null) {
														compareOtherTypeTimeControl = compareOtherTypeGameFactory.timeControl.v;
													}
												}
												editTimeControl.compareOtherType.v = new ReferenceData<Data> (compareOtherTypeTimeControl);
											}
											// canEdit
											editTimeControl.canEdit.v = editGameFactory.canEdit.v;
											// editType
											editTimeControl.editType.v = editGameFactory.editType.v;
										} else {
											Debug.LogError ("editTimeControl null: " + this);
										}
									} else {
										Debug.LogError ("timeControl null: " + this);
									}
								}
							}
							// reset
							if (needReset) {
								needReset = false;
								// gameDataFactoryType
								{
									RequestChangeEnumUI.UIData gameDataFactoryType = this.data.gameDataFactoryType.v;
									if (gameDataFactoryType != null) {
										// update
										RequestChangeUpdate<int>.UpdateData updateData = gameDataFactoryType.updateData.v;
										if (updateData != null) {
											updateData.current.v = (int)show.getGameDataFactoryType ();
											updateData.changeState.v = Data.ChangeState.None;
										} else {
											Debug.LogError ("updateData null: " + this);
										}
									} else {
										Debug.LogError ("gameDataFactoryType null: " + this);
									}
								}
							}
						}
					} else {
						Debug.LogError ("show null: " + this);
					}
				} else {
					Debug.LogError ("editGameFactory null: " + this);
				}
				// txt
				{
					if (lbTitle != null) {
						lbTitle.text = txtTitle.get ("Game Factory");
					} else {
						Debug.LogError ("lbTitle null: " + this);
					}
					if (lbGameDataFactoryType != null) {
						lbGameDataFactoryType.text = txtGameDataFactoryType.get ("Game data factory type");
					} else {
						Debug.LogError ("lbGameDataFactoryType null: " + this);
					}
				}
			} else {
				Debug.LogError ("data null: " + this);
			}
		}
	}

	public override bool isShouldDisableUpdate ()
	{
		return true;
	}

	#endregion

	#region implement callBacks

	public Transform gameDataFactoryContainer;
	public DefaultGameDataFactoryUI defaultGameDataFactoryPrefab;
	public PostureGameDataFactoryUI postureGameDataFactoryPrefab;

	public RequestChangeEnumUI requestEnumPrefab;
	public Transform gameDataFactoryTypeContainer;

	public TimeControlUI timeControlPrefab;
	public Transform timeControlContainer;

	private Server server = null;

	public override void onAddCallBack<T> (T data)
	{
		if (data is UIData) {
			UIData uiData = data as UIData;
			// Setting
			Setting.get().addCallBack(this);
			// Child
			{
				uiData.editGameFactory.allAddCallBack (this);
				uiData.gameDataFactoryType.allAddCallBack (this);
				uiData.gameDataFactoryUIData.allAddCallBack (this);
				uiData.timeControl.allAddCallBack (this);
			}
			dirty = true;
			return;
		}
		// Setting
		if (data is Setting) {
			dirty = true;
			return;
		}
		// Child
		{
			// editGameFactory
			{
				if (data is EditData<GameFactory>) {
					EditData<GameFactory> editGameFactory = data as EditData<GameFactory>;
					// Child
					{
						editGameFactory.show.allAddCallBack (this);
						editGameFactory.compare.allAddCallBack (this);
					}
					dirty = true;
					return;
				}
				// Child
				{
					if (data is GameFactory) {
						GameFactory gameFactory = data as GameFactory;
						// Parent
						{
							DataUtils.addParentCallBack (gameFactory, this, ref this.server);
						}
						needReset = true;
						dirty = true;
						return;
					}
					// Parent
					{
						if (data is Server) {
							dirty = true;
							return;
						}
					}
				}
			}
			if (data is RequestChangeEnumUI.UIData) {
				RequestChangeEnumUI.UIData requestChange = data as RequestChangeEnumUI.UIData;
				// UI
				{
					WrapProperty wrapProperty = requestChange.p;
					if (wrapProperty != null) {
						switch ((UIData.Property)wrapProperty.n) {
						case UIData.Property.gameDataFactoryType:
							UIUtils.Instantiate (requestChange, requestEnumPrefab, gameDataFactoryTypeContainer);
							break;
						default:
							Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
							break;
						}
					} else {
						Debug.LogError ("wrapProperty null: " + this);
					}
				}
				dirty = true;
				return;
			}
			if (data is UIData.GameDataFactoryUIData) {
				UIData.GameDataFactoryUIData gameDataFactoryUIData = data as UIData.GameDataFactoryUIData;
				// UI
				{
					switch (gameDataFactoryUIData.getType ()) {
					case GameDataFactory.Type.Default:
						{
							DefaultGameDataFactoryUI.UIData defaultGameDataUIData = gameDataFactoryUIData as DefaultGameDataFactoryUI.UIData;
							UIUtils.Instantiate (defaultGameDataUIData, defaultGameDataFactoryPrefab, gameDataFactoryContainer);
						}
						break;
					case GameDataFactory.Type.Posture:
						{
							PostureGameDataFactoryUI.UIData postureGameDataFactoryUIData = gameDataFactoryUIData as PostureGameDataFactoryUI.UIData;
							UIUtils.Instantiate (postureGameDataFactoryUIData, postureGameDataFactoryPrefab, gameDataFactoryContainer);
						}
						break;
					default:
						Debug.LogError ("unknown type: " + gameDataFactoryUIData.getType () + "; " + this);
						break;
					}
				}
				dirty = true;
				return;
			}
			if (data is TimeControlUI.UIData) {
				TimeControlUI.UIData timeControlUIData = data as TimeControlUI.UIData;
				// UI
				{
					UIUtils.Instantiate (timeControlUIData, timeControlPrefab, timeControlContainer);
				}
				dirty = true;
				return;
			}
		}
		Debug.LogError ("Don't process: " + data + "; " + this);
	}

	public override void onRemoveCallBack<T> (T data, bool isHide)
	{
		if (data is UIData) {
			UIData uiData = data as UIData;
			// Setting
			Setting.get().removeCallBack(this);
			// Child
			{
				uiData.editGameFactory.allRemoveCallBack (this);
				uiData.gameDataFactoryType.allRemoveCallBack (this);
				uiData.gameDataFactoryUIData.allRemoveCallBack (this);
				uiData.timeControl.allRemoveCallBack (this);
			}
			this.setDataNull (uiData);
			return;
		}
		// Setting
		if (data is Setting) {
			return;
		}
		// Child
		{
			// editGameFactory
			{
				if (data is EditData<GameFactory>) {
					EditData<GameFactory> editGameFactory = data as EditData<GameFactory>;
					// Child
					{
						editGameFactory.show.allRemoveCallBack (this);
						editGameFactory.compare.allRemoveCallBack (this);
					}
					return;
				}
				// Child
				{
					if (data is GameFactory) {
						GameFactory gameFactory = data as GameFactory;
						// Parent
						{
							DataUtils.removeParentCallBack (gameFactory, this, ref this.server);
						}
						return;
					}
					// Parent
					{
						if (data is Server) {
							return;
						}
					}
				}
			}
			if (data is RequestChangeEnumUI.UIData) {
				RequestChangeEnumUI.UIData requestChange = data as RequestChangeEnumUI.UIData;
				// UI
				{
					requestChange.removeCallBackAndDestroy (typeof(RequestChangeEnumUI));
				}
				return;
			}
			if (data is UIData.GameDataFactoryUIData) {
				UIData.GameDataFactoryUIData gameDataFactoryUIData = data as UIData.GameDataFactoryUIData;
				// UI
				{
					switch (gameDataFactoryUIData.getType ()) {
					case GameDataFactory.Type.Default:
						{
							DefaultGameDataFactoryUI.UIData defaultGameDataUIData = gameDataFactoryUIData as DefaultGameDataFactoryUI.UIData;
							defaultGameDataUIData.removeCallBackAndDestroy (typeof(DefaultGameDataFactoryUI));
						}
						break;
					case GameDataFactory.Type.Posture:
						{
							PostureGameDataFactoryUI.UIData postureGameDataFactoryUIData = gameDataFactoryUIData as PostureGameDataFactoryUI.UIData;
							postureGameDataFactoryUIData.removeCallBackAndDestroy (typeof(PostureGameDataFactoryUI));
						}
						break;
					default:
						Debug.LogError ("unknown type: " + gameDataFactoryUIData.getType () + "; " + this);
						break;
					}
				}
				return;
			}
			if (data is TimeControlUI.UIData) {
				TimeControlUI.UIData timeControlUIData = data as TimeControlUI.UIData;
				// UI
				{
					timeControlUIData.removeCallBackAndDestroy (typeof(TimeControlUI));
				}
				return;
			}
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
			case UIData.Property.editGameFactory:
				{
					ValueChangeUtils.replaceCallBack (this, syncs);
					dirty = true;
				}
				break;
			case UIData.Property.gameDataFactoryType:
				{
					ValueChangeUtils.replaceCallBack (this, syncs);
					dirty = true;
				}
				break;
			case UIData.Property.gameDataFactoryUIData:
				{
					ValueChangeUtils.replaceCallBack (this, syncs);
					dirty = true;
				}
				break;
			case UIData.Property.timeControl:
				{
					ValueChangeUtils.replaceCallBack (this, syncs);
					dirty = true;
				}
				break;
			default:
				Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
				break;
			}
			return;
		}
		// Setting
		if (wrapProperty.p is Setting) {
			switch ((Setting.Property)wrapProperty.n) {
			case Setting.Property.language:
				dirty = true;
				break;
			case Setting.Property.showLastMove:
				break;
			case Setting.Property.viewUrlImage:
				break;
			case Setting.Property.animationSetting:
				break;
			case Setting.Property.maxThinkCount:
				break;
			default:
				Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
				break;
			}
			return;
		}
		// Child
		{
			// editGameFactory
			{
				if (wrapProperty.p is EditData<GameFactory>) {
					switch ((EditData<GameFactory>.Property)wrapProperty.n) {
					case EditData<GameFactory>.Property.origin:
						dirty = true;
						break;
					case EditData<GameFactory>.Property.show:
						{
							ValueChangeUtils.replaceCallBack (this, syncs);
							dirty = true;
						}
						break;
					case EditData<GameFactory>.Property.compare:
						{
							ValueChangeUtils.replaceCallBack (this, syncs);
							dirty = true;
						}
						break;
					case EditData<GameFactory>.Property.compareOtherType:
						dirty = true;
						break;
					case EditData<GameFactory>.Property.canEdit:
						dirty = true;
						break;
					case EditData<GameFactory>.Property.editType:
						dirty = true;
						break;
					default:
						Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
						break;
					}
					return;
				}
				// Child
				{
					if (wrapProperty.p is GameFactory) {
						switch ((GameFactory.Property)wrapProperty.n) {
						case GameFactory.Property.timeControl:
							dirty = true;
							break;
						case GameFactory.Property.gameDataFactory:
							dirty = true;
							break;
						default:
							Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
							break;
						}
						return;
					}
					// Parent
					{
						if (wrapProperty.p is Server) {
							Server.State.OnUpdateSyncStateChange (wrapProperty, this);
							return;
						}
					}
				}
			}
			if (wrapProperty.p is RequestChangeEnumUI.UIData) {
				return;
			}
			if (wrapProperty.p is UIData.GameDataFactoryUIData) {
				return;
			}
			if (wrapProperty.p is TimeControlUI.UIData) {
				return;
			}
		}
		Debug.LogError ("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
	}

	#endregion

}