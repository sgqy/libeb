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
using System.Data;
using System.Collections;
using System.Diagnostics;
using System.ComponentModel;
using System.Threading;

namespace WinEBZip {
	/// <summary>
	/// ebinfo.exe を実行するクラス。
	/// </summary>
	public class EBRefile : EBCommand {
		// "ebrefile.exe" のデフォルトのパス
		public const string DefaultCommandPath = "ebrefile.exe";

		// 副本の一覧を取得する書籍へのパス
		private string bookPath;

		// 書籍内に残す副本の一覧
		private EBSubbookCollection subbooks;

		/// <summary>
		/// デフォルトのコンストラクタ
		/// </summary>
		public EBRefile()
			: base(DefaultCommandPath) {
			this.subbooks = new EBSubbookCollection();
			this.bookPath = ".";
		}

		/// <summary>
		/// コンストラクタ。
		/// ebrefile.exe のパスをセットする。
		/// </summary>
		/// <param name="commandPathArg">ebrefile.exe のパス</param>
		public EBRefile(string commandPathArg)
			: base(commandPathArg) {
			this.Initialize();
		}

		/// <summary>
		/// 各コンストラクタ共通の初期化処理。
		/// </summary>
		private void Initialize() {
			this.subbooks = new EBSubbookCollection();
			this.bookPath = ".";
		}

		/// <summary>
		/// 書籍内に残す複本の取得や設定
		/// </summary>
		public EBSubbookCollection Subbooks {
			get {
				EBSubbookCollection result = new EBSubbookCollection();
				foreach (EBSubbook subbook in this.subbooks)
					result.Add(EBSubbook.Copy(subbook));
				return result;
			}
			set {
				this.subbooks.Clear();
				foreach (EBSubbook subbook in value)
					this.subbooks.Add(subbook);
			}
		}

		/// <summary>
		/// 書籍のパスの取得もしくは設定を行う。
		/// </summary>
		public string BookPath {
			get {
				return string.Copy(this.bookPath);
			}
			set {
				if (value == null)
					throw new ArgumentException("内部エラー (EBRefile.BookPath)");
				this.bookPath = string.Copy(value);
				this.subbooks.Clear();
			}
		}

		/// <summary>
		/// ebrefile.exe を実行する。
		/// </summary>
		/// <returns></returns>
		public override void Execute() {
			// ebrefile.exe を実行。
			Process process = new Process();
			process.StartInfo.FileName  = this.commandPath;
			foreach (EBSubbook subbook in this.subbooks)
				process.StartInfo.Arguments += "--subbook " + subbook.Directory;
			process.StartInfo.Arguments +=
				string.Format(" \"{0}\"", this.bookPath.Replace('\\', '/'));
			process.StartInfo.CreateNoWindow         = true;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.RedirectStandardError  = true;
			process.StartInfo.UseShellExecute        = false;

			try {
				process.Start();
			}
			catch (Win32Exception e) {
				string message = String.Format("{0} を実行できませんでした\n{1}",
					this.commandPath, e.Message);
				throw new EBCommandException(message, e);
			}

			// ebrefile.exe が正常終了したかどうか判定。
			while (!process.HasExited)
				Thread.Sleep(1000);

			if (process.ExitCode != 0) {
				string line;
				try {
					line = process.StandardError.ReadLine();
				}
				catch {
					line = string.Format("{0}: 正常に終了しませんでした。",
						this.commandPath);
				}

				process.Close();
				throw new EBCommandException(line);
			}

			// ebrefile.exe を後始末。
			process.Close();
		}
	}
}

// Local Variables:
// tab-width: 4
