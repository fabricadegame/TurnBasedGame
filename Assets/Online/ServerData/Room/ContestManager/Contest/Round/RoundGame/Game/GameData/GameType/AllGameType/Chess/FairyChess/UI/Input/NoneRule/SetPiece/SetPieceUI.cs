﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace FairyChess.NoneRule
{
	public class SetPieceUI : UIBehavior<SetPieceUI.UIData>
	{

		#region UIData

		public class UIData : NoneRuleInputUI.UIData.Sub
		{

			public VP<int> x;

			public VP<int> y;

			public VP<ChoosePieceAdapter.UIData> choosePieceAdapter;

			#region Constructor

			public enum Property
			{
				x,
				y,
				choosePieceAdapter
			}

			public UIData() : base()
			{
				this.x = new VP<int>(this, (byte)Property.x, 0);
				this.y = new VP<int>(this, (byte)Property.y, 0);
				this.choosePieceAdapter = new VP<ChoosePieceAdapter.UIData>(this, (byte)Property.choosePieceAdapter, new ChoosePieceAdapter.UIData());
			}

			#endregion

			public override Type getType ()
			{
				return Type.SetPiece;
			}

			public override bool processEvent (Event e)
			{
				bool isProcess = false;
				{
					// back
					if (!isProcess) {
						if (InputEvent.isBackEvent (e)) {
							SetPieceUI setPieceUI = this.findCallBack<SetPieceUI> ();
							if (setPieceUI != null) {
								setPieceUI.onClickBtnBtnBack ();
							} else {
								Debug.LogError ("setPieceUI null: " + this);
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
		public Transform contentContainer;

		public override void refresh ()
		{
			if (dirty) {
				dirty = false;
				if (this.data != null) {
					// imgSelect
					{
						if (imgSelect != null) {
							// position
							imgSelect.transform.localPosition = Common.convertXYToLocalPosition (this.data.x.v, this.data.y.v);
							// Scale
							{
								int playerView = GameDataBoardUI.UIData.getPlayerView (this.data);
								imgSelect.transform.localScale = (playerView == 0 ? new Vector3 (1f, 1f, 1f) : new Vector3 (1f, -1f, 1f));
							}
						} else {
							Debug.LogError ("imgSelect null: " + this);
						}
					}
					// Scale
					{
						if (contentContainer != null) {
							int playerView = GameDataBoardUI.UIData.getPlayerView (this.data);
							float scale = 0.015f;
							contentContainer.localScale = (playerView == 0 ? new Vector3 (scale, scale, 1f) : new Vector3 (scale, -scale, 1f));
						} else {
							Debug.LogError ("contentContainer null: " + this);
						}
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

		private GameDataBoardCheckPerspectiveChange<UIData> checkPerspectiveChange = new GameDataBoardCheckPerspectiveChange<UIData> ();

		public ChoosePieceAdapter choosePieceAdapterPrefab;
		public Transform choosePieceAdapterContainer;

		public override void onAddCallBack<T> (T data)
		{
			if (data is UIData) {
				UIData uiData = data as UIData;
				// CheckChange
				{
					checkPerspectiveChange.addCallBack (this);
					checkPerspectiveChange.setData (uiData);
				}
				// Child
				{
					uiData.choosePieceAdapter.allAddCallBack (this);
				}
				dirty = true;
				return;
			}
			// CheckChange
			if (data is GameDataBoardCheckPerspectiveChange<UIData>) {
				dirty = true;
				return;
			}
			// Child
			if (data is ChoosePieceAdapter.UIData) {
				ChoosePieceAdapter.UIData choosePieceAdapterUIData = data as ChoosePieceAdapter.UIData;
				// UI
				{
					UIUtils.Instantiate (choosePieceAdapterUIData, choosePieceAdapterPrefab, choosePieceAdapterContainer);
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
					checkPerspectiveChange.removeCallBack (this);
					checkPerspectiveChange.setData (null);
				}
				// Child
				{
					uiData.choosePieceAdapter.allRemoveCallBack (this);
				}
				this.setDataNull (uiData);
				return;
			}
			// CheckChange
			if (data is GameDataBoardCheckPerspectiveChange<UIData>) {
				return;
			}
			// Child
			if (data is ChoosePieceAdapter.UIData) {
				ChoosePieceAdapter.UIData choosePieceAdapterUIData = data as ChoosePieceAdapter.UIData;
				// UI
				{
					choosePieceAdapterUIData.removeCallBackAndDestroy (typeof(ChoosePieceAdapter));
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
					break;
				case UIData.Property.y:
					break;
				case UIData.Property.choosePieceAdapter:
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
			// CheckChange
			if (wrapProperty.p is GameDataBoardCheckPerspectiveChange<UIData>) {
				dirty = true;
				return;
			}
			// Child
			if (wrapProperty.p is ChoosePieceAdapter.UIData) {
				return;
			}
			Debug.LogError ("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
		}

		#endregion

		public void onClickBtnBtnBack()
		{
			if (this.data != null) {
				NoneRuleInputUI.UIData noneRuleInputUIData = this.data.findDataInParent<NoneRuleInputUI.UIData> ();
				if (noneRuleInputUIData != null) {
					ClickNoneUI.UIData clickNoneUIData = noneRuleInputUIData.sub.newOrOld<ClickNoneUI.UIData> ();
					{

					}
					noneRuleInputUIData.sub.v = clickNoneUIData;
				} else {
					Debug.LogError ("noneRuleInputUIData null: " + this);
				}
			} else {
				Debug.LogError ("data null: " + this);
			}
		}

	}
}