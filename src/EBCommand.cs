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

namespace WinEBZip {
	/// <summary>
	/// EBZip, EBUnzip, EBZipInfo クラス内で発生した、コマンド実行エラーに伴う
	///	例外を扱うクラス。
	/// </summary>
	public class EBCommandException
		: System.ApplicationException {
		public EBCommandException()
			: base() {
		}

		public EBCommandException(string message)
			: base(message) {
		}

		public EBCommandException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}


	/// <summary>
	/// eb*.exe コマンドの実行オブジェクトを表現するためのクラス。
	/// EBZip, EBUnzip, EBZipInfo, EBInfo の基底クラス。
	/// </summary>
	public abstract class EBCommand {
		// "ebzip.exe" へのパス
		protected string commandPath;

		/// <summary>
		/// コンストラクタ。
		/// コマンドのパスをセットする。
		/// </summary>
		/// <param name="commandPathArg"></param>
		public EBCommand(string commandPathArg) {
			this.commandPath = string.Copy(commandPathArg);
		}

		/// <summary>
		/// コマンドのパスを返す/値をセットするプロパティ。
		/// </summary>
		public string CommandPath {
			get {
				return commandPath;
			}
			set {
				this.commandPath = string.Copy(value);
			}
		}

		/// <summary>
		/// コマンドを実行する。
		/// </summary>
		public abstract void Execute();

		/// <summary>
		/// 文字が ASCII のアルファベット ('A' ～ 'Z', 'a' ～ 'z') かどうかを判定する。
		/// </summary>
		/// <param name="c">判定する文字</param>
		/// <returns></returns>
		private static bool IsASCIIAlphabet(char c) {
			return ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z')); 
		}

		/// <summary>
		/// パスを正規化する。
		/// 相対パスは絶対パスに直し、ドライブ文字がなければ補う。
		/// </summary>
		/// <param name="path">正規化するパス</param>
		/// <returns></returns>
		protected static string NormalizePath(string path) {
			string cwd = Environment.CurrentDirectory;
			string result;

			if (path.Length == 0)
				result = ".";
			else {
				result = string.Copy(path);
				result.Replace("/", "\\");
			}

			if (result.Length >= 2 && result[0] == '\\' && result[1] == '\\') {
				// `path' は UNC パス。
				// 何もしない
			}
			else if (result.Length >= 2 && IsASCIIAlphabet(result[0]) && result[1] == ':') {
				if (result.Length <= 2 || result[2] != '\\') {
					// `path' は相対パスで、ドライブ文字が含まれている。
					// 絶対パスへ変換する。
					string drive = result.Substring(0, 2);
					Environment.CurrentDirectory = drive;
					string dcwd = Environment.CurrentDirectory;
					Environment.CurrentDirectory = cwd;
					result = string.Format("{0}\\{1}", dcwd, result.Substring(2));
				}
			}
			else if (result.Length >= 1 && result[0] == '\\') {
				// `path' は絶対パスだが、ドライブ文字が無い。
				// ドライブ文字を挿入する。
				result = string.Format("{0}\\{1}", cwd.Substring(0, 2), result);
			}
			else {
				// `path' はドライブ文字がなく、相対パスである。
				// ドライブ文字を挿入し、絶対パスに変換する。
				result = string.Format("{0}\\{1}", cwd, result);
			}

			result = result.TrimEnd(new char[1] {'\\'});
			if (result.IndexOf('\\') < 0)
				result += '\\';
			return result;
		}
	}
}

// Local Variables:
// tab-width: 4
