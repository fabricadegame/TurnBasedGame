﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class GlobalViewUI : UIBehavior<GlobalViewUI.UIData>
{

	#region UIData 

	public class UIData : AfterLoginUI.UIData.Sub
	{

		public VP<ReferenceData<Server>> server;

		public VP<AfterLoginMainBtnBackUI.UIData> btnBack;

		public VP<GlobalStateUI.UIData> state;

		#region Show

		public enum Show
		{
			rooms,
			chats,
			friends,
			profile
		}

		public VP<Show> show;

		#endregion

		public VP<GlobalRoomsUI.UIData> rooms;

		public VP<GlobalChatUI.UIData> chats;

		public VP<GlobalFriendsUI.UIData> friends;

		public VP<GlobalProfileUI.UIData> profile;

		#region Constructor

		public enum Property
		{
			server,
			btnBack,
			state,
			show,
			rooms,
			chats,
			friends,
			profile
		}

		public UIData() : base()
		{
			this.server = new VP<ReferenceData<Server>>(this, (byte)Property.server, new ReferenceData<Server>(null));
			this.btnBack = new VP<AfterLoginMainBtnBackUI.UIData>(this, (byte)Property.btnBack, new AfterLoginMainBtnBackUI.UIData());
			this.state = new VP<GlobalStateUI.UIData>(this, (byte)Property.state, new GlobalStateUI.UIData());
			this.show = new VP<Show>(this, (byte)Property.show, Show.rooms);
			this.rooms = new VP<GlobalRoomsUI.UIData>(this, (byte)Property.rooms, null);
			this.chats = new VP<GlobalChatUI.UIData>(this, (byte)Property.chats, null);
			this.friends = new VP<GlobalFriendsUI.UIData>(this, (byte)Property.friends, null);
			this.profile = new VP<GlobalProfileUI.UIData>(this, (byte)Property.profile, null);
		}

		#endregion

		public override Type getType ()
		{
			return Type.View;
		}

		public void reset()
		{
			this.show.v = Show.rooms;
		}

		public override bool processEvent (Event e)
		{
			bool isProcess = false;
			{
				// show
				if (!isProcess) {
					switch (this.show.v) {
					case Show.rooms:
						{
							GlobalRoomsUI.UIData rooms = this.rooms.v;
							if (rooms != null) {
								isProcess = rooms.processEvent (e);
							} else {
								Debug.LogError ("rooms null: " + this);
							}
						}
						break;
					case Show.chats:
						{
							GlobalChatUI.UIData chats = this.chats.v;
							if (chats != null) {
								isProcess = chats.processEvent (e);
							} else {
								Debug.LogError ("chats null: " + this);
							}
						}
						break;
					case Show.friends:
						{
							GlobalFriendsUI.UIData friends = this.friends.v;
							if (friends != null) {
								isProcess = friends.processEvent (e);
							} else {
								Debug.LogError ("friends null: " + this);
							}
						}
						break;
					case Show.profile:
						{
							GlobalProfileUI.UIData profile = this.profile.v;
							if (profile != null) {
								isProcess = profile.processEvent (e);
							} else {
								Debug.LogError ("profile null: " + this);
							}
						}
						break;
					default:
						Debug.LogError ("unknown show: " + this.show.v + "; " + this);
						break;
					}
				}
				// btnBack
				if (!isProcess) {
					AfterLoginMainBtnBackUI.UIData btnBack = this.btnBack.v;
					if (btnBack != null) {
						isProcess = btnBack.processEvent (e);
					} else {
						Debug.LogError ("btnBack null: " + this);
					}
				}
			}
			return isProcess;
		}

	}

	#endregion

	#region Refresh

	#region txt

	public Text tvRooms;
	public static readonly TxtLanguage txtRooms = new TxtLanguage();

	public Text tvChats;
	public static readonly TxtLanguage txtChats = new TxtLanguage();

	public Text tvFriends;
	public static readonly TxtLanguage txtFriends = new TxtLanguage();

	public Text tvProfile;
	public static readonly TxtLanguage txtProfile = new TxtLanguage();

	static GlobalViewUI()
	{
		txtRooms.add (Language.Type.vi, "Các Phòng");
		txtChats.add (Language.Type.vi, "Trò chuyện");
		txtFriends.add (Language.Type.vi, "Bạn bè");
		txtProfile.add (Language.Type.vi, "Tiểu sử");
	}

	#endregion

	public Button btnRooms;
	public Button btnChats;
	public Button btnFriends;
	public Button btnProfile;

	public override void refresh ()
	{
		if (dirty) {
			dirty = false;
			if (this.data != null) {
				// show
				{
					if (roomsContainer != null && chatsContainer != null && friendsContainer != null && profileContainer != null) {
						if (btnRooms != null && btnChats != null && btnFriends != null && btnProfile != null) {
							switch (this.data.show.v) {
							case UIData.Show.rooms:
								{
									// container
									{
										roomsContainer.gameObject.SetActive (true);
										chatsContainer.gameObject.SetActive (false);
										friendsContainer.gameObject.SetActive (false);
										profileContainer.gameObject.SetActive (false);
									}
									// btns
									{
										btnRooms.enabled = false;
										btnChats.enabled = true;
										btnFriends.enabled = true;
										btnProfile.enabled = true;
									}
								}
								break;
							case UIData.Show.chats:
								{
									// container
									{
										roomsContainer.gameObject.SetActive (false);
										chatsContainer.gameObject.SetActive (true);
										friendsContainer.gameObject.SetActive (false);
										profileContainer.gameObject.SetActive (false);
									}
									// btns
									{
										btnRooms.enabled = true;
										btnChats.enabled = false;
										btnFriends.enabled = true;
										btnProfile.enabled = true;
									}
								}
								break;
							case UIData.Show.friends:
								{
									// container
									{
										roomsContainer.gameObject.SetActive (false);
										chatsContainer.gameObject.SetActive (false);
										friendsContainer.gameObject.SetActive (true);
										profileContainer.gameObject.SetActive (false);
									}
									// btns
									{
										btnRooms.enabled = true;
										btnChats.enabled = true;
										btnFriends.enabled = false;
										btnProfile.enabled = true;
									}
								}
								break;
							case UIData.Show.profile:
								{
									// container
									{
										roomsContainer.gameObject.SetActive (false);
										chatsContainer.gameObject.SetActive (false);
										friendsContainer.gameObject.SetActive (false);
										profileContainer.gameObject.SetActive (true);
									}
									// btns
									{
										btnRooms.enabled = true;
										btnChats.enabled = true;
										btnFriends.enabled = true;
										btnProfile.enabled = false;
									}
								}
								break;
							default:
								Debug.LogError ("unknown show: " + this.data.show.v + "; " + this);
								break;
							}
						} else {
							Debug.LogError ("btn null: " + this);
						}
					} else {
						Debug.LogError ("container null: " + this);
					}
				}
				// set child ui
				{
					switch (this.data.show.v) {
					case UIData.Show.rooms:
						{
							GlobalRoomsUI.UIData globalRoomsUIData = this.data.rooms.newOrOld<GlobalRoomsUI.UIData> ();
							{

							}
							this.data.rooms.v = globalRoomsUIData;
						}
						break;
					case UIData.Show.chats:
						{
							GlobalChatUI.UIData globalChatUIData = this.data.chats.newOrOld<GlobalChatUI.UIData> ();
							{

							}
							this.data.chats.v = globalChatUIData;
						}
						break;
					case UIData.Show.friends:
						{
							GlobalFriendsUI.UIData globalFriendsUIData = this.data.friends.newOrOld<GlobalFriendsUI.UIData> ();
							{

							}
							this.data.friends.v = globalFriendsUIData;
						}
						break;
					case UIData.Show.profile:
						{
							GlobalProfileUI.UIData profileUIData = this.data.profile.newOrOld<GlobalProfileUI.UIData> ();
							{

							}
							this.data.profile.v = profileUIData;
						}
						break;
					default:
						Debug.LogError ("unknown show: " + this.data.show.v + "; " + this);
						break;
					}
				}
				// Update Child UI
				{
					Server server = this.data.server.v.data;
					if (server != null) {
						// btnBack
						{
							AfterLoginMainBtnBackUI.UIData btnBack = this.data.btnBack.v;
							if (btnBack != null) {
								btnBack.server.v = new ReferenceData<Server> (server);
							} else {
								Debug.LogError ("btnBack null: " + this);
							}
						}
						// state
						{
							GlobalStateUI.UIData state = this.data.state.v;
							if (state != null) {
								state.server.v = new ReferenceData<Server> (server);
							} else {
								Debug.LogError ("state null: " + this);
							}
						}
						// rooms
						{
							GlobalRoomsUI.UIData rooms = this.data.rooms.v;
							if (rooms != null) {
								rooms.server.v = new ReferenceData<Server> (server);
							} else {
								Debug.LogError ("rooms null: " + this);
							}
						}
						// chats
						{
							GlobalChatUI.UIData chats = this.data.chats.v;
							if (chats != null) {
								chats.server.v = new ReferenceData<Server> (server);
							} else {
								Debug.LogError ("chats null: " + this);
							}
						}
						// friends
						{
							GlobalFriendsUI.UIData friends = this.data.friends.v;
							if (friends != null) {
								friends.server.v = new ReferenceData<Server> (server);
							} else {
								Debug.LogError ("friends null: " + this);
							}
						}
						// profile
						{
							GlobalProfileUI.UIData profile = this.data.profile.v;
							if (profile != null) {
								profile.server.v = new ReferenceData<Server> (server);
							} else {
								Debug.LogError ("profile null: " + this);
							}
						}
					} else {
						Debug.LogError ("server null: " + this);
					}
				}
				// txt
				{
					if (tvRooms != null) {
						tvRooms.text = txtRooms.get ("Rooms");
					} else {
						Debug.LogError ("tvRooms null: " + this);
					}
					if (tvChats != null) {
						tvChats.text = txtChats.get ("Chats");
					} else {
						Debug.LogError ("tvChats null: " + this);
					}
					if (tvFriends != null) {
						tvFriends.text = txtFriends.get ("Friends");
					} else {
						Debug.LogError ("tvFriends null: " + this);
					}
					if (tvProfile != null) {
						tvProfile.text = txtProfile.get ("Profile");
					} else {
						Debug.LogError ("tvProfile null: " + this);
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

	public AfterLoginMainBtnBackUI btnBackPrefab;
	public Transform btnBackContainer;

	public GlobalStateUI statePrefab;
	public Transform stateContainer;

	public GlobalRoomsUI roomsPrefab;
	public GlobalChatUI chatsPrefab;
	public GlobalFriendsUI friendsPrefab;
	public GlobalProfileUI profilePrefab;

	public Transform roomsContainer;
	public Transform chatsContainer;
	public Transform friendsContainer;
	public Transform profileContainer;

	public override void onAddCallBack<T> (T data)
	{
		if (data is UIData) {
			UIData uiData = data as UIData;
			// Setting
			Setting.get().addCallBack(this);
			// Child
			{
				uiData.server.allAddCallBack (this);
				uiData.btnBack.allAddCallBack (this);
				uiData.state.allAddCallBack (this);
				uiData.rooms.allAddCallBack (this);
				uiData.chats.allAddCallBack (this);
				uiData.friends.allAddCallBack (this);
				uiData.profile.allAddCallBack (this);
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
			if (data is Server) {
				// Reset
				{
					if (this.data != null) {
						this.data.reset ();
					} else {
						Debug.LogError ("data null: " + this);
					}
				}
				dirty = true;
				return;
			}
			if (data is AfterLoginMainBtnBackUI.UIData) {
				AfterLoginMainBtnBackUI.UIData btnBack = data as AfterLoginMainBtnBackUI.UIData;
				// UI
				{
					UIUtils.Instantiate (btnBack, btnBackPrefab, btnBackContainer);
				}
				dirty = true;
				return;
			}
			if (data is GlobalStateUI.UIData) {
				GlobalStateUI.UIData state = data as GlobalStateUI.UIData;
				// UI
				{
					UIUtils.Instantiate (state, statePrefab, stateContainer);
				}
				dirty = true;
				return;
			}
			if (data is GlobalRoomsUI.UIData) {
				GlobalRoomsUI.UIData rooms = data as GlobalRoomsUI.UIData;
				// UI
				{
					UIUtils.Instantiate (rooms, roomsPrefab, roomsContainer);
				}
				dirty = true;
				return;
			}
			if (data is GlobalChatUI.UIData) {
				GlobalChatUI.UIData chats = data as GlobalChatUI.UIData;
				// UI
				{
					UIUtils.Instantiate (chats, chatsPrefab, chatsContainer);
				}
				dirty = true;
				return;
			}
			if (data is GlobalFriendsUI.UIData) {
				GlobalFriendsUI.UIData friends = data as GlobalFriendsUI.UIData;
				// UI
				{
					UIUtils.Instantiate (friends, friendsPrefab, friendsContainer);
				}
				dirty = true;
				return;
			}
			if (data is GlobalProfileUI.UIData) {
				GlobalProfileUI.UIData profile = data as GlobalProfileUI.UIData;
				// UI
				{
					UIUtils.Instantiate (profile, profilePrefab, profileContainer);
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
				uiData.server.allRemoveCallBack (this);
				uiData.btnBack.allRemoveCallBack (this);
				uiData.state.allRemoveCallBack (this);
				uiData.rooms.allRemoveCallBack (this);
				uiData.chats.allRemoveCallBack (this);
				uiData.friends.allRemoveCallBack (this);
				uiData.profile.allRemoveCallBack (this);
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
			if (data is Server) {
				return;
			}
			if(data is AfterLoginMainBtnBackUI.UIData){
				AfterLoginMainBtnBackUI.UIData btnBack = data as AfterLoginMainBtnBackUI.UIData;
				// UI
				{
					btnBack.removeCallBackAndDestroy (typeof(AfterLoginMainBtnBackUI));
				}
				return;
			}
			if (data is GlobalStateUI.UIData) {
				GlobalStateUI.UIData state = data as GlobalStateUI.UIData;
				// UI
				{
					state.removeCallBackAndDestroy (typeof(GlobalStateUI));
				}
				return;
			}
			if (data is GlobalRoomsUI.UIData) {
				GlobalRoomsUI.UIData rooms = data as GlobalRoomsUI.UIData;
				// UI
				{
					rooms.removeCallBackAndDestroy (typeof(GlobalRoomsUI));
				}
				return;
			}
			if (data is GlobalChatUI.UIData) {
				GlobalChatUI.UIData chats = data as GlobalChatUI.UIData;
				// UI
				{
					chats.removeCallBackAndDestroy (typeof(GlobalChatUI));
				}
				return;
			}
			if (data is GlobalFriendsUI.UIData) {
				GlobalFriendsUI.UIData friends = data as GlobalFriendsUI.UIData;
				// UI
				{
					friends.removeCallBackAndDestroy (typeof(GlobalFriendsUI));
				}
				return;
			}
			if (data is GlobalProfileUI.UIData) {
				GlobalProfileUI.UIData profile = data as GlobalProfileUI.UIData;
				// UI
				{
					profile.removeCallBackAndDestroy(typeof(GlobalProfileUI));
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
			case UIData.Property.server:
				{
					ValueChangeUtils.replaceCallBack (this, syncs);
					dirty = true;
				}
				break;
			case UIData.Property.btnBack:
				{
					ValueChangeUtils.replaceCallBack (this, syncs);
					dirty = true;
				}
				break;
			case UIData.Property.state:
				{
					ValueChangeUtils.replaceCallBack (this, syncs);
					dirty = true;
				}
				break;
			case UIData.Property.show:
				dirty = true;
				break;
			case UIData.Property.rooms:
				{
					ValueChangeUtils.replaceCallBack (this, syncs);
					dirty = true;
				}
				break;
			case UIData.Property.chats:
				{
					ValueChangeUtils.replaceCallBack (this, syncs);
					dirty = true;
				}
				break;
			case UIData.Property.friends:
				{
					ValueChangeUtils.replaceCallBack (this, syncs);
					dirty = true;
				}
				break;
			case UIData.Property.profile:
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
			if (wrapProperty.p is Server) {
				return;
			}
			if (wrapProperty.p is AfterLoginMainBtnBackUI.UIData) {
				return;
			}
			if (wrapProperty.p is GlobalStateUI.UIData) {
				return;
			}
			if (wrapProperty.p is GlobalRoomsUI.UIData) {
				return;
			}
			if (wrapProperty.p is GlobalChatUI.UIData) {
				return;
			}
			if (wrapProperty.p is GlobalFriendsUI.UIData) {
				return;
			}
			if (wrapProperty.p is GlobalProfileUI.UIData) {
				return;
			}
		}
		Debug.LogError ("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
	}

	#endregion

	#region btns

	public void onClickBtnRooms()
	{
		if (this.data != null) {
			this.data.show.v = UIData.Show.rooms;
		} else {
			Debug.LogError ("data null: " + this);
		}
	}

	public void onClickBtnChats()
	{
		if (this.data != null) {
			this.data.show.v = UIData.Show.chats;
		} else {
			Debug.LogError ("data null: " + this);
		}
	}

	public void onClickBtnFriends()
	{
		if (this.data != null) {
			this.data.show.v = UIData.Show.friends;
		} else {
			Debug.LogError ("data null: " + this);
		}
	}

	public void onClickBtnProfile()
	{
		if (this.data != null) {
			this.data.show.v = UIData.Show.profile;
		} else {
			Debug.LogError ("data null: " + this);
		}
	}

	#endregion

}