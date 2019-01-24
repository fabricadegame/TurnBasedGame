﻿using UnityEngine;
using UnityEngine.UI;
using frame8.Logic.Misc.Visual.UI.ScrollRectItemsAdapter;
using System;
using System.Collections;
using System.Collections.Generic;

public class ChatMessageHolder : SriaHolderBehavior<ChatMessageHolder.UIData>
{

	#region UIData

	public class UIData : BaseItemViewsHolder
	{

		public VP<ReferenceData<ChatMessage>> chatMessage;

		#region Sub

		public abstract class Sub : Data
		{
			public abstract ChatMessage.Content.Type getType ();
		}

		public VP<Sub> sub;

		#endregion

		#region Constructor

		public enum Property
		{
			chatMessage,
			sub
		}

		public UIData() : base()
		{
			this.chatMessage = new VP<ReferenceData<ChatMessage>>(this, (byte)Property.chatMessage, new ReferenceData<ChatMessage>(null));
			this.sub = new VP<Sub>(this, (byte)Property.sub, null);
		}

		#endregion

		public void updateView(ChatRoomAdapter.UIData myParams)
		{
			// Find
			ChatMessage chatMessage = null;
			{
				if (ItemIndex >= 0 && ItemIndex < myParams.chatMessages.Count) {
					chatMessage = myParams.chatMessages [ItemIndex];
				} else {
					Debug.LogError ("ItemIdex error: "+ItemIndex+"; "+myParams.chatMessages.Count+"; " + this);
				}
			}
			// Update
			this.chatMessage.v = new ReferenceData<ChatMessage> (chatMessage);
		}
	}

	#endregion

	#region Refresh

	public override void refresh ()
	{
		base.refresh ();
		if (dirty) {
			dirty = false;
			if (this.data != null) {
				ChatMessage chatMessage = this.data.chatMessage.v.data;
				if (chatMessage != null) {
					ChatMessage.Content content = chatMessage.content.v;
					if (content != null) {
						switch (content.getType ()) {
						case ChatMessage.Content.Type.Normal:
							{
								ChatNormalContent chatNormalContent = content as ChatNormalContent;
								// UIData
								ChatNormalContentUI.UIData chatNormalContentUIData = this.data.sub.newOrOld<ChatNormalContentUI.UIData> ();
								{
									chatNormalContentUIData.chatNormalContent.v = new ReferenceData<ChatNormalContent> (chatNormalContent);
								}
								this.data.sub.v = chatNormalContentUIData;
							}
							break;
						case ChatMessage.Content.Type.UserAction:
							{
								UserActionMessage userActionMessage = content as UserActionMessage;
								// UIData
								UserActionMessageUI.UIData userActionMessageUIData = this.data.sub.newOrOld<UserActionMessageUI.UIData> ();
								{
									userActionMessageUIData.userActionMessage.v = new ReferenceData<UserActionMessage>(userActionMessage);
								}
								this.data.sub.v = userActionMessageUIData;
							}
							break;
						case ChatMessage.Content.Type.RoomUserState:
							{
								ChatRoomUserStateContent chatRoomUserStateContent = content as ChatRoomUserStateContent;
								// UIData
								ChatRoomUserStateUI.UIData chatRoomUserStateUIData = this.data.sub.newOrOld<ChatRoomUserStateUI.UIData> ();
								{
									chatRoomUserStateUIData.chatRoomUserStateContent.v = new ReferenceData<ChatRoomUserStateContent>(chatRoomUserStateContent);
								}
								this.data.sub.v = chatRoomUserStateUIData;
							}
							break;
						case ChatMessage.Content.Type.FriendStateChange:
							{
								FriendStateChangeContent friendStateChangeContent = content as FriendStateChangeContent;
								// UIData
								FriendStateChangeContentUI.UIData friendStateChangeContentUIData = this.data.sub.newOrOld<FriendStateChangeContentUI.UIData> ();
								{
									friendStateChangeContentUIData.friendStateChangeContent.v = new ReferenceData<FriendStateChangeContent> (friendStateChangeContent);
								}
								this.data.sub.v = friendStateChangeContentUIData;
							}
							break;
						case ChatMessage.Content.Type.GameMove:
							{
								ChatGameMoveContent chatGameMoveContent = content as ChatGameMoveContent;
								// UIData
								ChatGameMoveContentUI.UIData chatGameMoveContentUIData = this.data.sub.newOrOld<ChatGameMoveContentUI.UIData> ();
								{
									chatGameMoveContentUIData.chatGameMoveContent.v = new ReferenceData<ChatGameMoveContent> (chatGameMoveContent);
								}
								this.data.sub.v = chatGameMoveContentUIData;
							}
							break;
						default:
							Debug.LogError ("unknown type: " + chatMessage.content.v.getType () + "; " + this);
							break;
						}
					} else {
						Debug.LogError ("content null: " + this);
					}
				} else {
					// Debug.LogError ("chatMessage null: " + this);
				}
			} else {
				// Debug.LogError ("data null: " + this);
			}
		}
	}

	#endregion

	#region implement callBacks

