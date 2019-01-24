﻿using UnityEngine;
using Mirror;
using System.Collections;
using System.Collections.Generic;

public class FriendStateAcceptIdentity : DataIdentity
{

	#region SyncVar

	#endregion

	#region NetData

	private NetData<FriendStateAccept> netData = new NetData<FriendStateAccept>();

	public override NetDataDelegate getNetData ()
	{
		return this.netData;
	}

	public override void refreshClientData ()
	{
		if (this.netData.clientData != null) {
		} else {
			Debug.Log ("clientData null");
		}
	}

	public override int refreshDataSize ()
	{
		int ret = GetDataSize (this.netId);
		{
		}
		return ret;
	}

	#endregion

	#region implemt callback

	public override void onAddCallBack<T> (T data)
	{
		if (data is FriendStateAccept) {
			FriendStateAccept friendStateAccept = data as FriendStateAccept;
			// Set new parent
			this.addTransformToParent();
			// Set property
			{
				this.serialize (this.searchInfor, friendStateAccept.makeSearchInforms ());
			}
			// Observer
			{
				GameObserver observer = GetComponent<GameObserver> ();
				if (observer != null) {
					observer.checkChange = new FollowParentObserver (observer);
					observer.setCheckChangeData (friendStateAccept);
				} else {
					Debug.LogError ("observer null");
				}
			}
			return;
		}
		Debug.LogError ("Don't process: " + data + "; " + this);
	}

	public override void onRemoveCallBack<T> (T data, bool isHide)
	{
		if (data is FriendStateAccept) {
			// FriendStateAccept friendStateAccept = data as FriendStateAccept;
			// Observer
			{
				GameObserver observer = GetComponent<GameObserver> ();
				if (observer != null) {
					observer.setCheckChangeData (null);
				} else {
					Debug.LogError ("observer null");
				}
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
		if (wrapProperty.p is FriendStateAccept) {
			switch ((FriendStateAccept.Property)wrapProperty.n) {
			default:
				Debug.LogError ("Unknown wrapProperty: " + wrapProperty + "; " + this);
				break;
			}
			return;
		}
		Debug.LogError ("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
	}

	#endregion

	#region UnFriend

	public void requestUnFriend(uint userId)
	{
		ClientConnectIdentity clientConnect = ClientConnectIdentity.findYourClientConnectIdentity (this.netData.clientData);
		if (clientConnect != null) {
			clientConnect.CmdFriendStateAcceptUnFriend (this.netId, userId);
		} else {
			Debug.LogError ("Cannot find clientConnect: " + this);
		}
	}

	public void unFriend(uint userId){
		if (this.netData.serverData != null) {
			this.netData.serverData.unFriend (userId);
		} else {
			Debug.LogError ("serverData null: " + this);
		}
	}

	#endregion

}