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
using System.Text.RegularExpressions;

namespace WinEBZip {
	/// <summary>
	/// 書籍内の各副本を表現するための構造体。
	/// </summary>
	public struct EBSubbook {
		// 副本のサブディレクトリ名
		private string directory;

		// 副本の題名
		private string title;

		/// <summary>
		/// 副本のサブディレクトリ名を返す/セットする。
		/// </summary>
		public string Directory	{
			get {
				return directory;
			}
			set {
				directory = string.Copy(value);
			}
		}

		/// <summary>
		/// 副本の題名を返す/セットする。
		/// </summary>
		public string Title	{
			get {
				return title;
			}
			set {
				title = string.Copy(value);
			}
		}

		/// <summary>
		/// EBSubbook オブジェクトの複製を返す。
		/// </summary>
		/// <param name="subbook"></param>
		/// <returns></returns>
		public static EBSubbook Copy(EBSubbook subbook) {
			EBSubbook result = new EBSubbook();

			result.directory = string.Copy(subbook.directory);
			result.title     = string.Copy(subbook.title);
			return result;
		}
	}

	/// <summary>
	/// EBSubbook のコレクションクラス
	/// </summary>
	public class EBSubbookCollection : CollectionBase {
		public EBSubbook this[int index] {
			get {
				return (EBSubbook) List[index];
			}
			set {
				List[index] = value;
			}
		}

		public int Add(EBSubbook subbook) {
			return List.Add(subbook);
		}

		public int IndexOf(EBSubbook subbook) {
			return List.IndexOf(subbook);
		}

		public void Insert(int index, EBSubbook subbook) {
			List.Insert(index, subbook);
		}

		public void Remove(EBSubbook subbook) {
			List.Remove(subbook);
		}

		public bool Contains(EBSubbook subbook) {
			return List.Contains(subbook);
		}
	}

	/// <summary>
	/// ebinfo.exe を実行するクラス。
	/// </summary>
	public class EBInfo : EBCommand {
		// "ebinfo.exe" のデフォルトのパス
		public const string DefaultCommandPath = "ebinfo.exe";

		// 副本の一覧を取得する書籍へのパス
		private string bookPath;

		// "ebinfo.exe" の実行によって取得した、副本の一覧
		private EBSubbookCollection subbooks;

		/// <summary>
		/// デフォルトのコンストラクタ
		/// </summary>
		public EBInfo()
			: base(DefaultCommandPath) {
			this.subbooks = new EBSubbookCollection();
			this.bookPath = ".";
		}

		/// <summary>
		/// コンストラクタ。
		/// ebinfo.exe のパスをセットする。
		/// </summary>
		/// <param name="commandPathArg">ebinfo.exe のパス</param>
		public EBInfo(string commandPathArg)
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
		/// ebinfo.exe の実行によって取得した、複本の一覧を返す。
		/// </summary>
		public EBSubbookCollection Subbooks {
			get {
				EBSubbookCollection result = new EBSubbookCollection();
				foreach (EBSubbook subbook in this.subbooks)
					result.Add(EBSubbook.Copy(subbook));
				return result;
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
					throw new ArgumentException("内部エラー (EBInfo.BookPath)");
				this.bookPath = string.Copy(value);
				this.subbooks.Clear();
			}
		}

		/// <summary>
		/// ebinfo.exe を実行し、書籍内の副本一覧を取得する。
		/// </summary>
		/// <returns></returns>
		public override void Execute() {
			Regex regex1 = new Regex("^  題名: (.+)$");
			Regex regex2 = new Regex("^  ディレクトリ: (.+)$");
			this.subbooks.Clear();

			// ebinfo.exe を実行。
			Process process = new Process();
			process.StartInfo.FileName  = this.commandPath;
			process.StartInfo.Arguments =
				string.Format("\"{0}\"", this.bookPath.Replace('\\', '/'));
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

			try {
				// ebinfo.exe が標準出力へ出力したメッセージを読み取る。
				for (;;) {
					string line = process.StandardOutput.ReadLine();
					if (line == null)
						break;

					Match match1 = regex1.Match(line);
					Match match2 = regex2.Match(line);

					if (match1.Success) {
						string title = match1.Groups[1].ToString();
						EBSubbook subbook = new EBSubbook();
						subbook.Title = title;
						this.subbooks.Add(subbook);
					}
					else if (match2.Success && this.subbooks.Count > 0) {
						int lastIndex = this.subbooks.Count - 1;
						EBSubbook subbook = this.subbooks[lastIndex];
						subbook.Directory = match2.Groups[1].ToString();
						this.subbooks[lastIndex] = subbook;
					}
				}
			}
			catch (Win32Exception e) {
				string message = String.Format("{0} の出力を読み込めませんでせした\n{1}", 
					this.commandPath, e.Message);
				process.Close();
				throw new EBCommandException(message, e);
			}

			// ebinfo.exe が正常終了したかどうか判定。
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

			// ebinfo.exe を後始末。
			process.Close();
		}
	}
}

// Local Variables:
// tab-width: 4
