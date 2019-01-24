﻿using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace GameManager.Match.Elimination
{
	public class ChooseEliminationRoundBracketUI : UIBehavior<ChooseEliminationRoundBracketUI.UIData>
	{

		#region UIData

		public class UIData : Data
		{

			public VP<ReferenceData<Bracket>> bracket;

			#region Constructor

			public enum Property
			{
				bracket
			}

			public UIData() : base()
			{
				this.bracket = new VP<ReferenceData<Bracket>>(this, (byte)Property.bracket, new ReferenceData<Bracket>(null));
			}

			#endregion

		}

		#endregion

		#region Refresh

		#region txt

		public static readonly TxtLanguage txtBracket = new TxtLanguage ();
		public static readonly TxtLanguage txtTeam = new TxtLanguage();
		public static readonly TxtLanguage txtWinner = new TxtLanguage ();
		public static readonly TxtLanguage txtLoser = new TxtLanguage ();

		static ChooseEliminationRoundBracketUI()
		{
			txtBracket.add (Language.Type.vi, "Nhánh");
			txtTeam.add (Language.Type.vi, "Đội");
			txtWinner.add (Language.Type.vi, "Đội thắng");
			txtLoser.add (Language.Type.vi, "Đội thua");
		}

		#endregion

		public Text tvIndex;
		public Text tvBracket;

		public override void refresh ()
		{
			if (dirty) {
				dirty = false;
				if (this.data != null) {
					Bracket bracket = this.data.bracket.v.data;
					if (bracket != null) {
						// tvIndex
						{
							if (tvIndex != null) {
								tvIndex.text = txtBracket.get ("Bracket") + ": " + bracket.index.v;
							} else {
								Debug.LogError ("tvIndex null: " + this);
							}
						}
						// tvBracket
						{
							if (tvBracket != null) {
								if (bracket.state.v.getType () == Bracket.State.Type.Play) {
									StringBuilder builder = new StringBuilder ();
									{
										// add team in contest
										foreach (BracketContest bracketContest in bracket.bracketContests.vs) {
											if (bracketContest.isActive.v) {
												foreach (int teamIndex in bracketContest.teamIndexs.vs) {
													builder.Append (teamIndex + ", ");
												}
											}
										}
										// add bye
										foreach (int byeTeamIndex in bracket.byeTeamIndexs.vs) {
											builder.Append (byeTeamIndex + ", ");
										}
									}
									tvBracket.text = txtTeam.get ("Team") + ": " + builder.ToString ();
								} else {
									BracketStateEnd bracketStateEnd = bracket.state.v as BracketStateEnd;
									if (bracketStateEnd != null) {
										// get winners
										StringBuilder winners = new StringBuilder ();
										{
											winners.Append (txtWinner.get ("Winner") + ": ");
											foreach (int winner in bracketStateEnd.winTeamIndexs.vs) {
												winners.Append (winner + ", ");
											}
										}
										// get losers
										StringBuilder losers = new StringBuilder ();
										{
											losers.Append (txtLoser.get ("Loser") + ": ");
											foreach (int loser in bracketStateEnd.loseTeamIndexs.vs) {
												losers.Append (loser + ", ");
											}
										}
										// set
										tvBracket.text = winners.ToString () + "\n" + losers.ToString ();
									} else {
										Debug.LogError ("bracketStateEnd null: " + this);
									}
								}
							} else {
								Debug.LogError ("tvBracket null: " + this);
							}
						}
					} else {
						// Debug.LogError ("bracket null: " + this);
					}
				} else {
					// Debug.LogError ("data null: " + this);
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
				// Setting
				Setting.get().addCallBack(this);
				// Child
				{
					uiData.bracket.allAddCallBack (this);
				}
				dirty = true;
				return;
			}
			// Setting
			if (data is Setting) {
				dirty = true;
				return;
			}
			// Child
			{
				if (data is Bracket) {
					Bracket bracket = data as Bracket;
					// Child
					{
						bracket.state.allAddCallBack (this);
						bracket.bracketContests.allAddCallBack (this);
					}
					dirty = true;
					return;
				}
				// Child
				{
					if (data is Bracket.State) {
						dirty = true;
						return;
					}
					if (data is BracketContest) {
						dirty = true;
						return;
					}
				}
			}
			Debug.LogError ("Don't process: " + data + "; " + this);
		}

		public override void onRemoveCallBack<T> (T data, bool isHide)
		{
			if (data is UIData) {
				UIData uiData = data as UIData;
				// Setting
				Setting.get().removeCallBack(this);
				// Child
				{
					uiData.bracket.allRemoveCallBack (this);
				}
				this.setDataNull (uiData);
				return;
			}
			// Setting
			if (data is Setting) {
				return;
			}
			// Child
			{
				if (data is Bracket) {
					Bracket bracket = data as Bracket;
					// Child
					{
						bracket.state.allRemoveCallBack (this);
						bracket.bracketContests.allRemoveCallBack (this);
					}
					return;
				}
				// Child
				{
					if (data is Bracket.State) {
						return;
					}
					if (data is BracketContest) {
						return;
					}
				}
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
				case UIData.Property.bracket:
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
			// Setting
			if (wrapProperty.p is Setting) {
				switch ((Setting.Property)wrapProperty.n) {
				case Setting.Property.language:
					dirty = true;
					break;
				case Setting.Property.showLastMove:
					break;
				case Setting.Property.viewUrlImage:
					break;
				case Setting.Property.animationSetting:
					break;
				case Setting.Property.maxThinkCount:
					break;
				default:
					Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
					break;
				}
				return;
			}
			// Child
			{
				if (wrapProperty.p is Bracket) {
					switch ((Bracket.Property)wrapProperty.n) {
					case Bracket.Property.isActive:
						break;
					case Bracket.Property.state:
						{
							ValueChangeUtils.replaceCallBack (this, syncs);
							dirty = true;
						}
						break;
					case Bracket.Property.index:
						dirty = true;
						break;
					case Bracket.Property.bracketContests:
						{
							ValueChangeUtils.replaceCallBack (this, syncs);
							dirty = true;
						}
						break;
					case Bracket.Property.byeTeamIndexs:
						dirty = true;
						break;
					default:
						Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
						break;
					}
					return;
				}
				// Child
				{
					if (wrapProperty.p is Bracket.State) {
						Bracket.State state = wrapProperty.p as Bracket.State;
						switch (state.getType ()) {
						case Bracket.State.Type.Play:
							break;
						case Bracket.State.Type.End:
							{
								switch ((BracketStateEnd.Property)wrapProperty.n) {
								case BracketStateEnd.Property.winTeamIndexs:
									dirty = true;
									break;
								case BracketStateEnd.Property.loseTeamIndexs:
									dirty = true;
									break;
								default:
									Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
									break;
								}
							}
							break;
						default:
							Debug.LogError ("unknown type: " + state.getType () + "; " + this);
							break;
						}
						return;
					}
					if (wrapProperty.p is BracketContest) {
						switch ((BracketContest.Property)wrapProperty.n) {
						case BracketContest.Property.isActive:
							dirty = true;
							break;
						case BracketContest.Property.index:
							break;
						case BracketContest.Property.teamIndexs:
							dirty = true;
							break;
						case BracketContest.Property.contest:
							break;
						default:
							Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
							break;
						}
					}
				}
			}
			Debug.LogError ("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
		}

		#endregion

	}
}