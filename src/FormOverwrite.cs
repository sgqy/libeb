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

namespace WinEBZip
{
	/// <summary>
	/// Form1 �̊T�v�̐����ł��B
	/// </summary>
	public class FormOverwrite : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button buttonYes;
		private System.Windows.Forms.Button buttonAll;
		private System.Windows.Forms.Button buttonNo;
		private System.Windows.Forms.Label labelNotice;
		private System.Windows.Forms.Label labelFileName;
		private System.Windows.Forms.Button buttonAbort;
		private System.ComponentModel.IContainer components;

		/// <summary>
		/// �f�t�H���g�̃R���X�g���N�^
		/// </summary>
		public FormOverwrite()
		{
			//
			// Windows �t�H�[�� �f�U�C�i �T�|�[�g�ɕK�v�ł��B
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent �Ăяo���̌�ɁA�R���X�g���N�^ �R�[�h��ǉ����Ă��������B
			//
		}

		/// <summary>
		/// �R���X�g���N�^
		/// </summary>
		/// <param name="fileName">�㏑���m�F����t�@�C����</param>
		public FormOverwrite(string fileName) {
			//
			// Windows �t�H�[�� �f�U�C�i �T�|�[�g�ɕK�v�ł��B
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent �Ăяo���̌�ɁA�R���X�g���N�^ �R�[�h��ǉ����Ă��������B
			//
			this.labelFileName.Text = string.Copy(fileName);
		}

		/// <summary>
		/// �g�p����Ă��郊�\�[�X�Ɍ㏈�������s���܂��B
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
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
		private void InitializeComponent()
		{
			this.buttonYes = new System.Windows.Forms.Button();
			this.buttonAll = new System.Windows.Forms.Button();
			this.buttonNo = new System.Windows.Forms.Button();
			this.labelNotice = new System.Windows.Forms.Label();
			this.buttonAbort = new System.Windows.Forms.Button();
			this.labelFileName = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// buttonYes
			// 
			this.buttonYes.DialogResult = System.Windows.Forms.DialogResult.Yes;
			this.buttonYes.Location = new System.Drawing.Point(8, 104);
			this.buttonYes.Name = "buttonYes";
			this.buttonYes.Size = new System.Drawing.Size(104, 24);
			this.buttonYes.TabIndex = 0;
			this.buttonYes.Text = "�͂�(&Y)";
			// 
			// buttonAll
			// 
			this.buttonAll.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonAll.Location = new System.Drawing.Point(120, 104);
			this.buttonAll.Name = "buttonAll";
			this.buttonAll.Size = new System.Drawing.Size(104, 24);
			this.buttonAll.TabIndex = 1;
			this.buttonAll.Text = "���ׂď㏑��(&A)";
			// 
			// buttonNo
			// 
			this.buttonNo.DialogResult = System.Windows.Forms.DialogResult.No;
			this.buttonNo.Location = new System.Drawing.Point(232, 104);
			this.buttonNo.Name = "buttonNo";
			this.buttonNo.Size = new System.Drawing.Size(104, 24);
			this.buttonNo.TabIndex = 2;
			this.buttonNo.Text = "������(&N)";
			// 
			// labelNotice
			// 
			this.labelNotice.Location = new System.Drawing.Point(96, 16);
			this.labelNotice.Name = "labelNotice";
			this.labelNotice.Size = new System.Drawing.Size(256, 16);
			this.labelNotice.TabIndex = 3;
			this.labelNotice.Text = "���̃t�@�C���͊��ɑ��݂��Ă��܂��B�㏑�����܂���?";
			// 
			// buttonAbort
			// 
			this.buttonAbort.DialogResult = System.Windows.Forms.DialogResult.Abort;
			this.buttonAbort.Location = new System.Drawing.Point(344, 104);
			this.buttonAbort.Name = "buttonAbort";
			this.buttonAbort.Size = new System.Drawing.Size(104, 24);
			this.buttonAbort.TabIndex = 4;
			this.buttonAbort.Text = "���~";
			// 
			// labelFileName
			// 
			this.labelFileName.Location = new System.Drawing.Point(24, 48);
			this.labelFileName.Name = "labelFileName";
			this.labelFileName.Size = new System.Drawing.Size(408, 32);
			this.labelFileName.TabIndex = 5;
			this.labelFileName.Text = "�t�@�C����";
			this.labelFileName.UseMnemonic = false;
			// 
			// FormOverwrite
			// 
			this.AcceptButton = this.buttonYes;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(456, 138);
			this.ControlBox = false;
			this.Controls.Add(this.labelFileName);
			this.Controls.Add(this.buttonAbort);
			this.Controls.Add(this.labelNotice);
			this.Controls.Add(this.buttonNo);
			this.Controls.Add(this.buttonAll);
			this.Controls.Add(this.buttonYes);
			this.Name = "FormOverwrite";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "WinEBZip (�㏑���̊m�F)";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// �㏑���m�F����t�@�C�������擾�������͐ݒ肷��B
		/// </summary>
		public string FileName {
			get {
				return string.Copy(this.labelFileName.Text);
			}
			set {
				this.labelFileName.Text = value;
			}
		}
	}
}

// Local Variables:
// tab-width: 4
