﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Janggi.NoneRule
{
	public class JanggiCustomMove : GameMove
	{

		public VP<int> fromX;

		public VP<int> fromY;

		public VP<int> destX;

		public VP<int> destY;

		#region Constructor

		public enum Property
		{
			fromX,
			fromY,
			destX,
			destY
		}

		public JanggiCustomMove() : base()
		{
			this.fromX = new VP<int> (this, (byte)Property.fromX, 0);
			this.fromY = new VP<int> (this, (byte)Property.fromY, 0);
			this.destX = new VP<int> (this, (byte)Property.destX, 0);
			this.destY = new VP<int> (this, (byte)Property.destY, 0);
		}

		#endregion

		public override Type getType()
		{
			return Type.JanggiCustomMove;
		}

		public override MoveAnimation makeAnimation (GameType gameType)
		{
			return null;
		}

		public override string print()
		{
			return string.Format ("JanggiCustomMove: {0}, {1}, {2}, {3}", this.fromX.v, this.fromY.v, this.destX.v, this.destY.v);
		}

		public override void getInforBeforeProcess (GameType gameType)
		{

		}

	}
}