//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.

using System.IO;
using MJHSC.MidtimeEngine.Plugins;
using MJHSC.MidtimeEngine.Languages.HSP;

namespace MJHSC.MidtimeEngine.Languages.HSP.axv26 {
	public class Language : IMidtimeLanguage {

		public MidtimeResponse CanRunScript(string ScriptFilePath) {
			if (File.Exists(ScriptFilePath + ".ax2")){
				return MidtimeResponse.OK;
			}
			if (File.Exists(ScriptFilePath + ".ax") && File.ReadAllText(ScriptFilePath + ".ax").Substring(0, 4) == "HSP2") {
				return MidtimeResponse.OK;
			}
			return MidtimeResponse.Error;
		}

		public LanguageVMEntry StartVM(string ScriptFilePath) {
			return VMMain;
		}

		public MidtimeResponse EndVM(string ScriptFilePath) {
			return MidtimeResponse.OK;
		}

		public MidtimeResponse VMMain(string ScriptFilePath) {

			HSPLibrary.Register();

			if (File.Exists(ScriptFilePath + ".ax2")){
				HSP2.RunHSP(ScriptFilePath + ".ax2");
			}
			if (File.Exists(ScriptFilePath + ".ax")){
				HSP2.RunHSP(ScriptFilePath + ".ax");
			}			
			return MidtimeResponse.OK;
		}

	}

}