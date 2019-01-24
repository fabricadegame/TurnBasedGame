﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using AdvancedCoroutines;
using Foundation.Tasks;
using GameManager.Match;

namespace GameManager.ContestManager
{
	public class RequestNewContestManagerAskBtnCancelUI : UIBehavior<RequestNewContestManagerAskBtnCancelUI.UIData>
	{

		#region UIData

		public class UIData : RequestNewContestManagerStateAskUI.UIData.Btn
		{

			#region state

			public enum State
			{
				None,
				Request,
				Wait
			}

			public VP<State> state;

			#endregion

			#region Constructor

			public enum Property
			{
				state
			}

			public UIData() : base()
			{
				this.state = new VP<State>(this, (byte)Property.state, State.None);
			}

			#endregion

			public override Type getType ()
			{
				return Type.Cancel;
			}

			public void reset()
			{
				this.state.v = State.None;
			}

		}

		#endregion

		#region Refresh

		#region txt

		public static readonly TxtLanguage txtCancel = new TxtLanguage ();
		public static readonly TxtLanguage txtCancelCancel = new TxtLanguage();
		public static readonly TxtLanguage txtCanceling = new TxtLanguage ();

		static RequestNewContestManagerAskBtnCancelUI()
		{
			txtCancel.add (Language.Type.vi, "Huỷ Bỏ");
			txtCancelCancel.add (Language.Type.vi, "Huỷ huỷ bỏ?");
			txtCanceling.add (Language.Type.vi, "Đang huỷ bỏ");
		}

		#endregion

		public Button btnCancel;
		public Text tvCancel;

