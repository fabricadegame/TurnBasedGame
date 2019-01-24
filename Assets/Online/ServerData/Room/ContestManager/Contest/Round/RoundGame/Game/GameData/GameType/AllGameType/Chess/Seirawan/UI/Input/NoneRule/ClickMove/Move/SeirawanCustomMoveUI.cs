﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using System.Collections;
using System.Collections.Generic;

namespace Seirawan.NoneRule
{
	public class SeirawanCustomMoveUI : UIBehavior<SeirawanCustomMoveUI.UIData>
	{

		#region UIData

		public class UIData : LastMoveSub
		{

			public VP<ReferenceData<SeirawanCustomMove>> seirawanCustomMove;

			public VP<bool> isHint;

			#region Constructor

			public enum Property
			{
				seirawanCustomMove,
				isHint
			}

			public UIData() : base()
			{
				this.seirawanCustomMove = new VP<ReferenceData<SeirawanCustomMove>>(this, (byte)Property.seirawanCustomMove, new ReferenceData<SeirawanCustomMove>(null));
				this.isHint = new VP<bool>(this, (byte)Property.isHint, false);
			}

			#endregion

			public override GameMove.Type getType ()
			{
				return GameMove.Type.SeirawanCustomMove;
			}

		}

		#endregion

		#region Refresh

		public UILineRenderer lineRendererFrom;
		public UILineRenderer lineRendererDest;

		public override void refresh ()
		{
			if (dirty) {
				dirty = false;
				if (this.data != null) {
					SeirawanCustomMove seirawanCustomMove = this.data.seirawanCustomMove.v.data;
					if (seirawanCustomMove != null) {
						// from
						if (lineRendererFrom != null) {
							// point
							{
								Vector2[] points = new Vector2[5];
								{
									Vector3 localPos = Common.convertXYToLocalPosition (seirawanCustomMove.fromX.v, seirawanCustomMove.fromY.v);
									points [0] = new Vector2 (localPos.x + 0.5f, localPos.y + 0.5f);
									points [1] = new Vector2 (localPos.x - 0.5f, localPos.y + 0.5f);
									points [2] = new Vector2 (localPos.x - 0.5f, localPos.y - 0.5f);
									points [3] = new Vector2 (localPos.x + 0.5f, localPos.y - 0.5f);
									points [4] = new Vector2 (localPos.x + 0.5f, localPos.y + 0.5f);
								}
								lineRendererFrom.Points = points;
							}
							// color
							{
								if (this.data.isHint.v) {
									lineRendererFrom.color = new Color (1, 0, 0, 0.5f);
								} else {
									lineRendererFrom.color = new Color (1, 0, 0, 1f);
								}
							}
						} else {
							Debug.LogError ("lineRendererFrom null: " + this);
						}
						// dest
						if (lineRendererDest != null) {
							// point
							{
								Vector2[] points = new Vector2[5];
								{
									Vector3 localPos = Common.convertXYToLocalPosition (seirawanCustomMove.destX.v, seirawanCustomMove.destY.v);
									points [0] = new Vector2 (localPos.x + 0.5f, localPos.y + 0.5f);
									points [1] = new Vector2 (localPos.x - 0.5f, localPos.y + 0.5f);
									points [2] = new Vector2 (localPos.x - 0.5f, localPos.y - 0.5f);
									points [3] = new Vector2 (localPos.x + 0.5f, localPos.y - 0.5f);
									points [4] = new Vector2 (localPos.x + 0.5f, localPos.y + 0.5f);
								}
								lineRendererDest.Points = points;
							}
							// color
							{
								if (this.data.isHint.v) {
									lineRendererDest.color = new Color (0, 1, 0, 0.5f);
								} else {
									lineRendererDest.color = new Color (0, 1, 0, 1f);
								}
							}
						} else {
							Debug.LogError ("lineRendererDest null: " + this);
						}
					} else {
						// Debug.LogError ("seirawanCustomMove null: " + this);
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

		public override void onAddCallBack<T> (T data)
		{
			if (data is UIData) {
				UIData uiData = data as UIData;
				// Child
				{
					uiData.seirawanCustomMove.allAddCallBack (this);
				}
				dirty = true;
				return;
			}
			// Child
			if (data is SeirawanCustomMove) {
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
					uiData.seirawanCustomMove.allRemoveCallBack (this);
				}
				this.setDataNull (uiData);
				return;
			}
			// Child
			if (data is SeirawanCustomMove) {
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
				case UIData.Property.seirawanCustomMove:
					{
						ValueChangeUtils.replaceCallBack (this, syncs);
						dirty = true;
					}
					break;
				case UIData.Property.isHint:
					dirty = true;
					break;
				default:
					Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
					break;
				}
				return;
			}
			// Child
			if (wrapProperty.p is SeirawanCustomMove) {
				switch ((SeirawanCustomMove.Property)wrapProperty.n) {
				case SeirawanCustomMove.Property.fromX:
					dirty = true;
					break;
				case SeirawanCustomMove.Property.fromY:
					dirty = true;
					break;
				case SeirawanCustomMove.Property.destX:
					dirty = true;
					break;
				case SeirawanCustomMove.Property.destY:
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

	}
}