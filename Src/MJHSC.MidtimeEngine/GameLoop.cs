using System;
using System.Reflection;
using System.Threading;
using MJHSC.MidtimeEngine.Plugins;
using System.IO;
using System.Windows.Forms;

namespace MJHSC.MidtimeEngine {

	internal class GameLoop {


		internal struct ScriptThreadArgs {
			public LanguageVMEntry LVME;
			public string ScriptName;
		}

		internal static void ScriptThread(Object Arg) {
			ScriptThreadArgs Args = (ScriptThreadArgs)Arg; 
			DebugServer.Out("スクリプト「" + Args.ScriptName + "」を開始しました。");
			Args.LVME(Args.ScriptName);
			DebugServer.Out("スクリプト「" + Args.ScriptName + "」が予期せず終了しました。システムを終了します。");
			Thread.Sleep(1000 * 10);
			Environment.Exit(0);
		}


				

		internal static void MEMain() {
			
			//MJHSC.MidtimeEngine.Plugins のエラー出力をリダイレクト
			ErrorWriter.ErrorOutFunction = DebugServer.Write;

			//プラグイン（内蔵・外部）のロード
			PluginsLoader PL = new PluginsLoader(Midtime.GetMidtimeRootPath(), DLLLoader.LoadDLL);
			PL.OpenAll();
			PL.Load();
			Midtime.LangPlugins = PL.GetLoadedLangPlugins();
			Midtime.FuncPlugins = PL.GetLoadedFuncPlugins();
			Midtime.FuncPluginsLegacy = PL.GetLoadedFuncPluginsLegacy();

			Midtime.GameWindow.Show();

			if (RunScene("Startup") != MidtimeResponse.OK) {
				//Midtime.GameWindow.Hide();
				MessageBox.Show("スタートアップ スクリプト の開始に失敗しました。", Midtime.METitle, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
				Environment.Exit(0);
			}

			while (true) {
				System.Windows.Forms.Application.DoEvents();
				Thread.Sleep(33);

				if (Midtime.ReloadSceneRequired) {
					Midtime.ReloadSceneRequired = false;
					if (RunScene(Midtime.NextSceneName) != MidtimeResponse.OK) {
						MessageBox.Show("スクリプト「"+Midtime.NextSceneName+"」の開始に失敗しました。", Midtime.METitle, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
						Environment.Exit(0);
					}
				}

				//その他処理
				if (Midtime.GameWindow.IsDisposed) {
					break;
				}
			}

			DebugServer.Out("ウィンドウが閉じられたため、終了します。");
			Environment.Exit(0);
		}



		internal static MidtimeResponse RunScene(string ScriptName) {

			string ScriptDir = @".\GameData\Scripts\";

			DebugServer.Out("スクリプト「" + ScriptName + "」を開始します。");

			LanguageVMEntry LVME = LoadScene(ScriptDir + ScriptName);
			if (LVME == null) {
				DebugServer.Out("「" + ScriptName + "」を処理できる言語プラグインが見つかりませんでした。");
				return MidtimeResponse.Error; 
			}


			ScriptThreadArgs SA = new ScriptThreadArgs();
			{
				SA.LVME = LVME;
				SA.ScriptName = ScriptDir + ScriptName;
			}
			Midtime.SceneThread = new Thread(new ParameterizedThreadStart(ScriptThread));
			Midtime.SceneThread.SetApartmentState(ApartmentState.STA);

			Graphic.SBStart();
			Midtime.SceneThread.Start(SA);

			return MidtimeResponse.OK;
		}

		internal static LanguageVMEntry LoadScene(string ScriptName) {
			MidtimeResponse MR;
			LanguageVMEntry LVME = null;

			
			for (int i = 0; i < Midtime.LangPlugins.Length; i++) {
				MR = Midtime.LangPlugins[i].IML.CanRunScript(ScriptName);
				if (MR == MidtimeResponse.OK) {
					LVME = Midtime.LangPlugins[i].IML.StartVM(ScriptName);
					if (LVME != null) {
						DebugServer.Out(string.Format("「{0}」は言語プラグイン「{1}」によって処理されます。", ScriptName, Midtime.LangPlugins[i].Name));
						return LVME;
					}
				}
			}
			

			return null;
		}



	}
}