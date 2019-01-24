﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Xiangqi.UseRule
{
	public class ClickDestUI : UIBehavior<ClickDestUI.UIData>, IPointerDownHandler
	{

		#region UIData

		public class UIData : ShowUI.UIData.Sub
		{
			
			public VP<int> x;

			public VP<int> y;

			public LP<LegalMoveUI.UIData> legalMoves;

			#region keyEvent

			public VP<int> keyX;

			public VP<int> keyY;

			#endregion

			#region Constructor

			public enum Property
			{
				x,
				y,
				legalMoves,
				keyX,
				keyY
			}

			public UIData() : base()
			{
				this.x = new VP<int>(this, (byte)Property.x, 0);
				this.y = new VP<int>(this, (byte)Property.y, 0);
				this.legalMoves = new LP<LegalMoveUI.UIData>(this, (byte)Property.legalMoves);
				// keyEvent
				{
					this.keyX = new VP<int>(this, (byte)Property.keyX, -1);
					this.keyY = new VP<int>(this, (byte)Property.keyY, -1);
				}
			}

			#endregion

			public override Type getType ()
			{
				return Type.ClickDest;
			}

			public override bool processEvent (Event e)
			{
				bool isProcess = false;
				{
					// key
					if (!isProcess) {
						if (InputEvent.isEnter (e)) {
							// enter
							ClickDestUI clickDestUI = this.findCallBack<ClickDestUI> ();
							if (clickDestUI != null) {
								clickDestUI.onEnterKey ();
							} else {
								Debug.LogError ("clickDestUI null: " + this);
							}
							isProcess = true;
						} else if (InputEvent.isArrow (e)) {
							if (this.keyX.v >= 0 && this.keyX.v < 9
								&& this.keyY.v >= 0 && this.keyY.v < 10) {
								// find new key position
								int newKeyX = this.keyX.v;
								int newKeyY = this.keyY.v;
								{
									switch (e.keyCode) {
									case KeyCode.LeftArrow:
										newKeyX--;
										break;
									case KeyCode.RightArrow:
										newKeyX++;
										break;
									case KeyCode.UpArrow:
										newKeyY++;
										break;
									case KeyCode.DownArrow:
										newKeyY--;
										break;
									default:
										Debug.LogError ("unknown keyCode: " + e.keyCode);
										break;
									}
								}
								// set
								if (newKeyX >= 0 && newKeyX < 9
									&& newKeyY >= 0 && newKeyY < 10) {
									this.keyX.v = newKeyX;
									this.keyY.v = newKeyY;
								}
							} else {
								this.keyX.v = this.x.v;
								this.keyY.v = this.y.v;
							}
							isProcess = true;
						}
					}
				}
				return isProcess;
			}

		}

		#endregion

		#region Refresh

		public Image imgSelect;

		public GameObject keySelect;

		public override void refresh ()
		{
			if (dirty) {
				dirty = false;
				if (this.data != null) {
					// keySelect
					{
						if (keySelect != null) {
							if (this.data.keyX.v >= 0 && this.data.keyX.v < 9
								&& this.data.keyY.v >= 0 && this.data.keyY.v < 10) {
								keySelect.SetActive (true);
								keySelect.transform.localPosition = Common.convertXYToLocalPosition (this.data.keyX.v, this.data.keyY.v);
							} else {
								keySelect.SetActive (false);
							}
						} else {
							Debug.LogError ("keySelect null: " + this);
						}
					}
					// imgSelect
					{
						if (imgSelect != null) {
							// localPosition
							{
								int x = this.data.x.v;
								int y = this.data.y.v;
								imgSelect.transform.localPosition = new Vector3 (x - 4f, y - 4.5f, 0);
							}
							// TODO co the co rotate
							{
								// TODO Can hoan thien
							}
						} else {
							Debug.LogError ("imgSelect null: " + this);
						}
					}
					// Legal moves
					{
						List<LegalMoveUI.UIData> oldLegalMoves = new List<LegalMoveUI.UIData> ();
						// get oldLegalMoves
						oldLegalMoves.AddRange(this.data.legalMoves.vs);
						// Update
						{
							// Get legal move list
							List<LegalMove> legalMoves = new List<LegalMove>();
							{
								ShowUI.UIData show = this.data.findDataInParent<ShowUI.UIData> ();
								if (show != null) {
									for (int i = 0; i < show.legalMoves.vs.Count; i++) {
										LegalMove legalMove = show.legalMoves.vs [i];
										// check correct
										bool isCorrect = false;
										{
											Common.MoveStruct moveStruct = new Common.MoveStruct(legalMove.move.v);
											int srcX = this.data.x.v;
											int srcY = this.data.y.v;
											if (moveStruct.srcX == srcX && moveStruct.srcY == srcY) {
												// Debug.LogError ("correct move: " + legalMove + "; " + srcX + "; " + srcY);
												isCorrect = true;
											} else {
												// Debug.LogError ("not correct move: " + legalMove + "; " + srcX + "; " + srcY);
											}
										}
										if (isCorrect) {
											legalMoves.Add (legalMove);
										}
									}
								} else {
									Debug.LogError ("show null: " + this);
								}
							}
							// Make UI
							{
								for (int legalMoveIndex = 0; legalMoveIndex < legalMoves.Count; legalMoveIndex++) {
									LegalMove legalMove = legalMoves [legalMoveIndex];
									// Find LegalMoveUIData
									LegalMoveUI.UIData legalMoveUIData = null;
									{
										// Find old
										{
											for (int i = 0; i < oldLegalMoves.Count; i++) {
												LegalMoveUI.UIData check = oldLegalMoves [i];
												if (check.legalMove.v.data == legalMove) {
													legalMoveUIData = check;
													break;
												} else if (check.legalMove.v.data == null) {
													legalMoveUIData = check;
												}
											}
											if (legalMoveUIData != null) {
												oldLegalMoves.Remove (legalMoveUIData);
											}
										}
										// Make new
										if (legalMoveUIData == null) {
											legalMoveUIData = new LegalMoveUI.UIData ();
											{
												legalMoveUIData.uid = this.data.legalMoves.makeId ();
											}
											this.data.legalMoves.add (legalMoveUIData);
										}
									}
									// Update property
									if (legalMoveUIData != null) {
										// legalMove
										legalMoveUIData.legalMove.v = new ReferenceData<LegalMove> (legalMove);
									} else {
										Debug.LogError ("legalMoveUIData null: " + this);
									}
								}
							}
						}
						// Remove old
						foreach (LegalMoveUI.UIData legalMoveUIData in oldLegalMoves) {
							legalMoveUIData.legalMove.v = new ReferenceData<LegalMove> (null);
						}
					}
				} else {
					// Debug.LogError ("data null: " + this);
				}
			}
		}

		public override bool isShouldDisableUpdate ()
		{
			return false;
		}

		#endregion

		#region implement callBacks

		public LegalMoveUI legalMovePrefab;

		private ShowUI.UIData show = null;

		public override void onAddCallBack<T> (T data)
		{
			if (data is UIData) {
				UIData uiData = data as UIData;
				// Parent
				{
					DataUtils.addParentCallBack (uiData, this, ref this.show);
				}
				// Child
				{
					uiData.legalMoves.allAddCallBack (this);
				}
				dirty = true;
				return;
			}
			// Parent
			if (data is ShowUI.UIData) {
				dirty = true;
				return;
			}
			// Child
			if (data is LegalMoveUI.UIData) {
				LegalMoveUI.UIData legalMoveUIData = data as LegalMoveUI.UIData;
				// UI
				{
					UIUtils.Instantiate (legalMoveUIData, legalMovePrefab, this.transform);
				}
				dirty = true;
				return;
			}
			Debug.LogError ("Don't process: " + data + "; " + this);
		}

		public override void onRemoveCallBack<T> (T data, bool isHide)
		{
			if (data is UIData) {
				UIData uiData = data as UIData;
				// Parent
				{
					DataUtils.removeParentCallBack (uiData, this, ref this.show);
				}
				// Child
				{
					uiData.legalMoves.allRemoveCallBack (this);
				}
				this.setDataNull (uiData);
				return;
			}
			// Parent
			if (data is ShowUI.UIData) {
				return;
			}
			// Child
			if (data is LegalMoveUI.UIData) {
				LegalMoveUI.UIData legalMoveUIData = data as LegalMoveUI.UIData;
				// UI
				{
					legalMoveUIData.removeCallBackAndDestroy (typeof(LegalMoveUI));
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
			if (wrapProperty.p is UIData) {
				switch ((UIData.Property)wrapProperty.n) {
				case UIData.Property.x:
					dirty = true;
					break;
				case UIData.Property.y:
					dirty = true;
					break;
				case UIData.Property.legalMoves:
					{
						ValueChangeUtils.replaceCallBack (this, syncs);
						dirty = true;
					}
					break;
				case UIData.Property.keyX:
					dirty = true;
					break;
				case UIData.Property.keyY:
					dirty = true;
					break;
				default:
					Debug.LogError ("unknown wrapProperty: " + wrapProperty + "; " + this);
					break;
				}
				return;
			}
			// Parent
			if (wrapProperty.p is ShowUI.UIData) {
				switch ((ShowUI.UIData.Property)wrapProperty.n) {
				case ShowUI.UIData.Property.legalMoves:
					dirty = true;
					break;
				case ShowUI.UIData.Property.sub:
					break;
				default:
					Debug.LogError ("unknown wrapProperty: " + wrapProperty + "; " + this);
					break;
				}
				return;
			}
			// Child
			if (wrapProperty.p is LegalMoveUI.UIData) {
				return;
			}
			Debug.LogError ("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
		}

		#endregion

		private void clickDest(int x, int y)
		{
			if (this.data != null) {
				Xiangqi xiangqi = null;
				// Check isActive
				bool isActive = false;
				{
					UseRuleInputUI.UIData useRuleInputUIData = this.data.findDataInParent<UseRuleInputUI.UIData> ();
					if (useRuleInputUIData != null) {
						xiangqi = useRuleInputUIData.xiangqi.v.data;
						if (xiangqi != null) {
							if (Game.IsPlaying (xiangqi)) {
								isActive = true;
							}
						} else {
							Debug.LogError ("xiangqi null: " + this);
							return;
						}
					} else {
						Debug.LogError ("useRuleInputUIData null: " + this);
					}
				}
				if (isActive) {
					if (x >= 0 && x < 9 && y >= 0 && y < 10) {
						// Click the same already click
						bool isCurrentSelect = false;
						{
							if (this.data.x.v == x && this.data.y.v == y) {
								isCurrentSelect = true;
							}
						}
						if (isCurrentSelect) {
							// Chuyen ve ClickPieceUI
							ShowUI.UIData show = this.data.findDataInParent<ShowUI.UIData>();
							if (show != null) {
								ClickPieceUI.UIData clickPieceUIData = new ClickPieceUI.UIData ();
								{
									clickPieceUIData.uid = show.sub.makeId ();
								}
								show.sub.v = clickPieceUIData;
							} else {
								Debug.LogError ("show null: " + this);
							}
						} else {
							// select other piece in the same side
							bool isSelectOtherPiece = false;
							{
								int coordXY = Common.COORD_XY (x + 3, (9 - y) + 3);
								// find pc
								int pc = 0;
								{
									if (coordXY >= 0 && coordXY < xiangqi.ucpcSquares.vs.Count) {
										pc = xiangqi.ucpcSquares.vs [coordXY];
									} else {
										Debug.LogError ("error, coorXY: " + coordXY);
									}
									if (pc != 0) {
										if (pc < 32) {
											if (xiangqi.getPlayerIndex () == 0) {
												isSelectOtherPiece = true;
											}
										} else {
											if (xiangqi.getPlayerIndex () == 1) {
												isSelectOtherPiece = true;
											}
										}
									}
								}
							}
							if (isSelectOtherPiece) {
								this.data.x.v = x;
								this.data.y.v = y;
							} else {
								// Find move click
								LegalMove legalMove = null;
								{
									ShowUI.UIData show = this.data.findDataInParent<ShowUI.UIData> ();
									if (show != null) {
										int srcX = this.data.x.v;
										int srcY = this.data.y.v;
										for (int i = 0; i < show.legalMoves.vs.Count; i++) {
											LegalMove check = show.legalMoves.vs [i];
											Common.MoveStruct moveStruct = new Common.MoveStruct (check.move.v);
											if (moveStruct.srcX == srcX && moveStruct.srcY == srcY && moveStruct.destX == x && moveStruct.destY == y) {
												legalMove = check;
												break;
											}
										}
									} else {
										Debug.LogError ("show null: " + this);
									}
								}
								if (legalMove != null) {
									if (legalMove.repStatus.v==(int)Common.RepStatus.REP_WIN) {
										Debug.LogError ("click repeat move: " + legalMove + "; " + this);
									} else {
										ClientInput clientInput = InputUI.UIData.findClientInput(this.data);
										if (clientInput != null) {
											XiangqiMove xiangqiMove = new XiangqiMove();
											{
												xiangqiMove.move.v = legalMove.move.v;
											}
											clientInput.makeSend (xiangqiMove);
										} else {
											Debug.LogError ("clientInput null: " + this);
										}
									}
								} else {
									Debug.LogError ("click not dest position: " + this);
								}
							}
						}
					} else {
						Debug.LogError ("click outside board");
					}
				} else {
					Debug.LogError ("not active: " + this);
				}
			} else {
				Debug.LogError ("data null: " + this);
			}
		}

		public void onEnterKey()
		{
			if (this.data != null) {
				this.clickDest (this.data.keyX.v, this.data.keyY.v);
			} else {
				Debug.LogError ("data null: " + this);
			}
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			Debug.LogError ("OnPointerDown: " + eventData + "; " + this);
			Vector3 localPosition = transform.InverseTransformPoint (eventData.position);
			int x = Mathf.RoundToInt (localPosition.x + 4f);
			int y = Mathf.RoundToInt (localPosition.y + 4.5f);
			Debug.LogError ("localPosition: " + localPosition + ", " + x + ", " + y);
			this.clickDest (x, y);
		}

	}
}