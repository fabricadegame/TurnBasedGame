﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace MineSweeper
{
	public class UITransformOrganizer : UpdateBehavior<UITransformOrganizer.UpdateData>
	{

		#region UpdateData

		public class UpdateData : Data
		{

			#region Constructor

			public enum Property
			{

			}

			public UpdateData() : base()
			{

			}

			#endregion

		}

		#endregion

		#region Update

		public override void update ()
		{
			if (dirty) {
				dirty = false;
				if (this.data != null) {
					MineSweeperGameDataUI.UIData mineSweeperGameDataUIData = this.data.findDataInParent<MineSweeperGameDataUI.UIData> ();
					GameDataBoardUI.UIData gameDataBoardUIData = this.data.findDataInParent<GameDataBoardUI.UIData> ();
					if (mineSweeperGameDataUIData != null && gameDataBoardUIData != null) {
						UpdateTransform.UpdateData mineSweeperTransform = mineSweeperGameDataUIData.updateTransform.v;
						UpdateTransform.UpdateData boardTransform = gameDataBoardUIData.updateTransform.v;
						if (mineSweeperTransform.size.v != Vector2.zero && boardTransform.size.v != Vector2.zero) {
							float scale = Mathf.Min (Mathf.Abs (boardTransform.size.v.x / 20f), Mathf.Abs (boardTransform.size.v.y / 20f));
							// new scale
							Vector3 newLocalScale = new Vector3 ();
							{
								Vector3 currentLocalScale = this.transform.localScale;
								// x
								newLocalScale.x = scale;
								// y
								newLocalScale.y = (gameDataBoardUIData.perspective.v.playerView.v == 0 ? 1 : -1) * scale;
								// z
								newLocalScale.z = 1;
							}
							this.transform.localScale = newLocalScale;
						} else {
							Debug.LogError ("why transform zero");
						}
					} else {
						Debug.LogError ("mineSweeperGameDataUIData or gameDataBoardUIData null: " + this);
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

		#region implement callBacks

		private MineSweeperGameDataUI.UIData mineSweeperGameDataUIData = null;
		private GameDataBoardCheckTransformChange<UpdateData> gameDataBoardCheckTransformChange = new GameDataBoardCheckTransformChange<UpdateData>();

		public override void onAddCallBack<T> (T data)
		{
			if (data is UpdateData) {
				UpdateData updateData = data as UpdateData;
				// CheckChange
				{
					gameDataBoardCheckTransformChange.addCallBack (this);
					gameDataBoardCheckTransformChange.setData (updateData);
				}
				// Parent
				{
					DataUtils.addParentCallBack (updateData, this, ref this.mineSweeperGameDataUIData);
				}
				dirty = true;
				return;
			}
			// CheckChange
			if (data is GameDataBoardCheckTransformChange<UpdateData>) {
				dirty = true;
				return;
			}
			// Parent
			{
				if (data is MineSweeperGameDataUI.UIData) {
					MineSweeperGameDataUI.UIData mineSweeperGameDataUIData = data as MineSweeperGameDataUI.UIData;
					{
						mineSweeperGameDataUIData.updateTransform.allAddCallBack (this);
					}
					dirty = true;
					return;
				}
				if (data is UpdateTransform.UpdateData) {
					dirty = true;
					return;
				}
			}
			Debug.LogError ("Don't process: " + data + "; " + this);
		}

		public override void onRemoveCallBack<T> (T data, bool isHide)
		{
			if (data is UpdateData) {
				UpdateData updateData = data as UpdateData;
				// CheckChange
				{
					gameDataBoardCheckTransformChange.removeCallBack (this);
					gameDataBoardCheckTransformChange.setData (null);
				}
				// Parent
				{
					DataUtils.removeParentCallBack (updateData, this, ref this.mineSweeperGameDataUIData);
				}
				this.setDataNull (updateData);
				return;
			}
			// CheckChange
			if (data is GameDataBoardCheckTransformChange<UpdateData>) {
				return;
			}
			// Parent
			{
				if (data is MineSweeperGameDataUI.UIData) {
					MineSweeperGameDataUI.UIData mineSweeperGameDataUIData = data as MineSweeperGameDataUI.UIData;
					{
						mineSweeperGameDataUIData.updateTransform.allRemoveCallBack (this);
					}
					return;
				}
				if (data is UpdateTransform.UpdateData) {
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
			if (wrapProperty.p is UpdateData) {
				switch ((UpdateData.Property)wrapProperty.n) {
				default:
					Debug.LogError ("unknown wrapProperty: " + wrapProperty + "; " + this);
					break;
				}
				return;
			}
			// CheckChange
			if (wrapProperty.p is GameDataBoardCheckTransformChange<UpdateData>) {
				dirty = true;
				return;
			}
			// Parent
			{
				if (wrapProperty.p is MineSweeperGameDataUI.UIData) {
					switch ((MineSweeperGameDataUI.UIData.Property)wrapProperty.n) {
					case MineSweeperGameDataUI.UIData.Property.gameData:
						break;
					case MineSweeperGameDataUI.UIData.Property.updateTransform:
						{
							ValueChangeUtils.replaceCallBack (this, syncs);
							dirty = true;
						}
						break;
					case MineSweeperGameDataUI.UIData.Property.isOnAnimation:
						break;
					/*case MineSweeperGameDataUI.UIData.Property.board:
						break;
					case MineSweeperGameDataUI.UIData.Property.lastMove:
						break;
					case MineSweeperGameDataUI.UIData.Property.showHint:
						break;
					case MineSweeperGameDataUI.UIData.Property.inputUI:
						break;*/
					default:
						Debug.LogError ("unknown wrapProperty: " + wrapProperty + "; " + this);
						break;
					}
					return;
				}
				if (wrapProperty.p is UpdateTransform.UpdateData) {
					switch ((UpdateTransform.UpdateData.Property)wrapProperty.n) {
					case UpdateTransform.UpdateData.Property.position:
						dirty = true;
						break;
					case UpdateTransform.UpdateData.Property.rotation:
						dirty = true;
						break;
					case UpdateTransform.UpdateData.Property.scale:
						dirty = true;
						break;
					case UpdateTransform.UpdateData.Property.size:
						dirty = true;
						break;
					default:
						Debug.LogError ("unknown wrapProperty: " + wrapProperty + "; " + this + "; " + syncs);
						break;
					}
					return;
				}
			}
			Debug.LogError ("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
		}

		#endregion

	}
}