		public override void refresh ()
		{
			if (dirty) {
				dirty = false;
				if (this.data != null) {
					// Task
					{
						switch (this.data.state.v) {
						case UIData.State.None:
							{
								destroyRoutine (wait);
							}
							break;
						case UIData.State.Request:
							{
								destroyRoutine (wait);
								// request
								{
									RequestNewContestManagerStateAskUI.UIData requestNewContestManagerStateAskUIData = this.data.findDataInParent<RequestNewContestManagerStateAskUI.UIData> ();
									if (requestNewContestManagerStateAskUIData != null) {
										RequestNewContestManagerStateAsk requestNewContestManagerStateAsk = requestNewContestManagerStateAskUIData.requestNewContestManagerStateAsk.v.data;
										if (requestNewContestManagerStateAsk != null) {
											if (Server.IsServerOnline (requestNewContestManagerStateAsk)) {
												requestNewContestManagerStateAsk.requestCancel (Server.getProfileUserId (requestNewContestManagerStateAsk));
												this.data.state.v = UIData.State.Wait;
											} else {
												Debug.LogError ("server not online: " + this);
											}
										} else {
											Debug.LogError ("requestNewContestManagerStateAsk null: " + this);
										}
									} else {
										Debug.LogError ("requestNewContestManagerStateAskUIData null: " + this);
									}
								}
							}
							break;
						case UIData.State.Wait:
							{
								RequestNewContestManagerStateAskUI.UIData requestNewContestManagerStateAskUIData = this.data.findDataInParent<RequestNewContestManagerStateAskUI.UIData> ();
								if (requestNewContestManagerStateAskUIData != null) {
									RequestNewContestManagerStateAsk requestNewContestManagerStateAsk = requestNewContestManagerStateAskUIData.requestNewContestManagerStateAsk.v.data;
									if (requestNewContestManagerStateAsk != null) {
										if (Server.IsServerOnline (requestNewContestManagerStateAsk)) {
											startRoutine (ref this.wait, TaskWait ());
										} else {
											Debug.LogError ("server not online: " + this);
											destroyRoutine (wait);
										}
									} else {
										Debug.LogError ("requestNewContestManagerStateAsk null: " + this);
									}
								} else {
									Debug.LogError ("requestNewContestManagerStateAskUIData null: " + this);
								}
							}
							break;
						default:
							Debug.LogError ("unknown state: " + this.data.state.v + "; " + this);
							break;
						}
					}
					// UI
					{
						if (btnCancel != null && tvCancel != null) {
							switch (this.data.state.v) {
							case UIData.State.None:
								{
									btnCancel.enabled = true;
									tvCancel.text = txtCancel.get ("Cancel");
								}
								break;
							case UIData.State.Request:
								{
									btnCancel.enabled = true;
									tvCancel.text = txtCancelCancel.get ("Cancel Cancel?");
								}
								break;
							case UIData.State.Wait:
								{
									btnCancel.enabled = false;
									tvCancel.text = txtCanceling.get ("Cancelling...");
								}
								break;
							default:
								Debug.LogError ("unknown state: " + this.data.state.v + "; " + this);
								break;
							}
						} else {
							Debug.LogError ("btnCancel, tvCancel null: " + this);
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

		#region Task wait

		private Routine wait;

		public IEnumerator TaskWait()
		{
			if (this.data != null) {
				yield return new Wait (Global.WaitSendTime);
				this.data.state.v = UIData.State.None;
				Toast.showMessage ("request error");
				Debug.LogError ("request error: " + this);
			} else {
				Debug.LogError ("data null: " + this);
			}
		}

		public override List<Routine> getRoutineList ()
		{
			List<Routine> ret = new List<Routine> ();
			{
				ret.Add (wait);
			}
			return ret;
		}

		#endregion

		#region implement callBacks

		private RequestNewContestManagerStateAskUI.UIData requestNewContestManagerStateAskUIData = null;
		private Server server = null;

		public override void onAddCallBack<T> (T data)
		{
			if (data is UIData) {
				UIData uiData = data as UIData;
				// Setting
				Setting.get().addCallBack(this);
				// Parent
				{
					DataUtils.addParentCallBack (uiData, this, ref this.requestNewContestManagerStateAskUIData);
				}
				dirty = true;
				return;
			}
			// Setting
			if (data is Setting) {
				dirty = true;
				return;
			}
			// Parent
			{
				if (data is RequestNewContestManagerStateAskUI.UIData) {
					RequestNewContestManagerStateAskUI.UIData requestNewContestManagerStateAskUIData = data as RequestNewContestManagerStateAskUI.UIData;
					// Child
					{
						requestNewContestManagerStateAskUIData.requestNewContestManagerStateAsk.allAddCallBack (this);
					}
					dirty = true;
					return;
				}
				// Child
				{
					if (data is RequestNewContestManagerStateAsk) {
						RequestNewContestManagerStateAsk requestNewContestManagerStateAsk = data as RequestNewContestManagerStateAsk;
						// Reset
						{
							if (this.data != null) {
								this.data.reset ();
							} else {
								Debug.LogError ("data null: " + this);
							}
						}
						// Parent
						{
							DataUtils.addParentCallBack (requestNewContestManagerStateAsk, this, ref this.server);
						}
						dirty = true;
						return;
					}
					// Parent
					if (data is Server) {
						dirty = true;
						return;
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
				// Parent
				{
					DataUtils.removeParentCallBack (uiData, this, ref this.requestNewContestManagerStateAskUIData);
				}
				this.setDataNull (uiData);
				return;
			}
			// Setting
			if (data is Setting) {
				return;
			}
			// Parent
			{
				if (data is RequestNewContestManagerStateAskUI.UIData) {
					RequestNewContestManagerStateAskUI.UIData requestNewContestManagerStateAskUIData = data as RequestNewContestManagerStateAskUI.UIData;
					// Child
					{
						requestNewContestManagerStateAskUIData.requestNewContestManagerStateAsk.allRemoveCallBack (this);
					}
					return;
				}
				// Child
				{
					if (data is RequestNewContestManagerStateAsk) {
						RequestNewContestManagerStateAsk requestNewContestManagerStateAsk = data as RequestNewContestManagerStateAsk;
						// Parent
						{
							DataUtils.removeParentCallBack (requestNewContestManagerStateAsk, this, ref this.server);
						}
						return;
					}
					// Parent
					if (data is Server) {
						return;
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
				case UIData.Property.state:
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
			// Parent
			{
				if (wrapProperty.p is RequestNewContestManagerStateAskUI.UIData) {
					switch ((RequestNewContestManagerStateAskUI.UIData.Property)wrapProperty.n) {
					case RequestNewContestManagerStateAskUI.UIData.Property.requestNewContestManagerStateAsk:
						{
							ValueChangeUtils.replaceCallBack (this, syncs);
							dirty = true;
						}
						break;
					case RequestNewContestManagerStateAskUI.UIData.Property.btn:
						break;
					case RequestNewContestManagerStateAskUI.UIData.Property.visibility:
						break;
					default:
						Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
						break;
					}
					return;
				}
				// Child
				{
					if (wrapProperty.p is RequestNewContestManagerStateAsk) {
						return;
					}
					// Parent
					if (wrapProperty.p is Server) {
						Server.State.OnUpdateSyncStateChange (wrapProperty, this);
						return;
					}
				}
			}
			Debug.LogError ("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
		}

		#endregion

		public void onClickBtnCancel()
		{
			if (this.data != null) {
				switch (this.data.state.v) {
				case UIData.State.None:
					this.data.state.v = UIData.State.Request;
					break;
				case UIData.State.Request:
					this.data.state.v = UIData.State.None;
					break;
				case UIData.State.Wait:
					Debug.LogError ("you are requesting: " + this);
					break;
				default:
					Debug.LogError ("unknown state: " + this.data.state.v + "; " + this);
					break;
				}
			} else {
				Debug.LogError ("data null: " + this);
			}
		}

	}
}