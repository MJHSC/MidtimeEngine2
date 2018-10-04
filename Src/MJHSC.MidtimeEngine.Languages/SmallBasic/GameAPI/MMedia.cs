//Midtime Engine
//	�{�\�[�X�R�[�h�̃��C�Z���X�ɂ��Ă͕t���� LICENSE.html �����ǂ݂��������B
//	Please read LICENSE.html for detail license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.SmallBasic.Library;
using MJHSC.MidtimeEngine;

namespace MJHSC.MidtimeEngine.Languages.SmallBasic.GameAPI {


	/// <summary>
	/// BGM �� SE �A�r�f�I �̍Đ��A��~�A���ʂ⑬�x���̐ݒ���s���܂��B
	/// </summary>
	[SmallBasicType]
	public static class MMedia {

		static dynamic GAMediaSS;
		static dynamic GADebug;

		//�ÓI�R���X�g���N�^
		static MMedia() {
			GADebug = new StaticMembersDynamicWrapper(MCore.GameAPI.GetType("MJHSC.MidtimeEngine.GameAPI.MDebug"));
			GAMediaSS = new StaticMembersDynamicWrapper(MCore.GameAPISS.GetType("MJHSC.MidtimeEngine.GameAPI.ScriptSupport.MMedia"));
			GADebug.Write("�uMJHSC.MidtimeEngine.Languages.SmallBasic.GameAPI.MMedia�v�����[�h����܂����B");
		}



		/// <summary>
		/// �w�肳�ꂽ�t�@�C������T�E���h�iBGM�ESE�j��ǂݍ��݁A�T�E���hID���擾���܂��B
		/// �T�E���h�́uGameData\Sounds�v�t�H���_�ɕۑ�����Ă���K�v������܂��B
		/// </summary>
		/// <param name="FileName">�t�@�C���� �i.\GameData\Sounds\�j</param>
		/// <returns>�T�E���hID</returns>
		public static Primitive CreateSound(Primitive FileName) {
			return (int)GAMediaSS.CreateSound((string)FileName);
		}
	
		/// <summary>
		/// �w�肳�ꂽ�t�@�C������T�E���h�iBGM�ESE�j��ǂݍ��݁A�r�f�IID���擾���܂��B
		/// �T�E���h�́uGameData\Sounds�v�t�H���_�ɕۑ�����Ă���K�v������܂��B
		/// </summary>
		/// <param name="FileName">�t�@�C���� �i.\GameData\Videos\�j</param>
		/// <returns>�r�f�IID</returns>
		public static Primitive CreateVideo(Primitive FileName) {
			return (int)GAMediaSS.CreateVideo((string)FileName);
		}

		/// <summary>
		/// ���ׂẴT�E���h�R���g���[�����폜���ă��Z�b�g���܂��B
		/// �X�N���v�g���ς��ۂ͎����I�Ɏ��s����܂��B
		/// </summary>
		public static Primitive Reset() {
			GAMediaSS.Reset();
			return true;
		}



		/// <summary>
		/// �ǂݍ��܂ꂽ�T�E���h�E�r�f�I����x�����Đ����܂��B
		/// SE��r�f�I�̍Đ��ɍœK�ł��B
		/// </summary>
		/// <param name="MMediaID">�T�E���hID �܂��� �r�f�IID (MMedia.CreateSound �܂��� MMedia.CreateVideo �Ŏ擾�ł��܂��B)</param>
		public static Primitive PlayOnce(Primitive MMediaID) {
			GAMediaSS.PlayOnce((int)MMediaID);
			return true;
		}

		/// <summary>
		/// �ǂݍ��܂ꂽ�T�E���h�E�r�f�I���J��Ԃ��Đ����܂��B
		/// BGM�̍Đ��ɍœK�ł��B
		/// </summary>
		/// <param name="MMediaID">�T�E���hID �܂��� �r�f�IID (MMedia.CreateSound �܂��� MMedia.CreateVideo �Ŏ擾�ł��܂��B)</param>
		public static Primitive PlayLoop(Primitive MMediaID) {
			GAMediaSS.PlayLoop((int)MMediaID);
			return true;
		}

		/// <summary>
		/// ���ݓǂݍ��܂�Ă���A�T�E���h�E�r�f�I���~���܂��B
		/// �ēx�A�Đ������ꍇ�͍ŏ�����Đ�����܂��B
		/// </summary>
		/// <param name="MMediaID">�T�E���hID �܂��� �r�f�IID (MMedia.CreateSound �܂��� MMedia.CreateVideo �Ŏ擾�ł��܂��B)</param>
		public static Primitive Stop(Primitive MMediaID) {
			GAMediaSS.Stop((int)MMediaID);
			return true;
		}

		/// <summary>
		/// ���ݓǂݍ��܂�Ă���A�T�E���h�E�r�f�I���~���܂��B
		/// �ēx�A�Đ������ꍇ�� Pause �����ʒu����Đ�����܂��B
		/// </summary>
		/// <param name="MMediaID">�T�E���hID �܂��� �r�f�IID (MMedia.CreateSound �܂��� MMedia.CreateVideo �Ŏ擾�ł��܂��B)</param>
		public static Primitive Pause(Primitive MMediaID) {
			GAMediaSS.Pause((int)MMediaID);
			return true;
		}

