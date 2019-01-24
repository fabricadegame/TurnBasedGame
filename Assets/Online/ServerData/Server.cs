﻿using UnityEngine;
using Mirror;
using System.Collections;

public class Server : Data
{

    public int maxClientUserCount = LLAPITransport.DefaultMaxConnections;
	
	public VP<ServerConfig> serverConfig;

	public VP<long> instanceId;

	#region StartState

	public enum StartState
	{
		Begin,
		Start,
		Starting,
		Success,
		Fail
	}

	public VP<StartState> startState;

	#endregion

	#region Server Type

	public enum Type
	{
		Server,
		Client,
		Host,
		Offline
	}

	public VP<Type> type;

	#endregion

	public VP<uint> profileId;

	public User getProfileUser()
	{
		for (int i = 0; i < this.users.vs.Count; i++) {
			User user = this.users.vs [i];
			if (user.human.v.playerId.v == this.profileId.v) {
				return user;
			}
		}
		return null;
	}

	public static User GetProfileUser(Data data)
	{
		if (data != null) {
			Server server = data.findDataInParent<Server> ();
			if (server != null) {
				return server.getProfileUser ();
			} else {
				Debug.LogError ("server null");
				return null;
			}
		} else {
			Debug.LogError ("data null");
			return null;
		}
	}

	public static uint getProfileUserId(Data data){
		if (data != null) {
			Server server = data.findDataInParent<Server> ();
			if (server != null) {
				return server.profileId.v;
			} else {
				// Debug.LogError ("server null: " + data);
			}
		} else {
			Debug.LogError ("data null");
		}
		return 0;
	}

	#region State

	public abstract class State : Data
	{

		public enum Type
		{
			Offline,
			Connect, 
			Disconnect
		}

		public abstract Type getType();

		public class Offline : State
		{
			public VP<Login> login;

			#region Constructor

			public enum Property
			{
				login
			}

			public Offline() : base()
			{
				this.login = new VP<Login>(this, (byte)Property.login, new Login());
			}

			#endregion

			public override Type getType ()
			{
				return Type.Offline;
			}

		}

		public class Connect : State
		{
			
			public enum State
			{
				Normal,
				Logout,
				LoggingOut
			}
			public VP<State> state;

			#region Constructor

			public enum Property
			{
				state
			}

			public Connect() : base()
			{
				this.state = new VP<State>(this, (byte)Property.state, State.Normal);
			}

			#endregion

			public override Type getType ()
			{
				return Type.Connect;
			}

		}

		public class Disconnect : State
		{

			public VP<float> time;

			public VP<Login> login;

			#region Constructor

			public enum Property
			{
				time,
				login
			}

			public Disconnect() : base()
			{
				this.time = new VP<float>(this, (byte)Property.time, 0);
				this.login = new VP<Login>(this, (byte)Property.login, new Login());
			}

			#endregion

			public override Type getType ()
			{
				return Type.Disconnect;
			}

		}

		public static void OnUpdateSyncStateChange(WrapProperty wrapProperty, DirtyInterface dirtyInterface)
		{
			switch ((Server.Property)wrapProperty.n) {
			case Property.serverConfig:
				break;
			case Property.startState:
				break;
			case Property.type:
				break;
			case Property.profile:
				break;
			case Property.state:
				dirtyInterface.makeDirty();
				break;
			case Property.users:
				break;
			case Property.disconnectTime:
				break;
			case Property.globalChat:
				break;
			case Property.friendWorld:
				break;
			case Property.guilds:
				break;
			default:
				Debug.LogError ("Don't process: " + wrapProperty + "; " + dirtyInterface);
				break;
			}
		}

	}
		
	public VP<State> state;

	public static bool IsServerOnline(Data data)
	{
		bool ret = false;
		if (data != null) {
			Server server = data.findDataInParent<Server> ();
			if (server != null) {
				Server.State state = server.state.v;
				if (state != null && state.getType () == Server.State.Type.Connect) {
					return true;
				}
			} else {
				// Debug.LogError ("server null: " + data);
				ret = true;
			}
		} else {
			Debug.LogError ("data null: " + data);
		}
		return ret;
	}

	#endregion

	#region User

	public LP<User> users;

	public User findUser(uint userId){
		return this.users.vs.Find (user => user.human.v.playerId.v == userId);
	}

	/** 30 minutes*/
	public const float DefaultDisconnectTime = 30 * 60;

	public VP<float> disconnectTime;

	#endregion

	#region Room

	public VP<RoomManager> roomManager;

	#endregion

	public VP<GlobalChat> globalChat;

	public VP<FriendWorld> friendWorld;
	public VP<GuildWorld> guilds;

	#region Constructor

	public enum Property
	{
		serverConfig,
		instanceId,
		startState,
		type,
		profile,
		state,
		users,
		disconnectTime,
		roomManager,
		globalChat,
		friendWorld,
		guilds
	}

	public Server() : base(){
		this.serverConfig = new VP<ServerConfig> (this, (byte)Property.serverConfig, new ServerConfig ());
		this.instanceId = new VP<long> (this, (byte)Property.instanceId, Global.getRealTimeInMiliSeconds ());
		this.startState = new VP<StartState> (this, (byte)Property.startState, StartState.Start);
		this.type = new VP<Type> (this, (byte)Property.type, Type.Client);
		this.profileId = new VP<uint> (this, (byte)Property.profile, Data.UNKNOWN_ID);
		this.state = new VP<State> (this, (byte)Property.state, new State.Offline());
		this.users = new LP<User> (this, (byte)Property.users);
		this.disconnectTime = new VP<float> (this, (byte)Property.disconnectTime, DefaultDisconnectTime);

		this.roomManager = new VP<RoomManager> (this, (byte)Property.roomManager, new RoomManager ());

		this.globalChat = new VP<GlobalChat> (this, (byte)Property.globalChat, new GlobalChat ());
		this.friendWorld = new VP<FriendWorld> (this, (byte)Property.friendWorld, new FriendWorld ());
		this.guilds = new VP<GuildWorld> (this, (byte)Property.guilds, new GuildWorld ());
	}

	#endregion

	public void init(Server.Type serverType, int port)
	{
		this.type.v = serverType;
		switch (this.type.v) {
		case Server.Type.Server:
			{
				this.serverConfig.v.address.v = "localhost";
				this.serverConfig.v.port.v = Config.serverPort;
				this.startState.v = Server.StartState.Start;
				this.state.v = new Server.State.Connect();
			}
			break;
		case Server.Type.Client:
			{
				Debug.LogError ("Cannot occur: " + this);
				this.state.v = new Server.State.Offline();
				this.startState.v = Server.StartState.Begin;
			}
			break;
		case Server.Type.Host:
			{
				this.state.v = new Server.State.Connect ();
				this.startState.v = Server.StartState.Begin;
				// Address
				{
					this.serverConfig.v.address.v = "localhost";
					this.serverConfig.v.port.v = port;
				}
			}
			break;
		case Server.Type.Offline:
			{
				this.state.v = new Server.State.Connect();
				this.startState.v = Server.StartState.Begin;
			}
			break;
		default:
			Debug.LogError ("unknown server type: " + this.type.v + "; " + this);
			break;
		}
	}

}