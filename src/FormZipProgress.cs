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
using System.Threading;
using System.IO;

namespace WinEBZip {
	/// <summary>
	/// FormZipProgress �̊T�v�̐����ł��B
	/// </summary>
	public class FormZipProgress : System.Windows.Forms.Form {
		/// <summary>
		/// �K�v�ȃf�U�C�i�ϐ��ł��B
		/// </summary>
		private System.ComponentModel.Container components = null;

		private System.Windows.Forms.ListView listViewFileList;
		private System.Windows.Forms.ColumnHeader columnHeaderFileName;
		private System.Windows.Forms.ColumnHeader columnHeaderUnzipSize;
		private System.Windows.Forms.ColumnHeader columnHeaderCurrentSize;
		private System.Windows.Forms.ColumnHeader columnHeaderZipRatio;
		private System.Windows.Forms.ColumnHeader columnHeaderNote;
		private System.Windows.Forms.Panel panelBottom;
		private System.Windows.Forms.Button buttonClose;
		private System.Windows.Forms.Label labelTotal3;
		private System.Windows.Forms.Label labelFile3;
		private System.Windows.Forms.ProgressBar progressBarTotal;
		private System.Windows.Forms.Label labelTotal2;
		private System.Windows.Forms.Label labelTotal;
		private System.Windows.Forms.Label labelFile;
		private System.Windows.Forms.Label labelFile2;
		private System.Windows.Forms.ProgressBar progressBarFile;
		private System.Windows.Forms.Panel panelTop;
		private System.Windows.Forms.Label labelDestination2;
		private System.Windows.Forms.Label labelSource2;
		private System.Windows.Forms.Label labelDestination;
		private System.Windows.Forms.Label labelSource;

		// ebrefile.exe �R�}���h�̎��s�I�u�W�F�N�g
		private EBRefile ebrefile;

		// ebzip.exe �R�}���h�̎��s�I�u�W�F�N�g
		private EBZip ebzip;

		// ebzip.exe �R�}���h�̎��s�X���b�h
		private Thread commandThread;

		// ���샂�[�h (���k/�L��)
		private EBZipBookOperation operation;

		// �S�����Ώۃt�@�C���̔񈳏k���̃T�C�Y�̍��v
		private uint totalUnzipSize;

		// ���݂̃t�@�C������ёS�̂̏����̐i�s��
		private uint totalUnzipOffset;
		private uint fileUnzipOffset;
		private System.Windows.Forms.Button buttonAbort;

		// �㏑���m�F�Łu���ׂď㏑���v��I�񂾂��ǂ����̃t���O
		private bool forceOverwrite;

