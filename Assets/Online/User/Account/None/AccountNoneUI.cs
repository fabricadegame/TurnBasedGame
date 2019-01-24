﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AccountNoneUI : UIBehavior<AccountNoneUI.UIData>
{

	#region UIData

	public class UIData : AccountUI.UIData.Sub
	{

		public VP<EditData<AccountNone>> editAccountNone;

		#region Constructor

		public enum Property
		{
			editAccountNone
		}

		public UIData() : base()
		{
			this.editAccountNone = new VP<EditData<AccountNone>>(this, (byte)Property.editAccountNone, new EditData<AccountNone>());
		}

		#endregion

		public override Account.Type getType ()
		{
			return Account.Type.NONE;
		}

	}

	#endregion

	#region Refresh

	public Text lbTitle;
	public static TxtLanguage txtTitle = new TxtLanguage ();

	static AccountNoneUI()
	{
		txtTitle.add (Language.Type.vi, "Tài khoản ẩn danh");
	}

	private bool needReset = true;
	public GameObject differentIndicator;

	public override void refresh ()
	{
		if (dirty) {
			dirty = false;
			if (this.data != null) {
				EditData<AccountNone> editAccountNone = this.data.editAccountNone.v;
				if (editAccountNone != null) {
					editAccountNone.update ();
					// get show
					AccountNone show = editAccountNone.show.v.data;
					AccountNone compare = editAccountNone.compare.v.data;
					if (show != null) {
						// differentIndicator
						if (differentIndicator != null) {
							bool isDifferent = false;
							{
								if (editAccountNone.compareOtherType.v.data != null) {
									if (editAccountNone.compareOtherType.v.data.GetType () != show.GetType ()) {
										isDifferent = true;
									}
								}
							}
							differentIndicator.SetActive (isDifferent);
						} else {
							Debug.LogError ("differentIndicator null: " + this);
						}
						// request
						{
							// get server state
							Server.State.Type serverState = Server.State.Type.Connect;
							{
								Server server = show.findDataInParent<Server> ();
								if (server != null) {
									if (server.state.v != null) {
										serverState = server.state.v.getType ();
									} else {
										Debug.LogError ("server state null: " + this);
									}
								} else {
									Debug.LogError ("server null: " + compare + "; " + serverState + "; " + this);
								}
							}
							// set origin
							{
								
							}
							// reset?
							if (needReset) {
								needReset = false;
							}
						}
					} else {
						Debug.LogError ("show null: " + this);
					}
				} else {
					Debug.LogError ("editAccountNone null: " + this);
				}
				// txt
				{
					if (lbTitle != null) {
						lbTitle.text = txtTitle.get ("Unknown Account");
					} else {
						Debug.LogError ("lbTitle null: " + this);
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

	private Server server = null;

	public override void onAddCallBack<T> (T data)
	{
		if (data is UIData) {
			UIData uiData = data as UIData;
			// Setting
			Setting.get().addCallBack(this);
			// Child
			{
				uiData.editAccountNone.allAddCallBack (this);
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
			// editAccountNone
			{
				if (data is EditData<AccountNone>) {
					EditData<AccountNone> editAccountNone = data as EditData<AccountNone>;
					// Child
					{
						editAccountNone.show.allAddCallBack (this);
						editAccountNone.compare.allAddCallBack (this);
					}
					dirty = true;
					return;
				}
				// Child
				{
					if (data is AccountNone) {
						AccountNone accountNone = data as AccountNone;
						// Parent
						{
							DataUtils.addParentCallBack (accountNone, this, ref this.server);
						}
						needReset = true;
						dirty = true;
						return;
					}
					// Parent
					{
						if (data is Server) {
							dirty = true;
							return;
						}
					}
				}
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
				uiData.editAccountNone.allRemoveCallBack (this);
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
			// editAccountNone
			{
				if (data is EditData<AccountNone>) {
					EditData<AccountNone> editAccountNone = data as EditData<AccountNone>;
					// Child
					{
						editAccountNone.show.allRemoveCallBack (this);
						editAccountNone.compare.allRemoveCallBack (this);
					}
					return;
				}
				// Child
				{
					if (data is AccountNone) {
						AccountNone accountNone = data as AccountNone;
						// Parent
						{
							DataUtils.removeParentCallBack (accountNone, this, ref this.server);
						}
						return;
					}
					// Parent
					{
						if (data is Server) {
							return;
						}
					}
				}
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
			case UIData.Property.editAccountNone:
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
			// editAccountNone
			{
				if (wrapProperty.p is EditData<AccountNone>) {
					switch ((EditData<AccountNone>.Property)wrapProperty.n) {
					case EditData<AccountNone>.Property.origin:
						dirty = true;
						break;
					case EditData<AccountNone>.Property.show:
						{
							ValueChangeUtils.replaceCallBack (this, syncs);
							dirty = true;
						}
						break;
					case EditData<AccountNone>.Property.compare:
						{
							ValueChangeUtils.replaceCallBack (this, syncs);
							dirty = true;
						}
						break;
					case EditData<AccountNone>.Property.compareOtherType:
						dirty = true;
						break;
					case EditData<AccountNone>.Property.canEdit:
						dirty = true;
						break;
					case EditData<AccountNone>.Property.editType:
						dirty = true;
						break;
					default:
						Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
						break;
					}
					return;
				}
				// Child
				{
					if (wrapProperty.p is AccountNone) {
						switch ((AccountNone.Property)wrapProperty.n) {
						default:
							Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
							break;
						}
						return;
					}
					// Parent
					{
						if (wrapProperty.p is Server) {
							Server.State.OnUpdateSyncStateChange (wrapProperty, this);
							return;
						}
					}
				}
			}
		}
		Debug.LogError ("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
	}

	#endregion

}