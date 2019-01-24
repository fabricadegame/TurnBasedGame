﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using MineSweeper.NoneRule;

namespace MineSweeper
{
	public class MineSweeper : GameType
	{

		public const int MAX_DIMENSION_SIZE = 100;
		public const int MIN_DIMENSION_SIZE = 5;

		public VP<int> Y;

		public VP<int> X;

		public VP<int> K;

		#region bombs

		public LP<sbyte> bombs;

		public sbyte getBomb(int square)
		{
			if (square >= 0 && square < this.bombs.vs.Count) {
				return this.bombs.vs [square];
			} else {
				Debug.LogError ("square error: " + square + "; " + this);
				return (sbyte)-1;
			}
		}

		public void setBomb(int square, sbyte bomb)
		{
			if (square >= 0 && square < this.bombs.vs.Count) {
				this.bombs.set (square, bomb);
			} else {
				Debug.LogError ("square error: " + square + "; " + this);
			}
		}

		#endregion

		public VP<bool> booom;

		//DoubleMatrix flags;//default is 0, -1 for mine, 1 for unpicked safe, 2 for picked safe
		public LP<sbyte> flags;

		#region board

		public LP<sbyte> board;//-1 for unknown; non-neg for safe

		public sbyte getPiece(int square)
		{
			if (square >= 0 && square < this.board.vs.Count) {
				return this.board.vs [square];
			} else {
				Debug.LogError ("square error: " + square + "; " + this);
				return (sbyte)-1;
			}
		}

		public void setPiece(int square, sbyte piece)
		{
			if (square >= 0 && square < this.board.vs.Count) {
				this.board.set (square, piece);
			} else {
				Debug.LogError ("square error: " + square + "; " + this);
			}
		}

		#endregion

		public VP<int> minesFound;

		public VP<bool> init;

		public LP<Neb> neb;

		public VP<bool> allowWatchBoomb;

		public VP<bool> isCustom;

		#region Constructor

		public enum Property
		{
			Y,
			X,
			K,
		 	bombs,
			booom,
			flags,
			board,
			minesFound,
			init,
			neb,
			allowWatchBoomb,
			isCustom
		}

		public static readonly List<byte> AllowNames = new List<byte> ();

		static MineSweeper()
		{
			AllowNames.Add ((byte)Property.Y);
			AllowNames.Add ((byte)Property.X);
			AllowNames.Add ((byte)Property.K);
			AllowNames.Add ((byte)Property.bombs);
			AllowNames.Add ((byte)Property.booom);
			AllowNames.Add ((byte)Property.flags);
			AllowNames.Add ((byte)Property.board);
			AllowNames.Add ((byte)Property.minesFound);
			AllowNames.Add ((byte)Property.init);
			AllowNames.Add ((byte)Property.neb);
			AllowNames.Add ((byte)Property.isCustom);
		}

		public MineSweeper() : base()
		{
			this.Y = new VP<int> (this, (byte)Property.Y, 10);
			this.X = new VP<int> (this, (byte)Property.X, 10);
			this.K = new VP<int> (this, (byte)Property.K, 10);
			this.bombs = new LP<sbyte> (this, (byte)Property.bombs);
			this.booom = new VP<bool> (this, (byte)Property.booom, false);
			this.flags = new LP<sbyte> (this, (byte)Property.flags);
			this.board = new LP<sbyte> (this, (byte)Property.board);
			this.minesFound = new VP<int> (this, (byte)Property.minesFound, 0);
			this.init = new VP<bool> (this, (byte)Property.init, false);
			this.neb = new LP<Neb> (this, (byte)Property.neb);
			this.allowWatchBoomb = new VP<bool> (this, (byte)Property.allowWatchBoomb, true);
			this.isCustom = new VP<bool> (this, (byte)Property.isCustom, false);
		}

