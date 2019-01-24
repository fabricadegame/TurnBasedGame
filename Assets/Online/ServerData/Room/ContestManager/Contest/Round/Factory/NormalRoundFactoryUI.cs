﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace GameManager.Match
{
	public class NormalRoundFactoryUI : UIBehavior<NormalRoundFactoryUI.UIData>
	{

		#region UIData

		public class UIData : SingleContestFactoryUI.UIData.RoundFactoryUI
		{

			public VP<EditData<NormalRoundFactory>> editNormalRoundFactory;

			#region gameFactory

			public VP<GameFactoryUI.UIData> gameFactory;

			#endregion

			#region isChangeSideBetweenRound

			public VP<RequestChangeBoolUI.UIData> isChangeSideBetweenRound;

			public void makeRequestChangeIsChangeSideBetweenRound (RequestChangeUpdate<bool>.UpdateData update, bool newIsChangeSideBetweenRound)
			{
				// Find
				NormalRoundFactory normalRoundFactory = null;
				{
					EditData<NormalRoundFactory> editNormalRoundFactory = this.editNormalRoundFactory.v;
					if (editNormalRoundFactory != null) {
						normalRoundFactory = editNormalRoundFactory.show.v.data;
					} else {
						Debug.LogError ("editNormalRoundFactory null: " + this);
					}
				}
				// Process
				if (normalRoundFactory != null) {
					normalRoundFactory.requestChangeIsChangeSideBetweenRound (Server.getProfileUserId (normalRoundFactory), newIsChangeSideBetweenRound);
				} else {
					Debug.LogError ("normalRoundFactory null: " + this);
				}
			}

			#endregion

			#region isSwitchPlayer

			public VP<RequestChangeBoolUI.UIData> isSwitchPlayer;

			public void makeRequestChangeIsSwitchPlayer (RequestChangeUpdate<bool>.UpdateData update, bool newIsSwitchPlayer)
			{
				// Find
				NormalRoundFactory normalRoundFactory = null;
				{
					EditData<NormalRoundFactory> editNormalRoundFactory = this.editNormalRoundFactory.v;
					if (editNormalRoundFactory != null) {
						normalRoundFactory = editNormalRoundFactory.show.v.data;
					} else {
						Debug.LogError ("editNormalRoundFactory null: " + this);
					}
				}
				// Process
				if (normalRoundFactory != null) {
					normalRoundFactory.requestChangeIsSwitchPlayer (Server.getProfileUserId (normalRoundFactory), newIsSwitchPlayer);
				} else {
					Debug.LogError ("normalRoundFactory null: " + this);
				}
			}

			#endregion

			#region isDifferentInTeam

			public VP<RequestChangeBoolUI.UIData> isDifferentInTeam;

			public void makeRequestChangeIsDifferentInTeam (RequestChangeUpdate<bool>.UpdateData update, bool newIsDifferentInTeam)
			{
				// Find
				NormalRoundFactory normalRoundFactory = null;
				{
					EditData<NormalRoundFactory> editNormalRoundFactory = this.editNormalRoundFactory.v;
					if (editNormalRoundFactory != null) {
						normalRoundFactory = editNormalRoundFactory.show.v.data;
					} else {
						Debug.LogError ("editNormalRoundFactory null: " + this);
					}
				}
				// Process
				if (normalRoundFactory != null) {
					normalRoundFactory.requestChangeIsDifferentInTeam (Server.getProfileUserId (normalRoundFactory), newIsDifferentInTeam);
				} else {
					Debug.LogError ("normalRoundFactory null: " + this);
				}
			}

			#endregion

			#region calculateScore

			public VP<RequestChangeEnumUI.UIData> calculateScoreType;

			public void makeRequestChangeCalculateScoreType (RequestChangeUpdate<int>.UpdateData update, int newCalculateScoreType)
			{
				// Find
				NormalRoundFactory normalRoundFactory = null;
				{
					EditData<NormalRoundFactory> editNormalRoundFactory = this.editNormalRoundFactory.v;
					if (editNormalRoundFactory != null) {
						normalRoundFactory = editNormalRoundFactory.show.v.data;
					} else {
						Debug.LogError ("editNormalRoundFactory null: " + this);
					}
				}
				// Process
				if (normalRoundFactory != null) {
					normalRoundFactory.requestChangeCalculateScoreType (Server.getProfileUserId (normalRoundFactory), newCalculateScoreType);
				} else {
					Debug.LogError ("normalRoundFactory null: " + this);
				}
			}

			public VP<CalculateScore.UIData> calculateScoreUI;

			#endregion

			#region Constructor

			public enum Property
			{
				editNormalRoundFactory,
				gameFactory,
				isChangeSideBetweenRound,
				isSwitchPlayer,
				isDifferentInTeam,

				calculateScoreType,
				calculateScoreUI
			}

			public UIData() : base()
			{
				this.editNormalRoundFactory = new VP<EditData<NormalRoundFactory>>(this, (byte)Property.editNormalRoundFactory, new EditData<NormalRoundFactory>());
				this.gameFactory = new VP<GameFactoryUI.UIData>(this, (byte)Property.gameFactory, new GameFactoryUI.UIData());
				// isChangeSideBetweenRound
				{
					this.isChangeSideBetweenRound = new VP<RequestChangeBoolUI.UIData>(this, (byte)Property.isChangeSideBetweenRound, new RequestChangeBoolUI.UIData());
					this.isChangeSideBetweenRound.v.updateData.v.request.v = makeRequestChangeIsChangeSideBetweenRound;
				}
				// isSwitchPlayer
				{
					this.isSwitchPlayer = new VP<RequestChangeBoolUI.UIData>(this, (byte)Property.isSwitchPlayer, new RequestChangeBoolUI.UIData());
					this.isSwitchPlayer.v.updateData.v.request.v = makeRequestChangeIsSwitchPlayer;
				}
				// isDifferentInTeam
				{
					this.isDifferentInTeam = new VP<RequestChangeBoolUI.UIData>(this, (byte)Property.isDifferentInTeam, new RequestChangeBoolUI.UIData());
					this.isDifferentInTeam.v.updateData.v.request.v = makeRequestChangeIsDifferentInTeam;
				}
				// calculateScoreType
				{
					this.calculateScoreType = new VP<RequestChangeEnumUI.UIData>(this, (byte)Property.calculateScoreType, new RequestChangeEnumUI.UIData());
					// event
					this.calculateScoreType.v.updateData.v.request.v = makeRequestChangeCalculateScoreType;
					{
						foreach (CalculateScore.Type type in System.Enum.GetValues(typeof(CalculateScore.Type))) {
							this.calculateScoreType.v.options.add(type.ToString());
						}
					}
				}
				this.calculateScoreUI = new VP<CalculateScore.UIData>(this, (byte)Property.calculateScoreUI, null);
			}

			#endregion

			public override RoundFactory.Type getType ()
			{
				return RoundFactory.Type.Normal;
			}

			public override bool processEvent (Event e)
			{
				Debug.LogError ("processEvent: " + e + "; " + this);
				bool isProcess = false;
				{
					// gameFactory
					if (!isProcess) {
						GameFactoryUI.UIData gameFactoryUIData = this.gameFactory.v;
						if (gameFactoryUIData != null) {
							isProcess = gameFactoryUIData.processEvent (e);
						} else {
							Debug.LogError ("gameFactoryUIData null: " + this);
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

		public Text lbIsChangeSideBetweenRound;
		public static readonly TxtLanguage txtIsChangeSideBetweenRound = new TxtLanguage ();

		public Text lbIsSwitchPlayer;
		public static readonly TxtLanguage txtIsSwitchPlayer = new TxtLanguage();

		public Text lbIsDifferentInTeam;
		public static readonly TxtLanguage txtIsDifferentInTeam = new TxtLanguage ();

		public Text lbCalculateScoreType;
		public static readonly TxtLanguage txtCalculateScoreType = new TxtLanguage ();

		static NormalRoundFactoryUI()
		{
			txtTitle.add (Language.Type.vi, "Cách Tạo Set");
			txtIsChangeSideBetweenRound.add (Language.Type.vi, "Đổi bên giữa các set");
			txtIsSwitchPlayer.add (Language.Type.vi, "Hoán đổi bên");
			txtIsDifferentInTeam.add (Language.Type.vi, "Đổi trong đội");
			txtCalculateScoreType.add (Language.Type.vi, "Loại tính điểm");
		}

		#endregion

		private bool needReset = true;
		public GameObject differentIndicator;

		public override void refresh ()
		{
			if (dirty) {
				dirty = false;
				if (this.data != null) {
					EditData<NormalRoundFactory> editNormalRoundFactory = this.data.editNormalRoundFactory.v;
					if (editNormalRoundFactory != null) {
						editNormalRoundFactory.update ();
						// get show
						NormalRoundFactory show = editNormalRoundFactory.show.v.data;
						NormalRoundFactory compare = editNormalRoundFactory.compare.v.data;
						if (show != null) {
							// differentIndicator
							if (differentIndicator != null) {
								bool isDifferent = false;
								{
									if (editNormalRoundFactory.compareOtherType.v.data != null) {
										if (editNormalRoundFactory.compareOtherType.v.data.GetType () != show.GetType ()) {
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
										Debug.LogError ("server null: " + this);
									}
								}
								// set origin
								{
									// gameFactory
									{
										GameFactoryUI.UIData gameFactory = this.data.gameFactory.v;
										if (gameFactory != null) {
											EditData<GameFactory> editGameFactory = gameFactory.editGameFactory.v;
											if (editGameFactory != null) {
												// origin
												{
													GameFactory originGameFactory = null;
													{
														NormalRoundFactory originNormalRoundFactory = editNormalRoundFactory.origin.v.data;
														if (originNormalRoundFactory != null) {
															originGameFactory = originNormalRoundFactory.gameFactory.v;
														} else {
															Debug.LogError ("originNormalRoundFactory null: " + this);
														}
													}
													editGameFactory.origin.v = new ReferenceData<GameFactory> (originGameFactory);
												}
												// show
												{
													GameFactory showGameFactory = null;
													{
														NormalRoundFactory showNormalRoundFactory = editNormalRoundFactory.show.v.data;
														if (showNormalRoundFactory != null) {
															showGameFactory = showNormalRoundFactory.gameFactory.v;
														} else {
															Debug.LogError ("showNormalRoundFactory null: " + this);
														}
													}
													editGameFactory.show.v = new ReferenceData<GameFactory> (showGameFactory);
												}
												// compare
												{
													GameFactory compareGameFactory = null;
													{
														NormalRoundFactory compareNormalRoundFactory = editNormalRoundFactory.compare.v.data;
														if (compareNormalRoundFactory != null) {
															compareGameFactory = compareNormalRoundFactory.gameFactory.v;
														} else {
															Debug.LogError ("compareNormalRoundFactory null: " + this);
														}
													}
													editGameFactory.compare.v = new ReferenceData<GameFactory> (compareGameFactory);
												}
												// compare other type
												{
													GameFactory compareOtherTypeGameFactory = null;
													{
														NormalRoundFactory compareOtherTypeNormalRoundFactory = (NormalRoundFactory)editNormalRoundFactory.compareOtherType.v.data;
														if (compareOtherTypeNormalRoundFactory != null) {
															compareOtherTypeGameFactory = compareOtherTypeNormalRoundFactory.gameFactory.v;
														}
													}
													editGameFactory.compareOtherType.v = new ReferenceData<Data> (compareOtherTypeGameFactory);
												}
												// canEdit
												editGameFactory.canEdit.v = editNormalRoundFactory.canEdit.v;
												// editType
												editGameFactory.editType.v = editNormalRoundFactory.editType.v;
											} else {
												Debug.LogError ("editGameFactory null: " + this);
											}
										} else {
											Debug.LogError ("gameFactory null: " + this);
										}
									}
									// isChangeSideBetweenRound
									{
										RequestChangeBoolUI.UIData isChangeSideBetweenRound = this.data.isChangeSideBetweenRound.v;
										if (isChangeSideBetweenRound != null) {
											// update
											RequestChangeUpdate<bool>.UpdateData updateData = isChangeSideBetweenRound.updateData.v;
											if (updateData != null) {
												updateData.origin.v = show.isChangeSideBetweenRound.v;
												updateData.canRequestChange.v = editNormalRoundFactory.canEdit.v;
												updateData.serverState.v = serverState;
											} else {
												Debug.LogError ("updateData null: " + this);
											}
											// compare
											{
												if (compare != null) {
													isChangeSideBetweenRound.showDifferent.v = true;
													isChangeSideBetweenRound.compare.v = compare.isChangeSideBetweenRound.v;
												} else {
													isChangeSideBetweenRound.showDifferent.v = false;
												}
											}
										} else {
											Debug.LogError ("isChangeSideBetweenRound null: " + this);
										}
									}
									// isSwitchPlayer
									{
										RequestChangeBoolUI.UIData isSwitchPlayer = this.data.isSwitchPlayer.v;
										if (isSwitchPlayer != null) {
											// update
											RequestChangeUpdate<bool>.UpdateData updateData = isSwitchPlayer.updateData.v;
											if (updateData != null) {
												updateData.origin.v = show.isSwitchPlayer.v;
												updateData.canRequestChange.v = editNormalRoundFactory.canEdit.v;
												updateData.serverState.v = serverState;
											} else {
												Debug.LogError ("updateData null: " + this);
											}
											// compare
											{
												if (compare != null) {
													isSwitchPlayer.showDifferent.v = true;
													isSwitchPlayer.compare.v = compare.isSwitchPlayer.v;
												} else {
													isSwitchPlayer.showDifferent.v = false;
												}
											}
										} else {
											Debug.LogError ("isSwitchPlayer null: " + this);
										}
									}
									// isDifferentInTeam
									{
										RequestChangeBoolUI.UIData isDifferentInTeam = this.data.isDifferentInTeam.v;
										if (isDifferentInTeam != null) {
											// update
											RequestChangeUpdate<bool>.UpdateData updateData = isDifferentInTeam.updateData.v;
											if (updateData != null) {
												updateData.origin.v = show.isDifferentInTeam.v;
												updateData.canRequestChange.v = editNormalRoundFactory.canEdit.v;
												updateData.serverState.v = serverState;
											} else {
												Debug.LogError ("updateData null: " + this);
											}
											// compare
											{
												if (compare != null) {
													isDifferentInTeam.showDifferent.v = true;
													isDifferentInTeam.compare.v = compare.isDifferentInTeam.v;
												} else {
													isDifferentInTeam.showDifferent.v = false;
												}
											}
										} else {
											Debug.LogError ("isDifferentInTeam null: " + this);
										}
									}
									// calculateScoreType
									{
										RequestChangeEnumUI.UIData calculateScoreType = this.data.calculateScoreType.v;
										if (calculateScoreType != null) {
											// update
											RequestChangeUpdate<int>.UpdateData updateData = calculateScoreType.updateData.v;
											if (updateData != null) {
												updateData.origin.v = (int)show.getCalculateScoreType ();
												updateData.canRequestChange.v = editNormalRoundFactory.canEdit.v;
												updateData.serverState.v = serverState;
											} else {
												Debug.LogError ("updateData null: " + this);
											}
											// compare
											{
												if (compare != null) {
													calculateScoreType.showDifferent.v = true;
													calculateScoreType.compare.v = (int)compare.getCalculateScoreType ();
												} else {
													calculateScoreType.showDifferent.v = false;
												}
											}
										} else {
											Debug.LogError ("calculateScoreType null: " + this);
										}
									}
									// calculateScoreUI
									{
										CalculateScore calculateScore = show.calculateScore.v;
										if (calculateScore != null) {
											// find origin 
											CalculateScore originCalculateScore = null;
											{
												NormalRoundFactory originNormalRoundFactory = editNormalRoundFactory.origin.v.data;
												if (originNormalRoundFactory != null) {
													originCalculateScore = originNormalRoundFactory.calculateScore.v;
												} else {
													Debug.LogError ("origin null: " + this);
												}
											}
											// find compare
											CalculateScore compareCalculateScore = null;
											{
												if (compare != null) {
													compareCalculateScore = compare.calculateScore.v;
												} else {
													Debug.LogError ("compare null: " + this);
												}
											}
											switch (calculateScore.getType ()) {
											case CalculateScore.Type.Sum:
												{
													CalculateScoreSum calculateScoreSum = calculateScore as CalculateScoreSum;
													// UIData
													CalculateScoreSumUI.UIData calculateScoreSumUIData = this.data.calculateScoreUI.newOrOld<CalculateScoreSumUI.UIData> ();
													{
														EditData<CalculateScoreSum> editCalculateScoreSum = calculateScoreSumUIData.editCalculateScoreSum.v;
														if (editCalculateScoreSum != null) {
															// origin
															editCalculateScoreSum.origin.v = new ReferenceData<CalculateScoreSum> ((CalculateScoreSum)originCalculateScore);
															// show
															editCalculateScoreSum.show.v = new ReferenceData<CalculateScoreSum> (calculateScoreSum);
															// compare
															editCalculateScoreSum.compare.v = new ReferenceData<CalculateScoreSum> ((CalculateScoreSum)compareCalculateScore);
															// compareOtherType
															editCalculateScoreSum.compareOtherType.v = new ReferenceData<Data> (compareCalculateScore);
															// canEdit
															editCalculateScoreSum.canEdit.v = editNormalRoundFactory.canEdit.v;
															// editType
															editCalculateScoreSum.editType.v = editNormalRoundFactory.editType.v;
														} else {
															Debug.LogError ("editCalculateScoreSum null: " + this);
														}
													}
													this.data.calculateScoreUI.v = calculateScoreSumUIData;
												}
												break;
											case CalculateScore.Type.WinLoseDraw:
												{
													CalculateScoreWinLoseDraw calculateScoreWinLoseDraw = calculateScore as CalculateScoreWinLoseDraw;
													// UIData
													CalculateScoreWinLoseDrawUI.UIData calculateScoreWinLoseDrawUIData = this.data.calculateScoreUI.newOrOld<CalculateScoreWinLoseDrawUI.UIData> ();
													{
														EditData<CalculateScoreWinLoseDraw> editCalculateScoreWinLoseDraw = calculateScoreWinLoseDrawUIData.editCalculateScoreWinLoseDraw.v;
														if (editCalculateScoreWinLoseDraw != null) {
															// origin
															editCalculateScoreWinLoseDraw.origin.v = new ReferenceData<CalculateScoreWinLoseDraw> ((CalculateScoreWinLoseDraw)originCalculateScore);
															// show
															editCalculateScoreWinLoseDraw.show.v = new ReferenceData<CalculateScoreWinLoseDraw> (calculateScoreWinLoseDraw);
															// compare
															editCalculateScoreWinLoseDraw.compare.v = new ReferenceData<CalculateScoreWinLoseDraw> ((CalculateScoreWinLoseDraw)compareCalculateScore);
															// compareOtherType
															editCalculateScoreWinLoseDraw.compareOtherType.v = new ReferenceData<Data> (compareCalculateScore);
															// canEdit
															editCalculateScoreWinLoseDraw.canEdit.v = editNormalRoundFactory.canEdit.v;
															// editType
															editCalculateScoreWinLoseDraw.editType.v = editNormalRoundFactory.editType.v;
														} else {
															Debug.LogError ("editCalculateScoreWinLoseDraw null: " + this);
														}
													}
													this.data.calculateScoreUI.v = calculateScoreWinLoseDrawUIData;
												}
												break;
											default:
												Debug.LogError ("unknown type: " + calculateScore.getType () + "; " + this);
												break;
											}
										} else {
											Debug.LogError ("show null: " + this);
										}
									}
								}
								// reset
								if (needReset) {
									needReset = false;
									// isChangeSideBetweenRound
									{
										RequestChangeBoolUI.UIData isChangeSideBetweenRound = this.data.isChangeSideBetweenRound.v;
										if (isChangeSideBetweenRound != null) {
											// update
											RequestChangeUpdate<bool>.UpdateData updateData = isChangeSideBetweenRound.updateData.v;
											if (updateData != null) {
												updateData.current.v = show.isChangeSideBetweenRound.v;
												updateData.changeState.v = Data.ChangeState.None;
											} else {
												Debug.LogError ("updateData null: " + this);
											}
										} else {
											Debug.LogError ("isChangeSideBetweenRound null: " + this);
										}
									}
									// isSwitchPlayer
									{
										RequestChangeBoolUI.UIData isSwitchPlayer = this.data.isSwitchPlayer.v;
										if (isSwitchPlayer != null) {
											// update
											RequestChangeUpdate<bool>.UpdateData updateData = isSwitchPlayer.updateData.v;
											if (updateData != null) {
												updateData.current.v = show.isSwitchPlayer.v;
												updateData.changeState.v = Data.ChangeState.None;
											} else {
												Debug.LogError ("updateData null: " + this);
											}
										} else {
											Debug.LogError ("isSwitchPlayer null: " + this);
										}
									}
									// isDifferentInTeam
									{
										RequestChangeBoolUI.UIData isDifferentInTeam = this.data.isDifferentInTeam.v;
										if (isDifferentInTeam != null) {
											// update
											RequestChangeUpdate<bool>.UpdateData updateData = isDifferentInTeam.updateData.v;
											if (updateData != null) {
												updateData.current.v = show.isDifferentInTeam.v;
												updateData.changeState.v = Data.ChangeState.None;
											} else {
												Debug.LogError ("updateData null: " + this);
											}
										} else {
											Debug.LogError ("isDifferentInTeam null: " + this);
										}
									}
									// calculateScoreType
									{
										RequestChangeEnumUI.UIData calculateScoreType = this.data.calculateScoreType.v;
										if (calculateScoreType != null) {
											// update
											RequestChangeUpdate<int>.UpdateData updateData = calculateScoreType.updateData.v;
											if (updateData != null) {
												updateData.current.v = (int)show.getCalculateScoreType ();
												updateData.changeState.v = Data.ChangeState.None;
											} else {
												Debug.LogError ("updateData null: " + this);
											}
										} else {
											Debug.LogError ("calculateScoreType null: " + this);
										}
									}
								}
							}
						} else {
							Debug.LogError ("show null: " + this);
						}
					} else {
						Debug.LogError ("editNormalRoundFactory null: " + this);
					}
					// txt
					{
						if (lbTitle != null) {
							lbTitle.text = txtTitle.get ("Set Factory");
						} else {
							Debug.LogError ("lbTitle null: " + this);
						}
						if (lbIsChangeSideBetweenRound != null) {
							lbIsChangeSideBetweenRound.text = txtIsChangeSideBetweenRound.get ("Change side between round");
						} else {
							Debug.LogError ("lbIsChangeSideBetweenRound null: " + this);
						}
						if (lbIsSwitchPlayer != null) {
							lbIsSwitchPlayer.text = txtIsSwitchPlayer.get ("Switch player");
						} else {
							Debug.LogError ("lbIsSwitchPlayer null: " + this);
						}
						if (lbIsDifferentInTeam != null) {
							lbIsDifferentInTeam.text = txtIsDifferentInTeam.get ("Different in team");
						} else {
							Debug.LogError ("lbIsDifferentInTeam null: " + this);
						}
						if (lbCalculateScoreType != null) {
							lbCalculateScoreType.text = txtCalculateScoreType.get ("Calculate score type");
						} else {
							Debug.LogError ("lbCalculateScoreType null: " + this);
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

		public GameFactoryUI gameFactoryPrefab;
		public RequestChangeBoolUI requestBoolPrefab;
		public RequestChangeEnumUI requestEnumPrefab;

		public CalculateScoreSumUI calculateScoreSumPrefab;
		public CalculateScoreWinLoseDrawUI calculateScoreWinLoseDrawPrefab;

		public Transform gameFactoryContainer;
		public Transform isChangeSideBetweenRoundContainer;
		public Transform isSwitchPlayerContainer;
		public Transform isDifferentInTeamContainer;

		public Transform calculateScoreTypeContainer;
		public Transform calculateScoreUIContainer;

		private Server server = null;

		public override void onAddCallBack<T> (T data)
		{
			if (data is UIData) {
				UIData uiData = data as UIData;
				// Setting
				Setting.get().addCallBack(this);
				// Child
				{
					uiData.editNormalRoundFactory.allAddCallBack (this);
					uiData.gameFactory.allAddCallBack (this);
					uiData.isChangeSideBetweenRound.allAddCallBack (this);
					uiData.isSwitchPlayer.allAddCallBack (this);
					uiData.isDifferentInTeam.allAddCallBack (this);
					// calculateScore
					{
						uiData.calculateScoreType.allAddCallBack (this);
						uiData.calculateScoreUI.allAddCallBack (this);
					}
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
				// editNormalRoundFactory
				{
					if (data is EditData<NormalRoundFactory>) {
						EditData<NormalRoundFactory> editNormalRoundFactory = data as EditData<NormalRoundFactory>;
						// Child
						{
							editNormalRoundFactory.show.allAddCallBack (this);
							editNormalRoundFactory.compare.allAddCallBack (this);
						}
						dirty = true;
						return;
					}
					// Child
					{
						if (data is NormalRoundFactory) {
							NormalRoundFactory normalRoundFactory = data as NormalRoundFactory;
							// Parent
							{
								DataUtils.addParentCallBack (normalRoundFactory, this, ref this.server);
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
				// isChangeSideBetweenRound, isSwitchPlayer, isDifferentInTeam
				if (data is RequestChangeBoolUI.UIData) {
					RequestChangeBoolUI.UIData requestChange = data as RequestChangeBoolUI.UIData;
					// UI
					{
						WrapProperty wrapProperty = requestChange.p;
						if (wrapProperty != null) {
							switch ((UIData.Property)wrapProperty.n) {
							case UIData.Property.isChangeSideBetweenRound:
								UIUtils.Instantiate (requestChange, requestBoolPrefab, isChangeSideBetweenRoundContainer);
								break;
							case UIData.Property.isSwitchPlayer:
								UIUtils.Instantiate (requestChange, requestBoolPrefab, isSwitchPlayerContainer);
								break;
							case UIData.Property.isDifferentInTeam:
								UIUtils.Instantiate (requestChange, requestBoolPrefab, isDifferentInTeamContainer);
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
				if (data is GameFactoryUI.UIData) {
					GameFactoryUI.UIData gameFactoryUIData = data as GameFactoryUI.UIData;
					// UI
					{
						UIUtils.Instantiate (gameFactoryUIData, gameFactoryPrefab, gameFactoryContainer);
					}
					dirty = true;
					return;
				}
				// calculateScoreType
				if (data is RequestChangeEnumUI.UIData) {
					RequestChangeEnumUI.UIData requestChange = data as RequestChangeEnumUI.UIData;
					// UI
					{
						WrapProperty wrapProperty = requestChange.p;
						if (wrapProperty != null) {
							switch ((UIData.Property)wrapProperty.n) {
							case UIData.Property.calculateScoreType:
								UIUtils.Instantiate (requestChange, requestEnumPrefab, calculateScoreTypeContainer);
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
				// calculateScoreUI
				if (data is CalculateScore.UIData) {
					CalculateScore.UIData calculateScoreUIData = data as CalculateScore.UIData;
					// UI
					{
						switch (calculateScoreUIData.getType ()) {
						case CalculateScore.Type.Sum:
							{
								CalculateScoreSumUI.UIData calculateScoreSumUIData = calculateScoreUIData as CalculateScoreSumUI.UIData;
								UIUtils.Instantiate (calculateScoreSumUIData, calculateScoreSumPrefab, calculateScoreUIContainer);
							}
							break;
						case CalculateScore.Type.WinLoseDraw:
							{
								CalculateScoreWinLoseDrawUI.UIData calculateScoreWinLoseDrawUIData = calculateScoreUIData as CalculateScoreWinLoseDrawUI.UIData;
								UIUtils.Instantiate (calculateScoreWinLoseDrawUIData, calculateScoreWinLoseDrawPrefab, calculateScoreUIContainer);
							}
							break;
						default:
							Debug.LogError ("unknown type: " + calculateScoreUIData.getType () + "; " + this);
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
					uiData.editNormalRoundFactory.allRemoveCallBack (this);
					uiData.gameFactory.allRemoveCallBack (this);
					uiData.isChangeSideBetweenRound.allRemoveCallBack (this);
					uiData.isSwitchPlayer.allRemoveCallBack (this);
					uiData.isDifferentInTeam.allRemoveCallBack (this);
					// calculateScore
					{
						uiData.calculateScoreType.allRemoveCallBack (this);
						uiData.calculateScoreUI.allRemoveCallBack (this);
					}
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
				// editNormalRoundFactory
				{
					if (data is EditData<NormalRoundFactory>) {
						EditData<NormalRoundFactory> editNormalRoundFactory = data as EditData<NormalRoundFactory>;
						// Child
						{
							editNormalRoundFactory.show.allRemoveCallBack (this);
							editNormalRoundFactory.compare.allRemoveCallBack (this);
						}
						return;
					}
					// Child
					{
						if (data is NormalRoundFactory) {
							NormalRoundFactory normalRoundFactory = data as NormalRoundFactory;
							// Parent
							{
								DataUtils.removeParentCallBack (normalRoundFactory, this, ref this.server);
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
				// isChangeSideBetweenRound, isSwitchPlayer, isDifferentInTeam
				if (data is RequestChangeBoolUI.UIData) {
					RequestChangeBoolUI.UIData requestChange = data as RequestChangeBoolUI.UIData;
					// UI
					{
						requestChange.removeCallBackAndDestroy (typeof(RequestChangeBoolUI));
					}
					dirty = true;
					return;
				}
				if (data is GameFactoryUI.UIData) {
					GameFactoryUI.UIData gameFactoryUIData = data as GameFactoryUI.UIData;
					// UI
					{
						gameFactoryUIData.removeCallBackAndDestroy (typeof(GameFactoryUI));
					}
					return;
				}
				// calculateScoreType
				if (data is RequestChangeEnumUI.UIData) {
					RequestChangeEnumUI.UIData requestChange = data as RequestChangeEnumUI.UIData;
					// UI
					{
						requestChange.removeCallBackAndDestroy (typeof(RequestChangeEnumUI));
					}
					return;
				}
				// calculateScoreUI
				if (data is CalculateScore.UIData) {
					CalculateScore.UIData calculateScoreUIData = data as CalculateScore.UIData;
					// UI
					{
						switch (calculateScoreUIData.getType ()) {
						case CalculateScore.Type.Sum:
							{
								CalculateScoreSumUI.UIData calculateScoreSumUIData = calculateScoreUIData as CalculateScoreSumUI.UIData;
								calculateScoreSumUIData.removeCallBackAndDestroy (typeof(CalculateScoreSumUI));
							}
							break;
						case CalculateScore.Type.WinLoseDraw:
							{
								CalculateScoreWinLoseDrawUI.UIData calculateScoreWinLoseDrawUIData = calculateScoreUIData as CalculateScoreWinLoseDrawUI.UIData;
								calculateScoreWinLoseDrawUIData.removeCallBackAndDestroy (typeof(CalculateScoreWinLoseDrawUI));
							}
							break;
						default:
							Debug.LogError ("unknown type: " + calculateScoreUIData.getType () + "; " + this);
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
				case UIData.Property.editNormalRoundFactory:
					{
						ValueChangeUtils.replaceCallBack (this, syncs);
						dirty = true;
					}
					break;
				case UIData.Property.gameFactory:
					{
						ValueChangeUtils.replaceCallBack (this, syncs);
						dirty = true;
					}
					break;
				case UIData.Property.isChangeSideBetweenRound:
					{
						ValueChangeUtils.replaceCallBack (this, syncs);
						dirty = true;
					}
					break;
				case UIData.Property.isSwitchPlayer:
					{
						ValueChangeUtils.replaceCallBack (this, syncs);
						dirty = true;
					}
					break;
				case UIData.Property.isDifferentInTeam:
					{
						ValueChangeUtils.replaceCallBack (this, syncs);
						dirty = true;
					}
					break;
				case UIData.Property.calculateScoreType:
					{
						ValueChangeUtils.replaceCallBack (this, syncs);
						dirty = true;
					}
					break;
				case UIData.Property.calculateScoreUI:
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
				// editNormalRoundFactory
				{
					if (wrapProperty.p is EditData<NormalRoundFactory>) {
						switch ((EditData<NormalRoundFactory>.Property)wrapProperty.n) {
						case EditData<NormalRoundFactory>.Property.origin:
							dirty = true;
							break;
						case EditData<NormalRoundFactory>.Property.show:
							{
								ValueChangeUtils.replaceCallBack (this, syncs);
								dirty = true;
							}
							break;
						case EditData<NormalRoundFactory>.Property.compare:
							{
								ValueChangeUtils.replaceCallBack (this, syncs);
								dirty = true;
							}
							break;
						case EditData<NormalRoundFactory>.Property.compareOtherType:
							dirty = true;
							break;
						case EditData<NormalRoundFactory>.Property.canEdit:
							dirty = true;
							break;
						case EditData<NormalRoundFactory>.Property.editType:
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
						if (wrapProperty.p is NormalRoundFactory) {
							switch ((NormalRoundFactory.Property)wrapProperty.n) {
							case NormalRoundFactory.Property.gameFactory:
								dirty = true;
								break;
							case NormalRoundFactory.Property.isChangeSideBetweenRound:
								dirty = true;
								break;
							case NormalRoundFactory.Property.isSwitchPlayer:
								dirty = true;
								break;
							case NormalRoundFactory.Property.isDifferentInTeam:
								dirty = true;
								break;
							case NormalRoundFactory.Property.calculateScore:
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
				// isChangeSideBetweenRound, isSwitchPlayer, isDifferentInTeam
				if (wrapProperty.p is RequestChangeBoolUI.UIData) {
					return;
				}
				if (wrapProperty.p is GameFactoryUI.UIData) {
					return;
				}
				// calculateScoreType
				if (wrapProperty.p is RequestChangeEnumUI.UIData) {
					return;
				}
				// calculateScoreUI
				if (wrapProperty.p is CalculateScore.UIData) {
					return;
				}
			}
			Debug.LogError ("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
		}

		#endregion

	}
}