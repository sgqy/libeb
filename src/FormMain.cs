//
// Copyright (c) 2004  Motoyuki Kasahara
// 
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2, or (at your option)
// any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Threading;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;

namespace WinEBZip {
	/// <summary>
	/// Form1 �̊T�v�̐����ł��B
	/// </summary>
	public class FormMain : System.Windows.Forms.Form {
		private System.Windows.Forms.ImageList imageList;
		private System.Windows.Forms.MainMenu mainMenu;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ToolBar toolBar;
		private System.Windows.Forms.MenuItem menuItemSeparatorView1;
		private System.Windows.Forms.MenuItem menuItemAbout;
		private System.Windows.Forms.ListView listViewFileList;
		private System.Windows.Forms.Label labelCurrentSize;
		private System.Windows.Forms.MenuItem menuItemUnzip;
		private System.Windows.Forms.MenuItem menuItemToolBar;
		private System.Windows.Forms.MenuItem menuItemFile;
		private System.Windows.Forms.MenuItem menuItemOpen;
		private System.Windows.Forms.MenuItem menuItemSeparatorFile1;
		private System.Windows.Forms.MenuItem menuItemZip;
		private System.Windows.Forms.MenuItem menuItemSeparatorFile2;
		private System.Windows.Forms.MenuItem menuItemExit;
		private System.Windows.Forms.MenuItem menuItemView;
		private System.Windows.Forms.MenuItem menuItemMaximize;
		private System.Windows.Forms.MenuItem menuItemHelp;
		private System.Windows.Forms.ColumnHeader columnHeaderFileName;
		private System.Windows.Forms.ColumnHeader columnHeaderType;
		private System.Windows.Forms.ColumnHeader columnHeaderUnzipSize;
		private System.Windows.Forms.ColumnHeader columnHeaderCurrentSize;
		private System.Windows.Forms.ColumnHeader columnHeaderZipRatio;
		private System.Windows.Forms.ColumnHeader columnHeaderZipMethod;
		private System.Windows.Forms.Panel panelPath;
		private System.Windows.Forms.Button buttonSourcePath;
		private System.Windows.Forms.TextBox textBoxSourcePath;
		private System.Windows.Forms.Label labelSourcePath;
		private System.Windows.Forms.ToolBarButton toolBarButtonZip;
		private System.Windows.Forms.ToolBarButton toolBarButtonUnzip;
		private System.Windows.Forms.Panel panelTotal;
		private System.Windows.Forms.Label labelZipRatio2;
		private System.Windows.Forms.Label labelUnzipSize2;
		private System.Windows.Forms.Label labelCurrentSize2;
		private System.Windows.Forms.Label labelZipRatio;
		private System.Windows.Forms.Label labelUnzipSize;

		// ���Г��̕��{�ꗗ
		private EBSubbookCollection subbooks;
		private System.Windows.Forms.MenuItem menuItemDocument;

		// ���Г��̃t�@�C���ꗗ
		private EBZipFileCollection infoFiles;

		// EB �R�}���h�̃p�X
		private string ebCommandPath;

		// �h�L�������g�t�@�C����
		private const string DocumentFileName = "WinEBZip.html";
		private System.Windows.Forms.MenuItem menuItemEBCommandPath;
		private System.Windows.Forms.MenuItem menuItemSeparatorfile3;

		// �h�L�������g�̃p�X
		private string documentPath;

