﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using AdvancedCoroutines;
using Foundation.Tasks;

public class ChatMessageDeleteUI : UIBehavior<ChatMessageDeleteUI.UIData>
{

	#region UIData

	public class UIData : Data
	{

		public VP<ReferenceData<ChatMessage>> chatMessage;

		#region state

		public enum State
		{
			None,
			Request,
			Wait
		}

		public VP<State> state;

		#endregion

		#region Constructor

		public enum Property
		{
			chatMessage,
			state
		}

		public UIData() : base()
		{
			this.chatMessage = new VP<ReferenceData<ChatMessage>>(this, (byte)Property.chatMessage, new ReferenceData<ChatMessage>(null));
			this.state = new VP<State>(this, (byte)Property.state, State.None);
		}

		#endregion

		public void reset()
		{
			this.state.v = State.None;
		}

	}

	#endregion

	#region Refresh

	#region txt

	public static readonly TxtLanguage txtDelete = new TxtLanguage();
	public static readonly TxtLanguage txtCancelDelete = new TxtLanguage ();
	public static readonly TxtLanguage txtDeleting = new TxtLanguage ();

	public static readonly TxtLanguage txtUnDelete = new TxtLanguage();
	public static readonly TxtLanguage txtCancelUnDelete = new TxtLanguage ();
	public static readonly TxtLanguage txtUnDeleting = new TxtLanguage();

	static ChatMessageDeleteUI()
	{
		txtDelete.add (Language.Type.vi, "Xoá");
		txtCancelDelete.add (Language.Type.vi, "Huỷ xoá");
		txtDeleting.add (Language.Type.vi, "Đang xoá");

		txtUnDelete.add (Language.Type.vi, "Không xoá");
		txtCancelUnDelete.add (Language.Type.vi, "Dừng không xoá");
		txtUnDeleting.add (Language.Type.vi, "Đang không xoá");
	}

	#endregion

	private bool needReset = false;

	public Button btnDelete;
	public Text tvDelete;

