﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace Shogi.NoneRule
{
	public class ClickNoneUI : UIBehavior<ClickNoneUI.UIData>, IPointerDownHandler
	{

		#region UIData

		public class UIData : NoneRuleInputUI.UIData.Sub
		{

			public VP<int> keyX;

			public VP<int> keyY;

			#region Constructor

			public enum Property
			{
				keyX,
				keyY
			}

			public UIData() : base()
			{
				this.keyX = new VP<int>(this, (byte)Property.keyX, -1);
				this.keyY = new VP<int>(this, (byte)Property.keyY, -1);
			}

			#endregion

			public override Type getType ()
			{
				return Type.ClickNone;
			}

			public override bool processEvent (Event e)
			{
				bool isProcess = false;
				{
					if (InputEvent.isEnter (e)) {
						// enter
						ClickNoneUI clickNoneUI = this.findCallBack<ClickNoneUI> ();
						if (clickNoneUI != null) {
							clickNoneUI.onEnterKey ();
						} else {
							Debug.LogError ("clickNoneUI null: " + this);
						}
						isProcess = true;
					} else if (InputEvent.isArrow (e)) {
						if (this.keyX.v >= 0 && this.keyX.v < 9
							&& this.keyY.v >= 0 && this.keyY.v < 9) {
							// find new key position
							int newKeyX = this.keyX.v;
							int newKeyY = this.keyY.v;
							{
								switch (e.keyCode) {
								case KeyCode.LeftArrow:
									newKeyX++;
									break;
								case KeyCode.RightArrow:
									newKeyX--;
									break;
								case KeyCode.UpArrow:
									newKeyY--;
									break;
								case KeyCode.DownArrow:
									newKeyY++;
									break;
								default:
									Debug.LogError ("unknown keyCode: " + e.keyCode);
									break;
								}
							}
							// set
							if (newKeyX >= 0 && newKeyX < 9
								&& newKeyY >= 0 && newKeyY < 9) {
								this.keyX.v = newKeyX;
								this.keyY.v = newKeyY;
							}
						} else {
							// bring to lastMove
							int lastKeyX = 4;
							int lastKeyY = 4;
							{
								// find gameMove
								GameMove gameMove = null;
								{
									// Find gameData
									GameData gameData = null;
									{
										GameDataUI.UIData gameDataUIData = this.findDataInParent<GameDataUI.UIData> ();
										if (gameDataUIData != null) {
											gameData = gameDataUIData.gameData.v.data;
										} else {
											Debug.LogError ("gameDataUIData null: ");
										}
									}
									// Process
									if (gameData != null) {
										LastMove lastMove = gameData.lastMove.v;
										if (lastMove != null) {
											gameMove = lastMove.gameMove.v;
										} else {
											Debug.LogError ("lastMove null: " + this);
										}
									} else {
										// Debug.LogError ("gameData null: " + data);
									}
								}
								// process
								if (gameMove != null) {
									switch (gameMove.getType ()) {
									case GameMove.Type.ShogiMove:
										{
											ShogiMove shogiMove = gameMove as ShogiMove;
											Common.Square square = shogiMove.to ();
											lastKeyX = (int)Common.makeFile (square);
											lastKeyY = (int)Common.makeRank (square);
										}
										break;
									case GameMove.Type.ShogiCustomSet:
										{
											NoneRule.ShogiCustomSet shogiCustomSet = gameMove as NoneRule.ShogiCustomSet;
											Common.Square square = shogiCustomSet.square.v;
											lastKeyX = (int)Common.makeFile (square);
											lastKeyY = (int)Common.makeRank (square);
										}
										break;
									case GameMove.Type.ShogiCustomMove:
										{
											NoneRule.ShogiCustomMove shogiCustomMove = gameMove as NoneRule.ShogiCustomMove;
											Common.Square square = shogiCustomMove.dest.v;
											lastKeyX = (int)Common.makeFile (square);
											lastKeyY = (int)Common.makeRank (square);
										}
										break;
									case GameMove.Type.None:
										break;
									default:
										Debug.LogError ("unknown type: " + gameMove.getType () + "; " + this);
										break;
									}
								} else {
									Debug.LogError ("gameMove null: " + this);
								}
							}
							// set
							this.keyX.v = lastKeyX;
							this.keyY.v = lastKeyY;
						}
						isProcess = true;
					}
				}
				return isProcess;
			}

		}

		#endregion

		#region Refresh

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
								&& this.data.keyY.v >= 0 && this.data.keyY.v < 9) {
								keySelect.SetActive (true);
								keySelect.transform.localPosition = Common.convertSquareToLocalPosition (Common.makeSquare ((Common.File)this.data.keyX.v, (Common.Rank)this.data.keyY.v));
							} else {
								keySelect.SetActive (false);
							}
						} else {
							Debug.LogError ("keySelect null: " + this);
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
				case UIData.Property.keyX:
					dirty = true;
					break;
				case UIData.Property.keyY:
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

		#region IPointerDownHandler

		private void clickNone(int x, int y)
		{
			if (this.data != null) {
				Shogi shogi = null;
				// Check isActive
				bool isActive = false;
				{
					NoneRuleInputUI.UIData noneRuleInputUIData = this.data.findDataInParent<NoneRuleInputUI.UIData> ();
					if (noneRuleInputUIData != null) {
						shogi = noneRuleInputUIData.shogi.v.data;
						if (shogi != null) {
							if (Game.IsPlaying (shogi)) {
								isActive = true;
							}
						} else {
							Debug.LogError ("shogi null: " + this);
							return;
						}
					} else {
						Debug.LogError ("useRuleInputUIData null: " + this);
					}
				}
				if (isActive) {
					if (x >= 0 && x < 9 && y >= 0 && y < 9) {
						Common.Square square = Common.makeSquare ((Common.File)x, (Common.Rank)y);
						// change to click pos
						NoneRuleInputUI.UIData noneRuleInputUIData = this.data.findDataInParent<NoneRuleInputUI.UIData> ();
						if (noneRuleInputUIData != null) {
							ClickPosUI.UIData clickPosUIData = new ClickPosUI.UIData ();
							{
								clickPosUIData.uid = noneRuleInputUIData.sub.makeId ();
								clickPosUIData.square.v = square;
							}
							noneRuleInputUIData.sub.v = clickPosUIData;
						} else {
							Debug.LogError ("noneRuleInputUIData null: " + this);
						}
					} else {
						Debug.LogError ("click outside board: " + this);
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
				this.clickNone (this.data.keyX.v, this.data.keyY.v);
			} else {
				Debug.LogError ("data null: " + this);
			}
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			// Debug.LogError ("OnPointerDown: " + eventData);
			Vector3 localPosition = transform.InverseTransformPoint (eventData.position);
			int x = 0;
			int y = 0;
			Common.convertLocalPositionToXY (localPosition, out x, out y);
			Debug.LogError ("localPosition: " + localPosition + ", " + x + ", " + y);
			this.clickNone (x, y);
		}

		#endregion

	}
}