//Midtime Engine
//	�{�\�[�X�R�[�h�̃��C�Z���X�ɂ��Ă͕t���� LICENSE.html �����ǂ݂��������B
//	Please read LICENSE.html for detail license information.

using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

//MidtimeEngine�ƃv���O�C�����g�p����t�@�C���ǂݏ����N���X�B
namespace MJHSC.MidtimeEngine.Plugins {

	public class MidtimeFileUnknownVersionException : Exception {
		public MidtimeFileUnknownVersionException(String Message)
			: base(Message) {
		}
	}

	public class MidtimeFileCorrupttedException : Exception {
		public MidtimeFileCorrupttedException(String Message)
			: base(Message) {
		}
	}

	//MidtimeFile�`���̓ǂݏ����B
	//MidtimeFile�̍\��:
	//	MEFF		//byte[4],	"Midtime Engine File Format"
	//	0x02		//byte[1],	�o�[�W���� 2.
	//	0x00		//byte[1],	�o�[�W���� 0.
	//	0x00		//byte[1],	�o�[�W���� 0.
	//	0x00		//byte[1],	�o�[�W���� 0
	//	ContentsKey	//byte[256],	�t�@�C���̌ʃL�[���Í�����������
	//	HMACMD5		//byte[16],	�t�@�C����"�`�F�b�N�T��"�B�����񂳂ꂽ���͕̂��������Ȃ��B
	//	!MD5		//byte[16], �t�@�C����"�`�F�b�N�T��"�B�������������ł������m�F���邽�߁B
	//	!Contents	//byte[*],	�t�@�C���{��
	
	// ��L�I����n�܂���̂�ContentsKey�ňÍ��������B
	public class MidtimeFile : Stream {

		//Static
		private static byte[] DefaultContentsKey = Encoding.UTF8.GetBytes("MJHSC.MidtimeEngine.MidtileFile.DefaultKey+Conents");
		private static byte[] DefaultAuthKey = Encoding.UTF8.GetBytes("MJHSC.MidtimeEngine.MidtileFile.DefaultKey+Auth");
		public static void SetDefaultContentsKey(String Key) {
			SetDefaultContentsKey(Encoding.UTF8.GetBytes(Key));
		}
		public static void SetDefaultAuthKey(String Key) {
			SetDefaultAuthKey(Encoding.UTF8.GetBytes(Key));
		}
		public static void SetDefaultContentsKey(byte[] Key) {
			DefaultContentsKey = Key;
		}
		public static void SetDefaultAuthKey(byte[] Key) {
			DefaultAuthKey = Key;
		}

		public static bool SaveFile(MidtimeFile MF, String FilePath) {
			try {
				FileStream FS = new FileStream(FilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);

				//�f�[�^�̏���
				byte[] TEMPByte;

				//MEFF�F��������
				TEMPByte = Encoding.ASCII.GetBytes("MEFF");
				FS.Write(TEMPByte, 0, TEMPByte.Length);

				//�o�[�W�����F��������
				FS.WriteByte(0x02);
				FS.WriteByte(0x00);
				FS.WriteByte(0x00);
				FS.WriteByte(0x00);

				//�R���e���c�L�[�F�����E��������
				byte[] ContentsKey = new byte[256];
				byte[] ContentsKeyCrypted = new byte[256];
				RNGCryptoServiceProvider RNG = new RNGCryptoServiceProvider();
				RNG.GetBytes(ContentsKey);
				ARCIV KeyCrypter = new ARCIV(DefaultContentsKey);
				KeyCrypter.Exec(ContentsKey, ContentsKeyCrypted);
				FS.Write(ContentsKeyCrypted, 0, ContentsKeyCrypted.Length);

				//�{�̂̃��[�h & MD5�F�����̂�
				byte[] Content = MF.Data.ToArray();
				ARCIV DataCrypter = new ARCIV(ContentsKey, true);

				MD5 MD5 = new MD5CryptoServiceProvider();
				byte[] MD5Hash = MD5.ComputeHash(Content, 0, Content.Length);
				DataCrypter.Exec(MD5Hash, MD5Hash);
				DataCrypter.Exec(Content, Content);

				//�O���`�F�b�N�T���FHMACMD5�F�����E��������
				HMACMD5 HMAC = new HMACMD5(DefaultAuthKey);
				TEMPByte = HMAC.ComputeHash(Content, 0, Content.Length);
				FS.Write(TEMPByte, 0, TEMPByte.Length);
				HMAC.Clear();

				//�����`�F�b�N�T���FMD5�F�Í����ς�/��������
				FS.Write(MD5Hash, 0, MD5Hash.Length);

				//�{�́F�Í����ς�/��������
				FS.Write(Content, 0, Content.Length);
				FS.Close();

				return true;
			} catch { }
			return false;
		}


		//Instance
		private MemoryStream Data;
		private bool CanAccess = true;
		private string Path = "";

		public MidtimeFile() {
			InitMidtimeFile(new byte[0]);
		}

		public MidtimeFile(String FilePath) {
			FileStream FS = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read | FileShare.Write);
			this.Path = FilePath;
			byte[] B = new byte[FS.Length];
			FS.Read(B, 0, B.Length);
			FS.Close();
			InitMidtimeFile(B);
		}

