﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Shogi
{
    public class HandUI : UIBehavior<HandUI.UIData>
    {

        #region UIData

        public class UIData : Data
        {

            public VP<ReferenceData<Shogi>> shogi;

            public LP<HandPieceUI.UIData> handPieces;

            #region Constructor

            public enum Property
            {
                shogi,
                handPieces
            }

            public UIData() : base()
            {
                this.shogi = new VP<ReferenceData<Shogi>>(this, (byte)Property.shogi, new ReferenceData<Shogi>(null));
                this.handPieces = new LP<HandPieceUI.UIData>(this, (byte)Property.handPieces);
            }

            #endregion

        }

        #endregion

        public override int getStartAllocate()
        {
            return Setting.get().defaultChosenGame.v.getGame() == GameType.Type.SHOGI ? 2 : 0;
        }

        #region Refresh

        private static readonly Common.Color[] colors = new Common.Color[]{
            Common.Color.Black,
            Common.Color.White
        };

        private static readonly Common.HandPiece[] handPieces = new Common.HandPiece[]{
            Common.HandPiece.HPawn,
            Common.HandPiece.HLance,
            Common.HandPiece.HKnight,
            Common.HandPiece.HSilver,
            Common.HandPiece.HGold,
            Common.HandPiece.HBishop,
            Common.HandPiece.HRook
        };

        public override void refresh()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    Shogi shogi = this.data.shogi.v.data;
                    if (shogi != null)
                    {
                        // check load full
                        bool isLoadFull = true;
                        {
                            // shogi
                            if (isLoadFull)
                            {
                                isLoadFull = shogi.isLoadFull();
                            }
                            // animation
                            if (isLoadFull)
                            {
                                isLoadFull = AnimationManager.IsLoadFull(this.data);
                            }
                        }
                        // process
                        if (isLoadFull)
                        {
                            // Find old
                            List<HandPieceUI.UIData> blacks = new List<HandPieceUI.UIData>();
                            List<HandPieceUI.UIData> whites = new List<HandPieceUI.UIData>();
                            {
                                for (int i = 0; i < this.data.handPieces.vs.Count; i++)
                                {
                                    HandPieceUI.UIData handPieceUIData = this.data.handPieces.vs[i];
                                    if (handPieceUIData.color.v == Common.Color.Black)
                                    {
                                        blacks.Add(handPieceUIData);
                                    }
                                    else
                                    {
                                        whites.Add(handPieceUIData);
                                    }
                                }
                            }
                            // Update
                            {
                                // find hand
                                List<uint> hand = shogi.hand.vs;
                                {
                                    MoveAnimation moveAnimation = GameDataBoardUI.UIData.getCurrentMoveAnimation(this.data);
                                    if (moveAnimation != null)
                                    {
                                        switch (moveAnimation.getType())
                                        {
                                            case GameMove.Type.ShogiMove:
                                                {
                                                    ShogiMoveAnimation shogiMoveAnimation = moveAnimation as ShogiMoveAnimation;
                                                    hand = shogiMoveAnimation.hand.vs;
                                                }
                                                break;
                                            default:
                                                Debug.LogError("unknown type: " + moveAnimation.getType() + "; " + this);
                                                break;
                                        }
                                    }
                                }
                                // update
                                for (int colorIndex = 0; colorIndex < colors.Length; colorIndex++)
                                {
                                    Common.Color color = colors[colorIndex];
                                    for (int i = 0; i < handPieces.Length; i++)
                                    {
                                        Common.HandPiece handPiece = handPieces[i];
                                        uint count = Shogi.numOf(Shogi.getHand(hand, color), handPiece);
                                        if (count > 0)
                                        {
                                            // Find HandPieceUI.UIData
                                            HandPieceUI.UIData handPieceUIData = null;
                                            {
                                                // find old
                                                {
                                                    if (color == Common.Color.Black)
                                                    {
                                                        if (blacks.Count > 0)
                                                        {
                                                            handPieceUIData = blacks[0];
                                                            blacks.RemoveAt(0);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (whites.Count > 0)
                                                        {
                                                            handPieceUIData = whites[0];
                                                            whites.RemoveAt(0);
                                                        }
                                                    }
                                                }
                                                // Make new
                                                if (handPieceUIData == null)
                                                {
                                                    handPieceUIData = new HandPieceUI.UIData();
                                                    {
                                                        handPieceUIData.color.v = color;
                                                        handPieceUIData.uid = this.data.handPieces.makeId();
                                                    }
                                                    this.data.handPieces.add(handPieceUIData);
                                                }
                                            }
                                            // Update Property
                                            {
                                                handPieceUIData.handPiece.v = handPiece;
                                                handPieceUIData.color.v = color;
                                                handPieceUIData.count.v = count;
                                            }
                                        }
                                    }
                                }
                            }
                            // Remove old
                            {
                                foreach (HandPieceUI.UIData black in blacks)
                                {
                                    this.data.handPieces.remove(black);
                                }
                                foreach (HandPieceUI.UIData white in whites)
                                {
                                    this.data.handPieces.remove(white);
                                }
                            }
                        }
                        else
                        {
                            Debug.LogError("not load full");
                            dirty = true;
                        }
                    }
                    else
                    {
                        Debug.LogError("shogi null: " + this);
                    }
                    // boardIndexs
                    {
                        if(whiteTransform!=null && blackTransform != null)
                        {
                            // find distance
                            float distance = 5.5f;
                            {
                                switch (Setting.get().boardIndex.v)
                                {
                                    case Setting.BoardIndex.None:
                                        // nhu default
                                        break;
                                    case Setting.BoardIndex.InBoard:
                                        // nhu default
                                        break;
                                    case Setting.BoardIndex.OutBoard:
                                        distance = 6.0f;
                                        break;
                                    default:
                                        Debug.LogError("unknown boardIndex: " + Setting.get().boardIndex.v);
                                        break;
                                }
                            }
                            // process
                            {
                                // black
                                {
                                    UIRectTransform rect = new UIRectTransform();
                                    {
                                        rect.anchoredPosition = new Vector3(0.0f, -distance, 0.0f);
                                        rect.anchorMin = new Vector2(0.5f, 0.5f);
                                        rect.anchorMax = new Vector2(0.5f, 0.5f);
                                        rect.pivot = new Vector2(0.5f, 0.5f);
                                        rect.offsetMin = new Vector2(-4.5f, -distance - 0.7f);
                                        rect.offsetMax = new Vector2(4.5f, -distance + 0.7f);
                                        rect.sizeDelta = new Vector2(9.0f, 1.4f);
                                    }
                                    rect.set(blackTransform);
                                }
                                // white
                                {
                                    UIRectTransform rect = new UIRectTransform();
                                    {
                                        rect.anchoredPosition = new Vector3(0.0f, distance, 0.0f);
                                        rect.anchorMin = new Vector2(0.5f, 0.5f);
                                        rect.anchorMax = new Vector2(0.5f, 0.5f);
                                        rect.pivot = new Vector2(0.5f, 0.5f);
                                        rect.offsetMin = new Vector2(-4.5f, distance + 0.7f);
                                        rect.offsetMax = new Vector2(4.5f, distance - 0.7f);
                                        rect.sizeDelta = new Vector2(9.0f, 1.4f);
                                    }
                                    rect.set(whiteTransform);
                                }
                            }
                        }
                        else
                        {
                            Debug.LogError("whiteTransform, blackTransform null");
                        }
                    }
                }
                else
                {
                    // Debug.LogError ("data null: " + this);
                }
            }
        }

        public override bool isShouldDisableUpdate()
        {
            return true;
        }

        #endregion

        #region implement callBacks

        public HandPieceUI handPiecePrefab;
        public RectTransform blackTransform;
        public RectTransform whiteTransform;

        private AnimationManagerCheckChange<UIData> animationManagerCheckChange = new AnimationManagerCheckChange<UIData>();

        public override void onAddCallBack<T>(T data)
        {
            if (data is UIData)
            {
                UIData uiData = data as UIData;
                // Setting
                Setting.get().addCallBack(this);
                // CheckChange
                {
                    animationManagerCheckChange.needTimeChange = false;
                    animationManagerCheckChange.addCallBack(this);
                    animationManagerCheckChange.setData(uiData);
                }
                // Child
                {
                    uiData.shogi.allAddCallBack(this);
                    uiData.handPieces.allAddCallBack(this);
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
            // checkChange
            if (data is AnimationManagerCheckChange<UIData>)
            {
                dirty = true;
                return;
            }
            // Child
            {
                if (data is Shogi)
                {
                    dirty = true;
                    return;
                }
                if (data is HandPieceUI.UIData)
                {
                    HandPieceUI.UIData handPieceUIData = data as HandPieceUI.UIData;
                    // UI
                    {
                        UIUtils.Instantiate(handPieceUIData, handPiecePrefab, handPieceUIData.color.v == Common.Color.Black ? blackTransform : whiteTransform);
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
                // CheckChange
                {
                    animationManagerCheckChange.removeCallBack(this);
                    animationManagerCheckChange.setData(null);
                }
                // Child
                {
                    uiData.shogi.allRemoveCallBack(this);
                    uiData.handPieces.allRemoveCallBack(this);
                }
                this.setDataNull(uiData);
                return;
            }
            // Setting
            if(data is Setting)
            {
                return;
            }
            // checkChange
            if (data is AnimationManagerCheckChange<UIData>)
            {
                return;
            }
            // Child
            {
                if (data is Shogi)
                {
                    return;
                }
                if (data is HandPieceUI.UIData)
                {
                    HandPieceUI.UIData handPieceUIData = data as HandPieceUI.UIData;
                    // UI
                    {
                        handPieceUIData.removeCallBackAndDestroy(typeof(HandPieceUI));
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
                    case UIData.Property.shogi:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case UIData.Property.handPieces:
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
                        break;
                    case Setting.Property.style:
                        break;
                    case Setting.Property.contentTextSize:
                        break;
                    case Setting.Property.titleTextSize:
                        break;
                    case Setting.Property.labelTextSize:
                        break;
                    case Setting.Property.buttonSize:
                        break;
                    case Setting.Property.itemSize:
                        break;
                    case Setting.Property.confirmQuit:
                        break;
                    case Setting.Property.boardIndex:
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
                    case Setting.Property.defaultChosenGame:
                        break;
                    case Setting.Property.defaultRoomName:
                        break;
                    case Setting.Property.defaultChatRoomStyle:
                        break;
                    default:
                        Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                        break;
                }
                return;
            }
            // Check Change
            if (wrapProperty.p is AnimationManagerCheckChange<UIData>)
            {
                dirty = true;
                return;
            }
            // Child
            {
                if (wrapProperty.p is Shogi)
                {
                    switch ((Shogi.Property)wrapProperty.n)
                    {
                        case Shogi.Property.byTypeBB:
                            break;
                        case Shogi.Property.byColorBB:
                            break;
                        case Shogi.Property.goldsBB:
                            break;
                        case Shogi.Property.piece:
                            break;
                        case Shogi.Property.kingSquare:
                            break;
                        case Shogi.Property.hand:
                            dirty = true;
                            break;
                        case Shogi.Property.turn:
                            break;
                        case Shogi.Property.evalList:
                            break;
                        case Shogi.Property.startState:
                            break;
                        case Shogi.Property.st:
                            break;
                        case Shogi.Property.gamePly:
                            break;
                        case Shogi.Property.nodes:
                            break;
                        default:
                            Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                            break;
                    }
                    return;
                }
                if (wrapProperty.p is HandPieceUI.UIData)
                {
                    return;
                }
            }
            Debug.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
        }

        #endregion

    }
}