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
	/// �A�v���P�[�V�����̐ݒ�̕ۑ��Ɠǂݍ���
	/// </summary>
	public class WinEBZipRegistry {
		// ���W�X�g���̕��L�[��
		private const string SubKeyName = "Software\\WinEBZip";

		// EB���C�u�����R�}���h�̃f�t�H���g�̃C���X�g�[���p�X
		private const string DefaultEBCommandPath = "";

		// �h�L�������g�̃f�t�H���g�̃C���X�g�[���p�X
		private const string DefaultDocumentPath = "\\Program Files\\WinEBZip\\doc";

		// �u�c�[���o�[�\���̗L���v�̃f�t�H���g�l
		private const bool DefaultShowToolBar = true;

		// �S��ʂŕ\�����邩�ǂ����̃f�t�H���g�l
		private const bool DefaultMaximized = false;

		// �f�t�H���g�̈��k���x��
		private const int DefaultZipLevel = 0;

		/// �u�O���t�@�C�����X�L�b�v�ɂ��邩�ǂ����v�̃f�t�H���g�l
		private const bool DefaultSkipFont = false;

		/// �u�摜�t�@�C�����X�L�b�v�ɂ��邩�ǂ����v�̃f�t�H���g�l
		private const bool DefaultSkipGraphic = false;

		/// �u�����t�@�C�����X�L�b�v�ɂ��邩�ǂ����v�̃f�t�H���g�l
		private const bool DefaultSkipSound = false;

		/// �u����t�@�C�����X�L�b�v�ɂ��邩�ǂ����v�̃f�t�H���g�l
		private const bool DefaultSkipMovie = false;

		// ���W�X�g���L�[
		RegistryKey userRegistryKey;
		RegistryKey machineRegistryKey;

		/// <summary>
		/// �f�t�H���g�̃R���X�g���N�^
		/// </summary>
		public WinEBZipRegistry() {
			this.userRegistryKey    = null;
			this.machineRegistryKey = null;

			this.userRegistryKey = Registry.CurrentUser.CreateSubKey(SubKeyName);
			if (this.userRegistryKey == null)
				throw new ApplicationException("���W�X�g���ɃA�N�Z�X�ł��܂���ł���");

			try {
				this.machineRegistryKey = Registry.LocalMachine.OpenSubKey(SubKeyName);
			}
			catch {
				// ��O�͖�������B
			}
		}

		/// <summary>
		/// ���W�X�g�����N���[�Y����B
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
		/// EB���C�u�����R�}���h�̃C���X�g�[���p�X���擾����B
		/// </summary>
		public string EBCommandPath {
			get {
				string result = DefaultEBCommandPath;
				try {
					result = (string) this.userRegistryKey.GetValue("EBCommandPath", result);
				}
				catch {
					// ��O�͖�������
				}
				return result;
			}
			set {
				try {
					this.userRegistryKey.SetValue("EBCommandPath", value);
				}
				catch {
					// ��O�͖�������
				}
			}
		}

		/// <summary>
		/// �h�L�������g�̃C���X�g�[���p�X���擾����B
		/// </summary>
		public string DocumentPath {
			get {
				string result = DefaultDocumentPath;
				try {
					result = (string) this.machineRegistryKey.GetValue("DocumentPath", result);
				}
				catch {
					// ��O�͖�������
				}

				return result;
			}
		}

		/// <summary>
		/// �c�[���o�[�\���̗L�����擾/�ݒ肷��B
		/// </summary>
		public bool ShowToolBar {
			get {
				int result = BoolToInt(DefaultShowToolBar);
				try {
					result = (int) this.userRegistryKey.GetValue("ShowToolBar", result);
				}
				catch {
					// ��O�͖�������
				}
				return (result != 0);
			}
			set {
				try {
					this.userRegistryKey.SetValue("ShowToolBar", BoolToInt(value));
				}
				catch {
					// ��O�͖�������
				}
			}
		}

		/// <summary>
		/// �S��ʕ\���ɂ��邩�ǂ������擾/�ݒ肷��B
		/// </summary>
		public bool Maximized {
			get {
				int result = BoolToInt(DefaultMaximized);
				try {
					result = (int) this.userRegistryKey.GetValue("Maximized", result);
				}
				catch {
					// ��O�͖�������
				}
				return (result != 0);
			}
			set {
				try {
					this.userRegistryKey.SetValue("Maximized", BoolToInt(value));
				}
				catch {
					// ��O�͖�������
				}
			}
		}

		/// <summary>
		/// ���k���x�����擾/�ݒ肷��B
		/// </summary>
		public int ZipLevel {
			get {
				int result = DefaultZipLevel;
				try {
					result = (int) this.userRegistryKey.GetValue("ZipLevel", result);
				}
				catch {
					// ��O�͖�������
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
					// ��O�͖�������
				}
			}
		}

		/// <summary>
		/// �O���t�@�C�����X�L�b�v�ɂ��邩�ǂ������擾/�ݒ肷��B
		/// </summary>
		public bool SkipFont {
			get {
				int result = BoolToInt(DefaultSkipFont);
				try {
					result = (int) this.userRegistryKey.GetValue("SkipFont", result);
				}
				catch {
					// ��O�͖�������
				}
				return (result != 0);
			}
			set {
				try {
					this.userRegistryKey.SetValue("SkipFont", BoolToInt(value));
				}
				catch {
					// ��O�͖�������
				}
			}
		}

		/// <summary>
		/// �摜�t�@�C�����X�L�b�v�ɂ��邩�ǂ������擾/�ݒ肷��B
		/// </summary>
		public bool SkipGraphic {
			get {
				int result = BoolToInt(DefaultSkipGraphic);
				try {
					result = (int) this.userRegistryKey.GetValue("SkipGraphic", result);
				}
				catch {
					// ��O�͖�������
				}
				return (result != 0);
			}
			set {
				try {
					this.userRegistryKey.SetValue("SkipGraphic", BoolToInt(value));
				}
				catch {
					// ��O�͖�������
				}
			}
		}

		/// <summary>
		/// �����t�@�C�����X�L�b�v�ɂ��邩�ǂ������擾/�ݒ肷��B
		/// </summary>
		public bool SkipSound {
			get {
				int result = BoolToInt(DefaultSkipSound);
				try {
					result = (int) this.userRegistryKey.GetValue("SkipSound", result);
				}
				catch {
					// ��O�͖�������
				}
				return (result != 0);
			}
			set {
				try {
					this.userRegistryKey.SetValue("SkipSound", BoolToInt(value));
				}
				catch {
					// ��O�͖�������
				}
			}
		}

		/// <summary>
		/// ����t�@�C�����X�L�b�v�ɂ��邩�ǂ������擾/�ݒ肷��B
		/// </summary>
		public bool SkipMovie {
			get {
				int result = BoolToInt(DefaultSkipMovie);
				try {
					result = (int) this.userRegistryKey.GetValue("SkipMovie", result);
				}
				catch {
					// ��O�͖�������
				}
				return (result != 0);
			}
			set {
				try {
					this.userRegistryKey.SetValue("SkipMovie", BoolToInt(value));
				}
				catch {
					// ��O�͖�������
				}
			}
		}

		/// <summary>
		/// bool �� int �ɕϊ�����B
		/// (0 �� false�A����ȊO�͂��ׂ� true)
		/// </summary>
		/// <param name="value">�ϊ�����bool�l</param>
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
