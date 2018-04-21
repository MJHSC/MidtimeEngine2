//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.

using System;
using System.Globalization;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace MJHSC.MidtimeEngine.LicenseService {
	class LicenseServer {

		[DllImport("urlmon.dll", CharSet = CharSet.Ansi)]
		private static extern int UrlMkSetSessionOption(
			int dwOption, string pBuffer, int dwBufferLength, int dwReserved);

		static readonly int URLMON_OPTION_USERAGENT = 0x10000001;


		private static readonly uint LifeTimeFull = 14;
		private static uint LifeTime;

		private static void LicenseService() {

			while (LicenseServer.LifeTime > 0) {
				try {
					string text = Console.ReadLine();
					if (text == null) { Environment.Exit(0); } //STDIN が閉じた

					LicenseServer.LifeTime = LicenseServer.LifeTimeFull;
					string[] array = text.Split('|');
					string key;
					switch (key = array[0]) {

						case "Check":
							Console.WriteLine("Verification-Key: MJHSC!");
							break;

						case "Exit":
							Environment.Exit(0);
							break;

						case "Title":
							AppName = array[1];
							ChangedValue = true;
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

			return;
		}


		private static bool ChangedValue = true;
		private static string EngineName = "Midtime";
		private static string EngineVer = "2.x";
		private static string AppName = "Unknown";

		[STAThread]
		private static void Main(string[] Args) {

			LicenseServer.LifeTime = LicenseServer.LifeTimeFull;

			Thread thread = new Thread(new ThreadStart(LicenseServer.LicenseService));
			thread.Start();


			//情報の取得
			string CPUName, GPUName, OSVer, OSType;
			CPUName = GPUName = OSVer = OSType = "";
			try {
				EngineVer = Args[0];
			} catch { }
			try {
				AppName = Args[1];
			} catch { }
			try {
				ManagementClass CPU = new ManagementClass("Win32_Processor");
				ManagementObjectCollection CPUs = CPU.GetInstances();
				foreach (ManagementObject Info in CPUs) {
					CPUName += (string)Info["Name"] + ", ";
					break;
				}
			} catch { }
			try {
				ManagementClass GPU = new ManagementClass("Win32_VideoController");
				ManagementObjectCollection GPUs = GPU.GetInstances();
				foreach (ManagementObject Info in GPUs) {
					GPUName += (string)Info["Name"] + ", ";
				}
			} catch { }
			try {
				OSVer = Environment.OSVersion.Version.ToString();
			} catch { }
			try {
				OSType = "Win32; x86";
				if (Environment.Is64BitOperatingSystem) {
					OSType = "Win64; x64";
				}
			} catch { }




			//メイン
			try {

				string DevInfo = string.Format("Midtime={0}&MEVer={1}&Lang={3}&Display={4}x{5}&CPU={6}&GPU={7}&OSVer={8}&OSType={9}&Title={2}", EngineName, EngineVer, AppName,
					CultureInfo.CurrentCulture.Name, SystemInformation.PrimaryMonitorSize.Width, SystemInformation.PrimaryMonitorSize.Height
					, CPUName, GPUName, OSVer, OSType);

				string UA = string.Format("User-Agent: {0}/{1} (Windows NT {2}; {3}) Title/\"{4}\"", EngineName, EngineVer, OSVer, OSType, AppName);


				UrlMkSetSessionOption(URLMON_OPTION_USERAGENT, UA, UA.Length, 0);

				Form F = new Form();
				WebBrowser WB = new WebBrowser();
				F.Controls.Add(WB);
				//F.Show();

				while (LicenseServer.LifeTime > 0) {
					if (ChangedValue) {
						ChangedValue = false;

	
						WB.Navigate(string.Format("https://mjhsc.nl/VerifyLicense?utm_source={0}&utm_medium=referral&{1}", EngineName+'_'+EngineVer, DevInfo), null, null, UA);
					}
					Thread.Sleep(400);
					Application.DoEvents();
				}
			} catch {
				//最低限の動作を維持
				while (LicenseServer.LifeTime > 0) {
					Thread.Sleep(1000);
				}
			}
		}
	}
}
