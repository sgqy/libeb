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
	/// ファイルの上書き方針の一覧
	/// </summary>
	public enum EBZipOverwriteMode {
		Confirm, Force, Skip
	}


	/// <summary>
	/// 書籍内の各ファイルの種類
	/// </summary>
	public class EBZipFileCategory {
		// ID 番号
		private int id;

		// 日本語の名称の一覧
		private static string[] categoryNames = new string[7] {
			"カタログ",
			"メッセージ",
			"本文",
			"画像",
			"音声",
			"外字",
			"動画"
		};

		// 種類が不明なときの日本語の名称
		private const string unknownName = "不明";

		// 既存のファイル種別
		public static EBZipFileCategory Catalog  = new EBZipFileCategory(0);
		public static EBZipFileCategory Language = new EBZipFileCategory(1);
		public static EBZipFileCategory Text     = new EBZipFileCategory(2);
		public static EBZipFileCategory Graphic  = new EBZipFileCategory(3);
		public static EBZipFileCategory Sound    = new EBZipFileCategory(4);
		public static EBZipFileCategory Font     = new EBZipFileCategory(5);
		public static EBZipFileCategory Movie    = new EBZipFileCategory(6);
		public static EBZipFileCategory Unknown  = new EBZipFileCategory(-1);

		// ファイルの種別判定用の正規表現
		private static Regex regexGraphic  = new Regex("\\\\data\\\\honmong(\\.[a-z0-9]+)?(;1)?$");
		private static Regex regexSound    = new Regex("\\\\data\\\\honmons(\\.[a-z0-9]+)?(;1)?$");
		private static Regex regexFont     = new Regex("\\\\gaiji\\\\[^\\\\]+$");
		private static Regex regexMovie    = new Regex("\\\\movie\\\\[^\\\\]+$");
		private static Regex regexText1    = new Regex("\\\\data\\\\honmon2?(\\.[a-z0-9]+)?(;1)?$");
		private static Regex regexText2    = new Regex("\\\\start(\\.[a-z0-9]+)?(;1)?$");
		private static Regex regexCatalog  = new Regex("^catalogs?(\\.[a-z0-9]+)?(;1)?$");
		private static Regex regexLanguage = new Regex("language(\\.[a-z0-9]+)?(;1)?$");

		/// <summary>
		/// 非公開のコンストラクタ。
		/// ID (整数値) をセットする。
		/// ファイルの種別を表す static オブジェクト Catalog, Language, ... を初期化
		/// するために使用。
		/// </summary>
		/// <param name="idArg"></param>
		private EBZipFileCategory(int idArg) {
			this.id = idArg;
		}

		/// <summary>
		/// ID (整数値) を返す。
		/// </summary>
		public int Id {
			get {
				return id;
			}
		}

		/// <summary>
		/// ファイル名に応じて、ファイルの種別を表す static オブジェクト Catalog,
		/// Language, ... を返す。
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
		public static EBZipFileCategory Category(string file) {
			string lowerFile = file.ToLower(new System.Globalization.CultureInfo("en-US"));

			if (regexGraphic.IsMatch(lowerFile))
				return EBZipFileCategory.Graphic;
			else if (regexSound.IsMatch(lowerFile))
				return EBZipFileCategory.Sound;
			else if (regexFont.IsMatch(lowerFile))
				return EBZipFileCategory.Font;
			else if (regexMovie.IsMatch(lowerFile))
				return EBZipFileCategory.Movie;
			else if (regexText1.IsMatch(lowerFile) || regexText2.IsMatch(lowerFile))
				return EBZipFileCategory.Text;
			else if (regexCatalog.IsMatch(lowerFile))
				return EBZipFileCategory.Catalog;
			else if (regexLanguage.IsMatch(lowerFile))
				return EBZipFileCategory.Language;

			return EBZipFileCategory.Unknown;
		}

		/// <summary>
		/// 日本語の名称 (文字列) を返す。
		/// </summary>
		/// <param name="category"></param>
		/// <returns></returns>
		public new string ToString() {
			if (this.id < EBZipFileCategory.Catalog.id
				|| this.id > EBZipFileCategory.Movie.id)
				return unknownName;
			return string.Copy(categoryNames[this.id]);
		}
	}


	/// <summary>
	/// 辞書内の各ファイルを表現するための構造体。
	/// </summary>
	public struct EBZipFile {
		// ファイル名
		private string name;

		// 圧縮形式 ("ebzip 圧縮", "EPWING 圧縮" など)
		private string zipMethod;

		// ファイルの分類
		public EBZipFileCategory Category;

		// 非圧縮時のサイズ
		public uint UnzipSize;

		// 現在のサイズ
		public uint CurrentSize;

		// 圧縮形式が不明な場合に zipMethod にセットする文字列
		private const string unknownZipMethod = "不明";

		/// <summary>
		/// ファイル名を nameArg にセットするコンストラクタ。
		/// </summary>
		/// <param name="nameArg"></param>
		public EBZipFile(string nameArg) {
			this.name = string.Copy(nameArg);
			this.Category = EBZipFileCategory.Category(nameArg);
			this.UnzipSize = 0;
			this.CurrentSize = 0;
			this.zipMethod = string.Copy(unknownZipMethod);
		}

		/// <summary>
		/// ファイル名を返す/セットするプロパティ。
		/// </summary>
		public string Name {
			get {
				return name;
			}
			set {
				if (value == null)
					throw new ArgumentException("内部エラー (EBZipFile.Name)");
				this.name = string.Copy(value);
			}
		}

		/// <summary>
		/// 圧縮形式を返す/セットするプロパティ。
		/// </summary>
		public string ZipMethod {
			get {
				return zipMethod;
			}
			set {
				this.zipMethod = string.Copy(value);
			}
		}

		/// <summary>
		/// 現在の圧縮率 (現在のサイズ / 非圧縮時のサイズ) を返す。
		/// ただし、非圧縮時のサイズが 0 のときは +∞ を返す。
		/// </summary>
		public double ZipRatio() {
			if (this.UnzipSize == 0)
				return Single.PositiveInfinity;
			else
				return (double)CurrentSize / (double)UnzipSize;
		}

		/// <summary>
		/// EBZipFile オブジェクトの複製を作る。
		/// </summary>
		/// <param name="zipFile">複製元のオブジェクト</param>
		/// <returns></returns>
		public static EBZipFile Copy(EBZipFile zipFile) {
			EBZipFile result = new EBZipFile();

			result.name        = string.Copy(zipFile.name);
			result.zipMethod   = string.Copy(zipFile.zipMethod);
			result.Category    = zipFile.Category;
			result.UnzipSize   = zipFile.UnzipSize;
			result.CurrentSize = zipFile.CurrentSize;
			return result;
		}
	}

	/// <summary>
	/// EBZipFile のコレクションクラス
	/// </summary>
	public class EBZipFileCollection : CollectionBase {
		public EBZipFile this[int index] {
			get {
				return (EBZipFile) List[index];
			}
			set {
				List[index] = value;
			}
		}

		public int Add(EBZipFile file) {
			return List.Add(file);
		}

		public int IndexOf(EBZipFile file) {
			return List.IndexOf(file);
		}

		public void Insert(int index, EBZipFile file) {
			List.Insert(index, file);
		}

		public void Remove(EBZipFile file) {
			List.Remove(file);
		}

		public bool Contains(EBZipFile file) {
			return List.Contains(file);
		}
	}

	/// <summary>
	/// ebzip.exe --information (= ebzipinfo.exe) を実行するクラス。
	/// </summary>
	public class EBZipInfo : EBCommand {
		// "ebzip.exe" のデフォルトのパス
		private const string defaultCommandPath = "ebzip.exe";

		// 圧縮情報を取得する書籍へのパス
		private string bookPath;

		// "ebzip.exe --information" の実行によって取得した、ファイルの一覧
		private EBZipFileCollection files;

		/// <summary>
		/// デフォルトのコンストラクタ
		/// </summary>
		public EBZipInfo()
			: base(defaultCommandPath) {
			Initialize();
		}

		/// <summary>
		/// コンストラクタ。
		/// ebzip.exe のパスを ebzipPathArg にセットする。
		/// </summary>
		/// <param name="ebzipPathArg">ebzip.exe のパス</param>
		public EBZipInfo(string commandPathArg)
			: base(commandPathArg) {
			Initialize();
		}

		/// <summary>
		/// 各コンストラクタ共有の初期化メソッド
		/// </summary>
		private void Initialize() {
			this.files = new EBZipFileCollection();
			this.bookPath = ".";
		}

		/// <summary>
		/// 書籍のパスの取得および設定を行う。
		/// </summary>
		public string BookPath {
			get {
				return string.Copy(this.bookPath);
			}
			set {
				if (value == null)
					throw new ArgumentException("内部エラー (EBInfo.BookPath)");
				this.bookPath = EBCommand.NormalizePath(value);
				this.files.Clear();
			}
		}

		/// <summary>
		/// "ebzip.exe --information" の実行によって取得した、ファイルの情報の一覧
		/// を返す。
		/// </summary>
		public EBZipFileCollection Files {
			get {
				EBZipFileCollection result = new EBZipFileCollection();
				foreach (EBZipFile file in this.files)
					result.Add(EBZipFile.Copy(file));
				return result;
			}
		}

		/// <summary>
		/// "ebzip.exe --information" を実行し、bookPath にある書籍の圧縮情報を
		/// 取得する。
		/// </summary>
		public override void Execute() {
			Regex regex1 = new Regex("^==> (.+) <==$");
			Regex regex2 = new Regex("^([0-9]+) バイト \\((.+)\\)$");
			Regex regex3 = new Regex("^([0-9]+) -> ([0-9]+) バイト \\(.*%, (.+)\\)");
			int nFiles = 0;

			// ebzip.exe を実行。
			Process process = new Process();
			process.StartInfo.FileName  = this.commandPath;
			process.StartInfo.Arguments = String.Format("--information \"{0}\"",
				this.bookPath.Replace('\\', '/'));
			process.StartInfo.CreateNoWindow         = true;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.RedirectStandardError  = true;
			process.StartInfo.UseShellExecute        = false;
			Console.WriteLine("{0}\n", process.StartInfo.Arguments);

			try {
				process.Start();
			}
			catch (Win32Exception e) {
				string message = String.Format("{0} を実行できませんでした\n{1}",
					this.commandPath, e.Message);
				throw new EBCommandException(message, e);
			}

			try {
				// ebzip.exe が標準出力へ出力したメッセージを読み取る。
				for (;;) {
					string line = process.StandardOutput.ReadLine();
					if (line == null)
						break;

					Match match1 = regex1.Match(line);
					Match match2 = regex2.Match(line);
					Match match3 = regex3.Match(line);

					if (match1.Success) {
						string name = match1.Groups[1].ToString();
						if (name.StartsWith(this.bookPath))
							name = name.Substring(this.bookPath.Length);
						if (name.StartsWith("\\"))
							name = name.Substring(1);

						EBZipFile file = new EBZipFile(name);
						this.files.Add(file);
						nFiles++;
					}
					else if (match2.Success && nFiles > 0) {
						EBZipFile file = this.files[nFiles - 1];
						file.UnzipSize = uint.Parse(match2.Groups[1].ToString());
						file.CurrentSize = file.UnzipSize;
						file.ZipMethod   = match2.Groups[2].ToString();
						this.files[nFiles - 1] = file;
					}
					else if (match3.Success && nFiles > 0) {
						EBZipFile file = this.files[nFiles - 1];
						file.UnzipSize = uint.Parse(match3.Groups[1].ToString());
						file.CurrentSize = uint.Parse(match3.Groups[2].ToString());
						file.ZipMethod   = match3.Groups[3].ToString();
						this.files[nFiles - 1] = file;
					}
				}
			}
			catch (Win32Exception e) {
				string message = String.Format("{0} の出力を読み込めませんでせした\n{1}", 
					this.commandPath, e.Message);
				process.Close();
				throw new EBCommandException(message, e);
			}

			// ebzip.exe が正常終了したかどうか判定。
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

			// ebzip.exe を後始末。
			process.Close();
		}
	}

	/// <summary>
	/// ebzip が書籍に対して行う処理の種別
	/// </summary>
	public enum EBZipBookOperation {
		Zip, Unzip
	}

	/// <summary>
	/// ebzip が各ファイルに対して行う処理の種別
	/// </summary>
	public enum EBZipFileOperation {
		Zip, Unzip, Copy, Rewrite
	}

	/// <summary>
	/// 書籍の圧縮/伸長を開始する際に発生させるイベントのためのデリゲート
	/// </summary>
	public delegate void StartBook(EBZip ebzip, string sourcePath,
		string destinationPath);

	/// <summary>
	/// 書籍の圧縮/伸長を完了した際に発生させるイベントのためのデリゲート
	/// </summary>
	public delegate void EndBook(EBZip ebzip, string sourcePath,
		string destinationPath);

	/// <summary>
	/// ファイルの処理を開始する際に発生させるイベントのためのデリゲート
	/// </summary>
	public delegate void StartFile(EBZip ebzip, EBZipFileOperation operation,
		string sourceFile, string destinationFile);

	/// <summary>
	/// ファイルの処理の進行状況を報告するイベントのためのデリゲート
	/// </summary>
	public delegate void ProgressFile(EBZip ebzip, EBZipFileOperation operation,
		string sourceFile, string destinationFile, uint currentOffset,
		uint sourceSize);

	/// <summary>
	/// ファイルの処理を完了した際に発生させるイベントのためのデリゲート
	/// </summary>
	public delegate void EndFile(EBZip ebzip, EBZipFileOperation operation, 
		string sourceFile, string destinationFile, uint sourceSize,
		uint destinationSize);

	/// <summary>
	/// ファイルの処理をスキップする際に発生させるイベントのためのデリゲート
	/// </summary>
	public delegate void SkipFile(EBZip ebzip, EBZipFileOperation operation,
		string sourceFile, string destinationFile);

	/// <summary>
	/// 出力先のファイルの上書き確認を行う際に発生させるイベントのための
	/// デリゲート
	/// </summary>
	public delegate bool ConfirmOverwrite(EBZip ebzip, EBZipFileOperation operation,
		string sourceFile, string destinationFile);

	/// <summary>
	/// ファイル処理中の警告メッセージを伝える際に発生させるイベントのための
	/// デリゲート
	/// </summary>
	public delegate void DeleteFileError(EBZip ebzip, EBZipFileOperation operation,
		string sourceFile);

	/// <summary>
	/// エラーが起きて、処理を中止する際に発生させるイベントのためのデリゲート
	/// </summary>
	public delegate void Abort(EBZip ebzip, string message);

	/// <summary>
	/// "ebzip.exe --compress" (= ebzip.exe) もしくは
	/// "ebzip.exe --uncompress" (= ebunzip.exe) を実行するクラス。
	/// </summary>
	public class EBZip : EBCommand {
		// 処理の種別 (圧縮/伸長)
		private EBZipBookOperation operation;

		// ebzip.exe のデフォルトのパス
		public const string DefaultCommandPath = "ebzip.exe";

		// ebzip.exe で圧縮/伸長する書籍のパス
		private string sourcePath;

		// 圧縮/伸長したデータの書き込み先のパス
		private string destinationPath;

		// 圧縮/伸長する副本の一覧
		private EBSubbookCollection subbooks;

		// ファイルの上書き方針
		public EBZipOverwriteMode OverwriteMode;

		// 圧縮レベル
		private int zipLevel;

		public const int MinZipLevel     = 0;
		public const int MaxZipLevel     = 5;

		// 画像、音声、外字、動画ファイルを無視するかどうか
		public bool SkipGraphic;
		public bool SkipSound;
		public bool SkipFont;
		public bool SkipMovie;

		// 圧縮/伸長した後、元のファイルを削除するかどうか
		public bool KeepSource;

		// テストモード
		public bool TestMode;

		// 各種イベント
		public event StartBook        StartBookEvent;
		public event EndBook          EndBookEvent;
		public event StartFile        StartFileEvent;
		public event ProgressFile     ProgressFileEvent;
		public event EndFile          EndFileEvent;
		public event SkipFile         SkipFileEvent;
		public event ConfirmOverwrite ConfirmOverwriteEvent;
		public event DeleteFileError  DeleteFileErrorEvent;
		public event Abort            AbortEvent;

		/// <summary>
		/// デフォルトのコンストラクタ。
		/// </summary>
		public EBZip()
			: base(DefaultCommandPath) {
			Initialize(EBZipBookOperation.Zip);
		}

		/// <summary>
		/// コンストラクタ。
		/// </summary>
		/// <param name="operationArg">処理の種別 (圧縮/伸長)</param>
		public EBZip(EBZipBookOperation operationArg)
			: base(DefaultCommandPath) {
			Initialize(operationArg);
		}

		/// <summary>
		/// コンストラクタ。
		/// </summary>
		/// <param name="operationArg">処理の種別 (圧縮/伸長)</param>
		/// <param name="commandPathArg">ebzip.exe のパス</param>
		public EBZip(EBZipBookOperation operationArg, string commandPathArg)
			: base(commandPathArg) {
			Initialize(operationArg);
		}

		/// <summary>
		/// 各コンストラクタ共通の初期化処理メソッド
		/// </summary>
		/// <param name="operation">処理の種別 (圧縮/伸長)</param>
		private void Initialize(EBZipBookOperation operationArg) {
			this.operation       = operationArg;
			this.sourcePath      = ".";
			this.destinationPath = ".";
			this.subbooks        = null;
			this.OverwriteMode   = EBZipOverwriteMode.Confirm;
			this.zipLevel        = MinZipLevel;
			this.SkipGraphic     = false;
			this.SkipSound       = false;
			this.SkipFont        = false;
			this.SkipMovie       = false;
			this.KeepSource      = false;
			this.TestMode        = false;
		}

		/// <summary>
		/// 処理種別 (圧縮/伸長) の取得
		/// </summary>
		public EBZipBookOperation Operation {
			get {
				return this.operation;
			}
			set {
				this.operation = value;
			}
		}

		/// <summary>
		/// ebzip.exe で圧縮/伸長する書籍のパスの取得と設定
		/// </summary>
		public string SourcePath {
			get {
				return string.Copy(this.sourcePath);
			}
			set {
				if (value == null)
					throw new ArgumentException("内部エラー (EBZip.SourcePath)");
				this.sourcePath = string.Copy(value);
			}
		}

		/// <summary>
		/// ebzip.exe で圧縮/伸長したデータの書き込み先のパスの取得と設定
		/// </summary>
		public string DestinationPath {
			get {
				return string.Copy(this.destinationPath);
			}
			set {
				if (value == null)
					throw new ArgumentException("内部エラー (EBZip.SourcePath)");
				this.destinationPath = string.Copy(value);
			}
		}

		/// <summary>
		/// ebzip.exe で圧縮/伸長する複本の一覧の取得や設定
		/// </summary>
		public EBSubbookCollection Subbooks {
			get {
				if (this.subbooks == null)
					return null;
				else {
					EBSubbookCollection result = new EBSubbookCollection();
					foreach (EBSubbook subbook in this.subbooks)
						result.Add(EBSubbook.Copy(subbook));
					return result;
				}
			}
			set {
				if (value == null)
					this.subbooks = null;
				else {
					this.subbooks = new EBSubbookCollection();
					foreach (EBSubbook subbook in value)
						this.subbooks.Add(subbook);
				}
			}
		}

		/// <summary>
		/// 圧縮レベルの取得と設定
		/// </summary>
		public int ZipLevel {
			get {
				return zipLevel;
			}
			set {
				if (value < MinZipLevel || value > MaxZipLevel)
					throw new ArgumentException("内部エラー (EBZip.ZipLevel)");
				zipLevel = value;
			}
		}

		/// <summary>
		/// ebzip.exe へ渡すべきコマンド行引数を返す。
		/// </summary>
		/// <returns></returns>
		private string CommandArguments() {
			string arguments = "";

			// 圧縮/伸長オプション
			if (this.operation == EBZipBookOperation.Zip)
				arguments += "--compress";
			else
				arguments += "--uncompress";

			// --output-directory オプション
			arguments += string.Format(" --output-directory \"{0}\"",
				this.destinationPath.Replace('\\', '/'));

			// --level オプション
			arguments += " --level " + this.zipLevel;

			// --subbook オプション
			if (this.subbooks != null) {
				foreach (EBSubbook subbook in this.subbooks)
					arguments += " --subbook " + subbook.Directory;
			}

			// --skip-content オプション
			if (this.SkipFont)
				arguments += " --skip-content font";
			if (this.SkipGraphic)
				arguments += " --skip-content graphic";
			if (this.SkipSound)
				arguments += " --skip-content sound";
			if (this.SkipMovie)
				arguments += " --skip-content movie";

			// --overwrite オプション
			if (this.OverwriteMode == EBZipOverwriteMode.Force)
				arguments += " --overwrite force";
			else if (this.OverwriteMode == EBZipOverwriteMode.Skip)
				arguments += " --overwrite no";
			else
				arguments += " --overwrite confirm";

			// --keep オプション
			if (this.KeepSource)
				arguments += " --keep";

			// --test オプション
			if (this.TestMode)
				arguments += " --test";

			arguments += string.Format(" \"{0}\"", this.sourcePath.Replace('\\', '/'));

			return arguments;
		}

		/// <summary>
		/// "ebzip.exe" を実行する。
		/// </summary>
		public override void Execute() {
			Regex regex1 = new Regex("^==> (.+) を(圧縮|伸長|コピー|書き換え) <==$");
			Regex regex2 = new Regex("^(.+) に出力$");
			Regex regex3 = new Regex("^ *[0-9.]+% 処理済み \\(([0-9]+) / ([0-9]+) バイト\\)$");
			Regex regex4 = new Regex("^完了 \\(([0-9]+) / ([0-9]+) バイト\\)$");
			Regex regex5 = new Regex("^([0-9]+) -> ([0-9]+) バイト");
			Regex regex6 = new Regex("^ファイルがすでに存在します: (.+)$");
			Regex regex7 = new Regex("警告: ファイルを削除できませんでした");
			string errorLine = "";

			// ebzip.exe を実行。
			Process process = new Process();
			process.StartInfo.FileName  = this.commandPath;
			process.StartInfo.Arguments = CommandArguments();
			process.StartInfo.CreateNoWindow         = true;
			process.StartInfo.RedirectStandardInput  = true;
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

			if (this.StartBookEvent != null) {
				this.StartBookEvent(this, string.Copy(this.SourcePath),
					string.Copy(this.DestinationPath));
			}

			try {
				string sourceFile = "";
				string destinationFile = "";
				EBZipFileOperation operation = EBZipFileOperation.Zip;

				// ebzip.exe が標準出力へ出力したメッセージを読み取る。
				for (;;) {
					string line = process.StandardError.ReadLine();
					if (line == null)
						break;

					Match match1 = regex1.Match(line);
					Match match2 = regex2.Match(line);
					Match match3 = regex3.Match(line);
					Match match4 = regex4.Match(line);
					Match match5 = regex5.Match(line);
					Match match6 = regex6.Match(line);
					Match match7 = regex7.Match(line);

					if (match1.Success) {
						// ファイルの処理を開始
						sourceFile = match1.Groups[1].ToString();
						switch (match1.Groups[2].ToString()) {
							case "圧縮":
								operation = EBZipFileOperation.Zip;
								break;
							case "伸長":
								operation = EBZipFileOperation.Unzip;
								break;
							case "コピー":
								operation = EBZipFileOperation.Copy;
								break;
							case "書き換え":
								operation = EBZipFileOperation.Rewrite;
								break;
							default:
								throw new EBCommandException("正規表現の不整合 (EBZip.Execute)");
						}
					}
					else if (match2.Success) {
						// 書き込み先のファイル名を表示
						destinationFile = match2.Groups[1].ToString();
						if (this.StartFileEvent != null) {
							this.StartFileEvent(this, operation, 
								string.Copy(sourceFile), string.Copy(destinationFile));
						}
					}
					else if (match3.Success) {
						// ファイルの処理状況を表示
						uint offset = uint.Parse(match3.Groups[1].ToString());
						uint size   = uint.Parse(match3.Groups[2].ToString());
						if (this.ProgressFileEvent != null) {
							this.ProgressFileEvent(this, operation,
								string.Copy(sourceFile), string.Copy(destinationFile),
								offset, size);
						}
					}
					else if (match4.Success) {
						// ファイルの処理の完了
						uint size = uint.Parse(match4.Groups[1].ToString());
						if (operation == EBZipFileOperation.Copy && 
							this.EndFileEvent != null) {
							this.EndFileEvent(this, operation,
								string.Copy(sourceFile), string.Copy(destinationFile),
								size, size);
						}
					}
					else if (match5.Success) {
						// ファイルを圧縮/伸長した結果 (圧縮率) を表示
						uint unzipSize = uint.Parse(match5.Groups[1].ToString());
						uint zipSize   = uint.Parse(match5.Groups[2].ToString());
						if (this.EndFileEvent != null) {
							this.EndFileEvent(this, operation,
								string.Copy(sourceFile), string.Copy(destinationFile),
								unzipSize, zipSize);
						}
					}
					else if (match6.Success) {
						bool yes;

						// 出力先のファイルが既に存在する旨を表示
						if (this.ConfirmOverwriteEvent != null &&
							this.ConfirmOverwriteEvent(this, operation,
							string.Copy(sourceFile), string.Copy(destinationFile))) {
							process.StandardInput.WriteLine("y");
							yes = true;
						}
						else {
							process.StandardInput.WriteLine("n");
							yes = false;
						}

						if (!yes && this.SkipFileEvent != null) {
							this.SkipFileEvent(this, operation,
								string.Copy(sourceFile), string.Copy(destinationFile));
						}

						// 上書き確認のメッセージを読み捨てる。
						line = process.StandardError.ReadLine();
					
					}
					else if (match7.Success) {
						if (this.DeleteFileErrorEvent != null) {
							this.DeleteFileErrorEvent(this, operation,
								string.Copy(sourceFile));
						}
					}
					else if (line == "このファイルはすでに存在するので、処理しません" ||
						line == "入力と出力ファイルが同一なので、処理しません") {
						// ファイルの処理をスキップする旨を表示
						if (this.SkipFileEvent != null) {
							this.SkipFileEvent(this, operation,
								string.Copy(sourceFile), string.Copy(destinationFile));
						}
					}
					else if (line != "") {
						if (this.AbortEvent != null)
							this.AbortEvent(this, string.Copy(line));
						if (errorLine == "")
							errorLine = string.Copy(line);
					}
				}
			}
			catch (Win32Exception e) {
				string message = String.Format("{0} の出力を読み込めませんでせした\n{1}", 
					this.commandPath, e.Message);
				process.Close();
				throw new EBCommandException(message, e);
			}

			// ebzip.exe が正常終了したかどうか判定。
			while (!process.HasExited)
				Thread.Sleep(1000);

			if (process.ExitCode != 0) {
				process.Close();
				throw new EBCommandException(errorLine);
			}

			if (this.EndBookEvent != null) {
				this.EndBookEvent(this, string.Copy(this.SourcePath), 
					string.Copy(this.DestinationPath));
			}

			// ebzip.exe を後始末。
			process.Close();
		}
	}
}

// Local Variables:
// tab-width: 4
