﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UndoRedoRequestMessageUI : UIBehavior<UndoRedoRequestMessageUI.UIData>
{

    #region UIData

    public class UIData : ChatMessageHolder.UIData.Sub
    {

        public VP<ReferenceData<UndoRedoRequestMessage>> undoRedoRequestMessage;

        public VP<AccountAvatarUI.UIData> avatar;

        #region Constructor

        public enum Property
        {
            undoRedoRequestMessage,
            avatar
        }

        public UIData() : base()
        {
            this.undoRedoRequestMessage = new VP<ReferenceData<UndoRedoRequestMessage>>(this, (byte)Property.undoRedoRequestMessage, new ReferenceData<UndoRedoRequestMessage>(null));
            this.avatar = new VP<AccountAvatarUI.UIData>(this, (byte)Property.avatar, new AccountAvatarUI.UIData());
        }

        #endregion

        public override ChatMessage.Content.Type getType()
        {
            return ChatMessage.Content.Type.UndoRedoRequest;
        }

    }

    #endregion

    #region txt, rect

    private static readonly TxtLanguage txtAsk = new TxtLanguage();
    private static readonly TxtLanguage txtAccept = new TxtLanguage();
    private static readonly TxtLanguage txtRefuse = new TxtLanguage();

    private static readonly TxtLanguage txtUndo = new TxtLanguage();
    private static readonly TxtLanguage txtRedo = new TxtLanguage();

    static UndoRedoRequestMessageUI()
    {
        // txt
        {
            txtAsk.add(Language.Type.vi, "yêu cầu");
            txtAccept.add(Language.Type.vi, "chấp nhận");
            txtRefuse.add(Language.Type.vi, "từ chối");

            txtUndo.add(Language.Type.vi, "đi lại");
            txtRedo.add(Language.Type.vi, "đi tiếp");
        }
        // avatarRect
        {
            // anchoredPosition: (0.0, 0.0); anchorMin: (0.0, 0.5); anchorMax: (0.0, 0.5); pivot: (0.0, 0.5);
            // offsetMin: (0.0, -18.0); offsetMax: (36.0, 18.0); sizeDelta: (36.0, 36.0);
            avatarRect.anchoredPosition = new Vector3(0.0f, 0.0f, 0.0f);
            avatarRect.anchorMin = new Vector2(0.0f, 0.5f);
            avatarRect.anchorMax = new Vector2(0.0f, 0.5f);
            avatarRect.pivot = new Vector2(0.0f, 0.5f);
            avatarRect.offsetMin = new Vector2(0.0f, -15.0f);
            avatarRect.offsetMax = new Vector2(30.0f, 15.0f);
            avatarRect.sizeDelta = new Vector2(30.0f, 30.0f);
        }
    }

    #endregion

    #region Refresh

    public Text tvContent;
    public Text tvTime;

    public override void refresh()
    {
        if (dirty)
        {
            dirty = false;
            if (this.data != null)
            {
                UndoRedoRequestMessage undoRedoRequestMessage = this.data.undoRedoRequestMessage.v.data;
                if (undoRedoRequestMessage != null)
                {
                    ChatMessage chatMessage = undoRedoRequestMessage.findDataInParent<ChatMessage>();
                    if (chatMessage != null)
                    {
                        // Find human
                        {
                            Human human = ChatRoom.findHuman(undoRedoRequestMessage, undoRedoRequestMessage.userId.v);
                            if (this.human != human)
                            {
                                // remove old
                                if (this.human != null)
                                {
                                    this.human.removeCallBack(this);
                                }
                                // set new
                                this.human = human;
                                if (this.human != null)
                                {
                                    this.human.addCallBack(this);
                                }
                            }
                        }
                        // Avatar
                        {
                            AccountAvatarUI.UIData accountAvatarUIData = this.data.avatar.v;
                            if (accountAvatarUIData != null)
                            {
                                Account account = null;
                                {
                                    if (this.human != null)
                                    {
                                        account = this.human.account.v;
                                    }
                                }
                                accountAvatarUIData.account.v = new ReferenceData<Account>(account);
                            }
                            else
                            {
                                Debug.LogError("accountAvatarUIData null: " + this);
                            }
                        }
                        // time
                        {
                            if (tvTime != null)
                            {
                                tvTime.text = chatMessage.TimestampAsDateTime.ToString("HH:mm");
                            }
                            else
                            {
                                Debug.LogError("tvTime null");
                            }
                        }
                        // content
                        {
                            if (tvContent != null)
                            {
                                // Find user name
                                string userName = "";
                                {
                                    if (this.human != null)
                                    {
                                        userName = this.human.getPlayerName();
                                    }
                                    else
                                    {
                                        Debug.LogError("human null: " + this);
                                    }
                                }
                                // operation
                                string strOperation = txtUndo.get("undo");
                                {
                                    switch (undoRedoRequestMessage.operation.v)
                                    {
                                        case UndoRedoRequest.Operation.Undo:
                                            strOperation = txtUndo.get("undo");
                                            break;
                                        case UndoRedoRequest.Operation.Redo:
                                            strOperation = txtRedo.get("redo");
                                            break;
                                        default:
                                            Debug.LogError("unknown operation: " + undoRedoRequestMessage.operation.v);
                                            break;
                                    }
                                }
                                // state
                                switch (undoRedoRequestMessage.action.v)
                                {
                                    case UndoRedoRequestMessage.Action.Ask:
                                        tvContent.text = "<color=grey>" + userName + "</color> " + txtAsk.get("request") + " " + strOperation;
                                        break;
                                    case UndoRedoRequestMessage.Action.Accept:
                                        tvContent.text = "<color=grey>" + userName + "</color> " + txtAccept.get("accept") + " " + strOperation;
                                        break;
                                    case UndoRedoRequestMessage.Action.Refuse:
                                        tvContent.text = "<color=grey>" + userName + "</color> " + txtRefuse.get("refuse") + " " + strOperation;
                                        break;
                                    default:
                                        Debug.LogError("unknown action: " + undoRedoRequestMessage.action.v + "; " + this);
                                        break;
                                }
                            }
                            else
                            {
                                Debug.LogError("tvContent null");
                            }
                        }
                    }
                    else
                    {
                        Debug.LogError("chatMessage null: " + this);
                    }
                }
                else
                {
                    Debug.LogError("chatRoomUserStateContent null: " + this);
                }
            }
            else
            {
                Debug.LogError("data null: " + this);
            }
        }
    }

    public override bool isShouldDisableUpdate()
    {
        return true;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        if (this.human != null)
        {
            this.human.removeCallBack(this);
            this.human = null;
        }
        else
        {
            // Debug.LogError ("human null: " + this);
        }
    }

    #endregion

    #region implement callBacks

    private ChatMessage chatMessage = null;

    public AccountAvatarUI avatarPrefab;
    private static readonly UIRectTransform avatarRect = new UIRectTransform();

    private Human human = null;

    public override void onAddCallBack<T>(T data)
    {
        if (data is UIData)
        {
            UIData uiData = data as UIData;
            // Setting
            Setting.get().addCallBack(this);
            // Child
            {
                uiData.undoRedoRequestMessage.allAddCallBack(this);
                uiData.avatar.allAddCallBack(this);
            }
            dirty = true;
            return;
        }
        // Setting
        if (data is Setting)
        {
            dirty = true;
            return;
        }
        // Child
        {
            // UndoRedoRequestMessage
            {
                if (data is UndoRedoRequestMessage)
                {
                    UndoRedoRequestMessage undoRedoRequestMessage = data as UndoRedoRequestMessage;
                    // Parent
                    {
                        DataUtils.addParentCallBack(undoRedoRequestMessage, this, ref this.chatMessage);
                    }
                    dirty = true;
                    return;
                }
                // Parent
                {
                    if (data is ChatMessage)
                    {
                        dirty = true;
                        return;
                    }
                    // Human
                    {
                        if (data is Human)
                        {
                            Human human = data as Human;
                            // Child
                            {
                                human.account.allAddCallBack(this);
                            }
                            dirty = true;
                            return;
                        }
                        // child
                        if (data is Account)
                        {
                            dirty = true;
                            return;
                        }
                    }
                }
            }
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
            // Setting
            Setting.get().removeCallBack(this);
            // Child
            {
                uiData.undoRedoRequestMessage.allRemoveCallBack(this);
                uiData.avatar.allRemoveCallBack(this);
            }
            this.setDataNull(uiData);
            return;
        }
        // Setting
        if (data is Setting)
        {
            return;
        }
        // Child
        {
            // UndoRedoRequestMessage
            {
                if (data is UndoRedoRequestMessage)
                {
                    UndoRedoRequestMessage undoRedoRequestMessage = data as UndoRedoRequestMessage;
                    // Parent
                    {
                        DataUtils.removeParentCallBack(undoRedoRequestMessage, this, ref this.chatMessage);
                    }
                    return;
                }
                // Parent
                {
                    if (data is ChatMessage)
                    {
                        return;
                    }
                    // Human
                    {
                        if (data is Human)
                        {
                            Human human = data as Human;
                            // Child
                            {
                                human.account.allRemoveCallBack(this);
                            }
                            return;
                        }
                        // child
                        if (data is Account)
                        {
                            return;
                        }
                    }
                }
            }
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
                case UIData.Property.undoRedoRequestMessage:
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
        // Setting
        if (wrapProperty.p is Setting)
        {
            switch ((Setting.Property)wrapProperty.n)
            {
                case Setting.Property.language:
                    dirty = true;
                    break;
                case Setting.Property.style:
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
                    Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                    break;
            }
            return;
        }
        // Child
        {
            // UndoRedoRequestMessage
            {
                if (wrapProperty.p is UndoRedoRequestMessage)
                {
                    switch ((UndoRedoRequestMessage.Property)wrapProperty.n)
                    {
                        case UndoRedoRequestMessage.Property.userId:
                            dirty = true;
                            break;
                        case UndoRedoRequestMessage.Property.action:
                            dirty = true;
                            break;
                        case UndoRedoRequestMessage.Property.operation:
                            dirty = true;
                            break;
                        default:
                            Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                            break;
                    }
                    return;
                }
                // Parent
                {
                    if (wrapProperty.p is ChatMessage)
                    {
                        switch ((ChatMessage.Property)wrapProperty.n)
                        {
                            case ChatMessage.Property.state:
                                dirty = true;
                                break;
                            case ChatMessage.Property.time:
                                dirty = true;
                                break;
                            case ChatMessage.Property.content:
                                break;
                            default:
                                Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                                break;
                        }
                        return;
                    }
                    // Human
                    {
                        if (wrapProperty.p is Human)
                        {
                            switch ((Human.Property)wrapProperty.n)
                            {
                                case Human.Property.playerId:
                                    dirty = true;
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
                        // child
                        if (wrapProperty.p is Account)
                        {
                            Account.OnUpdateSyncAccount(wrapProperty, this);
                            return;
                        }
                    }
                }
            }
            if (wrapProperty.p is AccountAvatarUI.UIData)
            {
                return;
            }
        }
        Debug.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
    }

    #endregion

}