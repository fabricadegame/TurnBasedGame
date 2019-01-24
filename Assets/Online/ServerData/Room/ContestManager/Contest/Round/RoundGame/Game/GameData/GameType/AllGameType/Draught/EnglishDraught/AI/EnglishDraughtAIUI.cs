﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace EnglishDraught
{
	public class EnglishDraughtAIUI : UIBehavior<EnglishDraughtAIUI.UIData>
	{

		#region UIData

		public class UIData : AIUI.UIData.Sub
		{

			public VP<EditData<EnglishDraughtAI>> editAI;

			#region threeMoveRandom

			public VP<RequestChangeBoolUI.UIData> threeMoveRandom;

			public void makeRequestChangeThreeMoveRandom (RequestChangeUpdate<bool>.UpdateData update, bool newThreeMoveRandom)
			{
				// Find englishDraughtAI
				EnglishDraughtAI englishDraughtAI = null;
				{
					EditData<EnglishDraughtAI> editEnglishDraughtAI = this.editAI.v;
					if (editEnglishDraughtAI != null) {
						englishDraughtAI = editEnglishDraughtAI.show.v.data;
					} else {
						Debug.LogError ("editEnglishDraughtAI null: " + this);
					}
				}
				// Process
				if (englishDraughtAI != null) {
					englishDraughtAI.requestChangeThreeMoveRandom (Server.getProfileUserId(englishDraughtAI), newThreeMoveRandom);
				} else {
					Debug.LogError ("englishDraughtAI null: " + this);
				}
			}

			#endregion

			#region fMaxSeconds

			public VP<RequestChangeFloatUI.UIData> fMaxSeconds;

			public void makeRequestChangeFMaxSeconds (RequestChangeUpdate<float>.UpdateData update, float newFMaxSeconds)
			{
				// Find englishDraughtAI
				EnglishDraughtAI englishDraughtAI = null;
				{
					EditData<EnglishDraughtAI> editEnglishDraughtAI = this.editAI.v;
					if (editEnglishDraughtAI != null) {
						englishDraughtAI = editEnglishDraughtAI.show.v.data;
					} else {
						Debug.LogError ("editEnglishDraughtAI null: " + this);
					}
				}
				// Process
				if (englishDraughtAI != null) {
					englishDraughtAI.requestChangeFMaxSeconds (Server.getProfileUserId(englishDraughtAI), newFMaxSeconds);
				} else {
					Debug.LogError ("englishDraughtAI null: " + this);
				}
			}

			#endregion

			#region g_MaxDepth

			public VP<RequestChangeIntUI.UIData> g_MaxDepth;

			public void makeRequestChangeGMaxDepth (RequestChangeUpdate<int>.UpdateData update, int newGMaxDepth)
			{
				// Find englishDraughtAI
				EnglishDraughtAI englishDraughtAI = null;
				{
					EditData<EnglishDraughtAI> editEnglishDraughtAI = this.editAI.v;
					if (editEnglishDraughtAI != null) {
						englishDraughtAI = editEnglishDraughtAI.show.v.data;
					} else {
						Debug.LogError ("editEnglishDraughtAI null: " + this);
					}
				}
				// Process
				if (englishDraughtAI != null) {
					englishDraughtAI.requestChangeGMaxDepth (Server.getProfileUserId(englishDraughtAI), newGMaxDepth);
				} else {
					Debug.LogError ("englishDraughtAI null: " + this);
				}
			}

			#endregion

			#region pickBestMove

			public VP<RequestChangeIntUI.UIData> pickBestMove;

			public void makeRequestChangePickBestMove (RequestChangeUpdate<int>.UpdateData update, int newPickBestMove)
			{
				// Find englishDraughtAI
				EnglishDraughtAI englishDraughtAI = null;
				{
					EditData<EnglishDraughtAI> editEnglishDraughtAI = this.editAI.v;
					if (editEnglishDraughtAI != null) {
						englishDraughtAI = editEnglishDraughtAI.show.v.data;
					} else {
						Debug.LogError ("editEnglishDraughtAI null: " + this);
					}
				}
				// Process
				if (englishDraughtAI != null) {
					englishDraughtAI.requestChangePickBestMove (Server.getProfileUserId(englishDraughtAI), newPickBestMove);
				} else {
					Debug.LogError ("englishDraughtAI null: " + this);
				}
			}

			#endregion

			#region Constructor

			public enum Property
			{
				editAI,
				threeMoveRandom,
				fMaxSeconds,
				g_MaxDepth,
				pickBestMove
			}

			public UIData() : base()
			{
				this.editAI = new VP<EditData<EnglishDraughtAI>>(this, (byte)Property.editAI, new EditData<EnglishDraughtAI>());
				// threeMoveRandom
				{
					this.threeMoveRandom = new VP<RequestChangeBoolUI.UIData>(this, (byte)Property.threeMoveRandom, new RequestChangeBoolUI.UIData());
					// event
					this.threeMoveRandom.v.updateData.v.request.v = makeRequestChangeThreeMoveRandom;
				}
				// fMaxSeconds
				{
					this.fMaxSeconds = new VP<RequestChangeFloatUI.UIData>(this, (byte)Property.fMaxSeconds, new RequestChangeFloatUI.UIData());
					// event
					this.fMaxSeconds.v.updateData.v.request.v = makeRequestChangeFMaxSeconds;
				}
				// g_MaxDepth
				{
					this.g_MaxDepth = new VP<RequestChangeIntUI.UIData>(this, (byte)Property.g_MaxDepth, new RequestChangeIntUI.UIData());
					// have limit
					{
						IntLimit.Have have = new IntLimit.Have();
						{
							have.uid = this.g_MaxDepth.v.limit.makeId();
							have.min.v = 0;
							have.max.v = 30;
						}
						this.g_MaxDepth.v.limit.v = have;
					}
					// event
					this.g_MaxDepth.v.updateData.v.request.v = makeRequestChangeGMaxDepth;
				}
				// pickBestMove
				{
					this.pickBestMove = new VP<RequestChangeIntUI.UIData>(this, (byte)Property.pickBestMove, new RequestChangeIntUI.UIData());
					// have limit
					{
						IntLimit.Have have = new IntLimit.Have();
						{
							have.uid = this.pickBestMove.v.limit.makeId();
							have.min.v = 0;
							have.max.v = 100;
						}
						this.pickBestMove.v.limit.v = have;
					}
					// event
					this.pickBestMove.v.updateData.v.request.v = makeRequestChangePickBestMove;
				}
			}

			#endregion

			public override GameType.Type getType ()
			{
				return GameType.Type.EnglishDraught;
			}

		}

		#endregion

		#region Refresh

		#region txt

		public Text lbTitle;
		public static readonly TxtLanguage txtTitle = new TxtLanguage();

		public Text lbThreeMoveRandom;
		public static readonly TxtLanguage txtThreeMoveRandom = new TxtLanguage ();

		public Text lbMaxSeconds;
		public static readonly TxtLanguage txtMaxSeconds = new TxtLanguage ();

		public Text lbMaxDepth;
		public static readonly TxtLanguage txtMaxDepth = new TxtLanguage();

		public Text lbPickBestMove;
		public static readonly TxtLanguage txtPickBestMove = new TxtLanguage ();

		static EnglishDraughtAIUI()
		{
			txtTitle.add (Language.Type.vi, "Cờ Đam Kiểu Anh AI");
			txtThreeMoveRandom.add (Language.Type.vi, "Ba nước đầu ngẫu nhiên");
			txtMaxSeconds.add (Language.Type.vi, "Số giây tối đa");
			txtMaxDepth.add (Language.Type.vi, "Độ sâu tối đa");
			txtPickBestMove.add (Language.Type.vi, "Chọn nước đi tốt nhất");
		}

		#endregion

		private bool needReset = true;
		public GameObject differentIndicator;

		public override void refresh ()
		{
			if (dirty) {
				dirty = false;
				if (this.data != null) {
					EditData<EnglishDraughtAI> editEnglishDraughtAI = this.data.editAI.v;
					if (editEnglishDraughtAI != null) {
						editEnglishDraughtAI.update ();
						// get show
						EnglishDraughtAI show = editEnglishDraughtAI.show.v.data;
						EnglishDraughtAI compare = editEnglishDraughtAI.compare.v.data;
						if (show != null) {
							// differentIndicator
							if (differentIndicator != null) {
								bool isDifferent = false;
								{
									if (editEnglishDraughtAI.compareOtherType.v.data != null) {
										if (editEnglishDraughtAI.compareOtherType.v.data.GetType () != show.GetType ()) {
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
								// threeMoveRandom
								{
									RequestChangeBoolUI.UIData threeMoveRandom = this.data.threeMoveRandom.v;
									if (threeMoveRandom != null) {
										// update
										RequestChangeUpdate<bool>.UpdateData updateData = threeMoveRandom.updateData.v;
										if (updateData != null) {
											updateData.origin.v = show.threeMoveRandom.v;
											updateData.canRequestChange.v = editEnglishDraughtAI.canEdit.v;
											updateData.serverState.v = serverState;
										} else {
											Debug.LogError ("updateData null: " + this);
										}
										// compare
										{
											if (compare != null) {
												threeMoveRandom.showDifferent.v = true;
												threeMoveRandom.compare.v = compare.threeMoveRandom.v;
											} else {
												threeMoveRandom.showDifferent.v = false;
											}
										}
									} else {
										Debug.LogError ("threeMoveRandom null: " + this);
									}
								}
								// fMaxSeconds
								{
									RequestChangeFloatUI.UIData fMaxSeconds = this.data.fMaxSeconds.v;
									if (fMaxSeconds != null) {
										// update
										RequestChangeUpdate<float>.UpdateData updateData = fMaxSeconds.updateData.v;
										if (updateData != null) {
											updateData.origin.v = show.fMaxSeconds.v;
											updateData.canRequestChange.v = editEnglishDraughtAI.canEdit.v;
											updateData.serverState.v = serverState;
										} else {
											Debug.LogError ("updateData null: " + this);
										}
										// compare
										{
											if (compare != null) {
												fMaxSeconds.showDifferent.v = true;
												fMaxSeconds.compare.v = compare.fMaxSeconds.v;
											} else {
												fMaxSeconds.showDifferent.v = false;
											}
										}
									} else {
										Debug.LogError ("fMaxSeconds null: " + this);
									}
								}
								// g_MaxDepth
								{
									RequestChangeIntUI.UIData g_MaxDepth = this.data.g_MaxDepth.v;
									if (g_MaxDepth != null) {
										// update
										RequestChangeUpdate<int>.UpdateData updateData = g_MaxDepth.updateData.v;
										if (updateData != null) {
											updateData.origin.v = show.g_MaxDepth.v;
											updateData.canRequestChange.v = editEnglishDraughtAI.canEdit.v;
											updateData.serverState.v = serverState;
										} else {
											Debug.LogError ("updateData null: " + this);
										}
										// compare
										{
											if (compare != null) {
												g_MaxDepth.showDifferent.v = true;
												g_MaxDepth.compare.v = compare.g_MaxDepth.v;
											} else {
												g_MaxDepth.showDifferent.v = false;
											}
										}
									} else {
										Debug.LogError ("g_MaxDepth null: " + this);
									}
								}
								// pickBestMove
								{
									RequestChangeIntUI.UIData pickBestMove = this.data.pickBestMove.v;
									if (pickBestMove != null) {
										// update
										RequestChangeUpdate<int>.UpdateData updateData = pickBestMove.updateData.v;
										if (updateData != null) {
											updateData.origin.v = show.pickBestMove.v;
											updateData.canRequestChange.v = editEnglishDraughtAI.canEdit.v;
											updateData.serverState.v = serverState;
										} else {
											Debug.LogError ("updateData null: " + this);
										}
										// compare
										{
											if (compare != null) {
												pickBestMove.showDifferent.v = true;
												pickBestMove.compare.v = compare.pickBestMove.v;
											} else {
												pickBestMove.showDifferent.v = false;
											}
										}
									} else {
										Debug.LogError ("pickBestMove null: " + this);
									}
								}
							}
							// reset?
							if (needReset) {
								needReset = false;
								// threeMoveRandom
								{
									RequestChangeBoolUI.UIData threeMoveRandom = this.data.threeMoveRandom.v;
									if (threeMoveRandom != null) {
										// update
										RequestChangeUpdate<bool>.UpdateData updateData = threeMoveRandom.updateData.v;
										if (updateData != null) {
											updateData.current.v = show.threeMoveRandom.v;
											updateData.changeState.v = Data.ChangeState.None;
										} else {
											Debug.LogError ("updateData null: " + this);
										}
									} else {
										Debug.LogError ("threeMoveRandom null: " + this);
									}
								}
								// fMaxSeconds
								{
									RequestChangeFloatUI.UIData fMaxSeconds = this.data.fMaxSeconds.v;
									if (fMaxSeconds != null) {
										// update
										RequestChangeUpdate<float>.UpdateData updateData = fMaxSeconds.updateData.v;
										if (updateData != null) {
											updateData.current.v = show.fMaxSeconds.v;
											updateData.changeState.v = Data.ChangeState.None;
										} else {
											Debug.LogError ("updateData null: " + this);
										}
									} else {
										Debug.LogError ("fMaxSeconds null: " + this);
									}
								}
								// g_MaxDepth
								{
									RequestChangeIntUI.UIData g_MaxDepth = this.data.g_MaxDepth.v;
									if (g_MaxDepth != null) {
										// update
										RequestChangeUpdate<int>.UpdateData updateData = g_MaxDepth.updateData.v;
										if (updateData != null) {
											updateData.current.v = show.g_MaxDepth.v;
											updateData.changeState.v = Data.ChangeState.None;
										} else {
											Debug.LogError ("updateData null: " + this);
										}
									} else {
										Debug.LogError ("g_MaxDepth null: " + this);
									}
								}
								// pickBestMove
								{
									RequestChangeIntUI.UIData pickBestMove = this.data.pickBestMove.v;
									if (pickBestMove != null) {
										// update
										RequestChangeUpdate<int>.UpdateData updateData = pickBestMove.updateData.v;
										if (updateData != null) {
											updateData.current.v = show.pickBestMove.v;
											updateData.changeState.v = Data.ChangeState.None;
										} else {
											Debug.LogError ("updateData null: " + this);
										}
									} else {
										Debug.LogError ("pickBestMove null: " + this);
									}
								}
							}
						} else {
							Debug.LogError ("chessAI null: " + this);
						}
					} else {
						Debug.LogError ("editChessAI null: " + this);
					}
					// txt
					{
						if (lbTitle != null) {
							lbTitle.text = txtTitle.get ("English Draughts AI");
						} else {
							Debug.LogError ("lbTitle null: " + this);
						}
						if (lbThreeMoveRandom != null) {
							lbThreeMoveRandom.text = txtThreeMoveRandom.get ("Three moves random");
						} else {
							Debug.LogError ("lbThreeMoveRandom null: " + this);
						}
						if (lbMaxSeconds != null) {
							lbMaxSeconds.text = txtMaxSeconds.get ("Max seconds");
						} else {
							Debug.LogError ("lbMaxSeconds null: " + this);
						}
						if (lbMaxDepth != null) {
							lbMaxDepth.text = txtMaxDepth.get ("Max depth");
						} else {
							Debug.LogError ("lbMaxDepth null: " + this);
						}
						if (lbPickBestMove != null) {
							lbPickBestMove.text = txtPickBestMove.get ("Pick best move");
						} else {
							Debug.LogError ("lbPickBestMove null: " + this);
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

		public Transform threeMoveRandomContainer;
		public Transform fMaxSecondsContainer;
		public Transform g_MaxDepthContainer;
		public Transform pickBestMoveContainer;

		public RequestChangeIntUI requestIntPrefab;
		public RequestChangeBoolUI requestBoolPrefab;
		public RequestChangeFloatUI requestFloatPrefab;

		private Server server = null;

		public override void onAddCallBack<T> (T data)
		{
			if (data is UIData) {
				UIData uiData = data as UIData;
				// Setting
				Setting.get ().addCallBack (this);
				// Child
				{
					uiData.editAI.allAddCallBack (this);
					uiData.threeMoveRandom.allAddCallBack (this);
					uiData.fMaxSeconds.allAddCallBack (this);
					uiData.g_MaxDepth.allAddCallBack (this);
					uiData.pickBestMove.allAddCallBack (this);
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
				// editAI
				{
					if (data is EditData<EnglishDraughtAI>) {
						EditData<EnglishDraughtAI> editAI = data as EditData<EnglishDraughtAI>;
						// Child
						{
							editAI.show.allAddCallBack (this);
							editAI.compare.allAddCallBack (this);
						}
						dirty = true;
						return;
					}
					// Child
					{
						if (data is EnglishDraughtAI) {
							EnglishDraughtAI englishDraughtAI = data as EnglishDraughtAI;
							// Parent
							{
								DataUtils.addParentCallBack (englishDraughtAI, this, ref this.server);
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
				if (data is RequestChangeBoolUI.UIData) {
					RequestChangeBoolUI.UIData requestChange = data as RequestChangeBoolUI.UIData;
					// UI
					{
						WrapProperty wrapProperty = requestChange.p;
						if (wrapProperty != null) {
							switch ((UIData.Property)wrapProperty.n) {
							case UIData.Property.threeMoveRandom:
								{
									UIUtils.Instantiate (requestChange, requestBoolPrefab, threeMoveRandomContainer);
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
				if (data is RequestChangeFloatUI.UIData) {
					RequestChangeFloatUI.UIData requestChange = data as RequestChangeFloatUI.UIData;
					// UI
					{
						WrapProperty wrapProperty = requestChange.p;
						if (wrapProperty != null) {
							switch ((UIData.Property)wrapProperty.n) {
							case UIData.Property.fMaxSeconds:
								{
									UIUtils.Instantiate (requestChange, requestFloatPrefab, fMaxSecondsContainer);
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
				if (data is RequestChangeIntUI.UIData) {
					RequestChangeIntUI.UIData requestChange = data as RequestChangeIntUI.UIData;
					// UI
					{
						WrapProperty wrapProperty = requestChange.p;
						if (wrapProperty != null) {
							switch ((UIData.Property)wrapProperty.n) {
							case UIData.Property.g_MaxDepth:
								{
									UIUtils.Instantiate (requestChange, requestIntPrefab, g_MaxDepthContainer);
								}
								break;
							case UIData.Property.pickBestMove:
								{
									UIUtils.Instantiate (requestChange, requestIntPrefab, pickBestMoveContainer);
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
				Setting.get ().removeCallBack (this);
				// Child
				{
					uiData.editAI.allRemoveCallBack (this);
					uiData.threeMoveRandom.allRemoveCallBack (this);
					uiData.fMaxSeconds.allRemoveCallBack (this);
					uiData.g_MaxDepth.allRemoveCallBack (this);
					uiData.pickBestMove.allRemoveCallBack (this);
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
				// editAI
				{
					if (data is EditData<EnglishDraughtAI>) {
						EditData<EnglishDraughtAI> editAI = data as EditData<EnglishDraughtAI>;
						// Child
						{
							editAI.show.allRemoveCallBack (this);
							editAI.compare.allRemoveCallBack (this);
						}
						return;
					}
					// Child
					{
						if (data is EnglishDraughtAI) {
							EnglishDraughtAI englishDraughtAI = data as EnglishDraughtAI;
							// Parent
							{
								DataUtils.removeParentCallBack (englishDraughtAI, this, ref this.server);
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
				if (data is RequestChangeBoolUI.UIData) {
					RequestChangeBoolUI.UIData requestChange = data as RequestChangeBoolUI.UIData;
					// UI
					{
						requestChange.removeCallBackAndDestroy (typeof(RequestChangeBoolUI));
					}
					return;
				}
				if (data is RequestChangeFloatUI.UIData) {
					RequestChangeFloatUI.UIData requestChange = data as RequestChangeFloatUI.UIData;
					// UI
					{
						requestChange.removeCallBackAndDestroy (typeof(RequestChangeFloatUI));
					}
					return;
				}
				if (data is RequestChangeIntUI.UIData) {
					RequestChangeIntUI.UIData requestChange = data as RequestChangeIntUI.UIData;
					// UI
					{
						requestChange.removeCallBackAndDestroy (typeof(RequestChangeIntUI));
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
				case UIData.Property.editAI:
					{
						ValueChangeUtils.replaceCallBack (this, syncs);
						dirty = true;
					}
					break;
				case UIData.Property.threeMoveRandom:
					{
						ValueChangeUtils.replaceCallBack (this, syncs);
						dirty = true;
					}
					break;
				case UIData.Property.fMaxSeconds:
					{
						ValueChangeUtils.replaceCallBack (this, syncs);
						dirty = true;
					}
					break;
				case UIData.Property.g_MaxDepth:
					{
						ValueChangeUtils.replaceCallBack (this, syncs);
						dirty = true;
					}
					break;
				case UIData.Property.pickBestMove:
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
				// editAI
				{
					if (wrapProperty.p is EditData<EnglishDraughtAI>) {
						switch ((EditData<EnglishDraughtAI>.Property)wrapProperty.n) {
						case EditData<EnglishDraughtAI>.Property.origin:
							dirty = true;
							break;
						case EditData<EnglishDraughtAI>.Property.show:
							{
								ValueChangeUtils.replaceCallBack (this, syncs);
								dirty = true;
							}
							break;
						case EditData<EnglishDraughtAI>.Property.compare:
							{
								ValueChangeUtils.replaceCallBack (this, syncs);
								dirty = true;
							}
							break;
						case EditData<EnglishDraughtAI>.Property.compareOtherType:
							dirty = true;
							break;
						case EditData<EnglishDraughtAI>.Property.canEdit:
							dirty = true;
							break;
						case EditData<EnglishDraughtAI>.Property.editType:
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
						if (wrapProperty.p is EnglishDraughtAI) {
							switch ((EnglishDraughtAI.Property)wrapProperty.n) {
							case EnglishDraughtAI.Property.threeMoveRandom:
								dirty = true;
								break;
							case EnglishDraughtAI.Property.fMaxSeconds:
								dirty = true;
								break;
							case EnglishDraughtAI.Property.g_MaxDepth:
								dirty = true;
								break;
							case EnglishDraughtAI.Property.pickBestMove:
								dirty = true;
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
				if (wrapProperty.p is RequestChangeBoolUI.UIData) {
					return;
				}
				if (wrapProperty.p is RequestChangeFloatUI.UIData) {
					return;
				}
				if (wrapProperty.p is RequestChangeIntUI.UIData) {
					return;
				}
			}
			Debug.LogError ("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
		}

		#endregion

	}
}