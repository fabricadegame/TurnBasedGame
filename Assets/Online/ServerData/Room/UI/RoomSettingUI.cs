﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Rights;

public class RoomSettingUI : UIBehavior<RoomSettingUI.UIData>
{

	#region UIData

	public class UIData : Data
	{

		public VP<EditData<Room>> editRoom;

		#region name

		public VP<RequestChangeStringUI.UIData> name;

		public void makeRequestChangeName (RequestChangeUpdate<string>.UpdateData update, string newName)
		{
			// Find
			Room room = null;
			{
				EditData<Room> editRoom = this.editRoom.v;
				if (editRoom != null) {
					room = editRoom.show.v.data;
				} else {
					Debug.LogError ("editRoom null: " + this);
				}
			}
			// Process
			if (room != null) {
				room.requestChangeName (Server.getProfileUserId (room), newName);
			} else {
				Debug.LogError ("room null: " + this);
			}
		}

		#endregion

		#region allowHint

		public VP<RequestChangeEnumUI.UIData> allowHint;

		public void makeRequestChangeAllowHint (RequestChangeUpdate<int>.UpdateData update, int newAllowHint)
		{
			// Find
			Room room = null;
			{
				EditData<Room> editRoom = this.editRoom.v;
				if (editRoom != null) {
					room = editRoom.show.v.data;
				} else {
					Debug.LogError ("editRoom null: " + this);
				}
			}
			// Process
			if (room != null) {
				room.requestChangeAllowHint (Server.getProfileUserId (room), newAllowHint);
			} else {
				Debug.LogError ("room null: " + this);
			}
		}

		#endregion

		public VP<ChangeRightsUI.UIData> changeRights;

		#region Constructor

		public enum Property
		{
			editRoom,
			name,
			allowHint,
			changeRights
		}

		public static readonly List<byte> AllowNames = new List<byte>();

		static UIData()
		{
			AllowNames.Add((byte)Room.Property.name);
			AllowNames.Add((byte)Room.Property.changeRights);
			AllowNames.Add((byte)Room.Property.allowHint);
		}

		public UIData() : base()
		{
			// editRoom
			{
				EditData<Room> editDataRoom = new EditData<Room>();
				{
					editDataRoom.allowNames = AllowNames;
				}
				this.editRoom = new VP<EditData<Room>>(this, (byte)Property.editRoom, editDataRoom);
			}
			// name
			{
				this.name = new VP<RequestChangeStringUI.UIData>(this, (byte)Property.name, new RequestChangeStringUI.UIData());
				this.name.v.updateData.v.request.v = makeRequestChangeName;
			}
			// allowHint
			{
				this.allowHint = new VP<RequestChangeEnumUI.UIData>(this, (byte)Property.allowHint, new RequestChangeEnumUI.UIData());
				// event
				this.allowHint.v.updateData.v.request.v = makeRequestChangeAllowHint;
				{
					foreach (Room.AllowHint type in System.Enum.GetValues(typeof(Room.AllowHint))) {
						this.allowHint.v.options.add(type.ToString());
					}
				}
			}
			this.changeRights = new VP<ChangeRightsUI.UIData>(this, (byte)Property.changeRights, new ChangeRightsUI.UIData());
		}

		#endregion

		public bool processEvent(Event e)
		{
			bool isProcess = false;
			{

			}
			return isProcess;
		}

	}

	#endregion
	
	#region Refresh

	#region txt

	public Text lbTitle;
	public static readonly TxtLanguage txtTitle = new TxtLanguage();

	public Text lbName;
	public static readonly TxtLanguage txtName = new TxtLanguage ();

	public Text lbAllowHint;
	public static readonly TxtLanguage txtAllowHint = new TxtLanguage();

	static RoomSettingUI()
	{
		txtTitle.add (Language.Type.vi, "Thiết Lập Phòng");
		txtName.add (Language.Type.vi, "Tên");
		txtAllowHint.add (Language.Type.vi, "Cho phép gợi ý");
	}

	#endregion

	private bool needReset = true;
	public GameObject differentIndicator;

