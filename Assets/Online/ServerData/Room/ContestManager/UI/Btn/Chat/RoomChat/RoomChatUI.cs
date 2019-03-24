﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using GameManager.Match;

public class RoomChatUI : UIBehavior<RoomChatUI.UIData>
{

    #region UIData

    public class UIData : Data
    {

        public VP<ChatRoomUI.UIData> chatRoom;

        #region style

        public VP<RequestChangeEnumUI.UIData> style;

        public void makeRequestChangeStyle(RequestChangeUpdate<int>.UpdateData update, int newStyle)
        {
            // find btnChat
            ContestManagerBtnChatUI.UIData btnChatUIData = null;
            {
                ContestManagerUI.UIData contestManagerUIData = this.findDataInParent<ContestManagerUI.UIData>();
                if (contestManagerUIData != null)
                {
                    ContestManagerBtnUI.UIData btns = contestManagerUIData.btns.v;
                    if (btns != null)
                    {
                        btnChatUIData = btns.btnChat.v;
                    }
                    else
                    {
                        Debug.LogError("btns null");
                    }
                }
                else
                {
                    Debug.LogError("contestManagerUIData null");
                }
            }
            // process
            if (btnChatUIData != null)
            {
                btnChatUIData.style.v = (ContestManagerBtnChatUI.UIData.Style)newStyle;
                Setting.get().defaultChatRoomStyle.v.setLastStyle((ContestManagerBtnChatUI.UIData.Style)newStyle);
            }
            else
            {
                Debug.LogError("btnChatUIData null");
            }
        }

        #endregion

        #region Constructor

        public enum Property
        {
            chatRoom,
            style
        }

        public UIData() : base()
        {
            // chatRoom
            {
                this.chatRoom = new VP<ChatRoomUI.UIData>(this, (byte)Property.chatRoom, new ChatRoomUI.UIData());
                this.chatRoom.v.needHeader.v = false;
            }
            // style
            {
                this.style = new VP<RequestChangeEnumUI.UIData>(this, (byte)Property.style, new RequestChangeEnumUI.UIData());
                foreach (ContestManagerBtnChatUI.UIData.Style styleType in System.Enum.GetValues(typeof(ContestManagerBtnChatUI.UIData.Style)))
                {
                    this.style.v.options.add(styleType.ToString());
                }
                this.style.v.updateData.v.request.v = makeRequestChangeStyle;
            }
        }

        #endregion

        public bool processEvent(Event e)
        {
            bool isProcess = false;
            {
                // chatRoom
                if (!isProcess)
                {
                    ChatRoomUI.UIData chatRoom = this.chatRoom.v;
                    if (chatRoom != null)
                    {
                        isProcess = chatRoom.processEvent(e);
                    }
                    else
                    {
                        Debug.LogError("chatRoom null");
                    }
                }
                // back
                if (!isProcess)
                {
                    if (InputEvent.isBackEvent(e))
                    {
                        // find isOverlay
                        bool isOverlay = false;
                        {
                            // find btnChat
                            ContestManagerBtnChatUI.UIData btnChatUIData = null;
                            {
                                ContestManagerUI.UIData contestManagerUIData = this.findDataInParent<ContestManagerUI.UIData>();
                                if (contestManagerUIData != null)
                                {
                                    ContestManagerBtnUI.UIData btns = contestManagerUIData.btns.v;
                                    if (btns != null)
                                    {
                                        btnChatUIData = btns.btnChat.v;
                                    }
                                    else
                                    {
                                        Debug.LogError("btns null");
                                    }
                                }
                                else
                                {
                                    Debug.LogError("contestManagerUIData null");
                                }
                            }
                            // process
                            if (btnChatUIData != null)
                            {
                                if(btnChatUIData.style.v== ContestManagerBtnChatUI.UIData.Style.Overlay)
                                {
                                    isOverlay = true;
                                }
                            }
                            else
                            {
                                Debug.LogError("btnChatUIData null");
                            }
                        }
                        // process
                        if (isOverlay)
                        {
                            RoomChatUI roomChatUI = this.findCallBack<RoomChatUI>();
                            if (roomChatUI != null)
                            {
                                roomChatUI.onClickBtnBack();
                                isProcess = true;
                            }
                            else
                            {
                                Debug.LogError("roomChatUI null");
                            }
                        }
                    }
                }
            }
            return isProcess;
        }

    }

    #endregion

    #region txt, rect

    public Text lbTitle;
    private static readonly TxtLanguage txtTitle = new TxtLanguage();

