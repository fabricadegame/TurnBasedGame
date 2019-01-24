﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameActionsUI : UIBehavior<GameActionsUI.UIData>
{
	#region UIData

	public class UIData : Data
	{
		public VP<ReferenceData<GameAction>> gameAction;

		#region Sub

		public abstract class Sub : Data
		{
			public abstract GameAction.Type getType();
		}

		public VP<Sub> sub;

		#endregion

		#region Constructor

		public enum Property
		{
			gameAction,
			sub
		}

		public UIData(): base()
		{
			this.gameAction = new VP<ReferenceData<GameAction>>(this, (byte)Property.gameAction, new ReferenceData<GameAction>(null));
			this.sub = new VP<Sub>(this, (byte)Property.sub, null);
		}

		#endregion
	}

	#endregion

	#region Update

	public override void refresh ()
	{
		if (dirty) {
			dirty = false;
			if (this.data != null) {
				GameAction gameAction = this.data.gameAction.v.data;
				if (gameAction != null) {
					// sub
					{
						switch (gameAction.getType ()) {
						case GameAction.Type.ProcessMove:
							{
								ProcessMoveAction processMoveAction = gameAction as ProcessMoveAction;
								// UIData
								ProcessMoveActionUI.UIData processMoveActionUIData = this.data.sub.newOrOld<ProcessMoveActionUI.UIData>();
								{
									processMoveActionUIData.processMoveAction.v = new ReferenceData<ProcessMoveAction> (processMoveAction);
								}
								this.data.sub.v = processMoveActionUIData;
							}
							break;
						case GameAction.Type.StartTurn:
							{
								StartTurnAction startTurnAction = gameAction as StartTurnAction;
								// UIData
								StartTurnActionUI.UIData startTurnActionUIData = this.data.sub.newOrOld<StartTurnActionUI.UIData>();
								{
									startTurnActionUIData.startTurnAction.v = new ReferenceData<StartTurnAction> (startTurnAction);
								}
								this.data.sub.v = startTurnActionUIData;
							}
							break;
						case GameAction.Type.Non:
							{
								NonAction nonAction = gameAction as NonAction;
								// UIData
								NonActionUI.UIData nonActionUIData = this.data.sub.newOrOld<NonActionUI.UIData>();
								{
									nonActionUIData.nonAction.v = new ReferenceData<NonAction> (nonAction);
								}
								this.data.sub.v = nonActionUIData;
							}
							break;
						case GameAction.Type.UndoRedo:
							{
								UndoRedoAction undoRedoAction = gameAction as UndoRedoAction;
								// UIData
								UndoRedoActionUI.UIData undoRedoActionUIData = this.data.sub.newOrOld<UndoRedoActionUI.UIData>();
								{
									undoRedoActionUIData.undoRedoAction.v = new ReferenceData<UndoRedoAction> (undoRedoAction);
								}
								this.data.sub.v = undoRedoActionUIData;
							}
							break;
						case GameAction.Type.WaitInput:
							{
								WaitInputAction waitInputAction = gameAction as WaitInputAction;
								// UIData
								WaitInputActionUI.UIData waitInputActionUIData = this.data.sub.newOrOld<WaitInputActionUI.UIData>();
								{
									waitInputActionUIData.waitInputAction.v = new ReferenceData<WaitInputAction> (waitInputAction);
								}
								this.data.sub.v = waitInputActionUIData;
							}
							break;
						default:
							Debug.LogError ("Unknown gameAction Type: " + gameAction.getType ());
							break;
						}
					}
				} else {
					// Debug.LogError ("gameAction null: " + this);
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

	public ProcessMoveActionUI processMoveActionPrefab;
	public StartTurnActionUI startTurnActionPrefab;
	public NonActionUI nonActionPrefab;
	public UndoRedoActionUI undoRedoActionPrefab;
	public WaitInputActionUI waitInputActionPrefab;

	public override void onAddCallBack<T> (T data)
	{
		if (data is UIData) {
			UIData uiData = data as UIData;
			// Child
			{
				uiData.gameAction.allAddCallBack (this);
				uiData.sub.allAddCallBack (this);
			}
			dirty = true;
			return;
		}
		// Child
		{
			if (data is UIData.Sub) {
				UIData.Sub sub = data as UIData.Sub;
				{
					switch (sub.getType ()) {
					case GameAction.Type.ProcessMove:
						{
							ProcessMoveActionUI.UIData subUIData = sub as ProcessMoveActionUI.UIData;
							UIUtils.Instantiate (subUIData, processMoveActionPrefab, this.transform);
						}
						break;
					case GameAction.Type.StartTurn:
						{
							StartTurnActionUI.UIData subUIData = sub as StartTurnActionUI.UIData;
							UIUtils.Instantiate (subUIData, startTurnActionPrefab, this.transform);
						}
						break;
					case GameAction.Type.Non:
						{
							NonActionUI.UIData subUIData = sub as NonActionUI.UIData;
							UIUtils.Instantiate (subUIData, nonActionPrefab, this.transform);
						}
						break;
					case GameAction.Type.UndoRedo:
						{
							UndoRedoActionUI.UIData subUIData = sub as UndoRedoActionUI.UIData;
							UIUtils.Instantiate (subUIData, undoRedoActionPrefab, this.transform);
						}
						break;
					case GameAction.Type.WaitInput:
						{
							WaitInputActionUI.UIData subUIData = sub as WaitInputActionUI.UIData;
							UIUtils.Instantiate (subUIData, waitInputActionPrefab, this.transform);
						}
						break;
					default:
						Debug.LogError ("Unknown gameAction Type: " + sub.getType ());
						break;
					}
				}
				return;
			}
			if (data is GameAction) {
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
				uiData.gameAction.allRemoveCallBack (this);
				uiData.sub.allRemoveCallBack (this);
			}
			this.setDataNull (uiData);
			return;
		}
		// Child
		{
			if (data is UIData.Sub) {
				UIData.Sub sub = data as UIData.Sub;
				{
					switch (sub.getType ()) {
					case GameAction.Type.ProcessMove:
						{
							ProcessMoveActionUI.UIData subUIData = sub as ProcessMoveActionUI.UIData;
							subUIData.removeCallBackAndDestroy(typeof(ProcessMoveActionUI));
						}
						break;
					case GameAction.Type.StartTurn:
						{
							StartTurnActionUI.UIData subUIData = sub as StartTurnActionUI.UIData;
							subUIData.removeCallBackAndDestroy(typeof(StartTurnActionUI));
						}
						break;
					case GameAction.Type.Non:
						{
							NonActionUI.UIData subUIData = sub as NonActionUI.UIData;
							subUIData.removeCallBackAndDestroy(typeof(NonActionUI));
						}
						break;
					case GameAction.Type.UndoRedo:
						{
							UndoRedoActionUI.UIData subUIData = sub as UndoRedoActionUI.UIData;
							subUIData.removeCallBackAndDestroy(typeof(UndoRedoActionUI));
						}
						break;
					case GameAction.Type.WaitInput:
						{
							WaitInputActionUI.UIData subUIData = sub as WaitInputActionUI.UIData;
							subUIData.removeCallBackAndDestroy (typeof(WaitInputActionUI));
						}
						break;
					default:
						Debug.LogError ("Unknown gameAction Type: " + sub.getType ());
						break;
					}
				}
				return;
			}
			if (data is GameAction) {
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
			case UIData.Property.gameAction:
				{
					ValueChangeUtils.replaceCallBack (this, syncs);
					dirty = true;
				}
				break;
			case UIData.Property.sub:
				{
					ValueChangeUtils.replaceCallBack (this, syncs);
					dirty = true;
				}
				break;
			default:
				Debug.LogError ("unknown wrapProperty: " + wrapProperty + "; " + this);
				break;
			}
			return;
		}
		if (wrapProperty.p is UIData.Sub) {
			return;
		}
		if (wrapProperty.p is GameAction) {
			return;
		}
		Debug.LogError ("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
	}

	#endregion

}