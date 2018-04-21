//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.

using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

//MidtimeEngineとプラグインが使用するファイル読み書きクラス。
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

	//MidtimeFile形式の読み書き。
	//MidtimeFileの構造:
	//	MEFF		//byte[4],	"Midtime Engine File Format"
	//	0x02		//byte[1],	バージョン 2.
	//	0x00		//byte[1],	バージョン 0.
	//	0x00		//byte[1],	バージョン 0.
	//	0x00		//byte[1],	バージョン 0
	//	ContentsKey	//byte[256],	ファイルの個別キーを暗号化したもの
	//	HMACMD5		//byte[16],	ファイルの"チェックサム"。改ざんされたものは復号化しない。
	//	!MD5		//byte[16], ファイルの"チェックサム"。正しく復号化できたか確認するため。
	//	!Contents	//byte[*],	ファイル本体
	
	// 上記！から始まるものはContentsKeyで暗号化される。
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

				//データの準備
				byte[] TEMPByte;

				//MEFF：書き込み
				TEMPByte = Encoding.ASCII.GetBytes("MEFF");
				FS.Write(TEMPByte, 0, TEMPByte.Length);

				//バージョン：書き込み
				FS.WriteByte(0x02);
				FS.WriteByte(0x00);
				FS.WriteByte(0x00);
				FS.WriteByte(0x00);

				//コンテンツキー：生成・書き込み
				byte[] ContentsKey = new byte[256];
				byte[] ContentsKeyCrypted = new byte[256];
				RNGCryptoServiceProvider RNG = new RNGCryptoServiceProvider();
				RNG.GetBytes(ContentsKey);
				ARCIV KeyCrypter = new ARCIV(DefaultContentsKey);
				KeyCrypter.Exec(ContentsKey, ContentsKeyCrypted);
				FS.Write(ContentsKeyCrypted, 0, ContentsKeyCrypted.Length);

				//本体のロード & MD5：生成のみ
				byte[] Content = MF.Data.ToArray();
				ARCIV DataCrypter = new ARCIV(ContentsKey, true);

				MD5 MD5 = new MD5CryptoServiceProvider();
				byte[] MD5Hash = MD5.ComputeHash(Content, 0, Content.Length);
				DataCrypter.Exec(MD5Hash, MD5Hash);
				DataCrypter.Exec(Content, Content);

				//外部チェックサム：HMACMD5：生成・書き込み
				HMACMD5 HMAC = new HMACMD5(DefaultAuthKey);
				TEMPByte = HMAC.ComputeHash(Content, 0, Content.Length);
				FS.Write(TEMPByte, 0, TEMPByte.Length);
				HMAC.Clear();

				//内部チェックサム：MD5：暗号化済み/書き込み
				FS.Write(MD5Hash, 0, MD5Hash.Length);

				//本体：暗号化済み/書き込み
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

			//データの準備
			byte[] TEMPByte;

			//MEFF
			TEMPByte = new byte[4];
			this.Data.Read(TEMPByte, 0, TEMPByte.Length);
			if(Encoding.ASCII.GetString(TEMPByte) != "MEFF"){ //Not MEFF, It may RAW file.
				this.Data.Seek(0, SeekOrigin.Begin); //"MEEF"の取得でずれた位置を最初に戻す。
				return;
			}

			//バージョン
			if (this.Data.ReadByte() != 0x02) { this.CanAccess = false; ErrorWriter.Write(string.Format("未対応バージョンのMidtimeFile「{0}」をロードしようとしました。[1]", this.Path)); return; }
			if (this.Data.ReadByte() != 0x00) { this.CanAccess = false; ErrorWriter.Write(string.Format("未対応バージョンのMidtimeFile「{0}」をロードしようとしました。[2]", this.Path)); return; }
			this.Data.ReadByte(); //バージョンの下位部はチェックしない
			this.Data.ReadByte(); //バージョンの下位部はチェックしない

			//コンテンツキー取得
			byte[] ContentsKey = new byte[256];
			this.Data.Read(ContentsKey, 0, ContentsKey.Length);

			//外部チェックサム取得
			byte[] CheckSum = new byte[16];
			this.Data.Read(CheckSum, 0, CheckSum.Length);

			//内部チェックサム取得
			byte[] CheckSumI = new byte[16];
			this.Data.Read(CheckSumI, 0, CheckSumI.Length);

			//本体の取得
			byte[] Content = new byte[this.Data.Length - this.Data.Position];
			this.Data.Read(Content, 0, Content.Length);

			//現データから外部チェックサム生成
			HMACMD5 HMAC = new HMACMD5(DefaultAuthKey);
			TEMPByte = HMAC.ComputeHash(Content, 0, Content.Length);
			HMAC.Clear();

			for (int i = 0; i < TEMPByte.Length; i++) {
				if (CheckSum[i] != TEMPByte[i]) {
					this.CanAccess = false;
					ErrorWriter.Write(string.Format("ファイル「{0}」が破損しているため、ロードに失敗しました。[1]", this.Path));
					return;
					}
			}

			//復号化: コンテンツキー
			ARCIV KeyDeCrypter = new ARCIV(DefaultContentsKey);
			KeyDeCrypter.Exec(ContentsKey, ContentsKey);

			//復号化: 本体の取得
			ARCIV DataDeCrypter = new ARCIV(ContentsKey, true);
			DataDeCrypter.Exec(CheckSumI, CheckSumI); //先に内部チェックサムを復号化 （これをしないとARCIVの内部ステートがずれる）
			DataDeCrypter.Exec(Content, Content); //本体の復号化
			MemoryStream MS = new MemoryStream(Content);

			//現データから内部チェックサム生成
			MD5 MD5 = new MD5CryptoServiceProvider();
			TEMPByte = MD5.ComputeHash(Content, 0, Content.Length);

			for (int i = 0; i < TEMPByte.Length; i++) {
				if (CheckSumI[i] != TEMPByte[i]) {
					this.CanAccess = false;
					ErrorWriter.Write(string.Format("ファイル「{0}」が破損しているため、ロードに失敗しました。[2]", this.Path));
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