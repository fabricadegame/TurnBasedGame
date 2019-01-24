﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AccountEmailUI : UIBehavior<AccountEmailUI.UIData>
{

	#region UIData

	public class UIData : AccountUI.UIData.Sub
	{

		public VP<EditData<AccountEmail>> editAccountEmail;

		#region Type

		public enum Type
		{
			Login,
			Register,
			Show
		}

		public VP<Type> type;

		public VP<RequestChangeEnumUI.UIData> changeType;

		public void makeRequestChangeType (RequestChangeUpdate<int>.UpdateData update, int newTypeInt)
		{
			Type newType = (Type)newTypeInt;
			if (newType == Type.Login || newType == Type.Register) {
				this.type.v = newType;
			} else {
				Debug.LogError ("wrong type: " + this);
			}
		}

		#endregion

		#region email

		public VP<RequestChangeStringUI.UIData> email;

		public void makeRequestChangeEmail (RequestChangeUpdate<string>.UpdateData update, string newEmail)
		{
			// Find
			AccountEmail accountEmail = null;
			{
				EditData<AccountEmail> editAccountEmail = this.editAccountEmail.v;
				if (editAccountEmail != null) {
					accountEmail = editAccountEmail.show.v.data;
				} else {
					Debug.LogError ("editAccountEmail null: " + this);
				}
			}
			// Process
			if (accountEmail != null) {
				if (this.type.v == Type.Login || this.type.v == Type.Register) {
					accountEmail.email.v = newEmail;
				}
			} else {
				Debug.LogError ("accountEmail null: " + this);
			}
		}

		#endregion

		#region password

		public VP<RequestChangeStringUI.UIData> password;

		public void makeRequestChangePassword (RequestChangeUpdate<string>.UpdateData update, string newPassword)
		{
			// Find
			AccountEmail accountEmail = null;
			{
				EditData<AccountEmail> editAccountEmail = this.editAccountEmail.v;
				if (editAccountEmail != null) {
					accountEmail = editAccountEmail.show.v.data;
				} else {
					Debug.LogError ("editAccountEmail null: " + this);
				}
			}
			// Process
			if (accountEmail != null) {
				if (this.type.v == Type.Login || this.type.v == Type.Register) {
					accountEmail.password.v = newPassword;
				} else {
					Debug.LogError ("why not login: " + this);
				}
			} else {
				Debug.LogError ("accountEmail null: " + this);
			}
		}

		public VP<RequestChangeStringUI.UIData> retypePassword;

		#endregion

		#region customName

		public VP<RequestChangeStringUI.UIData> customName;

		public void makeRequestChangeCustomName (RequestChangeUpdate<string>.UpdateData update, string newCustomName)
		{
			// Find
			AccountEmail accountEmail = null;
			{
				EditData<AccountEmail> editAccountEmail = this.editAccountEmail.v;
				if (editAccountEmail != null) {
					accountEmail = editAccountEmail.show.v.data;
				} else {
					Debug.LogError ("editAccountEmail null: " + this);
				}
			}
			// Process
			if (accountEmail != null) {
				accountEmail.requestChangeCustomName (accountEmail.getRequestAccountUserId(), newCustomName);
			} else {
				Debug.LogError ("accountEmail null: " + this);
			}
		}

		#endregion

		#region avatarUrl

		public VP<RequestChangeStringUI.UIData> avatarUrl;

		public void makeRequestChangeAvatarUrl (RequestChangeUpdate<string>.UpdateData update, string newAvatarUrl)
		{
			// Find
			AccountEmail accountEmail = null;
			{
				EditData<AccountEmail> editAccountEmail = this.editAccountEmail.v;
				if (editAccountEmail != null) {
					accountEmail = editAccountEmail.show.v.data;
				} else {
					Debug.LogError ("editAccountEmail null: " + this);
				}
			}
			// Process
			if (accountEmail != null) {
				accountEmail.requestChangeAvatarUrl (accountEmail.getRequestAccountUserId (), newAvatarUrl);
			} else {
				Debug.LogError ("accountEmail null: " + this);
			}
		}

		#endregion

		#region Constructor

		public enum Property
		{
			editAccountEmail,
			type,
			changeType,
			email,
			password,
			retypePassword,
			customName,
			avatarUrl
		}

		public UIData() : base()
		{
			this.editAccountEmail = new VP<EditData<AccountEmail>>(this, (byte)Property.editAccountEmail, new EditData<AccountEmail>());
			// type
			{
				this.type = new VP<Type>(this, (byte)Property.type, Type.Login);
				// changeType
				{
					this.changeType = new VP<RequestChangeEnumUI.UIData>(this, (byte)Property.changeType, new RequestChangeEnumUI.UIData());
					// event
					this.changeType.v.updateData.v.request.v = makeRequestChangeType;
					// options
					{
						this.changeType.v.options.add("Login");
						this.changeType.v.options.add("Register");
					}
				}
			}
			// email
			{
				this.email = new VP<RequestChangeStringUI.UIData>(this, (byte)Property.email, new RequestChangeStringUI.UIData());
				// event
				this.email.v.updateData.v.request.v = makeRequestChangeEmail;
			}
			// password
			{
				this.password = new VP<RequestChangeStringUI.UIData>(this, (byte)Property.password, new RequestChangeStringUI.UIData());
				// event
				this.password.v.updateData.v.request.v = makeRequestChangePassword;
			}
			// retypePassword
			{
				this.retypePassword = new VP<RequestChangeStringUI.UIData>(this, (byte)Property.retypePassword, new RequestChangeStringUI.UIData());
			}
			// customName
			{
				this.customName = new VP<RequestChangeStringUI.UIData>(this, (byte)Property.customName, new RequestChangeStringUI.UIData());
				// event
				this.customName.v.updateData.v.request.v = makeRequestChangeCustomName;
			}
			// avatarUrl
			{
				this.avatarUrl = new VP<RequestChangeStringUI.UIData>(this, (byte)Property.avatarUrl, new RequestChangeStringUI.UIData());
				// event
				this.avatarUrl.v.updateData.v.request.v = makeRequestChangeAvatarUrl;
			}
		}

		#endregion

		public override Account.Type getType ()
		{
			return Account.Type.EMAIL;
		}

	}

	#endregion

	#region Refresh

	#region txt

	public Text lbTitle;
	public static readonly TxtLanguage txtTitle = new TxtLanguage();

	public Text lbChangeType;
	public static readonly TxtLanguage txtChangeType = new TxtLanguage();
	public static readonly TxtLanguage txtLogin = new TxtLanguage ();
	public static readonly TxtLanguage txtRegister = new TxtLanguage();

	public Text lbEmail;
	public static readonly TxtLanguage txtEmail = new TxtLanguage();

	public Text lbPassword;
	public static readonly TxtLanguage txtPassword = new TxtLanguage();

	public Text lbRetypePassword;
	public static readonly TxtLanguage txtRetypePassword = new TxtLanguage();

	public Text tvChangePassword;
	public static readonly TxtLanguage txtChangePassword = new TxtLanguage();

	public Text lbCustomName;
	public static readonly TxtLanguage txtCustomName = new TxtLanguage();

	public Text lbAvatarUrl;
	public static readonly TxtLanguage txtAvatarUrl = new TxtLanguage ();

	static AccountEmailUI()
	{
		txtTitle.add (Language.Type.vi, "Tài khoản email");

		txtChangeType.add (Language.Type.vi, "Đổi Loại");
		txtLogin.add (Language.Type.vi, "Đăng Nhập");
		txtRegister.add (Language.Type.vi, "Đăng Ký");

		txtEmail.add (Language.Type.vi, "Email");
		txtPassword.add (Language.Type.vi, "Mật Khẩu");
		txtRetypePassword.add (Language.Type.vi, "Gõ lại mật khẩu");
		txtChangePassword.add (Language.Type.vi, "Đổi mật khẩu");
		txtCustomName.add (Language.Type.vi, "Tên");
		txtAvatarUrl.add (Language.Type.vi, "Đường dẫn avatar");
	}

	#endregion

	private bool needReset = true;
	public GameObject differentIndicator;

	public override void refresh ()
	{
		if (dirty) {
			dirty = false;
			if (this.data != null) {
				EditData<AccountEmail> editAccountEmail = this.data.editAccountEmail.v;
				if (editAccountEmail != null) {
					editAccountEmail.update ();
					// get show
					AccountEmail show = editAccountEmail.show.v.data;
					AccountEmail compare = editAccountEmail.compare.v.data;
					if (show != null) {
						// differentIndicator
						if (differentIndicator != null) {
							bool isDifferent = false;
							{
								if (editAccountEmail.compareOtherType.v.data != null) {
									if (editAccountEmail.compareOtherType.v.data.GetType () != show.GetType ()) {
										isDifferent = true;
									}
								}
							}
							differentIndicator.SetActive (isDifferent);
						} else {
							Debug.LogError ("differentIndicator null: " + this);
						}
						// set component
						{
							// changeType
							{
								if (this.data.type.v == UIData.Type.Show) {
									this.data.changeType.v = null;
								} else {
									RequestChangeEnumUI.UIData changeType = this.data.changeType.newOrOld<RequestChangeEnumUI.UIData> ();
									{
										// event
										changeType.updateData.v.request.v = this.data.makeRequestChangeType;
										// options
										{
											if (changeType.options.vs.Count == 2) {
												changeType.options.set (0, txtLogin.get ("Login"));
												changeType.options.set (1, txtRegister.get ("Register"));
											} else {
												changeType.options.clear ();
												changeType.options.add (txtLogin.get ("Login"));
												changeType.options.add (txtRegister.get ("Register"));
											}
										}
									}
									this.data.changeType.v = changeType;
								}
							}
							// email
							// password
							{
								bool isNeed = false;
								{
									if (this.data.type.v == UIData.Type.Login || this.data.type.v == UIData.Type.Register) {
										isNeed = true;
									} else if (this.data.type.v == UIData.Type.Show) {
										if (editAccountEmail.canEdit.v) {
											// is your account?
											uint humanId = 0;
											{
												Human human = show.findDataInParent<Human> ();
												if (human != null) {
													humanId = human.playerId.v;
												} else {
													Debug.LogError ("human null: " + this);
												}
											}
											isNeed = (humanId == Server.getProfileUserId (show));
										} else {
											isNeed = false;
										}
									}
								}
								if (isNeed) {
									if (this.data.password.v == null) {
										RequestChangeStringUI.UIData password = new RequestChangeStringUI.UIData ();
										{
											password.uid = this.data.password.makeId ();
											// event
											password.updateData.v.request.v = this.data.makeRequestChangePassword;
										}
										this.data.password.v = password;
									}
								} else {
									this.data.password.v = null;
								}
							}
							// retypePassword
							{
								bool isNeed = false;
								{
									if (this.data.type.v == UIData.Type.Register) {
										isNeed = true;
									} else if (this.data.type.v == UIData.Type.Show) {
										if (editAccountEmail.canEdit.v) {
											// is your account?
											uint humanId = 0;
											{
												Human human = show.findDataInParent<Human> ();
												if (human != null) {
													humanId = human.playerId.v;
												} else {
													Debug.LogError ("human null: " + this);
												}
											}
											isNeed = (humanId == Server.getProfileUserId (show));
										} else {
											isNeed = false;
										}
									}
								}
								if (isNeed) {
									if (this.data.retypePassword.v == null) {
										RequestChangeStringUI.UIData retypePassword = new RequestChangeStringUI.UIData ();
										{
											retypePassword.uid = this.data.retypePassword.makeId ();
										}
										this.data.retypePassword.v = retypePassword;
									}
								} else {
									this.data.retypePassword.v = null;
								}
							}
							// customName
							{
								bool isNeed = false;
								{
									if (this.data.type.v == UIData.Type.Register || this.data.type.v == UIData.Type.Show) {
										isNeed = true;
									} else if (this.data.type.v == UIData.Type.Login) {
										isNeed = false;
									}
								}
								if (isNeed) {
									if (this.data.customName.v == null) {
										RequestChangeStringUI.UIData customName = new RequestChangeStringUI.UIData ();
										{
											customName.uid = this.data.customName.makeId ();
										}
										this.data.customName.v = customName;
									}
								} else {
									this.data.customName.v = null;
								}
							}
							// avatarUrl
							{
								bool isNeed = false;
								{
									if (this.data.type.v == UIData.Type.Show) {
										isNeed = true;
									}
								}
								if (isNeed) {
									if (this.data.avatarUrl.v == null) {
										RequestChangeStringUI.UIData avatarUrl = new RequestChangeStringUI.UIData ();
										{
											avatarUrl.uid = this.data.avatarUrl.makeId ();
										}
										this.data.avatarUrl.v = avatarUrl;
									}
								} else {
									this.data.avatarUrl.v = null;
								}
							}

							// btnChangePasswordContainer
							{
								if (btnChangePasswordContainer != null) {
									btnChangePasswordContainer.gameObject.SetActive (this.data.password.v != null 
										&& this.data.retypePassword.v != null && this.data.type.v == UIData.Type.Show);
								} else {
									Debug.LogError ("btnChangePasswordContainer");
								}
							}
						}
						// request
						{
							// get server state
							Server.State.Type serverState = Server.State.Type.Connect;
							{
								if (this.data.type.v == UIData.Type.Login || this.data.type.v == UIData.Type.Register) {
									serverState = Server.State.Type.Connect;
								} else {
									Server server = show.findDataInParent<Server> ();
									if (server != null) {
										if (server.state.v != null) {
											serverState = server.state.v.getType ();
										} else {
											Debug.LogError ("server state null: " + this);
										}
									} else {
										Debug.LogError ("server null: " + this);
									}
								}
							}
							// set origin
							{
								// changeType
								{
									RequestChangeEnumUI.UIData changeType = this.data.changeType.v;
									if (changeType != null) {
										// update
										RequestChangeUpdate<int>.UpdateData updateData = changeType.updateData.v;
										if (updateData != null) {
											updateData.origin.v = (int)this.data.type.v;
											updateData.canRequestChange.v = editAccountEmail.canEdit.v;
											updateData.serverState.v = serverState;
										} else {
											Debug.LogError ("updateData null: " + this);
										}
									} else {
										Debug.LogError ("changeType null: " + this);
									}
									// Container
									if (changeTypeContainer != null) {
										changeTypeContainer.parent.gameObject.SetActive (changeType != null);
									} else {
										Debug.LogError ("changeTypeContainer null: " + this);
									}
								}
								// email
								{
									RequestChangeStringUI.UIData email = this.data.email.v;
									if (email != null) {
										// update
										RequestChangeUpdate<string>.UpdateData updateData = email.updateData.v;
										if (updateData != null) {
											updateData.origin.v = show.email.v;
											// requestChange?
											{
												bool canChange = false;
												{
													if (this.data.type.v == UIData.Type.Login || this.data.type.v == UIData.Type.Register) {
														canChange = true;
													}
												}
												updateData.canRequestChange.v = canChange;
											}
											updateData.serverState.v = serverState;
										} else {
											Debug.LogError ("updateData null: " + this);
										}
										// compare
										{
											if (compare != null) {
												email.showDifferent.v = true;
												email.compare.v = compare.email.v;
											} else {
												email.showDifferent.v = false;
											}
										}
									} else {
										Debug.LogError ("email null: " + this);
									}
									// Container
									if (emailContainer != null) {
										emailContainer.parent.gameObject.SetActive (email != null);
									} else {
										Debug.LogError ("emailContainer null: " + this);
									}
								}
								// password
								{
									RequestChangeStringUI.UIData password = this.data.password.v;
									if (password != null) {
										// update
										RequestChangeUpdate<string>.UpdateData updateData = password.updateData.v;
										if (updateData != null) {
											updateData.origin.v = "";
											updateData.canRequestChange.v = editAccountEmail.canEdit.v;
											updateData.serverState.v = serverState;
										} else {
											Debug.LogError ("updateData null: " + this);
										}
										// compare
										{
											password.showDifferent.v = false;
										}
									} else {
										Debug.LogError ("password null: " + this);
									}
									// Container
									if (passwordContainer != null) {
										passwordContainer.parent.gameObject.SetActive (password != null);
									} else {
										Debug.LogError ("passwordContainer null: " + this);
									}
								}
								// retypePassword
								{
									RequestChangeStringUI.UIData retypePassword = this.data.retypePassword.v;
									if (retypePassword != null) {
										// update
										RequestChangeUpdate<string>.UpdateData updateData = retypePassword.updateData.v;
										if (updateData != null) {
											updateData.origin.v = "";
											updateData.canRequestChange.v = editAccountEmail.canEdit.v;
											updateData.serverState.v = serverState;
										} else {
											Debug.LogError ("updateData null: " + this);
										}
										// compare
										{
											retypePassword.showDifferent.v = false;
										}
									} else {
										Debug.LogError ("retypePassword null: " + this);
									}
									// Container
									if (retypePasswordContainer != null) {
										retypePasswordContainer.parent.gameObject.SetActive (retypePassword != null);
									} else {
										Debug.LogError ("retypePasswordContainer null: " + this);
									}
								}
								// customName
								{
									RequestChangeStringUI.UIData customName = this.data.customName.v;
									if (customName != null) {
										// update
										RequestChangeUpdate<string>.UpdateData updateData = customName.updateData.v;
										if (updateData != null) {
											updateData.origin.v = show.customName.v;
											updateData.canRequestChange.v = editAccountEmail.canEdit.v;
											updateData.serverState.v = serverState;
										} else {
											Debug.LogError ("updateData null: " + this);
										}
										// compare
										{
											if (compare != null) {
												customName.showDifferent.v = true;
												customName.compare.v = compare.customName.v;
											} else {
												customName.showDifferent.v = false;
											}
										}
									} else {
										Debug.LogError ("customName null: " + this);
									}
									// Container
									if (customNameContainer != null) {
										customNameContainer.parent.gameObject.SetActive (customName != null);
									} else {
										Debug.LogError ("customNameContainer null: " + this);
									}
								}
								// avatarUrl
								{
									RequestChangeStringUI.UIData avatarUrl = this.data.avatarUrl.v;
									if (avatarUrl != null) {
										// update
										RequestChangeUpdate<string>.UpdateData updateData = avatarUrl.updateData.v;
										if (updateData != null) {
											updateData.origin.v = show.avatarUrl.v;
											updateData.canRequestChange.v = editAccountEmail.canEdit.v;
											updateData.serverState.v = serverState;
										} else {
											Debug.LogError ("updateData null: " + this);
										}
										// compare
										{
											if (compare != null) {
												avatarUrl.showDifferent.v = true;
												avatarUrl.compare.v = compare.avatarUrl.v;
											} else {
												avatarUrl.showDifferent.v = false;
											}
										}
									} else {
										Debug.LogError ("avatarUrl null: " + this);
									}
									// Container
									if (avatarUrlContainer != null) {
										avatarUrlContainer.parent.gameObject.SetActive (avatarUrl != null);
									} else {
										Debug.LogError ("avatarUrlContainer null: " + this);
									}
								}
							}
						}
						// reset?
						if (needReset) {
							needReset = false;
							// changeType
							{
								RequestChangeEnumUI.UIData changeType = this.data.changeType.v;
								if (changeType != null) {
									// update
									RequestChangeUpdate<int>.UpdateData updateData = changeType.updateData.v;
									if (updateData != null) {
										updateData.current.v = (int)this.data.type.v;
										updateData.changeState.v = Data.ChangeState.None;
									} else {
										Debug.LogError ("updateData null: " + this);
									}
								} else {
									Debug.LogError ("changeType null: " + this);
								}
							}
							// email
							{
								RequestChangeStringUI.UIData email = this.data.email.v;
								if (email != null) {
									// update
									RequestChangeUpdate<string>.UpdateData updateData = email.updateData.v;
									if (updateData != null) {
										updateData.current.v = show.email.v;
										updateData.changeState.v = Data.ChangeState.None;
									} else {
										Debug.LogError ("updateData null: " + this);
									}
								} else {
									Debug.LogError ("email null: " + this);
								}
							}
							// password
							{
								RequestChangeStringUI.UIData password = this.data.password.v;
								if (password != null) {
									// update
									RequestChangeUpdate<string>.UpdateData updateData = password.updateData.v;
									if (updateData != null) {
										updateData.current.v = "";
										updateData.changeState.v = Data.ChangeState.None;
									} else {
										Debug.LogError ("updateData null: " + this);
									}
								} else {
									Debug.LogError ("password null: " + this);
								}
							}
							// retypePassword
							{
								RequestChangeStringUI.UIData retypePassword = this.data.retypePassword.v;
								if (retypePassword != null) {
									// update
									RequestChangeUpdate<string>.UpdateData updateData = retypePassword.updateData.v;
									if (updateData != null) {
										updateData.current.v = "";
										updateData.changeState.v = Data.ChangeState.None;
									} else {
										Debug.LogError ("updateData null: " + this);
									}
								} else {
									Debug.LogError ("retypePassword null: " + this);
								}
							}
							// customName
							{
								RequestChangeStringUI.UIData customName = this.data.customName.v;
								if (customName != null) {
									// update
									RequestChangeUpdate<string>.UpdateData updateData = customName.updateData.v;
									if (updateData != null) {
										updateData.current.v = show.customName.v;
										updateData.changeState.v = Data.ChangeState.None;
									} else {
										Debug.LogError ("updateData null: " + this);
									}
								} else {
									Debug.LogError ("customName null: " + this);
								}
							}
							// avatarUrl
							{
								RequestChangeStringUI.UIData avatarUrl = this.data.avatarUrl.v;
								if (avatarUrl != null) {
									// update
									RequestChangeUpdate<string>.UpdateData updateData = avatarUrl.updateData.v;
									if (updateData != null) {
										updateData.current.v = show.avatarUrl.v;
										updateData.changeState.v = Data.ChangeState.None;
									} else {
										Debug.LogError ("updateData null: " + this);
									}
								} else {
									Debug.LogError ("avatarUrl null: " + this);
								}
							}
						}
					} else {
						Debug.LogError ("show null: " + this);
					}
				} else {
					Debug.LogError ("editAccountEmail null: " + this);
				}
				// txt
				{
					if (lbTitle != null) {
						lbTitle.text = txtTitle.get ("Account Email");
					} else {
						Debug.LogError ("lbTitle null: " + this);
					}
					if (lbChangeType != null) {
						lbChangeType.text = txtChangeType.get ("Change Type");
					} else {
						Debug.LogError ("lbChangeType null: " + this);
					}
					if (lbEmail != null) {
						lbEmail.text = txtEmail.get ("Email");
					} else {
						Debug.LogError ("lbEmail null: " + this);
					}
					if (lbPassword != null) {
						lbPassword.text = txtPassword.get ("Password");
					} else {
						Debug.LogError ("lbPassword null: " + this);
					}
					if (lbRetypePassword != null) {
						lbRetypePassword.text = txtRetypePassword.get ("Retype Password");
					} else {
						Debug.LogError ("lbRetypePassword null: " + this);
					}
					if (tvChangePassword != null) {
						tvChangePassword.text = txtChangePassword.get ("Change Password");
					} else {
						Debug.LogError ("tvChangePassword null: " + this);
					}
					if (lbCustomName != null) {
						lbCustomName.text = txtCustomName.get ("Custom Name");
					} else {
						Debug.LogError ("lbCustomName null: " + this);
					}
					if (lbAvatarUrl != null) {
						lbAvatarUrl.text = txtAvatarUrl.get ("Avatar Url");
					} else {
						Debug.LogError ("lbAvatarUrl null: " + this);
					}
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

	public Transform btnChangePasswordContainer;

	public void onClickBtnChangePassword()
	{
		if (this.data != null) {
			EditData<AccountEmail> editAccountEmail = this.data.editAccountEmail.v;
			if (editAccountEmail != null) {
				AccountEmail originAccountEmail = editAccountEmail.origin.v.data;
				if (originAccountEmail != null) {
					originAccountEmail.requestChangePassword (Server.getProfileUserId (originAccountEmail), 
						this.data.password.v.updateData.v.current.v, this.data.retypePassword.v.updateData.v.current.v);
				} else {
					Debug.LogError ("originAccountEmail null: " + this);
				}
			} else {
				Debug.LogError ("editAccountEmail null: " + this);
			}
		} else {
			Debug.LogError ("data null: " + this);
		}
	}

	#region implement callBacks

	public RequestChangeEnumUI requestEnumPrefab;
	public RequestChangeStringUI requestStringPrefab;

	public Transform changeTypeContainer;
	public Transform emailContainer;
	public Transform passwordContainer;
	public Transform retypePasswordContainer;
	public Transform customNameContainer;
	public Transform avatarUrlContainer;

	private Server server = null;
	private Human human = null;

	public override void onAddCallBack<T> (T data)
	{
		if (data is UIData) {
			UIData uiData = data as UIData;
			// Setting
			Setting.get().addCallBack(this);
			// Child
			{
				uiData.editAccountEmail.allAddCallBack (this);
				// type
				uiData.changeType.allAddCallBack(this);
				uiData.email.allAddCallBack (this);
				uiData.password.allAddCallBack (this);
				uiData.retypePassword.allAddCallBack (this);
				uiData.customName.allAddCallBack (this);
				uiData.avatarUrl.allAddCallBack (this);
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
			// editAccountEmail
			{
				if (data is EditData<AccountEmail>) {
					EditData<AccountEmail> editAccountEmail = data as EditData<AccountEmail>;
					// Child
					{
						editAccountEmail.show.allAddCallBack (this);
						editAccountEmail.compare.allAddCallBack (this);
					}
					dirty = true;
					return;
				}
				// Child
				{
					if (data is AccountEmail) {
						AccountEmail accountEmail = data as AccountEmail;
						// Parent
						{
							DataUtils.addParentCallBack (accountEmail, this, ref this.server);
							DataUtils.addParentCallBack (accountEmail, this, ref this.human);
						}
						needReset = true;
						dirty = true;
						return;
					}
					// Parent
					{
						if (data is Server) {
							dirty = true;
							return;
						}
						if (data is Human) {
							dirty = true;
							return;
						}
					}
				}
			}
			if (data is RequestChangeStringUI.UIData) {
				RequestChangeStringUI.UIData requestChange = data as RequestChangeStringUI.UIData;
				// UI
				{
					WrapProperty wrapProperty = requestChange.p;
					if (wrapProperty != null) {
						switch ((UIData.Property)wrapProperty.n) {
						case UIData.Property.email:
							UIUtils.Instantiate (requestChange, requestStringPrefab, emailContainer);
							break;
						case UIData.Property.password:
							UIUtils.Instantiate (requestChange, requestStringPrefab, passwordContainer);
							break;
						case UIData.Property.retypePassword:
							UIUtils.Instantiate (requestChange, requestStringPrefab, retypePasswordContainer);
							break;
						case UIData.Property.customName:
							UIUtils.Instantiate (requestChange, requestStringPrefab, customNameContainer);
							break;
						case UIData.Property.avatarUrl:
							UIUtils.Instantiate (requestChange, requestStringPrefab, avatarUrlContainer);
							break;
						default:
							Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
							break;
						}
					} else {
						Debug.LogError ("wrapProperty null: " + this);
					}
				}
				dirty = true;
				return;
			}
			if (data is RequestChangeEnumUI.UIData) {
				RequestChangeEnumUI.UIData requestChange = data as RequestChangeEnumUI.UIData;
				// UI
				{
					WrapProperty wrapProperty = requestChange.p;
					if (wrapProperty != null) {
						switch ((UIData.Property)wrapProperty.n) {
						case UIData.Property.changeType:
							UIUtils.Instantiate (requestChange, requestEnumPrefab, changeTypeContainer);
							break;
						default:
							Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
							break;
						}
					} else {
						Debug.LogError ("wrapProperty null: " + this);
					}
				}
				dirty = true;
				return;
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
				uiData.editAccountEmail.allRemoveCallBack (this);
				// type
				uiData.changeType.allRemoveCallBack(this);
				uiData.email.allRemoveCallBack (this);
				uiData.password.allRemoveCallBack (this);
				uiData.retypePassword.allRemoveCallBack (this);
				uiData.customName.allRemoveCallBack (this);
				uiData.avatarUrl.allRemoveCallBack (this);
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
			// editAccountEmail
			{
				if (data is EditData<AccountEmail>) {
					EditData<AccountEmail> editAccountEmail = data as EditData<AccountEmail>;
					// Child
					{
						editAccountEmail.show.allRemoveCallBack (this);
						editAccountEmail.compare.allRemoveCallBack (this);
					}
					return;
				}
				// Child
				{
					if (data is AccountEmail) {
						AccountEmail accountEmail = data as AccountEmail;
						// Parent
						{
							DataUtils.removeParentCallBack (accountEmail, this, ref this.server);
							DataUtils.removeParentCallBack (accountEmail, this, ref this.human);
						}
						return;
					}
					// Parent
					{
						if (data is Server) {
							return;
						}
						if (data is Human) {
							return;
						}
					}
				}
			}
			if (data is RequestChangeStringUI.UIData) {
				RequestChangeStringUI.UIData requestChange = data as RequestChangeStringUI.UIData;
				// UI
				{
					requestChange.removeCallBackAndDestroy (typeof(RequestChangeStringUI));
				}
				return;
			}
			if (data is RequestChangeEnumUI.UIData) {
				RequestChangeEnumUI.UIData requestChange = data as RequestChangeEnumUI.UIData;
				// UI
				{
					requestChange.removeCallBackAndDestroy (typeof(RequestChangeEnumUI));
				}
				return;
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
			case UIData.Property.editAccountEmail:
				{
					ValueChangeUtils.replaceCallBack (this, syncs);
					dirty = true;
				}
				break;
			case UIData.Property.type:
				dirty = true;
				break;
			case UIData.Property.changeType:
				{
					ValueChangeUtils.replaceCallBack (this, syncs);
					dirty = true;
				}
				break;
			case UIData.Property.email:
				{
					ValueChangeUtils.replaceCallBack (this, syncs);
					dirty = true;
				}
				break;
			case UIData.Property.password:
				{
					ValueChangeUtils.replaceCallBack (this, syncs);
					dirty = true;
				}
				break;
			case UIData.Property.retypePassword:
				{
					ValueChangeUtils.replaceCallBack (this, syncs);
					dirty = true;
				}
				break;
			case UIData.Property.customName:
				{
					ValueChangeUtils.replaceCallBack (this, syncs);
					dirty = true;
				}
				break;
			case UIData.Property.avatarUrl:
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
			// editAccountEmail
			{
				if (wrapProperty.p is EditData<AccountEmail>) {
					switch ((EditData<AccountEmail>.Property)wrapProperty.n) {
					case EditData<AccountEmail>.Property.origin:
						dirty = true;
						break;
					case EditData<AccountEmail>.Property.show:
						{
							ValueChangeUtils.replaceCallBack (this, syncs);
							dirty = true;
						}
						break;
					case EditData<AccountEmail>.Property.compare:
						{
							ValueChangeUtils.replaceCallBack (this, syncs);
							dirty = true;
						}
						break;
					case EditData<AccountEmail>.Property.compareOtherType:
						dirty = true;
						break;
					case EditData<AccountEmail>.Property.canEdit:
						dirty = true;
						break;
					case EditData<AccountEmail>.Property.editType:
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
					if (wrapProperty.p is AccountEmail) {
						switch ((AccountEmail.Property)wrapProperty.n) {
						case AccountEmail.Property.email:
							dirty = true;
							break;
						case AccountEmail.Property.password:
							dirty = true;
							break;
						case AccountEmail.Property.customName:
							dirty = true;
							break;
						case AccountEmail.Property.avatarUrl:
							dirty = true;
							break;
						default:
							Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
							break;
						}
						return;
					}
					// Parent
					{
						if (wrapProperty.p is Server) {
							Server.State.OnUpdateSyncStateChange (wrapProperty, this);
							return;
						}
						if (wrapProperty.p is Human) {
							Human.onUpdateSyncPlayerIdChange (wrapProperty, this);
							return;
						}
					}
				}
			}
			if (wrapProperty.p is RequestChangeStringUI.UIData) {
				return;
			}
			if (wrapProperty.p is RequestChangeEnumUI.UIData) {
				return;
			}
		}
		Debug.LogError ("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
	}

	#endregion

}