	public override void refresh ()
	{
		if (dirty) {
			dirty = false;
			if (this.data != null) {
				ChatMessage chatMessage = this.data.chatMessage.v.data;
				if (chatMessage != null) {
					// reset
					{
						if (needReset) {
							this.data.state.v = UIData.State.None;
							needReset = false;
						}
					}
					// Task
					{
						switch (this.data.state.v) {
						case UIData.State.None:
							{
								destroyRoutine (wait);
							}
							break;
						case UIData.State.Request:
							{
								destroyRoutine (wait);
								// request
								{
									if (Server.IsServerOnline (chatMessage)) {
										switch (chatMessage.state.v) {
										case ChatMessage.State.Normal:
											chatMessage.requestChangeState (Server.getProfileUserId (chatMessage), ChatMessage.State.Delete);
											break;
										case ChatMessage.State.Delete:
											chatMessage.requestChangeState (Server.getProfileUserId (chatMessage), ChatMessage.State.Normal);
											break;
										case ChatMessage.State.TrueDelete:
											break;
										default:
											Debug.LogError ("unknown state: " + chatMessage.state.v + "; " + this);
											break;
										}
										this.data.state.v = UIData.State.Wait;
									} else {
										Debug.LogError ("server not online: " + this);
									}
								}
							}
							break;
						case UIData.State.Wait:
							{
								if (Server.IsServerOnline (chatMessage)) {
									startRoutine (ref this.wait, TaskWait ());
								} else {
									this.data.state.v = UIData.State.None;
									destroyRoutine (wait);
								}
							}
							break;
						default:
							Debug.LogError ("unknown state: " + this.data.state.v + "; " + this);
							break;
						}
					}
					// UI
					{
						if (btnDelete != null && tvDelete != null) {
							switch (this.data.state.v) {
							case UIData.State.None:
								{
									switch (chatMessage.state.v) {
									case ChatMessage.State.Normal:
										{
											btnDelete.enabled = true;
											tvDelete.text = txtDelete.get ("Delete");
										}
										break;
									case ChatMessage.State.Delete:
										{
											btnDelete.enabled = true;
											tvDelete.text = txtUnDelete.get ("UnDelete");
										}
										break;
									case ChatMessage.State.TrueDelete:
										{
											btnDelete.enabled = false;
											tvDelete.text = "Delete";
										}
										break;
									default:
										Debug.LogError ("unknown state: " + chatMessage.state.v + "; " + this);
										break;
									}
								}
								break;
							case UIData.State.Request:
								{
									switch (chatMessage.state.v) {
									case ChatMessage.State.Normal:
										{
											btnDelete.enabled = true;
											tvDelete.text = txtCancelDelete.get ("Cancel Delete?");
										}
										break;
									case ChatMessage.State.Delete:
										{
											btnDelete.enabled = true;
											tvDelete.text = txtCancelUnDelete.get ("Cancel UnDelete?");
										}
										break;
									case ChatMessage.State.TrueDelete:
										{
											btnDelete.enabled = false;
											tvDelete.text = "Delete";
										}
										break;
									default:
										Debug.LogError ("unknown state: " + chatMessage.state.v + "; " + this);
										break;
									}
								}
								break;
							case UIData.State.Wait:
								{
									switch (chatMessage.state.v) {
									case ChatMessage.State.Normal:
										{
											btnDelete.enabled = false;
											tvDelete.text = txtDeleting.get ("Deleting");
										}
										break;
									case ChatMessage.State.Delete:
										{
											btnDelete.enabled = false;
											tvDelete.text = txtUnDeleting.get ("UnDeleting");
										}
										break;
									case ChatMessage.State.TrueDelete:
										{
											btnDelete.enabled = false;
											tvDelete.text = "Delete";
										}
										break;
									default:
										Debug.LogError ("unknown state: " + chatMessage.state.v + "; " + this);
										break;
									}
								}
								break;
							default:
								Debug.LogError ("unknown state: " + this.data.state.v + "; " + this);
								break;
							}
						} else {
							Debug.LogError ("btnDelete, tvDelete null: " + this);
						}
					}
				} else {
					Debug.LogError ("chatMessage null: " + this);
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

	#region Task wait

	private Routine wait;

	public IEnumerator TaskWait()
	{
		if (this.data != null) {
			yield return new Wait (Global.WaitSendTime);
			this.data.state.v = UIData.State.None;
			Toast.showMessage ("request error");
			Debug.LogError ("request error: " + this);
		} else {
			Debug.LogError ("data null: " + this);
		}
	}

	public override List<Routine> getRoutineList ()
	{
		List<Routine> ret = new List<Routine> ();
		{
			ret.Add (wait);
		}
		return ret;
	}

	#endregion

	#region implement callBacks

	private Server server = null;

	public override void onAddCallBack<T> (T data)
	{
		if (data is UIData) {
			UIData uiData = data as UIData;
			// Setting
			Setting.get().addCallBack(this);
			// Child
			{
				uiData.chatMessage.allAddCallBack (this);
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
			if (data is ChatMessage) {
				ChatMessage chatMessage = data as ChatMessage;
				// reset
				{
					if (this.data != null) {
						this.data.reset ();
					} else {
						Debug.LogError ("data null: " + this);
					}
				}
				// Parent
				{
					DataUtils.addParentCallBack (chatMessage, this, ref this.server);
				}
				dirty = true;
				return;
			}
			// Parent
			if (data is Server) {
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
				uiData.chatMessage.allRemoveCallBack (this);
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
			if (data is ChatMessage) {
				ChatMessage chatMessage = data as ChatMessage;
				// Parent
				{
					DataUtils.removeParentCallBack (chatMessage, this, ref this.server);
				}
				return;
			}
			// Parent
			if (data is Server) {
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
			case UIData.Property.state:
				dirty = true;
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
			if (wrapProperty.p is ChatMessage) {
				switch ((ChatMessage.Property)wrapProperty.n) {
				case ChatMessage.Property.state:
					{
						dirty = true;
						needReset = true;
					}
					break;
				case ChatMessage.Property.time:
					break;
				case ChatMessage.Property.content:
					break;
				default:
					Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
					break;
				}
				return;
			}
			// Parent
			if (wrapProperty.p is Server) {
				Server.State.OnUpdateSyncStateChange (wrapProperty, this);
				return;
			}
		}
		Debug.LogError ("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
	}

	#endregion

	public void onClickBtnDelete()
	{
		if (this.data != null) {
			switch (this.data.state.v) {
			case UIData.State.None:
				this.data.state.v = UIData.State.Request;
				break;
			case UIData.State.Request:
				this.data.state.v = UIData.State.None;
				break;
			case UIData.State.Wait:
				Debug.LogError ("You are requesting: " + this);
				break;
			default:
				Debug.LogError ("unknown state: " + this.data.state.v + "; " + this);
				break;
			}
		} else {
			Debug.LogError ("data null: " + this);
		}
	}

}