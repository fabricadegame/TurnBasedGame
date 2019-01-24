﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace NineMenMorris
{
	public class NineMenMorrisAIUI : UIBehavior<NineMenMorrisAIUI.UIData>
	{

		#region UIData

		public class UIData : AIUI.UIData.Sub
		{

			public VP<EditData<NineMenMorrisAI>> editAI;

			#region MaxNormal

			public VP<RequestChangeIntUI.UIData> MaxNormal;

			public void makeRequestChangeMaxNormal (RequestChangeUpdate<int>.UpdateData update, int newMaxNormal)
			{
				// Find
				NineMenMorrisAI nineMenMorrisAI = null;
				{
					EditData<NineMenMorrisAI> editNineMenMorrisAI = this.editAI.v;
					if (editNineMenMorrisAI != null) {
						nineMenMorrisAI = editNineMenMorrisAI.show.v.data;
					} else {
						Debug.LogError ("editNineMenMorrisAI null: " + this);
					}
				}
				// Process
				if (nineMenMorrisAI != null) {
					nineMenMorrisAI.requestChangeMaxNormal (Server.getProfileUserId (nineMenMorrisAI), newMaxNormal);
				} else {
					Debug.LogError ("nineMenMorrisAI null: " + this);
				}
			}

			#endregion

			#region MaxPositioning

			public VP<RequestChangeIntUI.UIData> MaxPositioning;

			public void makeRequestChangeMaxPositioning (RequestChangeUpdate<int>.UpdateData update, int newMaxPositioning)
			{
				// Find
				NineMenMorrisAI nineMenMorrisAI = null;
				{
					EditData<NineMenMorrisAI> editNineMenMorrisAI = this.editAI.v;
					if (editNineMenMorrisAI != null) {
						nineMenMorrisAI = editNineMenMorrisAI.show.v.data;
					} else {
						Debug.LogError ("editNineMenMorrisAI null: " + this);
					}
				}
				// Process
				if (nineMenMorrisAI != null) {
					nineMenMorrisAI.requestChangeMaxPositioning (Server.getProfileUserId (nineMenMorrisAI), newMaxPositioning);
				} else {
					Debug.LogError ("nineMenMorrisAI null: " + this);
				}
			}

			#endregion

			#region MaxBlackAndWhite3

			public VP<RequestChangeIntUI.UIData> MaxBlackAndWhite3;

			public void makeRequestChangeMaxBlackAndWhite3 (RequestChangeUpdate<int>.UpdateData update, int newMaxBlackAndWhite3)
			{
				// Find
				NineMenMorrisAI nineMenMorrisAI = null;
				{
					EditData<NineMenMorrisAI> editNineMenMorrisAI = this.editAI.v;
					if (editNineMenMorrisAI != null) {
						nineMenMorrisAI = editNineMenMorrisAI.show.v.data;
					} else {
						Debug.LogError ("editNineMenMorrisAI null: " + this);
					}
				}
				// Process
				if (nineMenMorrisAI != null) {
					nineMenMorrisAI.requestChangeMaxBlackAndWhite3 (Server.getProfileUserId (nineMenMorrisAI), newMaxBlackAndWhite3);
				} else {
					Debug.LogError ("nineMenMorrisAI null: " + this);
				}
			}

			#endregion

			#region MaxBlackOrWhite3

			public VP<RequestChangeIntUI.UIData> MaxBlackOrWhite3;

			public void makeRequestChangeMaxBlackOrWhite3 (RequestChangeUpdate<int>.UpdateData update, int newMaxBlackOrWhite3)
			{
				// Find
				NineMenMorrisAI nineMenMorrisAI = null;
				{
					EditData<NineMenMorrisAI> editNineMenMorrisAI = this.editAI.v;
					if (editNineMenMorrisAI != null) {
						nineMenMorrisAI = editNineMenMorrisAI.show.v.data;
					} else {
						Debug.LogError ("editNineMenMorrisAI null: " + this);
					}
				}
				// Process
				if (nineMenMorrisAI != null) {
					nineMenMorrisAI.requestChangeMaxBlackOrWhite3 (Server.getProfileUserId (nineMenMorrisAI), newMaxBlackOrWhite3);
				} else {
					Debug.LogError ("nineMenMorrisAI null: " + this);
				}
			}

			#endregion

			#region pickBestMove

			public VP<RequestChangeIntUI.UIData> pickBestMove;

			public void makeRequestChangeMaxPickBestMove (RequestChangeUpdate<int>.UpdateData update, int newPickBestMove)
			{
				// Find
				NineMenMorrisAI nineMenMorrisAI = null;
				{
					EditData<NineMenMorrisAI> editNineMenMorrisAI = this.editAI.v;
					if (editNineMenMorrisAI != null) {
						nineMenMorrisAI = editNineMenMorrisAI.show.v.data;
					} else {
						Debug.LogError ("editNineMenMorrisAI null: " + this);
					}
				}
				// Process
				if (nineMenMorrisAI != null) {
					nineMenMorrisAI.requestChangePickBestMove (Server.getProfileUserId (nineMenMorrisAI), newPickBestMove);
				} else {
					Debug.LogError ("nineMenMorrisAI null: " + this);
				}
			}

			#endregion

			#region Constructor

			public enum Property
			{
				editAI,
				MaxNormal,
				MaxPositioning,
				MaxBlackAndWhite3,
				MaxBlackOrWhite3,
				pickBestMove
			}

			public UIData() : base()
			{
				this.editAI = new VP<EditData<NineMenMorrisAI>>(this, (byte)Property.editAI, new EditData<NineMenMorrisAI>());
				// MaxNormal
				{
					this.MaxNormal = new VP<RequestChangeIntUI.UIData>(this, (byte)Property.MaxNormal, new RequestChangeIntUI.UIData());
					// have limit
					{
						IntLimit.Have have = new IntLimit.Have();
						{
							have.uid = this.MaxNormal.v.limit.makeId();
							have.min.v = 1;
							have.max.v = 10;
						}
						this.MaxNormal.v.limit.v = have;
					}
					// event
					this.MaxNormal.v.updateData.v.request.v = makeRequestChangeMaxNormal;
				}
				// MaxPositioning
				{
					this.MaxPositioning = new VP<RequestChangeIntUI.UIData>(this, (byte)Property.MaxPositioning, new RequestChangeIntUI.UIData());
					// have limit
					{
						IntLimit.Have have = new IntLimit.Have();
						{
							have.uid = this.MaxPositioning.v.limit.makeId();
							have.min.v = 1;
							have.max.v = 10;
						}
						this.MaxPositioning.v.limit.v = have;
					}
					// event
					this.MaxPositioning.v.updateData.v.request.v = makeRequestChangeMaxPositioning;
				}
				// MaxBlackAndWhite3
				{
					this.MaxBlackAndWhite3 = new VP<RequestChangeIntUI.UIData>(this, (byte)Property.MaxBlackAndWhite3, new RequestChangeIntUI.UIData());
					// have limit
					{
						IntLimit.Have have = new IntLimit.Have();
						{
							have.uid = this.MaxBlackAndWhite3.v.limit.makeId();
							have.min.v = 1;
							have.max.v = 10;
						}
						this.MaxBlackAndWhite3.v.limit.v = have;
					}
					// event
					this.MaxBlackAndWhite3.v.updateData.v.request.v = makeRequestChangeMaxBlackAndWhite3;
				}
				// MaxBlackOrWhite3
				{
					this.MaxBlackOrWhite3 = new VP<RequestChangeIntUI.UIData>(this, (byte)Property.MaxBlackOrWhite3, new RequestChangeIntUI.UIData());
					// have limit
					{
						IntLimit.Have have = new IntLimit.Have();
						{
							have.uid = this.MaxBlackOrWhite3.v.limit.makeId();
							have.min.v = 1;
							have.max.v = 10;
						}
						this.MaxBlackOrWhite3.v.limit.v = have;
					}
					// event
					this.MaxBlackOrWhite3.v.updateData.v.request.v = makeRequestChangeMaxBlackOrWhite3;
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
					this.pickBestMove.v.updateData.v.request.v = makeRequestChangeMaxPickBestMove;
				}
			}

			#endregion

			public override GameType.Type getType ()
			{
				return GameType.Type.NineMenMorris;
			}

		}

		#endregion

		#region Refresh

		#region txt

		public Text lbTitle;
		public static readonly TxtLanguage txtTitle = new TxtLanguage();

		public Text lbMaxNormal;
		public static readonly TxtLanguage txtMaxNormal = new TxtLanguage();

		public Text lbMaxPositioning;
		public static readonly TxtLanguage txtMaxPositioning = new TxtLanguage ();

		public Text lbMaxBlackAndWhite3;
		public static readonly TxtLanguage txtMaxBlackAndWhite3 = new TxtLanguage();

		public Text lbMaxBlackOrWhite3;
		public static readonly TxtLanguage txtMaxBlackOrWhite3 = new TxtLanguage ();

		public Text lbPickBestMove;
		public static readonly TxtLanguage txtPickBestMove = new TxtLanguage();

		static NineMenMorrisAIUI()
		{
			txtTitle.add (Language.Type.vi, "AI Nine Men's Morriss");
			txtMaxNormal.add (Language.Type.vi, "Độ sâu lúc bình ");
			txtMaxPositioning.add (Language.Type.vi, "Độ sâu lúc đặt quân");
			txtMaxBlackAndWhite3.add (Language.Type.vi, "Độ sâu khi cả 2 bên còn 3 quân");
			txtMaxBlackOrWhite3.add (Language.Type.vi, "Độ sâu khi 1 trong 2 bên còn 3 quân");
			txtPickBestMove.add (Language.Type.vi, "Tỷ lệ chọn nước đi tốt nhất");
		}

		#endregion

		private bool needReset = true;
		public GameObject differentIndicator;

		public override void refresh ()
		{
			if (dirty) {
				dirty = false;
				if (this.data != null) {
					EditData<NineMenMorrisAI> editNineMenMorrisAI = this.data.editAI.v;
					if (editNineMenMorrisAI != null) {
						editNineMenMorrisAI.update ();
						// get show
						NineMenMorrisAI show = editNineMenMorrisAI.show.v.data;
						NineMenMorrisAI compare = editNineMenMorrisAI.compare.v.data;
						if (show != null) {
							// differentIndicator
							if (differentIndicator != null) {
								bool isDifferent = false;
								{
									if (editNineMenMorrisAI.compareOtherType.v.data != null) {
										if (editNineMenMorrisAI.compareOtherType.v.data.GetType () != show.GetType ()) {
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
									// Debug.LogError ("server null: " + this);
								}
							}
							// set origin
							{
								// MaxNormal
								{
									RequestChangeIntUI.UIData maxNormal = this.data.MaxNormal.v;
									if (maxNormal != null) {
										// update
										RequestChangeUpdate<int>.UpdateData updateData = maxNormal.updateData.v;
										if (updateData != null) {
											updateData.origin.v = show.MaxNormal.v;
											updateData.canRequestChange.v = editNineMenMorrisAI.canEdit.v;
											updateData.serverState.v = serverState;
										} else {
											Debug.LogError ("updateData null: " + this);
										}
										// compare
										{
											if (compare != null) {
												maxNormal.showDifferent.v = true;
												maxNormal.compare.v = compare.MaxNormal.v;
											} else {
												maxNormal.showDifferent.v = false;
											}
										}
									} else {
										Debug.LogError ("MaxNormal null: " + this);
									}
								}
								// MaxPositioning
								{
									RequestChangeIntUI.UIData maxPositioning = this.data.MaxPositioning.v;
									if (maxPositioning != null) {
										// update
										RequestChangeUpdate<int>.UpdateData updateData = maxPositioning.updateData.v;
										if (updateData != null) {
											updateData.origin.v = show.MaxPositioning.v;
											updateData.canRequestChange.v = editNineMenMorrisAI.canEdit.v;
											updateData.serverState.v = serverState;
										} else {
											Debug.LogError ("updateData null: " + this);
										}
										// compare
										{
											if (compare != null) {
												maxPositioning.showDifferent.v = true;
												maxPositioning.compare.v = compare.MaxPositioning.v;
											} else {
												maxPositioning.showDifferent.v = false;
											}
										}
									} else {
										Debug.LogError ("MaxPositioning null: " + this);
									}
								}
								// MaxBlackAndWhite3
								{
									RequestChangeIntUI.UIData maxBlackAndWhite3 = this.data.MaxBlackAndWhite3.v;
									if (maxBlackAndWhite3 != null) {
										// update
										RequestChangeUpdate<int>.UpdateData updateData = maxBlackAndWhite3.updateData.v;
										if (updateData != null) {
											updateData.origin.v = show.MaxBlackAndWhite3.v;
											updateData.canRequestChange.v = editNineMenMorrisAI.canEdit.v;
											updateData.serverState.v = serverState;
										} else {
											Debug.LogError ("updateData null: " + this);
										}
										// compare
										{
											if (compare != null) {
												maxBlackAndWhite3.showDifferent.v = true;
												maxBlackAndWhite3.compare.v = compare.MaxBlackAndWhite3.v;
											} else {
												maxBlackAndWhite3.showDifferent.v = false;
											}
										}
									} else {
										Debug.LogError ("MaxBlackAndWhite3 null: " + this);
									}
								}
								// MaxBlackOrWhite3
								{
									RequestChangeIntUI.UIData maxBlackOrWhite3 = this.data.MaxBlackOrWhite3.v;
									if (maxBlackOrWhite3 != null) {
										// update
										RequestChangeUpdate<int>.UpdateData updateData = maxBlackOrWhite3.updateData.v;
										if (updateData != null) {
											updateData.origin.v = show.MaxBlackOrWhite3.v;
											updateData.canRequestChange.v = editNineMenMorrisAI.canEdit.v;
											updateData.serverState.v = serverState;
										} else {
											Debug.LogError ("updateData null: " + this);
										}
										// compare
										{
											if (compare != null) {
												maxBlackOrWhite3.showDifferent.v = true;
												maxBlackOrWhite3.compare.v = compare.MaxBlackOrWhite3.v;
											} else {
												maxBlackOrWhite3.showDifferent.v = false;
											}
										}
									} else {
										Debug.LogError ("MaxBlackOrWhite3 null: " + this);
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
											updateData.canRequestChange.v = editNineMenMorrisAI.canEdit.v;
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
								// MaxNormal
								{
									RequestChangeIntUI.UIData maxNormal = this.data.MaxNormal.v;
									if (maxNormal != null) {
										// update
										RequestChangeUpdate<int>.UpdateData updateData = maxNormal.updateData.v;
										if (updateData != null) {
											updateData.current.v = show.MaxNormal.v;
											updateData.changeState.v = Data.ChangeState.None;
										} else {
											Debug.LogError ("updateData null: " + this);
										}
									} else {
										Debug.LogError ("MaxNormal null: " + this);
									}
								}
								// MaxPositioning
								{
									RequestChangeIntUI.UIData maxPositioning = this.data.MaxPositioning.v;
									if (maxPositioning != null) {
										// update
										RequestChangeUpdate<int>.UpdateData updateData = maxPositioning.updateData.v;
										if (updateData != null) {
											updateData.current.v = show.MaxPositioning.v;
											updateData.changeState.v = Data.ChangeState.None;
										} else {
											Debug.LogError ("updateData null: " + this);
										}
									} else {
										Debug.LogError ("MaxPositioning null: " + this);
									}
								}
								// MaxBlackAndWhite3
								{
									RequestChangeIntUI.UIData maxBlackAndWhite3 = this.data.MaxBlackAndWhite3.v;
									if (maxBlackAndWhite3 != null) {
										// update
										RequestChangeUpdate<int>.UpdateData updateData = maxBlackAndWhite3.updateData.v;
										if (updateData != null) {
											updateData.current.v = show.MaxBlackAndWhite3.v;
											updateData.changeState.v = Data.ChangeState.None;
										} else {
											Debug.LogError ("updateData null: " + this);
										}
									} else {
										Debug.LogError ("MaxBlackAndWhite3 null: " + this);
									}
								}
								// MaxBlackOrWhite3
								{
									RequestChangeIntUI.UIData maxBlackOrWhite3 = this.data.MaxBlackOrWhite3.v;
									if (maxBlackOrWhite3 != null) {
										// update
										RequestChangeUpdate<int>.UpdateData updateData = maxBlackOrWhite3.updateData.v;
										if (updateData != null) {
											updateData.current.v = show.MaxBlackOrWhite3.v;
											updateData.changeState.v = Data.ChangeState.None;
										} else {
											Debug.LogError ("updateData null: " + this);
										}
									} else {
										Debug.LogError ("MaxBlackOrWhite3 null: " + this);
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
							lbTitle.text = txtTitle.get ("Nine Men's Morriss AI");
						} else {
							Debug.LogError ("lbTitle null: " + this);
						}
						if (lbMaxNormal != null) {
							lbMaxNormal.text = txtMaxNormal.get ("Max normal depth");
						} else {
							Debug.LogError ("lbMaxNormal null: " + this);
						}
						if (lbMaxPositioning != null) {
							lbMaxPositioning.text = txtMaxPositioning.get ("Max positioning depth");
						} else {
							Debug.LogError ("lbMaxPositioning null: " + this);
						}
						if (lbMaxBlackAndWhite3 != null) {
							lbMaxBlackAndWhite3.text = txtMaxBlackAndWhite3.get ("Max black and white 3 depth");
						} else {
							Debug.LogError ("lbMaxBlackAndWhite3 null: " + this);
						}
						if (lbMaxBlackOrWhite3 != null) {
							lbMaxBlackOrWhite3.text = txtMaxBlackOrWhite3.get ("Max black or white 3 depth");
						} else {
							Debug.LogError ("lbMaxBlackOrWhite3 null: " + this);
						}
						if (lbPickBestMove != null) {
							lbPickBestMove.text = txtPickBestMove.get ("Pick best move depth");
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

		public Transform maxNormalContainer;
		public Transform maxPositioningContainer;
		public Transform maxBlackAndWhite3Container;
		public Transform maxBlackOrWhite3Container;
		public Transform pickBestMoveContainer;

		public RequestChangeIntUI requestIntPrefab;

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
					uiData.MaxNormal.allAddCallBack (this);
					uiData.MaxPositioning.allAddCallBack (this);
					uiData.MaxBlackAndWhite3.allAddCallBack (this);
					uiData.MaxBlackOrWhite3.allAddCallBack (this);
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
					if (data is EditData<NineMenMorrisAI>) {
						EditData<NineMenMorrisAI> editAI = data as EditData<NineMenMorrisAI>;
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
						if (data is NineMenMorrisAI) {
							NineMenMorrisAI nineMenMorrisAI = data as NineMenMorrisAI;
							// Parent
							{
								DataUtils.addParentCallBack (nineMenMorrisAI, this, ref this.server);
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
				if (data is RequestChangeIntUI.UIData) {
					RequestChangeIntUI.UIData requestChange = data as RequestChangeIntUI.UIData;
					// UI
					{
						WrapProperty wrapProperty = requestChange.p;
						if (wrapProperty != null) {
							switch ((UIData.Property)wrapProperty.n) {
							case UIData.Property.MaxNormal:
								UIUtils.Instantiate (requestChange, requestIntPrefab, maxNormalContainer);
								break;
							case UIData.Property.MaxPositioning:
								UIUtils.Instantiate (requestChange, requestIntPrefab, maxPositioningContainer);
								break;
							case UIData.Property.MaxBlackAndWhite3:
								UIUtils.Instantiate (requestChange, requestIntPrefab, maxBlackAndWhite3Container);
								break;
							case UIData.Property.MaxBlackOrWhite3:
								UIUtils.Instantiate (requestChange, requestIntPrefab, maxBlackOrWhite3Container);
								break;
							case UIData.Property.pickBestMove:
								UIUtils.Instantiate (requestChange, requestIntPrefab, pickBestMoveContainer);
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
					uiData.editAI.allRemoveCallBack (this);
					uiData.MaxNormal.allRemoveCallBack (this);
					uiData.MaxPositioning.allRemoveCallBack (this);
					uiData.MaxBlackAndWhite3.allRemoveCallBack (this);
					uiData.MaxBlackOrWhite3.allRemoveCallBack (this);
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
					if (data is EditData<NineMenMorrisAI>) {
						EditData<NineMenMorrisAI> editAI = data as EditData<NineMenMorrisAI>;
						// Child
						{
							editAI.show.allRemoveCallBack (this);
							editAI.compare.allRemoveCallBack (this);
						}
						return;
					}
					// Child
					{
						if (data is NineMenMorrisAI) {
							NineMenMorrisAI nineMenMorrisAI = data as NineMenMorrisAI;
							// Parent
							{
								DataUtils.removeParentCallBack (nineMenMorrisAI, this, ref this.server);
							}
							return;
						}
						if (data is Server) {
							return;
						}
					}
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
			if(WrapProperty.checkError(wrapProperty)){
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
				case UIData.Property.MaxNormal:
					{
						ValueChangeUtils.replaceCallBack (this, syncs);
						dirty = true;
					}
					break;
				case UIData.Property.MaxPositioning:
					{
						ValueChangeUtils.replaceCallBack (this, syncs);
						dirty = true;
					}
					break;
				case UIData.Property.MaxBlackAndWhite3:
					{
						ValueChangeUtils.replaceCallBack (this, syncs);
						dirty = true;
					}
					break;
				case UIData.Property.MaxBlackOrWhite3:
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
					if (wrapProperty.p is EditData<NineMenMorrisAI>) {
						switch ((EditData<NineMenMorrisAI>.Property)wrapProperty.n) {
						case EditData<NineMenMorrisAI>.Property.origin:
							dirty = true;
							break;
						case EditData<NineMenMorrisAI>.Property.show:
							{
								ValueChangeUtils.replaceCallBack (this, syncs);
								dirty = true;
							}
							break;
						case EditData<NineMenMorrisAI>.Property.compare:
							{
								ValueChangeUtils.replaceCallBack (this, syncs);
								dirty = true;
							}
							break;
						case EditData<NineMenMorrisAI>.Property.compareOtherType:
							dirty = true;
							break;
						case EditData<NineMenMorrisAI>.Property.canEdit:
							dirty = true;
							break;
						case EditData<NineMenMorrisAI>.Property.editType:
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
						if (wrapProperty.p is NineMenMorrisAI) {
							switch ((NineMenMorrisAI.Property)wrapProperty.n) {
							case NineMenMorrisAI.Property.MaxNormal:
								dirty = true;
								break;
							case NineMenMorrisAI.Property.MaxPositioning:
								dirty = true;
								break;
							case NineMenMorrisAI.Property.MaxBlackAndWhite3:
								dirty = true;
								break;
							case NineMenMorrisAI.Property.MaxBlackOrWhite3:
								dirty = true;
								break;
							case NineMenMorrisAI.Property.pickBestMove:
								dirty = true;
								break;
							default:
								Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
								break;
							}
							return;
						}
						if (wrapProperty.p is Server) {
							Server.State.OnUpdateSyncStateChange (wrapProperty, this);
							return;
						}
					}
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