//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.

using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using MJHSC.MidtimeEngine.GameAPI;
using MJHSC.MidtimeEngine.Languages.HSP.HSPPlugin;
using MJHSC.MidtimeEngine.Plugins;

namespace MJHSC.MidtimeEngine.Languages.HSP {

	public class HSPLibrary {

		private static Hashtable CacheDB = new Hashtable();

		private static IHSPAccess HSPAccess = new HSPAccessAll();

		public static void Register() {
			CacheDB = new Hashtable();
			HSPProxy.cmdfunc = cmdfunc;
		}


		private static bool SearchFunction(string FuncName) {
			if (FuncName.IndexOf('.') == -1) { return false; } //不正な名前
			string NS = Path.GetFileNameWithoutExtension(FuncName); //名前空間とクラス名
			string FN = Path.GetExtension(FuncName).Substring(1); //関数名

			DebugServer.Write("関数「{0}」を検索して登録中...", FuncName);

			for (int i = 0; i < Midtime.FuncPluginsLegacy.Length; i++) {
				for (int n = 0; n < Midtime.FuncPluginsLegacy[i].Classes.Length; n++) {
					if (
						(Midtime.FuncPluginsLegacy[i].Classes[n].FullName == NS) //フル関数名が一致 (例: MJHSC.MidtimeEngine.GameAPI.ScriptSupport.MCore.LoopFPS)
						|| (Midtime.FuncPluginsLegacy[i].Classes[n].Name == NS) //関数名が一致 (例: MCore.LoopFPS)
					) {
						for (int m = 0; m < Midtime.FuncPluginsLegacy[i].Classes[n].Functions.Length; m++) {
							if (Midtime.FuncPluginsLegacy[i].Classes[n].Functions[m].Name == FN) {
								//次回以降探さなくて済むようにキャッシュに保存（高速化）
								CacheDB[FuncName] = Midtime.FuncPluginsLegacy[i].Classes[n].Functions[m];
								return true;
							}
						}
					}

				}
			}
			return false;
		}


		public static int cmdfunc(int p1) {

			if (p1 == 0x0000) {
				string FuncName = HSPAccess.GetNextArgAsString();

				//関数がキャッシュになければ探す
				if (!CacheDB.ContainsKey(FuncName)) {					
					if (!SearchFunction(FuncName)) {//探しても、まだないならその関数は存在しない
						//Error
						DebugServer.Write("関数「{0}」が見つかりません", FuncName);
						return 0;
					}
				}

				MFLegacyFuncInfo LFI = (MFLegacyFuncInfo)CacheDB[FuncName];
				//引数取得・生成
				object[] Args = new object[LFI.Args.Length];
				for (int i = 0; i < LFI.Args.Length; i++) {
					if (LFI.Args[i].ParameterType == typeof(int)) { Args[i] = HSPAccess.GetNextArgAsInt(); }
					if (LFI.Args[i].ParameterType == typeof(bool)) { Args[i] = ((HSPAccess.GetNextArgAsInt()) == 1); }
					if (LFI.Args[i].ParameterType == typeof(string)) { Args[i] = HSPAccess.GetNextArgAsString(); }
				}

				object R = LFI.Func.Invoke(null, Args);

				//返値
				if (LFI.ReturnType == typeof(int)) { HSPAccess.SetReturnValue((int)R); }
				if (LFI.ReturnType == typeof(bool)) { HSPAccess.SetReturnValue((bool)R); }
				if (LFI.ReturnType == typeof(string)) { HSPAccess.SetReturnValue((string)R); }

			}

			return 0;
		}

	}

}
