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
	/// �t�@�C���̏㏑�����j�̈ꗗ
	/// </summary>
	public enum EBZipOverwriteMode {
		Confirm, Force, Skip
	}


	/// <summary>
	/// ���Г��̊e�t�@�C���̎��
	/// </summary>
	public class EBZipFileCategory {
		// ID �ԍ�
		private int id;

		// ���{��̖��̂̈ꗗ
		private static string[] categoryNames = new string[7] {
			"�J�^���O",
			"���b�Z�[�W",
			"�{��",
			"�摜",
			"����",
			"�O��",
			"����"
		};

		// ��ނ��s���ȂƂ��̓��{��̖���
		private const string unknownName = "�s��";

		// �����̃t�@�C�����
		public static EBZipFileCategory Catalog  = new EBZipFileCategory(0);
		public static EBZipFileCategory Language = new EBZipFileCategory(1);
		public static EBZipFileCategory Text     = new EBZipFileCategory(2);
		public static EBZipFileCategory Graphic  = new EBZipFileCategory(3);
		public static EBZipFileCategory Sound    = new EBZipFileCategory(4);
		public static EBZipFileCategory Font     = new EBZipFileCategory(5);
		public static EBZipFileCategory Movie    = new EBZipFileCategory(6);
		public static EBZipFileCategory Unknown  = new EBZipFileCategory(-1);

		// �t�@�C���̎�ʔ���p�̐��K�\��
		private static Regex regexGraphic  = new Regex("\\\\data\\\\honmong(\\.[a-z0-9]+)?(;1)?$");
		private static Regex regexSound    = new Regex("\\\\data\\\\honmons(\\.[a-z0-9]+)?(;1)?$");
		private static Regex regexFont     = new Regex("\\\\gaiji\\\\[^\\\\]+$");
		private static Regex regexMovie    = new Regex("\\\\movie\\\\[^\\\\]+$");
		private static Regex regexText1    = new Regex("\\\\data\\\\honmon2?(\\.[a-z0-9]+)?(;1)?$");
		private static Regex regexText2    = new Regex("\\\\start(\\.[a-z0-9]+)?(;1)?$");
		private static Regex regexCatalog  = new Regex("^catalogs?(\\.[a-z0-9]+)?(;1)?$");
		private static Regex regexLanguage = new Regex("language(\\.[a-z0-9]+)?(;1)?$");

		/// <summary>
		/// ����J�̃R���X�g���N�^�B
		/// ID (�����l) ���Z�b�g����B
		/// �t�@�C���̎�ʂ�\�� static �I�u�W�F�N�g Catalog, Language, ... ��������
		/// ���邽�߂Ɏg�p�B
		/// </summary>
		/// <param name="idArg"></param>
		private EBZipFileCategory(int idArg) {
			this.id = idArg;
		}

		/// <summary>
		/// ID (�����l) ��Ԃ��B
		/// </summary>
		public int Id {
			get {
				return id;
			}
		}

		/// <summary>
		/// �t�@�C�����ɉ����āA�t�@�C���̎�ʂ�\�� static �I�u�W�F�N�g Catalog,
		/// Language, ... ��Ԃ��B
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
		/// ���{��̖��� (������) ��Ԃ��B
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
	/// �������̊e�t�@�C����\�����邽�߂̍\���́B
	/// </summary>
	public struct EBZipFile {
		// �t�@�C����
		private string name;

		// ���k�`�� ("ebzip ���k", "EPWING ���k" �Ȃ�)
		private string zipMethod;

		// �t�@�C���̕���
		public EBZipFileCategory Category;

		// �񈳏k���̃T�C�Y
		public uint UnzipSize;

		// ���݂̃T�C�Y
		public uint CurrentSize;

		// ���k�`�����s���ȏꍇ�� zipMethod �ɃZ�b�g���镶����
		private const string unknownZipMethod = "�s��";

		/// <summary>
		/// �t�@�C������ nameArg �ɃZ�b�g����R���X�g���N�^�B
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
		/// �t�@�C������Ԃ�/�Z�b�g����v���p�e�B�B
		/// </summary>
		public string Name {
			get {
				return name;
			}
			set {
				if (value == null)
					throw new ArgumentException("�����G���[ (EBZipFile.Name)");
				this.name = string.Copy(value);
			}
		}

		/// <summary>
		/// ���k�`����Ԃ�/�Z�b�g����v���p�e�B�B
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
		/// ���݂̈��k�� (���݂̃T�C�Y / �񈳏k���̃T�C�Y) ��Ԃ��B
		/// �������A�񈳏k���̃T�C�Y�� 0 �̂Ƃ��� +�� ��Ԃ��B
		/// </summary>
		public double ZipRatio() {
			if (this.UnzipSize == 0)
				return Single.PositiveInfinity;
			else
				return (double)CurrentSize / (double)UnzipSize;
		}

		/// <summary>
		/// EBZipFile �I�u�W�F�N�g�̕��������B
		/// </summary>
		/// <param name="zipFile">�������̃I�u�W�F�N�g</param>
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
	/// EBZipFile �̃R���N�V�����N���X
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
	/// ebzip.exe --information (= ebzipinfo.exe) �����s����N���X�B
	/// </summary>
	public class EBZipInfo : EBCommand {
		// "ebzip.exe" �̃f�t�H���g�̃p�X
		private const string defaultCommandPath = "ebzip.exe";

		// ���k�����擾���鏑�Ђւ̃p�X
		private string bookPath;

		// "ebzip.exe --information" �̎��s�ɂ���Ď擾�����A�t�@�C���̈ꗗ
		private EBZipFileCollection files;

		/// <summary>
		/// �f�t�H���g�̃R���X�g���N�^
		/// </summary>
		public EBZipInfo()
			: base(defaultCommandPath) {
			Initialize();
		}

		/// <summary>
		/// �R���X�g���N�^�B
		/// ebzip.exe �̃p�X�� ebzipPathArg �ɃZ�b�g����B
		/// </summary>
		/// <param name="ebzipPathArg">ebzip.exe �̃p�X</param>
		public EBZipInfo(string commandPathArg)
			: base(commandPathArg) {
			Initialize();
		}

		/// <summary>
		/// �e�R���X�g���N�^���L�̏��������\�b�h
		/// </summary>
		private void Initialize() {
			this.files = new EBZipFileCollection();
			this.bookPath = ".";
		}

		/// <summary>
		/// ���Ђ̃p�X�̎擾����ѐݒ���s���B
		/// </summary>
		public string BookPath {
			get {
				return string.Copy(this.bookPath);
			}
			set {
				if (value == null)
					throw new ArgumentException("�����G���[ (EBInfo.BookPath)");
				this.bookPath = EBCommand.NormalizePath(value);
				this.files.Clear();
			}
		}

		/// <summary>
		/// "ebzip.exe --information" �̎��s�ɂ���Ď擾�����A�t�@�C���̏��̈ꗗ
		/// ��Ԃ��B
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
		/// "ebzip.exe --information" �����s���AbookPath �ɂ��鏑�Ђ̈��k����
		/// �擾����B
		/// </summary>
		public override void Execute() {
			Regex regex1 = new Regex("^==> (.+) <==$");
			Regex regex2 = new Regex("^([0-9]+) �o�C�g \\((.+)\\)$");
			Regex regex3 = new Regex("^([0-9]+) -> ([0-9]+) �o�C�g \\(.*%, (.+)\\)");
			int nFiles = 0;

			// ebzip.exe �����s�B
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
				string message = String.Format("{0} �����s�ł��܂���ł���\n{1}",
					this.commandPath, e.Message);
				throw new EBCommandException(message, e);
			}

			try {
				// ebzip.exe ���W���o�͂֏o�͂������b�Z�[�W��ǂݎ��B
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
				string message = String.Format("{0} �̏o�͂�ǂݍ��߂܂���ł�����\n{1}", 
					this.commandPath, e.Message);
				process.Close();
				throw new EBCommandException(message, e);
			}

			// ebzip.exe ������I���������ǂ�������B
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

			// ebzip.exe ����n���B
			process.Close();
		}
	}

	/// <summary>
	/// ebzip �����Ђɑ΂��čs�������̎��
	/// </summary>
	public enum EBZipBookOperation {
		Zip, Unzip
	}

	/// <summary>
	/// ebzip ���e�t�@�C���ɑ΂��čs�������̎��
	/// </summary>
	public enum EBZipFileOperation {
		Zip, Unzip, Copy, Rewrite
	}

	/// <summary>
	/// ���Ђ̈��k/�L�����J�n����ۂɔ���������C�x���g�̂��߂̃f���Q�[�g
	/// </summary>
	public delegate void StartBook(EBZip ebzip, string sourcePath,
		string destinationPath);

	/// <summary>
	/// ���Ђ̈��k/�L�������������ۂɔ���������C�x���g�̂��߂̃f���Q�[�g
	/// </summary>
	public delegate void EndBook(EBZip ebzip, string sourcePath,
		string destinationPath);

	/// <summary>
	/// �t�@�C���̏������J�n����ۂɔ���������C�x���g�̂��߂̃f���Q�[�g
	/// </summary>
	public delegate void StartFile(EBZip ebzip, EBZipFileOperation operation,
		string sourceFile, string destinationFile);

	/// <summary>
	/// �t�@�C���̏����̐i�s�󋵂�񍐂���C�x���g�̂��߂̃f���Q�[�g
	/// </summary>
	public delegate void ProgressFile(EBZip ebzip, EBZipFileOperation operation,
		string sourceFile, string destinationFile, uint currentOffset,
		uint sourceSize);

	/// <summary>
	/// �t�@�C���̏��������������ۂɔ���������C�x���g�̂��߂̃f���Q�[�g
	/// </summary>
	public delegate void EndFile(EBZip ebzip, EBZipFileOperation operation, 
		string sourceFile, string destinationFile, uint sourceSize,
		uint destinationSize);

	/// <summary>
	/// �t�@�C���̏������X�L�b�v����ۂɔ���������C�x���g�̂��߂̃f���Q�[�g
	/// </summary>
	public delegate void SkipFile(EBZip ebzip, EBZipFileOperation operation,
		string sourceFile, string destinationFile);

	/// <summary>
	/// �o�͐�̃t�@�C���̏㏑���m�F���s���ۂɔ���������C�x���g�̂��߂�
	/// �f���Q�[�g
	/// </summary>
	public delegate bool ConfirmOverwrite(EBZip ebzip, EBZipFileOperation operation,
		string sourceFile, string destinationFile);

	/// <summary>
	/// �t�@�C���������̌x�����b�Z�[�W��`����ۂɔ���������C�x���g�̂��߂�
	/// �f���Q�[�g
	/// </summary>
	public delegate void DeleteFileError(EBZip ebzip, EBZipFileOperation operation,
		string sourceFile);

	/// <summary>
	/// �G���[���N���āA�����𒆎~����ۂɔ���������C�x���g�̂��߂̃f���Q�[�g
	/// </summary>
	public delegate void Abort(EBZip ebzip, string message);

	/// <summary>
	/// "ebzip.exe --compress" (= ebzip.exe) ��������
	/// "ebzip.exe --uncompress" (= ebunzip.exe) �����s����N���X�B
	/// </summary>
	public class EBZip : EBCommand {
		// �����̎�� (���k/�L��)
		private EBZipBookOperation operation;

		// ebzip.exe �̃f�t�H���g�̃p�X
		public const string DefaultCommandPath = "ebzip.exe";

		// ebzip.exe �ň��k/�L�����鏑�Ђ̃p�X
		private string sourcePath;

		// ���k/�L�������f�[�^�̏������ݐ�̃p�X
		private string destinationPath;

		// ���k/�L�����镛�{�̈ꗗ
		private EBSubbookCollection subbooks;

		// �t�@�C���̏㏑�����j
		public EBZipOverwriteMode OverwriteMode;

		// ���k���x��
		private int zipLevel;

		public const int MinZipLevel     = 0;
		public const int MaxZipLevel     = 5;

		// �摜�A�����A�O���A����t�@�C���𖳎����邩�ǂ���
		public bool SkipGraphic;
		public bool SkipSound;
		public bool SkipFont;
		public bool SkipMovie;

		// ���k/�L��������A���̃t�@�C�����폜���邩�ǂ���
		public bool KeepSource;

		// �e�X�g���[�h
		public bool TestMode;

		// �e��C�x���g
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
		/// �f�t�H���g�̃R���X�g���N�^�B
		/// </summary>
		public EBZip()
			: base(DefaultCommandPath) {
			Initialize(EBZipBookOperation.Zip);
		}

		/// <summary>
		/// �R���X�g���N�^�B
		/// </summary>
		/// <param name="operationArg">�����̎�� (���k/�L��)</param>
		public EBZip(EBZipBookOperation operationArg)
			: base(DefaultCommandPath) {
			Initialize(operationArg);
		}

		/// <summary>
		/// �R���X�g���N�^�B
		/// </summary>
		/// <param name="operationArg">�����̎�� (���k/�L��)</param>
		/// <param name="commandPathArg">ebzip.exe �̃p�X</param>
		public EBZip(EBZipBookOperation operationArg, string commandPathArg)
			: base(commandPathArg) {
			Initialize(operationArg);
		}

		/// <summary>
		/// �e�R���X�g���N�^���ʂ̏������������\�b�h
		/// </summary>
		/// <param name="operation">�����̎�� (���k/�L��)</param>
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
		/// ������� (���k/�L��) �̎擾
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
		/// ebzip.exe �ň��k/�L�����鏑�Ђ̃p�X�̎擾�Ɛݒ�
		/// </summary>
		public string SourcePath {
			get {
				return string.Copy(this.sourcePath);
			}
			set {
				if (value == null)
					throw new ArgumentException("�����G���[ (EBZip.SourcePath)");
				this.sourcePath = string.Copy(value);
			}
		}

		/// <summary>
		/// ebzip.exe �ň��k/�L�������f�[�^�̏������ݐ�̃p�X�̎擾�Ɛݒ�
		/// </summary>
		public string DestinationPath {
			get {
				return string.Copy(this.destinationPath);
			}
			set {
				if (value == null)
					throw new ArgumentException("�����G���[ (EBZip.SourcePath)");
				this.destinationPath = string.Copy(value);
			}
		}

		/// <summary>
		/// ebzip.exe �ň��k/�L�����镡�{�̈ꗗ�̎擾��ݒ�
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
		/// ���k���x���̎擾�Ɛݒ�
		/// </summary>
		public int ZipLevel {
			get {
				return zipLevel;
			}
			set {
				if (value < MinZipLevel || value > MaxZipLevel)
					throw new ArgumentException("�����G���[ (EBZip.ZipLevel)");
				zipLevel = value;
			}
		}

		/// <summary>
		/// ebzip.exe �֓n���ׂ��R�}���h�s������Ԃ��B
		/// </summary>
		/// <returns></returns>
		private string CommandArguments() {
			string arguments = "";

			// ���k/�L���I�v�V����
			if (this.operation == EBZipBookOperation.Zip)
				arguments += "--compress";
			else
				arguments += "--uncompress";

			// --output-directory �I�v�V����
			arguments += string.Format(" --output-directory \"{0}\"",
				this.destinationPath.Replace('\\', '/'));

			// --level �I�v�V����
			arguments += " --level " + this.zipLevel;

			// --subbook �I�v�V����
			if (this.subbooks != null) {
				foreach (EBSubbook subbook in this.subbooks)
					arguments += " --subbook " + subbook.Directory;
			}

			// --skip-content �I�v�V����
			if (this.SkipFont)
				arguments += " --skip-content font";
			if (this.SkipGraphic)
				arguments += " --skip-content graphic";
			if (this.SkipSound)
				arguments += " --skip-content sound";
			if (this.SkipMovie)
				arguments += " --skip-content movie";

			// --overwrite �I�v�V����
			if (this.OverwriteMode == EBZipOverwriteMode.Force)
				arguments += " --overwrite force";
			else if (this.OverwriteMode == EBZipOverwriteMode.Skip)
				arguments += " --overwrite no";
			else
				arguments += " --overwrite confirm";

			// --keep �I�v�V����
			if (this.KeepSource)
				arguments += " --keep";

			// --test �I�v�V����
			if (this.TestMode)
				arguments += " --test";

			arguments += string.Format(" \"{0}\"", this.sourcePath.Replace('\\', '/'));

			return arguments;
		}

		/// <summary>
		/// "ebzip.exe" �����s����B
		/// </summary>
		public override void Execute() {
			Regex regex1 = new Regex("^==> (.+) ��(���k|�L��|�R�s�[|��������) <==$");
			Regex regex2 = new Regex("^(.+) �ɏo��$");
			Regex regex3 = new Regex("^ *[0-9.]+% �����ς� \\(([0-9]+) / ([0-9]+) �o�C�g\\)$");
			Regex regex4 = new Regex("^���� \\(([0-9]+) / ([0-9]+) �o�C�g\\)$");
			Regex regex5 = new Regex("^([0-9]+) -> ([0-9]+) �o�C�g");
			Regex regex6 = new Regex("^�t�@�C�������łɑ��݂��܂�: (.+)$");
			Regex regex7 = new Regex("�x��: �t�@�C�����폜�ł��܂���ł���");
			string errorLine = "";

			// ebzip.exe �����s�B
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
				string message = String.Format("{0} �����s�ł��܂���ł���\n{1}",
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

				// ebzip.exe ���W���o�͂֏o�͂������b�Z�[�W��ǂݎ��B
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
						// �t�@�C���̏������J�n
						sourceFile = match1.Groups[1].ToString();
						switch (match1.Groups[2].ToString()) {
							case "���k":
								operation = EBZipFileOperation.Zip;
								break;
							case "�L��":
								operation = EBZipFileOperation.Unzip;
								break;
							case "�R�s�[":
								operation = EBZipFileOperation.Copy;
								break;
							case "��������":
								operation = EBZipFileOperation.Rewrite;
								break;
							default:
								throw new EBCommandException("���K�\���̕s���� (EBZip.Execute)");
						}
					}
					else if (match2.Success) {
						// �������ݐ�̃t�@�C������\��
						destinationFile = match2.Groups[1].ToString();
						if (this.StartFileEvent != null) {
							this.StartFileEvent(this, operation, 
								string.Copy(sourceFile), string.Copy(destinationFile));
						}
					}
					else if (match3.Success) {
						// �t�@�C���̏����󋵂�\��
						uint offset = uint.Parse(match3.Groups[1].ToString());
						uint size   = uint.Parse(match3.Groups[2].ToString());
						if (this.ProgressFileEvent != null) {
							this.ProgressFileEvent(this, operation,
								string.Copy(sourceFile), string.Copy(destinationFile),
								offset, size);
						}
					}
					else if (match4.Success) {
						// �t�@�C���̏����̊���
						uint size = uint.Parse(match4.Groups[1].ToString());
						if (operation == EBZipFileOperation.Copy && 
							this.EndFileEvent != null) {
							this.EndFileEvent(this, operation,
								string.Copy(sourceFile), string.Copy(destinationFile),
								size, size);
						}
					}
					else if (match5.Success) {
						// �t�@�C�������k/�L���������� (���k��) ��\��
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

						// �o�͐�̃t�@�C�������ɑ��݂���|��\��
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

						// �㏑���m�F�̃��b�Z�[�W��ǂݎ̂Ă�B
						line = process.StandardError.ReadLine();
					
					}
					else if (match7.Success) {
						if (this.DeleteFileErrorEvent != null) {
							this.DeleteFileErrorEvent(this, operation,
								string.Copy(sourceFile));
						}
					}
					else if (line == "���̃t�@�C���͂��łɑ��݂���̂ŁA�������܂���" ||
						line == "���͂Əo�̓t�@�C��������Ȃ̂ŁA�������܂���") {
						// �t�@�C���̏������X�L�b�v����|��\��
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
				string message = String.Format("{0} �̏o�͂�ǂݍ��߂܂���ł�����\n{1}", 
					this.commandPath, e.Message);
				process.Close();
				throw new EBCommandException(message, e);
			}

			// ebzip.exe ������I���������ǂ�������B
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

			// ebzip.exe ����n���B
			process.Close();
		}
	}
}

// Local Variables:
// tab-width: 4
