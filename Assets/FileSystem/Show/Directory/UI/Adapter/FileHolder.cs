﻿using UnityEngine;
using UnityEngine.UI;
using frame8.Logic.Misc.Visual.UI.ScrollRectItemsAdapter;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace FileSystem
{
	public class FileHolder : SriaHolderBehavior<FileHolder.UIData>
	{

		#region UIData

		public class UIData : BaseItemViewsHolder
		{

			public VP<FileSystemInfo> file;

			#region Constructor

			public enum Property
			{
				file
			}

			public UIData() : base()
			{
				this.file = new VP<FileSystemInfo>(this, (byte)Property.file, null);
			}

			#endregion

			public void updateView(FileAdapter.UIData myParams)
			{
				// Find
				FileSystemInfo fileSystemInfo = null;
				{
					if (ItemIndex >= 0 && ItemIndex < myParams.files.Count) {
						fileSystemInfo = myParams.files [ItemIndex];
					} else {
						Debug.LogError ("ItemIdex error: " + this);
					}
				}
				// Update
				this.file.v = fileSystemInfo;
			}

		}

		#endregion

		#region Refresh

		public Sprite fileSprite;
		public Sprite directorySprite;
		public Image ivIcon;

		public Text tvName;
		public Text tvTime;
		public Text tvSize;

		public GameObject selectIndicator;

		public override void refresh ()
		{
			base.refresh ();
			if (dirty) {
				dirty = false;
				if (this.data != null) {
					FileSystemInfo file = this.data.file.v;
					if (file != null) {
						// icon
						{
							if (ivIcon != null) {
								if (file is FileInfo) {
									ivIcon.sprite = fileSprite;
								} else {
									ivIcon.sprite = directorySprite;
								}
							} else {
								Debug.LogError ("ivIcon null: " + this);
							}
						}
						// tvName
						{
							if (tvName != null) {
								tvName.text = file.Name;
							} else {
								Debug.LogError ("tvName null: " + this);
							}
						}
						// tvTime
						{
							if (tvTime != null) {
								tvTime.text = "" + file.LastWriteTime;
							} else {
								Debug.LogError ("tvTime null: " + this);
							}
						}
						// tvSize
						{
							if (tvSize != null) {
								if (file is FileInfo) {
									tvSize.gameObject.SetActive (true);
									FileInfo fileInfo = file as FileInfo;
									tvSize.text = "" + fileInfo.Length;
								} else {
									tvSize.gameObject.SetActive (false);
								}
							} else {
								Debug.LogError ("tvSize null: " + this);
							}
						}
						// selectIndicator
						{
							if (selectIndicator != null) {
								// find
								bool isSelect = false;
								{
									FileSystemBrowserUI.UIData fileSystemBrowserUIData = this.data.findDataInParent<FileSystemBrowserUI.UIData> ();
									if (fileSystemBrowserUIData != null) {
										FileSystemBrowser fileSystemBrowser = fileSystemBrowserUIData.fileSystemBrowser.v.data;
										if (fileSystemBrowser != null) {
											Action action = fileSystemBrowser.action.v;
											if (action != null) {
												switch (action.getType ()) {
												case Action.Type.None:
													{
														ActionNone actionNone = fileSystemBrowser.action.v as ActionNone;
														foreach (FileSystemInfo selectFile in actionNone.selectFiles.vs) {
															if (selectFile == file || selectFile.FullName == file.FullName) {
																isSelect = true;
																break;
															}
														}
													}
													break;
												case Action.Type.Edit:
													{
														ActionEdit actionEdit = fileSystemBrowser.action.v as ActionEdit;
														foreach (FileSystemInfo selectFile in actionEdit.files.vs) {
															if (selectFile == file || selectFile.FullName == file.FullName) {
																isSelect = true;
																break;
															}
														}
													}
													break;
												default:
													Debug.LogError ("unknown type: " + action.getType () + "; " + this);
													break;
												}
											} else {
												Debug.LogError ("action null: " + this);
											}
										} else {
											Debug.LogError ("fileSystemBrowser null: " + this);
										}
									} else {
										Debug.LogError ("fileSystemBrowserUIData null: " + this);
									}
								}
								// process
								selectIndicator.SetActive(isSelect);
							} else {
								Debug.LogError ("selectIndicator null: " + this);
							}
						}
					} else {
						Debug.LogError ("file null: " + this);
					}
				} else {
					// Debug.LogError ("data null: " + this);
				}
			}
		}

		#endregion

		#region implement callBacks

		private FileSystemBrowserUI.UIData fileSystemBrowserUIData = null;

		public override void onAddCallBack<T> (T data)
		{
			if (data is UIData) {
				UIData uiData = data as UIData;
				// Parent
				{
					DataUtils.addParentCallBack (uiData, this, ref this.fileSystemBrowserUIData);
				}
				dirty = true;
				return;
			}
			// Parent
			{
				if (data is FileSystemBrowserUI.UIData) {
					FileSystemBrowserUI.UIData fileSystemBrowserUIData = data as FileSystemBrowserUI.UIData;
					// Child
					{
						fileSystemBrowserUIData.fileSystemBrowser.allAddCallBack (this);
					}
					dirty = true;
					return;
				}
				// Child
				{
					if (data is FileSystemBrowser) {
						FileSystemBrowser fileSystemBrowser = data as FileSystemBrowser;
						// Child
						{
							fileSystemBrowser.action.allAddCallBack (this);
						}
						dirty = true;
						return;
					}
					// Child
					if (data is Action) {
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
				// Parent
				{
					DataUtils.removeParentCallBack (uiData, this, ref this.fileSystemBrowserUIData);
				}
				this.setDataNull (uiData);
				return;
			}
			// Parent
			{
				if (data is FileSystemBrowserUI.UIData) {
					FileSystemBrowserUI.UIData fileSystemBrowserUIData = data as FileSystemBrowserUI.UIData;
					// Child
					{
						fileSystemBrowserUIData.fileSystemBrowser.allRemoveCallBack (this);
					}
					return;
				}
				// Child
				{
					if (data is FileSystemBrowser) {
						FileSystemBrowser fileSystemBrowser = data as FileSystemBrowser;
						// Child
						{
							fileSystemBrowser.action.allRemoveCallBack (this);
						}
						return;
					}
					// Child
					if (data is Action) {
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
				case UIData.Property.file:
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
				if (wrapProperty.p is FileSystemBrowserUI.UIData) {
					switch ((FileSystemBrowserUI.UIData.Property)wrapProperty.n) {
					case FileSystemBrowserUI.UIData.Property.fileSystemBrowser:
						{
							ValueChangeUtils.replaceCallBack (this, syncs);
							dirty = true;
						}
						break;
					case FileSystemBrowserUI.UIData.Property.actionUIData:
						break;
					case FileSystemBrowserUI.UIData.Property.showUIData:
						break;
					default:
						Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
						break;
					}
					return;
				}
				// Child
				{
					if (wrapProperty.p is FileSystemBrowser) {
						switch ((FileSystemBrowser.Property)wrapProperty.n) {
						case FileSystemBrowser.Property.action:
							{
								ValueChangeUtils.replaceCallBack (this, syncs);
								dirty = true;
							}
							break;
						case FileSystemBrowser.Property.show:
							break;
						default:
							Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
							break;
						}
						return;
					}
					// Child
					if (wrapProperty.p is Action) {
						Action action = wrapProperty.p as Action;
						switch (action.getType ()) {
						case Action.Type.None:
							{
								switch ((ActionNone.Property)wrapProperty.n) {
								case ActionNone.Property.state:
									break;
								case ActionNone.Property.selectFiles:
									dirty = true;
									break;
								default:
									Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
									break;
								}
							}
							break;
						case Action.Type.Edit:
							{
								switch ((ActionEdit.Property)wrapProperty.n) {
								case ActionEdit.Property.action:
									break;
								case ActionEdit.Property.state:
									break;
								case ActionEdit.Property.files:
									dirty = true;
									break;
								case ActionEdit.Property.destDir:
									break;
								default:
									Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
									break;
								}
							}
							break;
						default:
							Debug.LogError ("unknown type: " + action.getType () + "; " + this);
							break;
						}
						return;
					}
				}
			}
			Debug.LogError ("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
		}

		#endregion

		public void onClickFile()
		{
			if (this.data != null) {
				FileSystemInfo file = this.data.file.v;
				if (file != null) {
					if (file is DirectoryInfo) {
						DirectoryInfo directoryInfo = file as DirectoryInfo;
						// change directory
						{
							ShowDirectoryUI.UIData showDirectoryUIData = this.data.findDataInParent<ShowDirectoryUI.UIData> ();
							if (showDirectoryUIData != null) {
								ShowDirectory showDirectory = showDirectoryUIData.showDirectory.v.data;
								if (showDirectory != null) {
									showDirectory.directory.v = directoryInfo;
								} else {
									Debug.LogError ("showDirectory null: " + this);
								}
							} else {
								Debug.LogError ("showDirectoryUIData null: " + this);
							}
						}
					} else {
						FileSystemBrowserUI.UIData fileSystemBrowserUIData = this.data.findDataInParent<FileSystemBrowserUI.UIData> ();
						if (fileSystemBrowserUIData != null) {
							FileSystemBrowser fileSystemBrowser = fileSystemBrowserUIData.fileSystemBrowser.v.data;
							if (fileSystemBrowser != null) {
								fileSystemBrowser.selectFile (file, false, true);
							} else {
								Debug.LogError ("fileSystemBrowser null: " + this);
							}
						} else {
							Debug.LogError ("fileSystemBrowserUIData null: " + this);
						}
					}
				} else {
					Debug.LogError ("file null: " + this);
				}
			} else {
				Debug.LogError ("data null: " + this);
			}
		}

	}
}