﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

namespace UndoRedo
{
	public class RequestLastTurnIdentity : DataIdentity
	{

		#region SyncVar

		#region operation

		[SyncVar(hook="onChangeOperation")]
		public UndoRedoRequest.Operation operation;

		public void onChangeOperation(UndoRedoRequest.Operation newOperation)
		{
			this.operation = newOperation;
			if (this.netData.clientData != null) {
				this.netData.clientData.operation.v = newOperation;
			} else {
				// Debug.LogError ("clientData null: "+this);
			}
		}

		#endregion

		#endregion

		#region NetData

		private NetData<RequestLastTurn> netData = new NetData<RequestLastTurn>();

		public override NetDataDelegate getNetData ()
		{
			return this.netData;
		}

		public override void refreshClientData ()
		{
			if (this.netData.clientData != null) {
				this.onChangeOperation(this.operation);
			} else {
				Debug.Log ("clientData null");
			}
		}

		public override int refreshDataSize ()
		{
			int ret = GetDataSize (this.netId);
			{
				ret += GetDataSize (this.operation);
			}
			return ret;
		}

		#endregion

		#region implemt callback

		public override void onAddCallBack<T> (T data)
		{
			if (data is RequestLastTurn) {
				RequestLastTurn requestLastTurn = data as RequestLastTurn;
				// Set new parent
				this.addTransformToParent();
				// Set property
				{
					this.serialize (this.searchInfor, requestLastTurn.makeSearchInforms ());
					this.operation = requestLastTurn.operation.v;
				}
				// Observer
				{
					GameObserver observer = GetComponent<GameObserver> ();
					if (observer != null) {
						observer.checkChange = new FollowParentObserver (observer);
						observer.setCheckChangeData (requestLastTurn);
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
			if (data is RequestLastTurn) {
				// RequestLastTurn requestLastTurn = data as RequestLastTurn;
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
			if (wrapProperty.p is RequestLastTurn) {
				switch ((RequestLastTurn.Property)wrapProperty.n) {
				case RequestLastTurn.Property.operation:
					this.operation = (UndoRedoRequest.Operation)wrapProperty.getValue ();
					break;
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
}