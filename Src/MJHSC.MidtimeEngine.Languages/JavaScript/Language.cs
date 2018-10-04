//Midtime Engine
//	�{�\�[�X�R�[�h�̃��C�Z���X�ɂ��Ă͕t���� LICENSE.html �����ǂ݂��������B
//	Please read LICENSE.html for detail license information.

using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Microsoft.JScript;
using Microsoft.JScript.Vsa;
using MJHSC.MidtimeEngine.GameAPI;
using MJHSC.MidtimeEngine.Plugins;

namespace MJHSC.MidtimeEngine.Languages.JavaScript {
	public class Language : IMidtimeLanguage {

		public MidtimeResponse CanRunScript(string ScriptFilePath) {
			if (File.Exists(ScriptFilePath + ".js")) {
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

			//�֐��v���O�C���̓K�p
			string[] References = new string[Midtime.FuncPlugins.Length];
			for (int i = 0; i < Midtime.FuncPlugins.Length; i++) {
				References[i] = Midtime.FuncPlugins[i].NameForImports;
			}

			GlobalScope GS = VsaEngine.CreateEngineAndGetGlobalScope(true, References);
			VsaEngine VE = GS.engine;

			//����GameAPI�͎��O��`���Ă���(����)
			Import.JScriptImport("MJHSC", VE);
			Import.JScriptImport("MJHSC.MidtimeEngine", VE);
			Import.JScriptImport("MJHSC.MidtimeEngine.GameAPI", VE);
			
			try {
				string Script = File.ReadAllText(ScriptFilePath + ".js");
				Microsoft.JScript.Eval.JScriptEvaluate(Script, "unsafe", VE);
			} catch (JScriptException E) {

				if (E.GetBaseException().GetType().Name == typeof(System.Threading.ThreadAbortException).Name) { //�X�N���v�g�̏I��
					return MidtimeResponse.OK;
				}

				string[] Code = E.LineText.Split('\n');
				string CodeWhenError = "�i�s���j";
				try { CodeWhenError = Code[E.Line - 1].Substring(E.StartColumn - 1, (E.EndColumn - E.StartColumn)); } catch { } //����擾�Ɏ��s����ƁA��O�̗�O���������ĉ��̃G���[���b�Z�[�W���o�Ȃ��Ȃ�
				MessageBox.Show(string.Format("�X�N���v�g \"{0}.js\" �̎��s���ɃG���[���������܂����B\n\n�ꏊ: {2} �s�� {3} ������: \"{4}\"\n\n{1}"
					, ScriptFilePath, E.Message, E.Line, E.Column, CodeWhenError)
					, Midtime.METitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
			} catch (ThreadAbortException) { //�X�N���v�g�̏I��
				return MidtimeResponse.OK;
			} catch (Exception E) {

				MessageBox.Show(string.Format("�X�N���v�g \"{0}.js\" �̎��s���ɃG���[���������܂����B\n\n{1}", ScriptFilePath, E.Message), Midtime.METitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}


			return MidtimeResponse.OK;
		}

	}

}