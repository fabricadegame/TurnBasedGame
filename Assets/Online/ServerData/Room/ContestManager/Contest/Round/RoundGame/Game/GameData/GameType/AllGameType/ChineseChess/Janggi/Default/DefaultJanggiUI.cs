﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Janggi
{
	public class DefaultJanggiUI : UIBehavior<DefaultJanggiUI.UIData>
	{

		#region UIData

		public class UIData : DefaultGameTypeUI
		{

			public VP<EditData<DefaultJanggi>> editDefaultJanggi;

			public VP<MiniGameDataUI.UIData> miniGameDataUIData;

			#region Constructor

			public enum Property
			{
				editDefaultJanggi,
				miniGameDataUIData
			}

			public UIData() : base()
			{
				this.editDefaultJanggi = new VP<EditData<DefaultJanggi>>(this, (byte)Property.editDefaultJanggi, new EditData<DefaultJanggi>());
				this.miniGameDataUIData = new VP<MiniGameDataUI.UIData>(this, (byte)Property.miniGameDataUIData, new MiniGameDataUI.UIData());
			}

			#endregion

			public override GameType.Type getType ()
			{
				return GameType.Type.Janggi;
			}

		}

		#endregion

		#region Refresh

		#region txt

		public Text lbTitle;
		public static readonly TxtLanguage txtTitle = new TxtLanguage();

		static DefaultJanggiUI()
		{
			txtTitle.add (Language.Type.vi, "Mặc Định Cờ Tướng Triều Tiên");
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
					EditData<DefaultJanggi> editDefaultJanggi = this.data.editDefaultJanggi.v;
					if (editDefaultJanggi != null) {
						editDefaultJanggi.update ();
						// get show
						DefaultJanggi show = editDefaultJanggi.show.v.data;
						DefaultJanggi compare = editDefaultJanggi.compare.v.data;
						if (show != null) {
							// differentIndicator
							if (differentIndicator != null) {
								bool isDifferent = false;
								{
									if (editDefaultJanggi.compareOtherType.v.data != null) {
										if (editDefaultJanggi.compareOtherType.v.data.GetType () != show.GetType ()) {
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
												// Find janggi
												Janggi janggi = gameData.gameType.newOrOld<Janggi> ();
												{
													// Make new janggi to update
													Janggi newJanggi = (Janggi)show.makeDefaultGameType ();
													// Copy
													DataUtils.copyData (janggi, newJanggi);
												}
												gameData.gameType.v = janggi;
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
						Debug.LogError ("editDefaultJanggi null: " + this);
					}
					// txt
					{
						if (lbTitle != null) {
							lbTitle.text = txtTitle.get ("Default Janggi");
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
					uiData.editDefaultJanggi.allAddCallBack (this);
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
				// editDefaultJanggi
				{
					if (data is EditData<DefaultJanggi>) {
						EditData<DefaultJanggi> editDefaultJanggi = data as EditData<DefaultJanggi>;
						// Child
						{
							editDefaultJanggi.show.allAddCallBack (this);
							editDefaultJanggi.compare.allAddCallBack (this);
						}
						dirty = true;
						return;
					}
					// Child
					{
						if (data is DefaultJanggi) {
							DefaultJanggi defaultJanggi = data as DefaultJanggi;
							// Parent
							{
								DataUtils.addParentCallBack (defaultJanggi, this, ref this.server);
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
					uiData.editDefaultJanggi.allRemoveCallBack (this);
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
				// editDefaultJanggi
				{
					if (data is EditData<DefaultJanggi>) {
						EditData<DefaultJanggi> editDefaultJanggi = data as EditData<DefaultJanggi>;
						// Child
						{
							editDefaultJanggi.show.allRemoveCallBack (this);
							editDefaultJanggi.compare.allRemoveCallBack (this);
						}
						return;
					}
					// Child
					{
						if (data is DefaultJanggi) {
							DefaultJanggi defaultJanggi = data as DefaultJanggi;
							// Parent
							{
								DataUtils.removeParentCallBack (defaultJanggi, this, ref this.server);
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
				case UIData.Property.editDefaultJanggi:
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
			// EditDefaultJanggi
			{
				if (wrapProperty.p is EditData<DefaultJanggi>) {
					switch ((EditData<DefaultJanggi>.Property)wrapProperty.n) {
					case EditData<DefaultJanggi>.Property.origin:
						dirty = true;
						break;
					case EditData<DefaultJanggi>.Property.show:
						{
							ValueChangeUtils.replaceCallBack (this, syncs);
							dirty = true;
						}
						break;
					case EditData<DefaultJanggi>.Property.compare:
						{
							ValueChangeUtils.replaceCallBack (this, syncs);
							dirty = true;
						}
						break;
					case EditData<DefaultJanggi>.Property.compareOtherType:
						dirty = true;
						break;
					case EditData<DefaultJanggi>.Property.canEdit:
						dirty = true;
						break;
					case EditData<DefaultJanggi>.Property.editType:
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
					if (wrapProperty.p is DefaultJanggi) {
						switch ((DefaultJanggi.Property)wrapProperty.n) {
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