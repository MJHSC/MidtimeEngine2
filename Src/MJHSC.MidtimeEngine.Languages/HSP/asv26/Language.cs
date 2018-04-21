//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.

using System.IO;
using MJHSC.MidtimeEngine.Plugins;

namespace MJHSC.MidtimeEngine.Languages.HSP.asv26 {
	public class Language : IMidtimeLanguage {

		internal IMidtimeLanguage axv26;
		public Language() {
			this.axv26 = new MJHSC.MidtimeEngine.Languages.HSP.axv26.Language();
		}

		public MidtimeResponse CanRunScript(string ScriptFilePath) {
			if (File.Exists(ScriptFilePath + ".hsp2")){
				return MidtimeResponse.OK;
			}
			if (File.Exists(ScriptFilePath + ".as")) {
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

			if (File.Exists(ScriptFilePath + ".hsp2")){
				HSP2.RunHSP(ScriptFilePath + ".hsp2", axv26);
			}
			if (File.Exists(ScriptFilePath + ".as")){
				HSP2.RunHSP(ScriptFilePath + ".as", axv26);
			}			
			return MidtimeResponse.OK;
		}

	}

}