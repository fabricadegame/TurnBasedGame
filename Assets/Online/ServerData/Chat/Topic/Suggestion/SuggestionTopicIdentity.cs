﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class SuggestionTopicIdentity : DataIdentity
{

	#region SyncVar

	#endregion

	#region NetData

	private NetData<SuggestionTopic> netData = new NetData<SuggestionTopic>();

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
		if (data is SuggestionTopic) {
			SuggestionTopic suggestionTopic = data as SuggestionTopic;
			// Set new parent
			this.addTransformToParent();
			// Set property
			{
				this.serialize (this.searchInfor, suggestionTopic.makeSearchInforms ());
			}
			// Observer
			{
				GameObserver observer = GetComponent<GameObserver> ();
				if (observer != null) {
					observer.checkChange = new FollowParentObserver (observer);
					observer.setCheckChangeData (suggestionTopic);
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
		if (data is SuggestionTopic) {
			// SuggestionTopic suggestionTopic = data as SuggestionTopic;
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
		if (wrapProperty.p is SuggestionTopic) {
			switch ((SuggestionTopic.Property)wrapProperty.n) {
			default:
				Debug.LogError ("Unknown wrapProperty: " + wrapProperty + "; " + this);
				break;
			}
			return;
		}
		Debug.LogError ("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
	}

	#endregion

}