//Midtime Engine
//	�{�\�[�X�R�[�h�̃��C�Z���X�ɂ��Ă͕t���� LICENSE.html �����ǂ݂��������B
//	Please read LICENSE.html for detail license information.

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using MJHSC.MidtimeEngine.Plugins;

namespace MJHSC.MidtimeEngine.GameAPI {

	/// <summary>
	/// �R���g���[����L�[�{�[�h�A�}�E�X����̓��͂��擾���܂��B
	/// </summary>
	[MidtimeFunction2]
	public partial class MInput {

		#region MInput �S�̂̏�����
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

		#region �L�[�R���t�B�O ini �T�|�[�g
		[DllImport("kernel32.dll")]	internal static extern int GetPrivateProfileString(string lpApplicationName, string lpKeyName, string lpDefault, StringBuilder lpReturnedstring, int nSize, string lpFileName);

		internal static string GetKeyBind(string Key) {
			StringBuilder SB = new StringBuilder(1024);
			GetPrivateProfileString("Midtime", Key, string.Empty, SB, SB.Capacity, Midtime.GetSaveDataPath() + "KeyConfig.ini");
			return SB.ToString();
		}
		#endregion

	}
}