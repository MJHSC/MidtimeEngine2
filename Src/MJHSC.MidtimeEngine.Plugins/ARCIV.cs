//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.

using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

//MidtimeEngineとプラグインが使用するファイル読み書きクラス。
namespace MJHSC.MidtimeEngine.Plugins {

	//ARC IVの実装。2017年では安全な暗号とはいえない面もあるが、ゲームデータの保護には十分。
	//ゲームデータはプレイ中に必ず複合される都合、どんなに強い暗号を使っても解除される。
	//ここで必要なのは「簡単にはデータが取り出せない(ARCIV)」「簡単にはデータが改ざんできない(MidtimeFileのハッシュ)」。
	public class ARCIV {

		//State
		private int State1;
		private int State2;
		private byte[] StateMap = new byte[0x100];

		public ARCIV(string Key) {
			SHA256 KeyByte = new SHA256Managed();
			InitARCIV(KeyByte.ComputeHash(Encoding.UTF8.GetBytes(Key)));
			KeyByte.Clear();
		}
		public ARCIV(byte[] Key) {
			SHA256 KeyByte = new SHA256Managed();
			InitARCIV(KeyByte.ComputeHash(Key));
			KeyByte.Clear();
		}

		public ARCIV(byte[] Key, bool UseKeyAsRaw) {
			if (UseKeyAsRaw) {
				InitARCIV(Key);
			} else {
				SHA256 KeyByte = new SHA256Managed();
				InitARCIV(KeyByte.ComputeHash(Key));
				KeyByte.Clear();
			}
		}

		private void InitARCIV(byte[] Key) {
			int n, m;
			uint k;

			this.State1 = 0;
			this.State2 = 0;
			n = 0;
			k = 0;

			//StateMapの初期化
			for (int i = 0; i < 256; i++) {
				this.StateMap[i] = (byte)i;
			}

			//Keyのセット
			for (int i = 0; i < 256; i++, k++) {
				if (k >= Key.Length) {
					k = 0;
				}

				m = this.StateMap[i];
				n = (n + m + Key[k]) & 0xFF;
				this.StateMap[i] = this.StateMap[n];
				this.StateMap[n] = (byte)m;
			}
		}


		public int Exec(byte[] input, byte[] output) {
			int State1X, State2X, n, m;

			State1X = this.State1;
			State2X = this.State2;

			for (int i = 0; i < input.Length; i++) {
				State1X = (State1X + 1) & 0xFF;
				n = this.StateMap[State1X];

				State2X = (State2X + n) & 0xFF;
				m = this.StateMap[State2X];

				this.StateMap[State1X] = (byte)m;
				this.StateMap[State2X] = (byte)n;

				output[i] = (byte)(input[i] ^ this.StateMap[(byte)(n + m)]);
			}

			this.State1 = State1X;
			this.State2 = State2X;

			return (0);
		}

	}

}