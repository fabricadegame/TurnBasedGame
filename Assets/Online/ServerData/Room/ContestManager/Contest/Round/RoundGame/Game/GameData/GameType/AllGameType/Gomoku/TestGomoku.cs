﻿using UnityEngine;
using System.Collections;
using Foundation.Tasks;

namespace Gomoku
{
	public class TestGomoku
	{

		public static void startTestMatch(int matchCount)
		{
			for (int i = 0; i < matchCount; i++) {
				UnityTask.Run (() => { 
					Gomoku startGomoku =  null;
					{
						int size = 19;
						startGomoku = Core.unityMakeDefaultPosition(size);
					}
					// Make a match
					{
						int turn = 0;
						Gomoku gomoku = startGomoku;
						do{
							Debug.Log(string.Format("before letComputerThink: {0}", turn));
							Debug.Log(Common.printPosition(gomoku));
							int gameFinish = Core.unityIsGameFinish(gomoku, true);
							Debug.Log("gameFinish: "+gameFinish);
							if(gameFinish==0){
								// letComputerThink
								int searchDepth = 8;
								int timeLimit = 10000;
								int level = 12;
								int move = Core.unityLetComputerThink(gomoku, true, searchDepth, timeLimit, level);
								// print move to string
								Debug.Log("find ai move: "+move);
								{
									// check legal move
									if(Core.unityIsLegalMove(gomoku, true, move)){
										// do move
										Gomoku newGomoku = Core.unityDoMove(gomoku, true, move);
										// set new position bytes
										if(newGomoku!=null){
											gomoku = newGomoku;
										}else{
											Debug.Log("error, do move");
											break;
										}
									}else{
										Debug.Log("error: why not legal move: "+move);
										break;
									}
								}
								turn++;
							}else{
								Debug.LogWarning("game finish in turn: "+turn);
								Debug.LogWarning(Common.printPosition(gomoku));
								switch (gameFinish) {
								case 1:
									Debug.LogWarning("black win: "+ turn);
									break;
								case 2:
									Debug.LogWarning("white win: "+ turn);
									break;
								case 3:
									Debug.LogWarning("the game is draw: "+turn);
									break;
								default:
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