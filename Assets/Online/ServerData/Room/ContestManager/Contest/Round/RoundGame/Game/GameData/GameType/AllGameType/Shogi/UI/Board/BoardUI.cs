﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Shogi
{
	public class BoardUI : UIBehavior<BoardUI.UIData>
	{

		#region UIData

		public class UIData: Data
		{
			
			public VP<ReferenceData<Shogi>> shogi;

			public VP<ShogiFenUI.UIData> shogiFen;

			public LP<PieceUI.UIData> pieces;

			public VP<HandUI.UIData> hand;

			#region Constructor

			public enum Property
			{
				shogi,
				shogiFen,
				pieces,
				hand
			}

			public UIData() : base()
			{
				this.shogi = new VP<ReferenceData<Shogi>>(this, (byte)Property.shogi, new ReferenceData<Shogi>(null));
				this.shogiFen = new VP<ShogiFenUI.UIData>(this, (byte)Property.shogiFen, new ShogiFenUI.UIData());
				this.pieces = new LP<PieceUI.UIData>(this, (byte)Property.pieces);
				this.hand = new VP<HandUI.UIData>(this, (byte)Property.hand, new HandUI.UIData());
			}

			#endregion

		}

		#endregion

		#region Refresh

		public override void refresh ()
		{
			if (dirty) {
				dirty = false;
				if (this.data != null) {
					Shogi shogi = this.data.shogi.v.data;
					if (shogi != null) {
						// shogiFen
						{
							ShogiFenUI.UIData shogiFenUIData = this.data.shogiFen.v;
							if (shogiFenUIData != null) {
								shogiFenUIData.shogi.v = new ReferenceData<Shogi> (shogi);
							} else {
								Debug.LogError ("shogiFenUIData null: " + this);
							}
						}
						// check load full
						bool isLoadFull = true;
						{
							// shogi
							if (isLoadFull) {
								isLoadFull = shogi.isLoadFull ();
							}
							// animation
							if (isLoadFull) {
								isLoadFull = AnimationManager.IsLoadFull (this.data);
							}
						}
						// process
						if (isLoadFull) {
							// Normal board
							{
								// get olds
								List<PieceUI.UIData> oldPieceUIs = new List<PieceUI.UIData> ();
								{
									oldPieceUIs.AddRange (this.data.pieces.vs);
								}
								// Find piece list
								List<int> pieces = shogi.piece.vs;
								Common.Piece dropPiece = Common.Piece.Empty;
								int dropPosition = -1;
								{
									MoveAnimation moveAnimation = GameDataBoardUI.UIData.getCurrentMoveAnimation (this.data);
									if (moveAnimation != null) {
										switch (moveAnimation.getType ()) {
										case GameMove.Type.ShogiMove:
											{
												ShogiMoveAnimation shogiMoveAnimation = moveAnimation as ShogiMoveAnimation;
												pieces = shogiMoveAnimation.piece.vs;
												// get drop
												{
													ShogiMove shogiMove = new ShogiMove ();
													{
														shogiMove.move.v = shogiMoveAnimation.move.v;
													}
													if (shogiMove.isDrop ()) {
														dropPosition = (int)shogiMove.to ();
														Common.Color color = shogiMoveAnimation.playerIndex.v == 0 ? Common.Color.Black : Common.Color.White;
														dropPiece = Common.colorAndPieceTypeToPiece (color, shogiMove.pieceTypeDropped ());
													}
												}
											}
											break;
										default:
											Debug.LogError ("unknown type: " + moveAnimation.getType () + "; " + this);
											break;
										}
									}
								}
								// Make pieceUI
								{
									for (int index = 0; index < pieces.Count; index++) {
										Common.Piece piece = (Common.Piece)pieces [index];
										if (Common.isRealPiece (piece) || index == dropPosition) {
											bool needAdd = false;
											// Find pieceUI
											PieceUI.UIData pieceUIData = null;
											{
												// Find old
												{
													for (int i = 0; i < oldPieceUIs.Count; i++) {
														PieceUI.UIData check = oldPieceUIs [i];
														if (check.position.v < 0) {
															pieceUIData = check;
														} else {
															if (check.position.v == index) {
																pieceUIData = check;
																break;
															}
														}
													}
												}
												// Make new
												if (pieceUIData == null) {
													pieceUIData = new PieceUI.UIData ();
													{
														pieceUIData.uid = this.data.pieces.makeId ();
													}
													needAdd = true;
												} else {
													oldPieceUIs.Remove (pieceUIData);
												}
											}
											// Update Property
											{
												// piece
												if (index != dropPosition) {
													pieceUIData.piece.v = piece;
												} else {
													pieceUIData.piece.v = dropPiece;
												}
												// position
												pieceUIData.position.v = index;
											}
											// add
											if (needAdd) {
												this.data.pieces.add (pieceUIData);
											}
										} else {
											// Debug.Log ("not real piece: " + piece + "; " + this);
										}
									}
								}
								// Remove oldPieceUIs not reuse
								// Debug.LogError("remove oldPieceUI: "+oldPieceUIs.Count);
								foreach (PieceUI.UIData oldPieceUI in oldPieceUIs) {
									// oldPieceUI.position.v = -1;
									this.data.pieces.remove (oldPieceUI);
								}
							}
						} else {
							Debug.LogError ("not load full");
							dirty = true;
						}
					} else {
						Debug.LogError ("shogi null: " + this);
					}
					// Hand
					{
						if (this.data.hand.v != null) {
							this.data.hand.v.shogi.v = new ReferenceData<Shogi> (shogi);
						} else {
							Debug.LogError ("hand null: " + this);
						}
					}
				} else {
					// Debug.LogError ("data null: " + this);
				}
			}
		}

		public override bool isShouldDisableUpdate ()
		{
			return true;
		}

		#endregion

		#region implement callBacks

		public ShogiFenUI shogiFenPrefab;
		public Transform shogiFenContainer;

		public PieceUI piecePrefab;
		public HandUI handPrefab;

		private AnimationManagerCheckChange<UIData> animationManagerCheckChange = new AnimationManagerCheckChange<UIData> ();

		public override void onAddCallBack<T> (T data)
		{
			if (data is UIData) {
				UIData uiData = data as UIData;
				// CheckChange
				{
					animationManagerCheckChange.needTimeChange = false;
					animationManagerCheckChange.addCallBack (this);
					animationManagerCheckChange.setData (uiData);
				}
				// Update
				{
					UpdateUtils.makeUpdate<AnimationSetDirtyUpdate, UIData> (uiData, this.transform);
				}
				// Child
				{
					uiData.shogi.allAddCallBack (this);
					uiData.shogiFen.allAddCallBack (this);
					uiData.pieces.allAddCallBack (this);
					uiData.hand.allAddCallBack (this);
				}
				dirty = true;
				return;
			}
			// checkChange
			{
				if (data is AnimationManagerCheckChange<UIData>) {
					dirty = true;
					return;
				}
			}
			// Child
			{
				if (data is Shogi) {
					dirty = true;
					return;
				}
				if (data is ShogiFenUI.UIData) {
					ShogiFenUI.UIData shogiFenUIData = data as ShogiFenUI.UIData;
					// UI
					{
						UIUtils.Instantiate (shogiFenUIData, shogiFenPrefab, shogiFenContainer);
					}
					dirty = true;
					return;
				}
				if (data is PieceUI.UIData) {
					PieceUI.UIData subUIData = data as PieceUI.UIData;
					// UI
					{
						UIUtils.Instantiate (subUIData, piecePrefab, this.transform);
					}
					dirty = true;
					return;
				}
				if (data is HandUI.UIData) {
					HandUI.UIData handUIData = data as HandUI.UIData;
					// UI
					{
						UIUtils.Instantiate (handUIData, handPrefab, this.transform);
					}
					dirty = true;
					return;
				}
			}
			Debug.LogError ("Don't process: " + data + "; " + this);
		}

		public override void onRemoveCallBack<T> (T data, bool isHide)
		{
			if (data is UIData) {
				UIData uiData = data as UIData;
				// CheckChange
				{
					animationManagerCheckChange.removeCallBack (this);
					animationManagerCheckChange.setData (null);
				}
				// Update
				{
					uiData.removeCallBackAndDestroy (typeof(AnimationSetDirtyUpdate));
				}
				// Child
				{
					uiData.shogi.allRemoveCallBack (this);
					uiData.shogiFen.allRemoveCallBack (this);
					uiData.pieces.allRemoveCallBack (this);
					uiData.hand.allRemoveCallBack (this);
				}
				this.setDataNull (uiData);
				return;
			}
			// checkChange
			{
				if (data is AnimationManagerCheckChange<UIData>) {
					return;
				}
			}
			// Child
			{
				if (data is Shogi) {
					return;
				}
				if (data is ShogiFenUI.UIData) {
					ShogiFenUI.UIData shogiFenUIData = data as ShogiFenUI.UIData;
					// UI
					{
						shogiFenUIData.removeCallBackAndDestroy (typeof(ShogiFenUI));
					}
					return;
				}
				if (data is PieceUI.UIData) {
					PieceUI.UIData subUIData = data as PieceUI.UIData;
					// UI
					{
						subUIData.removeCallBackAndDestroy (typeof(PieceUI));
					}
					return;
				}
				if (data is HandUI.UIData) {
					HandUI.UIData handUIData = data as HandUI.UIData;
					// UI
					{
						handUIData.removeCallBackAndDestroy (typeof(HandUI));
					}
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
			if (wrapProperty.p is UIData) {
				switch ((UIData.Property)wrapProperty.n) {
				case UIData.Property.shogi:
					{
						ValueChangeUtils.replaceCallBack (this, syncs);
						dirty = true;
					}
					break;
				case UIData.Property.shogiFen:
					{
						ValueChangeUtils.replaceCallBack (this, syncs);
						dirty = true;
					}
					break;
				case UIData.Property.pieces:
					{
						ValueChangeUtils.replaceCallBack (this, syncs);
						dirty = true;
					}
					break;
				case UIData.Property.hand:
					{
						ValueChangeUtils.replaceCallBack (this, syncs);
						dirty = true;
					}
					break;
				default:
					Debug.LogError ("unknown wrapProperty: " + wrapProperty + "; " + this);
					break;
				}
				return;
			}
			// Check Change
			{
				if (wrapProperty.p is AnimationManagerCheckChange<UIData>) {
					dirty = true;
					return;
				}
			}
			// Child
			{
				if (wrapProperty.p is Shogi) {
					switch ((Shogi.Property)wrapProperty.n) {
					case Shogi.Property.byTypeBB:
						break;
					case Shogi.Property.byColorBB:
						break;
					case Shogi.Property.goldsBB:
						break;
					case Shogi.Property.piece:
						dirty = true;
						break;
					case Shogi.Property.kingSquare:
						break;
					case Shogi.Property.hand:
						break;
					case Shogi.Property.turn:
						break;
					case Shogi.Property.evalList:
						break;
					case Shogi.Property.startState:
						break;
					case Shogi.Property.st:
						break;
					case Shogi.Property.gamePly:
						break;
					case Shogi.Property.nodes:
						break;
					default:
						Debug.LogError ("unknown wrapProperty: " + wrapProperty + "; " + this);
						break;
					}
					return;
				}
				if (wrapProperty.p is ShogiFenUI.UIData) {
					return;
				}
				if (wrapProperty.p is PieceUI.UIData) {
					return;
				}
				if (wrapProperty.p is HandUI.UIData) {
					return;
				}
			}
			Debug.LogError ("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
		}

		#endregion
	}
}