		public bool isLoadFull()
		{
			bool ret = true;
			{
				// board
				if (ret) {
					if (this.board.vs.Count == 0) {
						Debug.LogError ("Don't have any piece");
						ret = false;
					}
				}
				// neb
				if (ret) {
					DataIdentity dataIdentity = null;
					if (DataIdentity.clientMap.TryGetValue (this, out dataIdentity)) {
						if (dataIdentity is MineSweeperIdentity) {
							MineSweeperIdentity mineSweeperIdentity = dataIdentity as MineSweeperIdentity;
							if (mineSweeperIdentity.neb != this.neb.vs.Count) {
								Debug.LogError ("neb count error");
								ret = false;
							}
						} else {
							Debug.LogError ("why not nebIdentity");
						}
					}
				}
				// each neb
				if (ret) {
					for (int i = 0; i < this.neb.vs.Count; i++) {
						Neb neb = this.neb.vs [i];
						if (!neb.isLoadFull ()) {
							ret = false;
							break;
						}
					}
				}
			}
			return ret;
		}

		#endregion

		#region Convert

		public static byte[] convertToBytes(MineSweeper mineSweeper, bool needCheckCustom = true)
		{
			// custom
			/*if (chess.isCustom.v && needCheckCustom) {
				string strFen = Core.unityPositionToFen (chess, Core.CanCorrect);
				Debug.LogError ("chess custom fen: " + strFen);
				Chess newChess = Core.unityMakePositionByFen (strFen, chess.chess960.v);
				return convertToBytes (newChess);
			}*/
			// normal
			byte[] byteArray;
			using(MemoryStream memStream = new MemoryStream())
			{
				using (BinaryWriter writer = new BinaryWriter (memStream)) {
					// write value
					{
						// public VP<int> N
						writer.Write(mineSweeper.Y.v);
						// public VP<int> M
						writer.Write(mineSweeper.X.v);
						// public VP<int> K
						writer.Write(mineSweeper.K.v);
						// public LP<sbyte> bombs
						for (int y = 0; y < mineSweeper.Y.v; y++)
							for (int x = 0; x < mineSweeper.X.v; x++) {
								sbyte value = 0;
								{
									int index = y * mineSweeper.X.v + x;
									if (index >= 0 && index < mineSweeper.bombs.vs.Count) {
										value = mineSweeper.bombs.vs [index];
									} else {
										Debug.LogError ("index error: " + index + "; " + mineSweeper);
									}
								}
								writer.Write (value);
							}
						// public VP<bool> booom
						writer.Write(mineSweeper.booom.v);
						// public LP<sbyte> flags
						for (int y = 0; y < mineSweeper.Y.v; y++)
							for (int x = 0; x < mineSweeper.X.v; x++) {
								sbyte value = 0;
								{
									int index = y * mineSweeper.X.v + x;
									if (index >= 0 && index < mineSweeper.flags.vs.Count) {
										value = mineSweeper.flags.vs [index];
									} else {
										Debug.LogError ("index error: " + index + "; " + mineSweeper);
									}
								}
								writer.Write (value);
							}
						// public LP<sbyte> board
						for (int y = 0; y < mineSweeper.Y.v; y++)
							for (int x = 0; x < mineSweeper.X.v; x++) {
								sbyte value = 0;
								{
									int index = y * mineSweeper.X.v + x;
									if (index >= 0 && index < mineSweeper.board.vs.Count) {
										value = mineSweeper.board.vs [index];
									} else {
										Debug.LogError ("index error: " + index + "; " + mineSweeper);
									}
								}
								writer.Write (value);
							}
						// public VP<int> minesFound
						writer.Write(mineSweeper.minesFound.v);
						// public VP<bool> init
						writer.Write(mineSweeper.init.v);
						// public LP<Neb> neb
						{
							writer.Write (mineSweeper.neb.vs.Count);
							for (int i = 0; i < mineSweeper.neb.vs.Count; i++) {
								Neb neb = mineSweeper.neb.vs [i];
								writer.Write (Neb.convertToBytes (neb));
							}
						}
					}
					// write to byteArray
					byteArray = memStream.ToArray ();
				}
			}
			return byteArray;
		}

