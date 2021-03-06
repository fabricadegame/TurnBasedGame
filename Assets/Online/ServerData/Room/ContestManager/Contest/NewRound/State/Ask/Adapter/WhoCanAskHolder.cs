﻿using UnityEngine;
using UnityEngine.UI;
using frame8.Logic.Misc.Visual.UI.ScrollRectItemsAdapter;
using System;
using System.Collections;
using System.Collections.Generic;

namespace GameManager.Match
{
    public class WhoCanAskHolder : SriaHolderBehavior<WhoCanAskHolder.UIData>
    {

        #region UIData

        public class UIData : BaseItemViewsHolder
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

            public void updateView(WhoCanAskAdapter.UIData myParams)
            {
                // Find
                Human human = null;
                {
                    if (ItemIndex >= 0 && ItemIndex < myParams.humans.Count)
                    {
                        human = myParams.humans[ItemIndex];
                    }
                    else
                    {
                        Debug.LogError("ItemIdex error: " + this);
                    }
                }
                // Update
                this.human.v = new ReferenceData<Human>(human);
            }

        }

        #endregion

        #region txt, rect

        static WhoCanAskHolder()
        {
            // rect
            {
                // avatarRect
                {
                    // anchoredPosition: (0.0, -4.0); anchorMin: (0.5, 1.0); anchorMax: (0.5, 1.0); pivot: (0.5, 1.0);
                    // offsetMin: (-18.0, -40.0); offsetMax: (18.0, -4.0); sizeDelta: (36.0, 36.0);
                    avatarRect.anchoredPosition = new Vector3(0.0f, -4.0f, 0.0f);
                    avatarRect.anchorMin = new Vector2(0.5f, 1.0f);
                    avatarRect.anchorMax = new Vector2(0.5f, 1.0f);
                    avatarRect.pivot = new Vector2(0.5f, 1.0f);
                    avatarRect.offsetMin = new Vector2(-18.0f, -40.0f);
                    avatarRect.offsetMax = new Vector2(18.0f, -4.0f);
                    avatarRect.sizeDelta = new Vector2(36.0f, 36.0f);
                }
            }
        }

        #endregion

        #region Refresh

        public Text tvName;

        public Sprite ivAnswerNone;
        public Sprite ivAnswerAccept;
        public Sprite ivAnswerCancel;
        public Image ivAnswer;

