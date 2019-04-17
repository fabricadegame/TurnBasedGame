﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace FairyChess
{
    public class PieceUI : UIBehavior<PieceUI.UIData>
    {

        #region UIData

        public class UIData : Data
        {

            #region Property

            public VP<Common.VariantType> variantType;

            public VP<Common.Piece> piece;

            public VP<int> position;

            public VP<bool> blindFold;

            #endregion

            #region Constructor

            public enum Property
            {
                variantType,
                piece,
                position,
                blindFold
            }

            public UIData() : base()
            {
                this.variantType = new VP<Common.VariantType>(this, (byte)Property.variantType, Common.VariantType.asean);
                this.piece = new VP<Common.Piece>(this, (byte)Property.piece, Common.Piece.NO_PIECE);
                this.position = new VP<int>(this, (byte)Property.position, 0);
                this.blindFold = new VP<bool>(this, (byte)Property.blindFold, false);
            }

            #endregion

        }

        #endregion

        #region Refresh

        public Image image;

        public static Vector2 ConvertPositionToLocalPosition(int position)
        {
            float x = position % 8;
            float y = position / 8;
            return new Vector2(x + 0.5f - 8 / 2f, y + 0.5f - 8 / 2f);
        }

        private readonly Color EnPassantColor = new Color(256 / 256f, 256 / 256f, 256 / 256f, 72 / 256f);

        public override void refresh()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    if (this.data.position.v >= 0 && this.data.position.v < 64)
                    {
                        // check load full
                        bool isLoadFull = true;
                        {
                            // animation
                            if (isLoadFull)
                            {
                                isLoadFull = AnimationManager.IsLoadFull(this.data);
                            }
                        }
                        // process
                        if (isLoadFull)
                        {
                            // Find MoveAnimation
                            MoveAnimation moveAnimation = null;
                            float time = 0;
                            float duration = 0;
                            {
                                GameDataBoardUI.UIData.getCurrentMoveAnimationInfo(this.data, out moveAnimation, out time, out duration);
                            }
                            // imagePieceUIData
                            {
                                if (image != null)
                                {
                                    // sprite
                                    if(!this.data.blindFold.v)
                                    {
                                        image.enabled = true;
                                        Common.Piece piece = this.data.piece.v;
                                        {
                                            if (moveAnimation != null)
                                            {
                                                switch (moveAnimation.getType())
                                                {
                                                    case GameMove.Type.FairyChessMove:
                                                        {
                                                            FairyChessMoveAnimation fairyChessMoveAnimation = moveAnimation as FairyChessMoveAnimation;
                                                            FairyChessMove.Move move = new FairyChessMove.Move(fairyChessMoveAnimation.move.v);
                                                            switch (move.type)
                                                            {
                                                                case Common.MoveType.PIECE_PROMOTION:
                                                                case Common.MoveType.PROMOTION:
                                                                    {
                                                                        if ((int)move.ori == this.data.position.v)
                                                                        {
                                                                            float distanceDuration = MoveAnimation.GetDistanceMoveDuration(Common.GetDistance(move.ori, move.dest));
                                                                            if (time <= distanceDuration)
                                                                            {
                                                                                piece = this.data.piece.v;
                                                                            }
                                                                            else
                                                                            {
                                                                                bool showPromotion = true;
                                                                                {
                                                                                    float promotionTime = time - distanceDuration;
                                                                                    int flipFlop = Mathf.CeilToInt(promotionTime / (FairyChessMoveAnimation.PromotionDuration * AnimationManager.DefaultDuration / 4));
                                                                                    showPromotion = (flipFlop % 2 == 0);
                                                                                }
                                                                                piece = showPromotion ? fairyChessMoveAnimation.promoteOrDropPiece.v : this.data.piece.v;
                                                                            }
                                                                        }
                                                                    }
                                                                    break;
                                                                case Common.MoveType.NORMAL:
                                                                    break;
                                                                case Common.MoveType.ENPASSANT:
                                                                    break;
                                                                case Common.MoveType.CASTLING:
                                                                    break;
                                                                case Common.MoveType.DROP:
                                                                    // Debug.LogError ("pieceUI: dropPiece: " + this.data.piece.v);
                                                                    break;
                                                                default:
                                                                    Debug.LogError("unknown moveType: " + move.GetType() + "; " + this);
                                                                    break;
                                                            }
                                                        }
                                                        break;
                                                    default:
                                                        // Debug.LogError ("unknown moveAnimationType: " + moveAnimation.getType () + "; " + this);
                                                        break;
                                                }
                                            }
                                        }
                                        SpriteContainer.setImagePiece(image, this.data.variantType.v,
                                            Common.color_of(piece), Common.type_of(piece));
                                    }
                                    else
                                    {
                                        image.enabled = false;
                                    }
                                    // alpha
                                    {
                                        Color color = Color.white;
                                        {
                                            if (moveAnimation != null)
                                            {
                                                switch (moveAnimation.getType())
                                                {
                                                    case GameMove.Type.FairyChessMove:
                                                        {
                                                            FairyChessMoveAnimation fairyChessMoveAnimation = moveAnimation as FairyChessMoveAnimation;
                                                            FairyChessMove.Move move = new FairyChessMove.Move(fairyChessMoveAnimation.move.v);
                                                            switch (move.type)
                                                            {
                                                                case Common.MoveType.ENPASSANT:
                                                                    {
                                                                        bool isEnPassant = false;
                                                                        {
                                                                            if (Common.type_of(this.data.piece.v) == Common.PieceType.PAWN)
                                                                            {
                                                                                Common.File destFile = Common.file_of(move.dest);
                                                                                Common.Rank destRank = Common.rank_of(move.dest);
                                                                                // find captureSquare
                                                                                Common.Square captureSquare = Common.Square.SQUARE_NB;
                                                                                if (destRank == Common.Rank.RANK_3)
                                                                                {
                                                                                    captureSquare = Common.make_square(destFile, Common.Rank.RANK_4);
                                                                                }
                                                                                else if (destRank == Common.Rank.RANK_6)
                                                                                {
                                                                                    captureSquare = Common.make_square(destFile, Common.Rank.RANK_5);
                                                                                }
                                                                                // Process
                                                                                if (this.data.position.v == (int)captureSquare)
                                                                                {
                                                                                    isEnPassant = true;
                                                                                }
                                                                            }
                                                                        }
                                                                        color = isEnPassant ? EnPassantColor : Color.white;
                                                                    }
                                                                    break;
                                                                default:
                                                                    break;
                                                            }
                                                        }
                                                        break;
                                                    default:
                                                        break;
                                                }
                                            }
                                        }
                                        image.color = color;
                                    }
                                }
                                else
                                {
                                    Debug.LogError("image null: " + this);
                                }
                            }
                            // Position
                            {
                                Vector2 localPosition = ConvertPositionToLocalPosition(this.data.position.v);
                                {
                                    if (moveAnimation != null)
                                    {
                                        switch (moveAnimation.getType())
                                        {
                                            case GameMove.Type.FairyChessMove:
                                                {
                                                    FairyChessMoveAnimation fairyChessMoveAnimation = moveAnimation as FairyChessMoveAnimation;
                                                    FairyChessMove.Move move = new FairyChessMove.Move(fairyChessMoveAnimation.move.v);
                                                    switch (move.type)
                                                    {
                                                        case Common.MoveType.NORMAL:
                                                            {
                                                                if ((int)move.ori == this.data.position.v)
                                                                {
                                                                    this.transform.SetAsLastSibling();
                                                                    Vector2 from = ConvertPositionToLocalPosition((int)move.ori);
                                                                    Vector2 dest = ConvertPositionToLocalPosition((int)move.dest);
                                                                    // set 
                                                                    if (duration > 0)
                                                                    {
                                                                        localPosition = Vector2.Lerp(from, dest, MoveAnimation.getAccelerateDecelerateInterpolation(time / duration));
                                                                    }
                                                                    else
                                                                    {
                                                                        Debug.LogError("why duration < 0: " + duration + "; " + this);
                                                                    }
                                                                }
                                                            }
                                                            break;
                                                        case Common.MoveType.PROMOTION:
                                                        case Common.MoveType.PIECE_PROMOTION:
                                                            {
                                                                if ((int)move.ori == this.data.position.v)
                                                                {
                                                                    this.transform.SetAsLastSibling();
                                                                    Vector2 from = ConvertPositionToLocalPosition((int)move.ori);
                                                                    Vector2 dest = ConvertPositionToLocalPosition((int)move.dest);
                                                                    // set 
                                                                    if (duration > 0)
                                                                    {
                                                                        localPosition = Vector2.Lerp(from, dest, MoveAnimation.getAccelerateDecelerateInterpolation(time / (duration - FairyChessMoveAnimation.PromotionDuration * AnimationManager.DefaultDuration)));
                                                                    }
                                                                    else
                                                                    {
                                                                        Debug.LogError("why duration < 0: " + duration + "; " + this);
                                                                    }
                                                                }
                                                            }
                                                            break;
                                                        case Common.MoveType.ENPASSANT:
                                                            {
                                                                if ((int)move.ori == this.data.position.v)
                                                                {
                                                                    this.transform.SetAsLastSibling();
                                                                    Vector2 from = ConvertPositionToLocalPosition((int)move.ori);
                                                                    Vector2 dest = ConvertPositionToLocalPosition((int)move.dest);
                                                                    // set 
                                                                    if (duration > 0)
                                                                    {
                                                                        localPosition = Vector2.Lerp(from, dest, MoveAnimation.getAccelerateDecelerateInterpolation(time / duration));
                                                                    }
                                                                    else
                                                                    {
                                                                        Debug.LogError("why duration < 0: " + duration + "; " + this);
                                                                    }
                                                                }
                                                            }
                                                            break;
                                                        case Common.MoveType.CASTLING:
                                                            {
                                                                if (Common.rank_of(move.ori) == Common.Rank.RANK_1)
                                                                {
                                                                    if (Common.color_of(this.data.piece.v) == Common.Color.WHITE)
                                                                    {
                                                                        if (Common.type_of(this.data.piece.v) == Common.PieceType.KING)
                                                                        {
                                                                            Vector2 from = ConvertPositionToLocalPosition(this.data.position.v);
                                                                            // dest
                                                                            Vector2 dest = ConvertPositionToLocalPosition((int)move.dest);
                                                                            {
                                                                                Common.File originFile = Common.file_of(move.ori);
                                                                                Common.File destFile = Common.file_of(move.dest);
                                                                                if (destFile >= originFile)
                                                                                {
                                                                                    dest = ConvertPositionToLocalPosition((int)Common.make_square(Common.File.FILE_G, Common.Rank.RANK_1));
                                                                                }
                                                                                else
                                                                                {
                                                                                    dest = ConvertPositionToLocalPosition((int)Common.make_square(Common.File.FILE_C, Common.Rank.RANK_1));
                                                                                }
                                                                            }
                                                                            // set 
                                                                            if (duration > 0)
                                                                            {
                                                                                localPosition = Vector2.Lerp(from, dest, MoveAnimation.getAccelerateDecelerateInterpolation(time / duration));
                                                                            }
                                                                            else
                                                                            {
                                                                                Debug.LogError("why duration < 0: " + duration + "; " + this);
                                                                            }
                                                                        }
                                                                        else if (Common.type_of(this.data.piece.v) == Common.PieceType.ROOK)
                                                                        {
                                                                            // isCastiling or not and find dest
                                                                            bool isCastlingPiece = false;
                                                                            Common.File yourDestFile = Common.File.FILE_C;
                                                                            {
                                                                                Common.File yourFile = Common.file_of((Common.Square)this.data.position.v);
                                                                                Common.File originFile = Common.file_of(move.ori);
                                                                                Common.File destFile = Common.file_of(move.dest);
                                                                                // Debug.LogError ("originFile: " + originFile + "; destFile: " + destFile);
                                                                                if (destFile >= yourFile && destFile <= originFile)
                                                                                {
                                                                                    isCastlingPiece = true;
                                                                                    yourDestFile = Common.File.FILE_D;
                                                                                }
                                                                                else if (destFile >= originFile && destFile <= yourFile)
                                                                                {
                                                                                    isCastlingPiece = true;
                                                                                    yourDestFile = Common.File.FILE_F;
                                                                                }
                                                                            }
                                                                            // Process
                                                                            if (isCastlingPiece)
                                                                            {
                                                                                this.transform.SetAsLastSibling();
                                                                                Vector2 from = ConvertPositionToLocalPosition(this.data.position.v);
                                                                                Vector2 dest = ConvertPositionToLocalPosition((int)Common.make_square(yourDestFile, Common.Rank.RANK_1));
                                                                                // set 
                                                                                if (duration > 0)
                                                                                {
                                                                                    localPosition = Vector2.Lerp(from, dest, MoveAnimation.getAccelerateDecelerateInterpolation(time / duration));
                                                                                }
                                                                                else
                                                                                {
                                                                                    Debug.LogError("why duration < 0: " + duration + "; " + this);
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                else if (Common.rank_of(move.ori) == Common.Rank.RANK_8)
                                                                {
                                                                    if (Common.color_of(this.data.piece.v) == Common.Color.BLACK)
                                                                    {
                                                                        if (Common.type_of(this.data.piece.v) == Common.PieceType.KING)
                                                                        {
                                                                            Vector2 from = ConvertPositionToLocalPosition((int)move.ori);
                                                                            // dest
                                                                            Vector2 dest = ConvertPositionToLocalPosition((int)move.dest);
                                                                            {
                                                                                Common.File originFile = Common.file_of(move.ori);
                                                                                Common.File destFile = Common.file_of(move.dest);
                                                                                if (destFile >= originFile)
                                                                                {
                                                                                    dest = ConvertPositionToLocalPosition((int)Common.make_square(Common.File.FILE_G, Common.Rank.RANK_8));
                                                                                }
                                                                                else
                                                                                {
                                                                                    dest = ConvertPositionToLocalPosition((int)Common.make_square(Common.File.FILE_C, Common.Rank.RANK_8));
                                                                                }
                                                                            }
                                                                            // set 
                                                                            if (duration > 0)
                                                                            {
                                                                                localPosition = Vector2.Lerp(from, dest, MoveAnimation.getAccelerateDecelerateInterpolation(time / duration));
                                                                            }
                                                                            else
                                                                            {
                                                                                Debug.LogError("why duration < 0: " + duration + "; " + this);
                                                                            }
                                                                        }
                                                                        else if (Common.type_of(this.data.piece.v) == Common.PieceType.ROOK)
                                                                        {
                                                                            // isCastiling or not and find dest
                                                                            bool isCastlingPiece = false;
                                                                            Common.File yourDestFile = Common.File.FILE_C;
                                                                            {
                                                                                Common.File yourFile = Common.file_of((Common.Square)this.data.position.v);
                                                                                Common.File originFile = Common.file_of(move.ori);
                                                                                Common.File destFile = Common.file_of(move.dest);
                                                                                if (destFile >= yourFile && destFile <= originFile)
                                                                                {
                                                                                    isCastlingPiece = true;
                                                                                    yourDestFile = Common.File.FILE_D;
                                                                                }
                                                                                else if (destFile >= originFile && destFile <= yourFile)
                                                                                {
                                                                                    isCastlingPiece = true;
                                                                                    yourDestFile = Common.File.FILE_F;
                                                                                }
                                                                            }
                                                                            // Process
                                                                            if (isCastlingPiece)
                                                                            {
                                                                                this.transform.SetAsLastSibling();
                                                                                Vector2 from = ConvertPositionToLocalPosition(this.data.position.v);
                                                                                Vector2 dest = ConvertPositionToLocalPosition((int)Common.make_square(yourDestFile, Common.Rank.RANK_8));
                                                                                // set 
                                                                                if (duration > 0)
                                                                                {
                                                                                    localPosition = Vector2.Lerp(from, dest, MoveAnimation.getAccelerateDecelerateInterpolation(time / duration));
                                                                                }
                                                                                else
                                                                                {
                                                                                    Debug.LogError("why duration < 0: " + duration + "; " + this);
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            break;
                                                        case Common.MoveType.DROP:
                                                            break;
                                                        default:
                                                            Debug.LogError("unknown move type: " + move.type + "; " + this);
                                                            break;
                                                    }
                                                }
                                                break;
                                            default:
                                                Debug.LogError("unknown moveAnimation: " + moveAnimation + "; " + this);
                                                break;
                                        }
                                    }
                                }
                                this.transform.localPosition = localPosition;
                            }
                            // Scale
                            {
                                int playerView = GameDataBoardUI.UIData.getPlayerView(this.data);
                                // get localScale
                                float localScale = 1;
                                {
                                    if (moveAnimation != null)
                                    {
                                        switch (moveAnimation.getType())
                                        {
                                            case GameMove.Type.FairyChessMove:
                                                {
                                                    FairyChessMoveAnimation fairyChessMoveAnimation = moveAnimation as FairyChessMoveAnimation;
                                                    // Get Inform
                                                    FairyChessMove.Move move = new FairyChessMove.Move(fairyChessMoveAnimation.move.v);
                                                    switch (move.type)
                                                    {
                                                        case Common.MoveType.NORMAL:
                                                            break;
                                                        case Common.MoveType.ENPASSANT:
                                                            break;
                                                        case Common.MoveType.CASTLING:
                                                            break;
                                                        case Common.MoveType.PROMOTION:
                                                            break;
                                                        case Common.MoveType.PIECE_PROMOTION:
                                                            break;
                                                        case Common.MoveType.DROP:
                                                            {
                                                                if (this.data.position.v == (int)move.dest)
                                                                {
                                                                    if (duration > 0)
                                                                    {
                                                                        localScale = MoveAnimation.getAccelerateDecelerateInterpolation(time / duration);
                                                                        // Debug.LogError ("localScale: " + localScale);
                                                                    }
                                                                }
                                                            }
                                                            break;
                                                        default:
                                                            Debug.LogError("unknown moveType: " + move.type + "; " + this);
                                                            break;
                                                    }
                                                }
                                                break;
                                            default:
                                                Debug.LogError("unknown moveAnimation: " + moveAnimation + "; " + this);
                                                break;
                                        }
                                    }
                                }
                                this.transform.localScale = (playerView == 0 ? new Vector3(localScale, localScale, 1f) : new Vector3(localScale, -localScale, 1f));
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
                        Debug.LogError("outside board: " + this);
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

        private GameDataBoardCheckPerspectiveChange<UIData> perspectiveChange = new GameDataBoardCheckPerspectiveChange<UIData>();

        public override void onAddCallBack<T>(T data)
        {
            if (data is UIData)
            {
                UIData uiData = data as UIData;
                // CheckChange
                {
                    perspectiveChange.addCallBack(this);
                    perspectiveChange.setData(uiData);
                }
                dirty = true;
                return;
            }
            // CheckChange
            if (data is GameDataBoardCheckPerspectiveChange<UIData>)
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
                // CheckChange
                {
                    perspectiveChange.removeCallBack(this);
                    perspectiveChange.setData(null);
                }
                this.setDataNull(uiData);
                return;
            }
            // CheckChange
            if (data is GameDataBoardCheckPerspectiveChange<UIData>)
            {
                dirty = true;
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
                    case UIData.Property.variantType:
                        dirty = true;
                        break;
                    case UIData.Property.piece:
                        dirty = true;
                        break;
                    case UIData.Property.position:
                        dirty = true;
                        break;
                    case UIData.Property.blindFold:
                        dirty = true;
                        break;
                    default:
                        Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                        break;
                }
                return;
            }
            // CheckChange
            if (wrapProperty.p is GameDataBoardCheckPerspectiveChange<UIData>)
            {
                dirty = true;
                return;
            }
            Debug.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
        }

        #endregion

    }
}