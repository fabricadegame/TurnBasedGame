﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using frame8.Logic.Misc.Visual.UI.ScrollRectItemsAdapter;

namespace Posture
{
	public class ChoosePostureHolder : SriaHolderBehavior<ChoosePostureHolder.UIData>
	{

		#region UIData

		public class UIData : BaseItemViewsHolder
		{
			
			public VP<ReferenceData<PostureGameData>> postureGameData;

			public VP<MiniGameDataUI.UIData> miniGameDataUI;

			#region Constructor

			public enum Property
			{
				postureGameData,
				miniGameDataUI
			}

			public UIData() : base()
			{
				this.postureGameData = new VP<ReferenceData<PostureGameData>>(this, (byte)Property.postureGameData, new ReferenceData<PostureGameData>(null));
				this.miniGameDataUI = new VP<MiniGameDataUI.UIData>(this, (byte)Property.miniGameDataUI, new MiniGameDataUI.UIData());
			}

			#endregion

			public void updateView(ChoosePostureAdapter.UIData myParams)
			{
				// Find postureGameData
				PostureGameData postureGameData = null;
				{
					if (ItemIndex >= 0 && ItemIndex < myParams.postureGameDatas.Count) {
						postureGameData = myParams.postureGameDatas [ItemIndex];
					} else {
						Debug.LogError ("ItemIdex error: " + this);
					}
				}
				// Updae
				this.postureGameData.v = new ReferenceData<PostureGameData> (postureGameData);
			}

		}

		#endregion

		#region Refresh

		#region txt

		public Text tvChoose;
		public static readonly TxtLanguage txtChoose = new TxtLanguage();

		public static readonly TxtLanguage txtIndex = new TxtLanguage();
		public static readonly TxtLanguage txtName = new TxtLanguage ();

		static ChoosePostureHolder()
		{
			txtChoose.add (Language.Type.vi, "Chọn");
			txtIndex.add (Language.Type.vi, "");
			txtName.add (Language.Type.vi, "");
		}

		#endregion

		public Text tvIndex;
		public InputField tvName;
		public Button btnChoose;

		public override void refresh ()
		{
			base.refresh ();
			if (dirty) {
				dirty = false;
				if (this.data != null) {
					PostureGameData postureGameData = this.data.postureGameData.v.data;
					if (postureGameData != null) {
						// Index
						{
							if (tvIndex != null) {
								tvIndex.text = txtIndex.get ("") + "" + (postureGameData.postureIndex.v + 1);
							} else {
								Debug.LogError ("tvIndex null: " + this);
							}
						}
						// Name
						{
							if (tvName != null) {
								tvName.text = txtName.get ("") + "" + postureGameData.name.v;
							} else {
								Debug.LogError ("tvName null: " + this);
							}
						}
						// miniGameDataUIData
						{
							MiniGameDataUI.UIData miniGameDataUIData = this.data.miniGameDataUI.v;
							if (miniGameDataUIData != null) {
								miniGameDataUIData.gameData.v = new ReferenceData<GameData> (postureGameData.gameData.v);
							} else {
								Debug.LogError ("miniGameDataUIData null: " + this);
							}
						}
					} else {
						Debug.LogError ("postureGameData null: " + this);
					}
					// txt
					{
						if (tvChoose != null) {
							tvChoose.text = txtChoose.get ("Choose");
						} else {
							Debug.LogError ("tvChoose null: " + this);
						}
					}
				} else {
					// Debug.LogError ("data null: " + this);
				}
			}
		}

		#endregion

		#region implement callBacks

		public Transform miniGameDataUIContainer;
		public MiniGameDataUI miniGameDataPrefab;

		public override void onAddCallBack<T> (T data)
		{
			if (data is UIData) {
				UIData uiData = data as UIData;
				// Setting
				Setting.get().addCallBack(this);
				// Child
				{
					uiData.postureGameData.allAddCallBack (this);
					uiData.miniGameDataUI.allAddCallBack (this);
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
				if (data is PostureGameData) {
					dirty = true;
					return;
				}
				if (data is MiniGameDataUI.UIData) {
					MiniGameDataUI.UIData subUIData = data as MiniGameDataUI.UIData;
					// UI
					{
						UIUtils.Instantiate (subUIData, miniGameDataPrefab, miniGameDataUIContainer);
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
					uiData.postureGameData.allRemoveCallBack (this);
					uiData.miniGameDataUI.allRemoveCallBack (this);
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
				if (data is PostureGameData) {
					return;
				}
				if (data is MiniGameDataUI.UIData) {
					MiniGameDataUI.UIData subUIData = data as MiniGameDataUI.UIData;
					// UI
					{
						subUIData.removeCallBackAndDestroy (typeof(MiniGameDataUI));
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
				case UIData.Property.postureGameData:
					{
						ValueChangeUtils.replaceCallBack (this, syncs);
						dirty = true;
					}
					break;
				case UIData.Property.miniGameDataUI:
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
				if (wrapProperty.p is PostureGameData) {
					switch ((PostureGameData.Property)wrapProperty.n) {
					case PostureGameData.Property.postureIndex:
						dirty = true;
						break;
					case PostureGameData.Property.name:
						dirty = true;
						break;
					case PostureGameData.Property.gameData:
						dirty = true;
						break;
					default:
						Debug.LogError ("unknown wrapProperty: " + wrapProperty + "; " + this);
						break;
					}
					return;
				}
				if (wrapProperty.p is MiniGameDataUI.UIData) {
					return;
				}
			}
			Debug.LogError ("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
		}

		#endregion

		public void onClickBtnChoose()
		{
			if (this.data != null) {
				PostureGameData postureGameData = this.data.postureGameData.v.data;
				if (postureGameData != null) {
					EditPostureGameDataUI.UIData editPostureGameDataUIData = this.data.findDataInParent<EditPostureGameDataUI.UIData> ();
					if (editPostureGameDataUIData != null) {
						EditPostureGameDataUI editPostureGameDataUI = editPostureGameDataUIData.findCallBack<EditPostureGameDataUI> ();
						if (editPostureGameDataUI != null) {
							editPostureGameDataUI.makeNewGame (postureGameData.gameData.v);
						} else {
							Debug.LogError ("editPostureGameDataUI null: " + this);
						}
					} else {
						Debug.LogError ("editPostureGameDataUIData null: " + this);
					}
				} else {
					Debug.LogError ("postureGameData null: " + this);
				}
			} else {
				Debug.LogError ("data null: " + this);
			}
		}

	}
}