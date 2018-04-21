//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;
using System.IO;
using System.Diagnostics;

namespace MJHSC.MidtimeEngine {

	public class MediaServer {

		private MediaServer() { } //Instance not allowed.

		private static Process P;

		private static void Watcher() {
			try {
				while (true) {
					Thread.Sleep(1000);
					if (P.HasExited) {
						StartServer();
					}
				}
			} catch { }
		}

		private static string GetServer() {
			return Midtime.GetMidtimeRootPath() + "MidtimeEngine\\Binaries\\MJHSC.MidtimeEngine.Server.Media.exe";
		}

		public static void Initialize() {
			try {
				StartServer();

				Thread W = new Thread(Watcher);
				W.Start();
			} catch { }
		}

		private static void StartServer() {
			P = new Process();
			P.StartInfo.UseShellExecute = false;
			P.StartInfo.RedirectStandardError = false;
			P.StartInfo.RedirectStandardInput = true;
			P.StartInfo.RedirectStandardOutput = true;
			P.StartInfo.WorkingDirectory = Midtime.GetMidtimeRootPath();
			P.StartInfo.FileName = GetServer();
			P.StartInfo.CreateNoWindow = true;
			try {
				P.Start();
				DebugServer.Out("メディアサーバーが開始されました。");
			} catch {
				MessageBox.Show("メデイア サービスの開始に失敗しました。", "MEDIA_SERVICE_START_FAILED - Midtime Engine", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Environment.Exit(2);
			}
		}

		public static string Read() {
			try {
				if (!P.HasExited) {
					return P.StandardOutput.ReadLine();
				} else {
					return "_SERVER_EXITED_";
				}
			} catch {
				return "_SERVER_EXITED_";
			}
		}

		public static void Write(string S) {
			try {
				if (!P.HasExited) {
					P.StandardInput.WriteLine(S);
				}
			} catch { }
		}


	}

}