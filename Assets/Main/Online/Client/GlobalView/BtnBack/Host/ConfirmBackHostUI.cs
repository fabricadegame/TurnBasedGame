﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ConfirmBackHostUI : UIBehavior<ConfirmBackHostUI.UIData>
{

    #region UIData

    public class UIData : Data
    {

        #region Constructor

        public enum Property
        {

        }

        public UIData() : base()
        {

        }

        #endregion

        public bool processEvent(Event e)
        {
            bool isProcess = false;
            {
                // back
                if (!isProcess)
                {
                    if (InputEvent.isBackEvent(e))
                    {
                        ConfirmBackHostUI confirmBackHostUI = this.findCallBack<ConfirmBackHostUI>();
                        if (confirmBackHostUI != null)
                        {
                            confirmBackHostUI.onClickBtnCancel();
                        }
                        else
                        {
                            Debug.LogError("confirmBackHostUI null: " + this);
                        }
                        isProcess = true;
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
    private static readonly TxtLanguage txtTitle = new TxtLanguage();

    public Text tvMessage;
    private static readonly TxtLanguage txtMessage = new TxtLanguage();

    public Text tvConfirm;
    private static readonly TxtLanguage txtConfirm = new TxtLanguage();

    public Text tvCancel;
    private static readonly TxtLanguage txtCancel = new TxtLanguage();

    static ConfirmBackHostUI()
    {
        txtTitle.add(Language.Type.vi, "Xác Nhận Thoát");
        txtMessage.add(Language.Type.vi, "Bạn có chắc chắn thoát không?");
        txtConfirm.add(Language.Type.vi, "Xác nhận");
        txtCancel.add(Language.Type.vi, "Huỷ bỏ");
    }

    #endregion

    public override void refresh()
    {
        if (dirty)
        {
            dirty = false;
            if (this.data != null)
            {
                if (lbTitle != null)
                {
                    lbTitle.text = txtTitle.get("Confirm Back");
                }
                else
                {
                    Debug.LogError("lbTitle null");
                }
                if (tvMessage != null)
                {
                    tvMessage.text = txtMessage.get("Are you sure to back?");
                }
                else
                {
                    Debug.LogError("tvMessage null");
                }
                if (tvConfirm != null)
                {
                    tvConfirm.text = txtConfirm.get("Confirm");
                }
                else
                {
                    Debug.LogError("tvConfirm null");
                }
                if (tvCancel != null)
                {
                    tvCancel.text = txtCancel.get("Cancel");
                }
                else
                {
                    Debug.LogError("tvCancel null");
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

    public override void onAddCallBack<T>(T data)
    {
        if (data is UIData)
        {
            // Setting
            {
                Setting.get().addCallBack(this);
            }
            dirty = true;
            return;
        }
        // Child
        if (data is Setting)
        {
            dirty = true;
            return;
        }
        Debug.LogError("Don't process: " + data + "; " + this);
    }

    public override void onRemoveCallBack<T>(T data, bool isHide)
    {
        if (data is UIData)
        {
            UIData uiData = data as UIData;
            // Setting
            {
                Setting.get().removeCallBack(this);
            }
            // Child
            {

            }
            this.setDataNull(uiData);
            return;
        }
        // Child
        if (data is Setting)
        {
            return;
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
        Debug.LogError("Don't process: " + wrapProperty + "; " + this);
    }

    #endregion

    public void onClickBtnConfirm()
    {
        if (this.data != null)
        {
            AfterLoginMainBtnBackHostUI.UIData backHostUIData = this.data.findDataInParent<AfterLoginMainBtnBackHostUI.UIData>();
            if (backHostUIData != null)
            {
                backHostUIData.back();
            }
            else
            {
                Debug.LogError("backHostUIData null: " + this);
            }
        }
        else
        {
            Debug.LogError("data null: " + this);
        }
    }

    public void onClickBtnCancel()
    {
        if (this.data != null)
        {
            AfterLoginMainBtnBackHostUI.UIData backHostUIData = this.data.findDataInParent<AfterLoginMainBtnBackHostUI.UIData>();
            if (backHostUIData != null)
            {
                backHostUIData.confirmUI.v = null;
            }
            else
            {
                Debug.LogError("backHostUIData null: " + this);
            }
        }
        else
        {
            Debug.LogError("data null: " + this);
        }
    }

}