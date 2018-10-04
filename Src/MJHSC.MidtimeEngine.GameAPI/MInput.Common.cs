//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using MJHSC.MidtimeEngine.Plugins;

namespace MJHSC.MidtimeEngine.GameAPI {

	/// <summary>
	/// コントローラやキーボード、マウスからの入力を取得します。
	/// </summary>
	[MidtimeFunction2]
	public partial class MInput {

		#region MInput 全体の初期化
		static MInput() {
			Midtime.GameWindow.KeyDown += KeyDownEv;
			Midtime.GameWindow.KeyUp += KeyUpEv;
			Midtime.GameWindow.MouseDown += MouseDownEv;
			Midtime.GameWindow.MouseUp += MouseUpEv;
			Midtime.GameWindow.MouseWheel += MouseWheelEv;
			Midtime.GameWindow.MouseMove += MouseMoveEv;
			ReportGamePadStatus();
			Initialize_VController();
			Initialize_KB2VConverter();
		}
		#endregion

		#region キーコンフィグ ini サポート
		[DllImport("kernel32.dll")]	internal static extern int GetPrivateProfileString(string lpApplicationName, string lpKeyName, string lpDefault, StringBuilder lpReturnedstring, int nSize, string lpFileName);

		internal static string GetKeyBind(string Key) {
			StringBuilder SB = new StringBuilder(1024);
			GetPrivateProfileString("Midtime", Key, string.Empty, SB, SB.Capacity, Midtime.GetSaveDataPath() + "KeyConfig.ini");
			return SB.ToString();
		}
		#endregion

	}
}