	public override void refresh ()
	{
		if (dirty) {
			dirty = false;
			if (this.data != null) {
				EditData<Room> editRoom = this.data.editRoom.v;
				if (editRoom != null) {
					editRoom.update ();
					// get show
					Room show = editRoom.show.v.data;
					Room compare = editRoom.compare.v.data;
					if (show != null) {
						// differentIndicator
						if (differentIndicator != null) {
							bool isDifferent = false;
							{
								if (editRoom.compareOtherType.v.data != null) {
									if (editRoom.compareOtherType.v.data.GetType () != show.GetType ()) {
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
								// name
								{
									RequestChangeStringUI.UIData name = this.data.name.v;
									if (name != null) {
										// update
										RequestChangeUpdate<string>.UpdateData updateData = name.updateData.v;
										if (updateData != null) {
											updateData.origin.v = show.name.v;
											updateData.canRequestChange.v = editRoom.canEdit.v;
											updateData.serverState.v = serverState;
										} else {
											Debug.LogError ("updateData null: " + this);
										}
										// compare
										{
											if (compare != null) {
												name.showDifferent.v = true;
												name.compare.v = compare.name.v;
											} else {
												name.showDifferent.v = false;
											}
										}
									} else {
										Debug.LogError ("name null: " + this);
									}
								}
								// allowHint
								{
									RequestChangeEnumUI.UIData allowHint = this.data.allowHint.v;
									if (allowHint != null) {
										// update
										RequestChangeUpdate<int>.UpdateData updateData = allowHint.updateData.v;
										if (updateData != null) {
											updateData.origin.v = (int)show.allowHint.v;
											updateData.canRequestChange.v = editRoom.canEdit.v;
											updateData.serverState.v = serverState;
										} else {
											Debug.LogError ("updateData null: " + this);
										}
										// compare
										{
											if (compare != null) {
												allowHint.showDifferent.v = true;
												allowHint.compare.v = (int)compare.allowHint.v;
											} else {
												allowHint.showDifferent.v = false;
											}
										}
									} else {
										Debug.LogError ("allowHint null: " + this);
									}
								}
								// changeRights
								{
									ChangeRightsUI.UIData changeRights = this.data.changeRights.v;
									if (changeRights != null) {
										EditData<ChangeRights> editChangeRights = changeRights.editChangeRights.v;
										if (editChangeRights != null) {
											// origin
											{
												ChangeRights originChangeRights = null;
												{
													Room originRoom = editRoom.origin.v.data;
													if (originRoom != null) {
														originChangeRights = originRoom.changeRights.v;
													} else {
														Debug.LogError ("originRoom null: " + this);
													}
												}
												editChangeRights.origin.v = new ReferenceData<ChangeRights> (originChangeRights);
											}
											// show
											{
												ChangeRights showChangeRights = null;
												{
													Room showRoom = editRoom.show.v.data;
													if (showRoom != null) {
														showChangeRights = showRoom.changeRights.v;
													} else {
														Debug.LogError ("showRoom null: " + this);
													}
												}
												editChangeRights.show.v = new ReferenceData<ChangeRights> (showChangeRights);
											}
											// compare
											{
												ChangeRights compareChangeRights = null;
												{
													Room compareRoom = editRoom.compare.v.data;
													if (compareRoom != null) {
														compareChangeRights = compareRoom.changeRights.v;
													} else {
														Debug.LogError ("compareChangeRights null: " + this);
													}
												}
												editChangeRights.compare.v = new ReferenceData<ChangeRights> (compareChangeRights);
											}
											// compare other type
											{
												ChangeRights compareOtherTypeChangeRights = null;
												{
													Room compareOtherTypeRoom = (Room)editRoom.compareOtherType.v.data;
													if (compareOtherTypeRoom != null) {
														compareOtherTypeChangeRights = compareOtherTypeRoom.changeRights.v;
													}
												}
												editChangeRights.compareOtherType.v = new ReferenceData<Data> (compareOtherTypeChangeRights);
											}
											// canEdit
											editChangeRights.canEdit.v = editRoom.canEdit.v;
											// editType
											editChangeRights.editType.v = editRoom.editType.v;
										} else {
											Debug.LogError ("editChangeRights null: " + this);
										}
									} else {
										Debug.LogError ("changeRights null: " + this);
									}
								}
							}
							// reset
							if (needReset) {
								needReset = false;
								// name
								{
									RequestChangeStringUI.UIData name = this.data.name.v;
									if (name != null) {
										// update
										RequestChangeUpdate<string>.UpdateData updateData = name.updateData.v;
										if (updateData != null) {
											updateData.current.v = show.name.v;
											updateData.changeState.v = Data.ChangeState.None;
										} else {
											Debug.LogError ("updateData null: " + this);
										}
									} else {
										Debug.LogError ("name null: " + this);
									}
								}
								// allowHint
								{
									RequestChangeEnumUI.UIData allowHint = this.data.allowHint.v;
									if (allowHint != null) {
										// update
										RequestChangeUpdate<int>.UpdateData updateData = allowHint.updateData.v;
										if (updateData != null) {
											updateData.current.v = (int)show.allowHint.v;
											updateData.changeState.v = Data.ChangeState.None;
										} else {
											Debug.LogError ("updateData null: " + this);
										}
									} else {
										Debug.LogError ("allowHint null: " + this);
									}
								}
							}
						}
					} else {
						Debug.LogError ("show null: " + this);
					}
				} else {
					Debug.LogError ("editRoom null: " + this);
				}
				// txt
				{
					if (lbTitle != null) {
						lbTitle.text = txtTitle.get ("Room Setting");
					} else {
						Debug.LogError ("lbTitle null: " + this);
					}
					if (lbName != null) {
						lbName.text = txtName.get ("Name");
					} else {
						Debug.LogError ("lbName null: " + this);
					}
					if (lbAllowHint != null) {
						lbAllowHint.text = txtAllowHint.get ("Allow Hint");
					} else {
						Debug.LogError ("lbAllowHint null: " + this);
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

	public RequestChangeStringUI requestStringPrefab;
	public RequestChangeEnumUI requestEnumPrefab;
	public ChangeRightsUI changeRightsPrefab;

	public Transform nameContainer;
	public Transform allowHintContainer;
	public Transform changeRightsContainer;

	private Server server = null;

	public override void onAddCallBack<T> (T data)
	{
		if (data is UIData) {
			UIData uiData = data as UIData;
			// Setting
			Setting.get().addCallBack(this);
			// Child
			{
				uiData.editRoom.allAddCallBack (this);
				uiData.name.allAddCallBack (this);
				uiData.allowHint.allAddCallBack (this);
				uiData.changeRights.allAddCallBack (this);
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
			// editRoom
			{
				if (data is EditData<Room>) {
					EditData<Room> editRoom = data as EditData<Room>;
					// Child
					{
						editRoom.show.allAddCallBack (this);
						editRoom.compare.allAddCallBack (this);
					}
					dirty = true;
					return;
				}
				// Child
				{
					if (data is Room) {
						Room room = data as Room;
						// Parent
						{
							DataUtils.addParentCallBack (room, this, ref this.server);
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
			// name
			if (data is RequestChangeStringUI.UIData) {
				RequestChangeStringUI.UIData requestChange = data as RequestChangeStringUI.UIData;
				// UI
				{
					WrapProperty wrapProperty = requestChange.p;
					if (wrapProperty != null) {
						switch ((UIData.Property)wrapProperty.n) {
						case UIData.Property.name:
							UIUtils.Instantiate (requestChange, requestStringPrefab, nameContainer);
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
			// allowHint
			if (data is RequestChangeEnumUI.UIData) {
				RequestChangeEnumUI.UIData requestChange = data as RequestChangeEnumUI.UIData;
				// UI
				{
					WrapProperty wrapProperty = requestChange.p;
					if (wrapProperty != null) {
						switch ((UIData.Property)wrapProperty.n) {
						case UIData.Property.allowHint:
							UIUtils.Instantiate (requestChange, requestEnumPrefab, allowHintContainer);
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
			// changeRights
			if (data is ChangeRightsUI.UIData) {
				ChangeRightsUI.UIData changeRightsUIData = data as ChangeRightsUI.UIData;
				// UI
				{
					UIUtils.Instantiate (changeRightsUIData, changeRightsPrefab, changeRightsContainer);
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
				uiData.editRoom.allRemoveCallBack (this);
				uiData.name.allRemoveCallBack (this);
				uiData.allowHint.allRemoveCallBack (this);
				uiData.changeRights.allRemoveCallBack (this);
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
			// editRoom
			{
				if (data is EditData<Room>) {
					EditData<Room> editRoom = data as EditData<Room>;
					// Child
					{
						editRoom.show.allRemoveCallBack (this);
						editRoom.compare.allRemoveCallBack (this);
					}
					return;
				}
				// Child
				{
					if (data is Room) {
						Room room = data as Room;
						// Parent
						{
							DataUtils.removeParentCallBack (room, this, ref this.server);
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
			// name
			if (data is RequestChangeStringUI.UIData) {
				RequestChangeStringUI.UIData requestChange = data as RequestChangeStringUI.UIData;
				// UI
				{
					requestChange.removeCallBackAndDestroy (typeof(RequestChangeStringUI));
				}
				return;
			}
			// allowHint
			if (data is RequestChangeEnumUI.UIData) {
				RequestChangeEnumUI.UIData requestChange = data as RequestChangeEnumUI.UIData;
				// UI
				{
					requestChange.removeCallBackAndDestroy (typeof(RequestChangeEnumUI));
				}
				return;
			}
			// changeRights
			if (data is ChangeRightsUI.UIData) {
				ChangeRightsUI.UIData changeRightsUIData = data as ChangeRightsUI.UIData;
				// UI
				{
					changeRightsUIData.removeCallBackAndDestroy (typeof(ChangeRightsUI));
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
			case UIData.Property.editRoom:
				{
					ValueChangeUtils.replaceCallBack (this, syncs);
					dirty = true;
				}
				break;
			case UIData.Property.name:
				{
					ValueChangeUtils.replaceCallBack (this, syncs);
					dirty = true;
				}
				break;
			case UIData.Property.allowHint:
				{
					ValueChangeUtils.replaceCallBack (this, syncs);
					dirty = true;
				}
				break;
			case UIData.Property.changeRights:
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
			// editRoom
			{
				if (wrapProperty.p is EditData<Room>) {
					switch ((EditData<Room>.Property)wrapProperty.n) {
					case EditData<Room>.Property.origin:
						dirty = true;
						break;
					case EditData<Room>.Property.show:
						{
							ValueChangeUtils.replaceCallBack (this, syncs);
							dirty = true;
						}
						break;
					case EditData<Room>.Property.compare:
						{
							ValueChangeUtils.replaceCallBack (this, syncs);
							dirty = true;
						}
						break;
					case EditData<Room>.Property.compareOtherType:
						dirty = true;
						break;
					case EditData<Room>.Property.canEdit:
						dirty = true;
						break;
					case EditData<Room>.Property.editType:
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
					if (wrapProperty.p is Room) {
						switch ((Room.Property)wrapProperty.n) {
						case Room.Property.changeRights:
							dirty = true;
							break;
						case Room.Property.name:
							dirty = true;
							break;
						case Room.Property.password:
							break;
						case Room.Property.users:
							break;
						case Room.Property.state:
							break;
						case Room.Property.contestManagers:
							break;
						case Room.Property.timeCreated:
							break;
						case Room.Property.chatRoom:
							break;
						case Room.Property.allowHint:
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
			// name
			if (wrapProperty.p is RequestChangeStringUI.UIData) {
				return;
			}
			// allowHint
			if (wrapProperty.p is RequestChangeEnumUI.UIData) {
				return;
			}
			// changeRights
			if (wrapProperty.p is ChangeRightsUI.UIData) {
				return;
			}
		}
		Debug.LogError ("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
	}

	#endregion

}