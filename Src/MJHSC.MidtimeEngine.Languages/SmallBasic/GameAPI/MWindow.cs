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
	public static class MWindow {

		static dynamic GAWindow;
		static dynamic GADebug;

		//�ÓI�R���X�g���N�^
		static MWindow() {
			GADebug = new StaticMembersDynamicWrapper(MCore.GameAPI.GetType("MJHSC.MidtimeEngine.GameAPI.MDebug"));
			GAWindow = new StaticMembersDynamicWrapper(MCore.GameAPI.GetType("MJHSC.MidtimeEngine.GameAPI.MWindow"));
			GADebug.Write("�uMJHSC.MidtimeEngine.Languages.SmallBasic.GameAPI.MMedia�v�����[�h����܂����B");
		}



		/// <summary>
		/// �yWindows��p�z�Q�[����ʂ̃^�C�g���\�L��ύX���܂��B�ʏ�g�p����K�v�͂���܂���B
		/// </summary>
		/// <param name="Title">�V�����^�C�g��</param>
		[Obsolete("�ʏ�A�K�v�Ƃ��Ȃ��R�[�h�ł��B�{���ɕK�v���悭�m�F���Ă��������B", false)]
		public static Primitive SetTitle(Primitive Title) {
			GAWindow.SetTitle((string)Title);
			return true;
		}

		/// <summary>
		/// �yWindows��p�z�Q�[����ʂ̑傫����ύX���܂��B �𑜓x�͕ς��Ȃ����߁A960x540 �ȊO�ɂ����ꍇ�͏k���܂��͊g�傳��܂��B
		/// �i���̈ێ��̂��߁A�u480x270�E960x540�i1K�j�E1440x810�E1920x1080�i2K�j�E2880x1620�E3840x2160�i4K�j�E5760x3240�E7680x4320�i8K�j�v�ȊO�̐��l�͐������܂���B
		/// </summary>
		/// <param name="Width">�V��������</param>
		/// <param name="Height">�V�����c��</param>
		public static Primitive SetSize(Primitive Width, Primitive Height) {
			GAWindow.SetSize((int)Width, (int)Height);
			return true;
		}

		/// <summary>
		/// �yWindows��p�z��ʂ��t���X�N���[���\���ɂ��܂��B���Ƀt���X�N���[���̏ꍇ�A���ɖ߂��܂��B
		/// �uAlt+Enter�v�������ƁA���ł��v���C���[�͐؂�ւ����邱�Ƃɒ��ӂ��Ă��������B
		/// �@Windows�ȊO�ł͏�Ƀt���X�N���[���ł��B
		/// </summary>
		public static Primitive ToggleFullscreen() {
			GAWindow.ToggleFullscreen();
			return true;
		}


	}

}