		public static int parse(MineSweeper mineSweeper, byte[] byteArray, int start)
		{
			int count = start;
			int index = 0;
			bool isParseCorrect = true;
			while (count < byteArray.Length) {
				bool alreadyPassAll = false;
				switch (index) {
				case 0:
					{
						// public VP<int> N
						int size = sizeof(int);
						if (count + size <= byteArray.Length) {
							mineSweeper.Y.v = BitConverter.ToInt32 (byteArray, count);
							count += size;
						} else {
							Debug.LogError ("array not enough length: N: " + count + "; " + byteArray.Length);
							isParseCorrect = false;
						}
					}
					break;
				case 1:
					{
						// public VP<int> M
						int size = sizeof(int);
						if (count + size <= byteArray.Length) {
							mineSweeper.X.v = BitConverter.ToInt32 (byteArray, count);
							count += size;
						} else {
							Debug.LogError ("array not enough length: M: " + count + "; " + byteArray.Length);
							isParseCorrect = false;
						}
					}
					break;
				case 2:
					{
						// public VP<int> K
						int size = sizeof(int);
						if (count + size <= byteArray.Length) {
							mineSweeper.K.v = BitConverter.ToInt32 (byteArray, count);
							count += size;
						} else {
							Debug.LogError ("array not enough length: K: " + count + "; " + byteArray.Length);
							isParseCorrect = false;
						}
					}
					break;
				case 3:
					{
						// public LP<sbyte> bombs
						if (mineSweeper.Y.v >= MIN_DIMENSION_SIZE && mineSweeper.Y.v <= MAX_DIMENSION_SIZE
							&& mineSweeper.X.v >= MIN_DIMENSION_SIZE && mineSweeper.X.v <= MAX_DIMENSION_SIZE) {
							mineSweeper.bombs.clear();
							int size = sizeof(sbyte);
							for (int i = 0; i < mineSweeper.Y.v * mineSweeper.X.v; i++) {
								if (count + size <= byteArray.Length) {
									mineSweeper.bombs.add ((sbyte)byteArray [count]);
									count += size;
								} else {
									Debug.LogError ("array not enough length: bombs: " + count + "; " + byteArray.Length);
									isParseCorrect = false;
									break;
								}
							}
						} else {
							Debug.LogError ("dimension error: " + mineSweeper.Y.v + "; " + mineSweeper.X.v);
						}
					}
					break;
				case 4:
					{
						// public VP<bool> booom
						int size = sizeof(bool);
						if (count + size <= byteArray.Length) {
							mineSweeper.booom.v = BitConverter.ToBoolean (byteArray, count);
							count += size;
						} else {
							Debug.LogError ("array not enough length: boom: " + count + "; " + byteArray.Length);
							isParseCorrect = false;
						}
					}
					break;
				case 5:
					{
						// public LP<sbyte> flags
						if (mineSweeper.Y.v >= MIN_DIMENSION_SIZE && mineSweeper.Y.v <= MAX_DIMENSION_SIZE
							&& mineSweeper.X.v >= MIN_DIMENSION_SIZE && mineSweeper.X.v <= MAX_DIMENSION_SIZE) {
							mineSweeper.flags.clear();
							int size = sizeof(sbyte);
							for (int i = 0; i < mineSweeper.Y.v * mineSweeper.X.v; i++) {
								if (count + size <= byteArray.Length) {
									mineSweeper.flags.add ((sbyte)byteArray [count]);
									count += size;
								} else {
									Debug.LogError ("array not enough length: flags: " + count + "; " + byteArray.Length);
									isParseCorrect = false;
									break;
								}
							}
						} else {
							Debug.LogError ("dimension error: " + mineSweeper.Y.v + "; " + mineSweeper.X.v);
						}
					}
					break;
				case 6:
					{
						// public LP<sbyte> board
						if (mineSweeper.Y.v >= MIN_DIMENSION_SIZE && mineSweeper.Y.v <= MAX_DIMENSION_SIZE
							&& mineSweeper.X.v >= MIN_DIMENSION_SIZE && mineSweeper.X.v <= MAX_DIMENSION_SIZE) {
							mineSweeper.board.clear();
							int size = sizeof(sbyte);
							for (int i = 0; i < mineSweeper.Y.v * mineSweeper.X.v; i++) {
								if (count + size <= byteArray.Length) {
									mineSweeper.board.add ((sbyte)byteArray [count]);
									count += size;
								} else {
									Debug.LogError ("array not enough length: board: " + count + "; " + byteArray.Length);
									isParseCorrect = false;
									break;
								}
							}
						} else {
							Debug.LogError ("dimension error: " + mineSweeper.Y.v + "; " + mineSweeper.X.v);
						}
					}
					break;
				case 7:
					{
						// public VP<int> minesFound
						int size = sizeof(int);
						if (count + size <= byteArray.Length) {
							mineSweeper.minesFound.v = BitConverter.ToInt32 (byteArray, count);
							count += size;
						} else {
							Debug.LogError ("array not enough length: minesFound: " + count + "; " + byteArray.Length);
							isParseCorrect = false;
						}
					}
					break;
				case 8:
					{
						// public VP<bool> init
						int size = sizeof(bool);
						if (count + size <= byteArray.Length) {
							mineSweeper.init.v = BitConverter.ToBoolean (byteArray, count);
							count += size;
						} else {
							Debug.LogError ("array not enough length: init: " + count + "; " + byteArray.Length);
							isParseCorrect = false;
						}
					}
					break;
				case 9:
					{
						// public LP<Neb> neb
						mineSweeper.neb.clear ();
						int nebCount = 0;
						{
							int size = sizeof(int);
							if (count + size <= byteArray.Length) {
								nebCount = BitConverter.ToInt32 (byteArray, count);
								count += size;
							} else {
								Debug.LogError ("array not enough length: nebCount: " + count + "; " + byteArray.Length);
								isParseCorrect = false;
							}
						}
						// parse
						{
							// get list
							List<Neb> nebs = new List<Neb> ();
							for (int i = 0; i < nebCount; i++) {
								Neb neb = new Neb ();
								int nebByteLength = Neb.parse (neb, byteArray, count);
								if (nebByteLength > 0) {
									nebs.Add (neb);
									count += nebByteLength;
								} else {
									Debug.LogError ("cannot parse");
									break;
								}
							}
							// add
							for (int i = 0; i < nebs.Count; i++) {
								Neb neb = nebs [i];
								{
									neb.uid = mineSweeper.neb.makeId ();
								}
								mineSweeper.neb.add (neb);
							}
						}
					}
					break;
				default:
					// Debug.LogError ("unknown index: " + index);
					alreadyPassAll = true;
					break;
				}
				index++;
				if (!isParseCorrect) {
					Debug.LogError ("not parse correct");
					break;
				}
				if (alreadyPassAll) {
					break;
				}
			}
			// return
			if (!isParseCorrect) {
				Debug.LogError ("parse fail: " + count + "; " + byteArray.Length + "; " + start);
				return -1;
			} else {
				// Debug.LogError ("parse success: " + count + "; " + byteArray.Length + "; " + start);
				return (count - start);
			}
		}