		/// <summary>
		/// ���ݓǂݍ��܂�Ă���A�T�E���h�E�r�f�I����āA�ǂݍ��݂��s���O�̏�Ԃɖ߂��܂��B
		/// ������s���� �T�E���h�R���g���[�� �͂����g�p�ł��܂���B
		/// </summary>
		/// <param name="MMediaID">�T�E���hID �܂��� �r�f�IID (MMedia.CreateSound �܂��� MMedia.CreateVideo �Ŏ擾�ł��܂��B)</param>
		public static Primitive Close(Primitive MMediaID) {
			GAMediaSS.Close((int)MMediaID);
			return true;
		}

		/// <summary>
		/// ���ݓǂݍ��܂�Ă���A�T�E���h�E�r�f�I�́u�����v���擾���܂��B 
		/// �����̓~���b(1ms)�P�ʂł��B�i1000�~���b��1�b�ł��j
		/// </summary>
		/// <param name="MMediaID">�T�E���hID �܂��� �r�f�IID (MMedia.CreateSound �܂��� MMedia.CreateVideo �Ŏ擾�ł��܂��B)</param>
		/// <returns>���� (�~���b)</returns>
		public static Primitive GetLength(Primitive MMediaID) {
			return (int)GAMediaSS.GetLength((int)MMediaID);
		}

		/// <summary>
		/// ���ݓǂݍ��܂�Ă���A�T�E���h�E�r�f�I�́u���݂̍Đ��ʒu�v���擾���܂��B 
		/// �����̓~���b(1ms)�P�ʂł��B�i1000�~���b��1�b�ł��j
		/// </summary>
		/// <param name="MMediaID">�T�E���hID �܂��� �r�f�IID (MMedia.CreateSound �܂��� MMedia.CreateVideo �Ŏ擾�ł��܂��B)</param>
		/// <returns>���݂̍Đ��ʒu (�~���b)</returns>
		public static Primitive GetPosition(Primitive MMediaID) {
			return (int)GAMediaSS.GetPosition((int)MMediaID);
		}

		/// <summary>
		/// ���ݓǂݍ��܂�Ă���A�w�肵���T�E���h�E�r�f�I�́u�Đ��ʒu�v��ύX���܂��B 
		/// �����̓~���b(1ms)�P�ʂł��B�i1000�~���b��1�b�ł��j
		/// </summary>
		/// <param name="MMediaID">�T�E���hID �܂��� �r�f�IID (MMedia.CreateSound �܂��� MMedia.CreateVideo �Ŏ擾�ł��܂��B)</param>
		/// <param name="p">�V�����Đ��ʒu (�~���b)</param>
		public static Primitive SetPosition(Primitive MMediaID, Primitive p) {
			GAMediaSS.SetPosition((int)MMediaID, (int)p);
			return true;
		}

		/// <summary>
		/// ���ݓǂݍ��܂�Ă���A�T�E���h�E�r�f�I�́u���ʁv���擾���܂��B 
		/// </summary>
		/// <param name="MMediaID">�T�E���hID �܂��� �r�f�IID (MMedia.CreateSound �܂��� MMedia.CreateVideo �Ŏ擾�ł��܂��B)</param>
		/// <returns>���݂̉��� (0�`100)</returns>
		public static Primitive GetVolume(Primitive MMediaID) {
			return (int)GAMediaSS.GetVolume((int)MMediaID);
		}

		/// <summary>
		/// ���ݓǂݍ��܂�Ă���A�T�E���h�E�r�f�I�́u���ʁv��ύX���܂��B 
		/// </summary>
		/// <param name="MMediaID">�T�E���hID �܂��� �r�f�IID (MMedia.CreateSound �܂��� MMedia.CreateVideo �Ŏ擾�ł��܂��B)</param>
		/// <param name="v">�V�������� (0�`100)</param>
		public static Primitive SetVolume(Primitive MMediaID, Primitive v) {
			GAMediaSS.SetPosSetVolumeition((int)MMediaID, (int)v);
			return true;
		}

		/// <summary>
		/// ���ݓǂݍ��܂�Ă���A�T�E���h�E�r�f�I�́u���x�v���擾���܂��B
		/// ���x��1000�œ����ix1.0�j�ł��B1200��1.2�{�A800��0.8�{�ɂȂ�܂��B
		/// </summary>
		/// <param name="MMediaID">�T�E���hID �܂��� �r�f�IID (MMedia.CreateSound �܂��� MMedia.CreateVideo �Ŏ擾�ł��܂��B)</param>
		/// <returns>�Đ����x (x1.0 = 1000)</returns>
		public static Primitive GetSpeed(Primitive MMediaID) {
			return (int)GAMediaSS.GetSpeed((int)MMediaID);
		}

		/// <summary>
		/// ���ݓǂݍ��܂�Ă���A�T�E���h�E�r�f�I�́u���x�v��ύX���܂��B
		/// ���x��1000�œ����ix1.0�j�ł��B1200��1.2�{�A800��0.8�{�ɂȂ�܂��B
		/// </summary>
		/// <param name="MMediaID">�T�E���hID �܂��� �r�f�IID (MMedia.CreateSound �܂��� MMedia.CreateVideo �Ŏ擾�ł��܂��B)</param>
		/// <param name="s">�Đ����x (x1.0 = 1000)</param>
		public static Primitive SetSpeed(Primitive MMediaID, Primitive s) {
			GAMediaSS.SetSpeed((int)MMediaID, (int)s);
			return true;
		}

	}

}