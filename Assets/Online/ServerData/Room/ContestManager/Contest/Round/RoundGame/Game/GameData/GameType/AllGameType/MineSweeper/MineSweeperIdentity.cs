﻿using UnityEngine;
using Mirror;
using System.Collections;
using System.Collections.Generic;

namespace MineSweeper
{
	public class MineSweeperIdentity : DataIdentity
	{

		#region SyncVar

		#region N

		[SyncVar(hook="onChangeN")]
		public System.Int32 N;

		public void onChangeN(System.Int32 newN)
		{
			this.N = newN;
			if (this.netData.clientData != null) {
				this.netData.clientData.Y.v = newN;
			} else {
				// Debug.LogError ("clientData null: "+this);
			}
		}

		#endregion

		#region M

		[SyncVar(hook="onChangeM")]
		public System.Int32 M;

		public void onChangeM(System.Int32 newM)
		{
			this.M = newM;
			if (this.netData.clientData != null) {
				this.netData.clientData.X.v = newM;
			} else {
				// Debug.LogError ("clientData null: "+this);
			}
		}

		#endregion

		#region K

		[SyncVar(hook="onChangeK")]
		public System.Int32 K;

		public void onChangeK(System.Int32 newK)
		{
			this.K = newK;
			if (this.netData.clientData != null) {
				this.netData.clientData.K.v = newK;
			} else {
				// Debug.LogError ("clientData null: "+this);
			}
		}

		#endregion

		#region bombs

		public SyncListSByte bombs = new SyncListSByte();

		private void OnBombsChanged(SyncListSByte.Operation op, int index, MySByte item)
		{
			if (this.netData.clientData != null) {
				IdentityUtils.onSyncListChange (this.netData.clientData.bombs, this.bombs, op, index, MySByte.sbyteConvert);
			} else {
				// Debug.LogError ("clientData null: " + this);
			}
		}
		#endregion

		#region booom

		[SyncVar(hook="onChangeBooom")]
		public System.Boolean booom;

		public void onChangeBooom(System.Boolean newBooom)
		{
			this.booom = newBooom;
			if (this.netData.clientData != null) {
				this.netData.clientData.booom.v = newBooom;
			} else {
				// Debug.LogError ("clientData null: "+this);
			}
		}

		#endregion

		#region flags

		public SyncListSByte flags = new SyncListSByte();

		private void OnFlagsChanged(SyncListSByte.Operation op, int index, MySByte item)
		{
			if (this.netData.clientData != null) {
				IdentityUtils.onSyncListChange (this.netData.clientData.flags, this.flags, op, index, MySByte.sbyteConvert);
			} else {
				// Debug.LogError ("clientData null: " + this);
			}
		}
		#endregion

		#region board

		public SyncListSByte board = new SyncListSByte();

		private void OnBoardChanged(SyncListSByte.Operation op, int index, MySByte item)
		{
			if (this.netData.clientData != null) {
				IdentityUtils.onSyncListChange (this.netData.clientData.board, this.board, op, index, MySByte.sbyteConvert);
			} else {
				// Debug.LogError ("clientData null: " + this);
			}
		}
		#endregion

		#region minesFound

		[SyncVar(hook="onChangeMinesFound")]
		public System.Int32 minesFound;

		public void onChangeMinesFound(System.Int32 newMinesFound)
		{
			this.minesFound = newMinesFound;
			if (this.netData.clientData != null) {
				this.netData.clientData.minesFound.v = newMinesFound;
			} else {
				// Debug.LogError ("clientData null: "+this);
			}
		}

		#endregion

		#region init

		[SyncVar(hook="onChangeInit")]
		public System.Boolean init;

		public void onChangeInit(System.Boolean newInit)
		{
			this.init = newInit;
			if (this.netData.clientData != null) {
				this.netData.clientData.init.v = newInit;
			} else {
				// Debug.LogError ("clientData null: "+this);
			}
		}

		#endregion

		#region neb

		[SyncVar]
		public int neb;

		#endregion

		#region allowWatchBomb

		[SyncVar(hook="onChangeAllowWatchBomb")]
		public System.Boolean allowWatchBomb;

		public void onChangeAllowWatchBomb(System.Boolean newAllowWatchBomb)
		{
			this.allowWatchBomb = newAllowWatchBomb;
			if (this.netData.clientData != null) {
				this.netData.clientData.allowWatchBoomb.v = newAllowWatchBomb;
			} else {
				// Debug.LogError ("clientData null: "+this);
			}
		}

		#endregion

		#region isCustom

		[SyncVar(hook="onChangeIsCustom")]
		public System.Boolean isCustom;

		public void onChangeIsCustom(System.Boolean newIsCustom)
		{
			this.isCustom = newIsCustom;
			if (this.netData.clientData != null) {
				this.netData.clientData.isCustom.v = newIsCustom;
			} else {
				// Debug.LogError ("clientData null: "+this);
			}
		}

		#endregion

		#endregion

		#region NetData

		private NetData<MineSweeper> netData = new NetData<MineSweeper>();

