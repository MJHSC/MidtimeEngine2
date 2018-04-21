//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

//THIS CODE MUST BUILD AS x86. If others, will cause 277 error in open

namespace MJHSC.MidtimeEngine.LicenseService {
	class MediaServer {

		[DllImport("winmm.dll")]
		private static extern int mciSendString(string command, System.Text.StringBuilder buffer, int bufferSize, IntPtr hwndCallback);

		private static readonly uint LifeTimeFull = 14;
		private static uint LifeTime;

		private static void LifeTimer() {
			while (MediaServer.LifeTime > 0u) {
				MediaServer.LifeTime -= 1u;
				//Console.WriteLine(MediaServer.LifeTime);
				Thread.Sleep(1000);
			}
			Console.WriteLine("Timeout");
			Environment.Exit(0);
		}


		[STAThread]
		private static void Main() {

			MediaServer.LifeTime = MediaServer.LifeTimeFull;

			//Thread thread = new Thread(new ThreadStart(MediaServer.LifeTimer));
			//thread.Start();

			StringBuilder stringBuilder = new StringBuilder(128);

			while (MediaServer.LifeTime > 0) {
				try {
					string text = Console.ReadLine();
					if (text == null) { Environment.Exit(0); } //STDIN が閉じた

					MediaServer.LifeTime = MediaServer.LifeTimeFull;
					string[] array = text.Split('|');
					string key;
					switch (key = array[0]) {
					case "Open":
						mciSendString(string.Format("open \"{0}\" type MPEGVIDEO alias {1}", array[1], array[2]), null, 0, IntPtr.Zero);
						break;
					case "Close":
						mciSendString(string.Format("close {0}", array[1]), null, 0, IntPtr.Zero);
						break;
					case "Play":
						mciSendString(string.Format("play {0}", array[1]), null, 0, IntPtr.Zero);
						break;
					case "Loop":
						mciSendString(string.Format("play {0} repeat", array[1]), null, 0, IntPtr.Zero);
						break;
					case "Stop":
						mciSendString(string.Format("stop {0}", array[1]), null, 0, IntPtr.Zero);
						mciSendString(string.Format("seek {0} to start", array[1]), null, 0, IntPtr.Zero);
						break;
					case "Pause":
						mciSendString(string.Format("pause {0}", array[1]), null, 0, IntPtr.Zero);
						break;
					case "GetLength": {
							if (mciSendString(string.Format("status {0} length", array[1]), stringBuilder, 128, IntPtr.Zero) == 9) {
								Console.WriteLine(stringBuilder.ToString());
							} else {
								Console.WriteLine("0");
							}
							break;
						}
					case "GetPosition": {
							if (mciSendString(string.Format("status {0} position", array[1]), stringBuilder, 128, IntPtr.Zero) == 0) {
								Console.WriteLine(stringBuilder.ToString());
							} else {
								Console.WriteLine("0");
							}
							break;
						}
					case "SetPosition":
						mciSendString(string.Format("seek {0} to {1}", array[1], array[2]), null, 0, IntPtr.Zero);
						mciSendString(string.Format("play {0}", array[1], array[2]), null, 0, IntPtr.Zero);
						break;
					case "SetVolume":
						mciSendString(string.Format("setaudio {0} volume to {1}", array[1], array[2]), null, 0, IntPtr.Zero);
						break;
					case "GetVolume":							
							if (mciSendString(string.Format("status {0} volume", array[1]), stringBuilder, 128, IntPtr.Zero) == 0) {
								Console.WriteLine(stringBuilder.ToString());
							} else {
								Console.WriteLine("0");
							}
						break;
					case "SetSpeed":
						int A = mciSendString(string.Format("set {0} speed {1}", array[1], array[2]), null, 0, IntPtr.Zero);
						break;
					case "GetSpeed":
						if (mciSendString(string.Format("status {0} speed", array[1]), stringBuilder, 128, IntPtr.Zero) == 0) {
							Console.WriteLine(stringBuilder.ToString());
						} else {
							Console.WriteLine("0");
						}
						break;
					case "Exit":
						Environment.Exit(0);
						break;
					case "*MEv1*":
						mciSendString(array[1], stringBuilder, 128, IntPtr.Zero);
						Console.WriteLine(stringBuilder.ToString());
						break;
					}
				} catch (NullReferenceException E) { //STDINが閉じた
					Environment.Exit(0);
				} catch (IOException E) { //IOエラー 直接起動したとき等。Windowアプリなので直接だとコンソールが割り当てられない
					Environment.Exit(0);
				} catch {
					Thread.Sleep(100);
				}
			}
		}
	}
}
