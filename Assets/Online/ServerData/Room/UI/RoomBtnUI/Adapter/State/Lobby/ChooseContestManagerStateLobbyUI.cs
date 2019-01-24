﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameManager.Match
{
	public class ChooseContestManagerStateLobbyUI : UIBehavior<ChooseContestManagerStateLobbyUI.UIData>
	{

		#region UIData

		public class UIData : ChooseContestManagerHolder.UIData.StateUI
		{

			public VP<ReferenceData<ContestManagerStateLobby>> contestManagerStateLobby;

			#region Constructor

			public enum Property
			{
				contestManagerStateLobby
			}

			public UIData() : base()
			{
				this.contestManagerStateLobby = new VP<ReferenceData<ContestManagerStateLobby>>(this, (byte)Property.contestManagerStateLobby, new ReferenceData<ContestManagerStateLobby>(null));
			}

			#endregion

			public override ContestManager.State.Type getType ()
			{
				return ContestManager.State.Type.Lobby;
			}

		}

		#endregion

		#region Refresh

		public override void refresh ()
		{
			if (dirty) {
				dirty = false;
				if (this.data != null) {
					ContestManagerStateLobby contestManagerStateLobby = this.data.contestManagerStateLobby.v.data;
					if (contestManagerStateLobby != null) {

					} else {
						Debug.LogError ("contestManagerStateLobby null: " + this);
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

		public override void onAddCallBack<T> (T data)
		{
			if (data is UIData) {
				UIData uiData = data as UIData;
				// Child
				{
					uiData.contestManagerStateLobby.allAddCallBack (this);
				}
				dirty = true;
				return;
			}
			// Child
			if (data is ContestManagerStateLobby) {
				dirty = true;
				return;
			}
			Debug.LogError ("Don't process: " + data + "; " + this);
		}

		public override void onRemoveCallBack<T> (T data, bool isHide)
		{
			if (data is UIData) {
				UIData uiData = data as UIData;
				// Child
				{
					uiData.contestManagerStateLobby.allRemoveCallBack (this);
				}
				this.setDataNull (uiData);
				return;
			}
			// Child
			if (data is ContestManagerStateLobby) {
				return;
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
				case UIData.Property.contestManagerStateLobby:
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
			// Child
			if (wrapProperty.p is ContestManagerStateLobby) {
				return;
			}
			Debug.LogError ("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
		}

		#endregion

	}
}