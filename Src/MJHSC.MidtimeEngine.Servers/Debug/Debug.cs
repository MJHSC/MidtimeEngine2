//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.

using System;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Diagnostics;

namespace MJHSC.MidtimeEngine.DebugService {
	class DebugServer {

		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		internal static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, String lParam);
		internal static readonly uint EM_REPLACESEL = 0x00C2;

		static Form DebugWindow;
		static TextBox DebugLogBox;

		private static void FormServer() {
			DebugWindow = new Form();
			DebugWindow.Text = "Midtime Debug Message";
			DebugLogBox = new TextBox();
			DebugLogBox.Dock = DockStyle.Fill;
			DebugLogBox.Multiline = true;
			DebugLogBox.ReadOnly = true;
			DebugLogBox.Font = new Font("Meiryo", 11);
			DebugLogBox.ScrollBars = ScrollBars.Vertical;

			DebugLogBox.MaxLength = 0x20000;

			DebugWindow.Controls.Add(DebugLogBox);
			DebugWindow.Width = 960;
			DebugWindow.Height = 540;
			DebugWindow.Show();
			DebugWindow.Icon = new Icon(".\\GameData\\Midtime\\Midtime.ico", 16, 16);
			Application.Run();

			/*
			while (!DebugWindow.IsDisposed) {
				Thread.Sleep(10);
				Application.DoEvents();
			}
			*/

			Environment.Exit(0);
		}

		private static void FormWatcher() {
			while (!DebugWindow.IsDisposed) {
				Thread.Sleep(4000);
			}
			Environment.Exit(0);
		}

		[STAThread]
		private static void Main() {

			Thread TFormServer = new Thread(new ThreadStart(FormServer));
			TFormServer.Start();

			while (DebugLogBox == null) {
				Thread.Sleep(1000);
			}


			Thread TFormWatcher = new Thread(new ThreadStart(FormWatcher));
			TFormWatcher.Start();


			Stopwatch SW = new Stopwatch();
			SW.Reset();
			SW.Start();

			int LogWroteByte = 0;
			while (true) {
				try {
					string S = Console.ReadLine();
					if (S == null) { Environment.Exit(0); } //STDIN が閉じた

					//遅いのでTextBox.Textは使わない。
					//string Current = DebugLogBox.Text;
					//DebugLogBox.Text = 

					//バッファがあふれたらクリア。１行消して１行追加だとまた遅くなる。
					//それなりの大きさ確保されているので問題は少ないはず。（上部「DebugLogBox.MaxLength」）
					//少なくとも、ベースとなったLightness v0.9のコンソールログよりは多く残ってる。
					if ( (LogWroteByte+S.Length) >= (DebugLogBox.MaxLength)) { 
						DebugLogBox.Text = "";
						LogWroteByte = 0;
					}

					String Log = string.Format("[{0:F3}] {1}\r\n", (double)SW.ElapsedMilliseconds / 1000, S); // +Current;
					SendMessage(DebugLogBox.Handle, EM_REPLACESEL, IntPtr.Zero, Log); //高速版追記処理
					LogWroteByte += S.Length * 2; //UTF-16


				} catch (NullReferenceException E) { //STDINが閉じた
					Environment.Exit(0);
				} catch (IOException E) { //IOエラー 直接起動したとき等。Windowアプリなので直接だとコンソールが割り当てられない
					Environment.Exit(0);
				}
			}
		}
	}
}
