//Midtime Engine
//	�{�\�[�X�R�[�h�̃��C�Z���X�ɂ��Ă͕t���� LICENSE.html �����ǂ݂��������B
//	Please read LICENSE.html for detail license information.

using System.IO;
using MJHSC.MidtimeEngine.Plugins;

namespace MJHSC.MidtimeEngine.Languages.HSP.asv3x {
	public class Language : IMidtimeLanguage {

		internal IMidtimeLanguage axv3x;
		public Language() {
			this.axv3x = new MJHSC.MidtimeEngine.Languages.HSP.axv3x.Language();
		}

		public MidtimeResponse CanRunScript(string ScriptFilePath) {
			if (File.Exists(ScriptFilePath + ".hsp3")){
				return MidtimeResponse.OK;
			}
			if (File.Exists(ScriptFilePath + ".hsp")) {
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

			if (File.Exists(ScriptFilePath + ".hsp3")){
				HSP3.RunHSP(ScriptFilePath + ".hsp3", axv3x);
			}
			if (File.Exists(ScriptFilePath + ".hsp")){
				HSP3.RunHSP(ScriptFilePath + ".hsp", axv3x);
			}			
			return MidtimeResponse.OK;
		}

	}

}