		public FormZipProgress(EBZip ebzipArg, EBRefile ebrefileArg, 
			uint totalUnzipSizeArg) {
			//
			// Windows �t�H�[�� �f�U�C�i �T�|�[�g�ɕK�v�ł��B
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent �Ăяo���̌�ɁA�R���X�g���N�^ �R�[�h��ǉ����Ă��������B
			//
			Initialize(ebzipArg, ebrefileArg, totalUnzipSizeArg);
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FormZipProgress));
			this.listViewFileList = new System.Windows.Forms.ListView();
			this.columnHeaderFileName = new System.Windows.Forms.ColumnHeader();
			this.columnHeaderUnzipSize = new System.Windows.Forms.ColumnHeader();
			this.columnHeaderCurrentSize = new System.Windows.Forms.ColumnHeader();
			this.columnHeaderZipRatio = new System.Windows.Forms.ColumnHeader();
			this.columnHeaderNote = new System.Windows.Forms.ColumnHeader();
			this.panelBottom = new System.Windows.Forms.Panel();
			this.buttonClose = new System.Windows.Forms.Button();
			this.labelTotal3 = new System.Windows.Forms.Label();
			this.labelFile3 = new System.Windows.Forms.Label();
			this.progressBarTotal = new System.Windows.Forms.ProgressBar();
			this.labelTotal2 = new System.Windows.Forms.Label();
			this.labelTotal = new System.Windows.Forms.Label();
			this.buttonAbort = new System.Windows.Forms.Button();
			this.labelFile = new System.Windows.Forms.Label();
			this.labelFile2 = new System.Windows.Forms.Label();
			this.progressBarFile = new System.Windows.Forms.ProgressBar();
			this.panelTop = new System.Windows.Forms.Panel();
			this.labelDestination2 = new System.Windows.Forms.Label();
			this.labelSource2 = new System.Windows.Forms.Label();
			this.labelDestination = new System.Windows.Forms.Label();
			this.labelSource = new System.Windows.Forms.Label();
			this.panelBottom.SuspendLayout();
			this.panelTop.SuspendLayout();
			this.SuspendLayout();
			// 
			// listViewFileList
			// 
			this.listViewFileList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							   this.columnHeaderFileName,
																							   this.columnHeaderUnzipSize,
																							   this.columnHeaderCurrentSize,
																							   this.columnHeaderZipRatio,
																							   this.columnHeaderNote});
			this.listViewFileList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listViewFileList.Location = new System.Drawing.Point(0, 72);
			this.listViewFileList.Name = "listViewFileList";
			this.listViewFileList.Size = new System.Drawing.Size(576, 186);
			this.listViewFileList.TabIndex = 5;
			this.listViewFileList.TabStop = false;
			this.listViewFileList.View = System.Windows.Forms.View.Details;
			// 
			// columnHeaderFileName
			// 
			this.columnHeaderFileName.Text = "�t�@�C����";
			this.columnHeaderFileName.Width = 180;
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
			// columnHeaderNote
			// 
			this.columnHeaderNote.Text = "���l";
			this.columnHeaderNote.Width = 160;
			// 
			// panelBottom
			// 
			this.panelBottom.Controls.Add(this.buttonClose);
			this.panelBottom.Controls.Add(this.labelTotal3);
			this.panelBottom.Controls.Add(this.labelFile3);
			this.panelBottom.Controls.Add(this.progressBarTotal);
			this.panelBottom.Controls.Add(this.labelTotal2);
			this.panelBottom.Controls.Add(this.labelTotal);
			this.panelBottom.Controls.Add(this.buttonAbort);
			this.panelBottom.Controls.Add(this.labelFile);
			this.panelBottom.Controls.Add(this.labelFile2);
			this.panelBottom.Controls.Add(this.progressBarFile);
			this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panelBottom.Location = new System.Drawing.Point(0, 258);
			this.panelBottom.Name = "panelBottom";
			this.panelBottom.Size = new System.Drawing.Size(576, 96);
			this.panelBottom.TabIndex = 9;
			// 
			// buttonClose
			// 
			this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonClose.Enabled = false;
			this.buttonClose.Location = new System.Drawing.Point(296, 64);
			this.buttonClose.Name = "buttonClose";
			this.buttonClose.Size = new System.Drawing.Size(72, 24);
			this.buttonClose.TabIndex = 1;
			this.buttonClose.Text = "����";
			this.buttonClose.Click += new System.EventHandler(this.ClickButtonClose);
			// 
			// labelTotal3
			// 
			this.labelTotal3.Location = new System.Drawing.Point(232, 40);
			this.labelTotal3.Name = "labelTotal3";
			this.labelTotal3.Size = new System.Drawing.Size(40, 16);
			this.labelTotal3.TabIndex = 8;
			this.labelTotal3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelFile3
			// 
			this.labelFile3.Location = new System.Drawing.Point(232, 16);
			this.labelFile3.Name = "labelFile3";
			this.labelFile3.Size = new System.Drawing.Size(40, 16);
			this.labelFile3.TabIndex = 7;
			this.labelFile3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// progressBarTotal
			// 
			this.progressBarTotal.Location = new System.Drawing.Point(280, 40);
			this.progressBarTotal.Name = "progressBarTotal";
			this.progressBarTotal.Size = new System.Drawing.Size(280, 16);
			this.progressBarTotal.TabIndex = 6;
			// 
			// labelTotal2
			// 
			this.labelTotal2.Location = new System.Drawing.Point(64, 40);
			this.labelTotal2.Name = "labelTotal2";
			this.labelTotal2.Size = new System.Drawing.Size(144, 16);
			this.labelTotal2.TabIndex = 4;
			this.labelTotal2.UseMnemonic = false;
			// 
			// labelTotal
			// 
			this.labelTotal.Location = new System.Drawing.Point(8, 40);
			this.labelTotal.Name = "labelTotal";
			this.labelTotal.Size = new System.Drawing.Size(48, 16);
			this.labelTotal.TabIndex = 3;
			this.labelTotal.Text = "�S�́F";
			// 
			// buttonAbort
			// 
			this.buttonAbort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonAbort.DialogResult = System.Windows.Forms.DialogResult.Abort;
			this.buttonAbort.Location = new System.Drawing.Point(200, 64);
			this.buttonAbort.Name = "buttonAbort";
			this.buttonAbort.Size = new System.Drawing.Size(72, 24);
			this.buttonAbort.TabIndex = 0;
			this.buttonAbort.Text = "���~";
			this.buttonAbort.Click += new System.EventHandler(this.ClickButtonAbort);
			// 
			// labelFile
			// 
			this.labelFile.Location = new System.Drawing.Point(8, 16);
			this.labelFile.Name = "labelFile";
			this.labelFile.Size = new System.Drawing.Size(48, 16);
			this.labelFile.TabIndex = 0;
			this.labelFile.Text = "�t�@�C���F";
			// 
			// labelFile2
			// 
			this.labelFile2.Location = new System.Drawing.Point(64, 16);
			this.labelFile2.Name = "labelFile2";
			this.labelFile2.Size = new System.Drawing.Size(144, 16);
			this.labelFile2.TabIndex = 1;
			this.labelFile2.UseMnemonic = false;
			// 
			// progressBarFile
			// 
			this.progressBarFile.Location = new System.Drawing.Point(280, 16);
			this.progressBarFile.Name = "progressBarFile";
			this.progressBarFile.Size = new System.Drawing.Size(280, 16);
			this.progressBarFile.TabIndex = 2;
			// 
			// panelTop
			// 
			this.panelTop.Controls.Add(this.labelDestination2);
			this.panelTop.Controls.Add(this.labelSource2);
			this.panelTop.Controls.Add(this.labelDestination);
			this.panelTop.Controls.Add(this.labelSource);
			this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelTop.Location = new System.Drawing.Point(0, 0);
			this.panelTop.Name = "panelTop";
			this.panelTop.Size = new System.Drawing.Size(576, 72);
			this.panelTop.TabIndex = 10;
			// 
			// labelDestination2
			// 
			this.labelDestination2.Location = new System.Drawing.Point(96, 40);
			this.labelDestination2.Name = "labelDestination2";
			this.labelDestination2.Size = new System.Drawing.Size(416, 24);
			this.labelDestination2.TabIndex = 4;
			this.labelDestination2.UseMnemonic = false;
			// 
			// labelSource2
			// 
			this.labelSource2.Location = new System.Drawing.Point(96, 8);
			this.labelSource2.Name = "labelSource2";
			this.labelSource2.Size = new System.Drawing.Size(416, 24);
			this.labelSource2.TabIndex = 3;
			this.labelSource2.UseMnemonic = false;
			// 
			// labelDestination
			// 
			this.labelDestination.Location = new System.Drawing.Point(8, 40);
			this.labelDestination.Name = "labelDestination";
			this.labelDestination.Size = new System.Drawing.Size(72, 16);
			this.labelDestination.TabIndex = 2;
			this.labelDestination.Text = "�������ݐ�F";
			// 
			// labelSource
			// 
			this.labelSource.Location = new System.Drawing.Point(8, 8);
			this.labelSource.Name = "labelSource";
			this.labelSource.Size = new System.Drawing.Size(72, 16);
			this.labelSource.TabIndex = 1;
			this.labelSource.Text = "�ǂݍ��݌��F";
			// 
			// FormZipProgress
			// 
			this.AcceptButton = this.buttonClose;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.CancelButton = this.buttonClose;
			this.ClientSize = new System.Drawing.Size(576, 354);
			this.Controls.Add(this.listViewFileList);
			this.Controls.Add(this.panelBottom);
			this.Controls.Add(this.panelTop);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormZipProgress";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "WinEBZip (������)";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.ClosingForm);
			this.panelBottom.ResumeLayout(false);
			this.panelTop.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// �u���~�v�{�^���������ꂽ�Ƃ��̏����B
		/// </summary>
		private void ClickButtonAbort(object sender, System.EventArgs e) {
			this.AbortCommand();
			this.Close();
		}

		/// <summary>
		/// �u����v�{�^���������ꂽ�Ƃ��̏����B
		/// </summary>
		private void ClickButtonClose(object sender, System.EventArgs e) {
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		/// <summary>
		/// �E�B���h�E�E��́u����v�{�^���������ꂽ�Ƃ��̏����B
		/// </summary>
		private void ClosingForm(object sender, System.ComponentModel.CancelEventArgs e) {
			this.AbortCommand();
			this.Close();
		}

		/// <summary>
		/// ���k�E�L�������𒆎~����B
		/// </summary>
		private void AbortCommand() {
			lock (this) {
				if (this.commandThread != null) {
					this.commandThread.Abort();
					this.commandThread = null;
					this.DialogResult = DialogResult.Abort;
				}
				else {
					this.DialogResult = DialogResult.OK;
				}
			}
		}

		/// <summary>
		/// ���k�E�L���̐ݒ���Z�b�g����B
		/// </summary>
		private void Initialize(EBZip ebzipArg, EBRefile ebrefileArg, uint totalUnzipSizeArg) {
			this.ebzip    = ebzipArg;
			this.ebrefile = ebrefileArg;
			ebzip.StartBookEvent        += new StartBook(this.StartBook);
			ebzip.EndBookEvent          += new EndBook(this.EndBook);
			ebzip.StartFileEvent        += new StartFile(this.StartFile);
			ebzip.ProgressFileEvent     += new ProgressFile(this.ProgressFile);
			ebzip.EndFileEvent          += new EndFile(this.EndFile);
			ebzip.ConfirmOverwriteEvent += new ConfirmOverwrite(this.ConfirmOverwrite);
			ebzip.SkipFileEvent         += new SkipFile(this.SkipFile);
			ebzip.DeleteFileErrorEvent  += new DeleteFileError(this.DeleteFileError);

			this.Text = string.Format("WinEBZip ({0})", (0.0).ToString("p1"));
			this.operation        = ebzip.Operation;
			this.totalUnzipSize   = totalUnzipSizeArg;
			this.totalUnzipOffset = 0;
			this.fileUnzipOffset  = 0;
			this.forceOverwrite   = false;

			this.labelTotal2.Text = 
				string.Format("{0} / {1}", this.totalUnzipOffset, this.totalUnzipSize);
			this.labelTotal2.Text = 
				string.Format("{0} / {1}", this.totalUnzipOffset, this.totalUnzipSize);
		}

		/// <summary>
		/// ebzip.exe �����s���āA���k�E�L���̏������s���B
		/// </summary>
		public void Execute() {
			DialogResult dialogResult;

			do {
				dialogResult = DialogResult.None;

				try {
					this.commandThread = new Thread(new ThreadStart(ExecuteThreadStart));
					this.commandThread.Start();
				}
				catch (Win32Exception e) {
					dialogResult = MessageBox.Show(e.Message,
						"WinEBZip (ebzip ���s�G���[)", MessageBoxButtons.RetryCancel,
						MessageBoxIcon.Error, MessageBoxDefaultButton.Button2);
				}
			} while (dialogResult == DialogResult.Retry);
		}

		/// <summary>
		/// ebzip.exe, ebrefile.exe ���s�X���b�h�̊J�n�|�C���g�B
		/// </summary>
		public void ExecuteThreadStart() {
			DialogResult dialogResult;

			// ebzip.exe �����s����B
			do {
				dialogResult = DialogResult.None;

				try {
					this.ebzip.Execute();
				}
				catch (EBCommandException e) {
					dialogResult = MessageBox.Show(e.Message,
						"WinEBZip (ebzip ���s�G���[)", MessageBoxButtons.RetryCancel,
						MessageBoxIcon.Error, MessageBoxDefaultButton.Button2);
				}
			} while (dialogResult == DialogResult.Retry);
		
			if (dialogResult == DialogResult.Cancel) {
				this.DialogResult = DialogResult.Cancel;
				this.Close();
			}

			// ebrefile.exe �����s����B
			do {
				dialogResult = DialogResult.None;
				try {
					if (!this.ebzip.TestMode)
						this.ebrefile.Execute();
				}
				catch (EBCommandException e) {
					dialogResult = MessageBox.Show(e.Message,
						"WinEBZip (ebrefile ���s�G���[)", MessageBoxButtons.RetryCancel,
						MessageBoxIcon.Error, MessageBoxDefaultButton.Button2);
					if (dialogResult == DialogResult.Cancel)
						return;
				}
			} while (dialogResult == DialogResult.Retry);

			if (dialogResult == DialogResult.Cancel) {
				this.DialogResult = DialogResult.Cancel;
				this.Close();
			}

			// �������b�Z�[�W��\������B
			MessageBox.Show("�������܂����B", "WinEBZip (����)", MessageBoxButtons.OK,
				MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

			// �u���~�v�̑���Ɂu����v�{�^����L���ɂ���B
			this.buttonAbort.Enabled = false;
			this.buttonClose.Enabled = true;

			lock (this) {
				this.commandThread = null;
			}
		}

		/// <summary>
		/// ���Ђ̈��k���J�n����ۂɎ��s�����f���Q�[�g
		/// </summary>
		public void StartBook(EBZip ebzip, string sourcePath, string destinationPath) {
			// �i�s�󋵒l������������B
			this.totalUnzipOffset = 0;
			this.fileUnzipOffset  = 0;

			// �������̃t�@�C���\����������������B
			this.labelSource2.Text = "";
			this.labelDestination2.Text = "";

			// ���X�g�r���[������������B
			this.listViewFileList.Items.Clear();

			// �v���O���X�o�[������������B
			this.labelFile2.Text = "";
			this.labelFile3.Text = (0.0).ToString("p1");
			this.progressBarFile.Value = 0;

			this.labelTotal2.Text = "";
			this.labelTotal3.Text = (0.0).ToString("p1");
			this.progressBarTotal.Value = 0;
		}

		/// <summary>
		/// ���Ђ̈��k�����������ۂɎ��s�����f���Q�[�g
		/// </summary>
		public void EndBook(EBZip ebzip, string sourcePath, string destinationPath) {
			// �i�s�󋵒l���X�V����B
			this.totalUnzipOffset = this.totalUnzipSize;
			double totalPercent = 1.0;

			// �^�C�g���o�[�̕\�����X�V����B
			this.Text = string.Format("EBWinZip ({0})", (1.0).ToString("p1")); 
			this.labelTotal2.Text = string.Format("{0} / {1}",
				this.totalUnzipOffset.ToString("n0"),	
				this.totalUnzipSize.ToString("n0"));
			this.labelTotal3.Text = totalPercent.ToString("p1"); 
			this.progressBarTotal.Value = (int)(totalPercent * 100.0);
		}

		/// <summary>
		/// �t�@�C���̏������J�n����ۂɎ��s�����f���Q�[�g
		/// </summary>
		public void StartFile(EBZip ebzip, EBZipFileOperation operation, 
			string sourceFile, string destinationFile) {
			// �i�s�󋵒l���X�V����B
			this.fileUnzipOffset = 0;

			// �������̃t�@�C���\�������X�V����B
			this.labelSource2.Text = sourceFile;
			this.labelDestination2.Text = destinationFile;

			// �v���O���X�o�[�̕\�����X�V����B
			this.labelFile2.Text = "";
			this.progressBarFile.Value = 0;

			this.Refresh();
		}

		/// <summary>
		/// �t�@�C���̏����̐i�s�󋵂��󂯎��f���Q�[�g
		/// </summary>
		public void ProgressFile(EBZip ebzip, EBZipFileOperation operation,
			string sourceFile, string destinationFile, uint sourceOffset, 
			uint sourceSize) {
			// �i�s�󋵒l���v�Z�������B
			if (sourceOffset > this.fileUnzipOffset) {
				this.totalUnzipOffset += sourceOffset - this.fileUnzipOffset;
				this.fileUnzipOffset = sourceOffset;
			}

			double filePercent = (double) sourceOffset / (double) sourceSize;
			if (filePercent > 1.0)
				filePercent = 1.0;
			double totalPercent = 
				(double) this.totalUnzipOffset / (double) this.totalUnzipSize;
			if (totalPercent > 1.0)
				totalPercent = 1.0;

			// �v���O���X�o�[�̕\�����X�V����B
			this.labelFile2.Text = string.Format("{0} / {1}",
				sourceOffset.ToString("n0"), sourceSize.ToString("n0"));
			this.labelFile3.Text = filePercent.ToString("p1");
			this.progressBarFile.Value = (int)(filePercent * 100.0);

			this.labelTotal2.Text = string.Format("{0} / {1}",
				this.totalUnzipOffset.ToString("n0"),	
				this.totalUnzipSize.ToString("n0"));
			this.labelTotal3.Text = totalPercent.ToString("p1"); 
			this.progressBarTotal.Value = (int)(totalPercent * 100.0);

			// �^�C�g���o�[�̕\�����X�V����B
			this.Text = string.Format("EBWinZip ({0})", totalPercent.ToString("p1")); 

			this.Refresh();
		}

		/// <summary>
		/// �t�@�C���̏��������������ۂɔ���������C�x���g�̂��߂̃f���Q�[�g
		/// </summary>
		public void EndFile(EBZip ebzip, EBZipFileOperation operation, 
			string sourceFile, string destinationFile, uint sourceSize,
			uint destinationSize) {
			EndOrSkipFile(ebzip, operation, sourceFile, destinationFile, sourceSize,
				destinationSize, "");
		}

		/// <summary>
		/// �t�@�C���̏������X�L�b�v�����ۂɎ��s�����f���Q�[�g
		/// </summary>
		public void SkipFile(EBZip ebzip, EBZipFileOperation operation, 
			string sourceFile, string destinationFile) {
			// ���̃t�@�C���Ə������ݐ�̃t�@�C���̑傫�����擾����B
			uint sourceSize = 0;
			uint destinationSize = 0;

			try  {
				FileInfo sourceInfo = new FileInfo(sourceFile);
				sourceSize = (uint) sourceInfo.Length;
			}
			catch {
				sourceSize = 0;
			}

			try {
				FileInfo destinationInfo = new FileInfo(destinationFile);
				destinationSize = (uint) destinationInfo.Length;
			}
			catch {
				destinationSize = 0;
			}

			// �t�@�C���̏����������ށB
			EndOrSkipFile(ebzip, operation, sourceFile, destinationFile, sourceSize,
				destinationSize, "�����O���瑶�݂���t�@�C��");
		}

		/// <summary>
		/// EndFile(), SkipFile() �ɋ��ʂ̓�������
		/// </summary>
		private void EndOrSkipFile(EBZip ebzip, EBZipFileOperation operation, 
			string sourceFile, string destinationFile, uint sourceSize,
			// Rewrite �̂Ƃ��������ʏ����B
			uint destinationSize, string comment) {
			if (operation == EBZipFileOperation.Rewrite) {
				this.progressBarFile.Value = 100;
				this.Refresh();
				return;
			}

			// �E�B���h�E�\���p�ɁA�t�@�C���̃p�X��Z������B
			// (���Ђ̃g�b�v�f�B���N�g������̑��Ε\�L�ɂ���B)
			if (destinationFile.StartsWith(ebzip.DestinationPath))
				destinationFile = destinationFile.Substring(ebzip.DestinationPath.Length);
			if (destinationFile.StartsWith("\\"))
				destinationFile = destinationFile.Substring(1);

			// �i�s�󋵂��v�Z�������B
			if (sourceSize > this.fileUnzipOffset) {
				this.totalUnzipOffset += sourceSize - this.fileUnzipOffset;
				this.fileUnzipOffset = sourceSize;
			}

			double filePercent = 1.0;
			double totalPercent = 
				(double) this.totalUnzipOffset / (double) this.totalUnzipSize;
			if (totalPercent > 1.0)
				totalPercent = 1.0;

			double zipRatio;
			if (sourceSize != 0)
				zipRatio = (double)destinationSize / (double)sourceSize;
			else
				zipRatio = Single.PositiveInfinity;

			// �v���O���X�o�[�̕\�����X�V����B
			this.labelFile2.Text = string.Format("{0} / {1}", 
				sourceSize.ToString("n0"), sourceSize.ToString("n0"));
			this.labelFile3.Text = filePercent.ToString("p1");
			this.progressBarFile.Value = (int)(filePercent * 100.0);

			this.labelTotal2.Text = string.Format("{0} / {1}",
				this.totalUnzipOffset.ToString("n0"),
				this.totalUnzipSize.ToString("n0"));
			this.labelTotal3.Text = totalPercent.ToString("p1");
			this.progressBarTotal.Value = (int)(totalPercent * 100.0);

			// ���X�g�r���[�ɂ��̃t�@�C����ǉ�����B
			ListViewItem item = new ListViewItem(destinationFile);
			item.SubItems.Add(sourceSize.ToString("n0"));
			item.SubItems.Add(destinationSize.ToString("n0"));
			item.SubItems.Add(zipRatio.ToString("p1"));
			item.SubItems.Add(comment);
			this.listViewFileList.Items.Add(item);

			this.Text = string.Format("EBWinZip ({0})", totalPercent.ToString("p1")); 
			this.Refresh();
		}

		/// <summary>
		/// �o�͐�̃t�@�C�������ɑ��݂����ۂɎ��s�����f���Q�[�g
		/// </summary>
		public bool ConfirmOverwrite(EBZip ebzip, EBZipFileOperation operation,
			string sourceFile, string destinationFile) {
			if (this.forceOverwrite)
				return true;

			FormOverwrite formOverwrite = new FormOverwrite(destinationFile);
			formOverwrite.Owner = this;
			formOverwrite.ShowDialog();
			if (formOverwrite.DialogResult == DialogResult.OK)
				this.forceOverwrite = true;
			else if (formOverwrite.DialogResult == DialogResult.Abort) {				
				this.Close();
				this.AbortCommand();
			}

			return (formOverwrite.DialogResult == DialogResult.Yes
				|| formOverwrite.DialogResult == DialogResult.OK);
		}

		/// <summary>
		/// ���̃t�@�C�����폜�ł��Ȃ������Ƃ��Ɏ��s�����f���Q�[�g
		/// </summary>
		private void DeleteFileError(EBZip ebzip, EBZipFileOperation operation,
			string sourceFile) {
			int nItems = this.listViewFileList.Items.Count;
			string comment = this.listViewFileList.Items[nItems - 1].SubItems[4].Text;
			if (comment != "")
				comment += "\n";
			comment += "���̃t�@�C�����폜�ł��܂���ł����B";
			this.listViewFileList.Items[nItems - 1].SubItems[4].Text = comment;
			this.Refresh();
		}
	}
}

// Local Variables:
// tab-width: 4