        public override void refresh()
        {
            base.refresh();
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    Human human = this.data.human.v.data;
                    if (human != null)
                    {
                        // avatar
                        {
                            AccountAvatarUI.UIData avatar = this.data.avatar.v;
                            if (avatar != null)
                            {
                                avatar.account.v = new ReferenceData<Account>(human.account.v);
                            }
                            else
                            {
                                Debug.LogError("avatar null: " + this);
                            }
                        }
                        // tvName
                        {
                            if (tvName != null)
                            {
                                tvName.text = human.getPlayerName();
                            }
                            else
                            {
                                Debug.LogError("tvName null: " + this);
                            }
                        }
                        // ivAnswer
                        {
                            if (ivAnswer != null)
                            {
                                bool alreadyAccept = false;
                                {
                                    RequestNewRoundStateAsk requestNewRoundStateAsk = human.findDataInParent<RequestNewRoundStateAsk>();
                                    if (requestNewRoundStateAsk != null)
                                    {
                                        alreadyAccept = requestNewRoundStateAsk.accepts.vs.Contains(human.playerId.v);
                                    }
                                    else
                                    {
                                        Debug.LogError("requestNewRoundStateAsk null: " + this);
                                    }
                                }
                                ivAnswer.sprite = alreadyAccept ? ivAnswerAccept : ivAnswerNone;
                            }
                            else
                            {
                                Debug.LogError("tvAnswer null: " + this);
                            }
                        }
                        // siblingIndex
                        {
                            UIRectTransform.SetSiblingIndex(this.data.avatar.v, 0);
                            if (ivAnswer != null)
                            {
                                ivAnswer.transform.SetSiblingIndex(1);
                            }
                            else
                            {
                                Debug.LogError("ivAnswer null");
                            }
                        }
                    }
                    else
                    {
                        // Debug.LogError ("human null: " + this);
                    }
                }
                else
                {
                    // Debug.LogError ("data null: " + this);
                }
            }
        }

        #endregion

        #region implement callBacks

        private RequestNewRoundStateAsk requestNewRoundStateAsk = null;

        public AccountAvatarUI avatarPrefab;
        private static readonly UIRectTransform avatarRect = new UIRectTransform();

        public override void onAddCallBack<T>(T data)
        {
            if (data is UIData)
            {
                UIData uiData = data as UIData;
                // Child
                {
                    uiData.human.allAddCallBack(this);
                    uiData.avatar.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            {
                // Human
                {
                    if (data is Human)
                    {
                        Human human = data as Human;
                        // Parent
                        {
                            DataUtils.addParentCallBack(human, this, ref this.requestNewRoundStateAsk);
                        }
                        // Child
                        {
                            human.account.allAddCallBack(this);
                        }
                        dirty = true;
                        return;
                    }
                    // Child
                    if (data is Account)
                    {
                        dirty = true;
                        return;
                    }
                }
                // Parent
                if (data is RequestNewRoundStateAsk)
                {
                    dirty = true;
                    return;
                }
                // avatar
                if (data is AccountAvatarUI.UIData)
                {
                    AccountAvatarUI.UIData accountAvatarUIData = data as AccountAvatarUI.UIData;
                    // UI
                    {
                        UIUtils.Instantiate(accountAvatarUIData, avatarPrefab, this.transform, avatarRect);
                    }
                    dirty = true;
                    return;
                }
            }
            Debug.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if (data is UIData)
            {
                UIData uiData = data as UIData;
                // Child
                {
                    uiData.human.allRemoveCallBack(this);
                    uiData.avatar.allRemoveCallBack(this);
                }
                this.setDataNull(uiData);
                return;
            }
            // Child
            {
                // Human
                {
                    if (data is Human)
                    {
                        Human human = data as Human;
                        // Parent
                        {
                            DataUtils.removeParentCallBack(human, this, ref this.requestNewRoundStateAsk);
                        }
                        // Child
                        {
                            human.account.allRemoveCallBack(this);
                        }
                        return;
                    }
                    // Parent
                    if (data is RequestNewRoundStateAsk)
                    {
                        return;
                    }
                    // Child
                    if (data is Account)
                    {
                        return;
                    }
                }
                // avatar
                if (data is AccountAvatarUI.UIData)
                {
                    AccountAvatarUI.UIData accountAvatarUIData = data as AccountAvatarUI.UIData;
                    // UI
                    {
                        accountAvatarUIData.removeCallBackAndDestroy(typeof(AccountAvatarUI));
                    }
                    return;
                }
            }
            Debug.LogError("Don't process: " + data + "; " + this);
        }

        public override void onUpdateSync<T>(WrapProperty wrapProperty, List<Sync<T>> syncs)
        {
            if (WrapProperty.checkError(wrapProperty))
            {
                return;
            }
            if (wrapProperty.p is UIData)
            {
                switch ((UIData.Property)wrapProperty.n)
                {
                    case UIData.Property.human:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case UIData.Property.avatar:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    default:
                        Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                        break;
                }
                return;
            }
            // Child
            {
                // Human
                {
                    if (wrapProperty.p is Human)
                    {
                        switch ((Human.Property)wrapProperty.n)
                        {
                            case Human.Property.playerId:
                                break;
                            case Human.Property.account:
                                {
                                    ValueChangeUtils.replaceCallBack(this, syncs);
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
                                Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                                break;
                        }
                        return;
                    }
                    // Parent
                    if (wrapProperty.p is RequestNewRoundStateAsk)
                    {
                        switch ((RequestNewRoundStateAsk.Property)wrapProperty.n)
                        {
                            case RequestNewRoundStateAsk.Property.whoCanAsks:
                                break;
                            case RequestNewRoundStateAsk.Property.accepts:
                                dirty = true;
                                break;
                            default:
                                Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                                break;
                        }
                        return;
                    }
                    // Child
                    if (wrapProperty.p is Account)
                    {
                        Account.OnUpdateSyncAccount(wrapProperty, this);
                        return;
                    }
                }
                // avatar
                if (wrapProperty.p is AccountAvatarUI.UIData)
                {
                    return;
                }
            }
            Debug.LogError("Don't process: " + wrapProperty + "; " + this);
        }

        #endregion

    }
}