		#endregion

		#region implement callBacks

		public override Type getType ()
		{
			return Type.MineSweeper;
		}

		public override int getTeamCount ()
		{
			return 1;
		}

		public override int getPerspectiveCount ()
		{
			return 1;
		}

		public override int getPlayerIndex ()
		{
			return 0;
		}

		public override bool checkLegalMove (InputData inputData)
		{
			GameMove gameMove = inputData.gameMove.v;
			if (gameMove != null) {
				if (GameData.IsUseRule (this)) {
					switch (gameMove.getType ()) {
					case GameMove.Type.MineSweeperMove:
						{
							MineSweeperMove mineSweeperMove = gameMove as MineSweeperMove;
							return Core.unityIsLegalMove (this, Core.CanCorrect, mineSweeperMove.move.v);
						}
						// break;
					default:
						Debug.LogError ("unknown game type: " + gameMove.getType () + "; " + this);
						break;
					}
				} else {
					switch (gameMove.getType ()) {
					case GameMove.Type.MineSweeperMove:
						return true;
					case GameMove.Type.MineSweeperCustomSet:
						return true;
					case GameMove.Type.EndTurn:
						return true;
					case GameMove.Type.Clear:
						return true;
					case GameMove.Type.MineSweeperCustomMove:
						return true;
					default:
						Debug.LogError ("unknown game type: " + gameMove.getType () + "; " + this);
						return true;
					}
				}
			} else {
				Debug.LogError ("gameMove null: " + this);
			}
			return false;
		}

