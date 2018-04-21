//Midtime Engine
//	�{�\�[�X�R�[�h�̃��C�Z���X�ɂ��Ă͕t���� LICENSE.html �����ǂ݂��������B
//	Please read LICENSE.html for detail license information.

using System;
using System.IO;
using System.Reflection;
using MJHSC.MidtimeEngine.Plugins;
using MJHSC.MidtimeEngine.GameAPI;
using System.Runtime.InteropServices;

namespace MJHSC.MidtimeEngine.Languages.HSP.axv26 {


	internal class HSP2DLL {
		[DllImport(@".\MidtimeEngine\Binaries\HSP.v26.VM.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MidtimeVMEntry")]
		internal static extern int MidtimeVMEntry(string BootFilePath, string cmdline);
	}

	internal class HSP2 {

		static bool RunOnce = false;
		
		internal static bool RunHSP(string ScriptPath) {

			if (!RunOnce) {
				RunOnce = true;
				HSP2DLL.MidtimeVMEntry(Midtime.GetMidtimeRootPath() + "MidtimeEngine\\Binaries\\MJHSC.MidtimeEngine.Languages.HSP.v26Boot.bin", ScriptPath);
			} else {
				MDebug.Write("HSP2.6�̃V�X�e����̐����ɂ��AHSP2.6�̃X�N���v�g��1�񂵂����[�h�ł��܂���B");
				MDebug.Write("��HSP2.6�Ή���Midtime v1�Ƃ̌݊����̂��߂̂��̂ł��BHSP3�ȍ~�̂��g�p���������߂��܂��B");
				MDebug.Write("��Midtime v1�X�N���v�g�̏ꍇ�ŁA�ʂ�Midtime v1�X�N���v�g �V�[���֐؂�ւ���ꍇ�́A�]���̃V�[���؂�ւ����@�umStart�v�����̂܂܎g�p���Ă��������B�uMCore.Goto�v�Ȃǂɒu�������Ȃ��ł��������B");
				MDebug.Write("�@�܂���L�����ɂ��A�uMidtime.Goto�v���ɂ�葼�̌���ɂ��V�[���Ɉړ��������Midtime v1�X�N���v�g�֖߂邱�Ƃ͂ł��܂���B");
			}
			return true;
		}
	}

}