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
using System.Collections;
using Microsoft.Win32;

namespace WinEBZip
{
	/// <summary>
	/// アプリケーションの設定の保存と読み込み
	/// </summary>
	public class WinEBZipRegistry {
		// レジストリの副キー名
		private const string SubKeyName = "Software\\WinEBZip";

		// EBライブラリコマンドのデフォルトのインストールパス
		private const string DefaultEBCommandPath = "";

		// ドキュメントのデフォルトのインストールパス
		private const string DefaultDocumentPath = "\\Program Files\\WinEBZip\\doc";

		// 「ツールバー表示の有無」のデフォルト値
		private const bool DefaultShowToolBar = true;

		// 全画面で表示するかどうかのデフォルト値
		private const bool DefaultMaximized = false;

		// デフォルトの圧縮レベル
		private const int DefaultZipLevel = 0;

		/// 「外字ファイルをスキップにするかどうか」のデフォルト値
		private const bool DefaultSkipFont = false;

		/// 「画像ファイルをスキップにするかどうか」のデフォルト値
		private const bool DefaultSkipGraphic = false;

		/// 「音声ファイルをスキップにするかどうか」のデフォルト値
		private const bool DefaultSkipSound = false;

		/// 「動画ファイルをスキップにするかどうか」のデフォルト値
		private const bool DefaultSkipMovie = false;

		// レジストリキー
		RegistryKey userRegistryKey;
		RegistryKey machineRegistryKey;

		/// <summary>
		/// デフォルトのコンストラクタ
		/// </summary>
		public WinEBZipRegistry() {
			this.userRegistryKey    = null;
			this.machineRegistryKey = null;

			this.userRegistryKey = Registry.CurrentUser.CreateSubKey(SubKeyName);
			if (this.userRegistryKey == null)
				throw new ApplicationException("レジストリにアクセスできませんでした");

			try {
				this.machineRegistryKey = Registry.LocalMachine.OpenSubKey(SubKeyName);
			}
			catch {
				// 例外は無視する。
			}
		}

		/// <summary>
		/// レジストリをクローズする。
		/// </summary>
		public void Close() {
			if (this.userRegistryKey != null)
				this.userRegistryKey.Close();
			if (this.machineRegistryKey != null)
				this.machineRegistryKey.Close();

			this.userRegistryKey    = null;
			this.machineRegistryKey = null;
		}

		/// <summary>
		/// EBライブラリコマンドのインストールパスを取得する。
		/// </summary>
		public string EBCommandPath {
			get {
				string result = DefaultEBCommandPath;
				try {
					result = (string) this.userRegistryKey.GetValue("EBCommandPath", result);
				}
				catch {
					// 例外は無視する
				}
				return result;
			}
			set {
				try {
					this.userRegistryKey.SetValue("EBCommandPath", value);
				}
				catch {
					// 例外は無視する
				}
			}
		}

		/// <summary>
		/// ドキュメントのインストールパスを取得する。
		/// </summary>
		public string DocumentPath {
			get {
				string result = DefaultDocumentPath;
				try {
					result = (string) this.machineRegistryKey.GetValue("DocumentPath", result);
				}
				catch {
					// 例外は無視する
				}

				return result;
			}
		}

		/// <summary>
		/// ツールバー表示の有無を取得/設定する。
		/// </summary>
		public bool ShowToolBar {
			get {
				int result = BoolToInt(DefaultShowToolBar);
				try {
					result = (int) this.userRegistryKey.GetValue("ShowToolBar", result);
				}
				catch {
					// 例外は無視する
				}
				return (result != 0);
			}
			set {
				try {
					this.userRegistryKey.SetValue("ShowToolBar", BoolToInt(value));
				}
				catch {
					// 例外は無視する
				}
			}
		}

		/// <summary>
		/// 全画面表示にするかどうかを取得/設定する。
		/// </summary>
		public bool Maximized {
			get {
				int result = BoolToInt(DefaultMaximized);
				try {
					result = (int) this.userRegistryKey.GetValue("Maximized", result);
				}
				catch {
					// 例外は無視する
				}
				return (result != 0);
			}
			set {
				try {
					this.userRegistryKey.SetValue("Maximized", BoolToInt(value));
				}
				catch {
					// 例外は無視する
				}
			}
		}

		/// <summary>
		/// 圧縮レベルを取得/設定する。
		/// </summary>
		public int ZipLevel {
			get {
				int result = DefaultZipLevel;
				try {
					result = (int) this.userRegistryKey.GetValue("ZipLevel", result);
				}
				catch {
					// 例外は無視する
				}
				if (result < EBZip.MinZipLevel || result > EBZip.MaxZipLevel)
					return DefaultZipLevel;
				return result;
			}
			set {
				try {
					this.userRegistryKey.SetValue("ZipLevel", value);
				}
				catch {
					// 例外は無視する
				}
			}
		}

		/// <summary>
		/// 外字ファイルをスキップにするかどうかを取得/設定する。
		/// </summary>
		public bool SkipFont {
			get {
				int result = BoolToInt(DefaultSkipFont);
				try {
					result = (int) this.userRegistryKey.GetValue("SkipFont", result);
				}
				catch {
					// 例外は無視する
				}
				return (result != 0);
			}
			set {
				try {
					this.userRegistryKey.SetValue("SkipFont", BoolToInt(value));
				}
				catch {
					// 例外は無視する
				}
			}
		}

		/// <summary>
		/// 画像ファイルをスキップにするかどうかを取得/設定する。
		/// </summary>
		public bool SkipGraphic {
			get {
				int result = BoolToInt(DefaultSkipGraphic);
				try {
					result = (int) this.userRegistryKey.GetValue("SkipGraphic", result);
				}
				catch {
					// 例外は無視する
				}
				return (result != 0);
			}
			set {
				try {
					this.userRegistryKey.SetValue("SkipGraphic", BoolToInt(value));
				}
				catch {
					// 例外は無視する
				}
			}
		}

		/// <summary>
		/// 音声ファイルをスキップにするかどうかを取得/設定する。
		/// </summary>
		public bool SkipSound {
			get {
				int result = BoolToInt(DefaultSkipSound);
				try {
					result = (int) this.userRegistryKey.GetValue("SkipSound", result);
				}
				catch {
					// 例外は無視する
				}
				return (result != 0);
			}
			set {
				try {
					this.userRegistryKey.SetValue("SkipSound", BoolToInt(value));
				}
				catch {
					// 例外は無視する
				}
			}
		}

		/// <summary>
		/// 動画ファイルをスキップにするかどうかを取得/設定する。
		/// </summary>
		public bool SkipMovie {
			get {
				int result = BoolToInt(DefaultSkipMovie);
				try {
					result = (int) this.userRegistryKey.GetValue("SkipMovie", result);
				}
				catch {
					// 例外は無視する
				}
				return (result != 0);
			}
			set {
				try {
					this.userRegistryKey.SetValue("SkipMovie", BoolToInt(value));
				}
				catch {
					// 例外は無視する
				}
			}
		}

		/// <summary>
		/// bool を int に変換する。
		/// (0 は false、それ以外はすべて true)
		/// </summary>
		/// <param name="value">変換するbool値</param>
		/// <returns></returns>
		private static int BoolToInt(bool value) {
			if (value)
				return 1;
			else
				return 0;
		}
	}
}

// Local Variables:
// tab-width: 4
