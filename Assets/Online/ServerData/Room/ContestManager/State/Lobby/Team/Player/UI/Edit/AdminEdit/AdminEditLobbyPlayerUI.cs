﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace GameManager.Match
{
	public class AdminEditLobbyPlayerUI : UIBehavior<AdminEditLobbyPlayerUI.UIData>
	{

		#region UIData

		public class UIData : EditLobbyPlayerUI.UIData.Sub
		{

			public VP<ReferenceData<LobbyPlayer>> lobbyPlayer;

			#region Sub

			public VP<GamePlayer.Inform.Type> show;

			public VP<AdminEditLobbyPlayerHumanUI.UIData> human;

			public VP<AdminEditLobbyPlayerComputerUI.UIData> computer;

			public VP<AdminEditLobbyPlayerEmptyUI.UIData> empty;

			#endregion

			#region Constructor

			public enum Property
			{
				lobbyPlayer,
				show,
				human,
				computer,
				empty
			}

			public UIData() : base()
			{
				this.lobbyPlayer = new VP<ReferenceData<LobbyPlayer>>(this, (byte)Property.lobbyPlayer, new ReferenceData<LobbyPlayer>(null));
				// sub
				{
					this.show = new VP<GamePlayer.Inform.Type>(this, (byte)Property.show, GamePlayer.Inform.Type.Human);
					this.human = new VP<AdminEditLobbyPlayerHumanUI.UIData>(this, (byte)Property.human, new AdminEditLobbyPlayerHumanUI.UIData());
					this.computer = new VP<AdminEditLobbyPlayerComputerUI.UIData>(this, (byte)Property.computer, new AdminEditLobbyPlayerComputerUI.UIData());
					this.empty = new VP<AdminEditLobbyPlayerEmptyUI.UIData>(this, (byte)Property.empty, new AdminEditLobbyPlayerEmptyUI.UIData());
				}
			}

			#endregion

			public override RoomUser.Role getType ()
			{
				return RoomUser.Role.ADMIN;
			}

			public void reset()
			{
				GamePlayer.Inform.Type type = GamePlayer.Inform.Type.Human;
				{
					LobbyPlayer lobbyPlayer = this.lobbyPlayer.v.data;
					if (lobbyPlayer != null) {
						GamePlayer.Inform inform = lobbyPlayer.inform.v;
						if (inform != null) {
							switch (inform.getType ()) {
							case GamePlayer.Inform.Type.Human:
								type = GamePlayer.Inform.Type.Human;
								break;
							case GamePlayer.Inform.Type.Computer:
								type = GamePlayer.Inform.Type.Computer;
								break;
							case GamePlayer.Inform.Type.None:
								type = GamePlayer.Inform.Type.Human;
								break;
							default:
								Debug.LogError ("unknown type: " + inform.getType () + "; " + this);
								break;
							}
						} else {
							Debug.LogError ("inform null: " + this);
						}
					} else {
						Debug.LogError ("lobbyPlayer null: " + this);
					}
				}
				this.show.v = type;
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

		public Text tvHuman;
		public static readonly TxtLanguage txtHuman = new TxtLanguage ();

		public Text tvComputer;
		public static readonly TxtLanguage txtComputer = new TxtLanguage();

		public Text tvEmpty;
		public static readonly TxtLanguage txtEmpty = new TxtLanguage ();

		static AdminEditLobbyPlayerUI()
		{
			txtHuman.add (Language.Type.vi, "Con người");
			txtComputer.add (Language.Type.vi, "Máy");
			txtEmpty.add (Language.Type.vi, "Để trống");
		}

		#endregion

		public Button btnHuman;
		public Button btnComputer;
		public Button btnEmpty;

		public override void refresh ()
		{
			if (dirty) {
				dirty = false;
				if (this.data != null) {
					LobbyPlayer lobbyPlayer = this.data.lobbyPlayer.v.data;
					if (lobbyPlayer != null) {
						// show
						{
							if (humanContainer != null && computerContainer != null && emptyContainer != null) {
								if (btnHuman != null && btnComputer != null && btnEmpty != null) {
									switch (this.data.show.v) {
									case GamePlayer.Inform.Type.Human:
										{
											// container
											{
												humanContainer.gameObject.SetActive (true);
												computerContainer.gameObject.SetActive (false);
												emptyContainer.gameObject.SetActive (false);
											}
											// btn
											{
												btnHuman.enabled = false;
												btnComputer.enabled = true;
												btnEmpty.enabled = true;
											}
										}
										break;
									case GamePlayer.Inform.Type.Computer:
										{
											// container
											{
												humanContainer.gameObject.SetActive (false);
												computerContainer.gameObject.SetActive (true);
												emptyContainer.gameObject.SetActive (false);
											}
											// btn
											{
												btnHuman.enabled = true;
												btnComputer.enabled = false;
												btnEmpty.enabled = true;
											}
										}
										break;
									case GamePlayer.Inform.Type.None:
										{
											// container
											{
												humanContainer.gameObject.SetActive (false);
												computerContainer.gameObject.SetActive (false);
												emptyContainer.gameObject.SetActive (true);
											}
											// btn
											{
												btnHuman.enabled = true;
												btnComputer.enabled = true;
												btnEmpty.enabled = false;
											}
										}
										break;
									default:
										Debug.LogError ("unknown show : " + this.data.show.v);
										break;
									}
								} else {
									Debug.LogError ("btn null: " + this);
								}
							} else {
								Debug.LogError ("container null: " + this);
							}
						}
						// sub
						{
							// human
							if (this.data.human.v != null) {
								this.data.human.v.lobbyPlayer.v = new ReferenceData<LobbyPlayer> (lobbyPlayer);
							} else {
								Debug.LogError ("human null: " + this);
							}
							// computer
							if (this.data.computer.v != null) {
								this.data.computer.v.lobbyPlayer.v = new ReferenceData<LobbyPlayer> (lobbyPlayer);
							} else {
								Debug.LogError ("computer null: " + this);
							}
							// empty
							if (this.data.empty.v != null) {
								this.data.empty.v.lobbyPlayer.v = new ReferenceData<LobbyPlayer> (lobbyPlayer);
							} else {
								Debug.LogError ("empty null: " + this);
							}
						}
					} else {
						Debug.LogError ("lobbyPlayer null: " + this);
					}
					// txt
					{
						if (tvHuman != null) {
							tvHuman.text = txtHuman.get ("Human");
						} else {
							Debug.LogError ("tvHuman null: " + this);
						}
						if (tvComputer != null) {
							tvComputer.text = txtComputer.get ("Computer");
						} else {
							Debug.LogError ("tvComputer null: " + this);
						}
						if (tvEmpty != null) {
							tvEmpty.text = txtEmpty.get ("Empty");
						} else {
							Debug.LogError ("tvEmpty null: " + this);
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

		public AdminEditLobbyPlayerHumanUI humanPrefab;
		public AdminEditLobbyPlayerComputerUI computerPrefab;
		public AdminEditLobbyPlayerEmptyUI emptyPrefab;

		public Transform humanContainer;
		public Transform computerContainer;
		public Transform emptyContainer;

		public override void onAddCallBack<T> (T data)
		{
			if (data is UIData) {
				UIData uiData = data as UIData;
				// Setting
				Setting.get().addCallBack(this);
				// Child
				{
					uiData.lobbyPlayer.allAddCallBack (this);
					uiData.human.allAddCallBack(this);
					uiData.computer.allAddCallBack (this);
					uiData.empty.allAddCallBack (this);
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
				if (data is LobbyPlayer) {
					// Reset
					{
						if (this.data != null) {
							this.data.reset ();
						} else {
							Debug.LogError ("data null: " + this);
						}
					}
					dirty = true;
					return;
				}
				if (data is AdminEditLobbyPlayerHumanUI.UIData) {
					AdminEditLobbyPlayerHumanUI.UIData humanUIData = data as AdminEditLobbyPlayerHumanUI.UIData;
					// UI
					{
						UIUtils.Instantiate (humanUIData, humanPrefab, humanContainer);
					}
					dirty = true;
					return;
				}
				if (data is AdminEditLobbyPlayerComputerUI.UIData) {
					AdminEditLobbyPlayerComputerUI.UIData computerUIData = data as AdminEditLobbyPlayerComputerUI.UIData;
					// UI
					{
						UIUtils.Instantiate (computerUIData, computerPrefab, computerContainer);
					}
					dirty = true;
					return;
				}
				if (data is  AdminEditLobbyPlayerEmptyUI.UIData) {
					AdminEditLobbyPlayerEmptyUI.UIData emptyUIData = data as AdminEditLobbyPlayerEmptyUI.UIData;
					// UI
					{
						UIUtils.Instantiate (emptyUIData, emptyPrefab, emptyContainer);
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
					uiData.lobbyPlayer.allRemoveCallBack (this);
					uiData.human.allRemoveCallBack(this);
					uiData.computer.allRemoveCallBack (this);
					uiData.empty.allRemoveCallBack (this);
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
				if (data is LobbyPlayer) {
					return;
				}
				if (data is AdminEditLobbyPlayerHumanUI.UIData) {
					AdminEditLobbyPlayerHumanUI.UIData humanUIData = data as AdminEditLobbyPlayerHumanUI.UIData;
					// UI
					{
						humanUIData.removeCallBackAndDestroy (typeof(AdminEditLobbyPlayerHumanUI));
					}
					return;
				}
				if (data is AdminEditLobbyPlayerComputerUI.UIData) {
					AdminEditLobbyPlayerComputerUI.UIData computerUIData = data as AdminEditLobbyPlayerComputerUI.UIData;
					// UI
					{
						computerUIData.removeCallBackAndDestroy (typeof(AdminEditLobbyPlayerComputerUI));
					}
					return;
				}
				if (data is  AdminEditLobbyPlayerEmptyUI.UIData) {
					AdminEditLobbyPlayerEmptyUI.UIData emptyUIData = data as AdminEditLobbyPlayerEmptyUI.UIData;
					// UI
					{
						emptyUIData.removeCallBackAndDestroy (typeof(AdminEditLobbyPlayerEmptyUI));	
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
				case UIData.Property.lobbyPlayer:
					{
						ValueChangeUtils.replaceCallBack (this, syncs);
						dirty = true;
					}
					break;
				case UIData.Property.show:
					dirty = true;
					break;
				case UIData.Property.human:
					{
						ValueChangeUtils.replaceCallBack (this, syncs);
						dirty = true;
					}
					break;
				case UIData.Property.computer:
					{
						ValueChangeUtils.replaceCallBack (this, syncs);
						dirty = true;
					}
					break;
				case UIData.Property.empty:
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
				if (wrapProperty.p is LobbyPlayer) {
					return;
				}
				if (wrapProperty.p is AdminEditLobbyPlayerHumanUI.UIData) {
					return;
				}
				if (wrapProperty.p is AdminEditLobbyPlayerComputerUI.UIData) {
					return;
				}
				if (wrapProperty.p is  AdminEditLobbyPlayerEmptyUI.UIData) {
					return;
				}
			}
			Debug.LogError ("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
		}

		#endregion

		public void onClickBtnHuman()
		{
			if (this.data != null) {
				this.data.show.v = GamePlayer.Inform.Type.Human;
			} else {
				Debug.LogError ("data null: " + this);
			}
		}

		public void onClickBtnComputer()
		{
			if (this.data != null) {
				this.data.show.v = GamePlayer.Inform.Type.Computer;
			} else {
				Debug.LogError ("data null: " + this);
			}
		}

		public void onClickBtnEmpty()
		{
			if (this.data != null) {
				this.data.show.v = GamePlayer.Inform.Type.None;
			} else {
				Debug.LogError ("data null: " + this);
			}
		}

	}
}