		public override NetDataDelegate getNetData ()
		{
			return this.netData;
		}

		public override void addSyncListCallBack ()
		{
			base.addSyncListCallBack ();
			this.bombs.Callback += OnBombsChanged;
			this.flags.Callback += OnFlagsChanged;
			this.board.Callback += OnBoardChanged;
		}

		public override void refreshClientData ()
		{
			if (this.netData.clientData != null) {
				this.onChangeN(this.N);
				this.onChangeM(this.M);
				this.onChangeK(this.K);
				IdentityUtils.refresh(this.netData.clientData.bombs, this.bombs, MySByte.sbyteConvert);
				this.onChangeBooom(this.booom);
				IdentityUtils.refresh(this.netData.clientData.flags, this.flags, MySByte.sbyteConvert);
				IdentityUtils.refresh(this.netData.clientData.board, this.board, MySByte.sbyteConvert);
				this.onChangeMinesFound(this.minesFound);
				this.onChangeInit(this.init);
				this.onChangeAllowWatchBomb (this.allowWatchBomb);
				this.onChangeIsCustom (this.isCustom);
			} else {
				Debug.Log ("clientData null");
			}
		}

		public override int refreshDataSize ()
		{
			int ret = GetDataSize (this.netId);
			{
				ret += GetDataSize (this.N);
				ret += GetDataSize (this.M);
				ret += GetDataSize (this.K);
				ret += GetDataSize (this.bombs);
				ret += GetDataSize (this.booom);
				ret += GetDataSize (this.flags);
				ret += GetDataSize (this.board);
				ret += GetDataSize (this.minesFound);
				ret += GetDataSize (this.init);
				ret += GetDataSize (this.allowWatchBomb);
				ret += GetDataSize (this.isCustom);
			}
			return ret;
		}

		#endregion

		#region implemt callback

		public override void onAddCallBack<T> (T data)
		{
			if (data is MineSweeper) {
				MineSweeper mineSweeper = data as MineSweeper;
				// Set new parent
				this.addTransformToParent();
				// Set property
				{
					this.serialize (this.searchInfor, mineSweeper.makeSearchInforms ());
					this.N = mineSweeper.Y.v;
					this.M = mineSweeper.X.v;
					this.K = mineSweeper.K.v;
					IdentityUtils.InitSync(this.bombs, mineSweeper.bombs, MySByte.mySByteConvert);
					this.booom = mineSweeper.booom.v;
					IdentityUtils.InitSync(this.flags, mineSweeper.flags, MySByte.mySByteConvert);
					IdentityUtils.InitSync(this.board, mineSweeper.board, MySByte.mySByteConvert);
					this.minesFound = mineSweeper.minesFound.v;
					this.init = mineSweeper.init.v;
					this.neb = mineSweeper.neb.vs.Count;
					this.allowWatchBomb = mineSweeper.allowWatchBoomb.v;
					this.isCustom = mineSweeper.isCustom.v;
				}
				// Observer
				{
					GameObserver observer = GetComponent<GameObserver> ();
					if (observer != null) {
						observer.checkChange = new FollowParentObserver (observer);
						observer.setCheckChangeData (mineSweeper);
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
			if (data is MineSweeper) {
				// MineSweeper mineSweeper = data as MineSweeper;
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
			if (wrapProperty.p is MineSweeper) {
				switch ((MineSweeper.Property)wrapProperty.n) {
				case MineSweeper.Property.Y:
					this.N = (System.Int32)wrapProperty.getValue ();
					break;
				case MineSweeper.Property.X:
					this.M = (System.Int32)wrapProperty.getValue ();
					break;
				case MineSweeper.Property.K:
					this.K = (System.Int32)wrapProperty.getValue ();
					break;
				case MineSweeper.Property.bombs:
					IdentityUtils.UpdateSyncList (this.bombs, syncs, GlobalCast<T>.CastingMySByte);
					break;
				case MineSweeper.Property.booom:
					this.booom = (System.Boolean)wrapProperty.getValue ();
					break;
				case MineSweeper.Property.flags:
					IdentityUtils.UpdateSyncList (this.flags, syncs, GlobalCast<T>.CastingMySByte);
					break;
				case MineSweeper.Property.board:
					IdentityUtils.UpdateSyncList (this.board, syncs, GlobalCast<T>.CastingMySByte);
					break;
				case MineSweeper.Property.minesFound:
					this.minesFound = (System.Int32)wrapProperty.getValue ();
					break;
				case MineSweeper.Property.init:
					this.init = (System.Boolean)wrapProperty.getValue ();
					break;
				case MineSweeper.Property.neb:
					{
						MineSweeper mineSweeper = wrapProperty.p as MineSweeper;
						this.neb = mineSweeper.neb.vs.Count;
					}
					break;
				case MineSweeper.Property.allowWatchBoomb:
					this.allowWatchBomb = (System.Boolean)wrapProperty.getValue ();
					break;
				case MineSweeper.Property.isCustom:
					this.isCustom = (bool)wrapProperty.getValue ();
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