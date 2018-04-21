//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.

using System;
using System.IO;
using System.Windows.Forms;

namespace MJHSC.MidtimeEngine.GameAPI {
	public partial class MInput {

#region マウス

		internal static void Initialize_Mouse(Form Window) {
			Window.MouseDown += MouseDownEv;
			Window.MouseUp += MouseUpEv;
			Window.MouseWheel += MouseWheelEv;
			Window.MouseMove += MouseMoveEv;
		}

		internal static void MouseDownEv(Object sender, MouseEventArgs e) {
			MouseX = e.X;
			MouseY = e.Y;
			if (e.Button == MouseButtons.Left) { KeyData[(int)KeyID.LButton] = true; return; }
			if (e.Button == MouseButtons.Right) { KeyData[(int)KeyID.RButton] = true; return; }
			if (e.Button == MouseButtons.Middle) { KeyData[(int)KeyID.MButton] = true; return; }
		}

		internal static void MouseUpEv(Object sender, MouseEventArgs e) {
			MouseX = e.X;
			MouseY = e.Y;
			if (e.Button == MouseButtons.Left) { KeyData[(int)KeyID.LButton] = false; return; }
			if (e.Button == MouseButtons.Right) { KeyData[(int)KeyID.RButton] = false; return; }
			if (e.Button == MouseButtons.Middle) { KeyData[(int)KeyID.MButton] = false; return; }
		}

		internal static void MouseMoveEv(Object sender, MouseEventArgs e) {
			MouseX = e.X;
			MouseY = e.Y;
		}

		internal static void MouseWheelEv(Object sender, MouseEventArgs e) {
			MouseW = e.Delta;
			return;
		}
#endregion

#region キーボード
		internal static void KeyDownEv(object sender, System.Windows.Forms.KeyEventArgs e) {
			try {
				KeyData[(int)e.KeyCode] = true;
				MVCButton[1][(int)KB2VConvertTable[(int)e.KeyCode]] = true;
			} catch { } 
			LastKey = (KeyID) e.KeyCode;
		}
		internal static void KeyUpEv(object sender, System.Windows.Forms.KeyEventArgs e) {
			try {
				KeyData[(int)e.KeyCode] = false;
				MVCButton[1][(int)KB2VConvertTable[(int)e.KeyCode]] = false;
			} catch { } 
		}
#endregion

	}
}