﻿using UnityEngine;
using System.IO;
using System.Collections;
using Foundation.Tasks;

namespace Chess
{
	public class TestChess
	{

		public static void startTestMatch(int matchCount)
		{
			for (int i = 0; i < matchCount; i++) {
				UnityTask.Run (() => { 
					bool chess960 = false;
					Chess startChess = Core.unityMakePositionByFen (Chess.DefaultFen, chess960);
					// Make a match
					{
						int turn = 0;
						Chess chess = startChess;
						do{
							Debug.Log("before letComputerThink: "+turn);
							Debug.Log("positionToString: "+turn+"\n"+Common.positionToString(chess));
							Debug.Log("positionToFen: "+Core.unityPositionToFen(chess, Core.CanCorrect));

							// TODO cai nay de phong
							// Debug.LogError("positionToFen: \n"+Core.unityPositionToFen(positionBytes));
							int gameFinish = Core.unityIsGameFinish(chess, true);
							if(gameFinish==0){
								int move = Core.unityLetComputerThink(chess, true, 10, 19, 6000);
								if(move!=0){
									Debug.Log("test find ai move: "+turn+"; "+ move+"; "+Common.moveToString(move, chess960));
									// check legal move
									if(Core.unityIsLegalMove(chess, true, move)){
										Chess newChess = Core.unityDoMove(chess, true, move);
										// set new position bytes
										DataUtils.copyData(chess, newChess);
									}else{
										Debug.LogError("why not legal move: "+ move);
										break;
									}
								}else{
									Debug.LogError("why don't find any move, break");
								}
								turn++;
							}else{
								Debug.LogWarning("game finish in turn: "+ turn+"; "+gameFinish+ "; positionToString: \n"+Common.positionToString(chess));
								switch (gameFinish) {
								case 1:
									Debug.LogWarning("white win");
									break;
								case 2:
									Debug.LogWarning("black win");
									break;
								case 3:
									Debug.LogWarning("the game is draw");
									break;
								default:
									Debug.LogWarning("unknown gameFinish: "+gameFinish);
									break;
								}
								break;
							}
							System.Threading.Thread.Sleep(1000);
						}while (!Test.stop);
					}
				});
			}
		}
	}
}