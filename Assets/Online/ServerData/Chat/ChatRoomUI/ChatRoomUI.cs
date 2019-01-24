﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class ChatRoomUI : UIBehavior<ChatRoomUI.UIData>
{

	#region UIData

	public class UIData : Data
	{
		
		public VP<ReferenceData<ChatRoom>> chatRoom;

		public VP<TopicUI> topicUI;

		public VP<ChatRoomAdapter.UIData> chatRoomAdapter;

		public VP<TypingUI.UIData> typingUI;

		#region edit

		public VP<ChatMessageMenuUI.UIData> chatMessageMenu;

		#endregion

		public VP<bool> canSendMessage;

		public VP<ChatRoomBtnLoadMoreUI.UIData> btnLoadMore;

		#region Constructor

		public enum Property
		{
			chatRoom,
			topicUI,
			chatRoomAdapter,
			typingUI,
			chatMessageMenu,
			canSendMessage,
			btnLoadMore
		}

		public UIData() : base()
		{
			this.chatRoom = new VP<ReferenceData<ChatRoom>>(this, (byte)Property.chatRoom, new ReferenceData<ChatRoom>(null));
			this.topicUI = new VP<TopicUI>(this, (byte)Property.topicUI, null);
			this.chatRoomAdapter = new VP<ChatRoomAdapter.UIData>(this, (byte)Property.chatRoomAdapter, new ChatRoomAdapter.UIData());
			this.typingUI = new VP<TypingUI.UIData>(this, (byte)Property.typingUI, new TypingUI.UIData());
			this.chatMessageMenu = new VP<ChatMessageMenuUI.UIData>(this, (byte)Property.chatMessageMenu, null);
			this.canSendMessage = new VP<bool>(this, (byte)Property.canSendMessage, true);
			this.btnLoadMore = new VP<ChatRoomBtnLoadMoreUI.UIData>(this, (byte)Property.btnLoadMore, null);
		}

		#endregion

		public void reset()
		{
			this.chatMessageMenu.v = null;
		}

		public bool processEvent(Event e)
		{
			Debug.LogError ("processEvent: " + e + "; " + this);
			bool isProcess = false;
			{
				// chatMessageMenu
				if (!isProcess) {
					ChatMessageMenuUI.UIData chatMessageMenu = this.chatMessageMenu.v;
					if (chatMessageMenu != null) {
						isProcess = chatMessageMenu.processEvent (e);
					} else {
						Debug.LogError ("chatMessageMenu null: " + this);
					}
				}
			}
			return isProcess;
		}

	}

	#endregion

	#region Refresh

	#region txt

	public Text tvSend;
	public static readonly TxtLanguage txtSend = new TxtLanguage();

	public Text edtMessagePlaceHolder;
	public static readonly TxtLanguage txtMessagePlaceHolder = new TxtLanguage ();

	static ChatRoomUI()
	{
		txtSend.add (Language.Type.vi, "Gửi");
		txtMessagePlaceHolder.add (Language.Type.vi, "Xin hãy gõ thông điệp của bạn");
	}

	#endregion

	public override void refresh ()
	{
		if (dirty) {
			dirty = false;
			if (this.data != null) {
				ChatRoom chatRoom = this.data.chatRoom.v.data;
				if (chatRoom != null) {
					// topicUI
					{
						Topic topic = chatRoom.topic.v;
						if (topic != null) {
							switch (topic.getType ()) {
							case Topic.Type.General:
								{
									GeneralTopic generalTopic = topic as GeneralTopic;
									// UIData
									GeneralTopicUI.UIData generalTopicUIData = this.data.topicUI.newOrOld<GeneralTopicUI.UIData>();
									{
										generalTopicUIData.generalTopic.v = new ReferenceData<GeneralTopic> (generalTopic);
									}
									this.data.topicUI.v = generalTopicUIData;
								}
								break;
							case Topic.Type.ShortQuestion:
								{
									ShortQuestionTopic shortQuestionTopic = topic as ShortQuestionTopic;
									// UIData
									ShortQuestionTopicUI.UIData shortQuestionTopicUIData = this.data.topicUI.newOrOld<ShortQuestionTopicUI.UIData>();
									{
										shortQuestionTopicUIData.shortQuestionTopic.v = new ReferenceData<ShortQuestionTopic> (shortQuestionTopic);
									}
									this.data.topicUI.v = shortQuestionTopicUIData;
								}
								break;
							case Topic.Type.Bug:
								{
									BugTopic bugTopic = topic as BugTopic;
									// UIData
									BugTopicUI.UIData bugTopicUIData = this.data.topicUI.newOrOld<BugTopicUI.UIData>();
									{
										bugTopicUIData.bugTopic.v = new ReferenceData<BugTopic> (bugTopic);
									}
									this.data.topicUI.v = bugTopicUIData;
								}
								break;
							case Topic.Type.Suggestion:
								{
									SuggestionTopic suggestionTopic = topic as SuggestionTopic;
									// UIData
									SuggestionTopicUI.UIData suggestionTopicUIData = this.data.topicUI.newOrOld<SuggestionTopicUI.UIData>();
									{
										suggestionTopicUIData.suggestionTopic.v = new ReferenceData<SuggestionTopic> (suggestionTopic);
									}
									this.data.topicUI.v = suggestionTopicUIData;
								}
								break;
							case Topic.Type.Off:
								{
									OffTopic offTopic = topic as OffTopic;
									// UIData
									OffTopicUI.UIData offTopicUIData = this.data.topicUI.newOrOld<OffTopicUI.UIData>();
									{
										offTopicUIData.offTopic.v = new ReferenceData<OffTopic> (offTopic);
									}
									this.data.topicUI.v = offTopicUIData;
								}
								break;
							case Topic.Type.User:
								{
									UserTopic userTopic = topic as UserTopic;
									// UIData
									UserTopicUI.UIData userTopicUIData = this.data.topicUI.newOrOld<UserTopicUI.UIData>();
									{
										userTopicUIData.userTopic.v = new ReferenceData<UserTopic> (userTopic);
									}
									this.data.topicUI.v = userTopicUIData;
								}
								break;
							case Topic.Type.Friend:
								{
									FriendTopic friendTopic = topic as FriendTopic;
									// UIData
									FriendTopicUI.UIData friendTopicUIData = this.data.topicUI.newOrOld<FriendTopicUI.UIData>();
									{
										friendTopicUIData.friendTopic.v = new ReferenceData<FriendTopic> (friendTopic);
									}
									this.data.topicUI.v = friendTopicUIData;
								}
								break;
							case Topic.Type.Guild:
								{
									GuildTopic guildTopic = topic as GuildTopic;
									// UIData
									GuildTopicUI.UIData guildTopicUIData = this.data.topicUI.newOrOld<GuildTopicUI.UIData>();
									{
										guildTopicUIData.guildTopic.v = new ReferenceData<GuildTopic> (guildTopic);
									}
									this.data.topicUI.v = guildTopicUIData;
								}
								break;
							case Topic.Type.Room:
								{
									RoomTopic roomTopic = topic as RoomTopic;
									// UIData
									RoomTopicUI.UIData roomTopicUIData = this.data.topicUI.newOrOld<RoomTopicUI.UIData>();
									{
										roomTopicUIData.roomTopic.v = new ReferenceData<RoomTopic> (roomTopic);
									}
									this.data.topicUI.v = roomTopicUIData;
								}
								break;
							case Topic.Type.Game:
								{
									GameTopic gameTopic = topic as GameTopic;
									// UIData
									GameTopicUI.UIData gameTopicUIData = this.data.topicUI.newOrOld<GameTopicUI.UIData>();
									{
										gameTopicUIData.gameTopic.v = new ReferenceData<GameTopic> (gameTopic);
									}
									this.data.topicUI.v = gameTopicUIData;
								}
								break;
							default:
								Debug.LogError ("unknown type: " + topic.getType () + "; " + this);
								break;
							}
						} else {
							Debug.LogError ("topic null: " + this);
						}
					}
					// adapter
					{
						ChatRoomAdapter.UIData chatRoomAdapterUIData = this.data.chatRoomAdapter.v;
						if (chatRoomAdapterUIData != null) {
							chatRoomAdapterUIData.chatRoom.v = new ReferenceData<ChatRoom> (chatRoom);
						} else {
							Debug.LogError ("chatRoomAdapterUIData null: " + this);
						}
					}
					// Typing
					{
						TypingUI.UIData typingUIData = this.data.typingUI.v;
						if (typingUIData != null) {
							typingUIData.typing.v = new ReferenceData<Typing> (chatRoom.typing.v);
						} else {
							Debug.LogError ("typingUIData null: " + this);
						}
					}
					// btnLoadMore
					{
						// find isClient
						bool isClient = false;
						{
							Server server = chatRoom.findDataInParent<Server> ();
							if (server != null) {
								isClient = server.type.v == Server.Type.Client;
							} else {
								Debug.LogError ("server null: " + this);
							}
						}
						// process
						if (isClient) {
							ChatRoomBtnLoadMoreUI.UIData btnLoadMoreUIData = this.data.btnLoadMore.newOrOld<ChatRoomBtnLoadMoreUI.UIData> ();
							{
								btnLoadMoreUIData.chatRoom.v = new ReferenceData<ChatRoom> (chatRoom);
							}
							this.data.btnLoadMore.v = btnLoadMoreUIData;
						} else {
							this.data.btnLoadMore.v = null;
						}
					}
				} else {
					Debug.LogError ("chatRoom null: " + this);
				}
				// edtMessage
				{
					if (edtMessage != null) {
						edtMessage.enabled = this.data.canSendMessage.v;
					} else {
						Debug.LogError ("edtMessage null: " + this);
					}
				}
				// txt
				{
					if (tvSend != null) {
						tvSend.text = txtSend.get ("Send");
					} else {
						Debug.LogError ("tvSend null: " + this);
					}
					if (edtMessagePlaceHolder != null) {
						edtMessagePlaceHolder.text = txtMessagePlaceHolder.get ("Please type your message");
					} else {
						Debug.LogError ("edtMessagePlaceHolder null: " + this);
					}
				}
			} else {
				// Debug.LogError ("data null: " + this);
			}
		}
	}

	public override bool isShouldDisableUpdate ()
	{
		return true;
	}

	#endregion

	#region implement callBacks

	public GeneralTopicUI generalTopicPrefab;
	public ShortQuestionTopicUI shortQuestionTopicPrefab;
	public BugTopicUI bugTopicPrefab;
	public SuggestionTopicUI suggestionTopicPrefab;
	public OffTopicUI offTopicPrefab;
	public UserTopicUI userTopicPrefab;
	public FriendTopicUI friendTopicPrefab;
	public GuildTopicUI guildTopicPrefab;
	public RoomTopicUI roomTopicPrefab;
	public GameTopicUI gameTopicPrefab;

	public Transform topicUIContainer;

	public ChatRoomAdapter chatRoomAdapterPrefab;
	public Transform chatRoomAdapterContainer;

	public TypingUI typingUI;

	public ChatMessageMenuUI chatMessageMenuPrefab;
	public Transform dialogContainer;

	public ChatRoomBtnLoadMoreUI btnLoadMorePrefab;
	public Transform btnLoadMoreContainer;

	public override void onAddCallBack<T> (T data)
	{
		if (data is UIData) {
			UIData uiData = data as UIData;
			// Setting
			Setting.get().addCallBack(this);
			// Child
			{
				uiData.chatRoom.allAddCallBack (this);
				uiData.topicUI.allAddCallBack (this);
				uiData.chatRoomAdapter.allAddCallBack (this);
				uiData.typingUI.allAddCallBack (this);
				uiData.chatMessageMenu.allAddCallBack (this);
				uiData.btnLoadMore.allAddCallBack (this);
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
			if (data is ChatRoom) {
				// reset
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
			if (data is TopicUI) {
				TopicUI topicUI = data as TopicUI;
				// UI
				{
					switch (topicUI.getType ()) {
					case Topic.Type.General:
						{
							GeneralTopicUI.UIData generalTopicUIData = topicUI as GeneralTopicUI.UIData;
							UIUtils.Instantiate (generalTopicUIData, generalTopicPrefab, topicUIContainer);
						}
						break;
					case Topic.Type.ShortQuestion:
						{
							ShortQuestionTopicUI.UIData shortQuestionTopicUIData = topicUI as ShortQuestionTopicUI.UIData;
							UIUtils.Instantiate (shortQuestionTopicUIData, shortQuestionTopicPrefab, topicUIContainer);
						}
						break;
					case Topic.Type.Bug:
						{
							BugTopicUI.UIData bugTopicUIData = topicUI as BugTopicUI.UIData;
							UIUtils.Instantiate (bugTopicUIData, bugTopicPrefab, topicUIContainer);
						}
						break;
					case Topic.Type.Suggestion:
						{
							SuggestionTopicUI.UIData suggestionTopicUIData = topicUI as SuggestionTopicUI.UIData;
							UIUtils.Instantiate (suggestionTopicUIData, suggestionTopicPrefab, topicUIContainer);
						}
						break;
					case Topic.Type.Off:
						{
							OffTopicUI.UIData offTopicUIData = topicUI as OffTopicUI.UIData;
							UIUtils.Instantiate (offTopicUIData, offTopicPrefab, topicUIContainer);
						}
						break;
					case Topic.Type.User:
						{
							UserTopicUI.UIData userTopicUIData = topicUI as UserTopicUI.UIData;
							UIUtils.Instantiate (userTopicUIData, userTopicPrefab, topicUIContainer);
						}
						break;
					case Topic.Type.Friend:
						{
							FriendTopicUI.UIData friendTopicUIData = topicUI as FriendTopicUI.UIData;
							UIUtils.Instantiate (friendTopicUIData, friendTopicPrefab, topicUIContainer);
						}
						break;
					case Topic.Type.Guild:
						{
							GuildTopicUI.UIData guildTopicUIData = topicUI as GuildTopicUI.UIData;
							UIUtils.Instantiate (guildTopicUIData, guildTopicPrefab, topicUIContainer);
						}
						break;
					case Topic.Type.Room:
						{
							RoomTopicUI.UIData roomTopicUIData = topicUI as RoomTopicUI.UIData;
							UIUtils.Instantiate (roomTopicUIData, roomTopicPrefab, topicUIContainer);
						}
						break;
					case Topic.Type.Game:
						{
							GameTopicUI.UIData gameTopicUIData = topicUI as GameTopicUI.UIData;
							UIUtils.Instantiate (gameTopicUIData, gameTopicPrefab, topicUIContainer);
						}
						break;
					default:
						Debug.LogError ("unknown type: " + topicUI.getType () + "; " + this);
						break;
					}
				}
				dirty = true;
				return;
			}
			if (data is ChatRoomAdapter.UIData) {
				ChatRoomAdapter.UIData chatRoomAdapterUIData = data as ChatRoomAdapter.UIData;
				// UI
				{
					UIUtils.Instantiate (chatRoomAdapterUIData, chatRoomAdapterPrefab, chatRoomAdapterContainer);
				}
				dirty = true;
				return;
			}
			if (data is TypingUI.UIData) {
				TypingUI.UIData subUIData = data as TypingUI.UIData;
				// UI
				{
					if (typingUI != null) {
						typingUI.setData (subUIData);
					} else {
						Debug.LogError ("typingUI null");
					}
				}
				dirty = true;
				return;
			}
			if (data is ChatMessageMenuUI.UIData) {
				ChatMessageMenuUI.UIData chatMessageMenuUIData = data as ChatMessageMenuUI.UIData;
				// UI
				{
					UIUtils.Instantiate (chatMessageMenuUIData, chatMessageMenuPrefab, dialogContainer);
				}
				dirty = true;
				return;
			}
			if (data is ChatRoomBtnLoadMoreUI.UIData) {
				ChatRoomBtnLoadMoreUI.UIData btnLoadMoreUIData = data as ChatRoomBtnLoadMoreUI.UIData;
				// UI
				{
					UIUtils.Instantiate (btnLoadMoreUIData, btnLoadMorePrefab, btnLoadMoreContainer);
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
				uiData.chatRoom.allRemoveCallBack (this);
				uiData.topicUI.allRemoveCallBack (this);
				uiData.chatRoomAdapter.allRemoveCallBack (this);
				uiData.typingUI.allRemoveCallBack (this);
				uiData.chatMessageMenu.allRemoveCallBack (this);
				uiData.btnLoadMore.allRemoveCallBack (this);
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
			if (data is ChatRoom) {
				return;
			}
			if (data is TopicUI) {
				TopicUI topicUI = data as TopicUI;
				// UI
				{
					switch (topicUI.getType ()) {
					case Topic.Type.General:
						{
							GeneralTopicUI.UIData generalTopicUIData = topicUI as GeneralTopicUI.UIData;
							generalTopicUIData.removeCallBackAndDestroy (typeof(GeneralTopicUI));
						}
						break;
					case Topic.Type.ShortQuestion:
						{
							ShortQuestionTopicUI.UIData shortQuestionTopicUIData = topicUI as ShortQuestionTopicUI.UIData;
							shortQuestionTopicUIData.removeCallBackAndDestroy (typeof(ShortQuestionTopicUI));
						}
						break;
					case Topic.Type.Bug:
						{
							BugTopicUI.UIData bugTopicUIData = topicUI as BugTopicUI.UIData;
							bugTopicUIData.removeCallBackAndDestroy (typeof(BugTopicUI));
						}
						break;
					case Topic.Type.Suggestion:
						{
							SuggestionTopicUI.UIData suggestionTopicUIData = topicUI as SuggestionTopicUI.UIData;
							suggestionTopicUIData.removeCallBackAndDestroy (typeof(SuggestionTopicUI));
						}
						break;
					case Topic.Type.Off:
						{
							OffTopicUI.UIData offTopicUIData = topicUI as OffTopicUI.UIData;
							offTopicUIData.removeCallBackAndDestroy (typeof(OffTopicUI));
						}
						break;
					case Topic.Type.User:
						{
							UserTopicUI.UIData userTopicUIData = topicUI as UserTopicUI.UIData;
							userTopicUIData.removeCallBackAndDestroy (typeof(UserTopicUI));
						}
						break;
					case Topic.Type.Friend:
						{
							FriendTopicUI.UIData friendTopicUIData = topicUI as FriendTopicUI.UIData;
							friendTopicUIData.removeCallBackAndDestroy (typeof(FriendTopicUI));
						}
						break;
					case Topic.Type.Guild:
						{
							GuildTopicUI.UIData guildTopicUIData = topicUI as GuildTopicUI.UIData;
							guildTopicUIData.removeCallBackAndDestroy (typeof(GuildTopicUI));
						}
						break;
					case Topic.Type.Room:
						{
							RoomTopicUI.UIData roomTopicUIData = topicUI as RoomTopicUI.UIData;
							roomTopicUIData.removeCallBackAndDestroy (typeof(RoomTopicUI));
						}
						break;
					case Topic.Type.Game:
						{
							GameTopicUI.UIData gameTopicUIData = topicUI as GameTopicUI.UIData;
							gameTopicUIData.removeCallBackAndDestroy (typeof(GameTopicUI));
						}
						break;
					default:
						Debug.LogError ("unknown type: " + topicUI.getType () + "; " + this);
						break;
					}
				}
				return;
			}
			if (data is ChatRoomAdapter.UIData) {
				ChatRoomAdapter.UIData chatRoomAdapterUIData = data as ChatRoomAdapter.UIData;
				{
					chatRoomAdapterUIData.removeCallBackAndDestroy (typeof(ChatRoomAdapter));
				}
				return;
			}
			if (data is TypingUI.UIData) {
				TypingUI.UIData subUIData = data as TypingUI.UIData;
				{
					if (typingUI != null) {
						if (typingUI.data == subUIData) {
							typingUI.setData (null);
						} else {
							Debug.LogError ("why different: " + this);
						}
					} else {
						Debug.LogError ("typingUI null: " + this);
					}
				}
				return;
			}
			if (data is ChatMessageMenuUI.UIData) {
				ChatMessageMenuUI.UIData chatMessageMenuUIData = data as ChatMessageMenuUI.UIData;
				// UI
				{
					chatMessageMenuUIData.removeCallBackAndDestroy (typeof(ChatMessageMenuUI));
				}
				return;
			}
			if (data is ChatRoomBtnLoadMoreUI.UIData) {
				ChatRoomBtnLoadMoreUI.UIData btnLoadMoreUIData = data as ChatRoomBtnLoadMoreUI.UIData;
				// UI
				{
					btnLoadMoreUIData.removeCallBackAndDestroy (typeof(ChatRoomBtnLoadMoreUI));
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
			case UIData.Property.topicUI:
				{
					ValueChangeUtils.replaceCallBack (this, syncs);
					dirty = true;
				}
				break;
			case UIData.Property.chatRoom:
				{
					ValueChangeUtils.replaceCallBack (this, syncs);
					dirty = true;
				}
				break;
			case UIData.Property.chatRoomAdapter:
				{
					ValueChangeUtils.replaceCallBack (this, syncs);
					dirty = true;
				}
				break;
			case UIData.Property.typingUI:
				{
					ValueChangeUtils.replaceCallBack (this, syncs);
					dirty = true;
				}
				break;
			case UIData.Property.chatMessageMenu:
				{
					ValueChangeUtils.replaceCallBack (this, syncs);
					dirty = true;
				}
				break;
			case UIData.Property.canSendMessage:
				dirty = true;
				break;
			case UIData.Property.btnLoadMore:
				{
					ValueChangeUtils.replaceCallBack (this, syncs);
					dirty = true;
				}
				break;
			default:
				Debug.LogError ("unknown wrapProperty: " + wrapProperty + "; " + this);
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
			if (wrapProperty.p is ChatRoom) {
				switch ((ChatRoom.Property)wrapProperty.n) {
				case ChatRoom.Property.topic:
					dirty = true;
					break;
				case ChatRoom.Property.players:
					break;
				case ChatRoom.Property.messages:
					break;
				case ChatRoom.Property.typing:
					dirty = true;
					break;
				default:
					Debug.LogError ("unknown wrapProperty: " + wrapProperty + "; " + this);
					break;
				}
				return;
			}
			if (wrapProperty.p is TopicUI) {
				return;
			}
			if (wrapProperty.p is ChatRoomAdapter.UIData) {
				return;
			}
			if (wrapProperty.p is TypingUI.UIData) {
				return;
			}
			if (wrapProperty.p is ChatMessageMenuUI.UIData) {
				return;
			}
			if (wrapProperty.p is ChatRoomBtnLoadMoreUI.UIData) {
				return;
			}
		}
		Debug.LogError ("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
	}

	#endregion

	#region Send Chat

	public InputField edtMessage;

	public void onClickBtnSendMessage()
	{
		if (edtMessage != null) {
			// Get message
			string message = edtMessage.text;
			if (!string.IsNullOrEmpty (message)) {
				this.sendMessage (message);
			} else {
				Debug.LogError ("Cannot send null text");
			}
			// reset input
			edtMessage.text = "";
		} else {
			Debug.LogError ("edtMessage null");
		}
	}

	/// <summary>
	/// Sends the message.
	/// </summary>
	/// <returns><c>true</c>, if message was sent, <c>false</c> otherwise.</returns>
	/// <param name="newMessage">New message.</param>
	public bool sendMessage(string message)
	{
		if (this.data != null) {
			ChatRoom chatRoom = this.data.chatRoom.v.data;
			if (chatRoom != null) {
				// Get message
				if (!string.IsNullOrEmpty (message)) {
					uint yourUserId = Server.getProfileUserId (chatRoom);
					// Send
					if (chatRoom.topic.v.isCanSendNormalMessage (yourUserId)) {
						chatRoom.requestSendNormalMessage (yourUserId, message);
					} else {
						Debug.LogError ("Cannot send normal message");
					}
				} else {
					Debug.LogError ("Cannot send null text");
				}
			} else {
				Debug.LogError ("chatRoom null");
			}
		} else {
			Debug.LogError ("chatRoom null");
		}
		return true;
	}

	#endregion

}