		public MidtimeFile(byte[] Data) {
			InitMidtimeFile(Data);
		}

		private void InitMidtimeFile(byte[] Data) {
			//thisContentsKey = DefaultContentsKey;
			//this.AuthKey = DefaultAuthKey;
			this.Data = new MemoryStream(Data);

			//�f�[�^�̏���
			byte[] TEMPByte;

			//MEFF
			TEMPByte = new byte[4];
			this.Data.Read(TEMPByte, 0, TEMPByte.Length);
			if(Encoding.ASCII.GetString(TEMPByte) != "MEFF"){ //Not MEFF, It may RAW file.
				this.Data.Seek(0, SeekOrigin.Begin); //"MEEF"�̎擾�ł��ꂽ�ʒu���ŏ��ɖ߂��B
				return;
			}

			//�o�[�W����
			if (this.Data.ReadByte() != 0x02) { this.CanAccess = false; ErrorWriter.Write(string.Format("���Ή��o�[�W������MidtimeFile�u{0}�v�����[�h���悤�Ƃ��܂����B[1]", this.Path)); return; }
			if (this.Data.ReadByte() != 0x00) { this.CanAccess = false; ErrorWriter.Write(string.Format("���Ή��o�[�W������MidtimeFile�u{0}�v�����[�h���悤�Ƃ��܂����B[2]", this.Path)); return; }
			this.Data.ReadByte(); //�o�[�W�����̉��ʕ��̓`�F�b�N���Ȃ�
			this.Data.ReadByte(); //�o�[�W�����̉��ʕ��̓`�F�b�N���Ȃ�

			//�R���e���c�L�[�擾
			byte[] ContentsKey = new byte[256];
			this.Data.Read(ContentsKey, 0, ContentsKey.Length);

			//�O���`�F�b�N�T���擾
			byte[] CheckSum = new byte[16];
			this.Data.Read(CheckSum, 0, CheckSum.Length);

			//�����`�F�b�N�T���擾
			byte[] CheckSumI = new byte[16];
			this.Data.Read(CheckSumI, 0, CheckSumI.Length);

			//�{�̂̎擾
			byte[] Content = new byte[this.Data.Length - this.Data.Position];
			this.Data.Read(Content, 0, Content.Length);

			//���f�[�^����O���`�F�b�N�T������
			HMACMD5 HMAC = new HMACMD5(DefaultAuthKey);
			TEMPByte = HMAC.ComputeHash(Content, 0, Content.Length);
			HMAC.Clear();

			for (int i = 0; i < TEMPByte.Length; i++) {
				if (CheckSum[i] != TEMPByte[i]) {
					this.CanAccess = false;
					ErrorWriter.Write(string.Format("�t�@�C���u{0}�v���j�����Ă��邽�߁A���[�h�Ɏ��s���܂����B[1]", this.Path));
					return;
					}
			}

			//������: �R���e���c�L�[
			ARCIV KeyDeCrypter = new ARCIV(DefaultContentsKey);
			KeyDeCrypter.Exec(ContentsKey, ContentsKey);

			//������: �{�̂̎擾
			ARCIV DataDeCrypter = new ARCIV(ContentsKey, true);
			DataDeCrypter.Exec(CheckSumI, CheckSumI); //��ɓ����`�F�b�N�T���𕜍��� �i��������Ȃ���ARCIV�̓����X�e�[�g�������j
			DataDeCrypter.Exec(Content, Content); //�{�̂̕�����
			MemoryStream MS = new MemoryStream(Content);

			//���f�[�^��������`�F�b�N�T������
			MD5 MD5 = new MD5CryptoServiceProvider();
			TEMPByte = MD5.ComputeHash(Content, 0, Content.Length);

			for (int i = 0; i < TEMPByte.Length; i++) {
				if (CheckSumI[i] != TEMPByte[i]) {
					this.CanAccess = false;
					ErrorWriter.Write(string.Format("�t�@�C���u{0}�v���j�����Ă��邽�߁A���[�h�Ɏ��s���܂����B[2]", this.Path));
					return;
				}
			}

			this.Data.Close();
			this.Data = MS;
		}


		public override bool CanRead {
			get { return this.CanAccess; }
		}

		public override bool CanSeek {
			get { return this.CanAccess; }
		}

		public override bool CanWrite {
			get { return this.CanAccess; }
		}

		public override void Flush() {
			this.Data.Flush();
		}

		public override long Length {
			get { return this.Data.Length; }
		}

		public override long Position {
			get {
				return this.Data.Position;
			}
			set {
				this.Data.Position = value;
			}
		}

		public override int Read(byte[] buffer, int offset, int count) {
			return this.Data.Read(buffer, offset, count);
		}

		public override long Seek(long offset, SeekOrigin origin) {
			return this.Data.Seek(offset, origin);
		}

		public override void SetLength(long value) {
			this.Data.SetLength(value);
		}

		public override void Write(byte[] buffer, int offset, int count) {
			this.Data.Write(buffer, offset, count);
		}

		public override void Close() {
			this.Data.Close();

		}

	}
}