	public ChatNormalContentUI chatNormalContentPrefab;
	public UserActionMessageUI userActionMessagePrefab;
	public ChatRoomUserStateUI chatRoomUserStatePrefab;
	public FriendStateChangeContentUI friendStateChangePrefab;
	public ChatGameMoveContentUI chatGameMoveContentPrefab;

	public override void onAddCallBack<T> (T data)
	{
		if (data is UIData) {
			UIData uiData = data as UIData;
			// Child
			{
				uiData.chatMessage.allAddCallBack (this);
				uiData.sub.allAddCallBack (this);
			}
			dirty = true;
			return;
		}
		// Child
		{
			if (data is ChatMessage) {
				dirty = true;
				return;
			}
			if (data is UIData.Sub) {
				UIData.Sub sub = data as UIData.Sub;
				// UI
				{
					switch (sub.getType ()) {
					case ChatMessage.Content.Type.Normal:
						{
							ChatNormalContentUI.UIData chatNormalContentUIData = sub as ChatNormalContentUI.UIData;
							UIUtils.Instantiate (chatNormalContentUIData, chatNormalContentPrefab, this.transform);
						}
						break;
					case ChatMessage.Content.Type.UserAction:
						{
							UserActionMessageUI.UIData userActionMessageUIData = sub as UserActionMessageUI.UIData;
							UIUtils.Instantiate (userActionMessageUIData, userActionMessagePrefab, this.transform);
						}
						break;
					case ChatMessage.Content.Type.RoomUserState:
						{
							ChatRoomUserStateUI.UIData chatRoomUserStateUIData = sub as ChatRoomUserStateUI.UIData;
							UIUtils.Instantiate (chatRoomUserStateUIData, chatRoomUserStatePrefab, this.transform);
						}
						break;
					case ChatMessage.Content.Type.FriendStateChange:
						{
							FriendStateChangeContentUI.UIData friendStateChangeContentUIData = sub as FriendStateChangeContentUI.UIData;
							UIUtils.Instantiate (friendStateChangeContentUIData, friendStateChangePrefab, this.transform);
						}
						break;
					case ChatMessage.Content.Type.GameMove:
						{
							ChatGameMoveContentUI.UIData chatGameMoveContentUIData = sub as ChatGameMoveContentUI.UIData;
							UIUtils.Instantiate (chatGameMoveContentUIData, chatGameMoveContentPrefab, this.transform);
						}
						break;
					default:
						Debug.LogError ("unknown type: " + sub.getType () + "; " + this);
						break;
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
				uiData.chatMessage.allRemoveCallBack (this);
				uiData.sub.allRemoveCallBack (this);
			}
			this.setDataNull (uiData);
			return;
		}
		// Child
		{
			if (data is ChatMessage) {
				return;
			}
			if (data is UIData.Sub) {
				UIData.Sub sub = data as UIData.Sub;
				// UI
				{
					switch (sub.getType ()) {
					case ChatMessage.Content.Type.Normal:
						{
							ChatNormalContentUI.UIData chatNormalContentUIData = sub as ChatNormalContentUI.UIData;
							chatNormalContentUIData.removeCallBackAndDestroy (typeof(ChatNormalContentUI));
						}
						break;
					case ChatMessage.Content.Type.UserAction:
						{
							UserActionMessageUI.UIData userActionMessageUIData = sub as UserActionMessageUI.UIData;
							userActionMessageUIData.removeCallBackAndDestroy (typeof(UserActionMessageUI));
						}
						break;
					case ChatMessage.Content.Type.RoomUserState:
						{
							ChatRoomUserStateUI.UIData chatRoomUserStateUIData = sub as ChatRoomUserStateUI.UIData;
							chatRoomUserStateUIData.removeCallBackAndDestroy (typeof(ChatRoomUserStateUI));
						}
						break;
					case ChatMessage.Content.Type.FriendStateChange:
						{
							FriendStateChangeContentUI.UIData friendStateChangeContentUIData = sub as FriendStateChangeContentUI.UIData;
							friendStateChangeContentUIData.removeCallBackAndDestroy (typeof(FriendStateChangeContentUI));
						}
						break;
					case ChatMessage.Content.Type.GameMove:
						{
							ChatGameMoveContentUI.UIData chatGameMoveContentUIData = sub as ChatGameMoveContentUI.UIData;
							chatGameMoveContentUIData.removeCallBackAndDestroy (typeof(ChatGameMoveContentUI));
						}
						break;
					default:
						Debug.LogError ("unknown type: " + sub.getType () + "; " + this);
						break;
					}
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
			case UIData.Property.chatMessage:
				{
					ValueChangeUtils.replaceCallBack (this, syncs);
					dirty = true;
				}
				break;
			case UIData.Property.sub:
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
			if (wrapProperty.p is ChatMessage) {
				switch ((ChatMessage.Property)wrapProperty.n) {
				case ChatMessage.Property.state:
					break;
				case ChatMessage.Property.time:
					break;
				case ChatMessage.Property.content:
					dirty = true;
					break;
				default:
					Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
					break;
				}
				return;
			}
			if (wrapProperty.p is UIData.Sub) {
				return;
			}
		}
		Debug.LogError ("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
	}

	#endregion

}