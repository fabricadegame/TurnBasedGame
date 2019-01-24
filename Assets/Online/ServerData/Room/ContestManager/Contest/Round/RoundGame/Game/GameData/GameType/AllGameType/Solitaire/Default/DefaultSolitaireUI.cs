﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Solitaire
{
	public class DefaultSolitaireUI : UIBehavior<DefaultSolitaireUI.UIData>
	{

		#region UIData

		public class UIData : DefaultGameTypeUI
		{

			public VP<EditData<DefaultSolitaire>> editDefaultSolitaire;

			#region boardSize

			public VP<RequestChangeIntUI.UIData> drawCount;

			public void makeRequestChangeDrawCount (RequestChangeUpdate<int>.UpdateData update, int newDrawCount)
			{
				// Find
				DefaultSolitaire defaultSolitaire = null;
				{
					EditData<DefaultSolitaire> editDefaultSolitaire = this.editDefaultSolitaire.v;
					if (editDefaultSolitaire != null) {
						defaultSolitaire = editDefaultSolitaire.show.v.data;
					} else {
						Debug.LogError ("editDefaultSolitaire null: " + this);
					}
				}
				// Process
				if (defaultSolitaire != null) {
					defaultSolitaire.requestChangeDrawCount (Server.getProfileUserId(defaultSolitaire), newDrawCount);
				} else {
					Debug.LogError ("defaultSolitaire null: " + this);
				}
			}

			#endregion

			public VP<MiniGameDataUI.UIData> miniGameDataUIData;

			#region Constructor

			public enum Property
			{
				editDefaultSolitaire,
				drawCount,
				miniGameDataUIData
			}

			public UIData() : base()
			{
				this.editDefaultSolitaire = new VP<EditData<DefaultSolitaire>>(this, (byte)Property.editDefaultSolitaire, new EditData<DefaultSolitaire>());
				{
					this.drawCount = new VP<RequestChangeIntUI.UIData>(this, (byte)Property.drawCount, new RequestChangeIntUI.UIData());
					// have limit
					{
						IntLimit.Have have = new IntLimit.Have();
						{
							have.uid = this.drawCount.v.limit.makeId();
							have.min.v = 1;
							have.max.v = 9;
						}
						this.drawCount.v.limit.v = have;
					}
					// event
					this.drawCount.v.updateData.v.request.v = makeRequestChangeDrawCount;
				}
				this.miniGameDataUIData = new VP<MiniGameDataUI.UIData>(this, (byte)Property.miniGameDataUIData, new MiniGameDataUI.UIData());
			}

			#endregion

			public override GameType.Type getType ()
			{
				return GameType.Type.Solitaire;
			}

		}

		#endregion

		#region Refresh

		#region txt

		public Text lbTitle;
		public static readonly TxtLanguage txtTitle = new TxtLanguage();

		public Text lbDrawCount;
		public static readonly TxtLanguage txtDrawCount = new TxtLanguage ();

		static DefaultSolitaireUI()
		{
			txtTitle.add (Language.Type.vi, "Mặc Định Solitaire");
			txtDrawCount.add (Language.Type.vi, "Số lá bài rút");
		}

		#endregion

		private bool needReset = true;
		private bool miniGameDataDirty = true;

		public GameObject differentIndicator;

		public override void refresh ()
		{
			if (dirty) {
				dirty = false;
				if (this.data != null) {
					EditData<DefaultSolitaire> editDefaultSolitaire = this.data.editDefaultSolitaire.v;
					if (editDefaultSolitaire != null) {
						editDefaultSolitaire.update ();
						// get show
						DefaultSolitaire show = editDefaultSolitaire.show.v.data;
						DefaultSolitaire compare = editDefaultSolitaire.compare.v.data;
						if (show != null) {
							// differentIndicator
							if (differentIndicator != null) {
								bool isDifferent = false;
								{
									if (editDefaultSolitaire.compareOtherType.v.data != null) {
										if (editDefaultSolitaire.compareOtherType.v.data.GetType () != show.GetType ()) {
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
									// drawCount
									{
										RequestChangeIntUI.UIData drawCount = this.data.drawCount.v;
										if (drawCount != null) {
											// update
											RequestChangeUpdate<int>.UpdateData updateData = drawCount.updateData.v;
											if (updateData != null) {
												updateData.origin.v = show.drawCount.v;
												updateData.canRequestChange.v = editDefaultSolitaire.canEdit.v;
												updateData.serverState.v = serverState;
											} else {
												Debug.LogError ("updateData null: " + this);
											}
											// compare
											{
												if (compare != null) {
													drawCount.showDifferent.v = true;
													drawCount.compare.v = compare.drawCount.v;
												} else {
													drawCount.showDifferent.v = false;
												}
											}
										} else {
											Debug.LogError ("drawCount null: " + this);
										}
									}
								}
								// reset?
								if (needReset) {
									needReset = false;
									// drawCount
									{
										RequestChangeIntUI.UIData drawCount = this.data.drawCount.v;
										if (drawCount != null) {
											// update
											RequestChangeUpdate<int>.UpdateData updateData = drawCount.updateData.v;
											if (updateData != null) {
												updateData.current.v = show.drawCount.v;
												updateData.changeState.v = Data.ChangeState.None;
											} else {
												Debug.LogError ("updateData null: " + this);
											}
										} else {
											Debug.LogError ("drawCount null: " + this);
										}
									}
								}
							}
							// miniGameDataUIData
							if (miniGameDataDirty) {
								miniGameDataDirty = false;
								// find miniGameDataUIData
								MiniGameDataUI.UIData miniGameDataUIData = this.data.miniGameDataUIData.newOrOld<MiniGameDataUI.UIData> ();
								// Update Property
								{
									// gameData
									{
										// Find GameData
										GameData gameData = null;
										{
											// Find old
											if (miniGameDataUIData.gameData.v.data != null) {
												gameData = miniGameDataUIData.gameData.v.data;
											}
											// Make new
											if (gameData == null) {
												gameData = new GameData ();
												miniGameDataUIData.gameData.v = new ReferenceData<GameData> (gameData);
											}
										}
										// Update Property
										{
											// GameType
											{
												// Find Solitaire
												Solitaire solitaire = gameData.gameType.newOrOld<Solitaire> ();
												{
													// find newSolitaire
													Solitaire newSolitaire = show.makeDefaultGameType () as Solitaire;
													// Copy
													DataUtils.copyData (solitaire, newSolitaire);
												}
												gameData.gameType.v = solitaire;
											}
										}
									}
								}
								this.data.miniGameDataUIData.v = miniGameDataUIData;
							}
						} else {
							Debug.LogError ("show null: " + this);
						}
					} else {
						Debug.LogError ("editDefaultSolitaire null: " + this);
					}
					// txt
					{
						if (lbTitle != null) {
							lbTitle.text = txtTitle.get ("Default Solitaire");
						} else {
							Debug.LogError ("lbTitle null: " + this);
						}
						if (lbDrawCount != null) {
							lbDrawCount.text = txtDrawCount.get ("Draw count");
						} else {
							Debug.LogError ("lbDrawCount null: " + this);
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

		public MiniGameDataUI miniGameDataUIPrefab;
		public Transform miniGameDataUIContainer;

		public RequestChangeIntUI requestIntPrefab;
		public Transform drawCountContainer;

		private Server server = null;

		public override void onAddCallBack<T> (T data)
		{
			if (data is UIData) {
				UIData uiData = data as UIData;
				// Setting
				Setting.get ().addCallBack (this);
				// Child
				{
					uiData.editDefaultSolitaire.allAddCallBack (this);
					uiData.drawCount.allAddCallBack (this);
					uiData.miniGameDataUIData.allAddCallBack (this);
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
				// editDefaultSolitaire
				{
					if (data is EditData<DefaultSolitaire>) {
						EditData<DefaultSolitaire> editDefaultSolitaire = data as EditData<DefaultSolitaire>;
						// Child
						{
							editDefaultSolitaire.show.allAddCallBack (this);
							editDefaultSolitaire.compare.allAddCallBack (this);
						}
						dirty = true;
						return;
					}
					// Child
					{
						if (data is DefaultSolitaire) {
							DefaultSolitaire defaultSolitaire = data as DefaultSolitaire;
							// Parent
							{
								DataUtils.addParentCallBack (defaultSolitaire, this, ref this.server);
							}
							needReset = true;
							miniGameDataDirty = true;
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
				if (data is RequestChangeIntUI.UIData) {
					RequestChangeIntUI.UIData requestChange = data as RequestChangeIntUI.UIData;
					// UI
					{
						WrapProperty wrapProperty = requestChange.p;
						if (wrapProperty != null) {
							switch ((UIData.Property)wrapProperty.n) {
							case UIData.Property.drawCount:
								{
									UIUtils.Instantiate (requestChange, requestIntPrefab, drawCountContainer);
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
				// MiniGameDataUIData
				{
					if (data is MiniGameDataUI.UIData) {
						MiniGameDataUI.UIData miniGameDataUIData = data as MiniGameDataUI.UIData;
						// UI
						{
							UIUtils.Instantiate (miniGameDataUIData, miniGameDataUIPrefab, miniGameDataUIContainer);
						}
						// Child
						{
							miniGameDataUIData.gameData.allAddCallBack (this);
						}
						miniGameDataDirty = true;
						dirty = true;
						return;
					}
					// GameData
					{
						if (data is GameData) {
							GameData gameData = data as GameData;
							{
								gameData.gameType.allAddCallBack (this);
							}
							dirty = true;
							return;
						}
						// GameType
						{
							data.addCallBackAllChildren (this);
							dirty = true;
							return;
						}
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
				Setting.get ().removeCallBack (this);
				// Child
				{
					uiData.editDefaultSolitaire.allRemoveCallBack (this);
					uiData.drawCount.allRemoveCallBack (this);
					uiData.miniGameDataUIData.allRemoveCallBack (this);
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
				// editDefaultSolitaire
				{
					if (data is EditData<DefaultSolitaire>) {
						EditData<DefaultSolitaire> editDefaultSolitaire = data as EditData<DefaultSolitaire>;
						// Child
						{
							editDefaultSolitaire.show.allRemoveCallBack (this);
							editDefaultSolitaire.compare.allRemoveCallBack (this);
						}
						return;
					}
					// Child
					{
						if (data is DefaultSolitaire) {
							DefaultSolitaire defaultSolitaire = data as DefaultSolitaire;
							// Parent
							{
								DataUtils.removeParentCallBack (defaultSolitaire, this, ref this.server);
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
				if (data is RequestChangeIntUI.UIData) {
					RequestChangeIntUI.UIData requestChange = data as RequestChangeIntUI.UIData;
					// UI
					{
						requestChange.removeCallBackAndDestroy (typeof(RequestChangeIntUI));
					}
					return;
				}
				// MiniGameDataUIData
				{
					if (data is MiniGameDataUI.UIData) {
						MiniGameDataUI.UIData miniGameDataUIData = data as MiniGameDataUI.UIData;
						// UI
						{
							miniGameDataUIData.removeCallBackAndDestroy (typeof(MiniGameDataUI));
						}
						// Child
						{
							miniGameDataUIData.gameData.allRemoveCallBack (this);
						}
						return;
					}
					// GameData
					{
						if (data is GameData) {
							GameData gameData = data as GameData;
							{
								gameData.gameType.allRemoveCallBack (this);
							}
							return;
						}
						// GameType
						{
							data.removeCallBackAllChildren (this);
							return;
						}
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
				case UIData.Property.editDefaultSolitaire:
					{
						ValueChangeUtils.replaceCallBack (this, syncs);
						dirty = true;
					}
					break;
				case UIData.Property.drawCount:
					{
						ValueChangeUtils.replaceCallBack (this, syncs);
						dirty = true;
					}
					break;
				case UIData.Property.miniGameDataUIData:
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
				// EditDefaultSolitaire
				{
					if (wrapProperty.p is EditData<DefaultSolitaire>) {
						switch ((EditData<DefaultSolitaire>.Property)wrapProperty.n) {
						case EditData<DefaultSolitaire>.Property.origin:
							dirty = true;
							break;
						case EditData<DefaultSolitaire>.Property.show:
							{
								ValueChangeUtils.replaceCallBack (this, syncs);
								dirty = true;
							}
							break;
						case EditData<DefaultSolitaire>.Property.compare:
							{
								ValueChangeUtils.replaceCallBack (this, syncs);
								dirty = true;
							}
							break;
						case EditData<DefaultSolitaire>.Property.compareOtherType:
							dirty = true;
							break;
						case EditData<DefaultSolitaire>.Property.canEdit:
							dirty = true;
							break;
						case EditData<DefaultSolitaire>.Property.editType:
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
						if (wrapProperty.p is DefaultSolitaire) {
							switch ((DefaultSolitaire.Property)wrapProperty.n) {
							case DefaultSolitaire.Property.drawCount:
								miniGameDataDirty = true;
								dirty = true;
								break;
							default:
								Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
								break;
							}
							return;
						}
						// Parent
						if (wrapProperty.p is Server) {
							Server.State.OnUpdateSyncStateChange (wrapProperty, this);
							return;
						}
					}
				}
				if (wrapProperty.p is RequestChangeIntUI.UIData) {
					return;
				}
				// MiniGameDataUIData
				{
					if (wrapProperty.p is MiniGameDataUI.UIData) {
						switch ((MiniGameDataUI.UIData.Property)wrapProperty.n) {
						case MiniGameDataUI.UIData.Property.gameData:
							{
								ValueChangeUtils.replaceCallBack (this, syncs);
								dirty = true;
							}
							break;
						case MiniGameDataUI.UIData.Property.board:
							break;
						default:
							Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
							break;
						}
						return;
					}
					// GameData
					{
						if (wrapProperty.p is GameData) {
							switch ((GameData.Property)wrapProperty.n) {
							case GameData.Property.gameType:
								{
									ValueChangeUtils.replaceCallBack (this, syncs);
									dirty = true;
								}
								break;
							case GameData.Property.useRule:
								break;
							case GameData.Property.turn:
								break;
							case GameData.Property.timeControl:
								break;
							case GameData.Property.lastMove:
								break;
							case GameData.Property.state:
								break;
							default:
								Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
								break;
							}
							return;
						}
						// GameType
						{
							if (Generic.IsAddCallBackInterface<T> ()) {
								ValueChangeUtils.replaceCallBack (this, syncs);
							}
							dirty = true;
							return;
						}
					}
				}
			}
			// Debug.LogError ("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
		}

		#endregion

	}
}