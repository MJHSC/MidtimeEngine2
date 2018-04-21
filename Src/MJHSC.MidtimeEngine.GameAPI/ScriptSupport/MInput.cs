using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using MJHSC.MidtimeEngine;
using MJHSC.MidtimeEngine.GameAPI;
using MJHSC.MidtimeEngine.Plugins;

namespace MJHSC.MidtimeEngine.GameAPI.ScriptSupport {

	[Obsolete("���̊֐��͈ꕔ�̌���T�|�[�g�̂��߂ɓ����Ŏg�p����A�R���e���c�ł̎g�p�͋�����Ă��܂���B", false)]
	[MidtimeFunction2Legacy]
	public static class MInput {

		public static bool GetButton(int PlayerID, string ButtonID) {
			try {
				return MJHSC.MidtimeEngine.GameAPI.MInput.GetButton(
					PlayerID
					, (MJHSC.MidtimeEngine.GameAPI.MInput.ButtonID)Enum.Parse(typeof(MJHSC.MidtimeEngine.GameAPI.MInput.ButtonID), ButtonID)
				);
			} catch {
				MDebug.WriteFormat("���݂��Ȃ��{�^�����u{0}�v���w�肳��܂���", ButtonID);
			}
			return false;
		}

		public static bool GetKey(int PlayerID, string KeyID) {
			try {
				return MJHSC.MidtimeEngine.GameAPI.MInput.GetKey(
					(MJHSC.MidtimeEngine.GameAPI.MInput.KeyID)Enum.Parse(typeof(MJHSC.MidtimeEngine.GameAPI.MInput.KeyID), KeyID)
				);
			} catch {
				MDebug.WriteFormat("���݂��Ȃ��L�[�{�[�h�{�^�������w�肳��܂���: {0}", KeyID);
			}
			return false;
		}

		public static string GetLastKey() {
			return Enum.GetName(
				typeof(MJHSC.MidtimeEngine.GameAPI.MInput.KeyID)
				, MJHSC.MidtimeEngine.GameAPI.MInput.GetLastKey()
			);
		}

		public static int GetMouse(string MouseMoveID) {
			try {
				return MJHSC.MidtimeEngine.GameAPI.MInput.GetMouse(
					(MJHSC.MidtimeEngine.GameAPI.MInput.MouseMoveID)Enum.Parse(typeof(MJHSC.MidtimeEngine.GameAPI.MInput.MouseMoveID), MouseMoveID)
				);
			} catch {
				MDebug.WriteFormat("���݂��Ȃ��}�E�X�ʒu�����w�肳��܂���: {0}", MouseMoveID);
			}
			return 0;
		}

		public static int GetStick(int PlayerID, string StickID) {
			try {
				return MJHSC.MidtimeEngine.GameAPI.MInput.GetStick(
					PlayerID
					, (MJHSC.MidtimeEngine.GameAPI.MInput.StickID)Enum.Parse(typeof(MJHSC.MidtimeEngine.GameAPI.MInput.StickID), StickID)
				);
			} catch {
				MDebug.WriteFormat("���݂��Ȃ��X�e�B�b�N���u{0}�v���w�肳��܂���", StickID);
			}
			return 0;
		}

		public static int GetTrigger(int PlayerID, string TriggerID) {
			try {
				return MJHSC.MidtimeEngine.GameAPI.MInput.GetTrigger(
					PlayerID
					, (MJHSC.MidtimeEngine.GameAPI.MInput.TriggerID)Enum.Parse(typeof(MJHSC.MidtimeEngine.GameAPI.MInput.TriggerID), TriggerID)
				);
			} catch {
				MDebug.WriteFormat("���݂��Ȃ��g���K�[���u{0}�v���w�肳��܂���", TriggerID);
			}
			return 0;
		}

	}

}