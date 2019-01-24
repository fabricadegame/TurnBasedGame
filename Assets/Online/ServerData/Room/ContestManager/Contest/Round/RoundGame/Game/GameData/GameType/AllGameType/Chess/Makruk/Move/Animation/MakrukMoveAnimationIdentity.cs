﻿using UnityEngine;
using Mirror;
using System.Collections;
using System.Collections.Generic;

namespace Makruk
{
	public class MakrukMoveAnimationIdentity : DataIdentity
	{

		#region SyncVar

		#region board

		public SyncListInt board = new SyncListInt();

		private void OnBoardChanged(SyncListInt.Operation op, int index, int item)
		{
			if (this.netData.clientData != null) {
				IdentityUtils.onSyncListChange (this.netData.clientData.board, this.board, op, index);
			} else {
				// Debug.LogError ("clientData null: " + this);
			}
		}
		#endregion

		#region move

		[SyncVar(hook="onChangeMove")]
		public System.Int32 move;

		public void onChangeMove(System.Int32 newMove)
		{
			this.move = newMove;
			if (this.netData.clientData != null) {
				this.netData.clientData.move.v = newMove;
			} else {
				// Debug.LogError ("clientData null: "+this);
			}
		}

		#endregion

		#region chess960

		[SyncVar(hook="onChangeChess960")]
		public System.Boolean chess960;

		public void onChangeChess960(System.Boolean newChess960)
		{
			this.chess960 = newChess960;
			if (this.netData.clientData != null) {
				this.netData.clientData.chess960.v = newChess960;
			} else {
				// Debug.LogError ("clientData null: "+this);
			}
		}

		#endregion

		#region duration

		[SyncVar(hook="onChangeDuration")]
		public float duration;

		public void onChangeDuration(float newDuration)
		{
			this.duration = newDuration;
			if (this.netData.clientData != null) {
				this.netData.clientData.duration.v = newDuration;
			} else {
				// Debug.LogError ("clientData null: "+this);
			}
		}

		#endregion

		#endregion

		#region NetData

		private NetData<MakrukMoveAnimation> netData = new NetData<MakrukMoveAnimation>();

		public override NetDataDelegate getNetData ()
		{
			return this.netData;
		}

		public override void addSyncListCallBack ()
		{
			base.addSyncListCallBack ();
			this.board.Callback += OnBoardChanged;
		}

		public override void refreshClientData ()
		{
			if (this.netData.clientData != null) {
				IdentityUtils.refresh(this.netData.clientData.board, this.board);
				this.onChangeMove(this.move);
				this.onChangeChess960 (this.chess960);
				this.onChangeDuration (this.duration);
			} else {
				Debug.Log ("clientData null");
			}
		}

		public override int refreshDataSize ()
		{
			int ret = GetDataSize (this.netId);
			{
				ret += GetDataSize (this.board);
				ret += GetDataSize (this.move);
				ret += GetDataSize (this.chess960);
				ret += GetDataSize (this.duration);
			}
			return ret;
		}

		#endregion

		#region implemt callback

		public override void onAddCallBack<T> (T data)
		{
			if (data is MakrukMoveAnimation) {
				MakrukMoveAnimation makrukMoveAnimation = data as MakrukMoveAnimation;
				// Set new parent
				this.addTransformToParent();
				// Set property
				{
					this.serialize (this.searchInfor, makrukMoveAnimation.makeSearchInforms ());
					IdentityUtils.InitSync(this.board, makrukMoveAnimation.board.vs);
					this.move = makrukMoveAnimation.move.v;
					this.chess960 = makrukMoveAnimation.chess960.v;
					this.duration = makrukMoveAnimation.duration.v;
				}
				// Observer
				{
					GameObserver observer = GetComponent<GameObserver> ();
					if (observer != null) {
						observer.checkChange = new FollowParentObserver (observer);
						observer.setCheckChangeData (makrukMoveAnimation);
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
			if (data is MakrukMoveAnimation) {
				// MakrukMoveAnimation makrukMoveAnimation = data as MakrukMoveAnimation;
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
			if (wrapProperty.p is MakrukMoveAnimation) {
				switch ((MakrukMoveAnimation.Property)wrapProperty.n) {
				case MakrukMoveAnimation.Property.board:
					IdentityUtils.UpdateSyncList (this.board, syncs, GlobalCast<T>.CastingInt32);
					break;
				case MakrukMoveAnimation.Property.move:
					this.move = (System.Int32)wrapProperty.getValue ();
					break;
				case MakrukMoveAnimation.Property.chess960:
					this.chess960 = (System.Boolean)wrapProperty.getValue ();
					break;
				case MakrukMoveAnimation.Property.duration:
					this.duration = (float)wrapProperty.getValue ();
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