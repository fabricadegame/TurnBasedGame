﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace TimeControl.HourGlass
{
	public class TimeControlHourGlassUI : UIBehavior<TimeControlHourGlassUI.UIData>
	{

		#region UIData

		public class UIData : TimeControlUI.UIData.Sub
		{

			public VP<EditData<TimeControlHourGlass>> editTimeControlHourGlass;

			#region initTime

			public VP<RequestChangeFloatUI.UIData> initTime;

			public void makeRequestChangeInitTime (RequestChangeUpdate<float>.UpdateData update, float newInitTime)
			{
				// Find
				TimeControlHourGlass timeControlHourGlass = null;
				{
					EditData<TimeControlHourGlass> editTimeControlHourGlass = this.editTimeControlHourGlass.v;
					if (editTimeControlHourGlass != null) {
						timeControlHourGlass = editTimeControlHourGlass.show.v.data;
					} else {
						Debug.LogError ("editTimeControlHourGlass null: " + this);
					}
				}
				// Process
				if (timeControlHourGlass != null) {
					timeControlHourGlass.requestChangeInitTime (Server.getProfileUserId(timeControlHourGlass), newInitTime);
				} else {
					Debug.LogError ("timeControlHourGlass null: " + this);
				}
			}

			#endregion

			#region lagCompensation

			public VP<RequestChangeFloatUI.UIData> lagCompensation;

			public void makeRequestChangeLagCompensation (RequestChangeUpdate<float>.UpdateData update, float newLagCompensation)
			{
				// Find
				TimeControlHourGlass timeControlHourGlass = null;
				{
					EditData<TimeControlHourGlass> editTimeControlHourGlass = this.editTimeControlHourGlass.v;
					if (editTimeControlHourGlass != null) {
						timeControlHourGlass = editTimeControlHourGlass.show.v.data;
					} else {
						Debug.LogError ("editTimeControlHourGlass null: " + this);
					}
				}
				// Process
				if (timeControlHourGlass != null) {
					timeControlHourGlass.requestChangeLagCompensation (Server.getProfileUserId(timeControlHourGlass), newLagCompensation);
				} else {
					Debug.LogError ("timeControlHourGlass null: " + this);
				}
			}

			#endregion

			#region Constructor

			public enum Property
			{
				editTimeControlHourGlass,
				initTime,
				lagCompensation
			}

			public UIData() : base()
			{
				this.editTimeControlHourGlass = new VP<EditData<TimeControlHourGlass>>(this, (byte)Property.editTimeControlHourGlass, new EditData<TimeControlHourGlass>());
				// initTime
				{
					this.initTime = new VP<RequestChangeFloatUI.UIData>(this, (byte)Property.initTime, new RequestChangeFloatUI.UIData());
					// event
					this.initTime.v.updateData.v.request.v = makeRequestChangeInitTime;
				}
				// lagCompensation
				{
					this.lagCompensation = new VP<RequestChangeFloatUI.UIData>(this, (byte)Property.lagCompensation, new RequestChangeFloatUI.UIData());
					// event
					this.lagCompensation.v.updateData.v.request.v = makeRequestChangeLagCompensation;
				}
			}

			#endregion

			public override TimeControl.Sub.Type getType ()
			{
				return TimeControl.Sub.Type.HourGlass;
			}

			public override bool processEvent (Event e)
			{
				bool isProcess = false;
				{

				}
				return isProcess;
			}

		}

		#endregion

		#region Refresh

		#region txt

		public Text lbTitle;
		public static readonly TxtLanguage txtTitle = new TxtLanguage();

		public Text lbInitTime;
		public static readonly TxtLanguage txtInitTime = new TxtLanguage();

		public Text lbLagCompensation;
		public static readonly TxtLanguage txtLagCompensation = new TxtLanguage();

		static TimeControlHourGlassUI()
		{
			txtTitle.add (Language.Type.vi, "Điểu Khiển Thời Gian Đồng Hồ Cát");
			txtInitTime.add (Language.Type.vi, "Thời gian ban đầu");
			txtLagCompensation.add (Language.Type.vi, "Bồi thường lag");
		}

		#endregion

		private bool needReset = true;
		public GameObject differentIndicator;

		public override void refresh ()
		{
			if (dirty) {
				dirty = false;
				if (this.data != null) {
					EditData<TimeControlHourGlass> editTimeControlHourGlass = this.data.editTimeControlHourGlass.v;
					if (editTimeControlHourGlass != null) {
						editTimeControlHourGlass.update ();
						// get show
						TimeControlHourGlass show = editTimeControlHourGlass.show.v.data;
						TimeControlHourGlass compare = editTimeControlHourGlass.compare.v.data;
						if (show != null) {
							// differentIndicator
							if (differentIndicator != null) {
								bool isDifferent = false;
								{
									if (editTimeControlHourGlass.compareOtherType.v.data != null) {
										if (editTimeControlHourGlass.compareOtherType.v.data.GetType () != show.GetType ()) {
											isDifferent = true;
										}
									}
								}
								differentIndicator.SetActive (isDifferent);
							} else {
								Debug.LogError ("differentIndicator null: " + this);
							}
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
									Debug.LogError ("server null: " + this);
								}
							}
							// set origin
							{
								// initTime
								{
									RequestChangeFloatUI.UIData initTime = this.data.initTime.v;
									if (initTime != null) {
										// update
										RequestChangeUpdate<float>.UpdateData updateData = initTime.updateData.v;
										if (updateData != null) {
											updateData.origin.v = show.initTime.v;
											updateData.canRequestChange.v = editTimeControlHourGlass.canEdit.v;
											updateData.serverState.v = serverState;
										} else {
											Debug.LogError ("updateData null: " + this);
										}
										// compare
										{
											if (compare != null) {
												initTime.showDifferent.v = true;
												initTime.compare.v = compare.initTime.v;
											} else {
												initTime.showDifferent.v = false;
											}
										}
									} else {
										Debug.LogError ("initTime null: " + this);
									}
								}
								// lagCompensation
								{
									RequestChangeFloatUI.UIData lagCompensation = this.data.lagCompensation.v;
									if (lagCompensation != null) {
										// update
										RequestChangeUpdate<float>.UpdateData updateData = lagCompensation.updateData.v;
										if (updateData != null) {
											updateData.origin.v = show.lagCompensation.v;
											updateData.canRequestChange.v = editTimeControlHourGlass.canEdit.v;
											updateData.serverState.v = serverState;
										} else {
											Debug.LogError ("updateData null: " + this);
										}
										// compare
										{
											if (compare != null) {
												lagCompensation.showDifferent.v = true;
												lagCompensation.compare.v = compare.lagCompensation.v;
											} else {
												lagCompensation.showDifferent.v = false;
											}
										}
									} else {
										Debug.LogError ("lagCompensation null: " + this);
									}
								}
							}
							// reset?
							if (needReset) {
								needReset = false;
								// initTime
								{
									RequestChangeFloatUI.UIData initTime = this.data.initTime.v;
									if (initTime != null) {
										// update
										RequestChangeUpdate<float>.UpdateData updateData = initTime.updateData.v;
										if (updateData != null) {
											updateData.current.v = show.initTime.v;
											updateData.changeState.v = Data.ChangeState.None;
										} else {
											Debug.LogError ("updateData null: " + this);
										}
									} else {
										Debug.LogError ("initTime null: " + this);
									}
								}
								// lagCompensation
								{
									RequestChangeFloatUI.UIData lagCompensation = this.data.lagCompensation.v;
									if (lagCompensation != null) {
										// update
										RequestChangeUpdate<float>.UpdateData updateData = lagCompensation.updateData.v;
										if (updateData != null) {
											updateData.current.v = show.lagCompensation.v;
											updateData.changeState.v = Data.ChangeState.None;
										} else {
											Debug.LogError ("updateData null: " + this);
										}
									} else {
										Debug.LogError ("lagCompensation null: " + this);
									}
								}
							}
						} else {
							Debug.LogError ("show null: " + this);
						}
					} else {
						Debug.LogError ("editTimeControlHourGlass null: " + this);
					}
					// txt
					{
						if (lbTitle != null) {
							lbTitle.text = txtTitle.get ("Time Control Hourglass");
						} else {
							Debug.LogError ("lbTitle null: " + this);
						}
						if (lbInitTime != null) {
							lbInitTime.text = txtInitTime.get ("Init time");
						} else {
							Debug.LogError ("lbInitTime null: " + this);
						}
						if (lbLagCompensation != null) {
							lbLagCompensation.text = txtLagCompensation.get ("Lag compensation");
						} else {
							Debug.LogError ("lbLagCompensation null: " + this);
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

		public RequestChangeFloatUI requestFloatPrefab;

		public Transform initTimeContainer;
		public Transform lagCompensationContainer;

		private Server server = null;

		public override void onAddCallBack<T> (T data)
		{
			if (data is UIData) {
				UIData uiData = data as UIData;
				// Setting
				Setting.get().addCallBack(this);
				// Child
				{
					uiData.editTimeControlHourGlass.allAddCallBack (this);
					uiData.initTime.allAddCallBack (this);
					uiData.lagCompensation.allAddCallBack (this);
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
				// editTimeControlHourGlass
				{
					if (data is EditData<TimeControlHourGlass>) {
						EditData<TimeControlHourGlass> editTimeControlHourGlass = data as EditData<TimeControlHourGlass>;
						// Child
						{
							editTimeControlHourGlass.show.allAddCallBack (this);
							editTimeControlHourGlass.compare.allAddCallBack (this);
						}
						dirty = true;
						return;
					}
					// Child
					{
						if (data is TimeControlHourGlass) {
							TimeControlHourGlass timeControlHourGlass = data as TimeControlHourGlass;
							// Parent
							{
								DataUtils.addParentCallBack (timeControlHourGlass, this, ref this.server);
							}
							dirty = true;
							needReset = true;
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
				if (data is RequestChangeFloatUI.UIData) {
					RequestChangeFloatUI.UIData requestChange = data as RequestChangeFloatUI.UIData;
					// UI
					{
						WrapProperty wrapProperty = requestChange.p;
						if (wrapProperty != null) {
							switch ((UIData.Property)wrapProperty.n) {
							case UIData.Property.initTime:
								{
									UIUtils.Instantiate (requestChange, requestFloatPrefab, initTimeContainer);
								}
								break;
							case UIData.Property.lagCompensation:
								{
									UIUtils.Instantiate (requestChange, requestFloatPrefab, lagCompensationContainer);
								}
								break;
							default:
								Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
								break;
							}
						} else {
							Debug.LogError ("wrapProperty null: " + this);
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
					uiData.editTimeControlHourGlass.allRemoveCallBack (this);
					uiData.initTime.allRemoveCallBack (this);
					uiData.lagCompensation.allRemoveCallBack (this);
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
				// editTimeControlHourGlass
				{
					if (data is EditData<TimeControlHourGlass>) {
						EditData<TimeControlHourGlass> editTimeControlHourGlass = data as EditData<TimeControlHourGlass>;
						// Child
						{
							editTimeControlHourGlass.show.allRemoveCallBack (this);
							editTimeControlHourGlass.compare.allRemoveCallBack (this);
						}
						return;
					}
					// Child
					{
						if (data is TimeControlHourGlass) {
							TimeControlHourGlass timeControlHourGlass = data as TimeControlHourGlass;
							// Parent
							{
								DataUtils.removeParentCallBack (timeControlHourGlass, this, ref this.server);
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
				if (data is RequestChangeFloatUI.UIData) {
					RequestChangeFloatUI.UIData requestChange = data as RequestChangeFloatUI.UIData;
					// UI
					{
						requestChange.removeCallBackAndDestroy (typeof(RequestChangeFloatUI));
					}
					return;
				}
			}
			Debug.LogError ("Don't process: " + data + "; " + this);
		}

		public override void onUpdateSync<T> (WrapProperty wrapProperty, List<Sync<T>> syncs)
		{
			if(WrapProperty.checkError(wrapProperty)){
				return;
			}
			if (wrapProperty.p is UIData) {
				switch ((UIData.Property)wrapProperty.n) {
				case UIData.Property.editTimeControlHourGlass:
					{
						ValueChangeUtils.replaceCallBack (this, syncs);
						dirty = true;
					}
					break;
				case UIData.Property.initTime:
					{
						ValueChangeUtils.replaceCallBack (this, syncs);
						dirty = true;
					}
					break;
				case UIData.Property.lagCompensation:
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
				// editTimeControlHourGlass
				{
					if (wrapProperty.p is EditData<TimeControlHourGlass>) {
						switch ((EditData<TimeControlHourGlass>.Property)wrapProperty.n) {
						case EditData<TimeControlHourGlass>.Property.origin:
							dirty = true;
							break;
						case EditData<TimeControlHourGlass>.Property.show:
							{
								ValueChangeUtils.replaceCallBack (this, syncs);
								dirty = true;
							}
							break;
						case EditData<TimeControlHourGlass>.Property.compare:
							{
								ValueChangeUtils.replaceCallBack (this, syncs);
								dirty = true;
							}
							break;
						case EditData<TimeControlHourGlass>.Property.compareOtherType:
							dirty = true;
							break;
						case EditData<TimeControlHourGlass>.Property.canEdit:
							dirty = true;
							break;
						case EditData<TimeControlHourGlass>.Property.editType:
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
						if (wrapProperty.p is TimeControlHourGlass) {
							switch ((TimeControlHourGlass.Property)wrapProperty.n) {
							case TimeControlHourGlass.Property.initTime:
								dirty = true;
								break;
							case TimeControlHourGlass.Property.lagCompensation:
								dirty = true;
								break;
							case TimeControlHourGlass.Property.playerTimes:
								break;
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
				if (wrapProperty.p is RequestChangeFloatUI.UIData) {
					return;
				}
			}
			Debug.LogError ("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
		}

		#endregion

	}
}