    static RoomChatUI()
    {
        // txt
        {
            txtTitle.add(Language.Type.vi, "Phòng Chat");
        }
        // rect
        {
            // styleRect
            {
                styleRect.anchoredPosition = new Vector3(0.0f, 0f, 0f);
                styleRect.anchorMin = new Vector2(1.0f, 1.0f);
                styleRect.anchorMax = new Vector2(1.0f, 1.0f);
                styleRect.pivot = new Vector2(1.0f, 1.0f);
                styleRect.offsetMin = new Vector2(-90.0f, 0.0f);
                styleRect.offsetMax = new Vector2(0.0f, 0.0f);
                styleRect.sizeDelta = new Vector2(90.0f, 30.0f);
            }
        }
    }

    #endregion

    #region Refresh

    private bool needReset = false;

    public override void refresh()
    {
        if (dirty)
        {
            dirty = false;
            if (this.data != null)
            {
                // chatRoom
                {
                    ChatRoomUI.UIData chatRoomUIData = this.data.chatRoom.v;
                    if (chatRoomUIData != null)
                    {
                        // chatRoom
                        {
                            // find
                            ChatRoom chatRoom = null;
                            {
                                ContestManagerUI.UIData contestManagerUIData = this.data.findDataInParent<ContestManagerUI.UIData>();
                                if (contestManagerUIData != null)
                                {
                                    ContestManager contestManager = contestManagerUIData.contestManager.v.data;
                                    if (contestManager != null)
                                    {
                                        Room room = contestManager.findDataInParent<Room>();
                                        if (room != null)
                                        {
                                            chatRoom = room.chatRoom.v;
                                        }
                                        else
                                        {
                                            Debug.LogError("room null");
                                        }
                                    }
                                    else
                                    {
                                        Debug.LogError("contestManager null");
                                    }
                                }
                                else
                                {
                                    Debug.LogError("contestManagerUIData null");
                                }
                            }
                            // set
                            chatRoomUIData.chatRoom.v = new ReferenceData<ChatRoom>(chatRoom);
                        }
                        // style
                        {
                            // find origin
                            ContestManagerBtnChatUI.UIData.Style orignStyle = ContestManagerBtnChatUI.UIData.Style.Overlay;
                            {
                                ContestManagerUI.UIData contestManagerUIData = this.data.findDataInParent<ContestManagerUI.UIData>();
                                if (contestManagerUIData != null)
                                {
                                    ContestManagerBtnUI.UIData btns = contestManagerUIData.btns.v;
                                    if (btns != null)
                                    {
                                        ContestManagerBtnChatUI.UIData btnChatUIData = btns.btnChat.v;
                                        if (btnChatUIData != null)
                                        {
                                            orignStyle = btnChatUIData.style.v;
                                        }
                                        else
                                        {
                                            Debug.LogError("btnChatUIData null");
                                        }
                                    }
                                    else
                                    {
                                        Debug.LogError("btns null");
                                    }
                                }
                                else
                                {
                                    Debug.LogError("contestManagerUIData null");
                                }
                            }
                            // origin
                            RequestChangeEnumUI.UIData style = this.data.style.v;
                            if (style != null)
                            {
                                // options
                                style.options.copyList(ContestManagerBtnChatUI.UIData.GetStrStyles());
                                // origin
                                RequestChangeUpdate<int>.UpdateData updateData = style.updateData.v;
                                if (updateData != null)
                                {
                                    updateData.canRequestChange.v = true;
                                    updateData.origin.v = (int)orignStyle;
                                    // reset
                                    if (needReset)
                                    {
                                        needReset = false;
                                        updateData.current.v = (int)orignStyle;
                                    }
                                }
                                else
                                {
                                    Debug.LogError("updateData null: " + this);
                                }
                            }
                            else
                            {
                                Debug.LogError("requestEditType null");
                            }
                        }
                    }
                    else
                    {
                        Debug.LogError("chatRoomUIData null");
                    }
                }
                // txt
                {
                    if (lbTitle != null)
                    {
                        lbTitle.text = txtTitle.get("Chat Room");
                    }
                    else
                    {
                        Debug.LogError("lbTitle null");
                    }
                }
            }
            else
            {
                Debug.LogError("data null");
            }
        }
    }

    public override bool isShouldDisableUpdate()
    {
        return true;
    }

    #endregion

    #region implement callBacks

    public ChatRoomUI chatRoomPrefab;
    private static readonly UIRectTransform chatRoomRect = UIRectTransform.CreateFullRect(0, 0, UIConstants.HeaderHeight, 0);

    private ContestManagerUI.UIData contestManagerUIData = null;
    private Room room = null;

