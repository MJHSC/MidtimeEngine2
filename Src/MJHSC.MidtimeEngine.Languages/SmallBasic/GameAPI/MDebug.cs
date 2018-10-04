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
	/// Midtime���̂�A�V�X�e���Ɋւ��鑀����s���܂��B
	/// </summary>
	[SmallBasicType]
	public static class MDebug {

		static dynamic GADebug;

		//�ÓI�R���X�g���N�^�iSmallBasic�Ɋ֌W�Ȃ��{�̕����j
		static MDebug() {
			GADebug = new StaticMembersDynamicWrapper(MCore.GameAPI.GetType("MJHSC.MidtimeEngine.GameAPI.MDebug"));
			GADebug.Write("�uMJHSC.MidtimeEngine.Languages.SmallBasic.GameAPI.MDebug�v�����[�h����܂����B");
		}

		/// <summary>
		/// �f�o�b�O���O�ɕ������������݂܂��B
		/// </summary>
		/// <param name="Text">�f�o�b�O���O�ɏ������݂�������</param>
		public static Primitive Write(Primitive Text) {
			GADebug.Write( (string) Text);
			return false;
		}


	}

}