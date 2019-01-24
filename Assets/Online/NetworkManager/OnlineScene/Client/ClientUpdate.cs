﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClientUpdate : UpdateBehavior<Server>
{

	#region Update

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
		if (data is Server) {
			Server client = data as Server;
			// Update
			{
				UpdateUtils.makeUpdate<ClientStateUpdate, Server> (client, this.transform);
			}
			dirty = true;
			return;
		}
		Debug.LogError ("Don't process: " + data + "; " + this);
	}

	public override void onRemoveCallBack<T> (T data, bool isHide)
	{
		if (data is Server) {
			Server client = data as Server;
			// Update
			{
				client.removeCallBackAndDestroy (typeof(ClientStateUpdate));
			}
			this.setDataNull (client);
			return;
		}
		Debug.LogError ("Don't process: " + data + "; " + this);
	}

	public override void onUpdateSync<T> (WrapProperty wrapProperty, List<Sync<T>> syncs)
	{
		if (WrapProperty.checkError (wrapProperty)) {
			return;
		}
		if (wrapProperty.p is Server) {
			switch ((Server.Property)wrapProperty.n) {
			case Server.Property.serverConfig:
				break;
			case Server.Property.startState:
				break;
			case Server.Property.type:
				break;
			case Server.Property.profile:
				break;
			case Server.Property.state:
				break;
			case Server.Property.users:
				break;
			case Server.Property.globalChat:
				break;
			case Server.Property.friendWorld:
				break;
			case Server.Property.guilds:
				break;
			default:
				Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
				break;
			}
			return;
		}
		Debug.LogError ("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
	}

	#endregion

}