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

namespace WinEBZip {
	public struct ZipSizeTable {
		public bool Selected;

		public uint CurrentRequiredSize;
		public uint UnzipRequiredSize;

		public uint CurrentFontSize;
		public uint UnzipFontSize;

		public uint CurrentGraphicSize;
		public uint UnzipGraphicSize;

		public uint CurrentSoundSize;
		public uint UnzipSoundSize;

		public uint CurrentMovieSize;
		public uint UnzipMovieSize;

		public ZipSizeTable(bool selectedArg) {
			this.Selected = selectedArg;
			this.CurrentRequiredSize = 0;
			this.UnzipRequiredSize = 0;
			this.CurrentFontSize = 0;
			this.UnzipFontSize = 0;
			this.CurrentGraphicSize = 0;
			this.UnzipGraphicSize = 0;
			this.CurrentSoundSize = 0;
			this.UnzipSoundSize = 0;
			this.CurrentMovieSize = 0;
			this.UnzipMovieSize = 0;
		}
	}

	/// <summary>
	/// FormZipConfig �̊T�v�̐����ł��B
	/// </summary>
	public class FormZipConfig : System.Windows.Forms.Form {
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.TabPage tabPageBasic;
		private System.Windows.Forms.GroupBox groupBoxZipLevel;
		private System.Windows.Forms.Label labelZipLevel;
		private System.Windows.Forms.Label labelFastBest;
		private System.Windows.Forms.TrackBar trackBarZipLevel;
		private System.Windows.Forms.GroupBox groupBoxDestinationPath;
		private System.Windows.Forms.Button buttonDestinationPath;
		private System.Windows.Forms.TextBox textBoxDestinationPath;
		private System.Windows.Forms.TabPage tabPageSubbooks;
		private System.Windows.Forms.GroupBox groupBoxSubbooks;
		private System.Windows.Forms.CheckedListBox checkedListBoxSubbooks;
		private System.Windows.Forms.TabPage tabPageFileTypes;
		private System.Windows.Forms.GroupBox groupBoxCategory;
		private System.Windows.Forms.Label labelOtherCategories;
		private System.Windows.Forms.CheckBox checkBoxCategorySound;
		private System.Windows.Forms.CheckBox checkBoxCategoryGraphic;
		private System.Windows.Forms.CheckBox checkBoxCategoryFont;
		private System.Windows.Forms.CheckBox checkBoxCategoryMovie;
		private System.Windows.Forms.TabPage tabPageOptions;
		private System.Windows.Forms.GroupBox groupBoxMiscOptions;
		private System.Windows.Forms.CheckBox checkBoxTest;
		private System.Windows.Forms.CheckBox checkBoxKeepSource;
		private System.Windows.Forms.GroupBox groupBoxOverwrite;
		private System.Windows.Forms.RadioButton radioButtonOverwriteForce;
		private System.Windows.Forms.RadioButton radioButtonOverwriteConfirm;
		private System.Windows.Forms.RadioButton radioButtonOverwriteSkip;
		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Label labelCurrentSize2;
		private System.Windows.Forms.Label labelCurrentSize;
		private System.Windows.Forms.Label labelUnzipSize2;
		private System.Windows.Forms.Label labelUnzipSize;
		private uint selectedUnzipSize;

		// ���Ђ̃p�X
		private string sourcePath;

		// ���{�̈ꗗ
		private EBSubbookCollection subbooks;

		// �e�t�@�C���J�e�S���A�e���{�ɊY������t�@�C���̍��v�T�C�Y�̕\
		private Hashtable zipSizeTables;

		// ���샂�[�h (���k�A�L���̂����ꂩ)
		private EBZipBookOperation operation;

		// �����ΏۂƂȂ����t�@�C���̌��݂̍��v�T�C�Y�A�񈳏k���̍��v�T�C�Y
		private uint selectedCurrentSize;

		// ebzip.exe �ւ̃p�X
		private string ebzipPath;

		// ebzip.exe �ւ̃p�X
		private string ebrefilePath;