    public RequestChangeEnumUI requestEnumPrefab;
    private static readonly UIRectTransform styleRect = new UIRectTransform();

    public override void onAddCallBack<T>(T data)
    {
        if(data is UIData)
        {
            UIData uiData = data as UIData;
            // Setting
            Setting.get().addCallBack(this);
            // Parent
            {
                DataUtils.addParentCallBack(uiData, this, ref this.contestManagerUIData);
            }
            // Child
            {
                uiData.style.allAddCallBack(this);
                uiData.chatRoom.allAddCallBack(this);
            }
            dirty = true;
            return;
        }
        // Setting
        if(data is Setting)
        {
            dirty = true;
            return;
        }
        // Parent
        {
            if(data is ContestManagerUI.UIData)
            {
                ContestManagerUI.UIData contestManagerUIData = data as ContestManagerUI.UIData;
                // Child
                {
                    contestManagerUIData.contestManager.allAddCallBack(this);
                    contestManagerUIData.btns.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            {
                // contestManager
                {
                    if (data is ContestManager)
                    {
                        ContestManager contestManager = data as ContestManager;
                        // Parent
                        {
                            DataUtils.addParentCallBack(contestManager, this, ref this.room);
                        }
                        dirty = true;
                        return;
                    }
                    // Parent
                    if (data is Room)
                    {
                        dirty = true;
                        return;
                    }
                }
                // btns
                {
                    if(data is ContestManagerBtnUI.UIData)
                    {
                        ContestManagerBtnUI.UIData btnUIData = data as ContestManagerBtnUI.UIData;
                        // Child
                        {
                            btnUIData.btnChat.allAddCallBack(this);
                        }
                        dirty = true;
                        return;
                    }
                    // Child
                    if(data is ContestManagerBtnChatUI.UIData)
                    {
                        needReset = true;
                        dirty = true;
                        return;
                    }
                }
            }
        }
        // Child
        {
            if (data is ChatRoomUI.UIData)
            {
                ChatRoomUI.UIData chatRoomUIData = data as ChatRoomUI.UIData;
                // UI
                {
                    UIUtils.Instantiate(chatRoomUIData, chatRoomPrefab, this.transform, chatRoomRect);
                }
                dirty = true;
                return;
            }
            if (data is RequestChangeEnumUI.UIData)
            {
                RequestChangeEnumUI.UIData requestChange = data as RequestChangeEnumUI.UIData;
                // UI
                {
                    WrapProperty wrapProperty = requestChange.p;
                    if (wrapProperty != null)
                    {
                        switch ((UIData.Property)wrapProperty.n)
                        {
                            case UIData.Property.style:
                                UIUtils.Instantiate(requestChange, requestEnumPrefab, this.transform, styleRect);
                                break;
                            default:
                                Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                                break;
                        }
                    }
                    else
                    {
                        Debug.LogError("wrapProperty null: " + this);
                    }
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
            // Parent
            {
                DataUtils.removeParentCallBack(uiData, this, ref this.contestManagerUIData);
            }
            // Child
            {
                uiData.chatRoom.allRemoveCallBack(this);
                uiData.style.allRemoveCallBack(this);
            }
            this.setDataNull(uiData);
            return;
        }
        // Setting
        if(data is Setting)
        {
            return;
        }
        // Parent
        {
            if (data is ContestManagerUI.UIData)
            {
                ContestManagerUI.UIData contestManagerUIData = data as ContestManagerUI.UIData;
                // Child
                {
                    contestManagerUIData.contestManager.allRemoveCallBack(this);
                    contestManagerUIData.btns.allRemoveCallBack(this);
                }
                return;
            }
            // Child
            {
                // contestManager
                {
                    if (data is ContestManager)
                    {
                        ContestManager contestManager = data as ContestManager;
                        // Parent
                        {
                            DataUtils.removeParentCallBack(contestManager, this, ref this.room);
                        }
                        return;
                    }
                    // Parent
                    if (data is Room)
                    {
                        return;
                    }
                }
                // btns
                {
                    if (data is ContestManagerBtnUI.UIData)
                    {
                        ContestManagerBtnUI.UIData btnUIData = data as ContestManagerBtnUI.UIData;
                        // Child
                        {
                            btnUIData.btnChat.allRemoveCallBack(this);
                        }
                        return;
                    }
                    // Child
                    if (data is ContestManagerBtnChatUI.UIData)
                    {
                        return;
                    }
                }
            }
        }
        // Child
        {
            if (data is ChatRoomUI.UIData)
            {
                ChatRoomUI.UIData chatRoomUIData = data as ChatRoomUI.UIData;
                // UI
                {
                    chatRoomUIData.removeCallBackAndDestroy(typeof(ChatRoomUI));
                }
                return;
            }
            if (data is RequestChangeEnumUI.UIData)
            {
                RequestChangeEnumUI.UIData requestChange = data as RequestChangeEnumUI.UIData;
                // UI
                {
                    requestChange.removeCallBackAndDestroy(typeof(RequestChangeEnumUI));
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
                case UIData.Property.chatRoom:
                    {
                        ValueChangeUtils.replaceCallBack(this, syncs);
                        dirty = true;
                    }
                    break;
                case UIData.Property.style:
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
        if(wrapProperty.p is Setting)
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
                case Setting.Property.defaultChosenGame:
                    break;
                default:
                    Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                    break;
            }
            return;
        }
        // Parent
        {
            if (wrapProperty.p is ContestManagerUI.UIData)
            {
                switch ((ContestManagerUI.UIData.Property)wrapProperty.n)
                {
                    case ContestManagerUI.UIData.Property.contestManager:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case ContestManagerUI.UIData.Property.sub:
                        break;
                    case ContestManagerUI.UIData.Property.btns:
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
                // contestManager
                {
                    if (wrapProperty.p is ContestManager)
                    {
                        return;
                    }
                    // Parent
                    if (wrapProperty.p is Room)
                    {
                        switch ((Room.Property)wrapProperty.n)
                        {
                            case Room.Property.roomInform:
                                break;
                            case Room.Property.changeRights:
                                break;
                            case Room.Property.name:
                                break;
                            case Room.Property.password:
                                break;
                            case Room.Property.users:
                                break;
                            case Room.Property.state:
                                break;
                            case Room.Property.requestNewContestManager:
                                break;
                            case Room.Property.contestManagers:
                                break;
                            case Room.Property.timeCreated:
                                break;
                            case Room.Property.chatRoom:
                                dirty = true;
                                break;
                            case Room.Property.allowHint:
                                break;
                            case Room.Property.allowLoadHistory:
                                break;
                            default:
                                Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                                break;
                        }
                        return;
                    }
                }
                // btns
                {
                    if (wrapProperty.p is ContestManagerBtnUI.UIData)
                    {
                        switch ((ContestManagerBtnUI.UIData.Property)wrapProperty.n)
                        {
                            case ContestManagerBtnUI.UIData.Property.btnChat:
                                {
                                    ValueChangeUtils.replaceCallBack(this, syncs);
                                    dirty = true;
                                }
                                break;
                            case ContestManagerBtnUI.UIData.Property.btnRoomUser:
                                break;
                            case ContestManagerBtnUI.UIData.Property.btnSetting:
                                break;
                            default:
                                Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                                break;
                        }
                        return;
                    }
                    // Child
                    if (wrapProperty.p is ContestManagerBtnChatUI.UIData)
                    {
                        switch ((ContestManagerBtnChatUI.UIData.Property)wrapProperty.n)
                        {
                            case ContestManagerBtnChatUI.UIData.Property.visibility:
                                dirty = true;
                                break;
                            case ContestManagerBtnChatUI.UIData.Property.style:
                                dirty = true;
                                break;
                            default:
                                Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                                break;
                        }
                        return;
                    }
                }
            }
        }
        // Child
        {
            if (wrapProperty.p is ChatRoomUI.UIData)
            {
                return;
            }
            if (wrapProperty.p is RequestChangeEnumUI.UIData)
            {
                return;
            }
        }
        Debug.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
    }

    #endregion

    public void onClickBtnBack()
    {
        if (this.data != null)
        {
            ContestManagerUI.UIData contestManagerUIData = this.data.findDataInParent<ContestManagerUI.UIData>();
            if (contestManagerUIData != null)
            {
                ContestManagerBtnUI.UIData btns = contestManagerUIData.btns.v;
                if (btns != null)
                {
                    ContestManagerBtnChatUI.UIData btnChatUIData = btns.btnChat.v;
                    if (btnChatUIData != null)
                    {
                        btnChatUIData.visibility.v = ContestManagerBtnChatUI.UIData.Visibility.Hide;
                        Setting.get().defaultChatRoomStyle.v.setLastVisibility(ContestManagerBtnChatUI.UIData.Visibility.Hide);
                    }
                    else
                    {
                        Debug.LogError("btnChatUIData null");
                    }
                }
                else
                {
                    Debug.LogError("btns null");
                }
            }
            else
            {
                Debug.LogError("contestManagerUIData null");
            }
        }
        else
        {
            Debug.LogError("data null");
        }
    }

}