﻿using UnityEngine;
using Mirror;
using System.Collections;
using System.Collections.Generic;

namespace Shatranj.NoneRule
{
	public class ShatranjCustomMoveIdentity : DataIdentity
	{

		#region SyncVar

		#region fromX

		[SyncVar(hook="onChangeFromX")]
		public System.Int32 fromX;

		public void onChangeFromX(System.Int32 newFromX)
		{
			this.fromX = newFromX;
			if (this.netData.clientData != null) {
				this.netData.clientData.fromX.v = newFromX;
			} else {
				// Debug.LogError ("clientData null: "+this);
			}
		}

		#endregion

		#region fromY

		[SyncVar(hook="onChangeFromY")]
		public System.Int32 fromY;

		public void onChangeFromY(System.Int32 newFromY)
		{
			this.fromY = newFromY;
			if (this.netData.clientData != null) {
				this.netData.clientData.fromY.v = newFromY;
			} else {
				// Debug.LogError ("clientData null: "+this);
			}
		}

		#endregion

		#region destX

		[SyncVar(hook="onChangeDestX")]
		public System.Int32 destX;

		public void onChangeDestX(System.Int32 newDestX)
		{
			this.destX = newDestX;
			if (this.netData.clientData != null) {
				this.netData.clientData.destX.v = newDestX;
			} else {
				// Debug.LogError ("clientData null: "+this);
			}
		}

		#endregion

		#region destY

		[SyncVar(hook="onChangeDestY")]
		public System.Int32 destY;

		public void onChangeDestY(System.Int32 newDestY)
		{
			this.destY = newDestY;
			if (this.netData.clientData != null) {
				this.netData.clientData.destY.v = newDestY;
			} else {
				// Debug.LogError ("clientData null: "+this);
			}
		}

		#endregion

		#endregion

		#region NetData

		private NetData<ShatranjCustomMove> netData = new NetData<ShatranjCustomMove>();

		public override NetDataDelegate getNetData ()
		{
			return this.netData;
		}

		public override void refreshClientData ()
		{
			if (this.netData.clientData != null) {
				this.onChangeFromX(this.fromX);
				this.onChangeFromY(this.fromY);
				this.onChangeDestX(this.destX);
				this.onChangeDestY(this.destY);
			} else {
				Debug.Log ("clientData null");
			}
		}

		public override int refreshDataSize ()
		{
			int ret = GetDataSize (this.netId);
			{
				ret += GetDataSize (this.fromX);
				ret += GetDataSize (this.fromY);
				ret += GetDataSize (this.destX);
				ret += GetDataSize (this.destY);
			}
			return ret;
		}

		#endregion

		#region implemt callback

		public override void onAddCallBack<T> (T data)
		{
			if (data is ShatranjCustomMove) {
				ShatranjCustomMove shatranjCustomMove = data as ShatranjCustomMove;
				// Set new parent
				this.addTransformToParent();
				// Set property
				{
					this.serialize (this.searchInfor, shatranjCustomMove.makeSearchInforms ());
					this.fromX = shatranjCustomMove.fromX.v;
					this.fromY = shatranjCustomMove.fromY.v;
					this.destX = shatranjCustomMove.destX.v;
					this.destY = shatranjCustomMove.destY.v;
				}
				// Observer
				{
					GameObserver observer = GetComponent<GameObserver> ();
					if (observer != null) {
						observer.checkChange = new FollowParentObserver (observer);
						observer.setCheckChangeData (shatranjCustomMove);
					} else {
						Debug.LogError ("observer null: " + this);
					}
				}
				return;
			}
			Debug.LogError ("Don't process: " + data + "; " + this);
		}

		public override void onRemoveCallBack<T> (T data, bool isHide)
		{
			if (data is ShatranjCustomMove) {
				// ShatranjCustomMove shatranjCustomMove = data as ShatranjCustomMove;
				// Observer
				{
					GameObserver observer = GetComponent<GameObserver> ();
					if (observer != null) {
						observer.setCheckChangeData (null);
					} else {
						Debug.LogError ("observer null: " + this);
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
			if (wrapProperty.p is ShatranjCustomMove) {
				switch ((ShatranjCustomMove.Property)wrapProperty.n) {
				case ShatranjCustomMove.Property.fromX:
					this.fromX = (System.Int32)wrapProperty.getValue ();
					break;
				case ShatranjCustomMove.Property.fromY:
					this.fromY = (System.Int32)wrapProperty.getValue ();
					break;
				case ShatranjCustomMove.Property.destX:
					this.destX = (System.Int32)wrapProperty.getValue ();
					break;
				case ShatranjCustomMove.Property.destY:
					this.destY = (System.Int32)wrapProperty.getValue ();
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