//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
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
				MDebug.Write("HSP2.6のシステム上の制限により、HSP2.6のスクリプトは1回しかロードできません。");
				MDebug.Write("※HSP2.6対応はMidtime v1との互換性のためのものです。HSP3以降のご使用をおすすめします。");
				MDebug.Write("※Midtime v1スクリプトの場合で、別のMidtime v1スクリプト シーンへ切り替える場合は、従来のシーン切り替え方法「mStart」をそのまま使用してください。「MCore.Goto」などに置き換えないでください。");
				MDebug.Write("　また上記制限により、「Midtime.Goto」等により他の言語によるシーンに移動した後にMidtime v1スクリプトへ戻ることはできません。");
			}
			return true;
		}
	}

}