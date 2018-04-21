using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using MJHSC.MidtimeEngine;
using MJHSC.MidtimeEngine.GameAPI;
using MJHSC.MidtimeEngine.Plugins;

namespace MJHSC.MidtimeEngine.GameAPI.ScriptSupport {

	[Obsolete("この関数は一部の言語サポートのために内部で使用され、コンテンツでの使用は許可されていません。", false)]
	[MidtimeFunction2Legacy]
	public static class MInput {

		public static bool GetButton(int PlayerID, string ButtonID) {
			try {
				return MJHSC.MidtimeEngine.GameAPI.MInput.GetButton(
					PlayerID
					, (MJHSC.MidtimeEngine.GameAPI.MInput.ButtonID)Enum.Parse(typeof(MJHSC.MidtimeEngine.GameAPI.MInput.ButtonID), ButtonID)
				);
			} catch {
				MDebug.WriteFormat("存在しないボタン名「{0}」が指定されました", ButtonID);
			}
			return false;
		}

		public static bool GetKey(int PlayerID, string KeyID) {
			try {
				return MJHSC.MidtimeEngine.GameAPI.MInput.GetKey(
					(MJHSC.MidtimeEngine.GameAPI.MInput.KeyID)Enum.Parse(typeof(MJHSC.MidtimeEngine.GameAPI.MInput.KeyID), KeyID)
				);
			} catch {
				MDebug.WriteFormat("存在しないキーボードボタン名が指定されました: {0}", KeyID);
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
				MDebug.WriteFormat("存在しないマウス位置名が指定されました: {0}", MouseMoveID);
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
				MDebug.WriteFormat("存在しないスティック名「{0}」が指定されました", StickID);
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
				MDebug.WriteFormat("存在しないトリガー名「{0}」が指定されました", TriggerID);
			}
			return 0;
		}

	}

}