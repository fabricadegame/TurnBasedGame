﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Sudoku
{
	public class SudokuMove : GameMove
	{

		public VP<byte> x;

		public VP<byte> y;

		public VP<byte> value;

		#region Constructor

		public enum Property
		{
			x,
			y,
			value
		}

		public SudokuMove() : base()
		{
			this.x = new VP<byte> (this, (byte)Property.x, 0);
			this.y = new VP<byte> (this, (byte)Property.y, 0);
			this.value = new VP<byte> (this, (byte)Property.value, 0);
		}

		#endregion

		public override Type getType()
		{
			return Type.SudokuMove;
		}

		public override MoveAnimation makeAnimation (GameType gameType)
		{
			return null;
		}

		public override string print()
		{
			return "SudokuMove: " + this.x.v + ", " + this.y.v + ", " + this.value.v;
		}

		public override void getInforBeforeProcess (GameType gameType)
		{

		}

	}
}