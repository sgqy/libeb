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
	/// ebinfo.exe �����s����N���X�B
	/// </summary>
	public class EBRefile : EBCommand {
		// "ebrefile.exe" �̃f�t�H���g�̃p�X
		public const string DefaultCommandPath = "ebrefile.exe";

		// ���{�̈ꗗ���擾���鏑�Ђւ̃p�X
		private string bookPath;

		// ���Г��Ɏc�����{�̈ꗗ
		private EBSubbookCollection subbooks;

		/// <summary>
		/// �f�t�H���g�̃R���X�g���N�^
		/// </summary>
		public EBRefile()
			: base(DefaultCommandPath) {
			this.subbooks = new EBSubbookCollection();
			this.bookPath = ".";
		}

		/// <summary>
		/// �R���X�g���N�^�B
		/// ebrefile.exe �̃p�X���Z�b�g����B
		/// </summary>
		/// <param name="commandPathArg">ebrefile.exe �̃p�X</param>
		public EBRefile(string commandPathArg)
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
		/// ���Г��Ɏc�����{�̎擾��ݒ�
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
		/// ���Ђ̃p�X�̎擾�������͐ݒ���s���B
		/// </summary>
		public string BookPath {
			get {
				return string.Copy(this.bookPath);
			}
			set {
				if (value == null)
					throw new ArgumentException("�����G���[ (EBRefile.BookPath)");
				this.bookPath = string.Copy(value);
				this.subbooks.Clear();
			}
		}

		/// <summary>
		/// ebrefile.exe �����s����B
		/// </summary>
		/// <returns></returns>
		public override void Execute() {
			// ebrefile.exe �����s�B
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
				string message = String.Format("{0} �����s�ł��܂���ł���\n{1}",
					this.commandPath, e.Message);
				throw new EBCommandException(message, e);
			}

			// ebrefile.exe ������I���������ǂ�������B
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

			// ebrefile.exe ����n���B
			process.Close();
		}
	}
}

// Local Variables:
// tab-width: 4
