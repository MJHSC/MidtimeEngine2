//Midtime Engine
//	�{�\�[�X�R�[�h�̃��C�Z���X�ɂ��Ă͕t���� LICENSE.html �����ǂ݂��������B
//	Please read LICENSE.html for detail license information.

using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

//MidtimeEngine�ƃv���O�C�����g�p����t�@�C���ǂݏ����N���X�B
namespace MJHSC.MidtimeEngine.Plugins {

	//ARC IV�̎����B2017�N�ł͈��S�ȈÍ��Ƃ͂����Ȃ��ʂ����邪�A�Q�[���f�[�^�̕ی�ɂ͏\���B
	//�Q�[���f�[�^�̓v���C���ɕK�����������s���A�ǂ�Ȃɋ����Í����g���Ă����������B
	//�����ŕK�v�Ȃ̂́u�ȒP�ɂ̓f�[�^�����o���Ȃ�(ARCIV)�v�u�ȒP�ɂ̓f�[�^��������ł��Ȃ�(MidtimeFile�̃n�b�V��)�v�B
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

			//StateMap�̏�����
			for (int i = 0; i < 256; i++) {
				this.StateMap[i] = (byte)i;
			}

			//Key�̃Z�b�g
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