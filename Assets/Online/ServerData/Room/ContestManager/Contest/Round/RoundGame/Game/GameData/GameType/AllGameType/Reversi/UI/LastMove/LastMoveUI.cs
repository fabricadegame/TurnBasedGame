﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Reversi.NoneRule;

namespace Reversi
{
	public class LastMoveUI : UIBehavior<LastMoveUI.UIData>
	{

		#region UIData

		public class UIData : Data
		{
			public VP<ReferenceData<GameData>> gameData;

			public VP<LastMoveSub> sub;

			#region Constructor

			public enum Property
			{
				gameData,
				sub
			}

			public UIData() : base()
			{
				this.gameData = new VP<ReferenceData<GameData>>(this, (byte)Property.gameData, new ReferenceData<GameData>(null));
				this.sub = new VP<LastMoveSub>(this, (byte)Property.sub, null);
			}

			#endregion
		}

		#endregion

		#region Refresh

		public Transform contentContainer;

		public override void refresh ()
		{
			if (dirty) {
				dirty = false;
				if (this.data != null) {
					// Find last move
					GameMove lastMove = LastMoveCheckChange<UIData>.getLastMove(this.data);
					// contentContainer
					{
						if (contentContainer != null) {
							contentContainer.gameObject.SetActive (lastMove != null);
						} else {
							Debug.LogError ("contentContainer null: " + this);
						}
					}
					// Process
					if (lastMove != null) {
						switch (lastMove.getType ()) {
						case GameMove.Type.ReversiMove:
							{
								ReversiMove reversiMove = lastMove as ReversiMove;
								// Find
								ReversiMoveUI.UIData reversiMoveUIData = this.data.sub.newOrOld<ReversiMoveUI.UIData>();
								{
									// move
									reversiMoveUIData.reversiMove.v = new ReferenceData<ReversiMove> (reversiMove);
									// isHint
									reversiMoveUIData.isHint.v = false;
								}
								this.data.sub.v = reversiMoveUIData;
							}
							break;
						case GameMove.Type.ReversiCustomSet:
							{
								ReversiCustomSet reversiCustomSet = lastMove as ReversiCustomSet;
								// Find
								ReversiCustomSetUI.UIData reversiCustomSetUIData = this.data.sub.newOrOld<ReversiCustomSetUI.UIData>();
								{
									// move
									reversiCustomSetUIData.reversiCustomSet.v = new ReferenceData<ReversiCustomSet> (reversiCustomSet);
									// isHint
									reversiCustomSetUIData.isHint.v = false;
								}
								this.data.sub.v = reversiCustomSetUIData;
							}
							break;
						case GameMove.Type.ReversiCustomMove:
							{
								ReversiCustomMove reversiCustomMove = lastMove as ReversiCustomMove;
								// Find
								ReversiCustomMoveUI.UIData reversiCustomMoveUIData = this.data.sub.newOrOld<ReversiCustomMoveUI.UIData>();
								{
									// move
									reversiCustomMoveUIData.reversiCustomMove.v = new ReferenceData<ReversiCustomMove> (reversiCustomMove);
									// isHint
									reversiCustomMoveUIData.isHint.v = false;
								}
								this.data.sub.v = reversiCustomMoveUIData;
							}
							break;
						case GameMove.Type.None:
							this.data.sub.v = null;
							break;
						default:
							Debug.LogError ("unknown lastMove: " + lastMove + ";" + this);
							this.data.sub.v = null;
							break;
						}
					} else {
						// Debug.LogError ("lastMove null: " + this);
						this.data.sub.v = null;
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

		public ReversiMoveUI reversiMovePrefab;
		public ReversiCustomSetUI reversiCustomSetPrefab;
		public ReversiCustomMoveUI reversiCustomMovePrefab;

		private LastMoveCheckChange<UIData> lastMoveCheckChange = new LastMoveCheckChange<UIData> ();

		public override void onAddCallBack<T> (T data)
		{
			if (data is UIData) {
				UIData uiData = data as UIData;
				// CheckChange
				{
					lastMoveCheckChange.addCallBack (this);
					lastMoveCheckChange.setData (uiData);
				}
				// Child
				{
					uiData.sub.allAddCallBack (this);
				}
				dirty = true;
				return;
			}
			// CheckChange
			if (data is LastMoveCheckChange<UIData>) {
				dirty = true;
				return;
			}
			// Child
			if (data is LastMoveSub) {
				LastMoveSub lastMoveSub = data as LastMoveSub;
				// UI
				{
					switch (lastMoveSub.getType ()) {
					case GameMove.Type.ReversiMove:
						{
							ReversiMoveUI.UIData reversiMoveUIData = lastMoveSub as ReversiMoveUI.UIData;
							UIUtils.Instantiate (reversiMoveUIData, reversiMovePrefab, this.transform);
						}
						break;
					case GameMove.Type.ReversiCustomSet:
						{
							ReversiCustomSetUI.UIData reversiCustomSetUIData = lastMoveSub as ReversiCustomSetUI.UIData;
							UIUtils.Instantiate (reversiCustomSetUIData, reversiCustomSetPrefab, this.transform);
						}
						break;
					case GameMove.Type.ReversiCustomMove:
						{
							ReversiCustomMoveUI.UIData reversiCustomMoveUIData = lastMoveSub as ReversiCustomMoveUI.UIData;
							UIUtils.Instantiate (reversiCustomMoveUIData, reversiCustomMovePrefab, this.transform);
						}
						break;
					default:
						Debug.LogError ("unknown type: " + lastMoveSub.getType () + "; " + this);
						break;
					}
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
				// CheckChange
				{
					lastMoveCheckChange.removeCallBack (this);
					lastMoveCheckChange.setData (null);
				}
				// Child
				{
					uiData.sub.allRemoveCallBack (this);
				}
				this.setDataNull (uiData);
				return;
			}
			// CheckChange
			if (data is LastMoveCheckChange<UIData>) {
				return;
			}
			// Child
			if (data is LastMoveSub) {
				LastMoveSub lastMoveSub = data as LastMoveSub;
				// UI
				{
					switch (lastMoveSub.getType ()) {
					case GameMove.Type.ReversiMove:
						{
							ReversiMoveUI.UIData reversiMoveUIData = lastMoveSub as ReversiMoveUI.UIData;
							reversiMoveUIData.removeCallBackAndDestroy (typeof(ReversiMoveUI));
						}
						break;
					case GameMove.Type.ReversiCustomSet:
						{
							ReversiCustomSetUI.UIData reversiCustomSetUIData = lastMoveSub as ReversiCustomSetUI.UIData;
							reversiCustomSetUIData.removeCallBackAndDestroy (typeof(ReversiCustomSetUI));
						}
						break;
					case GameMove.Type.ReversiCustomMove:
						{
							ReversiCustomMoveUI.UIData reversiCustomMoveUIData = lastMoveSub as ReversiCustomMoveUI.UIData;
							reversiCustomMoveUIData.removeCallBackAndDestroy (typeof(ReversiCustomMoveUI));
						}
						break;
					default:
						Debug.LogError ("unknown type: " + lastMoveSub.getType () + "; " + this);
						break;
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
			if (wrapProperty.p is UIData) {
				switch ((UIData.Property)wrapProperty.n) {
				case UIData.Property.gameData:
					dirty = true;
					break;
				case UIData.Property.sub:
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
			// CheckChange
			if (wrapProperty.p is LastMoveCheckChange<UIData>) {
				dirty = true;
				return;
			}
			// Child
			if (wrapProperty.p is LastMoveSub) {
				return;
			}
			Debug.LogError ("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
		}

		#endregion
	}
}