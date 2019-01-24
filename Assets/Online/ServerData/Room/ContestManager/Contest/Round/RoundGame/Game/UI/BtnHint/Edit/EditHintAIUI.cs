﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Hint
{
	public class EditHintAIUI : UIBehavior<EditHintAIUI.UIData>
	{

		#region UIData

		public class UIData : Data
		{

			public VP<AIUI.UIData> aiUIData;

			public VP<EditType> editType;

			#region Constructor

			public enum Property
			{
				aiUIData,
				editType
			}

			public UIData() : base()
			{
				this.aiUIData = new VP<AIUI.UIData>(this, (byte)Property.aiUIData, new AIUI.UIData());
				this.editType = new VP<EditType>(this, (byte)Property.editType, EditType.Later);
			}

			#endregion

			public bool processEvent(Event e)
			{
				bool isProcess = false;
				{
					// back
					if (!isProcess) {
						if (InputEvent.isBackEvent (e)) {
							EditHintAIUI editHintAIUI = this.findCallBack<EditHintAIUI> ();
							if (editHintAIUI != null) {
								editHintAIUI.onClickBtnBack ();
							} else {
								Debug.LogError ("editHintAIUI null: " + this);
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

		public Dropdown drEditType;

		public override void Awake ()
		{
			base.Awake ();
			if (drEditType != null) {
				drEditType.onValueChanged.AddListener (delegate(int newValue) {
					if (drEditType.gameObject.activeInHierarchy) {
						if (this.data != null) {
							this.data.editType.v = (Data.EditType)newValue;
						} else {
							Debug.LogError ("data null: " + this);
						}
					} else {
						Debug.LogError ("drEditType not active: " + this);
					}
				});
			} else {
				Debug.LogError ("drValue null: " + this);
			}
		}

		#region txt

		public static readonly TxtLanguage txtImmediately = new TxtLanguage ();
		public static readonly TxtLanguage txtLater = new TxtLanguage ();

		public Text lbTitle;
		public static readonly TxtLanguage txtTitle = new TxtLanguage ();

		public Text lbEditType;
		public static readonly TxtLanguage txtEditType = new TxtLanguage();

		public Text tvBack;
		public static readonly TxtLanguage txtBack = new TxtLanguage ();

		public Text tvApply;
		public static readonly TxtLanguage txtApply = new TxtLanguage ();
		public static readonly TxtLanguage txtNotDifferentApply = new TxtLanguage();

		public Text tvReset;
		public static readonly TxtLanguage txtReset = new TxtLanguage ();
		public static readonly TxtLanguage txtNotDifferentReset = new TxtLanguage ();

		static EditHintAIUI()
		{
			txtImmediately.add(Language.Type.vi, "Ngay lập tức");
			txtLater.add(Language.Type.vi, "Sau này");

			txtTitle.add(Language.Type.vi, "Chỉnh Sửa Gợi Ý Của AI");
			txtEditType.add(Language.Type.vi, "Loại chỉnh sửa");
			txtBack.add(Language.Type.vi, "Quay Lại");

			txtApply.add(Language.Type.vi, "Áp Dụng");
			txtNotDifferentApply.add(Language.Type.vi, "Không khác, không thể áp dụng");

			txtReset.add(Language.Type.vi, "Đặt Lại");
			txtNotDifferentReset.add(Language.Type.vi, "Không khác, không thể đặt lại");
		}

		#endregion

		public Button btnApply;
		public Button btnReset;

		public GameObject btnsContainer;

		public override void refresh ()
		{
			if (dirty) {
				dirty = false;
				if (this.data != null) {
					// drEditType
					if (drEditType != null) {
						// options
						{
							string[] options = new string[]{ txtImmediately.get ("Immediately"), txtLater.get ("Later") };
							// remove 
							{
								if (drEditType.options.Count > options.Length) {
									drEditType.options.RemoveRange (options.Length, drEditType.options.Count - options.Length);
								}
							}
							for (int i = 0; i < options.Length; i++) {
								if (i < drEditType.options.Count) {
									// Update
									drEditType.options [i].text = options [i];
								} else {
									// Add new
									Dropdown.OptionData optionData = new Dropdown.OptionData ();
									{
										optionData.text = options [i];
									}
									drEditType.options.Add (optionData);
								}
							}
						}
						// set value
						drEditType.value = (int)this.data.editType.v;
					} else {
						Debug.LogError ("drEditType null: " + this);
					}
					// aiUIData
					{
						EditData<Computer.AI> editComputerAI = null;
						{
							AIUI.UIData aiUIData = this.data.aiUIData.v;
							if (aiUIData != null) {
								editComputerAI = aiUIData.editAI.v;
							} else {
								Debug.LogError ("aiUIData null: " + this);
							}
						}
						if (editComputerAI != null) {
							editComputerAI.editType.v = this.data.editType.v;
						} else {
							Debug.LogError ("editComputerAI null: " + this);
						}
					}
					// btnsContainer
					{
						if (btnsContainer != null) {
							switch (this.data.editType.v) {
							case Data.EditType.Immediate:
								btnsContainer.SetActive (false);
								break;
							case Data.EditType.Later:
								btnsContainer.SetActive (true);
								break;
							default:
								Debug.LogError ("unknown editType: " + this.data.editType.v);
								break;
							}
						} else {
							Debug.LogError ("btnsContainer null: " + this);
						}
					}
					// btnUpdate, btnReset interactable
					{
						if (btnApply != null && tvApply!=null && btnReset!=null && tvReset!=null) {
							// Find
							bool isDifferent = true;
							{
								// Find
								EditData<Computer.AI> editComputerAI = null;
								{
									AIUI.UIData aiUIData = this.data.aiUIData.v;
									if (aiUIData != null) {
										editComputerAI = aiUIData.editAI.v;
									} else {
										Debug.LogError ("aiUIData null: " + this);
									}
								}
								// Process
								if (editComputerAI != null) {
									if (editComputerAI.origin.v.data != null && editComputerAI.show.v.data != null && editComputerAI.origin.v.data != editComputerAI.show.v.data) {
										isDifferent = DataUtils.IsDifferent (editComputerAI.show.v.data, editComputerAI.origin.v.data);
									} else {
										// Debug.LogError ("not different between origin and show, so cannot update: " + this);
									}
								} else {
									Debug.LogError ("editComputerAI null: " + this);
								}
							}
							// Process
							if (isDifferent) {
								// apply
								{
									btnApply.enabled = true;
									tvApply.text = txtApply.get ("Apply");
								}
								// reset
								{
									btnReset.enabled = true;
									tvReset.text = txtReset.get ("Reset");
								}
							} else {
								// apply
								{
									btnApply.enabled = false;
									tvApply.text = txtApply.get ("Not different, cannot apply");
								}
								// reset
								{
									btnReset.enabled = false;
									tvReset.text = txtReset.get ("Not diffrent, cannot reset");
								}
							}
						} else {
							Debug.LogError ("btnUpdate, btnReset null: " + this);
						}
					}
					// Compare
					{
						EditData<Computer.AI> editComputerAI = null;
						{
							AIUI.UIData aiUIData = this.data.aiUIData.v;
							if (aiUIData != null) {
								editComputerAI = aiUIData.editAI.v;
							} else {
								Debug.LogError ("aiUIData null: " + this);
							}
						}
						if (editComputerAI != null) {
							if (this.data.editType.v == Data.EditType.Later) {
								editComputerAI.compare.v = new ReferenceData<Computer.AI> (editComputerAI.origin.v.data);
							} else {
								editComputerAI.compare.v = new ReferenceData<Computer.AI> (null);
							}
						} else {
							Debug.LogError ("editComputerAI null: " + this);
						}
					}
					// TODO reset Layout
					{
						WrapContentVerticalLayoutGroup wrapContent = this.GetComponent<WrapContentVerticalLayoutGroup> ();
						if (wrapContent != null) {
							wrapContent.dirty = true;
						} else {
							Debug.LogError ("wrapContent null: " + this);
						}
					}
					// txt
					{
						if (lbTitle != null) {
							lbTitle.text = txtTitle.get ("Edit Hint AI");
						} else {
							Debug.LogError ("lbTitle null: " + this);
						}
						if (lbEditType != null) {
							lbEditType.text = txtEditType.get ("Edit type");
						} else {
							Debug.LogError ("tvEditType null: " + this);
						}
						if (tvBack != null) {
							tvBack.text = txtBack.get ("Back");
						} else {
							Debug.LogError ("tvBack null: " + this);
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

		public Transform aiContainer;
		public AIUI aiUIPrefab;

		public override void onAddCallBack<T> (T data)
		{
			if (data is UIData) {
				UIData uiData = data as UIData;
				// Setting
				Setting.get().addCallBack(this);
				// Child
				{
					uiData.aiUIData.allAddCallBack (this);
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
				if (data is AIUI.UIData) {
					AIUI.UIData aiUIData = data as AIUI.UIData;
					// UI
					{
						UIUtils.Instantiate (aiUIData, aiUIPrefab, aiContainer);
					}
					// Child
					{
						aiUIData.editAI.allAddCallBack (this);
					}
					dirty = true;
					return;
				}
				// Child
				{
					if (data is EditData<Computer.AI>) {
						EditData<Computer.AI> editComputerAI = data as EditData<Computer.AI>;
						// Child
						{
							editComputerAI.show.allAddCallBack (this);
						}
						dirty = true;
						return;
					}
					// Child
					{
						Debug.LogError ("addCallBackAllChildren: " + data + "; " + this);
						data.addCallBackAllChildren (this);
						dirty = true;
						return;
					}
				}
			}
			// Debug.LogError ("Don't process: " + data + "; " + this);
		}

		public override void onRemoveCallBack<T> (T data, bool isHide)
		{
			if (data is UIData) {
				UIData uiData = data as UIData;
				// Setting
				Setting.get().removeCallBack(this);
				// Child
				{
					uiData.aiUIData.allRemoveCallBack (this);
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
				if (data is AIUI.UIData) {
					AIUI.UIData aiUIData = data as AIUI.UIData;
					// UI
					{
						aiUIData.removeCallBackAndDestroy (typeof(AIUI));
					}
					// Child
					{
						aiUIData.editAI.allRemoveCallBack (this);
					}
					return;
				}
				// Child
				{
					if (data is EditData<Computer.AI>) {
						EditData<Computer.AI> editComputerAI = data as EditData<Computer.AI>;
						// Child
						{
							editComputerAI.show.allRemoveCallBack (this);
						}
						return;
					}
					// Child
					{
						data.removeCallBackAllChildren (this);
						dirty = true;
						return;
					}
				}
			}
			// Debug.LogError ("Don't process: " + data + "; " + this);
		}

		public override void onUpdateSync<T> (WrapProperty wrapProperty, List<Sync<T>> syncs)
		{
			if (WrapProperty.checkError (wrapProperty)) {
				return;
			}
			if (wrapProperty.p is UIData) {
				switch ((UIData.Property)wrapProperty.n) {
				case UIData.Property.aiUIData:
					{
						ValueChangeUtils.replaceCallBack (this, syncs);
						dirty = true;
					}
					break;
				case UIData.Property.editType:
					dirty = true;
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
				if (wrapProperty.p is AIUI.UIData) {
					switch ((AIUI.UIData.Property)wrapProperty.n) {
					case AIUI.UIData.Property.editAI:
						{
							ValueChangeUtils.replaceCallBack (this, syncs);
							dirty = true;
						}
						break;
					case AIUI.UIData.Property.sub:
						break;
					default:
						Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
						break;
					}
					return;
				}
				// Child
				{
					if (wrapProperty.p is EditData<Computer.AI>) {
						switch ((EditData<Computer.AI>.Property)wrapProperty.n) {
						case EditData<Computer.AI>.Property.origin:
							dirty = true;
							break;
						case EditData<Computer.AI>.Property.show:
							{
								ValueChangeUtils.replaceCallBack (this, syncs);
								dirty = true;
							}
							break;
						case EditData<Computer.AI>.Property.compare:
							dirty = true;
							break;
						case EditData<Computer.AI>.Property.canEdit:
							break;
						case EditData<Computer.AI>.Property.editType:
							break;
						default:
							Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
							break;
						}
						return;
					}
					// Child
					{
						if (Generic.IsAddCallBackInterface<T> ()) {
							ValueChangeUtils.replaceCallBack (this, syncs);
						}
						Debug.LogError ("have change in ai: " + this);
						dirty = true;
						return;
					}
				}
			}
			// Debug.LogError ("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
		}

		#endregion

		public void onClickBtnBack()
		{
			Debug.LogError ("onClickBtnBack: " + this);
			if (this.data != null) {
				HintUI.UIData hintUIData = this.data.findDataInParent<HintUI.UIData> ();
				if (hintUIData != null) {
					hintUIData.editHintAIUIData.v = null;
				} else {
					Debug.LogError ("hintUIData null: " + this);
				}
			} else {
				Debug.LogError ("data null: " + this);
			}
		}

		public void onClickBtnApply()
		{
			Debug.LogError ("onClickBtnUpdate: " + this);
			if (this.data != null) {
				// Find
				EditData<Computer.AI> editComputerAI = null;
				{
					AIUI.UIData aiUIData = this.data.aiUIData.v;
					if (aiUIData != null) {
						editComputerAI = aiUIData.editAI.v;
					} else {
						Debug.LogError ("aiUIData null: " + this);
					}
				}
				// Process
				if (editComputerAI != null) {
					if (editComputerAI.origin.v.data != null && editComputerAI.show.v.data != null && editComputerAI.origin.v.data != editComputerAI.show.v.data) {
						DataUtils.copyData (editComputerAI.origin.v.data, editComputerAI.show.v.data);
					} else {
						Debug.LogError ("not different between origin and show, so cannot update: " + this);
					}
				} else {
					Debug.LogError ("editComputerAI null: " + this);
				}
				dirty = true;
			} else {
				Debug.LogError ("data null: " + this);
			}
		}

		public void onClickBtnReset()
		{
			Debug.LogError ("onClickBtnReset: " + this);
			if (this.data != null) {
				// Find
				EditData<Computer.AI> editComputerAI = null;
				{
					AIUI.UIData aiUIData = this.data.aiUIData.v;
					if (aiUIData != null) {
						editComputerAI = aiUIData.editAI.v;
					} else {
						Debug.LogError ("aiUIData null: " + this);
					}
				}
				// Process
				if (editComputerAI != null) {
					editComputerAI.show.v = new ReferenceData<Computer.AI> (null);
				} else {
					Debug.LogError ("editComputerAI null: " + this);
				}
				dirty = true;
			} else {
				Debug.LogError ("data null: " + this);
			}
		}

	}
}