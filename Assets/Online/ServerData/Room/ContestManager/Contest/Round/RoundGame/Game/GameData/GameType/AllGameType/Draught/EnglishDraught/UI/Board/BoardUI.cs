﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace EnglishDraught
{
	public class BoardUI : UIBehavior<BoardUI.UIData>
	{

		#region UIData

		public class UIData : Data
		{
			
			public VP<ReferenceData<EnglishDraught>> englishDraught;

			public VP<EnglishDraughtFenUI.UIData> englishDraughtFen;

			public LP<PieceUI.UIData> pieces;

			#region Constructor

			public enum Property
			{
				englishDraught,
				englishDraughtFen,
				pieces
			}

			public UIData() : base()
			{
				this.englishDraught = new VP<ReferenceData<EnglishDraught>>(this, (byte)Property.englishDraught, new ReferenceData<EnglishDraught>(null));
				this.englishDraughtFen = new VP<EnglishDraughtFenUI.UIData>(this, (byte)Property.englishDraughtFen, new EnglishDraughtFenUI.UIData());
				this.pieces = new LP<PieceUI.UIData>(this, (byte)Property.pieces);
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
					EnglishDraught englishDraught = this.data.englishDraught.v.data;
					if (englishDraught != null) {
						// check load full
						bool isLoadFull = true;
						{
							// englishDraught
							if (isLoadFull) {
								if (englishDraught.Sqs.vs.Count == 0) {
									Debug.LogError ("board not load");
									isLoadFull = false;
								}
							}
							// animation
							if (isLoadFull) {
								isLoadFull = AnimationManager.IsLoadFull (this.data);
							}
						}
						// process
						if (isLoadFull) {
							// englishDraughtFen
							{
								EnglishDraughtFenUI.UIData englishDraughtFenUIData = this.data.englishDraughtFen.v;
								if (englishDraughtFenUIData != null) {
									englishDraughtFenUIData.englishDraught.v = new ReferenceData<EnglishDraught> (englishDraught);
								} else {
									Debug.LogError ("englishDraughtFenUIData null: " + this);
								}
							}
							// pieces
							{
								// get old
								List<PieceUI.UIData> oldPieces = new List<PieceUI.UIData> ();
								{
									oldPieces.AddRange (this.data.pieces.vs);
								}
								// Find Sqs
								List<byte> Sqs = englishDraught.Sqs.vs;
								{
									MoveAnimation moveAnimation = GameDataBoardUI.UIData.getCurrentMoveAnimation (this.data);
									if (moveAnimation != null) {
										switch (moveAnimation.getType ()) {
										case GameMove.Type.EnglishDraughtMove:
											{
												EnglishDraughtMoveAnimation englishDraughtMoveAnimation = moveAnimation as EnglishDraughtMoveAnimation;
												Sqs = englishDraughtMoveAnimation.Sqs.vs;
											}
											break;
										default:
											Debug.LogError ("unknown type: " + moveAnimation.getType () + "; " + this);
											break;
										}
									} else {
										// Debug.LogError ("moveAnimation null: " + this);
									}
								}
								// update board
								for (int y = 0; y < 8; y++) {
									for (int x = 0; x < 8; x++) {
										int square = x + 8 * y;
										byte piece = EnglishDraught.getPiece (Sqs, square);
										if (Common.isPiece (piece)) {
											bool needAdd = false;
											// Find PieceUI
											PieceUI.UIData pieceUIData = null;
											{
												// Find old
												{
													foreach (PieceUI.UIData check in oldPieces) {
														if (check.square.v == square) {
															pieceUIData = check;
															break;
														} else if (check.square.v < 0 || check.animationSquare == square) {
															pieceUIData = check;
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
													oldPieces.Remove (pieceUIData);
												}
											}
											// Update Property
											{
												pieceUIData.square.v = square;
												pieceUIData.piece.v = piece;
											}
											// add
											if (needAdd) {
												this.data.pieces.add (pieceUIData);
											}
										}
									}
								}
								// remove unused piece
								foreach (PieceUI.UIData oldPiece in oldPieces) {
									this.data.pieces.remove(oldPiece);
								}
							}
						} else {
							Debug.LogError ("not load full");
							dirty = true;
						}
					} else {
						Debug.LogError ("englishDraught null: " + this);
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

		public EnglishDraughtFenUI englishDraughtFenPrefab;
		public Transform englishDraughtFenContainer;

		public PieceUI piecePrefab;
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
					uiData.englishDraught.allAddCallBack (this);
					uiData.englishDraughtFen.allAddCallBack (this);
					uiData.pieces.allAddCallBack (this);
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
				if (data is EnglishDraught) {
					dirty = true;
					return;
				}
				if (data is EnglishDraughtFenUI.UIData) {
					EnglishDraughtFenUI.UIData englishDraughtFenUIData = data as EnglishDraughtFenUI.UIData;
					// UI
					{
						UIUtils.Instantiate (englishDraughtFenUIData, englishDraughtFenPrefab, englishDraughtFenContainer);
					}
					dirty = true;
					return;
				}
				if (data is PieceUI.UIData) {
					PieceUI.UIData pieceUIData = data as PieceUI.UIData;
					// UI
					{
						UIUtils.Instantiate (pieceUIData, piecePrefab, this.transform);
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
					uiData.englishDraught.allRemoveCallBack (this);
					uiData.englishDraughtFen.allRemoveCallBack (this);
					uiData.pieces.allRemoveCallBack (this);
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
				if (data is EnglishDraught) {
					return;
				}
				if (data is EnglishDraughtFenUI.UIData) {
					EnglishDraughtFenUI.UIData englishDraughtFenUIData = data as EnglishDraughtFenUI.UIData;
					// UI
					{
						englishDraughtFenUIData.removeCallBackAndDestroy (typeof(EnglishDraughtFenUI));
					}
					return;
				}
				if (data is PieceUI.UIData) {
					PieceUI.UIData pieceUIData = data as PieceUI.UIData;
					// UI
					{
						pieceUIData.removeCallBackAndDestroy (typeof(PieceUI));
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
				case UIData.Property.englishDraught:
					{
						ValueChangeUtils.replaceCallBack (this, syncs);
						dirty = true;
					}
					break;
				case UIData.Property.englishDraughtFen:
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
				default:
					Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
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
				if (wrapProperty.p is EnglishDraught) {
					switch ((EnglishDraught.Property)wrapProperty.n) {
					case EnglishDraught.Property.Sqs:
						dirty = true;
						break;
					case EnglishDraught.Property.C:
						break;
					case EnglishDraught.Property.nPSq:
						break;
					case EnglishDraught.Property.eval:
						break;
					case EnglishDraught.Property.nWhite:
						break;
					case EnglishDraught.Property.nBlack:
						break;
					case EnglishDraught.Property.SideToMove:
						break;
					case EnglishDraught.Property.extra:
						break;
					case EnglishDraught.Property.HashKey:
						break;
					case EnglishDraught.Property.ply:
						break;
					case EnglishDraught.Property.RepNum:
						break;
					case EnglishDraught.Property.maxPly:
						break;
					case EnglishDraught.Property.turn:
						break;
					default:
						Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
						break;
					}
					return;
				}
				if (wrapProperty.p is EnglishDraughtFenUI.UIData) {
					return;
				}
				if (wrapProperty.p is PieceUI.UIData) {
					return;
				}
			}
			Debug.LogError ("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
		}

		#endregion

	}
}