		/// <summary>
		/// �f�t�H���g�̃R���X�g���N�^
		/// </summary>
		public FormMain() {
			//
			// Windows �t�H�[�� �f�U�C�i �T�|�[�g�ɕK�v�ł��B
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent �Ăяo���̌�ɁA�R���X�g���N�^ �R�[�h��ǉ����Ă��������B
			//
			this.infoFiles = null;
			this.subbooks  = null;

			WinEBZipRegistry registry = null;
			try {
				registry = new WinEBZipRegistry();
				
				this.ebCommandPath = registry.EBCommandPath;
				this.documentPath  = registry.DocumentPath;

				if (registry.ShowToolBar) {
					this.toolBar.Show();
					this.menuItemToolBar.Checked = true;
				}
				else {
					this.toolBar.Hide();
					this.menuItemToolBar.Checked = false;
				}

				if (registry.Maximized)
					this.WindowState = FormWindowState.Maximized;
				else
					this.WindowState = FormWindowState.Normal;
			}
			finally {
				if (registry != null)
					registry.Close();
			}
		}

		/// <summary>
		/// �g�p����Ă��郊�\�[�X�Ɍ㏈�������s���܂��B
		/// </summary>
		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows �t�H�[�� �f�U�C�i�Ő������ꂽ�R�[�h 
		/// <summary>
		/// �f�U�C�i �T�|�[�g�ɕK�v�ȃ��\�b�h�ł��B���̃��\�b�h�̓��e��
		/// �R�[�h �G�f�B�^�ŕύX���Ȃ��ł��������B
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FormMain));
			this.mainMenu = new System.Windows.Forms.MainMenu();
			this.menuItemFile = new System.Windows.Forms.MenuItem();
			this.menuItemOpen = new System.Windows.Forms.MenuItem();
			this.menuItemSeparatorFile1 = new System.Windows.Forms.MenuItem();
			this.menuItemZip = new System.Windows.Forms.MenuItem();
			this.menuItemUnzip = new System.Windows.Forms.MenuItem();
			this.menuItemSeparatorFile2 = new System.Windows.Forms.MenuItem();
			this.menuItemEBCommandPath = new System.Windows.Forms.MenuItem();
			this.menuItemSeparatorfile3 = new System.Windows.Forms.MenuItem();
			this.menuItemExit = new System.Windows.Forms.MenuItem();
			this.menuItemView = new System.Windows.Forms.MenuItem();
			this.menuItemToolBar = new System.Windows.Forms.MenuItem();
			this.menuItemSeparatorView1 = new System.Windows.Forms.MenuItem();
			this.menuItemMaximize = new System.Windows.Forms.MenuItem();
			this.menuItemHelp = new System.Windows.Forms.MenuItem();
			this.menuItemDocument = new System.Windows.Forms.MenuItem();
			this.menuItemAbout = new System.Windows.Forms.MenuItem();
			this.listViewFileList = new System.Windows.Forms.ListView();
			this.columnHeaderFileName = new System.Windows.Forms.ColumnHeader();
			this.columnHeaderType = new System.Windows.Forms.ColumnHeader();
			this.columnHeaderUnzipSize = new System.Windows.Forms.ColumnHeader();
			this.columnHeaderCurrentSize = new System.Windows.Forms.ColumnHeader();
			this.columnHeaderZipRatio = new System.Windows.Forms.ColumnHeader();
			this.columnHeaderZipMethod = new System.Windows.Forms.ColumnHeader();
			this.panelPath = new System.Windows.Forms.Panel();
			this.buttonSourcePath = new System.Windows.Forms.Button();
			this.textBoxSourcePath = new System.Windows.Forms.TextBox();
			this.labelSourcePath = new System.Windows.Forms.Label();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.toolBar = new System.Windows.Forms.ToolBar();
			this.toolBarButtonZip = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonUnzip = new System.Windows.Forms.ToolBarButton();
			this.panelTotal = new System.Windows.Forms.Panel();
			this.labelZipRatio2 = new System.Windows.Forms.Label();
			this.labelUnzipSize2 = new System.Windows.Forms.Label();
			this.labelCurrentSize2 = new System.Windows.Forms.Label();
			this.labelZipRatio = new System.Windows.Forms.Label();
			this.labelCurrentSize = new System.Windows.Forms.Label();
			this.labelUnzipSize = new System.Windows.Forms.Label();
			this.panelPath.SuspendLayout();
			this.panelTotal.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.menuItemFile,
																					 this.menuItemView,
																					 this.menuItemHelp});
			// 
			// menuItemFile
			// 
			this.menuItemFile.Index = 0;
			this.menuItemFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuItemOpen,
																						 this.menuItemSeparatorFile1,
																						 this.menuItemZip,
																						 this.menuItemUnzip,
																						 this.menuItemSeparatorFile2,
																						 this.menuItemEBCommandPath,
																						 this.menuItemSeparatorfile3,
																						 this.menuItemExit});
			this.menuItemFile.Text = "�t�@�C��(&F)";
			// 
			// menuItemOpen
			// 
			this.menuItemOpen.Index = 0;
			this.menuItemOpen.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
			this.menuItemOpen.Text = "���Ђ��J��(&O)...";
			this.menuItemOpen.Click += new System.EventHandler(this.ClickButtonSourcePath);
			// 
			// menuItemSeparatorFile1
			// 
			this.menuItemSeparatorFile1.Index = 1;
			this.menuItemSeparatorFile1.Text = "-";
			// 
			// menuItemZip
			// 
			this.menuItemZip.Enabled = false;
			this.menuItemZip.Index = 2;
			this.menuItemZip.Shortcut = System.Windows.Forms.Shortcut.CtrlZ;
			this.menuItemZip.Text = "���k(&Z)...";
			this.menuItemZip.Click += new System.EventHandler(this.ClickMenuItemZip);
			// 
			// menuItemUnzip
			// 
			this.menuItemUnzip.Enabled = false;
			this.menuItemUnzip.Index = 3;
			this.menuItemUnzip.Shortcut = System.Windows.Forms.Shortcut.CtrlU;
			this.menuItemUnzip.Text = "�L��(&U)...";
			this.menuItemUnzip.Click += new System.EventHandler(this.ClickMenuItemUnzip);
			// 
			// menuItemSeparatorFile2
			// 
			this.menuItemSeparatorFile2.Index = 4;
			this.menuItemSeparatorFile2.Text = "-";
			// 
			// menuItemEBCommandPath
			// 
			this.menuItemEBCommandPath.Index = 5;
			this.menuItemEBCommandPath.Text = "EB���C�u�����̃p�X(&E)...";
			this.menuItemEBCommandPath.Click += new System.EventHandler(this.ClickMenuItemEBCommandPath);
			// 
			// menuItemSeparatorfile3
			// 
			this.menuItemSeparatorfile3.Index = 6;
			this.menuItemSeparatorfile3.Text = "-";
			// 
			// menuItemExit
			// 
			this.menuItemExit.Index = 7;
			this.menuItemExit.Text = "�I��(&X)";
			this.menuItemExit.Click += new System.EventHandler(this.ClickMenuItemExit);
			// 
			// menuItemView
			// 
			this.menuItemView.Index = 1;
			this.menuItemView.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuItemToolBar,
																						 this.menuItemSeparatorView1,
																						 this.menuItemMaximize});
			this.menuItemView.Text = "�\��(&V)";
			// 
			// menuItemToolBar
			// 
			this.menuItemToolBar.Checked = true;
			this.menuItemToolBar.Index = 0;
			this.menuItemToolBar.Text = "�c�[���o�[(&T)";
			this.menuItemToolBar.Click += new System.EventHandler(this.ClickMenuItemToolBar);
			// 
			// menuItemSeparatorView1
			// 
			this.menuItemSeparatorView1.Index = 1;
			this.menuItemSeparatorView1.Text = "-";
			// 
			// menuItemMaximize
			// 
			this.menuItemMaximize.Index = 2;
			this.menuItemMaximize.Text = "�S��ʕ\��";
			this.menuItemMaximize.Click += new System.EventHandler(this.ClickMenuItemMaximize);
			// 
			// menuItemHelp
			// 
			this.menuItemHelp.Index = 2;
			this.menuItemHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuItemDocument,
																						 this.menuItemAbout});
			this.menuItemHelp.Text = "�w���v(&H)";
			// 
			// menuItemDocument
			// 
			this.menuItemDocument.Index = 0;
			this.menuItemDocument.Text = "WinEBZip�̃h�L�������g(&D)...";
			this.menuItemDocument.Click += new System.EventHandler(this.ClickMenuItemDocument);
			// 
			// menuItemAbout
			// 
			this.menuItemAbout.Index = 1;
			this.menuItemAbout.Text = "WinEBZip�̃o�[�W�������(&A)...";
			this.menuItemAbout.Click += new System.EventHandler(this.ClickMenuItemAbout);
			// 
			// listViewFileList
			// 
			this.listViewFileList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							   this.columnHeaderFileName,
																							   this.columnHeaderType,
																							   this.columnHeaderUnzipSize,
																							   this.columnHeaderCurrentSize,
																							   this.columnHeaderZipRatio,
																							   this.columnHeaderZipMethod});
			this.listViewFileList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listViewFileList.Location = new System.Drawing.Point(0, 70);
			this.listViewFileList.Name = "listViewFileList";
			this.listViewFileList.Size = new System.Drawing.Size(584, 199);
			this.listViewFileList.TabIndex = 3;
			this.listViewFileList.TabStop = false;
			this.listViewFileList.View = System.Windows.Forms.View.Details;
			// 
			// columnHeaderFileName
			// 
			this.columnHeaderFileName.Text = "�t�@�C����";
			this.columnHeaderFileName.Width = 180;
			// 
			// columnHeaderType
			// 
			this.columnHeaderType.Text = "���";
			// 
			// columnHeaderUnzipSize
			// 
			this.columnHeaderUnzipSize.Text = "�񈳏k�T�C�Y";
			this.columnHeaderUnzipSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.columnHeaderUnzipSize.Width = 80;
			// 
			// columnHeaderCurrentSize
			// 
			this.columnHeaderCurrentSize.Text = "���݂̃T�C�Y";
			this.columnHeaderCurrentSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.columnHeaderCurrentSize.Width = 80;
			// 
			// columnHeaderZipRatio
			// 
			this.columnHeaderZipRatio.Text = "���k��";
			this.columnHeaderZipRatio.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.columnHeaderZipRatio.Width = 50;
			// 
			// columnHeaderZipMethod
			// 
			this.columnHeaderZipMethod.Text = "���k����";
			this.columnHeaderZipMethod.Width = 104;
			// 
			// panelPath
			// 
			this.panelPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelPath.Controls.Add(this.buttonSourcePath);
			this.panelPath.Controls.Add(this.textBoxSourcePath);
			this.panelPath.Controls.Add(this.labelSourcePath);
			this.panelPath.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelPath.Location = new System.Drawing.Point(0, 38);
			this.panelPath.Name = "panelPath";
			this.panelPath.Size = new System.Drawing.Size(584, 32);
			this.panelPath.TabIndex = 6;
			// 
			// buttonSourcePath
			// 
			this.buttonSourcePath.Location = new System.Drawing.Point(408, 4);
			this.buttonSourcePath.Name = "buttonSourcePath";
			this.buttonSourcePath.Size = new System.Drawing.Size(56, 24);
			this.buttonSourcePath.TabIndex = 2;
			this.buttonSourcePath.Text = "�Q��...";
			this.buttonSourcePath.Click += new System.EventHandler(this.ClickButtonSourcePath);
			// 
			// textBoxSourcePath
			// 
			this.textBoxSourcePath.Location = new System.Drawing.Point(80, 8);
			this.textBoxSourcePath.Name = "textBoxSourcePath";
			this.textBoxSourcePath.Size = new System.Drawing.Size(320, 19);
			this.textBoxSourcePath.TabIndex = 1;
			this.textBoxSourcePath.Text = "";
			this.textBoxSourcePath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDownTextBoxSourcePath);
			// 
			// labelSourcePath
			// 
			this.labelSourcePath.Location = new System.Drawing.Point(0, 8);
			this.labelSourcePath.Name = "labelSourcePath";
			this.labelSourcePath.Size = new System.Drawing.Size(72, 16);
			this.labelSourcePath.TabIndex = 0;
			this.labelSourcePath.Text = "���Ђ̃p�X�F";
			// 
			// imageList
			// 
			this.imageList.ImageSize = new System.Drawing.Size(24, 24);
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// folderBrowserDialog
			// 
			this.folderBrowserDialog.Description = "���Ђ̃p�X����͂��Ă��������B";
			this.folderBrowserDialog.ShowNewFolderButton = false;
			// 
			// toolBar
			// 
			this.toolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																					   this.toolBarButtonZip,
																					   this.toolBarButtonUnzip});
			this.toolBar.ButtonSize = new System.Drawing.Size(32, 32);
			this.toolBar.DropDownArrows = true;
			this.toolBar.ImageList = this.imageList;
			this.toolBar.Location = new System.Drawing.Point(0, 0);
			this.toolBar.Name = "toolBar";
			this.toolBar.ShowToolTips = true;
			this.toolBar.Size = new System.Drawing.Size(584, 38);
			this.toolBar.TabIndex = 0;
			this.toolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.ButtonClickToolBar);
			// 
			// toolBarButtonZip
			// 
			this.toolBarButtonZip.Enabled = false;
			this.toolBarButtonZip.ImageIndex = 0;
			this.toolBarButtonZip.ToolTipText = "���k...";
			// 
			// toolBarButtonUnzip
			// 
			this.toolBarButtonUnzip.Enabled = false;
			this.toolBarButtonUnzip.ImageIndex = 1;
			this.toolBarButtonUnzip.ToolTipText = "�L��...";
			// 
			// panelTotal
			// 
			this.panelTotal.Controls.Add(this.labelZipRatio2);
			this.panelTotal.Controls.Add(this.labelUnzipSize2);
			this.panelTotal.Controls.Add(this.labelCurrentSize2);
			this.panelTotal.Controls.Add(this.labelZipRatio);
			this.panelTotal.Controls.Add(this.labelCurrentSize);
			this.panelTotal.Controls.Add(this.labelUnzipSize);
			this.panelTotal.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panelTotal.Location = new System.Drawing.Point(0, 269);
			this.panelTotal.Name = "panelTotal";
			this.panelTotal.Size = new System.Drawing.Size(584, 64);
			this.panelTotal.TabIndex = 7;
			// 
			// labelZipRatio2
			// 
			this.labelZipRatio2.Location = new System.Drawing.Point(304, 40);
			this.labelZipRatio2.Name = "labelZipRatio2";
			this.labelZipRatio2.Size = new System.Drawing.Size(40, 16);
			this.labelZipRatio2.TabIndex = 11;
			this.labelZipRatio2.Text = "00.0%";
			// 
			// labelUnzipSize2
			// 
			this.labelUnzipSize2.Location = new System.Drawing.Point(304, 24);
			this.labelUnzipSize2.Name = "labelUnzipSize2";
			this.labelUnzipSize2.Size = new System.Drawing.Size(96, 16);
			this.labelUnzipSize2.TabIndex = 10;
			this.labelUnzipSize2.Text = "0";
			// 
			// labelCurrentSize2
			// 
			this.labelCurrentSize2.Location = new System.Drawing.Point(304, 8);
			this.labelCurrentSize2.Name = "labelCurrentSize2";
			this.labelCurrentSize2.Size = new System.Drawing.Size(96, 16);
			this.labelCurrentSize2.TabIndex = 9;
			this.labelCurrentSize2.Text = "0";
			// 
			// labelZipRatio
			// 
			this.labelZipRatio.Location = new System.Drawing.Point(176, 40);
			this.labelZipRatio.Name = "labelZipRatio";
			this.labelZipRatio.Size = new System.Drawing.Size(120, 16);
			this.labelZipRatio.TabIndex = 4;
			this.labelZipRatio.Text = "���k�� (����)�F";
			// 
			// labelCurrentSize
			// 
			this.labelCurrentSize.Location = new System.Drawing.Point(176, 8);
			this.labelCurrentSize.Name = "labelCurrentSize";
			this.labelCurrentSize.Size = new System.Drawing.Size(120, 16);
			this.labelCurrentSize.TabIndex = 2;
			this.labelCurrentSize.Text = "���݂̃T�C�Y (���v)�F";
			// 
			// labelUnzipSize
			// 
			this.labelUnzipSize.Location = new System.Drawing.Point(176, 24);
			this.labelUnzipSize.Name = "labelUnzipSize";
			this.labelUnzipSize.Size = new System.Drawing.Size(120, 16);
			this.labelUnzipSize.TabIndex = 0;
			this.labelUnzipSize.Text = "�񈳏k�T�C�Y (���v)�F";
			// 
			// FormMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(584, 333);
			this.Controls.Add(this.listViewFileList);
			this.Controls.Add(this.panelPath);
			this.Controls.Add(this.toolBar);
			this.Controls.Add(this.panelTotal);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mainMenu;
			this.Name = "FormMain";
			this.Text = "WinEBZip";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.ClosingForm);
			this.Load += new System.EventHandler(this.LoadForm);
			this.panelPath.ResumeLayout(false);
			this.panelTotal.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// �A�v���P�[�V�����̃��C�� �G���g�� �|�C���g�ł��B
		/// </summary>
		[STAThread]
		static void Main() {
			Application.Run(new FormMain());
		}

		/// <summary>
		/// �t�H�[�������[�h�����Ƃ��̏���
		/// </summary>
		private void LoadForm(object sender, System.EventArgs e) {
			while (!this.CheckEBCommandPath()) {
				this.folderBrowserDialog.SelectedPath = this.ebCommandPath;
				this.folderBrowserDialog.Description 
					= "EB ���C�u�����̃p�X����͂��ĉ������B";
				DialogResult result = this.folderBrowserDialog.ShowDialog();
				if (result == DialogResult.Cancel) {
					this.Close();
					return;
				}

				this.ebCommandPath = this.folderBrowserDialog.SelectedPath;
				if (!this.ebCommandPath.EndsWith("\\bin")) {
					this.ebCommandPath += "\\bin";
				}
			}
		}

		/// <summary>
		/// �c�[���o�[���̃{�^�����N���b�N���ꂽ�Ƃ��̏���
		/// </summary>
		private void ButtonClickToolBar(object sender,
			System.Windows.Forms.ToolBarButtonClickEventArgs e) {
			if (e.Button == this.toolBarButtonZip)
				this.menuItemZip.PerformClick();
			else if (e.Button == this.toolBarButtonUnzip)
				this.menuItemUnzip.PerformClick();
		}

		/// <summary>
		/// ���Ђ̃p�X�́u�Q��...�v�{�^���������ꂽ�Ƃ��̏���
		/// </summary>
		private void ClickButtonSourcePath(object sender, System.EventArgs e) {
			this.folderBrowserDialog.SelectedPath = this.textBoxSourcePath.Text;
			this.folderBrowserDialog.Description = "���Ђ̃p�X����͂��ĉ������B";
			DialogResult result = this.folderBrowserDialog.ShowDialog();
			if (result == DialogResult.OK) {
				this.textBoxSourcePath.Text = this.folderBrowserDialog.SelectedPath;
				this.Refresh();
				this.UpdateFileList();
			}
		}

		/// <summary>
		/// ���j���[�o�[�́uEB�R�}���h�̃p�X...�v���I�����ꂽ�Ƃ��̏���
		/// </summary>
		private void ClickMenuItemEBCommandPath(object sender, System.EventArgs e) {
			do {
				this.folderBrowserDialog.SelectedPath = this.ebCommandPath;
				this.folderBrowserDialog.Description 
					= "EB ���C�u�����̃p�X����͂��ĉ������B";
				DialogResult result = this.folderBrowserDialog.ShowDialog();
				if (result == DialogResult.Cancel) {
					return;
				}

				this.ebCommandPath = this.folderBrowserDialog.SelectedPath;
				if (!this.ebCommandPath.EndsWith("\\bin")) {
					this.ebCommandPath += "\\bin";
				}
			} while (!this.CheckEBCommandPath());
		}

		/// <summary>
		/// ���j���[�o�[���ɂ���A�c�[���o�[�\���ؑւ��I�����ꂽ�Ƃ��̏���
		/// </summary>
		private void ClickMenuItemToolBar(object sender, System.EventArgs e) {
			if (this.toolBar.Visible) {
				this.toolBar.Hide();
				this.menuItemToolBar.Checked = false;
			}
			else {
				this.toolBar.Show();
				this.menuItemToolBar.Checked = true;
			}
		}

		/// <summary>
		/// ���j���[�o�[�́u���k...�v���I�����ꂽ�Ƃ��̏���
		/// </summary>
		private void ClickMenuItemZip(object sender, System.EventArgs e) {
			this.ZipOrUnzip(EBZipBookOperation.Zip);
		}

		/// <summary>
		/// ���j���[�o�[�́u�L��...�v���I�����ꂽ�Ƃ��̏���
		/// </summary>
		private void ClickMenuItemUnzip(object sender, System.EventArgs e) {
			this.ZipOrUnzip(EBZipBookOperation.Unzip);
		}

		/// <summary>
		/// ���k�A�L�������s����
		/// </summary>
		private void ZipOrUnzip(EBZipBookOperation operation) {
			// FormZipConfig �t�H�[����\������B
			FormZipConfig formZipConfig = new FormZipConfig(operation);
			formZipConfig.EBZipPath       = this.ebCommandPath + "\\ebzip.exe";
			formZipConfig.EBRefilePath    = this.ebCommandPath + "\\ebrefile.exe";
			formZipConfig.SourcePath      = this.textBoxSourcePath.Text;
			formZipConfig.DestinationPath = this.textBoxSourcePath.Text;
			formZipConfig.LoadAuxInfo(this.subbooks, this.infoFiles);

			DialogResult result = formZipConfig.ShowDialog();
			if (result != DialogResult.OK)
				return;

			FormZipProgress progress =
				new FormZipProgress(formZipConfig.EBZip, formZipConfig.EBRefile,
					formZipConfig.SelectedUnzipSize);
			progress.Execute();
			progress.ShowDialog();

			this.ClearFileList();
			if (progress.DialogResult == DialogResult.OK)
				this.textBoxSourcePath.Text = formZipConfig.EBZip.DestinationPath;
			else
				this.textBoxSourcePath.Text = formZipConfig.EBZip.SourcePath;
			this.Refresh();
			this.UpdateFileList();
		}

		/// <summary>
		/// ���j���[�o�[���́u�I���v���I�����ꂽ�Ƃ��̏���
		/// </summary>
		private void ClickMenuItemExit(object sender, System.EventArgs e) {
			this.Close();
		}

		/// <summary>
		/// ���j���[�o�[���́u�S��ʕ\���v���I�����ꂽ�Ƃ��̏���
		/// </summary>
		private void ClickMenuItemMaximize(object sender, System.EventArgs e) {
			if (this.WindowState == FormWindowState.Maximized)
				this.WindowState = FormWindowState.Normal;
			else
				this.WindowState = FormWindowState.Maximized;
		}

		/// <summary>
		/// �c�[���o�[���́uWinEBZip �ɂ���...�v���I�����ꂽ�Ƃ��̏���
		/// </summary>
		private void ClickMenuItemAbout(object sender, System.EventArgs e) {
			FormVersion formVersion = new FormVersion();
			formVersion.ShowDialog();
		}

		/// <summary>
		/// �c�[���o�[���́uWinEBZip �̃h�L�������g...�v���I�����ꂽ�Ƃ��̏���
		/// </summary>
		private void ClickMenuItemDocument(object sender, System.EventArgs e) {
			Process process = new Process();
			process.StartInfo.FileName  = this.documentPath + "\\" + DocumentFileName;
			process.StartInfo.Arguments = "";
			process.StartInfo.CreateNoWindow         = true;
			process.StartInfo.UseShellExecute        = true;
			try {
				process.Start();
			}
			catch {
				// ��O�͖�������B
			}
		}

		/// <summary>
		/// ���Ђ̃p�X�̓��͗����ŃL�[�������ꂽ�Ƃ��̏���
		/// </summary>
		private void KeyDownTextBoxSourcePath(object sender,
			System.Windows.Forms.KeyEventArgs e) {
			if (e.KeyCode == Keys.Enter && !this.textBoxSourcePath.Text.Equals("")) {
				this.Refresh();
				this.UpdateFileList();
			}
		}

		/// <summary>
		/// �t�H�[���������悤�Ƃ��Ă���Ƃ��̏���
		/// </summary>
		private void ClosingForm(object sender, System.ComponentModel.CancelEventArgs e) {
			WinEBZipRegistry registry = null;
			try {
				registry = new WinEBZipRegistry();
				registry.EBCommandPath = this.ebCommandPath;
				registry.ShowToolBar = this.toolBar.Visible;
				registry.Maximized
					= (this.WindowState == FormWindowState.Maximized);
			}
			finally {
				if (registry != null)
					registry.Close();
			}
		}

		/// <summary>
		/// EB �R�}���h���{���ɂ��邩�ǂ����̃`�F�b�N
		/// </summary>
		/// <returns></returns>
		private bool CheckEBCommandPath() {
			if (this.ebCommandPath == "")
				return false;

			string ebzipPath    = this.ebCommandPath + "\\ebzip.exe";
			string ebinfoPath   = this.ebCommandPath + "\\ebinfo.exe";
			string ebrefilePath = this.ebCommandPath + "\\ebrefile.exe";

			if (!File.Exists(ebzipPath)) {
				MessageBox.Show("�R�}���h��������܂���B\n" + ebzipPath,
					"WinEBZip (�G���[)", MessageBoxButtons.OK, MessageBoxIcon.Error,
					MessageBoxDefaultButton.Button1);
				return false;
			}
			if (!File.Exists(ebinfoPath)) {
				MessageBox.Show("�R�}���h��������܂���B\n" + ebinfoPath,
					"WinEBZip (�G���[)", MessageBoxButtons.OK, MessageBoxIcon.Error,
					MessageBoxDefaultButton.Button1);
				return false;
			}
			if (!File.Exists(ebrefilePath)) {
				MessageBox.Show("�R�}���h��������܂���B\n" + ebrefilePath,
					"WinEBZip (�G���[)", MessageBoxButtons.OK, MessageBoxIcon.Error,
					MessageBoxDefaultButton.Button1);
				return false;
			}

			return true;
		}

		/// <summary>
		/// �t�@�C���ꗗ����������
		/// </summary>
		private void ClearFileList() {
			// ���k�L���{�^���𖳌��ɂ���B
			this.menuItemZip.Enabled = false;
			this.menuItemUnzip.Enabled = false;
			this.toolBarButtonZip.Enabled = false;
			this.toolBarButtonUnzip.Enabled = false;

			// �E�B���h�E�^�C�g�����珑�Ђ̃p�X���O���B
			this.Text = "WinEBZip";

			// ListView ���N���A����B
			this.listViewFileList.Items.Clear();
		}

		/// <summary>
		/// �t�@�C���ꗗ���X�V����
		/// </summary>
		private void UpdateFileList() {
			// �t�@�C���ꗗ����������
			this.ClearFileList();

			// ListView ���X�V����
			if (!this.GetSubbooks())
				return;
			if (!this.GetInfoFiles())
				return;

			string lastSubbookDirectory = null;
			string subbookDirectory = null;
			int subbookIndex = 0;

			foreach (EBZipFile file in this.infoFiles) {
				int backslashIndex = file.Name.IndexOf('\\');
				if (backslashIndex >= 0)
					subbookDirectory = file.Name.Substring(0, backslashIndex + 1);
				else
					subbookDirectory = "";
				if (lastSubbookDirectory == null
					|| subbookDirectory != lastSubbookDirectory) {
					string title;
					if (subbookDirectory == "")
						title = "���̑�";
					else
						title = this.subbooks[subbookIndex++].Title;
					ListViewItem titleItem = new ListViewItem(title);
					titleItem.ForeColor = System.Drawing.Color.Blue;
					this.listViewFileList.Items.Add(titleItem);
				}

				ListViewItem item = new ListViewItem(file.Name);
				item.SubItems.Add(file.Category.ToString());
				item.SubItems.Add(file.UnzipSize.ToString("n0"));
				item.SubItems.Add(file.CurrentSize.ToString("n0"));
				item.SubItems.Add(file.ZipRatio().ToString("p1"));
				item.SubItems.Add(file.ZipMethod);
				this.listViewFileList.Items.Add(item);

				lastSubbookDirectory = subbookDirectory;
			}
			this.Refresh();

			// ���v�T�C�Y�A���ψ��k�����v�Z����B
			uint totalUnzipSize   = 0;
			uint totalCurrentSize = 0;
			foreach (EBZipFile file in this.infoFiles) {
				totalUnzipSize   += file.UnzipSize;
				totalCurrentSize += file.CurrentSize;
			}
			this.labelUnzipSize2.Text   = totalUnzipSize.ToString("n0");
			this.labelCurrentSize2.Text = totalCurrentSize.ToString("n0");
			this.labelZipRatio2.Text    =
				((double)totalCurrentSize / (double)totalUnzipSize).ToString("p1");

			// ���k�E�L���{�^����L���ɂ���B
			this.menuItemZip.Enabled = true;
			this.menuItemUnzip.Enabled = true;
			this.toolBarButtonZip.Enabled = true;
			this.toolBarButtonUnzip.Enabled = true;

			// �E�B���h�E�^�C�g���ɏ��Ђ̃p�X������B
			this.Text = string.Format("WinEBZip - {0}", this.textBoxSourcePath.Text);
		}

		/// <summary>
		/// "ebinfo.exe" �����s���āA���{�̈ꗗ���擾����B
		/// </summary>
		/// <returns></returns>
		private bool GetSubbooks() {
			EBInfo ebinfo = new EBInfo(this.ebCommandPath + "\\ebinfo.exe");
			DialogResult dialogResult;
			EBSubbookCollection subbooks = null;

			do {
				dialogResult = DialogResult.None;
				try {
					ebinfo.BookPath = this.textBoxSourcePath.Text;
					ebinfo.Execute();
					subbooks = ebinfo.Subbooks;
				}
				catch (EBCommandException e) {
					dialogResult = MessageBox.Show(e.Message,
						"WinEBZip (ebinfo ���s�G���[)", MessageBoxButtons.RetryCancel,
						MessageBoxIcon.Error, MessageBoxDefaultButton.Button2);
					if (dialogResult == DialogResult.Cancel)
						return false;
				}
			} while (dialogResult == DialogResult.Retry);

			if (subbooks.Count == 0) {
				MessageBox.Show("���{��������܂���B", "WinEBZip (ebinfo �֘A�G���[)",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			this.subbooks = subbooks;
			return true;
		}

		/// <summary>
		/// "ebzip.exe --information" �����s���āA���Г��̃t�@�C���̈��k����
		/// �擾����B
		/// </summary>
		/// <returns></returns>
		private bool GetInfoFiles() {
			EBZipInfo ebzipinfo = new EBZipInfo(this.ebCommandPath + "\\ebzip.exe");
			DialogResult dialogResult;
			EBZipFileCollection files = null;

			do {
				dialogResult = DialogResult.None;
				try {
					ebzipinfo.BookPath = this.textBoxSourcePath.Text;
					ebzipinfo.Execute();
					files = ebzipinfo.Files;
					this.textBoxSourcePath.Text = ebzipinfo.BookPath;
				}
				catch (EBCommandException e) {
					dialogResult = MessageBox.Show(e.Message,
						"ebzip.exe���s�G���[", MessageBoxButtons.RetryCancel,
						MessageBoxIcon.Error, MessageBoxDefaultButton.Button2);
					if (dialogResult == DialogResult.Cancel)
						return false;
				}
			} while (dialogResult == DialogResult.Retry);

			if (files.Count == 0) {
				MessageBox.Show("���Ђ̍\���t�@�C����������܂���B",
					"WinEBZip (ebzip �֘A�G���[)", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			this.infoFiles = files;
			return true;
		}
	}
}

// Local Variables:
// tab-width: 4
