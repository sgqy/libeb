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
	public class FormVersion : System.Windows.Forms.Form {
		private System.ComponentModel.IContainer components;

		private System.Windows.Forms.Label labelVersion;
		private System.Windows.Forms.Label labelCopyright;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Label labelIcon;

		/// <summary>
		/// �R���X�g���N�^
		/// </summary>
		public FormVersion() {
			//
			// Windows �t�H�[�� �f�U�C�i �T�|�[�g�ɕK�v�ł��B
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent �Ăяo���̌�ɁA�R���X�g���N�^ �R�[�h��ǉ����Ă��������B
			//
			this.labelVersion.Text = string.Format("WinEBZip �o�[�W���� {0}", Version);
			this.labelCopyright.Text = Copyright;
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

		// WinEBZip �̒��쌠�\��
		public const string Copyright = "Copyright (C) 2004  �}�� ��V";

		// WinEBZip �̃o�[�W����
		public const string Version = "0.0";

		#region Windows �t�H�[�� �f�U�C�i�Ő������ꂽ�R�[�h 
		/// <summary>
		/// �f�U�C�i �T�|�[�g�ɕK�v�ȃ��\�b�h�ł��B���̃��\�b�h�̓��e��
		/// �R�[�h �G�f�B�^�ŕύX���Ȃ��ł��������B
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FormVersion));
			this.labelVersion = new System.Windows.Forms.Label();
			this.buttonOK = new System.Windows.Forms.Button();
			this.labelCopyright = new System.Windows.Forms.Label();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.labelIcon = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// labelVersion
			// 
			this.labelVersion.Location = new System.Drawing.Point(64, 8);
			this.labelVersion.Name = "labelVersion";
			this.labelVersion.Size = new System.Drawing.Size(216, 16);
			this.labelVersion.TabIndex = 2;
			this.labelVersion.Text = "WinEBZip �o�[�W����";
			this.labelVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.labelVersion.UseMnemonic = false;
			// 
			// buttonOK
			// 
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new System.Drawing.Point(96, 64);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(96, 24);
			this.buttonOK.TabIndex = 0;
			this.buttonOK.Text = "OK";
			this.buttonOK.Click += new System.EventHandler(this.ClickButtonaOK);
			// 
			// labelCopyright
			// 
			this.labelCopyright.Location = new System.Drawing.Point(64, 32);
			this.labelCopyright.Name = "labelCopyright";
			this.labelCopyright.Size = new System.Drawing.Size(216, 16);
			this.labelCopyright.TabIndex = 3;
			this.labelCopyright.Text = "���쌠�\��";
			this.labelCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.labelCopyright.UseMnemonic = false;
			// 
			// imageList1
			// 
			this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// labelIcon
			// 
			this.labelIcon.Image = ((System.Drawing.Image)(resources.GetObject("labelIcon.Image")));
			this.labelIcon.Location = new System.Drawing.Point(8, 8);
			this.labelIcon.Name = "labelIcon";
			this.labelIcon.Size = new System.Drawing.Size(48, 48);
			this.labelIcon.TabIndex = 1;
			// 
			// FormVersion
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.CancelButton = this.buttonOK;
			this.ClientSize = new System.Drawing.Size(288, 90);
			this.ControlBox = false;
			this.Controls.Add(this.labelIcon);
			this.Controls.Add(this.labelCopyright);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.labelVersion);
			this.Name = "FormVersion";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "WinEBZip (�o�[�W�������)";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// OK �{�^���������ꂽ�Ƃ��̃C�x���g
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ClickButtonaOK(object sender, System.EventArgs e) {
			this.Close();
		}
	}
}

// Local Variables:
// tab-width: 4
