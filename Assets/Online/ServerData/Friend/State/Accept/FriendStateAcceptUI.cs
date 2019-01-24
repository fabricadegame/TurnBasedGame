﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using AdvancedCoroutines;
using Foundation.Tasks;

public class FriendStateAcceptUI : UIBehavior<FriendStateAcceptUI.UIData>
{

	#region UIData

	public class UIData : FriendStateUI.UIData.Sub
	{

		public VP<ReferenceData<FriendStateAccept>> friendStateAccept;

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
			friendStateAccept,
			state
		}

		public UIData() : base()
		{
			this.friendStateAccept = new VP<ReferenceData<FriendStateAccept>>(this, (byte)Property.friendStateAccept, new ReferenceData<FriendStateAccept>(null));
			this.state = new VP<State>(this, (byte)Property.state, State.None);
		}

		#endregion

		public override Friend.State.Type getType ()
		{
			return Friend.State.Type.Accept;
		}

		public void reset()
		{
			this.state.v = State.None;
		}

	}

	#endregion

	#region Refresh

	public Button btnUnFriend;
	public GameObject requestingIndicator;

	public override void refresh ()
	{
		if (dirty) {
			dirty = false;
			if (this.data != null) {
				FriendStateAccept friendStateAccept = this.data.friendStateAccept.v.data;
				if (friendStateAccept != null) {
					// btnUnFriend
					{
						if (btnUnFriend != null) {
							switch (this.data.state.v) {
							case UIData.State.None:
								btnUnFriend.enabled = true;
								break;
							case UIData.State.Request:
							case UIData.State.Wait:
								btnUnFriend.enabled = false;
								break;
							default:
								Debug.LogError ("Unknown state: " + this.data.state.v + "; " + this);
								break;
							}
						} else {
							Debug.LogError ("btnUnFriend null: " + this);
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
								if (Server.IsServerOnline (friendStateAccept)) {
									friendStateAccept.requestUnFriend (Server.getProfileUserId (friendStateAccept));
									this.data.state.v = UIData.State.Wait;
								} else {
									Debug.LogError ("server not online: " + this);
								}
							}
							break;
						case UIData.State.Wait:
							{
								if (Server.IsServerOnline (friendStateAccept)) {
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
			this.data.state.v = UIData.State.None;
			Toast.showMessage ("request unfriend error");
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

	public override void onAddCallBack<T> (T data)
	{
		if (data is UIData) {
			UIData uiData = data as UIData;
			// Child
			{
				uiData.friendStateAccept.allAddCallBack (this);
			}
			dirty = true;
			return;
		}
		// Child
		if (data is FriendStateAccept) {
			FriendStateAccept friendStateAccept = data as FriendStateAccept;
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
				DataUtils.addParentCallBack (friendStateAccept, this, ref this.server);
			}
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
				uiData.friendStateAccept.allRemoveCallBack (this);
			}
			this.setDataNull (uiData);
			return;
		}
		// Child
		if (data is FriendStateAccept) {
			FriendStateAccept friendStateAccept = data as FriendStateAccept;
			// Parent
			{
				DataUtils.removeParentCallBack (friendStateAccept, this, ref this.server);
			}
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
			case UIData.Property.friendStateAccept:
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
			if (wrapProperty.p is FriendStateAccept) {
				switch ((FriendStateAccept.Property)wrapProperty.n) {
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

	public void onClickBtnUnFriend()
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