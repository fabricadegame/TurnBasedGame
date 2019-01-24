﻿using UnityEngine;
using UnityEngine.UI;
using frame8.Logic.Misc.Visual.UI.ScrollRectItemsAdapter;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Khet.NoneRule
{
	public class ChoosePieceHolder : SriaHolderBehavior<ChoosePieceHolder.UIData>
	{

		#region UIData

		public class UIData : BaseItemViewsHolder
		{

			public VP<Common.PieceAndPlayer> pieceAndPlayer;

			#region Constructor

			public enum Property
			{
				pieceAndPlayer
			}

			public UIData() : base()
			{
				this.pieceAndPlayer = new VP<Common.PieceAndPlayer>(this, (byte)Property.pieceAndPlayer, new Common.PieceAndPlayer());
			}

			#endregion

			public void updateView(ChoosePieceAdapter.UIData myParams)
			{
				// Find
				Common.PieceAndPlayer pieceAndPlayer = new Common.PieceAndPlayer();
				{
					if (ItemIndex >= 0 && ItemIndex < myParams.pieceAndPlayers.Count) {
						pieceAndPlayer = myParams.pieceAndPlayers [ItemIndex];
					} else {
						Debug.LogError ("ItemIdex error: " + this);
					}
				}
				// Update
				this.pieceAndPlayer.v = pieceAndPlayer;
			}

		}

		#endregion

		#region Refresh

		public Image imgPiece;

		public override void refresh ()
		{
			base.refresh ();
			if (dirty) {
				dirty = false;
				if (this.data != null) {
					// imgPiece
					{
						if (imgPiece != null) {
							imgPiece.sprite = KhetSpriteContainer.get ().GetSprite (this.data.pieceAndPlayer.v.player, this.data.pieceAndPlayer.v.piece, Common.Orientation.Up);
						} else {
							Debug.LogError ("imgPiece null: " + this);
						}
					}
				} else {
					// Debug.LogError ("data null: " + this);
				}
			}
		}

		#endregion

		#region implement callBacks

		public override void onAddCallBack<T> (T data)
		{
			if (data is UIData) {
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

				}
				this.setDataNull (uiData);
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
				case UIData.Property.pieceAndPlayer:
					dirty = true;
					break;
				default:
					Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
					break;
				}
				return;
			}
			Debug.LogError ("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
		}

		#endregion

		public void onClickBtnChoose()
		{
			if (this.data != null) {
				// Find ClientInput
				ClientInput clientInput = InputUI.UIData.findClientInput(this.data);
				// Process
				if (clientInput != null) {
					KhetCustomSet khetCustomSet = new KhetCustomSet ();
					{
						// x, y
						{
							SetPieceUI.UIData setPieceUIData = this.data.findDataInParent<SetPieceUI.UIData> ();
							if (setPieceUIData != null) {
								khetCustomSet.position.v = setPieceUIData.position.v;
							} else {
								Debug.LogError ("setPieceUIData null: " + this);
							}
						}
						khetCustomSet.player.v = this.data.pieceAndPlayer.v.player;
						khetCustomSet.piece.v = this.data.pieceAndPlayer.v.piece;
					}
					clientInput.makeSend (khetCustomSet);
				} else {
					Debug.LogError ("clientInput null: " + this);
				}
			} else {
				Debug.LogError ("data null: " + this);
			}
		}

	}
}