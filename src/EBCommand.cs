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
	/// EBZip, EBUnzip, EBZipInfo �N���X���Ŕ��������A�R�}���h���s�G���[�ɔ���
	///	��O�������N���X�B
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
	/// eb*.exe �R�}���h�̎��s�I�u�W�F�N�g��\�����邽�߂̃N���X�B
	/// EBZip, EBUnzip, EBZipInfo, EBInfo �̊��N���X�B
	/// </summary>
	public abstract class EBCommand {
		// "ebzip.exe" �ւ̃p�X
		protected string commandPath;

		/// <summary>
		/// �R���X�g���N�^�B
		/// �R�}���h�̃p�X���Z�b�g����B
		/// </summary>
		/// <param name="commandPathArg"></param>
		public EBCommand(string commandPathArg) {
			this.commandPath = string.Copy(commandPathArg);
		}

		/// <summary>
		/// �R�}���h�̃p�X��Ԃ�/�l���Z�b�g����v���p�e�B�B
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
		/// �R�}���h�����s����B
		/// </summary>
		public abstract void Execute();

		/// <summary>
		/// ������ ASCII �̃A���t�@�x�b�g ('A' �` 'Z', 'a' �` 'z') ���ǂ����𔻒肷��B
		/// </summary>
		/// <param name="c">���肷�镶��</param>
		/// <returns></returns>
		private static bool IsASCIIAlphabet(char c) {
			return ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z')); 
		}

		/// <summary>
		/// �p�X�𐳋K������B
		/// ���΃p�X�͐�΃p�X�ɒ����A�h���C�u�������Ȃ���Ε₤�B
		/// </summary>
		/// <param name="path">���K������p�X</param>
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
				// `path' �� UNC �p�X�B
				// �������Ȃ�
			}
			else if (result.Length >= 2 && IsASCIIAlphabet(result[0]) && result[1] == ':') {
				if (result.Length <= 2 || result[2] != '\\') {
					// `path' �͑��΃p�X�ŁA�h���C�u�������܂܂�Ă���B
					// ��΃p�X�֕ϊ�����B
					string drive = result.Substring(0, 2);
					Environment.CurrentDirectory = drive;
					string dcwd = Environment.CurrentDirectory;
					Environment.CurrentDirectory = cwd;
					result = string.Format("{0}\\{1}", dcwd, result.Substring(2));
				}
			}
			else if (result.Length >= 1 && result[0] == '\\') {
				// `path' �͐�΃p�X�����A�h���C�u�����������B
				// �h���C�u������}������B
				result = string.Format("{0}\\{1}", cwd.Substring(0, 2), result);
			}
			else {
				// `path' �̓h���C�u�������Ȃ��A���΃p�X�ł���B
				// �h���C�u������}�����A��΃p�X�ɕϊ�����B
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
