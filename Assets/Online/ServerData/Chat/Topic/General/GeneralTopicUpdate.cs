﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeneralTopicUpdate : UpdateBehavior<GeneralTopic>
{

	#region update

	public override void update ()
	{
		if (dirty) {
			dirty = false;
			if (this.data != null) {

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

	public override void onAddCallBack<T> (T data)
	{
		if (data is GeneralTopic) {
			dirty = true;
			return;
		}
		Debug.LogError ("Don't process: " + data + "; " + this);
	}

	public override void onRemoveCallBack<T> (T data, bool isHide)
	{
		if (data is GeneralTopic) {
			GeneralTopic generalTopic = data as GeneralTopic;
			{

			}
			this.setDataNull (generalTopic);
			return;
		}
		Debug.LogError ("Don't process: " + data + "; " + this);
	}

	public override void onUpdateSync<T> (WrapProperty wrapProperty, List<Sync<T>> syncs)
	{
		if (WrapProperty.checkError (wrapProperty)) {
			return;
		}
		if (wrapProperty.p is GeneralTopic) {
			switch ((GeneralTopic.Property)wrapProperty.n) {
			default:
				Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
				break;
			}
		}
		Debug.LogError ("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
	}

	#endregion

}