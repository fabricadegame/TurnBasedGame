﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace UndoRedo
{
	public class AskHumanUI : UIBehavior<AskHumanUI.UIData>
	{

		#region UIData

		public class UIData : Data
		{

			public VP<ReferenceData<Human>> human;

			public VP<AccountAvatarUI.UIData> avatar;

			#region Constructor

			public enum Property
			{
				human,
				avatar
			}

			public UIData() : base()
			{
				this.human = new VP<ReferenceData<Human>>(this, (byte)Property.human, new ReferenceData<Human>(null));
				this.avatar = new VP<AccountAvatarUI.UIData>(this, (byte)Property.avatar, new AccountAvatarUI.UIData());
			}

			#endregion

		}

		#endregion

		#region Refresh

		#region txt

		public static readonly TxtLanguage txtNone = new TxtLanguage ();
		public static readonly TxtLanguage txtAccept = new TxtLanguage ();
		public static readonly TxtLanguage txtCancel = new TxtLanguage();

		static AskHumanUI()
		{
			txtNone.add (Language.Type.vi, "Không");
			txtAccept.add (Language.Type.vi, "Chấp Nhận");
			txtCancel.add (Language.Type.vi, "Huỷ Bỏ");
		}

		#endregion

		public Text tvName;
		public Text tvAnswer;

		public override void refresh ()
		{
			if (dirty) {
				dirty = false;
				if (this.data != null) {
					Human human = this.data.human.v.data;
					if (human != null) {
						// avatar
						{
							AccountAvatarUI.UIData avatarUIData = this.data.avatar.v;
							if (avatarUIData != null) {
								avatarUIData.account.v = new ReferenceData<Account> (human.account.v);
							} else {
								Debug.LogError ("Don't process: " + avatarUIData + "; " + this);
							}
						}
						// tvName
						if (tvName != null) {
							string name = "";
							{
								if (human.account.v != null) {
									name = human.getPlayerName ();
								} else {
									Debug.LogError ("account null: " + this);
								}
							}
							tvName.text = "" + name;
						} else {
							Debug.LogError ("tvName null: " + this);
						}
						// tvAnswer
						if (tvAnswer != null) {
							// find answer
							Ask.Answer answer = Ask.Answer.None;
							{
								Ask ask = human.findDataInParent<Ask> ();
								if (ask != null) {
									if (ask.accepts.vs.Contains (human.playerId.v)) {
										answer = Ask.Answer.Accept;
									} else if (ask.cancels.vs.Contains (human.playerId.v)) {
										answer = Ask.Answer.Cancel;
									}
								} else {
									Debug.LogError ("ask null: " + this);
								}
							}
							// Process
							switch (answer) {
							case Ask.Answer.None:
								tvAnswer.text = txtNone.get ("None");
								break;
							case Ask.Answer.Accept:
								tvAnswer.text = txtAccept.get ("Accept");
								break;
							case Ask.Answer.Cancel:
								tvAnswer.text = txtCancel.get ("Cancel");
								break;
							default:
								Debug.LogError ("unknown answer: " + answer + "; " + this);
								break;
							}
						} else {
							Debug.LogError ("tvAnswer null: " + this);
						}
					} else {
						Debug.LogError ("human null: " + this);
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

		private Ask ask = null;

		public AccountAvatarUI avatarPrefab;
		public Transform avatarContainer;

		public override void onAddCallBack<T> (T data)
		{
			if (data is UIData) {
				UIData uiData = data as UIData;
				// Setting
				Setting.get().addCallBack(this);
				// Child
				{
					uiData.human.allAddCallBack (this);
					uiData.avatar.allAddCallBack (this);
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
				// Human
				{
					if (data is Human) {
						Human human = data as Human;
						// Parent
						{
							DataUtils.addParentCallBack (human, this, ref this.ask);
						}
						// Child
						{
							human.account.allAddCallBack (this);
						}
						dirty = true;
						return;
					}
					// Parent
					if (data is Ask) {
						dirty = true;
						return;
					}
					// Child
					if (data is Account) {
						dirty = true;
						return;
					}
				}
				// avatar
				if (data is AccountAvatarUI.UIData) {
					AccountAvatarUI.UIData accountAvatarUIData = data as AccountAvatarUI.UIData;
					// UI
					{
						UIUtils.Instantiate (accountAvatarUIData, avatarPrefab, avatarContainer);
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
					uiData.human.allRemoveCallBack (this);
					uiData.avatar.allRemoveCallBack (this);
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
				// Human
				{
					if (data is Human) {
						Human human = data as Human;
						// Parent
						{
							DataUtils.removeParentCallBack (human, this, ref this.ask);
						}
						// Child
						{
							human.account.allRemoveCallBack (this);
						}
						return;
					}
					// Parent
					if (data is Ask) {
						return;
					}
					// Child
					if (data is Account) {
						return;
					}
				}
				// avatar
				if (data is AccountAvatarUI.UIData) {
					AccountAvatarUI.UIData accountAvatarUIData = data as AccountAvatarUI.UIData;
					// UI
					{
						accountAvatarUIData.removeCallBackAndDestroy (typeof(AccountAvatarUI));
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
				case UIData.Property.human:
					{
						ValueChangeUtils.replaceCallBack (this, syncs);
						dirty = true;
					}
					break;
				case UIData.Property.avatar:
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
				// Human
				{
					if (wrapProperty.p is Human) {
						switch ((Human.Property)wrapProperty.n) {
						case Human.Property.playerId:
							dirty = true;
							break;
						case Human.Property.account:
							{
								ValueChangeUtils.replaceCallBack (this, syncs);
								dirty = true;
							}
							break;
						case Human.Property.state:
							break;
						case Human.Property.email:
							break;
						case Human.Property.phoneNumber:
							break;
						case Human.Property.status:
							break;
						case Human.Property.birthday:
							break;
						case Human.Property.sex:
							break;
						case Human.Property.connection:
							break;
						case Human.Property.ban:
							break;
						default:
							Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
							break;
						}
						return;
					}
					// Parent
					if (wrapProperty.p is Ask) {
						switch ((Ask.Property)wrapProperty.n) {
						case Ask.Property.requestInform:
							break;
						case Ask.Property.whoCanAsks:
							break;
						case Ask.Property.accepts:
							dirty = true;
							break;
						case Ask.Property.cancels:
							dirty = true;
							break;
						default:
							Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
							break;
						}
						return;
					}
					// Child
					if (wrapProperty.p is Account) {
						Account.OnUpdateSyncAccount (wrapProperty, this);
						return;
					}
				}
				// avatar
				if (wrapProperty.p is AccountAvatarUI.UIData) {
					return;
				}
			}
			Debug.LogError ("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
		}

		#endregion

	}
}