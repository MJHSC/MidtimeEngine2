//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.

using System;
using System.IO;
using System.Reflection;
using MJHSC.MidtimeEngine.Plugins;
using MJHSC.MidtimeEngine.GameAPI;
using System.Runtime.InteropServices;

namespace MJHSC.MidtimeEngine.Languages.HSP.axv3x {
	
	
	internal class HSP3DLL{
		[DllImport(@".\MidtimeEngine\Binaries\HSP.v3x.VM.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "_hspini@16")]
		internal static extern bool hspini(int a1, int a2, int a3, int a4);
		[DllImport(@".\MidtimeEngine\Binaries\HSP.v3x.VM.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "_hspbye@16")]
		internal static extern bool hspbye(int a1, int a2, int a3, int a4);
		[DllImport(@".\MidtimeEngine\Binaries\HSP.v3x.VM.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "_hspexec@16")]
		internal static extern bool hspexec(int a1, int a2, int a3, int a4);
		[DllImport(@".\MidtimeEngine\Binaries\HSP.v3x.VM.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "_hspprm@16")]
		internal static extern bool hspprm(int a1, int a2, int a3, IntPtr a4);
		[DllImport(@".\MidtimeEngine\Binaries\HSP.v3x.VM.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "_hspprm@16")]
		internal static extern bool hspprmR(int a1, int a2, int a3, ref IntPtr a4);
		[DllImport(@".\MidtimeEngine\Binaries\HSP.v3x.VM.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "_hspprm@16")]
		internal static extern bool hspprmS(int a1, int a2, int a3, string a4);
		[DllImport(@".\MidtimeEngine\Binaries\HSP.v3x.VM.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "_hspprm@16")]
		internal static extern bool hspprmD(int a1, int a2, int a3, HSP3.DLLEntry a4);
	}


	internal class HSP3 {

		public delegate void DLLEntry(IntPtr info);

		public static void Test(IntPtr Info) {
			MDebug.WriteFormat("HSP VMを起動しました。「{0}」", Info);
		}

		internal static bool RunHSP(string ScriptPath) {

			IntPtr hspctx = IntPtr.Zero;

			HSP3DLL.hspprm(0, 0, 0, IntPtr.Zero);
			HSP3DLL.hspprmR(0x100, 0, 0, ref hspctx);

			DLLEntry TEST = Test;
			GCHandle.Alloc(TEST);

			HSP3DLL.hspprmD(0x101, 0, 0, TEST);
			HSP3DLL.hspprmS(0x102, 0, 0, ScriptPath);
			HSP3DLL.hspini(0x03, 640, 480, 0);
			HSP3DLL.hspexec(0, 0, 0, 0);
			HSP3DLL.hspbye(0, 0, 0, 0);
			return true;
		}
	}

}