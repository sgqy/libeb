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
	/// Form1 の概要の説明です。
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
		/// デフォルトのコンストラクタ
		/// </summary>
		public FormOverwrite()
		{
			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent 呼び出しの後に、コンストラクタ コードを追加してください。
			//
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="fileName">上書き確認するファイル名</param>
		public FormOverwrite(string fileName) {
			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent 呼び出しの後に、コンストラクタ コードを追加してください。
			//
			this.labelFileName.Text = string.Copy(fileName);
		}

		/// <summary>
		/// 使用されているリソースに後処理を実行します。
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

		#region Windows フォーム デザイナで生成されたコード 
		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
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
			this.buttonYes.Text = "はい(&Y)";
			// 
			// buttonAll
			// 
			this.buttonAll.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonAll.Location = new System.Drawing.Point(120, 104);
			this.buttonAll.Name = "buttonAll";
			this.buttonAll.Size = new System.Drawing.Size(104, 24);
			this.buttonAll.TabIndex = 1;
			this.buttonAll.Text = "すべて上書き(&A)";
			// 
			// buttonNo
			// 
			this.buttonNo.DialogResult = System.Windows.Forms.DialogResult.No;
			this.buttonNo.Location = new System.Drawing.Point(232, 104);
			this.buttonNo.Name = "buttonNo";
			this.buttonNo.Size = new System.Drawing.Size(104, 24);
			this.buttonNo.TabIndex = 2;
			this.buttonNo.Text = "いいえ(&N)";
			// 
			// labelNotice
			// 
			this.labelNotice.Location = new System.Drawing.Point(96, 16);
			this.labelNotice.Name = "labelNotice";
			this.labelNotice.Size = new System.Drawing.Size(256, 16);
			this.labelNotice.TabIndex = 3;
			this.labelNotice.Text = "このファイルは既に存在しています。上書きしますか?";
			// 
			// buttonAbort
			// 
			this.buttonAbort.DialogResult = System.Windows.Forms.DialogResult.Abort;
			this.buttonAbort.Location = new System.Drawing.Point(344, 104);
			this.buttonAbort.Name = "buttonAbort";
			this.buttonAbort.Size = new System.Drawing.Size(104, 24);
			this.buttonAbort.TabIndex = 4;
			this.buttonAbort.Text = "中止";
			// 
			// labelFileName
			// 
			this.labelFileName.Location = new System.Drawing.Point(24, 48);
			this.labelFileName.Name = "labelFileName";
			this.labelFileName.Size = new System.Drawing.Size(408, 32);
			this.labelFileName.TabIndex = 5;
			this.labelFileName.Text = "ファイル名";
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
			this.Text = "WinEBZip (上書きの確認)";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// 上書き確認するファイル名を取得もしくは設定する。
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
