//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.

using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using MJHSC.MidtimeEngine.GameAPI;
using MJHSC.MidtimeEngine.Plugins;

namespace MJHSC.MidtimeEngine.Languages.NDP {
	public class Language : IMidtimeLanguage {

		private EngineEntry LoadScript(string ScriptFilePath) {
			try {
				//File
				MidtimeFile MF = new MidtimeFile(ScriptFilePath + ".dll");
				byte[] MemLib = new byte[MF.Length];
				MF.Read(MemLib, 0, (int)MF.Length);
				MF.Dispose();

				//Load
				Assembly dll = Assembly.Load(MemLib);
				Type t = dll.GetType("MidtimeScript.Startup");
				MethodInfo Method = t.GetMethod("Main", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

				//Delegate
				return (EngineEntry)Delegate.CreateDelegate(typeof(EngineEntry), Method);
			} catch { }
			return null;
		}

		public MidtimeResponse CanRunScript(string ScriptFilePath) {
			if (LoadScript(ScriptFilePath) != null) { return MidtimeResponse.OK; }
			return MidtimeResponse.Error;
		}

		public LanguageVMEntry StartVM(string ScriptFilePath) {
			return VMMain;
		}

		public MidtimeResponse EndVM(string ScriptFilePath) {
			return MidtimeResponse.OK;
		}

		internal delegate void EngineEntry();

		public MidtimeResponse VMMain(string ScriptFilePath) {
			EngineEntry EE = LoadScript(ScriptFilePath);
			if (EE != null) {
				try {
					EE();
				} catch (ThreadAbortException) { //スクリプトの終了
					return MidtimeResponse.OK;
				} catch (Exception E) {
					MessageBox.Show(string.Format("スクリプト \"{0}.cs\" の実行中にエラーが発生しました。\n\nエラー: {1}\n\n詳細: {2}\n{3}"
	, ScriptFilePath, E.GetType().ToString(), E.Message, E.StackTrace));
				}
			}
			return MidtimeResponse.OK;
		}

	}

}