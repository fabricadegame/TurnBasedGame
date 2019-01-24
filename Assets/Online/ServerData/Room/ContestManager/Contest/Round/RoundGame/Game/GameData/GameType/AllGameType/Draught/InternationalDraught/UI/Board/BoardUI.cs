﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace InternationalDraught
{
	public class BoardUI : UIBehavior<BoardUI.UIData>
	{

		#region UIData

		public class UIData : Data
		{
			
			public VP<ReferenceData<InternationalDraught>> internationalDraught;

			public VP<InternationalDraughtFenUI.UIData> internationalDraughtFen;

			public LP<PieceUI.UIData> pieces;

			#region Constructor

			public enum Property
			{
				internationalDraught,
				internationalDraughtFen,
				pieces
			}

			public UIData() : base()
			{
				this.internationalDraught = new VP<ReferenceData<InternationalDraught>>(this, (byte)Property.internationalDraught, new ReferenceData<InternationalDraught>(null));
				this.internationalDraughtFen = new VP<InternationalDraughtFenUI.UIData>(this, (byte)Property.internationalDraughtFen, new InternationalDraughtFenUI.UIData());
				this.pieces = new LP<PieceUI.UIData>(this, (byte)Property.pieces);
			}

			#endregion

		}

		#endregion

		#region Refresh

		public Text tvVariant;

		public override void refresh ()
		{
			if (dirty) {
				dirty = false;
				if (this.data != null) {
					InternationalDraught internationalDraught = this.data.internationalDraught.v.data;
					if (internationalDraught != null) {
						// check load full
						bool isLoadFull = true;
						{
							// internationalDraught
							if (isLoadFull) {
								isLoadFull = internationalDraught.isLoadFull ();
							}
							// animation
							if (isLoadFull) {
								isLoadFull = AnimationManager.IsLoadFull (this.data);
							}
						}
						// process
						if (isLoadFull) {
							// Debug.Log ("Fen: " + internationalDraught.getFen () + "; " + this);
							// internationalDraughtFen
							{
								InternationalDraughtFenUI.UIData internationalDraughtFenUIData = this.data.internationalDraughtFen.v;
								if (internationalDraughtFenUIData != null) {
									internationalDraughtFenUIData.internationalDraught.v = new ReferenceData<InternationalDraught> (internationalDraught);
								} else {
									Debug.LogError ("internationalDraughtFenUIData null: " + this);
								}
							}
							// pieces
							{
								// get old
								List<PieceUI.UIData> oldPieces = new List<PieceUI.UIData> ();
								{
									oldPieces.AddRange (this.data.pieces.vs);
								}
								// Find pos
								Pos pos = internationalDraught.getCurrentPos ();
								{
									MoveAnimation moveAnimation = GameDataBoardUI.UIData.getCurrentMoveAnimation (this.data);
									if (moveAnimation != null) {
										switch (moveAnimation.getType ()) {
										case GameMove.Type.InternationalDraughtMove:
											{
												InternationalDraughtMoveAnimation internationalDraughtMoveAnimation = moveAnimation as InternationalDraughtMoveAnimation;
												pos = internationalDraughtMoveAnimation.pos.v;
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
								if (pos != null) {
									// lastMove
									// int from = -1;
									// int dest = -1;
									{
										if (internationalDraught.lastMove.v > 0) {
											System.UInt64 mv = internationalDraught.lastMove.v;
											// from = InternationalDraughtMove.from (mv);
											// dest = InternationalDraughtMove.to (mv);
											// Debug.Log ("lastMove: " + from + "; " + dest + "; " + internationalDraught.lastMove.v);
										} else {
											Debug.Log ("don't have last move");
										}
									}
									// board
									for (int y = 0; y < Common.Line_Size; y++) {// Square.Rank_Size
										for (int x = 0; x < Common.Line_Size; x++) {// Square.File_Size
											if (Common.square_is_dark (x, y)) {
												int sq = Common.square_make (x, y);
												// Check capture square
												bool isLastCaptureSquare = false;
												{
													if (internationalDraught.captureSize.v >= 0 && internationalDraught.captureSize.v <= 20) {
														for (int i = 0; i < internationalDraught.captureSize.v; i++) {
															if (i >= 0 && i < internationalDraught.captureSquares.vs.Count) {
																if (internationalDraught.captureSquares.vs [i] == sq) {
																	isLastCaptureSquare = true;
																	break;
																}
															} else {
																Debug.LogError ("internationalDraught captureSquares error: " + i);
															}
														}
													} else {
														Debug.LogError ("error, captureSize wrong: " + internationalDraught.captureSize.v);
													}
												}
												// print piece
												// Debug.Log("pos piece_side: "+pos.piece_side(sq)+"; "+sq);
												int pieceSide = pos.piece_side (sq);
												// Check need piece
												bool needPiece = false;
												{
													switch (pieceSide) {
													case (int)Common.Piece_Side.White_Man:
													case (int)Common.Piece_Side.Black_Man:
													case (int)Common.Piece_Side.White_King:
													case (int)Common.Piece_Side.Black_King:
														{
															needPiece = true;
														}
														break;
													case (int)Common.Piece_Side.Empty:
														if (isLastCaptureSquare) {
															needPiece = true;
														}
														break;
													default:
														Debug.LogError ("error, unknown piece_side");
														break;
													}
												}
												if (needPiece) {
													bool needAdd = false;
													// Find piece
													PieceUI.UIData pieceUI = null;
													{
														// find old
														{
															for (int i = 0; i < oldPieces.Count; i++) {
																PieceUI.UIData oldPiece = oldPieces [i];
																if (oldPiece.square.v == sq) {
																	pieceUI = oldPiece;
																	break;
																} else if (oldPiece.square.v < 0 || oldPiece.animationSquare == sq) {
																	pieceUI = oldPiece;
																}
															}
														}
														// make new
														if (pieceUI == null) {
															// Debug.LogError ("make new piece: " + sq);
															pieceUI = new PieceUI.UIData ();
															{
																pieceUI.uid = this.data.pieces.makeId ();
															}
															needAdd = true;
														} else {
															oldPieces.Remove (pieceUI);
														}
													}
													// Update
													{
														// square
														pieceUI.square.v = sq;
														// pieceSide
														pieceUI.pieceSide.v = pieceSide;
														// isLastCapture
														pieceUI.isLastCapture.v = isLastCaptureSquare;
													}
													// Add
													if (needAdd) {
														this.data.pieces.add (pieceUI);
													}
												}
											}
										}
									}
								} else {
									Debug.LogError ("pos null: " + this);
								}
								// remove unused piece
								{
									foreach (PieceUI.UIData oldPiece in oldPieces) {
										oldPiece.square.v = -1;
										// this.data.pieces.remove (oldPiece);
									}
								}
							}
							// Variant
							{
								if (tvVariant != null) {
									// find variant
									Common.Variant_Type variant = Common.Variant_Type.Normal;
									{
										Var var = internationalDraught.var.v;
										if (var != null) {
											variant = (Common.Variant_Type)var.Variant.v;
										} else {
											Debug.LogError ("var null: " + this);
										}
									}
									// process
									switch (variant) {
									case Common.Variant_Type.Normal:
										tvVariant.text = "Normal";
										break;
									case Common.Variant_Type.BT:
										tvVariant.text = "BreakThrough";
										break;
									case Common.Variant_Type.Killer:
										tvVariant.text = "Killer";
										break;
									default:
										Debug.LogError ("unknown variant: " + variant + "; " + this);
										tvVariant.text = "";
										break;
									}
								} else {
									Debug.LogError ("tvVariant null: " + this);
								}
							}
						} else {
							Debug.LogError ("not load full");
							dirty = true;
						}
					} else {
						Debug.LogError ("internationalDraught null: " + this);
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

		public InternationalDraughtFenUI internationalDraughtFenPrefab;
		public Transform internationalDraughtFenContainer;

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
					uiData.internationalDraught.allAddCallBack (this);
					uiData.internationalDraughtFen.allAddCallBack (this);
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
				// InternationalDraught
				{
					if (data is InternationalDraught) {
						InternationalDraught internationalDraught = data as InternationalDraught;
						// Child
						{
							internationalDraught.node.allAddCallBack (this);
							internationalDraught.var.allAddCallBack (this);
						}
						dirty = true;
						return;
					}
					// Node
					{
						if (data is Node) {
							Node node = data as Node;
							// Child
							{
								node.p_pos.allAddCallBack (this);
							}
							dirty = true;
							return;
						}
						if (data is Pos) {
							dirty = true;
							return;
						}
					}
					// Var
					if (data is Var) {
						dirty = true;
						return;
					}
				}
				if (data is InternationalDraughtFenUI.UIData) {
					InternationalDraughtFenUI.UIData internationalDraughtFenUIData = data as InternationalDraughtFenUI.UIData;
					// UI
					{
						UIUtils.Instantiate (internationalDraughtFenUIData, internationalDraughtFenPrefab, internationalDraughtFenContainer);
					}
					dirty = true;
					return;
				}
				// pieceUIData
				if (data is PieceUI.UIData) {
					PieceUI.UIData subUIData = data as PieceUI.UIData;
					// UI
					{
						UIUtils.Instantiate (subUIData, piecePrefab, this.transform);
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
					uiData.internationalDraught.allRemoveCallBack (this);
					uiData.internationalDraughtFen.allRemoveCallBack (this);
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
				// InternationalDraught
				{
					if (data is InternationalDraught) {
						InternationalDraught internationalDraught = data as InternationalDraught;
						// Child
						{
							internationalDraught.node.allRemoveCallBack (this);
							internationalDraught.var.allRemoveCallBack (this);
						}
						return;
					}
					// Node
					{
						if (data is Node) {
							Node node = data as Node;
							// Child
							{
								node.p_pos.allRemoveCallBack (this);
							}
							return;
						}
						if (data is Pos) {
							return;
						}
					}
					// Var
					if (data is Var) {
						return;
					}
				}
				if (data is InternationalDraughtFenUI.UIData) {
					InternationalDraughtFenUI.UIData internationalDraughtFenUIData = data as InternationalDraughtFenUI.UIData;
					// UI
					{
						internationalDraughtFenUIData.removeCallBackAndDestroy (typeof(InternationalDraughtFenUI));
					}
					return;
				}
				// pieceUIData
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
				case UIData.Property.internationalDraught:
					{
						ValueChangeUtils.replaceCallBack (this, syncs);
						dirty = true;
					}
					break;
				case UIData.Property.internationalDraughtFen:
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
				// InternationalDraught
				{
					if (wrapProperty.p is InternationalDraught) {
						switch ((InternationalDraught.Property)wrapProperty.n) {
						case InternationalDraught.Property.node:
							{
								ValueChangeUtils.replaceCallBack (this, syncs);
								dirty = true;
							}
							break;
						case InternationalDraught.Property.var:
							{
								ValueChangeUtils.replaceCallBack (this, syncs);
								dirty = true;
							}
							break;
						case InternationalDraught.Property.lastMove:
							dirty = true;
							break;
						case InternationalDraught.Property.ply:
							break;
						case InternationalDraught.Property.captureSize:
							dirty = true;
							break;
						case InternationalDraught.Property.captureSquares:
							dirty = true;
							break;
						default:
							Debug.LogError ("unknown wrapProperty: " + wrapProperty + "; " + this);
							break;
						}
						return;
					}
					// Node
					{
						if (wrapProperty.p is Node) {
							switch ((Node.Property)wrapProperty.n) {
							case Node.Property.p_pos:
								{
									ValueChangeUtils.replaceCallBack (this, syncs);
									dirty = true;
								}
								break;
							case Node.Property.p_ply:
								break;
							default:
								Debug.LogError ("unknown wrapProperty: " + wrapProperty + "; " + this);
								break;
							}
							return;
						}
						if (wrapProperty.p is Pos) {
							switch ((Pos.Property)wrapProperty.n) {
							case Pos.Property.p_piece:
								dirty = true;
								break;
							case Pos.Property.p_side:
								dirty = true;
								break;
							case Pos.Property.p_all:
								dirty = true;
								break;
							case Pos.Property.p_turn:
								dirty = true;
								break;
							default:
								Debug.LogError ("unknown wrapProperty: " + wrapProperty + "; " + this);
								break;
							}
							return;
						}
					}
					// Var
					if (wrapProperty.p is Var) {
						switch ((Var.Property)wrapProperty.n) {
						case Var.Property.Variant:
							dirty = true;
							break;
						case Var.Property.Book:
							break;
						case Var.Property.Book_Ply:
							break;
						case Var.Property.Book_Margin:
							break;
						case Var.Property.Ponder:
							break;
						case Var.Property.SMP:
							break;
						case Var.Property.SMP_Threads:
							break;
						case Var.Property.TT_Size:
							break;
						case Var.Property.BB:
							break;
						case Var.Property.BB_Size:
							break;
						case Var.Property.pickBestMove:
							break;
						default:
							Debug.LogError ("unknown wrapProperty: " + wrapProperty + "; " + this);
							break;
						}
						return;
					}
				}
				if (wrapProperty.p is InternationalDraughtFenUI.UIData) {
					return;
				}
				// PieceUIData
				if (wrapProperty.p is PieceUI.UIData) {
					return;
				}
			}
			Debug.LogError ("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
		}

		#endregion

	}
}