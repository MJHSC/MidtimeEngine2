//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.

using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using MJHSC.MidtimeEngine.GameAPI;
using MJHSC.MidtimeEngine.Plugins;

namespace MJHSC.MidtimeEngine.Languages.HSP.asv3x {
	
	
	internal class HSP3DLL{
		[DllImport(@".\MidtimeEngine\Binaries\HSP.v3x.Language.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "_hsc_ini@16")]
		internal static extern void hsc_ini(int a1, string a2, int a3, int a4);

		[DllImport(@".\MidtimeEngine\Binaries\HSP.v3x.Language.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "_hsc_objname@16")]
		internal static extern void hsc_objname(int a1, string a2, int a3, int a4);

		[DllImport(@".\MidtimeEngine\Binaries\HSP.v3x.Language.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "_hsc_comp@16")]
		internal static extern int hsc_comp(int a1, int a2, int a3, int a4);

		[DllImport(@".\MidtimeEngine\Binaries\HSP.v3x.Language.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "_hsc_getmes@16")]
		internal static extern int hsc_getmes(StringBuilder a1, int a2, int a3, int a4);

		[DllImport(@".\MidtimeEngine\Binaries\HSP.v3x.Language.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "_hsc_compath@16")]
		internal static extern int hsc_compath(int a1, string a2, int a3, int a4);
	
	}


	internal class HSP3 {

		public delegate void DLLEntry(IntPtr info);

		internal static bool RunHSP(string ScriptPath, IMidtimeLanguage axv3x) {

			string TEMPAXName = Path.GetTempPath() + "\\MJHSC.MidtimeEngine.Languages.HSP.asv3x." + Path.GetFileName(ScriptPath);
			string TEMPAXFile = TEMPAXName + ".ax3";
			string CommonDir = Midtime.GetGameDataPath() + "Extras\\";

			HSP3DLL.hsc_ini(0, ScriptPath, 0, 0);
			HSP3DLL.hsc_compath(0, CommonDir, 0, 0); 
			HSP3DLL.hsc_objname(0, TEMPAXFile, 0, 0);
			int R = HSP3DLL.hsc_comp(0, 0, 0, 0);

			StringBuilder ErrMsg = new StringBuilder(3200);

			if(R != 0) {
				HSP3DLL.hsc_getmes(ErrMsg, 0, 0, 0);	// エラーを取得

				string ErrMsgEx = ErrMsg.ToString();
				ErrMsgEx = ErrMsgEx.Substring(ErrMsgEx.IndexOf('\n') + 1);
				ErrMsgEx = ErrMsgEx.Substring(ErrMsgEx.IndexOf('\n') + 1);
				MDebug.Write("HSP3コンパイルエラー「" + ErrMsgEx + "」");
				MessageBox.Show("HSP3コンパイルエラー\n\n" + ErrMsgEx, Midtime.METitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}


			LanguageVMEntry LVME = axv3x.StartVM(TEMPAXName);
			LVME(TEMPAXName);

			return true;
		}
	}

}