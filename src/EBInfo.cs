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
	/// ���Г��̊e���{��\�����邽�߂̍\���́B
	/// </summary>
	public struct EBSubbook {
		// ���{�̃T�u�f�B���N�g����
		private string directory;

		// ���{�̑薼
		private string title;

		/// <summary>
		/// ���{�̃T�u�f�B���N�g������Ԃ�/�Z�b�g����B
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
		/// ���{�̑薼��Ԃ�/�Z�b�g����B
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
		/// EBSubbook �I�u�W�F�N�g�̕�����Ԃ��B
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
	/// EBSubbook �̃R���N�V�����N���X
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
	/// ebinfo.exe �����s����N���X�B
	/// </summary>
	public class EBInfo : EBCommand {
		// "ebinfo.exe" �̃f�t�H���g�̃p�X
		public const string DefaultCommandPath = "ebinfo.exe";

		// ���{�̈ꗗ���擾���鏑�Ђւ̃p�X
		private string bookPath;

		// "ebinfo.exe" �̎��s�ɂ���Ď擾�����A���{�̈ꗗ
		private EBSubbookCollection subbooks;

		/// <summary>
		/// �f�t�H���g�̃R���X�g���N�^
		/// </summary>
		public EBInfo()
			: base(DefaultCommandPath) {
			this.subbooks = new EBSubbookCollection();
			this.bookPath = ".";
		}

		/// <summary>
		/// �R���X�g���N�^�B
		/// ebinfo.exe �̃p�X���Z�b�g����B
		/// </summary>
		/// <param name="commandPathArg">ebinfo.exe �̃p�X</param>
		public EBInfo(string commandPathArg)
			: base(commandPathArg) {
			this.Initialize();
		}

		/// <summary>
		/// �e�R���X�g���N�^���ʂ̏����������B
		/// </summary>
		private void Initialize() {
			this.subbooks = new EBSubbookCollection();
			this.bookPath = ".";
		}

		/// <summary>
		/// ebinfo.exe �̎��s�ɂ���Ď擾�����A���{�̈ꗗ��Ԃ��B
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
		/// ���Ђ̃p�X�̎擾�������͐ݒ���s���B
		/// </summary>
		public string BookPath {
			get {
				return string.Copy(this.bookPath);
			}
			set {
				if (value == null)
					throw new ArgumentException("�����G���[ (EBInfo.BookPath)");
				this.bookPath = string.Copy(value);
				this.subbooks.Clear();
			}
		}

		/// <summary>
		/// ebinfo.exe �����s���A���Г��̕��{�ꗗ���擾����B
		/// </summary>
		/// <returns></returns>
		public override void Execute() {
			Regex regex1 = new Regex("^  �薼: (.+)$");
			Regex regex2 = new Regex("^  �f�B���N�g��: (.+)$");
			this.subbooks.Clear();

			// ebinfo.exe �����s�B
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
				string message = String.Format("{0} �����s�ł��܂���ł���\n{1}",
					this.commandPath, e.Message);
				throw new EBCommandException(message, e);
			}

			try {
				// ebinfo.exe ���W���o�͂֏o�͂������b�Z�[�W��ǂݎ��B
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
				string message = String.Format("{0} �̏o�͂�ǂݍ��߂܂���ł�����\n{1}", 
					this.commandPath, e.Message);
				process.Close();
				throw new EBCommandException(message, e);
			}

			// ebinfo.exe ������I���������ǂ�������B
			while (!process.HasExited)
				Thread.Sleep(1000);

			if (process.ExitCode != 0) {
				string line;
				try {
					line = process.StandardError.ReadLine();
				}
				catch {
					line = string.Format("{0}: ����ɏI�����܂���ł����B",
						this.commandPath);
				}

				process.Close();
				throw new EBCommandException(line);
			}

			// ebinfo.exe ����n���B
			process.Close();
		}
	}
}

// Local Variables:
// tab-width: 4
