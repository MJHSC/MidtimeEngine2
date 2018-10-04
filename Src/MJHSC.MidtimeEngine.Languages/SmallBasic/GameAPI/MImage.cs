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
	/// Midtime Engine �̉�ʂɊւ���ݒ���s���܂��B
	/// </summary>
	[SmallBasicType]
	public static class MImage {

		static dynamic GAImage;
		static dynamic GAImageSS;
		static dynamic GADebug;

		//�ÓI�R���X�g���N�^
		static MImage() {
			GADebug = new StaticMembersDynamicWrapper(MCore.GameAPI.GetType("MJHSC.MidtimeEngine.GameAPI.MDebug"));
			GAImage = new StaticMembersDynamicWrapper(MCore.GameAPI.GetType("MJHSC.MidtimeEngine.GameAPI.MImage"));
			GAImageSS = new StaticMembersDynamicWrapper(MCore.GameAPISS.GetType("MJHSC.MidtimeEngine.GameAPI.ScriptSupport.MImage"));
			GADebug.Write("�uMJHSC.MidtimeEngine.Languages.SmallBasic.GameAPI.MImage�v�����[�h����܂����B");
		}


		/// <summary>
		/// ����A�V���ɍ쐬���镶���̐F��ύX���܂��B
		/// �e�F�̗ʂ́u0�`255�v�̐����Ō��̎O���F�Ƃ��ĕ\���܂��B
		/// ���ׂĂO�̏ꍇ�͍��A���ׂ�255�̏ꍇ�͔��ɂȂ�܂��B
		/// 
		/// ���ɍ쐬�ς݂̕����ɉe���͂���܂���B
		/// </summary>
		/// <param name="R">�ԐF�i0�`255�j</param>
		/// <param name="G">�ΐF�i0�`255�j</param>
		/// <param name="B">�F�i0�`255�j</param>
		public static Primitive SetTextColor(Primitive R, Primitive G, Primitive B) {
			GAImage.SetTextColor((int)R, (int)G, (int)B);
			return true;
		}

		/// <summary>
		/// ����A�V���ɍ쐬���镶���̑傫����ύX���܂��B
		/// �傫���̒P�ʂ�Word�ȂǂƓ����A�u�|�C���g�ipt�j�v�ł��B
		/// 
		/// ���ɍ쐬�ς݂̕����ɉe���͂���܂���B
		/// </summary>
		/// <param name="Size">�����̑傫���ipt�j</param>
		public static Primitive SetTextSize(Primitive Size) {
			GAImage.SetTextSize((int) Size);
			return true;
		}

		/// ����A�V���ɍ쐬���镶���̏��̂�ύX���܂��B
		/// �V�X�e���ɂ���Ďg�p�\�ȏ��̂��قȂ�̂ŁA�ʏ�g�p����ׂ��ł͂���܂���B
		/// 
		/// ���ɍ쐬�ς݂̕����ɉe���͂���܂���B
		/// <param name="FontName">�t�H���g��</param>
		[Obsolete("�V�X�e���ɂ���Ďg�p�\�ȏ��̂��قȂ�̂ŁA�ʏ�g�p����ׂ��ł͂���܂���B", false)]
		public static Primitive SetTextFont(Primitive FontName) {
			GAImage.SetTextFont((string) FontName);
			return true;
		}

		/// <summary>
		/// �w�肵����������ʕ\���ł���悤�ɂ��܂��B
		/// </summary>
		/// <param name="DrawText">�\������������</param>
		/// <returns>�摜ID</returns>
		public static Primitive CreateText(Primitive DrawText) {
			return (int) GAImageSS.CreateText((string) DrawText);
		}

		/// <summary>
		/// �w�肵���摜�t�@�C������ʕ\���ł���悤�ɂ��܂��B
		/// �摜�́uGameData\Images\�v�j�ɕۑ�����Ă���K�v������܂��B
		/// </summary>
		/// <param name="FileName">�\���������摜�t�@�C����</param>
		/// <returns>�摜ID</returns>
		public static Primitive CreateImage(Primitive FileName) {
			return (int)GAImageSS.CreateImage((string) FileName);
		}



		/// <summary>
		/// ���ׂẴC���[�W���폜���ă��Z�b�g���܂��B
		/// �X�N���v�g���ς��ۂ͎����I�Ɏ��s����܂��B
		/// </summary>
		public static Primitive Reset() {
			GAImage.Reset();
			return true;
		}




		/// <summary>
		/// �摜���u���݂̈ʒu����v�w�肳�ꂽ�ʂ����ړ����܂��B
		/// </summary>
		/// <param name="MImageID">�摜ID (MImage.CreateText �܂��� MImage.CreateImage �Ŏ擾�ł��܂��B)</param>
		/// <param name="X">�������̈ړ��ʁB</param>
		/// <param name="Y">�c�����̈ړ��ʁB</param>
		public static Primitive Move(Primitive MImageID, Primitive X, Primitive Y) {
			GAImageSS.Move((int)MImageID, (int)X, (int)Y);
			return true;
		}

		/// <summary>
		/// �摜���w�肳�ꂽ�ʒu�Ɉړ����܂��B
		/// </summary>
		/// <param name="MImageID">�摜ID (MImage.CreateText �܂��� MImage.CreateImage �Ŏ擾�ł��܂��B)</param>
		/// <param name="X">�������̈ʒu�B</param>
		/// <param name="Y">�c�����̈ʒu�B</param>
		public static Primitive SetPosition(Primitive MImageID, Primitive X, Primitive Y) {
			GAImageSS.SetPosition((int)MImageID, (int)X, (int)Y);
			return true;
		}

		/// <summary>
		/// �摜�̓����x���u���݂̈ʒu����v�w�肳�ꂽ�ʂ����ω������܂��B
		/// 
		/// ���̌��ʁA�ŏ��l�i0�j�ȉ��̏ꍇ�͓����ɂȂ�A����ȉ��ɂ͉�����܂���B
		/// �܂��A�ő�l�i255�j�ȏ�̏ꍇ�͕s�����ɂȂ�A����ȏ�ɂ͏オ��܂���B
		/// </summary>
		/// <param name="MImageID">�摜ID (MImage.CreateText �܂��� MImage.CreateImage �Ŏ擾�ł��܂��B)</param>
		/// <param name="Alpha">�����x�B</param>
		public static Primitive Alpha(Primitive MImageID, Primitive Alpha) {
			GAImageSS.Alpha((int)MImageID, (int)Alpha);
			return true;
		}

		/// <summary>
		/// �摜�̓����x���w�肵�܂��B
		/// 
		/// �ŏ��l�i0�j�ȉ��̏ꍇ�͓����ɂȂ�A����ȉ��ɂ͉�����܂���B
		/// �ő�l�i255�j�ȏ�̏ꍇ�͕s�����ɂȂ�A����ȏ�ɂ͏オ��܂���B
		/// </summary>
		/// <param name="MImageID">�摜ID (MImage.CreateText �܂��� MImage.CreateImage �Ŏ擾�ł��܂��B)</param>
		/// <param name="Alpha">�����x�B</param>
		public static Primitive SetAlpha(Primitive MImageID, Primitive Alpha) {
			GAImageSS.SetAlpha((int)MImageID, (int)Alpha);
			return true;
		}

		/// <summary>
		/// �摜�̊p�x���u���݂̊p�x����v�w�肳�ꂽ�x��������]�����܂��B
		/// 
		/// �P�ʂ͓x�i���j�ł��B
		/// </summary>
		/// <param name="MImageID">�摜ID (MImage.CreateText �܂��� MImage.CreateImage �Ŏ擾�ł��܂��B)</param>
		/// <param name="Rotate">��]������x���i-360�`360�j</param>
		public static Primitive Rotate(Primitive MImageID, Primitive Rotate) {
			GAImageSS.Rotate((int)MImageID, (int)Rotate);
			return true;
		}

		/// �摜�̊p�x���w�肳�ꂽ�p�x�ɂ��܂��B
		/// 
		/// �P�ʂ͓x�i���j�ł��B 90�Ő^���A180�Ő^�t�ɂȂ�܂��B
		/// <param name="MImageID">�摜ID (MImage.CreateText �܂��� MImage.CreateImage �Ŏ擾�ł��܂��B)</param>
		public static Primitive SetRotate(Primitive MImageID, Primitive Rotate) {
			GAImageSS.SetRotate((int)MImageID, (int)Rotate);
			return true;
		}

		/// <summary>
		/// ���݂܂łɐݒ肳�ꂽ�l���g�p���āA�摜����ʂɕ\�����܂��B
		/// </summary>
		/// <param name="MImageID">�摜ID (MImage.CreateText �܂��� MImage.CreateImage �Ŏ擾�ł��܂��B)</param>
		public static Primitive Draw(Primitive MImageID) {
			GAImageSS.Draw((int)MImageID);
			return true;
		}


		/// <summary>
		/// ���ݓǂݍ��܂�Ă���A�摜����āA�ǂݍ��݂��s���O�̏�Ԃɖ߂��܂��B
		/// ������s���� �摜�R���g���[�� �͂����g�p�ł��܂���B
		/// </summary>
		/// <param name="MImageID">�摜ID (MImage.CreateText �܂��� MImage.CreateImage �Ŏ擾�ł��܂��B)</param>
		public static Primitive Close(Primitive MImageID) {
			GAImageSS.Close((int)MImageID);
			return true;
		}
		


	}

}