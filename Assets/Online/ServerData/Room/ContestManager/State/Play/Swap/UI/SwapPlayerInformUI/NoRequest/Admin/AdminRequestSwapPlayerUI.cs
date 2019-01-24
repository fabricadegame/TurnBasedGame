﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace GameManager.Match.Swap
{
	public class AdminRequestSwapPlayerUI : UIBehavior<AdminRequestSwapPlayerUI.UIData>
	{

		#region UIData

		public class UIData : NoRequestSwapPlayerUI.UIData.Sub
		{

			public VP<ReferenceData<TeamPlayer>> teamPlayer;

			public VP<GamePlayer.Inform.Type> show;

			public VP<AdminRequestSwapPlayerHumanUI.UIData> human;

			public VP<AdminRequestSwapPlayerComputerUI.UIData> computer;

			#region Constructor

			public enum Property
			{
				teamPlayer,
				show,
				human,
				computer
			}

			public UIData() : base()
			{
				this.teamPlayer = new VP<ReferenceData<TeamPlayer>>(this, (byte)Property.teamPlayer, new ReferenceData<TeamPlayer>(null));
				this.show = new VP<GamePlayer.Inform.Type>(this, (byte)Property.show, GamePlayer.Inform.Type.Human);
				this.human = new VP<AdminRequestSwapPlayerHumanUI.UIData>(this, (byte)Property.human, new AdminRequestSwapPlayerHumanUI.UIData());
				this.computer = new VP<AdminRequestSwapPlayerComputerUI.UIData>(this, (byte)Property.computer, new AdminRequestSwapPlayerComputerUI.UIData());
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
					TeamPlayer teamPlayer = this.teamPlayer.v.data;
					if (teamPlayer != null) {
						GamePlayer.Inform inform = teamPlayer.inform.v;
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

		}

		#endregion

		#region Refresh

		public Button btnHuman;
		public Button btnComputer;

		public override void refresh ()
		{
			if (dirty) {
				dirty = false;
				if (this.data != null) {
					TeamPlayer teamPlayer = this.data.teamPlayer.v.data;
					if (teamPlayer != null) {
						// show
						{
							if (humanContainer != null && computerContainer != null) {
								if (btnHuman != null && btnComputer != null) {
									switch (this.data.show.v) {
									case GamePlayer.Inform.Type.Human:
										{
											// container
											{
												humanContainer.gameObject.SetActive (true);
												computerContainer.gameObject.SetActive (false);
											}
											// btn
											{
												btnHuman.enabled = false;
												btnComputer.enabled = true;
											}
										}
										break;
									case GamePlayer.Inform.Type.Computer:
										{
											// container
											{
												humanContainer.gameObject.SetActive (false);
												computerContainer.gameObject.SetActive (true);
											}
											// btn
											{
												btnHuman.enabled = true;
												btnComputer.enabled = false;
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
								this.data.human.v.teamPlayer.v = new ReferenceData<TeamPlayer> (teamPlayer);
							} else {
								Debug.LogError ("human null: " + this);
							}
							// computer
							if (this.data.computer.v != null) {
								this.data.computer.v.teamPlayer.v = new ReferenceData<TeamPlayer> (teamPlayer);
							} else {
								Debug.LogError ("computer null: " + this);
							}
						}
					} else {
						Debug.LogError ("teamPlayer null: " + this);
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

		public AdminRequestSwapPlayerHumanUI humanPrefab;
		public AdminRequestSwapPlayerComputerUI computerPrefab;

		public Transform humanContainer;
		public Transform computerContainer;

		public override void onAddCallBack<T> (T data)
		{
			if (data is UIData) {
				UIData uiData = data as UIData;
				// Child
				{
					uiData.teamPlayer.allAddCallBack (this);
					uiData.human.allAddCallBack(this);
					uiData.computer.allAddCallBack (this);
				}
				dirty = true;
				return;
			}
			// Child
			{
				if (data is TeamPlayer) {
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
				if (data is AdminRequestSwapPlayerHumanUI.UIData) {
					AdminRequestSwapPlayerHumanUI.UIData humanUIData = data as AdminRequestSwapPlayerHumanUI.UIData;
					// UI
					{
						UIUtils.Instantiate (humanUIData, humanPrefab, humanContainer);
					}
					dirty = true;
					return;
				}
				if (data is AdminRequestSwapPlayerComputerUI.UIData) {
					AdminRequestSwapPlayerComputerUI.UIData computerUIData = data as AdminRequestSwapPlayerComputerUI.UIData;
					// UI
					{
						UIUtils.Instantiate (computerUIData, computerPrefab, computerContainer);
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
				// Child
				{
					uiData.teamPlayer.allRemoveCallBack (this);
					uiData.human.allRemoveCallBack(this);
					uiData.computer.allRemoveCallBack (this);
				}
				this.setDataNull (uiData);
				return;
			}
			// Child
			{
				if (data is TeamPlayer) {
					return;
				}
				if (data is AdminRequestSwapPlayerHumanUI.UIData) {
					AdminRequestSwapPlayerHumanUI.UIData humanUIData = data as AdminRequestSwapPlayerHumanUI.UIData;
					// UI
					{
						humanUIData.removeCallBackAndDestroy (typeof(AdminRequestSwapPlayerHumanUI));
					}
					return;
				}
				if (data is AdminRequestSwapPlayerComputerUI.UIData) {
					AdminRequestSwapPlayerComputerUI.UIData computerUIData = data as AdminRequestSwapPlayerComputerUI.UIData;
					// UI
					{
						computerUIData.removeCallBackAndDestroy (typeof(AdminRequestSwapPlayerComputerUI));
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
				case UIData.Property.teamPlayer:
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
				default:
					Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
					break;
				}
				return;
			}
			// Child
			{
				if (wrapProperty.p is TeamPlayer) {
					return;
				}
				if (wrapProperty.p is AdminRequestSwapPlayerHumanUI.UIData) {
					return;
				}
				if (wrapProperty.p is AdminRequestSwapPlayerComputerUI.UIData) {
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

	}
}