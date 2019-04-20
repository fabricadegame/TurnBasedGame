﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Seirawan
{
    public class BoardUI : UIBehavior<BoardUI.UIData>
    {

        #region UIData

        public class UIData : Data
        {

            public VP<ReferenceData<Seirawan>> seirawan;

            public VP<BoardIndexsUI.UIData> boardIndexs;

            public VP<HandsUI.UIData> hands;

            public LP<PieceUI.UIData> pieces;

            #region Constructor

            public enum Property
            {
                seirawan,
                boardIndexs,
                hands,
                pieces
            }

            public UIData() : base()
            {
                this.seirawan = new VP<ReferenceData<Seirawan>>(this, (byte)Property.seirawan, new ReferenceData<Seirawan>(null));
                // boardIndexs
                {
                    this.boardIndexs = new VP<BoardIndexsUI.UIData>(this, (byte)Property.boardIndexs, new BoardIndexsUI.UIData());
                    this.boardIndexs.v.gameType.v = GameType.Type.Seirawan;
                }
                this.hands = new VP<HandsUI.UIData>(this, (byte)Property.hands, new HandsUI.UIData());
                this.pieces = new LP<PieceUI.UIData>(this, (byte)Property.pieces);
            }

            #endregion

        }

        #endregion

        #region refresh

        public override void refresh()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    Seirawan seirawan = this.data.seirawan.v.data;
                    if (seirawan != null)
                    {
                        // check load full
                        bool isLoadFull = true;
                        {
                            // chess
                            if (isLoadFull)
                            {
                                if (seirawan.board.vs.Count == 0)
                                {
                                    Debug.LogError("board not load");
                                    isLoadFull = false;
                                }
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
                            // Find data
                            List<int> board = seirawan.board.vs;
                            List<bool> inHand = seirawan.inHand.vs;
                            {
                                MoveAnimation moveAnimation = GameDataBoardUI.UIData.getCurrentMoveAnimation(this.data);
                                if (moveAnimation != null)
                                {
                                    switch (moveAnimation.getType())
                                    {
                                        case GameMove.Type.SeirawanMove:
                                            {
                                                SeirawanMoveAnimation seirawanMoveAnimation = moveAnimation as SeirawanMoveAnimation;
                                                board = seirawanMoveAnimation.board.vs;
                                                inHand = seirawanMoveAnimation.inHand.vs;
                                            }
                                            break;
                                        default:
                                            Debug.LogError("unknown type: " + moveAnimation.getType() + "; " + this);
                                            break;
                                    }
                                }
                                else
                                {
                                    // Debug.LogError ("moveAnimation null: " + this);
                                }
                            }
                            // Normal board
                            bool blindFold = GameData.IsBlindFold(seirawan);
                            {
                                // get olds
                                List<PieceUI.UIData> oldPieceUIs = new List<PieceUI.UIData>();
                                {
                                    oldPieceUIs.AddRange(this.data.pieces.vs);
                                }
                                // Make pieceUI
                                {
                                    for (int index = 0; index < board.Count; index++)
                                    {
                                        int pieceIndex = board[index];
                                        if (pieceIndex >= 0 && pieceIndex <= (int)Common.Piece.PIECE_NB)
                                        {
                                            Common.Piece piece = (Common.Piece)pieceIndex;
                                            if (piece != Common.Piece.NO_PIECE)
                                            {
                                                bool needAdd = false;
                                                // Find pieceUI
                                                PieceUI.UIData pieceUIData = null;
                                                {
                                                    // Find old
                                                    foreach (PieceUI.UIData check in oldPieceUIs)
                                                    {
                                                        if (check.position.v == index)
                                                        {
                                                            pieceUIData = check;
                                                            break;
                                                        }
                                                    }
                                                    // Make new
                                                    if (pieceUIData == null)
                                                    {
                                                        pieceUIData = new PieceUI.UIData();
                                                        {
                                                            pieceUIData.uid = this.data.pieces.makeId();
                                                        }
                                                        needAdd = true;
                                                    }
                                                    else
                                                    {
                                                        oldPieceUIs.Remove(pieceUIData);
                                                    }
                                                }
                                                // Update Property
                                                {
                                                    pieceUIData.piece.v = piece;
                                                    pieceUIData.position.v = index;
                                                    pieceUIData.blindFold.v = blindFold;
                                                }
                                                // add
                                                if (needAdd)
                                                {
                                                    this.data.pieces.add(pieceUIData);
                                                }
                                            }
                                            else
                                            {
                                                // Debug.Log ("pieceIndex wrong: " + piece + "; " + this);
                                            }
                                        }
                                    }
                                }
                                // Remove oldPieceUIs not reuse
                                foreach (PieceUI.UIData oldPieceUI in oldPieceUIs)
                                {
                                    // oldPieceUI.position.v = -1;
                                    this.data.pieces.remove(oldPieceUI);
                                }
                            }
                            // Hands
                            {
                                HandsUI.UIData handsUIData = this.data.hands.v;
                                if (handsUIData != null)
                                {
                                    handsUIData.whiteElephantInHand.v = Seirawan.IsInHand(inHand, Common.Piece.W_ELEPHANT);
                                    handsUIData.whiteHawkInHand.v = Seirawan.IsInHand(inHand, Common.Piece.W_HAWK);
                                    handsUIData.blackElephantInHand.v = Seirawan.IsInHand(inHand, Common.Piece.B_ELEPHANT);
                                    handsUIData.blackHawkInHand.v = Seirawan.IsInHand(inHand, Common.Piece.B_HAWK);
                                }
                                else
                                {
                                    Debug.LogError("handsUIData null: " + this);
                                }
                            }
                        }
                        else
                        {
                            Debug.LogError("not load full");
                            dirty = true;
                        }
                        // siblingIndex
                        {
                            // background 0
                            // boardIndex last
                            UIRectTransform.SetSiblingIndexLast(this.data.boardIndexs.v);
                        }
                    }
                    else
                    {
                        // Debug.LogError ("seirawan null: " + this);
                    }
                }
                else
                {
                    // Debug.LogError ("why data null: " + this);
                }
            }
        }

        public override bool isShouldDisableUpdate()
        {
            return true;
        }

        #endregion

        #region implement callBacks

        public PieceUI piecePrefab;
        public HandsUI handsPrefab;
        private AnimationManagerCheckChange<UIData> animationManagerCheckChange = new AnimationManagerCheckChange<UIData>();

        private GameDataCheckChangeBlindFold<Seirawan> gameDataCheckChangeBlindFold = new GameDataCheckChangeBlindFold<Seirawan>();

        public BoardIndexsUI boardIndexsPrefab;

        public override void onAddCallBack<T>(T data)
        {
            if (data is UIData)
            {
                UIData uiData = data as UIData;
                // CheckChange
                {
                    animationManagerCheckChange.needTimeChange = false;
                    animationManagerCheckChange.addCallBack(this);
                    animationManagerCheckChange.setData(uiData);
                }
                // Update
                {
                    UpdateUtils.makeUpdate<AnimationSetDirtyUpdate, UIData>(uiData, this.transform);
                }
                // Child
                {
                    uiData.seirawan.allAddCallBack(this);
                    uiData.boardIndexs.allAddCallBack(this);
                    uiData.hands.allAddCallBack(this);
                    uiData.pieces.allAddCallBack(this);
                }
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
                // seirawan
                {
                    if (data is Seirawan)
                    {
                        Seirawan seirawan = data as Seirawan;
                        // checkChange
                        {
                            gameDataCheckChangeBlindFold.addCallBack(this);
                            gameDataCheckChangeBlindFold.setData(seirawan);
                        }
                        dirty = true;
                        return;
                    }
                    // checkChange
                    if (data is GameDataCheckChangeBlindFold<Seirawan>)
                    {
                        dirty = true;
                        return;
                    }
                }
                if (data is BoardIndexsUI.UIData)
                {
                    BoardIndexsUI.UIData boardIndexsUIData = data as BoardIndexsUI.UIData;
                    // UI
                    {
                        UIUtils.Instantiate(boardIndexsUIData, boardIndexsPrefab, this.transform);
                    }
                    dirty = true;
                    return;
                }
                if (data is HandsUI.UIData)
                {
                    HandsUI.UIData handsUIData = data as HandsUI.UIData;
                    // UI
                    {
                        UIUtils.Instantiate(handsUIData, handsPrefab, this.transform);
                    }
                    // dirty = true;
                    return;
                }
                if (data is PieceUI.UIData)
                {
                    PieceUI.UIData pieceUIData = data as PieceUI.UIData;
                    // UI
                    {
                        UIUtils.Instantiate(pieceUIData, piecePrefab, this.transform);
                    }
                    // dirty = true;
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
                // CheckChange
                {
                    animationManagerCheckChange.removeCallBack(this);
                    animationManagerCheckChange.setData(null);
                }
                // Update
                {
                    uiData.removeCallBackAndDestroy(typeof(AnimationSetDirtyUpdate));
                }
                // Child
                {
                    uiData.seirawan.allRemoveCallBack(this);
                    uiData.boardIndexs.allRemoveCallBack(this);
                    uiData.hands.allRemoveCallBack(this);
                    uiData.pieces.allRemoveCallBack(this);
                }
                this.setDataNull(uiData);
                return;
            }
            // checkChange
            if (data is AnimationManagerCheckChange<UIData>)
            {
                return;
            }
            // Child
            {
                // seirawan
                {
                    if (data is Seirawan)
                    {
                        // Seirawan seirawan = data as Seirawan;
                        // checkChange
                        {
                            gameDataCheckChangeBlindFold.removeCallBack(this);
                            gameDataCheckChangeBlindFold.setData(null);
                        }
                        return;
                    }
                    // checkChange
                    if (data is GameDataCheckChangeBlindFold<Seirawan>)
                    {
                        return;
                    }
                }
                if (data is BoardIndexsUI.UIData)
                {
                    BoardIndexsUI.UIData boardIndexsUIData = data as BoardIndexsUI.UIData;
                    // UI
                    {
                        boardIndexsUIData.removeCallBackAndDestroy(typeof(BoardIndexsUI));
                    }
                    return;
                }
                if (data is HandsUI.UIData)
                {
                    HandsUI.UIData handsUIData = data as HandsUI.UIData;
                    // UI
                    {
                        handsUIData.removeCallBackAndDestroy(typeof(HandsUI));
                    }
                    return;
                }
                if (data is PieceUI.UIData)
                {
                    PieceUI.UIData pieceUIData = data as PieceUI.UIData;
                    // UI
                    {
                        pieceUIData.removeCallBackAndDestroy(typeof(PieceUI));
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
                    case UIData.Property.seirawan:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case UIData.Property.boardIndexs:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case UIData.Property.hands:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            // dirty = true;
                        }
                        break;
                    case UIData.Property.pieces:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            // dirty = true;
                        }
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
                // seirawan
                {
                    if (wrapProperty.p is Seirawan)
                    {
                        switch ((Seirawan.Property)wrapProperty.n)
                        {
                            case Seirawan.Property.board:
                                dirty = true;
                                break;
                            case Seirawan.Property.byTypeBB:
                                break;
                            case Seirawan.Property.byColorBB:
                                break;
                            case Seirawan.Property.inHand:
                                dirty = true;
                                break;
                            case Seirawan.Property.handScore:
                                break;
                            case Seirawan.Property.pieceCount:
                                break;
                            case Seirawan.Property.pieceList:
                                break;
                            case Seirawan.Property.index:
                                break;
                            case Seirawan.Property.castlingRightsMask:
                                break;
                            case Seirawan.Property.castlingRookSquare:
                                break;
                            case Seirawan.Property.castlingPath:
                                break;
                            case Seirawan.Property.gamePly:
                                break;
                            case Seirawan.Property.sideToMove:
                                break;
                            case Seirawan.Property.st:
                                break;
                            case Seirawan.Property.chess960:
                                break;
                            default:
                                Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                                break;
                        }
                        return;
                    }
                    // checkChange
                    if (wrapProperty.p is GameDataCheckChangeBlindFold<Seirawan>)
                    {
                        dirty = true;
                        return;
                    }
                }
                if (wrapProperty.p is BoardIndexsUI.UIData)
                {
                    return;
                }
                if (wrapProperty.p is HandsUI.UIData)
                {
                    return;
                }
                if (wrapProperty.p is PieceUI.UIData)
                {
                    return;
                }
            }
            Debug.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
        }

        #endregion

    }
}