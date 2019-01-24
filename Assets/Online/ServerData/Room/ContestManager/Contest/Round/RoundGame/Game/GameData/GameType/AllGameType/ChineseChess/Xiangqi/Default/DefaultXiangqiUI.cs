﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Xiangqi
{
	public class DefaultXiangqiUI : UIBehavior<DefaultXiangqiUI.UIData>
	{

		#region UIData

		public class UIData : DefaultGameTypeUI
		{

			public VP<EditData<DefaultXiangqi>> editDefaultXiangqi;

			public VP<MiniGameDataUI.UIData> miniGameDataUIData;

			#region Constructor

			public enum Property
			{
				editDefaultXiangqi,
				miniGameDataUIData
			}

			public UIData() : base()
			{
				this.editDefaultXiangqi = new VP<EditData<DefaultXiangqi>>(this, (byte)Property.editDefaultXiangqi, new EditData<DefaultXiangqi>());
				this.miniGameDataUIData = new VP<MiniGameDataUI.UIData>(this, (byte)Property.miniGameDataUIData, new MiniGameDataUI.UIData());
			}

			#endregion

			public override GameType.Type getType ()
			{
				return GameType.Type.Xiangqi;
			}

		}

		#endregion

		#region Refresh

		#region txt

		public Text lbTitle;
		public static readonly TxtLanguage txtTitle = new TxtLanguage();

		static DefaultXiangqiUI()
		{
			txtTitle.add (Language.Type.vi, "Mặc Định Cờ Tướng");
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
					EditData<DefaultXiangqi> editDefaultXiangqi = this.data.editDefaultXiangqi.v;
					if (editDefaultXiangqi != null) {
						editDefaultXiangqi.update ();
						// get show
						DefaultXiangqi show = editDefaultXiangqi.show.v.data;
						DefaultXiangqi compare = editDefaultXiangqi.compare.v.data;
						if (show != null) {
							// differentIndicator
							if (differentIndicator != null) {
								bool isDifferent = false;
								{
									if (editDefaultXiangqi.compareOtherType.v.data != null) {
										if (editDefaultXiangqi.compareOtherType.v.data.GetType () != show.GetType ()) {
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
									if (compare != null) {
										Debug.LogError ("serverState: " + serverState + "; " + this);
									}
								}
								// reset?
								if (needReset) {
									needReset = false;
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
												// Find xiangqi
												Xiangqi xiangqi = gameData.gameType.newOrOld<Xiangqi> ();
												{
													// Make new xiangqi to update
													Xiangqi newXiangqi = (Xiangqi)show.makeDefaultGameType ();
													// Copy
													DataUtils.copyData (xiangqi, newXiangqi);
												}
												gameData.gameType.v = xiangqi;
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
						Debug.LogError ("editDefaultXiangqi null: " + this);
					}
					// txt
					{
						if (lbTitle != null) {
							lbTitle.text = txtTitle.get ("Default Xiangqi");
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

		public MiniGameDataUI miniGameDataUIPrefab;
		public Transform miniGameDataUIContainer;

		private Server server = null;

		public override void onAddCallBack<T> (T data)
		{
			if (data is UIData) {
				UIData uiData = data as UIData;
				// Setting
				Setting.get ().addCallBack (this);
				// Child
				{
					uiData.editDefaultXiangqi.allAddCallBack (this);
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
				// editDefaultXiangqi
				{
					if (data is EditData<DefaultXiangqi>) {
						EditData<DefaultXiangqi> editDefaultXiangqi = data as EditData<DefaultXiangqi>;
						// Child
						{
							editDefaultXiangqi.show.allAddCallBack (this);
							editDefaultXiangqi.compare.allAddCallBack (this);
						}
						dirty = true;
						return;
					}
					// Child
					{
						if (data is DefaultXiangqi) {
							DefaultXiangqi defaultXiangqi = data as DefaultXiangqi;
							// Parent
							{
								DataUtils.addParentCallBack (defaultXiangqi, this, ref this.server);
							}
							needReset = true;
							miniGameDataDirty = true;
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
					uiData.editDefaultXiangqi.allRemoveCallBack (this);
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
				// editDefaultXiangqi
				{
					if (data is EditData<DefaultXiangqi>) {
						EditData<DefaultXiangqi> editDefaultXiangqi = data as EditData<DefaultXiangqi>;
						// Child
						{
							editDefaultXiangqi.show.allRemoveCallBack (this);
							editDefaultXiangqi.compare.allRemoveCallBack (this);
						}
						return;
					}
					// Child
					{
						if (data is DefaultXiangqi) {
							DefaultXiangqi defaultXiangqi = data as DefaultXiangqi;
							// Parent
							{
								DataUtils.removeParentCallBack (defaultXiangqi, this, ref this.server);
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
				case UIData.Property.editDefaultXiangqi:
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
			// EditDefaultXiangqi
			{
				if (wrapProperty.p is EditData<DefaultXiangqi>) {
					switch ((EditData<DefaultXiangqi>.Property)wrapProperty.n) {
					case EditData<DefaultXiangqi>.Property.origin:
						dirty = true;
						break;
					case EditData<DefaultXiangqi>.Property.show:
						{
							ValueChangeUtils.replaceCallBack (this, syncs);
							dirty = true;
						}
						break;
					case EditData<DefaultXiangqi>.Property.compare:
						{
							ValueChangeUtils.replaceCallBack (this, syncs);
							dirty = true;
						}
						break;
					case EditData<DefaultXiangqi>.Property.compareOtherType:
						dirty = true;
						break;
					case EditData<DefaultXiangqi>.Property.canEdit:
						dirty = true;
						break;
					case EditData<DefaultXiangqi>.Property.editType:
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
					if (wrapProperty.p is DefaultXiangqi) {
						switch ((DefaultXiangqi.Property)wrapProperty.n) {
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
						Debug.LogError ("unknown wrapProperty: " + wrapProperty + "; " + this);
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
							Debug.LogError ("unknown wrapProperty: " + wrapProperty + "; " + this);
							break;
						}
						return;
					}
					// GameType
					{
						if (Generic.IsAddCallBackInterface<T>()) {
							ValueChangeUtils.replaceCallBack (this, syncs);
						}
						dirty = true;
						return;
					}
				}
			}
			// Debug.LogError ("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
		}

		#endregion

	}
}