		#region processGameMove

		private void processCustomGameMove(GameMove gameMove)
		{
			if (gameMove != null) {
				// make tempMineSweeper
				MineSweeper tempMineSweeper = DataUtils.cloneData (this) as MineSweeper;
				bool needUpdate = true;
				{
					switch (gameMove.getType ()) {
					case GameMove.Type.MineSweeperCustomSet:
						{
							MineSweeperCustomSet mineSweeperCustomSet = gameMove as MineSweeperCustomSet;
							// set piece
							{
								tempMineSweeper.setPiece (mineSweeperCustomSet.square.v, mineSweeperCustomSet.piece.v);
							}
						}
						break;
					case GameMove.Type.Clear:
						{
							for (int i = 0; i < tempMineSweeper.board.vs.Count; i++) {
								tempMineSweeper.board.vs [i] = -1;
							}
						}
						break;
					case GameMove.Type.MineSweeperCustomMove:
						{
							MineSweeperCustomMove mineSweeperCustomMove = gameMove as MineSweeperCustomMove;
							// update
							{
								tempMineSweeper.setPiece (mineSweeperCustomMove.dest.v, tempMineSweeper.getPiece (mineSweeperCustomMove.from.v));
								tempMineSweeper.setPiece (mineSweeperCustomMove.from.v, -1);
							}
						}
						break;
					default:
						Debug.LogError ("unknown type: " + gameMove.getType () + "; " + this);
						needUpdate = false;
						break;
					}
				}
				// Update
				if (needUpdate) {
					tempMineSweeper.init.v = true;
					tempMineSweeper.isCustom.v = true;
					DataUtils.copyData (this, tempMineSweeper, AllowNames);
				}
			} else {
				Debug.LogError ("gameMove null: " + this);
			}
		}

		public override void processGameMove (GameMove gameMove)
		{
			switch (gameMove.getType ()) {
			case GameMove.Type.MineSweeperMove:
				{
					// get information
					MineSweeperMove mineSweeperMove = gameMove as MineSweeperMove;
					// make request to native
					MineSweeper newMineSweeper = Core.unityDoMove (this, Core.CanCorrect, mineSweeperMove.move.v);
					// Copy to current chess
					DataUtils.copyData (this, newMineSweeper, AllowNames);
				}
				break;
			case GameMove.Type.None:
				break;
			case GameMove.Type.EndTurn:
				break;
			case GameMove.Type.MineSweeperCustomSet:
			case GameMove.Type.Clear:
			case GameMove.Type.MineSweeperCustomMove:
				this.processCustomGameMove (gameMove);
				break;
			default:
				Debug.LogError ("unknown gameMove: " + gameMove + "; " + this);
				this.processCustomGameMove (gameMove);
				break;
			}
		}

		#endregion

		#region getAIMove

