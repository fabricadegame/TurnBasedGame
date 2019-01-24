﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace GameManager.Match.Elimination
{
	public class ChooseEliminationRoundUI : UIBehavior<ChooseEliminationRoundUI.UIData>
	{

		#region UIData

		public class UIData : Data
		{

			public VP<ReferenceData<EliminationContent>> eliminationContent;

			public VP<ChooseEliminationRoundAdapter.UIData> chooseEliminationRoundAdapter;

			#region Constructor

			public enum Property
			{
				eliminationContent,
				chooseEliminationRoundAdapter
			}

			public UIData() : base()
			{
				this.eliminationContent = new VP<ReferenceData<EliminationContent>>(this, (byte)Property.eliminationContent, new ReferenceData<EliminationContent>(null));
				this.chooseEliminationRoundAdapter = new VP<ChooseEliminationRoundAdapter.UIData>(this, (byte)Property.chooseEliminationRoundAdapter, new ChooseEliminationRoundAdapter.UIData());
			}

			#endregion

			public bool processEvent(Event e)
			{
				bool isProcess = false;
				{
					// back
					if (!isProcess) {
						if (InputEvent.isBackEvent (e)) {
							ChooseEliminationRoundUI chooseEmliminationRoundUI = this.findCallBack<ChooseEliminationRoundUI> ();
							if (chooseEmliminationRoundUI != null) {
								chooseEmliminationRoundUI.onClickBtnBack ();
							} else {
								Debug.LogError ("chooseEliminationRoundUI null: " + this);
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

		#region txt

		public Text lbTitle;
		public static readonly TxtLanguage txtTitle = new TxtLanguage ();

		public Text tvBack;
		public static readonly TxtLanguage txtBack = new TxtLanguage ();

		static ChooseEliminationRoundUI()
		{
			txtTitle.add (Language.Type.vi, "Chọn vòng đấu loại");
			txtBack.add (Language.Type.vi, "Quay Lại");
		}

		#endregion

		public override void refresh ()
		{
			if (dirty) {
				dirty = false;
				if (this.data != null) {
					EliminationContent eliminationContent = this.data.eliminationContent.v.data;
					if (eliminationContent != null) {
						// chooseEliminationRoundAdapter
						{
							ChooseEliminationRoundAdapter.UIData chooseEliminationRoundAdapter = this.data.chooseEliminationRoundAdapter.v;
							if (chooseEliminationRoundAdapter != null) {
								chooseEliminationRoundAdapter.eliminationContent.v = new ReferenceData<EliminationContent> (eliminationContent);
							} else {
								Debug.LogError ("chooseBracketAdapter null: " + this);
							}
						}
					} else {
						Debug.LogError ("eliminationContent null: " + this);
					}
					// txt
					{
						if (lbTitle != null) {
							lbTitle.text = txtTitle.get ("Choose Elimination Round");
						} else {
							Debug.LogError ("lbTitle null: " + this);
						}
						if (tvBack != null) {
							tvBack.text = txtBack.get ("Back");
						} else {
							Debug.LogError ("tvBack null: " + this);
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

		public ChooseEliminationRoundAdapter chooseEliminationRoundAdapterPrefab;
		public Transform chooseEliminationRoundAdapterContainer;

		public override void onAddCallBack<T> (T data)
		{
			if (data is UIData) {
				UIData uiData = data as UIData;
				// Setting
				Setting.get().addCallBack(this);
				// Child
				{
					uiData.eliminationContent.allAddCallBack (this);
					uiData.chooseEliminationRoundAdapter.allAddCallBack (this);
				}
				dirty = true;
				return;
			}
			// Setting
			if (data is Setting) {
				dirty = true;
				return;
			}
			// Child
			{
				if (data is EliminationContent) {
					dirty = true;
					return;
				}
				if (data is ChooseEliminationRoundAdapter.UIData) {
					ChooseEliminationRoundAdapter.UIData chooseEliminationRoundAdapterUIData = data as ChooseEliminationRoundAdapter.UIData;
					// UI
					{
						UIUtils.Instantiate (chooseEliminationRoundAdapterUIData, chooseEliminationRoundAdapterPrefab, chooseEliminationRoundAdapterContainer);
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
				// Setting
				Setting.get().removeCallBack(this);
				// Child
				{
					uiData.eliminationContent.allRemoveCallBack (this);
					uiData.chooseEliminationRoundAdapter.allRemoveCallBack (this);
				}
				this.setDataNull (uiData);
				return;
			}
			// Setting
			if (data is Setting) {
				return;
			}
			// Child
			{
				if (data is EliminationContent) {
					return;
				}
				if (data is ChooseEliminationRoundAdapter.UIData) {
					ChooseEliminationRoundAdapter.UIData chooseEliminationRoundAdapterUIData = data as ChooseEliminationRoundAdapter.UIData;
					// UI
					{
						chooseEliminationRoundAdapterUIData.removeCallBackAndDestroy (typeof(ChooseEliminationRoundAdapter));
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
				case UIData.Property.eliminationContent:
					{
						ValueChangeUtils.replaceCallBack (this, syncs);
						dirty = true;
					}
					break;
				case UIData.Property.chooseEliminationRoundAdapter:
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
			// Setting
			if (wrapProperty.p is Setting) {
				switch ((Setting.Property)wrapProperty.n) {
				case Setting.Property.language:
					dirty = true;
					break;
				case Setting.Property.showLastMove:
					break;
				case Setting.Property.viewUrlImage:
					break;
				case Setting.Property.animationSetting:
					break;
				case Setting.Property.maxThinkCount:
					break;
				default:
					Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
					break;
				}
				return;
			}
			// Child
			{
				if (wrapProperty.p is EliminationContent) {
					return;
				}
				if (wrapProperty.p is ChooseEliminationRoundAdapter.UIData) {
					return;
				}
			}
			Debug.LogError ("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
		}

		#endregion

		public void onClickBtnBack()
		{
			if (this.data != null) {
				EliminationContentUI.UIData eliminationContentUIData = this.data.findDataInParent<EliminationContentUI.UIData> ();
				if (eliminationContentUIData != null) {
					eliminationContentUIData.chooseEliminationRoundUIData.v = null;
				} else {
					Debug.LogError ("eliminationContentUIData null: " + this);
				}
			} else {
				Debug.LogError ("data null: " + this);
			}
		}

	}
}