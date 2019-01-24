﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace GameManager.Match
{
	public class ChooseRoundUI : UIBehavior<ChooseRoundUI.UIData>
	{

		#region UIData

		public class UIData : Data
		{

			public VP<ReferenceData<Contest>> contest;

			public VP<ChooseRoundAdapter.UIData> chooseRoundAdapter;

			public VP<RequestNewRoundInformUI.UIData> requestNewRoundInformUIData;

			#region Constructor

			public enum Property
			{
				contest,
				chooseRoundAdapter,
				requestNewRoundInformUIData
			}

			public UIData() : base()
			{
				this.contest = new VP<ReferenceData<Contest>>(this, (byte)Property.contest, new ReferenceData<Contest>(null));
				this.chooseRoundAdapter = new VP<ChooseRoundAdapter.UIData>(this, (byte)Property.chooseRoundAdapter, new ChooseRoundAdapter.UIData());
				this.requestNewRoundInformUIData = new VP<RequestNewRoundInformUI.UIData>(this, (byte)Property.requestNewRoundInformUIData, new RequestNewRoundInformUI.UIData());
			}

			#endregion

			public bool processEvent(Event e)
			{
				bool isProcess = false;
				{
					// requestNewRoundInformUIData
					if (!isProcess) {
						RequestNewRoundInformUI.UIData requestNewRoundInformUIData = this.requestNewRoundInformUIData.v;
						if (requestNewRoundInformUIData != null) {
							isProcess = requestNewRoundInformUIData.processEvent (e);
						} else {
							Debug.LogError ("requestNewRoundInformUIData null: " + this);
						}
					}
					// back
					if (!isProcess) {
						if (InputEvent.isBackEvent (e)) {
							ChooseRoundUI chooseRoundUI = this.findCallBack<ChooseRoundUI> ();
							if (chooseRoundUI != null) {
								chooseRoundUI.onClickBtnBack ();
							} else {
								Debug.LogError ("chooseRoundUI null: " + this);
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
		public static readonly TxtLanguage txtTitle = new TxtLanguage();

		public Text tvBack;
		public static readonly TxtLanguage txtBack = new TxtLanguage();

		static ChooseRoundUI()
		{
			txtTitle.add (Language.Type.vi, "Chọn set đấu");
			txtBack.add (Language.Type.vi, "Quay Lại");
		}

		#endregion

		public override void refresh ()
		{
			if (dirty) {
				dirty = false;
				if (this.data != null) {
					Contest contest = this.data.contest.v.data;
					if (contest != null) {
						// chooseRoundAdapter
						{
							ChooseRoundAdapter.UIData chooseRoundAdapterUIData = this.data.chooseRoundAdapter.v;
							if (chooseRoundAdapterUIData != null) {
								chooseRoundAdapterUIData.contest.v = new ReferenceData<Contest> (contest);
							} else {
								Debug.LogError ("chooseRoundAdapterUIData null: " + this);
							}
						}
						// requestNewRoundInformUIData
						{
							RequestNewRoundInformUI.UIData requestNewRoundInformUIData = this.data.requestNewRoundInformUIData.v;
							if (requestNewRoundInformUIData != null) {
								requestNewRoundInformUIData.requestNewRound.v = new ReferenceData<RequestNewRound> (contest.requestNewRound.v);
							} else {
								Debug.LogError ("requestNewRoundInformUIData null: " + this);
							}
						}
					} else {
						Debug.LogError ("contest null: " + this);
					}
					// txt
					{
						if (lbTitle != null) {
							lbTitle.text = txtTitle.get ("Choose Set In Match");
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

		public ChooseRoundAdapter chooseRoundAdapterPrefab;
		public Transform chooseRoundAdapterContainer;

		public RequestNewRoundInformUI requestNewRoundInformPrefab;
		public Transform requestNewRoundInformContainer;

		public override void onAddCallBack<T> (T data)
		{
			if (data is UIData) {
				UIData uiData = data as UIData;
				// Setting
				Setting.get().addCallBack(this);
				// Child
				{
					uiData.contest.allAddCallBack (this);
					uiData.chooseRoundAdapter.allAddCallBack (this);
					uiData.requestNewRoundInformUIData.allAddCallBack (this);
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
				if (data is Contest) {
					dirty = true;
					return;
				}
				if (data is ChooseRoundAdapter.UIData) {
					ChooseRoundAdapter.UIData chooseRoundAdapterUIData = data as ChooseRoundAdapter.UIData;
					// UI
					{
						UIUtils.Instantiate (chooseRoundAdapterUIData, chooseRoundAdapterPrefab, chooseRoundAdapterContainer);
					}
					dirty = true;
					return;
				}
				if (data is RequestNewRoundInformUI.UIData) {
					RequestNewRoundInformUI.UIData requestNewRoundInformUIData = data as RequestNewRoundInformUI.UIData;
					// UI
					{
						UIUtils.Instantiate (requestNewRoundInformUIData, requestNewRoundInformPrefab, requestNewRoundInformContainer);
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
					uiData.contest.allRemoveCallBack (this);
					uiData.chooseRoundAdapter.allRemoveCallBack (this);
					uiData.requestNewRoundInformUIData.allRemoveCallBack (this);
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
				if (data is Contest) {
					return;
				}
				if (data is ChooseRoundAdapter.UIData) {
					ChooseRoundAdapter.UIData chooseRoundAdapterUIData = data as ChooseRoundAdapter.UIData;
					// UI
					{
						chooseRoundAdapterUIData.removeCallBackAndDestroy (typeof(ChooseRoundAdapter));
					}
					return;
				}
				if (data is RequestNewRoundInformUI.UIData) {
					RequestNewRoundInformUI.UIData requestNewRoundInformUIData = data as RequestNewRoundInformUI.UIData;
					// UI
					{
						requestNewRoundInformUIData.removeCallBackAndDestroy (typeof(RequestNewRoundInformUI));
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
				case UIData.Property.contest:
					{
						ValueChangeUtils.replaceCallBack (this, syncs);
						dirty = true;
					}
					break;
				case UIData.Property.chooseRoundAdapter:
					{
						ValueChangeUtils.replaceCallBack (this, syncs);
						dirty = true;
					}
					break;
				case UIData.Property.requestNewRoundInformUIData:
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
				if (wrapProperty.p is Contest) {
					return;
				}
				if (wrapProperty.p is ChooseRoundAdapter.UIData) {
					return;
				}
				if (wrapProperty.p is RequestNewRoundInformUI.UIData) {
					return;
				}
			}
			Debug.LogError ("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
		}

		#endregion

		public void onClickBtnBack()
		{
			if (this.data != null) {
				ContestUI.UIData contestUIData = this.data.findDataInParent<ContestUI.UIData> ();
				if (contestUIData != null) {
					contestUIData.chooseRoundUIData.v = null;
				} else {
					Debug.LogError ("contestUIData null: " + this);
				}
			} else {
				Debug.LogError ("data null: " + this);
			}
		}

	}
}