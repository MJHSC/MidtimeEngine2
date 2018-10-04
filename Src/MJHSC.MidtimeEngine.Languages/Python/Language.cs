//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.

using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using IronPython.Runtime;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using MJHSC.MidtimeEngine.GameAPI;
using MJHSC.MidtimeEngine.Plugins;

namespace MJHSC.MidtimeEngine.Languages.Python {
	public class Language : IMidtimeLanguage {

		public MidtimeResponse CanRunScript(string ScriptFilePath) {
			if (File.Exists(ScriptFilePath + ".py")) {
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
			ScriptEngine SE = IronPython.Hosting.Python.CreateEngine();
			ScriptScope SC = SE.CreateScope();

			string DefineGAPIs = "";
			//関数プラグインの適用
			for (int i = 0; i < Midtime.FuncPlugins.Length; i++) {
				SE.Runtime.LoadAssembly(Assembly.GetAssembly(Midtime.FuncPlugins[i].Classes[0].T));
				DefineGAPIs += "import " + Midtime.FuncPlugins[i].NameForImports + '\n';
			}

			//内蔵GameAPIは事前定義しておく
			Type[] GAPIs = (Assembly.GetAssembly(typeof(MJHSC.MidtimeEngine.GameAPI.MCore))).GetTypes();
			for (int i = 0; i < GAPIs.Length; i++) {
				if (GAPIs[i].FullName.IndexOf('+') != -1) { continue; }
				DefineGAPIs += (GAPIs[i].Name + " = " + GAPIs[i].FullName) + '\n';
			}
			DebugServer.Write(DefineGAPIs);
			SE.Execute(DefineGAPIs, SC);
			

			try {
				SE.ExecuteFile(ScriptFilePath + ".py", SC);
			} catch (SyntaxErrorException E) {
				MessageBox.Show(string.Format("スクリプト \"{0}.py\" の実行中にエラーが発生しました。\n\n場所: {2} 行目 {3} 文字目\n\n{1}"
	, ScriptFilePath, E.Message, E.Line, E.Column)
	, Midtime.METitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
			} catch (UnboundNameException E) {
				MessageBox.Show(string.Format("スクリプト \"{0}.py\" の実行中にエラーが発生しました。\n\n未定義の変数が使用されました。\n\n{1}"
, ScriptFilePath, E.Message)
, Midtime.METitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
			} catch (ThreadAbortException) { //スクリプトの終了
				return MidtimeResponse.OK;
			} catch (Exception E) {
				MessageBox.Show(string.Format("スクリプト \"{0}.py\" の実行中にエラーが発生しました。\n\n{1}", ScriptFilePath, E.Message), Midtime.METitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			return MidtimeResponse.OK;
		}

	}

}