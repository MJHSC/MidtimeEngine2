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
	public static class MInput {

		static dynamic GAInputSS;
		static dynamic GADebug;

		//�ÓI�R���X�g���N�^
		static MInput() {
			GADebug = new StaticMembersDynamicWrapper(MCore.GameAPI.GetType("MJHSC.MidtimeEngine.GameAPI.MDebug"));
			GAInputSS = new StaticMembersDynamicWrapper(MCore.GameAPISS.GetType("MJHSC.MidtimeEngine.GameAPI.ScriptSupport.MInput"));
			GADebug.Write("�uMJHSC.MidtimeEngine.Languages.SmallBasic.GameAPI.MInput�v�����[�h����܂����B");
		}

		/// <summary>
		/// �w�肵�����Ԃ̊ԁA������~���܂��B�����̓~���b(ms)�P�ʂł��B�i1000�~���b��1�b�ł��j
		/// </summary>
		/// <param name="TimeInMS">��~���鎞�ԁi�~���b�j</param>
		/// <returns></returns>
		public static Primitive GetButton(Primitive PlayerID, Primitive ButtonID){
			if (
				GAInputSS.GetButton((int)PlayerID, (string)ButtonID)
			) {
				return 1;
			}
			return 0;
		}
		public static Primitive GetKey(Primitive KeyID){
			if (
				GAInputSS.GetKey((string)KeyID)
			) {
				return 1;
			}
			return 0;
		}
		public static Primitive GetLastKey(){
			return GAInputSS.GetLastKey();
		}
		public static Primitive GetMouse(Primitive MouseMoveID){
			return GAInputSS.GetMouse((string)MouseMoveID);
		}
		public static Primitive GetStick(Primitive PlayerID, Primitive StickID){
			return GAInputSS.GetStick((int)PlayerID, (string)StickID);
		}
		public static Primitive GetTrigger(Primitive PlayerID, Primitive TriggerID){
			return GAInputSS.GetTrigger((int)PlayerID, (string)TriggerID);
		}




	}

}