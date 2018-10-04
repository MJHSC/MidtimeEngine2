//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.

using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace MJHSC.MidtimeEngine.Loader {

	public class Startup {


		static Assembly LoadDLL(string FilePath){
			try {
				FileStream FS = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read | FileShare.Write | FileShare.Delete);
				byte[] ME = new byte[FS.Length];
				FS.Read(ME, 0, ME.Length);
				FS.Close();
				return Assembly.Load(ME);
			} catch { }
			return null;
		}

		public static void Main() {

			try {
				String MidtimeRoot = ".";
				try { //Midtime2.exe をパスの基点にする。
					MidtimeRoot = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
				} catch { //一部の特殊なローダーから起動した場合はGetExecutingAssemblyがnullになる。
					MidtimeRoot = Application.StartupPath;
				}
				Directory.SetCurrentDirectory(MidtimeRoot);

				//先に依存するPlugins.dllをロードしておく
				LoadDLL(".\\MidtimeEngine\\Binaries\\MJHSC.MidtimeEngine.Plugins.dll");

				//それから本体をロード
				Assembly A = LoadDLL(".\\MidtimeEngine\\Binaries\\MJHSC.MidtimeEngine.exe");

				//真のMainを探す
				Type T = A.GetType("MJHSC.MidtimeEngine.Startup");
				Object V = A.CreateInstance("MJHSC.MidtimeEngine.Startup", false, BindingFlags.NonPublic | BindingFlags.Instance, null, null, null, null);
				MethodInfo MI = T.GetMethod("MainEx", BindingFlags.NonPublic | BindingFlags.Static);

				//真のMainを起動
				MI.Invoke(V, new Object[] { MidtimeRoot });
			} catch (Exception E) {
				MessageBox.Show(string.Format("ゲームデータが破損しているため、起動に失敗しました。\nゲームの再インストールをお試しください。\n\nエラーコード: {0}\n\n{1}", E.GetType().ToString(), E.Message)
					, "Midtime Engine", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
			}
			Environment.Exit(0);
		}

	}

}