//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.

using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MJHSC.MidtimeEngine.Plugins;
using System.Reflection;
using System.IO;

namespace MJHSC.MidtimeEngine {

	public static partial class Midtime {

		//C#
		[DllImport("kernel32.dll")]
		internal static extern int GetPrivateProfileString(string lpApplicationName, string lpKeyName, string lpDefault, StringBuilder lpReturnedstring, int nSize, string lpFileName);

		static Midtime() {
			PathMidtimeRoot = Application.StartupPath + '\\';
			//return Path.GetDirectoryName(Application.ExecutablePath) + '\\';
			PathSaveData = PathMidtimeRoot + "SaveData\\";
			PathGameData = PathMidtimeRoot + "GameData\\";

		}

		//システム
		public static void SetWindowSize(int Width, int Height) {
			GameWindow.SetSize(Width, Height);
			Graphic.Initialize();
			GameWindow.MoveWindowToCenter();
		}
		internal static bool CheckXNA() {
			GraphicsAdapter GAdapter = GraphicsAdapter.DefaultAdapter;
			return true; //If XNA is not installed, call this function will cause exception.
		}

		//パス
		public static string GetMidtimeRootPath() { //オープンソース版Midtime2.exe(C#)、Lightness版Midtime2.exe（.NETインストールチェックあり）の両方に対応。
			return PathMidtimeRoot;
			//return Application.StartupPath + '\\';
			//return Path.GetDirectoryName(Application.ExecutablePath) + '\\';
		}
		public static string GetSaveDataPath() { //AppData\Roadmingへのデータ保存に対応する予定。必ずこの関数を使う。
			return PathSaveData;
		}
		public static string GetGameDataPath() {
			return PathGameData;
		}

		//スクリプト
		public static void Goto(string SceneName) {
			NextSceneName = SceneName;
			ReloadSceneRequired = true;
			Thread.CurrentThread.Abort(); //ここで呼び出し元自身も終了する。
		}

		//設定取得	
		internal static string GetConfig(string Key) {
			StringBuilder SB = new StringBuilder(1024);
			GetPrivateProfileString("Midtime", Key, string.Empty, SB, SB.Capacity, GetMidtimeRootPath() + "GameData\\Midtime\\Midtime.ini");
			return SB.ToString();
		}



	}
}