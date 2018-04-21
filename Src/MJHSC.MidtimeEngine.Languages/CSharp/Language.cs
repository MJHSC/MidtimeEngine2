//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Microsoft.CSharp;
using MJHSC.MidtimeEngine;
using MJHSC.MidtimeEngine.GameAPI;
using MJHSC.MidtimeEngine.Plugins;

namespace MJHSC.MidtimeEngine.Languages.CSharp {
	public class Language : IMidtimeLanguage {

		private EngineEntry LoadScript(string ScriptFilePath) {

			string CS = File.ReadAllText(ScriptFilePath + ".cs");

			CSharpCodeProvider CCSP = new CSharpCodeProvider(new Dictionary<string, string> { { "CompilerVersion", "v4.0" } });

			CompilerParameters CP = new CompilerParameters();
			CP.GenerateInMemory = true;
			CP.GenerateExecutable = false;

//			CP.CompilerOptions = "/t:library";
			for (int i = 0; i < Midtime.FuncPlugins.Length; i++) {
				CP.ReferencedAssemblies.Add(Midtime.FuncPlugins[i].PathForCompilerReferences);
			}

			CompilerResults CR = CCSP.CompileAssemblyFromSource(CP, CS);
			if (CR.Errors.Count > 0) {
				foreach (System.CodeDom.Compiler.CompilerError CE in CR.Errors) {
					string E = string.Format("ファイル名 {0}({1},{2}): {3}: {4}", CE.FileName, CE.Line, CE.Column, CE.ErrorNumber, CE.ErrorText);
					DebugServer.Write("C#コンパイルエラー: " + E);
					MessageBox.Show("C#コンパイルエラー\n\n" + E);
				}
			}

			Assembly dll = CR.CompiledAssembly;
			Type t = dll.GetType("MidtimeScript.Startup");
			MethodInfo Method = t.GetMethod("Main", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

			//Delegate
			return (EngineEntry)Delegate.CreateDelegate(typeof(EngineEntry), Method);

		}

		public MidtimeResponse CanRunScript(string ScriptFilePath) {
			if (File.Exists(ScriptFilePath + ".cs")) {
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