﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using AdvancedCoroutines;
using Foundation.Tasks;

public class FriendStateBanUI : UIBehavior<FriendStateBanUI.UIData>
{

	#region UIData

	public class UIData : FriendStateUI.UIData.Sub
	{

		public VP<ReferenceData<FriendStateBan>> friendStateBan;

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
			friendStateBan,
			state
		}

		public UIData() : base()
		{
			this.friendStateBan = new VP<ReferenceData<FriendStateBan>>(this, (byte)Property.friendStateBan, new ReferenceData<FriendStateBan>(null));
			this.state = new VP<State>(this, (byte)Property.state, State.None);
		}

		#endregion

		public override Friend.State.Type getType ()
		{
			return Friend.State.Type.Ban;
		}

		public void reset()
		{
			this.state.v = State.None;
		}

	}

	#endregion

	#region Refresh

	public Button btnUnBan;
	public GameObject requestingIndicator;

	public override void refresh ()
	{
		if (dirty) {
			dirty = false;
			if (this.data != null) {
				FriendStateBan friendStateBan = this.data.friendStateBan.v.data;
				if (friendStateBan != null) {
					// btnUnBan
					{
						if (btnUnBan != null) {
							if (friendStateBan.isCanUnBan (Server.getProfileUserId (friendStateBan))) {
								switch (this.data.state.v) {
								case UIData.State.None:
									btnUnBan.enabled = true;
									break;
								case UIData.State.Request:
								case UIData.State.Wait:
									btnUnBan.enabled = false;
									break;
								default:
									Debug.LogError ("Unknown state: " + this.data.state.v + "; " + this);
									break;
								}
							} else {
								btnUnBan.enabled = false;
								this.data.state.v = UIData.State.None;
							}
						} else {
							Debug.LogError ("btnUnBan null: " + this);
						}
					}
					// requestingIndicator
					{
						if (requestingIndicator != null) {
							switch (this.data.state.v) {
							case UIData.State.None:
								requestingIndicator.SetActive (false);
								break;
							case UIData.State.Request:
							case UIData.State.Wait:
								requestingIndicator.SetActive (true);
								break;
							default:
								Debug.LogError ("unknown state: " + this.data.state.v + "; " + this);
								break;
							}
						} else {
							Debug.LogError ("requestingIndicator null: " + this);
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
								if (Server.IsServerOnline (friendStateBan)) {
									friendStateBan.requestUnBan (Server.getProfileUserId (friendStateBan));
									this.data.state.v = UIData.State.Wait;
								} else {
									Debug.LogError ("server not online: " + this);
								}
							}
							break;
						case UIData.State.Wait:
							{
								if (Server.IsServerOnline (friendStateBan)) {
									startRoutine (ref this.wait, TaskWait ());
								} else {
									this.data.state.v = UIData.State.None;
								}
							}
							break;
						default:
							Debug.LogError ("unknown state: " + this.data.state.v + "; " + this);
							break;
						}
					}
				} else {
					Debug.LogError ("friendStateAccept null: " + this);
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

	#region Task wait

	private Routine wait;

	public IEnumerator TaskWait()
	{
		if (this.data != null) {
			yield return new Wait (Global.WaitSendTime);
			if (this.data != null) {
				this.data.state.v = UIData.State.None;
			} else {
				Debug.LogError ("data null: " + this);
			}
			Toast.showMessage ("request unban error");
			Debug.LogError ("request unfriend error: " + this);
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
	private Friend friend = null;

	public override void onAddCallBack<T> (T data)
	{
		if (data is UIData) {
			UIData uiData = data as UIData;
			// Child
			{
				uiData.friendStateBan.allAddCallBack (this);
			}
			dirty = true;
			return;
		}
		// Child
		{
			if (data is FriendStateBan) {
				FriendStateBan friendStateBan = data as FriendStateBan;
				// Reset
				{
					if (this.data != null) {
						this.data.reset ();
					} else {
						Debug.LogError ("data null: " + this);
					}
				}
				// Parent
				{
					DataUtils.addParentCallBack (friendStateBan, this, ref this.server);
					DataUtils.addParentCallBack (friendStateBan, this, ref this.friend);
				}
				dirty = true;
				return;
			}
			// Parent
			{
				// Server
				if (data is Server) {
					dirty = true;
					return;
				}
				// Friend
				{
					if (data is Friend) {
						Friend friend = data as Friend;
						// Child
						{
							friend.user1.allAddCallBack (this);
							friend.user2.allAddCallBack (this);
						}
						dirty = true;
						return;
					}
					// Child
					if (data is Human) {
						dirty = true;
						return;
					}
				}
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
				uiData.friendStateBan.allRemoveCallBack (this);
			}
			this.setDataNull (uiData);
			return;
		}
		// Child
		{
			if (data is FriendStateBan) {
				FriendStateBan friendStateBan = data as FriendStateBan;
				// Parent
				{
					DataUtils.removeParentCallBack (friendStateBan, this, ref this.server);
					DataUtils.removeParentCallBack (friendStateBan, this, ref this.friend);
				}
				return;
			}
			// Parent
			{
				// Server
				if (data is Server) {
					return;
				}
				// Friend
				{
					if (data is Friend) {
						Friend friend = data as Friend;
						// Child
						{
							friend.user1.allRemoveCallBack (this);
							friend.user2.allRemoveCallBack (this);
						}
						return;
					}
					// Child
					if (data is Human) {
						return;
					}
				}
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
			case UIData.Property.friendStateBan:
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
		// Child
		{
			if (wrapProperty.p is FriendStateBan) {
				switch ((FriendStateBan.Property)wrapProperty.n) {
				case FriendStateBan.Property.userId:
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
				// Server
				if (wrapProperty.p is Server) {
					Server.State.OnUpdateSyncStateChange (wrapProperty, this);
					return;
				}
				// Friend
				{
					if (wrapProperty.p is Friend) {
						switch ((Friend.Property)wrapProperty.n) {
						case Friend.Property.state:
							break;
						case Friend.Property.user1:
							{
								ValueChangeUtils.replaceCallBack (this, syncs);
								dirty = true;
							}
							break;
						case Friend.Property.user2:
							{
								ValueChangeUtils.replaceCallBack (this, syncs);
								dirty = true;
							}
							break;
						case Friend.Property.time:
							break;
						case Friend.Property.chatRoom:
							break;
						default:
							Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
							break;
						}
						return;
					}
					// Child
					if (wrapProperty.p is Human) {
						Human.onUpdateSyncPlayerIdChange (wrapProperty, this);
						return;
					}
				}
			}
		}
		Debug.LogError ("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
	}

	#endregion

	public void onClickBtnUnBan()
	{
		if (this.data != null) {
			if (this.data.state.v == UIData.State.None) {
				this.data.state.v = UIData.State.Request;
			}
		} else {
			Debug.LogError ("data null: " + this);
		}
	}

}