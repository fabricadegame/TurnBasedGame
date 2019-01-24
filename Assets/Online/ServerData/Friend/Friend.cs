﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Friend : Data
{

	public VP<Human> user1;
	public VP<Human> user2;

	public string getName(uint yourUserId)
	{
		if (this.user1.v.playerId.v != yourUserId) {
			return this.user1.v.account.v.getName ();
		} else {
			return this.user2.v.account.v.getName ();
		}
	}

	#region State

	public abstract class State : Data
	{
		public enum Type
		{
			Accept,
			Request,
			None,
			Cancel,
			Ban
		}

		public abstract Type getType();

		public static int compare(Type type1, Type type2)
		{
			if (type1 == type2) {
				return 0;
			} else {
				return (int)type2 - (int)type1;
			}
		}

	}

	public VP<State> state;

	#endregion

	public VP<long> time;

	public VP<ChatRoom> chatRoom;

	#region Constructor

	public enum Property
	{
		state,
		user1,
		user2,
		time,
		chatRoom
	}

	public Friend() : base()
	{
		this.state = new VP<State> (this, (byte)Property.state, new FriendStateNone ());
		this.user1 = new VP<Human> (this, (byte)Property.user1, new Human ());
		this.user2 = new VP<Human> (this, (byte)Property.user2, new Human ());
		this.time = new VP<long> (this, (byte)Property.time, Constants.UNKNOWN_TIME);
		{
			ChatRoom chatRoom = new ChatRoom ();
			{
				chatRoom.topic.v = new FriendTopic ();
			}
			this.chatRoom = new VP<ChatRoom> (this, (byte)Property.chatRoom, chatRoom);
		}
	}

	#endregion

	public static List<uint> GetWhoCanAnswer(Data data)
	{
		Friend friend = data.findDataInParent<Friend> ();
		if (friend != null) {
			return friend.getWhoCanAnswer ();
		} else {
			Debug.LogError ("friend null: " + data);
			return new List<uint> ();
		}
	}

	public List<uint> getWhoCanAnswer()
	{
		List<uint> ret = new List<uint> ();
		{
			ret.Add (this.user1.v.playerId.v);
			ret.Add (this.user2.v.playerId.v);
		}
		return ret;
	}

}