		public override GameMove getAIMove (Computer.AI ai, bool isFindHint)
		{
			GameMove ret = new NonMove ();
			{
				// check is userNormalMove
				bool useNormalMove = true;
				{
					if (GameData.IsUseRule (this)) {
						useNormalMove = true;
					} else {
						// TODO Test useNormalMove = false;
						/*GameData gameData = this.findDataInParent<GameData>();
						if (gameData != null) {
							Turn turn = gameData.turn.v;
							if (turn != null) {
								if (turn.turn.v % 4 == 1 || turn.turn.v % 4 == 3) {
									useNormalMove = false;
								}
							} else {
								Debug.LogError ("turn null: " + this);
							}
						} else {
							Debug.LogError ("gameData null: " + this);
						}*/
						useNormalMove = true;
					}
				}
				// Process
				if (useNormalMove) {
					// sleep until get enough data
					{
						int count = 0;
						while (true) {
							if (isLoadFull ()) {
								break;
							} else {
								System.Threading.Thread.Sleep (1000);
								Debug.LogError ("need sleep: " + count);
								count++;
								if (count >= 360) {
									Debug.LogError ("why don't have data");
									return new NonMove ();
								}
							}
						}
					}
					// get ai
					MineSweeperAI mineSweeperAI = (ai != null && ai is MineSweeperAI) ? (MineSweeperAI)ai : new MineSweeperAI ();
					// search
					int move = Core.unityLetComputerThink (this, Core.CanCorrect, (int)mineSweeperAI.firstMoveType.v);
					// make move
					{
						MineSweeperMove mineSweeperMove = new MineSweeperMove ();
						{
							mineSweeperMove.move.v = move;
						}
						ret = mineSweeperMove;
					}
				} else {
					GameMove customMove = getCustomMove ();
					if (customMove != null) {
						ret = customMove;
					} else {
						Debug.LogError ("customMove null: " + this);
					}
				}
			}
			return ret;
		}

		public GameMove getCustomMove()
		{
			// find moves
			List<GameMove> moves = new List<GameMove> ();
			{
				// get custom set
				{
					for (int square = 0; square < this.board.vs.Count; square++) {
						MineSweeperCustomSet mineSweeperCustomSet = new MineSweeperCustomSet ();
						{
							mineSweeperCustomSet.square.v = square;
							mineSweeperCustomSet.piece.v = (this.board.vs [square] != -1) ? (sbyte)-1 : (sbyte)1;
						}
						moves.Add (mineSweeperCustomSet);
					}
				}
				// get custom move
				{
					for (int square = 0; square < this.board.vs.Count; square++) {
						if (this.board.vs [square] != -1) {
							for (int destSquare = 0; destSquare < this.board.vs.Count; destSquare++) {
								if (destSquare != square) {
									MineSweeperCustomMove mineSweeperCustomMove = new MineSweeperCustomMove ();
									{
										mineSweeperCustomMove.from.v = square;
										mineSweeperCustomMove.dest.v = destSquare;
									}
									moves.Add (mineSweeperCustomMove);
								}
							}
						}
					}
				}
				// get clear
				{
					Clear clear = new Clear ();
					{

					}
					moves.Add (clear);
				}
				// endTurn
				{
					EndTurn endTurn = new EndTurn ();
					{

					}
					moves.Add (endTurn);
				}
			}
			// choose
			if (moves.Count > 0) {
				System.Random random = new System.Random ();
				int index = random.Next (0, moves.Count);
				if (index >= 0 && index < moves.Count) {
					return moves [index];
				} else {
					Debug.LogError ("index error: " + index + "; " + this);
					return null;
				}
			} else {
				return null;
			}
		}

		#endregion

		public override Result isGameFinish ()
		{
			Result result = Result.makeDefault ();
			// process
			// if (GameData.IsUseRule (this)) 
			{
				int isGameFinish = Core.unityIsGameFinish (this, Core.CanCorrect);
				switch (isGameFinish) {
				case 0:
					{
						result.isGameFinish = false;
					}
					break;
				case 1:
					// you win
					{
						result.isGameFinish = true;
						// score
						result.scores.Add (new GameType.Score (0, 1));// white
					}
					break;
				case 2:
					// you lose
					{
						result.isGameFinish = true;
						// score
						result.scores.Add (new GameType.Score (0, 0));// white
					}
					break;
				default:
					Debug.LogError ("unknown result: " + this);
					break;
				}
			}
			// return
			return result;
		}

		#endregion

	}
}