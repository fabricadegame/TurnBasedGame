﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace GameManager.Match.Elimination
{
	public class RequestNewEliminationRoundStateAskUI : UIBehavior<RequestNewEliminationRoundStateAskUI.UIData>
	{

		#region UIData

		public class UIData : RequestNewEliminationRoundUI.UIData.Sub
		{

			public VP<ReferenceData<RequestNewEliminationRoundStateAsk>> requestNewEliminationRoundStateAsk;

			public VP<WhoCanAskAdapter.UIData> whoCanAskAdapter;

			#region Btn

			public abstract class Btn : Data
			{

				public enum Type
				{
					Accept,
					Cancel
				}

				public abstract Type getType();

			}

			public VP<Btn> btn;

			#endregion

			public enum Visibility
			{
				Show,
				Hide
			}

			public VP<Visibility> visibility;

			#region Constructor

			public enum Property
			{
				requestNewEliminationRoundStateAsk,
				whoCanAskAdapter,
				btn,
				visibility
			}

			public UIData() : base()
			{
				this.requestNewEliminationRoundStateAsk = new VP<ReferenceData<RequestNewEliminationRoundStateAsk>>(this, (byte)Property.requestNewEliminationRoundStateAsk, new ReferenceData<RequestNewEliminationRoundStateAsk>(null));
				this.whoCanAskAdapter = new VP<WhoCanAskAdapter.UIData>(this, (byte)Property.whoCanAskAdapter, new WhoCanAskAdapter.UIData());
				this.btn = new VP<Btn>(this, (byte)Property.btn, null);
				this.visibility = new VP<Visibility>(this, (byte)Property.visibility, Visibility.Show);
			}

			#endregion

			public override RequestNewEliminationRound.State.Type getType ()
			{
				return RequestNewEliminationRound.State.Type.Ask;
			}

			public void reset()
			{
				this.visibility.v = Visibility.Show;
			}

			public override bool processEvent (Event e)
			{
				bool isProcess = false;
				{
					// back
					if (!isProcess) {
						if (InputEvent.isBackEvent (e)) {
							if (this.visibility.v == Visibility.Show) {
								this.visibility.v = Visibility.Hide;
								isProcess = true;
							}
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

		public static readonly TxtLanguage txtHide = new TxtLanguage ();
		public static readonly TxtLanguage txtShow = new TxtLanguage();

		static RequestNewEliminationRoundStateAskUI()
		{
			txtTitle.add (Language.Type.vi, "Yêu cầu tạo vòng đấu mới: trạng thái hỏi");
			txtHide.add (Language.Type.vi, "Dấu yêu cầu tạo vòng mới");
			txtShow.add (Language.Type.vi, "Hiện yêu cầu tạo vòng mới");
		}

		#endregion

		public GameObject contentContainer;
		public Text tvVisibility;

		public override void refresh ()
		{
			if (dirty) {
				dirty = false;
				if (this.data != null) {
					RequestNewEliminationRoundStateAsk requestNewEliminationRoundStateAsk = this.data.requestNewEliminationRoundStateAsk.v.data;
					if (requestNewEliminationRoundStateAsk != null) {
						// visibility
						{
							if (contentContainer != null && tvVisibility != null) {
								switch (this.data.visibility.v) {
								case UIData.Visibility.Show:
									{
										contentContainer.SetActive (true);
										tvVisibility.text = txtHide.get ("Hide Request New Elimination Round Ask");
									}
									break;
								case UIData.Visibility.Hide:
									{
										contentContainer.SetActive (false);
										tvVisibility.text = txtShow.get ("Show Request New Elimination Round Ask");
									}
									break;
								default:
									Debug.LogError ("unknown visiblity: " + this.data.visibility.v + "; " + this);
									break;
								}
							} else {
								Debug.LogError ("contentContainer, tvVisibility null: " + this);
							}
						}
						// whoCanAskAdapter
						{
							WhoCanAskAdapter.UIData whoCanAskAdapter = this.data.whoCanAskAdapter.v;
							if (whoCanAskAdapter != null) {
								whoCanAskAdapter.requestNewEliminationRoundStateAsk.v = new ReferenceData<RequestNewEliminationRoundStateAsk> (requestNewEliminationRoundStateAsk);
							} else {
								Debug.LogError ("whoCanAskAdapter null: " + this);
							}
						}
						// btn
						{
							uint profileId = Server.getProfileUserId (requestNewEliminationRoundStateAsk);
							if (requestNewEliminationRoundStateAsk.isCanAccept (profileId)) {
								RequestNewEliminationRoundAskBtnAcceptUI.UIData btnAcceptUIData = this.data.btn.newOrOld<RequestNewEliminationRoundAskBtnAcceptUI.UIData> ();
								{

								}
								this.data.btn.v = btnAcceptUIData;
							} else if (requestNewEliminationRoundStateAsk.isCanCancel (profileId)) {
								RequestNewEliminationRoundAskBtnCancelUI.UIData btnCancelUIData = this.data.btn.newOrOld<RequestNewEliminationRoundAskBtnCancelUI.UIData> ();
								{

								}
								this.data.btn.v = btnCancelUIData;
							} else {
								this.data.btn.v = null;
							}
						}
					} else {
						// Debug.LogError ("requestNewEliminationRoundStateAsk null: " + this);
					}
					// txt
					{
						if (lbTitle != null) {
							lbTitle.text = txtTitle.get ("Request New Elimination Round: State Ask");
						} else {
							Debug.LogError ("lbTitle null: " + this);
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

		public WhoCanAskAdapter whoCanAskAdapterPrefab;
		public Transform whoCanAskAdapterContainer;

		public RequestNewEliminationRoundAskBtnAcceptUI btnAcceptPrefab;
		public RequestNewEliminationRoundAskBtnCancelUI btnCancelPrefab;
		public Transform btnContainer;

		public override void onAddCallBack<T> (T data)
		{
			if (data is UIData) {
				UIData uiData = data as UIData;
				// Setting
				Setting.get().addCallBack(this);
				// Child
				{
					uiData.requestNewEliminationRoundStateAsk.allAddCallBack (this);
					uiData.whoCanAskAdapter.allAddCallBack (this);
					uiData.btn.allAddCallBack (this);
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
				// RequestNewEliminationRoundStateAsk
				{
					if (data is RequestNewEliminationRoundStateAsk) {
						RequestNewEliminationRoundStateAsk requestNewEliminationRoundStateAsk = data as RequestNewEliminationRoundStateAsk;
						// Reset
						{
							if (this.data != null) {
								this.data.reset ();
							} else {
								Debug.LogError ("data null: " + this);
							}
						}
						// Child
						{
							requestNewEliminationRoundStateAsk.whoCanAsks.allAddCallBack (this);
						}
						dirty = true;
						return;
					}
					// Child
					{
						if (data is Human) {
							dirty = true;
							return;
						}
					}
				}
				if (data is WhoCanAskAdapter.UIData) {
					WhoCanAskAdapter.UIData whoCanAskAdapterUIData = data as WhoCanAskAdapter.UIData;
					// UI
					{
						UIUtils.Instantiate (whoCanAskAdapterUIData, whoCanAskAdapterPrefab, whoCanAskAdapterContainer);
					}
					dirty = true;
					return;
				}
				if (data is UIData.Btn) {
					UIData.Btn btn = data as UIData.Btn;
					// UI
					{
						switch (btn.getType ()) {
						case UIData.Btn.Type.Accept:
							{
								RequestNewEliminationRoundAskBtnAcceptUI.UIData btnAcceptUIData = btn as RequestNewEliminationRoundAskBtnAcceptUI.UIData;
								UIUtils.Instantiate (btnAcceptUIData, btnAcceptPrefab, btnContainer);
							}
							break;
						case UIData.Btn.Type.Cancel:
							{
								RequestNewEliminationRoundAskBtnCancelUI.UIData btnCancelUIData = btn as RequestNewEliminationRoundAskBtnCancelUI.UIData;
								UIUtils.Instantiate (btnCancelUIData, btnCancelPrefab, btnContainer);
							}
							break;
						default:
							Debug.LogError ("unknown type: " + btn.getType () + "; " + this);
							break;
						}
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
					uiData.requestNewEliminationRoundStateAsk.allRemoveCallBack (this);
					uiData.whoCanAskAdapter.allRemoveCallBack (this);
					uiData.btn.allRemoveCallBack (this);
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
				// RequestNewEliminationRoundStateAsk
				{
					if (data is RequestNewEliminationRoundStateAsk) {
						RequestNewEliminationRoundStateAsk requestNewEliminationRoundStateAsk = data as RequestNewEliminationRoundStateAsk;
						// Child
						{
							requestNewEliminationRoundStateAsk.whoCanAsks.allRemoveCallBack (this);
						}
						return;
					}
					// Child
					{
						if (data is Human) {
							return;
						}
					}
				}
				if (data is WhoCanAskAdapter.UIData) {
					WhoCanAskAdapter.UIData whoCanAskAdapterUIData = data as WhoCanAskAdapter.UIData;
					// UI
					{
						whoCanAskAdapterUIData.removeCallBackAndDestroy (typeof(WhoCanAskAdapter));
					}
					return;
				}
				if (data is UIData.Btn) {
					UIData.Btn btn = data as UIData.Btn;
					// UI
					{
						switch (btn.getType ()) {
						case UIData.Btn.Type.Accept:
							{
								RequestNewEliminationRoundAskBtnAcceptUI.UIData btnAcceptUIData = btn as RequestNewEliminationRoundAskBtnAcceptUI.UIData;
								btnAcceptUIData.removeCallBackAndDestroy (typeof(RequestNewEliminationRoundAskBtnAcceptUI));
							}
							break;
						case UIData.Btn.Type.Cancel:
							{
								RequestNewEliminationRoundAskBtnCancelUI.UIData btnCancelUIData = btn as RequestNewEliminationRoundAskBtnCancelUI.UIData;
								btnCancelUIData.removeCallBackAndDestroy (typeof(RequestNewEliminationRoundAskBtnCancelUI));
							}
							break;
						default:
							Debug.LogError ("unknown type: " + btn.getType () + "; " + this);
							break;
						}
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
				case UIData.Property.requestNewEliminationRoundStateAsk:
					{
						ValueChangeUtils.replaceCallBack (this, syncs);
						dirty = true;
					}
					break;
				case UIData.Property.whoCanAskAdapter:
					{
						ValueChangeUtils.replaceCallBack (this, syncs);
						dirty = true;
					}
					break;
				case UIData.Property.btn:
					{
						ValueChangeUtils.replaceCallBack (this, syncs);
						dirty = true;
					}
					break;
				case UIData.Property.visibility:
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
				// RequestNewEliminationRoundStateAsk
				{
					if (wrapProperty.p is RequestNewEliminationRoundStateAsk) {
						switch ((RequestNewEliminationRoundStateAsk.Property)wrapProperty.n) {
						case RequestNewEliminationRoundStateAsk.Property.whoCanAsks:
							{
								ValueChangeUtils.replaceCallBack (this, syncs);
								dirty = true;
							}
							break;
						case RequestNewEliminationRoundStateAsk.Property.accepts:
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
						if (wrapProperty.p is Human) {
							switch ((Human.Property)wrapProperty.n) {
							case Human.Property.playerId:
								dirty = true;
								break;
							case Human.Property.account:
								break;
							case Human.Property.state:
								break;
							case Human.Property.email:
								break;
							case Human.Property.phoneNumber:
								break;
							case Human.Property.status:
								break;
							case Human.Property.birthday:
								break;
							case Human.Property.sex:
								break;
							case Human.Property.connection:
								break;
							case Human.Property.ban:
								break;
							default:
								Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
								break;
							}
							return;
						}
					}
				}
				if (wrapProperty.p is WhoCanAskAdapter.UIData) {
					return;
				}
				if (wrapProperty.p is UIData.Btn) {
					return;
				}
			}
			Debug.LogError ("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
		}

		#endregion

		public void onClickBtnVisibility()
		{
			if (this.data != null) {
				switch (this.data.visibility.v) {
				case UIData.Visibility.Show:
					this.data.visibility.v = UIData.Visibility.Hide;
					break;
				case UIData.Visibility.Hide:
					this.data.visibility.v = UIData.Visibility.Show;
					break;
				default:
					Debug.LogError ("unknown visibility: " + this.data.visibility.v + "; " + this);
					break;
				}
			} else {
				Debug.LogError ("data null: " + this);
			}
		}

	}
}