		/// <summary>
		/// �f�t�H���g�̃R���X�g���N�^
		/// </summary>
		public FormZipConfig() {
			//
			// Windows �t�H�[�� �f�U�C�i �T�|�[�g�ɕK�v�ł��B
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent �Ăяo���̌�ɁA�R���X�g���N�^ �R�[�h��ǉ����Ă��������B
			//
			Initialize(EBZipBookOperation.Zip);
		}

		/// <summary>
		/// �R���X�g���N�^
		/// </summary>
		/// <param name="operationArg">�����̎�� (���k/�L��)</param>
		public FormZipConfig(EBZipBookOperation operationArg) {
			//
			// Windows �t�H�[�� �f�U�C�i �T�|�[�g�ɕK�v�ł��B
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent �Ăяo���̌�ɁA�R���X�g���N�^ �R�[�h��ǉ����Ă��������B
			//
			Initialize(operationArg);
		}

		/// <summary>
		/// �e�R���X�g���N�^���ʂ̏���������
		/// </summary>
		/// <param name="operationArg">������� (���k/�L��)</param>
		private void Initialize(EBZipBookOperation operationArg) {
			this.sourcePath          = ".";
			this.subbooks            = new EBSubbookCollection();
			this.zipSizeTables       = new Hashtable();
			this.zipSizeTables[""]   = new ZipSizeTable(true);
			this.selectedCurrentSize = 0;
			this.selectedUnzipSize   = 0;
			this.ebzipPath           = EBZip.DefaultCommandPath;
			this.ebrefilePath        = EBRefile.DefaultCommandPath;
			this.Operation           = operationArg;

			WinEBZipRegistry registry = null;
			try {
				registry = new WinEBZipRegistry();
				this.trackBarZipLevel.Value = registry.ZipLevel;
				this.checkBoxCategoryFont.Checked    = !registry.SkipFont;
				this.checkBoxCategoryGraphic.Checked = !registry.SkipGraphic;
				this.checkBoxCategorySound.Checked   = !registry.SkipSound;
				this.checkBoxCategoryMovie.Checked   = !registry.SkipMovie;

				string commandPath = registry.EBCommandPath;
				this.ebzipPath = commandPath + "\\" + "ebzip.exe";
				this.ebrefilePath = commandPath + "\\" + "ebrefile.exe";
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
				if(components != null) {
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FormZipConfig));
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabPageBasic = new System.Windows.Forms.TabPage();
			this.groupBoxZipLevel = new System.Windows.Forms.GroupBox();
			this.labelZipLevel = new System.Windows.Forms.Label();
			this.labelFastBest = new System.Windows.Forms.Label();
			this.trackBarZipLevel = new System.Windows.Forms.TrackBar();
			this.groupBoxDestinationPath = new System.Windows.Forms.GroupBox();
			this.buttonDestinationPath = new System.Windows.Forms.Button();
			this.textBoxDestinationPath = new System.Windows.Forms.TextBox();
			this.tabPageSubbooks = new System.Windows.Forms.TabPage();
			this.groupBoxSubbooks = new System.Windows.Forms.GroupBox();
			this.checkedListBoxSubbooks = new System.Windows.Forms.CheckedListBox();
			this.tabPageFileTypes = new System.Windows.Forms.TabPage();
			this.groupBoxCategory = new System.Windows.Forms.GroupBox();
			this.labelOtherCategories = new System.Windows.Forms.Label();
			this.checkBoxCategorySound = new System.Windows.Forms.CheckBox();
			this.checkBoxCategoryGraphic = new System.Windows.Forms.CheckBox();
			this.checkBoxCategoryFont = new System.Windows.Forms.CheckBox();
			this.checkBoxCategoryMovie = new System.Windows.Forms.CheckBox();
			this.tabPageOptions = new System.Windows.Forms.TabPage();
			this.groupBoxMiscOptions = new System.Windows.Forms.GroupBox();
			this.checkBoxTest = new System.Windows.Forms.CheckBox();
			this.checkBoxKeepSource = new System.Windows.Forms.CheckBox();
			this.groupBoxOverwrite = new System.Windows.Forms.GroupBox();
			this.radioButtonOverwriteForce = new System.Windows.Forms.RadioButton();
			this.radioButtonOverwriteConfirm = new System.Windows.Forms.RadioButton();
			this.radioButtonOverwriteSkip = new System.Windows.Forms.RadioButton();
			this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.buttonOk = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.labelCurrentSize2 = new System.Windows.Forms.Label();
			this.labelCurrentSize = new System.Windows.Forms.Label();
			this.labelUnzipSize2 = new System.Windows.Forms.Label();
			this.labelUnzipSize = new System.Windows.Forms.Label();
			this.tabControl.SuspendLayout();
			this.tabPageBasic.SuspendLayout();
			this.groupBoxZipLevel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackBarZipLevel)).BeginInit();
			this.groupBoxDestinationPath.SuspendLayout();
			this.tabPageSubbooks.SuspendLayout();
			this.groupBoxSubbooks.SuspendLayout();
			this.tabPageFileTypes.SuspendLayout();
			this.groupBoxCategory.SuspendLayout();
			this.tabPageOptions.SuspendLayout();
			this.groupBoxMiscOptions.SuspendLayout();
			this.groupBoxOverwrite.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl
			// 
			this.tabControl.Controls.Add(this.tabPageBasic);
			this.tabControl.Controls.Add(this.tabPageSubbooks);
			this.tabControl.Controls.Add(this.tabPageFileTypes);
			this.tabControl.Controls.Add(this.tabPageOptions);
			this.tabControl.Location = new System.Drawing.Point(8, 8);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(336, 216);
			this.tabControl.TabIndex = 0;
			// 
			// tabPageBasic
			// 
			this.tabPageBasic.Controls.Add(this.groupBoxZipLevel);
			this.tabPageBasic.Controls.Add(this.groupBoxDestinationPath);
			this.tabPageBasic.Location = new System.Drawing.Point(4, 21);
			this.tabPageBasic.Name = "tabPageBasic";
			this.tabPageBasic.Size = new System.Drawing.Size(328, 191);
			this.tabPageBasic.TabIndex = 0;
			this.tabPageBasic.Text = "��{�ݒ�";
			// 
			// groupBoxZipLevel
			// 
			this.groupBoxZipLevel.Controls.Add(this.labelZipLevel);
			this.groupBoxZipLevel.Controls.Add(this.labelFastBest);
			this.groupBoxZipLevel.Controls.Add(this.trackBarZipLevel);
			this.groupBoxZipLevel.Location = new System.Drawing.Point(8, 72);
			this.groupBoxZipLevel.Name = "groupBoxZipLevel";
			this.groupBoxZipLevel.Size = new System.Drawing.Size(312, 112);
			this.groupBoxZipLevel.TabIndex = 1;
			this.groupBoxZipLevel.TabStop = false;
			this.groupBoxZipLevel.Text = "���k���x��";
			// 
			// labelZipLevel
			// 
			this.labelZipLevel.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.labelZipLevel.Location = new System.Drawing.Point(16, 48);
			this.labelZipLevel.Name = "labelZipLevel";
			this.labelZipLevel.Size = new System.Drawing.Size(40, 32);
			this.labelZipLevel.TabIndex = 2;
			this.labelZipLevel.Text = "0";
			this.labelZipLevel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// labelFastBest
			// 
			this.labelFastBest.Location = new System.Drawing.Point(72, 16);
			this.labelFastBest.Name = "labelFastBest";
			this.labelFastBest.Size = new System.Drawing.Size(184, 16);
			this.labelFastBest.TabIndex = 0;
			this.labelFastBest.Text = "���������� �� �� ���k�����ǂ�";
			this.labelFastBest.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// trackBarZipLevel
			// 
			this.trackBarZipLevel.LargeChange = 1;
			this.trackBarZipLevel.Location = new System.Drawing.Point(72, 48);
			this.trackBarZipLevel.Maximum = 5;
			this.trackBarZipLevel.Name = "trackBarZipLevel";
			this.trackBarZipLevel.Size = new System.Drawing.Size(184, 56);
			this.trackBarZipLevel.TabIndex = 0;
			this.trackBarZipLevel.ValueChanged += new System.EventHandler(this.ValueChangedTrackBarZipLevel);
			// 
			// groupBoxDestinationPath
			// 
			this.groupBoxDestinationPath.Controls.Add(this.buttonDestinationPath);
			this.groupBoxDestinationPath.Controls.Add(this.textBoxDestinationPath);
			this.groupBoxDestinationPath.Location = new System.Drawing.Point(8, 8);
			this.groupBoxDestinationPath.Name = "groupBoxDestinationPath";
			this.groupBoxDestinationPath.Size = new System.Drawing.Size(312, 56);
			this.groupBoxDestinationPath.TabIndex = 0;
			this.groupBoxDestinationPath.TabStop = false;
			this.groupBoxDestinationPath.Text = "�������ݐ�";
			// 
			// buttonDestinationPath
			// 
			this.buttonDestinationPath.Location = new System.Drawing.Point(240, 24);
			this.buttonDestinationPath.Name = "buttonDestinationPath";
			this.buttonDestinationPath.Size = new System.Drawing.Size(64, 24);
			this.buttonDestinationPath.TabIndex = 1;
			this.buttonDestinationPath.Text = "�Q��...";
			this.buttonDestinationPath.Click += new System.EventHandler(this.ClickButtonDestinationPath);
			// 
			// textBoxDestinationPath
			// 
			this.textBoxDestinationPath.Location = new System.Drawing.Point(8, 24);
			this.textBoxDestinationPath.Name = "textBoxDestinationPath";
			this.textBoxDestinationPath.Size = new System.Drawing.Size(224, 19);
			this.textBoxDestinationPath.TabIndex = 0;
			this.textBoxDestinationPath.Text = ".";
			this.textBoxDestinationPath.TextChanged += new System.EventHandler(this.TextChangedTextBoxDestinationPath);
			// 
			// tabPageSubbooks
			// 
			this.tabPageSubbooks.Controls.Add(this.groupBoxSubbooks);
			this.tabPageSubbooks.Location = new System.Drawing.Point(4, 21);
			this.tabPageSubbooks.Name = "tabPageSubbooks";
			this.tabPageSubbooks.Size = new System.Drawing.Size(328, 191);
			this.tabPageSubbooks.TabIndex = 1;
			this.tabPageSubbooks.Text = "���{�̑I��";
			// 
			// groupBoxSubbooks
			// 
			this.groupBoxSubbooks.Controls.Add(this.checkedListBoxSubbooks);
			this.groupBoxSubbooks.Location = new System.Drawing.Point(8, 7);
			this.groupBoxSubbooks.Name = "groupBoxSubbooks";
			this.groupBoxSubbooks.Size = new System.Drawing.Size(312, 177);
			this.groupBoxSubbooks.TabIndex = 0;
			this.groupBoxSubbooks.TabStop = false;
			this.groupBoxSubbooks.Text = "�����Ώۂ̕��{";
			// 
			// checkedListBoxSubbooks
			// 
			this.checkedListBoxSubbooks.CheckOnClick = true;
			this.checkedListBoxSubbooks.Location = new System.Drawing.Point(8, 16);
			this.checkedListBoxSubbooks.Name = "checkedListBoxSubbooks";
			this.checkedListBoxSubbooks.Size = new System.Drawing.Size(296, 144);
			this.checkedListBoxSubbooks.TabIndex = 0;
			this.checkedListBoxSubbooks.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ItemCheckCheckedListBoxSubbooks);
			// 
			// tabPageFileTypes
			// 
			this.tabPageFileTypes.Controls.Add(this.groupBoxCategory);
			this.tabPageFileTypes.Location = new System.Drawing.Point(4, 21);
			this.tabPageFileTypes.Name = "tabPageFileTypes";
			this.tabPageFileTypes.Size = new System.Drawing.Size(328, 191);
			this.tabPageFileTypes.TabIndex = 2;
			this.tabPageFileTypes.Text = "�t�@�C���̑I��";
			// 
			// groupBoxCategory
			// 
			this.groupBoxCategory.Controls.Add(this.labelOtherCategories);
			this.groupBoxCategory.Controls.Add(this.checkBoxCategorySound);
			this.groupBoxCategory.Controls.Add(this.checkBoxCategoryGraphic);
			this.groupBoxCategory.Controls.Add(this.checkBoxCategoryFont);
			this.groupBoxCategory.Controls.Add(this.checkBoxCategoryMovie);
			this.groupBoxCategory.Location = new System.Drawing.Point(8, 8);
			this.groupBoxCategory.Name = "groupBoxCategory";
			this.groupBoxCategory.Size = new System.Drawing.Size(312, 152);
			this.groupBoxCategory.TabIndex = 0;
			this.groupBoxCategory.TabStop = false;
			this.groupBoxCategory.Text = "�����Ώۂ̃t�@�C��";
			// 
			// labelOtherCategories
			// 
			this.labelOtherCategories.Location = new System.Drawing.Point(8, 128);
			this.labelOtherCategories.Name = "labelOtherCategories";
			this.labelOtherCategories.Size = new System.Drawing.Size(288, 16);
			this.labelOtherCategories.TabIndex = 7;
			this.labelOtherCategories.Text = "�����̃t�@�C���́A�˂ɏ����̑ΏۂɂȂ�܂�";
			// 
			// checkBoxCategorySound
			// 
			this.checkBoxCategorySound.Checked = true;
			this.checkBoxCategorySound.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxCategorySound.Location = new System.Drawing.Point(8, 72);
			this.checkBoxCategorySound.Name = "checkBoxCategorySound";
			this.checkBoxCategorySound.Size = new System.Drawing.Size(104, 16);
			this.checkBoxCategorySound.TabIndex = 2;
			this.checkBoxCategorySound.Text = "�����t�@�C��";
			this.checkBoxCategorySound.CheckedChanged += new System.EventHandler(this.CheckChangedCheckBoxCategory);
			// 
			// checkBoxCategoryGraphic
			// 
			this.checkBoxCategoryGraphic.Checked = true;
			this.checkBoxCategoryGraphic.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxCategoryGraphic.Location = new System.Drawing.Point(8, 48);
			this.checkBoxCategoryGraphic.Name = "checkBoxCategoryGraphic";
			this.checkBoxCategoryGraphic.Size = new System.Drawing.Size(104, 16);
			this.checkBoxCategoryGraphic.TabIndex = 1;
			this.checkBoxCategoryGraphic.Text = "�摜�t�@�C��";
			this.checkBoxCategoryGraphic.CheckedChanged += new System.EventHandler(this.CheckChangedCheckBoxCategory);
			// 
			// checkBoxCategoryFont
			// 
			this.checkBoxCategoryFont.Checked = true;
			this.checkBoxCategoryFont.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxCategoryFont.Location = new System.Drawing.Point(8, 24);
			this.checkBoxCategoryFont.Name = "checkBoxCategoryFont";
			this.checkBoxCategoryFont.Size = new System.Drawing.Size(104, 16);
			this.checkBoxCategoryFont.TabIndex = 0;
			this.checkBoxCategoryFont.Text = "�O���t�@�C��";
			this.checkBoxCategoryFont.CheckedChanged += new System.EventHandler(this.CheckChangedCheckBoxCategory);
			// 
			// checkBoxCategoryMovie
			// 
			this.checkBoxCategoryMovie.Checked = true;
			this.checkBoxCategoryMovie.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxCategoryMovie.Location = new System.Drawing.Point(8, 96);
			this.checkBoxCategoryMovie.Name = "checkBoxCategoryMovie";
			this.checkBoxCategoryMovie.Size = new System.Drawing.Size(104, 16);
			this.checkBoxCategoryMovie.TabIndex = 3;
			this.checkBoxCategoryMovie.Text = "����t�@�C��";
			this.checkBoxCategoryMovie.CheckedChanged += new System.EventHandler(this.CheckChangedCheckBoxCategory);
			// 
			// tabPageOptions
			// 
			this.tabPageOptions.Controls.Add(this.groupBoxMiscOptions);
			this.tabPageOptions.Controls.Add(this.groupBoxOverwrite);
			this.tabPageOptions.Location = new System.Drawing.Point(4, 21);
			this.tabPageOptions.Name = "tabPageOptions";
			this.tabPageOptions.Size = new System.Drawing.Size(328, 191);
			this.tabPageOptions.TabIndex = 3;
			this.tabPageOptions.Text = "�I�v�V����";
			// 
			// groupBoxMiscOptions
			// 
			this.groupBoxMiscOptions.Controls.Add(this.checkBoxTest);
			this.groupBoxMiscOptions.Controls.Add(this.checkBoxKeepSource);
			this.groupBoxMiscOptions.Location = new System.Drawing.Point(8, 112);
			this.groupBoxMiscOptions.Name = "groupBoxMiscOptions";
			this.groupBoxMiscOptions.Size = new System.Drawing.Size(312, 64);
			this.groupBoxMiscOptions.TabIndex = 1;
			this.groupBoxMiscOptions.TabStop = false;
			this.groupBoxMiscOptions.Text = "���̑��̃I�v�V����";
			// 
			// checkBoxTest
			// 
			this.checkBoxTest.Location = new System.Drawing.Point(8, 40);
			this.checkBoxTest.Name = "checkBoxTest";
			this.checkBoxTest.Size = new System.Drawing.Size(208, 16);
			this.checkBoxTest.TabIndex = 1;
			this.checkBoxTest.Text = "�t�@�C���ɏ����o���Ȃ� (�e�X�g�̂�)";
			this.checkBoxTest.CheckedChanged += new System.EventHandler(this.CheckChangedCheckBoxCategory);
			// 
			// checkBoxKeepSource
			// 
			this.checkBoxKeepSource.Location = new System.Drawing.Point(8, 16);
			this.checkBoxKeepSource.Name = "checkBoxKeepSource";
			this.checkBoxKeepSource.Size = new System.Drawing.Size(208, 16);
			this.checkBoxKeepSource.TabIndex = 0;
			this.checkBoxKeepSource.Text = "���̃t�@�C�����폜���Ȃ�";
			// 
			// groupBoxOverwrite
			// 
			this.groupBoxOverwrite.Controls.Add(this.radioButtonOverwriteForce);
			this.groupBoxOverwrite.Controls.Add(this.radioButtonOverwriteConfirm);
			this.groupBoxOverwrite.Controls.Add(this.radioButtonOverwriteSkip);
			this.groupBoxOverwrite.Location = new System.Drawing.Point(8, 8);
			this.groupBoxOverwrite.Name = "groupBoxOverwrite";
			this.groupBoxOverwrite.Size = new System.Drawing.Size(312, 96);
			this.groupBoxOverwrite.TabIndex = 0;
			this.groupBoxOverwrite.TabStop = false;
			this.groupBoxOverwrite.Text = "�t�@�C���̏㏑��";
			// 
			// radioButtonOverwriteForce
			// 
			this.radioButtonOverwriteForce.Location = new System.Drawing.Point(8, 48);
			this.radioButtonOverwriteForce.Name = "radioButtonOverwriteForce";
			this.radioButtonOverwriteForce.Size = new System.Drawing.Size(208, 16);
			this.radioButtonOverwriteForce.TabIndex = 1;
			this.radioButtonOverwriteForce.Text = "�˂ɏ㏑������";
			// 
			// radioButtonOverwriteConfirm
			// 
			this.radioButtonOverwriteConfirm.Checked = true;
			this.radioButtonOverwriteConfirm.Location = new System.Drawing.Point(8, 24);
			this.radioButtonOverwriteConfirm.Name = "radioButtonOverwriteConfirm";
			this.radioButtonOverwriteConfirm.Size = new System.Drawing.Size(208, 16);
			this.radioButtonOverwriteConfirm.TabIndex = 0;
			this.radioButtonOverwriteConfirm.TabStop = true;
			this.radioButtonOverwriteConfirm.Text = "���̂Ǌm�F�����߂�";
			// 
			// radioButtonOverwriteSkip
			// 
			this.radioButtonOverwriteSkip.Location = new System.Drawing.Point(8, 72);
			this.radioButtonOverwriteSkip.Name = "radioButtonOverwriteSkip";
			this.radioButtonOverwriteSkip.Size = new System.Drawing.Size(208, 16);
			this.radioButtonOverwriteSkip.TabIndex = 2;
			this.radioButtonOverwriteSkip.Text = "���̃t�@�C���͏������Ȃ�";
			// 
			// buttonOk
			// 
			this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOk.Location = new System.Drawing.Point(208, 272);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.Size = new System.Drawing.Size(64, 24);
			this.buttonOk.TabIndex = 1;
			this.buttonOk.Text = "OK";
			this.buttonOk.Click += new System.EventHandler(this.ClickButtonOk);
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(280, 272);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(64, 24);
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "�L�����Z��";
			this.buttonCancel.Click += new System.EventHandler(this.ClickButtonCancel);
			// 
			// labelCurrentSize2
			// 
			this.labelCurrentSize2.Location = new System.Drawing.Point(224, 232);
			this.labelCurrentSize2.Name = "labelCurrentSize2";
			this.labelCurrentSize2.Size = new System.Drawing.Size(88, 16);
			this.labelCurrentSize2.TabIndex = 15;
			this.labelCurrentSize2.Text = "0";
			// 
			// labelCurrentSize
			// 
			this.labelCurrentSize.Location = new System.Drawing.Point(40, 232);
			this.labelCurrentSize.Name = "labelCurrentSize";
			this.labelCurrentSize.Size = new System.Drawing.Size(176, 16);
			this.labelCurrentSize.TabIndex = 14;
			this.labelCurrentSize.Text = "���݂̃T�C�Y (�����Ώۂ̍��v)�F";
			// 
			// labelUnzipSize2
			// 
			this.labelUnzipSize2.Location = new System.Drawing.Point(224, 248);
			this.labelUnzipSize2.Name = "labelUnzipSize2";
			this.labelUnzipSize2.Size = new System.Drawing.Size(88, 16);
			this.labelUnzipSize2.TabIndex = 17;
			this.labelUnzipSize2.Text = "0";
			// 
			// labelUnzipSize
			// 
			this.labelUnzipSize.Location = new System.Drawing.Point(40, 248);
			this.labelUnzipSize.Name = "labelUnzipSize";
			this.labelUnzipSize.Size = new System.Drawing.Size(176, 16);
			this.labelUnzipSize.TabIndex = 16;
			this.labelUnzipSize.Text = "�񈳏k�T�C�Y (�����Ώۂ̍��v)�F";
			// 
			// FormZipConfig
			// 
			this.AcceptButton = this.buttonOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(352, 300);
			this.Controls.Add(this.labelUnzipSize2);
			this.Controls.Add(this.labelUnzipSize);
			this.Controls.Add(this.labelCurrentSize2);
			this.Controls.Add(this.labelCurrentSize);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOk);
			this.Controls.Add(this.tabControl);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormZipConfig";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "WinEBZip (���k�E�L���̐ݒ�)";
			this.tabControl.ResumeLayout(false);
			this.tabPageBasic.ResumeLayout(false);
			this.groupBoxZipLevel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.trackBarZipLevel)).EndInit();
			this.groupBoxDestinationPath.ResumeLayout(false);
			this.tabPageSubbooks.ResumeLayout(false);
			this.groupBoxSubbooks.ResumeLayout(false);
			this.tabPageFileTypes.ResumeLayout(false);
			this.groupBoxCategory.ResumeLayout(false);
			this.tabPageOptions.ResumeLayout(false);
			this.groupBoxMiscOptions.ResumeLayout(false);
			this.groupBoxOverwrite.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// �uOk�v�{�^���������ꂽ�Ƃ��̏���
		/// </summary>
		private void ClickButtonOk(object sender, System.EventArgs e) {
			WinEBZipRegistry registry = null;
			try {
				registry = new WinEBZipRegistry();
				registry.ZipLevel    = this.trackBarZipLevel.Value;
				registry.SkipFont    = !this.checkBoxCategoryFont.Checked;
				registry.SkipGraphic = !this.checkBoxCategoryGraphic.Checked;
				registry.SkipSound   = !this.checkBoxCategorySound.Checked;
				registry.SkipMovie   = !this.checkBoxCategoryMovie.Checked;
			}
			finally {
				if (registry != null)
					registry.Close();
			}

			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		/// <summary>
		/// �L�����Z���{�^���������ꂽ�Ƃ��̏���
		/// </summary>
		private void ClickButtonCancel(object sender, System.EventArgs e) {
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		/// <summary>
		/// ���k���x�����ύX���ꂽ�Ƃ��̏���
		/// </summary>
		private void ValueChangedTrackBarZipLevel(object sender, System.EventArgs e) {
			this.labelZipLevel.Text = this.trackBarZipLevel.Value.ToString();
		}

		/// <summary>
		/// �������ݐ�̃p�X�́u�Q��...�v�{�^���������ꂽ�Ƃ��̏���
		/// </summary>
		private void ClickButtonDestinationPath(object sender, System.EventArgs e) {
			FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
			folderBrowserDialog.SelectedPath = this.DestinationPath;
			DialogResult dialogResult = folderBrowserDialog.ShowDialog();
			if (dialogResult == DialogResult.OK)
				this.textBoxDestinationPath.Text = folderBrowserDialog.SelectedPath;
		}

		/// <summary>
		/// �e�X�g���[�h���L���ɂȂ����Ƃ��̏���
		/// </summary>
		private void CheckedChangedCheckButtonTest(object sender, System.EventArgs e) {
			bool flag = !this.checkBoxTest.Checked;
			this.radioButtonOverwriteConfirm.Enabled = flag;
			this.radioButtonOverwriteForce.Enabled   = flag;
			this.radioButtonOverwriteSkip.Enabled    = flag;
		}

		/// <summary>
		/// �����ΏۂƂȂ�t�@�C���̎�ނɕύX���������Ƃ��̏���
		/// </summary>
		public void CheckChangedCheckBoxCategory(object sender, System.EventArgs e) {
			this.UpdateTotalSize();
		}

		/// <summary>
		/// �����ΏۂƂȂ镛�{�ɕύX���������Ƃ��̏���
		/// </summary>
		private void ItemCheckCheckedListBoxSubbooks(object sender, 
			System.Windows.Forms.ItemCheckEventArgs e) {
			string directory = this.subbooks[e.Index].Directory;
 			object hashValue = this.zipSizeTables[directory];
			if (hashValue == null)
				return;
			ZipSizeTable table = (ZipSizeTable) hashValue;
			table.Selected = (e.NewValue == CheckState.Checked);
			this.zipSizeTables[directory] = table;
			this.UpdateTotalSize();
		}

		/// <summary>
		/// �������ݐ�̃p�X���ύX���ꂽ�Ƃ��̏���
		/// </summary>
		private void TextChangedTextBoxDestinationPath(object sender, System.EventArgs e) {
			this.buttonOk.Enabled = (((TextBox) sender).Text != "");
		}

		/// <summary>
		/// ������� (���k/�L��) �̎擾
		/// </summary>
		public EBZipBookOperation Operation {
			get {
				return this.Operation;
			}
			set {
				this.operation = value;

				if (value == EBZipBookOperation.Zip)
					this.Text = "WinEBZip (���k�̐ݒ�)";
				else
					this.Text = "WinEBZip (�L���̐ݒ�)";

				if (value == EBZipBookOperation.Zip)
					this.groupBoxZipLevel.Show();
				else
					this.groupBoxZipLevel.Hide();
			}
		}

		/// <summary>
		/// ebzip.exe �ň��k/�L�����鏑�Ђ̃p�X�̎擾�Ɛݒ�
		/// </summary>
		public string SourcePath {
			get {
				return string.Copy(this.sourcePath);
			}
			set {
				this.sourcePath = string.Copy(value);
			}
		}

		/// <summary>
		/// ebzip.exe �ň��k/�L�������f�[�^�̏������ݐ�̃p�X�̎擾�Ɛݒ�
		/// </summary>
		public string DestinationPath {
			get {
				return this.textBoxDestinationPath.Text;
			}
			set {
				this.textBoxDestinationPath.Text = value;
			}
		}

		/// <summary>
		/// ���k���x���̎擾�Ɛݒ�
		/// </summary>
		public int ZipLevel {
			get {
				return this.trackBarZipLevel.Value;
			}
			set {
				this.trackBarZipLevel.Value = value;
			}
		}

		/// <summary>
		/// ebzip.exe �ň��k/�L�����镡�{�̈ꗗ�̎擾��ݒ�
		/// </summary>
		public EBSubbookCollection SelectedSubbooks {
			get {
				EBSubbookCollection result = new EBSubbookCollection();

				foreach (EBSubbook subbook in this.subbooks) {
					object hashValue = this.zipSizeTables[subbook.Directory];
					if (hashValue != null&& ((ZipSizeTable) hashValue).Selected)
						result.Add(EBSubbook.Copy(subbook));
				}
				return result;
			}
		}

		/// <summary>
		/// �I������Ă��鈳�k�Ώۃt�@�C���̌��݂̃T�C�Y�̍��v
		/// </summary>
		public uint SelectedCurrentSize {
			get {
				return this.selectedCurrentSize;
			}
		}

		/// <summary>
		/// �I������Ă��鈳�k�Ώۃt�@�C���̔񈳏k���̃T�C�Y�̍��v
		/// </summary>
		public uint SelectedUnzipSize {
			get {
				return this.selectedUnzipSize;
			}
		}

		/// <summary>
		/// �O���t�@�C����ΏۂƂ��邩�ǂ����̃t���O�̎擾��ݒ�
		/// </summary>
		public bool SkipFont {
			get {
				return !this.checkBoxCategoryFont.Checked;
			}
			set {
				this.checkBoxCategoryFont.Checked = !value;
			}
		}

		/// <summary>
		/// �摜�t�@�C����ΏۂƂ��邩�ǂ����̃t���O�̎擾��ݒ�
		/// </summary>
		public bool SkipGraphic {
			get {
				return !this.checkBoxCategoryGraphic.Checked;
			}
			set {
				this.checkBoxCategoryGraphic.Checked = !value;
			}
		}

		/// <summary>
		/// �����t�@�C����ΏۂƂ��邩�ǂ����̃t���O�̎擾��ݒ�
		/// </summary>
		public bool SkipSound {
			get {
				return !this.checkBoxCategorySound.Checked;
			}
			set {
				this.checkBoxCategorySound.Checked = !value;
			}
		}

		/// <summary>
		/// ����t�@�C����ΏۂƂ��邩�ǂ����̃t���O�̎擾��ݒ�
		/// </summary>
		public bool SkipMovie {
			get {
				return !this.checkBoxCategoryMovie.Checked;
			}
			set {
				this.checkBoxCategoryMovie.Checked = !value;
			}
		}

		/// <summary>
		/// �㏑�����[�h�̎擾��ݒ�
		/// </summary>
		public EBZipOverwriteMode OverwriteMode {
			get {
				if (this.radioButtonOverwriteForce.Checked)
					return EBZipOverwriteMode.Force;
				else if (this.radioButtonOverwriteSkip.Checked)
					return EBZipOverwriteMode.Skip;
				else
					return EBZipOverwriteMode.Confirm;
			}
			set {
				if (value == EBZipOverwriteMode.Force)
					this.radioButtonOverwriteForce.Checked = true;
				else if (value == EBZipOverwriteMode.Skip)
					this.radioButtonOverwriteSkip.Checked = true;
				else
					this.radioButtonOverwriteConfirm.Checked = true;
			}
		}

		/// <summary>
		/// ���̃t�@�C�����������Ɏc�����ǂ����̃t���O�̎擾��ݒ�
		/// </summary>
		public bool KeepSource {
			get {
				return this.checkBoxKeepSource.Checked;
			}
			set {
				this.checkBoxKeepSource.Checked = value;
			}
		}

		/// <summary>
		/// �e�X�g���[�h�t���O�̎擾��ݒ�
		/// </summary>
		public bool TestMode {
			get {
				return this.checkBoxTest.Checked;
			}
			set {
				this.checkBoxKeepSource.Checked = value;
				this.radioButtonOverwriteConfirm.Enabled = !value;
				this.radioButtonOverwriteForce.Enabled   = !value;
				this.radioButtonOverwriteSkip.Enabled    = !value;
			}
		}

		/// <summary>
		/// ebzip.exe �ւ̃p�X�̎擾��ݒ�
		/// </summary>
		public string EBZipPath {
			get {
				return this.ebzipPath;
			}
			set {
				this.ebzipPath = String.Copy(value);
			}
		}

		/// <summary>
		/// ebrefile.exe �ւ̃p�X�̎擾��ݒ�
		/// </summary>
		public string EBRefilePath {
			get {
				return this.ebrefilePath;
			}
			set {
				this.ebrefilePath = String.Copy(value);
			}
		}

		/// <summary>
		/// ���{�̃��X�g�ƃt�@�C���̃��X�g����A�e���{�̃T�C�Y�A�t�@�C��
		/// �̎�ޕʂ̃T�C�Y�̕\�����B
		/// </summary>
		/// <param name="subbooksArg">���{�̃��X�g</param>
		/// <param name="filesArg">�t�@�C���̃��X�g</param>
		public void LoadAuxInfo(EBSubbookCollection subbooksArg,
			EBZipFileCollection filesArg) {
			this.subbooks.Clear();
			this.zipSizeTables.Clear();
			this.checkedListBoxSubbooks.Items.Clear();

			LoadSubbookInfo(subbooksArg);
			LoadFileInfo(filesArg);
		}

		/// <summary>
		/// ���{�̃��X�g����T�C�Y�̕\�����B(LoadAuxInfo �̉�����)
		/// </summary>
		/// <param name="subbooksArg">���{�̃��X�g</param>
		private void LoadSubbookInfo(EBSubbookCollection subbooksArg) {
			foreach (EBSubbook subbook in subbooksArg) {
				this.subbooks.Add(EBSubbook.Copy(subbook));
				this.checkedListBoxSubbooks.Items.Add(subbook.Title, true);
				if (!this.zipSizeTables.ContainsKey(subbook.Directory)) {
					ZipSizeTable table = new ZipSizeTable(true);
					this.zipSizeTables[subbook.Directory] = table;
				}
			}
		}
 
		/// <summary>
		/// �t�@�C���̃��X�g����T�C�Y�̕\�����B(LoadAuxInfo �̉�����)
		/// </summary>
		/// <param name="subbooksArg">���{�̃��X�g</param>
		private void LoadFileInfo(EBZipFileCollection filesArg) {
			string hashKey;

			foreach (EBZipFile file in filesArg) {
				int backslashIndex = file.Name.IndexOf('\\');
				if (backslashIndex >= 0)
					hashKey = file.Name.Substring(0, backslashIndex).ToLower();
				else
					hashKey = "";

				object hashValue = this.zipSizeTables[hashKey];
				if (hashValue == null)
					hashValue = (object) new ZipSizeTable(hashKey == "");
				ZipSizeTable table = (ZipSizeTable) hashValue;

				if (file.Category == EBZipFileCategory.Font) {
					table.CurrentFontSize  += file.CurrentSize;
					table.UnzipFontSize    += file.UnzipSize;
				}
				else if (file.Category == EBZipFileCategory.Graphic) {
					table.CurrentGraphicSize += file.CurrentSize;
					table.UnzipGraphicSize   += file.UnzipSize;
				}
				else if (file.Category == EBZipFileCategory.Sound) {
					table.CurrentSoundSize += file.CurrentSize;
					table.UnzipSoundSize   += file.UnzipSize;
				}
				else if (file.Category == EBZipFileCategory.Movie) {
					table.CurrentMovieSize += file.CurrentSize;
					table.UnzipMovieSize   += file.UnzipSize;
				}
				else {
					table.CurrentRequiredSize += file.CurrentSize;
					table.UnzipRequiredSize   += file.UnzipSize;
				}

				hashValue = table;
				this.zipSizeTables[hashKey] = hashValue;
			}

			UpdateTotalSize();
		}

		/// <summary>
		/// ���v�T�C�Y�̌v�Z����蒼���B
		/// </summary>
		private void UpdateTotalSize() {
			uint currentSize = 0;
			uint unzipSize   = 0;

			foreach (object hashValue in this.zipSizeTables.Values) {
				ZipSizeTable table = (ZipSizeTable) hashValue;
				if (!table.Selected)
					continue;

				currentSize += table.CurrentRequiredSize;
				unzipSize   += table.UnzipRequiredSize;
				if (this.checkBoxCategoryFont.Checked) {
					currentSize += table.CurrentFontSize;
					unzipSize   += table.UnzipFontSize;
				}
				if (this.checkBoxCategoryGraphic.Checked) {
					currentSize += table.CurrentGraphicSize;
					unzipSize   += table.UnzipGraphicSize;
				}
				if (this.checkBoxCategorySound.Checked) {
					currentSize += table.CurrentSoundSize;
					unzipSize   += table.UnzipSoundSize;
				}
				if (this.checkBoxCategoryMovie.Checked) {
					currentSize += table.CurrentMovieSize;
					unzipSize   += table.UnzipMovieSize;
				}
			}

			this.selectedCurrentSize = currentSize;
			this.selectedUnzipSize   = unzipSize;

			this.labelCurrentSize2.Text = currentSize.ToString("n0");
			this.labelUnzipSize2.Text   = unzipSize.ToString("n0");
		}

		/// <summary>
		/// ���k�E�L�����s�p�� EBZip �I�u�W�F�N�g��Ԃ��B
		/// </summary>
		public EBZip EBZip {
			get {
				EBZip ebzip = new EBZip(this.operation, this.ebzipPath);

				ebzip.SourcePath      = this.SourcePath;
				ebzip.DestinationPath = this.DestinationPath;
				ebzip.ZipLevel        = this.ZipLevel;
				ebzip.Subbooks        = this.SelectedSubbooks;
				ebzip.SkipFont        = this.SkipFont;
				ebzip.SkipGraphic     = this.SkipGraphic;
				ebzip.SkipSound       = this.SkipSound;
				ebzip.SkipMovie       = this.SkipMovie;
				ebzip.OverwriteMode   = this.OverwriteMode;
				ebzip.KeepSource      = this.KeepSource;
				ebzip.TestMode        = this.TestMode;
				return ebzip;
			}
		}

		/// <summary>
		/// ���k�E�L�����s�p�� EBRefile �I�u�W�F�N�g��Ԃ��B
		/// </summary>
		public EBRefile EBRefile {
			get {
				EBRefile ebrefile = new EBRefile(this.ebrefilePath);

				ebrefile.BookPath = this.DestinationPath;
				ebrefile.Subbooks = this.SelectedSubbooks;
				return ebrefile;
			}
		}
	}
}

// Local Variables:
// tab-width: 4
