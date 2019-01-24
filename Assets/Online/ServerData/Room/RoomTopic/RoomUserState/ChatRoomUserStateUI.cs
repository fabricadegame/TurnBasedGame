﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ChatRoomUserStateUI : UIBehavior<ChatRoomUserStateUI.UIData>
{

	#region UIData

	public class UIData : ChatMessageHolder.UIData.Sub
	{
		public VP<ReferenceData<ChatRoomUserStateContent>> chatRoomUserStateContent;

		public VP<AccountAvatarUI.UIData> avatar;

		public VP<RequestChangeStringUI.UIData> content;

		public VP<RequestChangeStringUI.UIData> time;

		#region Constructor

		public enum Property
		{
			chatRoomUserStateContent,
			avatar,
			content,
			time
		}

		public UIData() : base()
		{
			this.chatRoomUserStateContent = new VP<ReferenceData<ChatRoomUserStateContent>>(this, (byte)Property.chatRoomUserStateContent, new ReferenceData<ChatRoomUserStateContent>(null));
			this.avatar = new VP<AccountAvatarUI.UIData>(this, (byte)Property.avatar, new AccountAvatarUI.UIData());
			// content
			{
				this.content = new VP<RequestChangeStringUI.UIData>(this, (byte)Property.content, new RequestChangeStringUI.UIData());
				this.content.v.updateData.v.canRequestChange.v = false;
			}
			// time
			{
				this.time = new VP<RequestChangeStringUI.UIData>(this, (byte)Property.time, new RequestChangeStringUI.UIData());
				this.time.v.updateData.v.canRequestChange.v = false;
			}
		}

		#endregion

		public override ChatMessage.Content.Type getType ()
		{
			return ChatMessage.Content.Type.RoomUserState;
		}

	}

	#endregion

	#region Refresh

	private Human human = null;

	public override void refresh ()
	{
		if (dirty) {
			dirty = false;
			if (this.data != null) {
				ChatRoomUserStateContent chatRoomUserStateContent = this.data.chatRoomUserStateContent.v.data;
				if (chatRoomUserStateContent != null) {
					ChatMessage chatMessage = chatRoomUserStateContent.findDataInParent<ChatMessage>();
					if (chatMessage != null) {
						// Find human
						{
							Human human = ChatRoom.findHuman (chatRoomUserStateContent, chatRoomUserStateContent.userId.v);
							if (this.human != human) {
								// remove old
								if (this.human != null) {
									this.human.removeCallBack (this);
								}
								// set new
								this.human = human;
								if (this.human != null) {
									this.human.addCallBack (this);
								}
							}
						}
						// Avatar
						{
							AccountAvatarUI.UIData accountAvatarUIData = this.data.avatar.v;
							if (accountAvatarUIData != null) {
								Account account = null;
								{
									if (this.human != null) {
										account = this.human.account.v;
									}
								}
								accountAvatarUIData.account.v = new ReferenceData<Account> (account);
							} else {
								Debug.LogError ("accountAvatarUIData null: " + this);
							}
						}
						// time
						{
							RequestChangeStringUI.UIData time = this.data.time.v;
							if (time != null) {
								RequestChangeStringUpdate.UpdateData updateData = time.updateData.v;
								if (updateData != null) {
									updateData.origin.v = chatMessage.TimestampAsDateTime.ToString ("HH:mm");
								} else {
									Debug.LogError ("updateData null: " + this);
								}
							} else {
								Debug.LogError ("time null: " + this);
							}
						}
						// content
						{
							RequestChangeStringUI.UIData content = this.data.content.v;
							if (content != null) {
								RequestChangeStringUpdate.UpdateData updateData = content.updateData.v;
								if (updateData != null) {
									// Find user name
									string userName = "";
									{
										if (this.human != null) {
											userName = this.human.getPlayerName ();
										} else {
											Debug.LogError ("human null: " + this);
										}
									}
									// state
									switch (chatRoomUserStateContent.action.v) {
									case ChatRoomUserStateContent.Action.Create:
										updateData.origin.v = "<color=grey>" + userName + "</color> create room";
										break;
									case ChatRoomUserStateContent.Action.Join:
										updateData.origin.v = "<color=grey>" + userName + "</color> join room";
										break;
									case ChatRoomUserStateContent.Action.Left:
										updateData.origin.v = "<color=grey>" + userName + "</color> left room";
										break;
									case ChatRoomUserStateContent.Action.Disconnect:
										updateData.origin.v = "<color=grey>" + userName + "</color> disconnect";
										break;
									case ChatRoomUserStateContent.Action.Kick:
										updateData.origin.v = "<color=grey>" + userName + "</color> is kicked";
										break;
									case ChatRoomUserStateContent.Action.UnKick:
										updateData.origin.v = "<color=grey>" + userName + "</color> is unkicked";
										break;
									case ChatRoomUserStateContent.Action.Ban:
										updateData.origin.v = "<color=grey>" + userName + "</color> is banned";
										break;
									default:
										Debug.LogError ("unknown action: " + chatRoomUserStateContent.action.v + "; " + this);
										break;
									}
								} else {
									Debug.LogError ("updateData null: " + this);
								}
							} else {
								Debug.LogError ("content null: " + this);
							}
						}
					} else {
						Debug.LogError ("chatMessage null: " + this);
					}
				} else {
					Debug.LogError ("chatRoomUserStateContent null: " + this);
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

	public override void OnDestroy ()
	{
		base.OnDestroy ();
		if (this.human != null) {
			this.human.removeCallBack (this);
			this.human = null;
		} else {
			// Debug.LogError ("human null: " + this);
		}
	}

	#endregion

	#region implement callBacks

	private ChatMessage chatMessage = null;

	public AccountAvatarUI avatarPrefab;
	public RequestChangeStringUI requestStringPrefab;

	public Transform avatarContainer;
	public Transform contentContainer;
	public Transform timeContainer;

	public override void onAddCallBack<T> (T data)
	{
		if (data is UIData) {
			UIData uiData = data as UIData;
			// Child
			{
				uiData.chatRoomUserStateContent.allAddCallBack (this);
				uiData.avatar.allAddCallBack (this);
				uiData.content.allAddCallBack (this);
				uiData.time.allAddCallBack (this);
			}
			dirty = true;
			return;
		}
		// Child
		{
			// ChatRoomUserStateContent
			{
				if (data is ChatRoomUserStateContent) {
					ChatRoomUserStateContent chatRoomUserStateContent = data as ChatRoomUserStateContent;
					// Parent
					{
						DataUtils.addParentCallBack (chatRoomUserStateContent, this, ref this.chatMessage);
					}
					dirty = true;
					return;
				}
				// Parent
				{
					if (data is ChatMessage) {
						dirty = true;
						return;
					}
					// Human
					{
						if (data is Human) {
							Human human = data as Human;
							// Child
							{
								human.account.allAddCallBack (this);
							}
							dirty = true;
							return;
						}
						// child
						if (data is Account) {
							dirty = true;
							return;
						}
					}
				}
			}
			if (data is AccountAvatarUI.UIData) {
				AccountAvatarUI.UIData accountAvatarUIData = data as AccountAvatarUI.UIData;
				// UI
				{
					UIUtils.Instantiate (accountAvatarUIData, avatarPrefab, avatarContainer);
				}
				dirty = true;
				return;
			}
			// content, time
			if (data is RequestChangeStringUI.UIData) {
				RequestChangeStringUI.UIData requestChange = data as RequestChangeStringUI.UIData;
				// UI
				{
					WrapProperty wrapProperty = requestChange.p;
					if (wrapProperty != null) {
						switch ((UIData.Property)wrapProperty.n) {
						case UIData.Property.content:
							UIUtils.Instantiate (requestChange, requestStringPrefab, contentContainer);
							break;
						case UIData.Property.time:
							UIUtils.Instantiate (requestChange, requestStringPrefab, timeContainer);
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
		}
		Debug.LogError ("Don't process: " + data + "; " + this);
	}

	public override void onRemoveCallBack<T> (T data, bool isHide)
	{
		if (data is UIData) {
			UIData uiData = data as UIData;
			// Child
			{
				uiData.chatRoomUserStateContent.allRemoveCallBack (this);
				uiData.avatar.allRemoveCallBack (this);
				uiData.content.allRemoveCallBack (this);
				uiData.time.allRemoveCallBack (this);
			}
			this.setDataNull (uiData);
			return;
		}
		// Child
		{
			// ChatRoomUserStateContent
			{
				if (data is ChatRoomUserStateContent) {
					ChatRoomUserStateContent chatRoomUserStateContent = data as ChatRoomUserStateContent;
					// Parent
					{
						DataUtils.removeParentCallBack (chatRoomUserStateContent, this, ref this.chatMessage);
					}
					return;
				}
				// Parent
				{
					if (data is ChatMessage) {
						return;
					}
					// Human
					{
						if (data is Human) {
							Human human = data as Human;
							// Child
							{
								human.account.allRemoveCallBack (this);
							}
							return;
						}
						// child
						if (data is Account) {
							return;
						}
					}
				}
			}
			if (data is AccountAvatarUI.UIData) {
				AccountAvatarUI.UIData accountAvatarUIData = data as AccountAvatarUI.UIData;
				// UI
				{
					accountAvatarUIData.removeCallBackAndDestroy (typeof(AccountAvatarUI));
				}
				return;
			}
			// content, time
			if (data is RequestChangeStringUI.UIData) {
				RequestChangeStringUI.UIData requestChange = data as RequestChangeStringUI.UIData;
				// UI
				{
					requestChange.removeCallBackAndDestroy (typeof(RequestChangeStringUI));
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
			case UIData.Property.chatRoomUserStateContent:
				{
					ValueChangeUtils.replaceCallBack (this, syncs);
					dirty = true;
				}
				break;
			case UIData.Property.avatar:
				{
					ValueChangeUtils.replaceCallBack (this, syncs);
					dirty = true;
				}
				break;
			case UIData.Property.content:
				{
					ValueChangeUtils.replaceCallBack (this, syncs);
					dirty = true;
				}
				break;
			case UIData.Property.time:
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
		// Child
		{
			// ChatRoomUserStateContent
			{
				if (wrapProperty.p is ChatRoomUserStateContent) {
					switch ((ChatRoomUserStateContent.Property)wrapProperty.n) {
					case ChatRoomUserStateContent.Property.userId:
						dirty = true;
						break;
					case ChatRoomUserStateContent.Property.action:
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
					if (wrapProperty.p is ChatMessage) {
						switch ((ChatMessage.Property)wrapProperty.n) {
						case ChatMessage.Property.state:
							dirty = true;
							break;
						case ChatMessage.Property.time:
							dirty = true;
							break;
						case ChatMessage.Property.content:
							break;
						default:
							Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
							break;
						}
						return;
					}
					// Human
					{
						if (wrapProperty.p is Human) {
							switch ((Human.Property)wrapProperty.n) {
							case Human.Property.playerId:
								dirty = true;
								break;
							case Human.Property.account:
								{
									ValueChangeUtils.replaceCallBack (this, syncs);
									dirty = true;
								}
								break;
							case Human.Property.state:
								break;
							case Human.Property.email:
								break;
							case Human.Property.phoneNumber:
								break;
							case Human.Property.status:
								break;
							case Human.Property.birthday:
								break;
							case Human.Property.sex:
								break;
							case Human.Property.connection:
								break;
							case Human.Property.ban:
								break;
							default:
								Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
								break;
							}
							return;
						}
						// child
						if (wrapProperty.p is Account) {
							Account.OnUpdateSyncAccount (wrapProperty, this);
							return;
						}
					}
				}
			}
			if (wrapProperty.p is AccountAvatarUI.UIData) {
				return;
			}
			// content, time
			if (wrapProperty.p is RequestChangeStringUI.UIData) {
				return;
			}
		}
		Debug.LogError ("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
	}

	#endregion

}