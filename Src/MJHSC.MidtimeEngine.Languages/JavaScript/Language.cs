//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.

using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Microsoft.JScript;
using Microsoft.JScript.Vsa;
using MJHSC.MidtimeEngine.GameAPI;
using MJHSC.MidtimeEngine.Plugins;

namespace MJHSC.MidtimeEngine.Languages.JavaScript {
	public class Language : IMidtimeLanguage {

		public MidtimeResponse CanRunScript(string ScriptFilePath) {
			if (File.Exists(ScriptFilePath + ".js")) {
				return MidtimeResponse.OK;
			}
			return MidtimeResponse.Error;
		}

		public LanguageVMEntry StartVM(string ScriptFilePath) {
			return VMMain;
		}

		public MidtimeResponse EndVM(string ScriptFilePath) {
			return MidtimeResponse.OK;
		}

		public MidtimeResponse VMMain(string ScriptFilePath) {

			//関数プラグインの適用
			string[] References = new string[Midtime.FuncPlugins.Length];
			for (int i = 0; i < Midtime.FuncPlugins.Length; i++) {
				References[i] = Midtime.FuncPlugins[i].NameForImports;
			}

			GlobalScope GS = VsaEngine.CreateEngineAndGetGlobalScope(true, References);
			VsaEngine VE = GS.engine;

			//内蔵GameAPIは事前定義しておく(自動)
			Import.JScriptImport("MJHSC", VE);
			Import.JScriptImport("MJHSC.MidtimeEngine", VE);
			Import.JScriptImport("MJHSC.MidtimeEngine.GameAPI", VE);
			
			try {
				string Script = File.ReadAllText(ScriptFilePath + ".js");
				Microsoft.JScript.Eval.JScriptEvaluate(Script, "unsafe", VE);
			} catch (JScriptException E) {

				if (E.GetBaseException().GetType().Name == typeof(System.Threading.ThreadAbortException).Name) { //スクリプトの終了
					return MidtimeResponse.OK;
				}

				string[] Code = E.LineText.Split('\n');
				string CodeWhenError = "（不明）";
				try { CodeWhenError = Code[E.Line - 1].Substring(E.StartColumn - 1, (E.EndColumn - E.StartColumn)); } catch { } //万一取得に失敗すると、例外の例外が発生して下のエラーメッセージが出なくなる
				MessageBox.Show(string.Format("スクリプト \"{0}.js\" の実行中にエラーが発生しました。\n\n場所: {2} 行目 {3} 文字目: \"{4}\"\n\n{1}"
					, ScriptFilePath, E.Message, E.Line, E.Column, CodeWhenError)
					, Midtime.METitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
			} catch (ThreadAbortException) { //スクリプトの終了
				return MidtimeResponse.OK;
			} catch (Exception E) {

				MessageBox.Show(string.Format("スクリプト \"{0}.js\" の実行中にエラーが発生しました。\n\n{1}", ScriptFilePath, E.Message), Midtime.METitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}


			return MidtimeResponse.OK;
		}

	}

}