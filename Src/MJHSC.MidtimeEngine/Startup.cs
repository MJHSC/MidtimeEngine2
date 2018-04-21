//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

using MJHSC.MidtimeEngine.Plugins;

namespace MJHSC.MidtimeEngine {
	internal class Startup {
		private Startup() {
		}

		[DllImport("winmm.dll")]
		internal static extern uint timeBeginPeriod(uint uMilliseconds);

		[DllImport("winmm.dll")]
		internal static extern uint timeEndPeriod(uint uMilliseconds);

		private static void Main() {
			MessageBox.Show("ライセンスの検証に失敗しました。\n\nランチャー（Midtime2.exe）を使用してください。", "LICENSE_ERROR_NO_LAUNCHER - Midtime", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}

		internal static void LicenseThread() {
			while (true) {
				LicenseServer.Write("Check");
				string text = LicenseServer.Read();
				if (text == "LICENSE_SERVER_EXITED") {
					Midtime.GameWindow.Hide();
					MessageBox.Show("Midtime ライセンスの検証に失敗しました。(" + text + ")", "Midtime", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
					Environment.Exit(1);
				}
				Thread.Sleep(3000);
			}
		}

		[STAThread]
		private static void MainEx() {
			try {
				Startup.MainMidtime();
			} catch (Exception E) {
				MessageBox.Show(string.Format("スクリプトの実行中にエラーが発生しました。\n\n{0}\n\n{1}", E.Message, E.StackTrace), "Midtime", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
			}
		}

		private static void MainMidtime() {
			//初期化
			Startup.timeBeginPeriod(1u);
			Control.CheckForIllegalCrossThreadCalls = false;

			//DLLローダの有効化
			AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(DLLLoader.LoadDLLHandler);

			//デバッグサーバ起動
			DebugServer.Initialize();
			DebugServer.Out(string.Format("{0} バージョン {1} を開始しました。{2} {3}", Midtime.METitle, Midtime.MEVersion, Midtime.MECopyrigths, Midtime.MEURL));

			//XNAのインストールチェック
			try {
				Midtime.CheckXNA();
				DebugServer.Out("XNA Framework 4.0を読み込みました。");
			} catch {
				DebugServer.Out("XNA Framework 4.0の読み込みに失敗しました。");
				DialogResult dialogResult = MessageBox.Show("このゲームを起動するには、Microsoft XNA Framework 4.0 のインストールが必要です。\n\n今すぐインストールしてゲームを起動しますか？", Midtime.METitle, MessageBoxButtons.YesNo, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
				Assembly.GetEntryAssembly();
				if (dialogResult == DialogResult.Yes) {
					try {
						Process XNAInstaller = new Process();
						XNAInstaller.StartInfo.FileName = "msiexec";
						XNAInstaller.StartInfo.Arguments = "/i \"" + Midtime.GetMidtimeRootPath() + "\\MidtimeEngine\\Runtimes\\xnafx40_redist.msi\" /passive";
						XNAInstaller.Start();
						XNAInstaller.WaitForExit();

						Thread.Sleep(1000);

						Process Midtime2 = new Process();
						Midtime2.StartInfo.FileName = Application.ExecutablePath;
						Midtime2.StartInfo.Arguments = "";
						Midtime2.Start();
					} catch {}
				}
				Environment.Exit(0);
			}

			//ライセンスサーバ起動
			LicenseServer.Initialize();

			//ウィンドウ・グラフィック・サウンド等初期化
			Midtime.GameWindow = new GameWindow();
			Graphic.Initialize();
			MediaServer.Initialize();
			
			//ライセンスチェッカー有効化
			Thread thread = new Thread(new ThreadStart(Startup.LicenseThread));
			thread.Start();


			//プラグイン（内蔵・外部）のロード をこのクラスで行うと、MJHSC.MidtimeEngine.Plugins.dllを上記のDLLローダで処理できなくなるので、
			//GameLoop.MEMainでロードを行いDLL解決を遅延させる。

			//ゲームループ開始（スクリプト実行へ）
			GameLoop.MEMain();
		}
	}
}
