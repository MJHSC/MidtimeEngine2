//Midtime Engine
//	�{�\�[�X�R�[�h�̃��C�Z���X�ɂ��Ă͕t���� LICENSE.html �����ǂ݂��������B
//	Please read LICENSE.html for detail license information.

using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using MJHSC.MidtimeEngine.GameAPI;
using MJHSC.MidtimeEngine.Plugins;

namespace MJHSC.MidtimeEngine.Languages.Ruby {
	public class Language : IMidtimeLanguage {

		public MidtimeResponse CanRunScript(string ScriptFilePath) {
			if (File.Exists(ScriptFilePath + ".rb")) {
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
			ScriptEngine SE = IronRuby.Ruby.CreateEngine();
			ScriptScope SC = SE.CreateScope();

			//�֐��v���O�C���̓K�p
			for (int i = 0; i < Midtime.FuncPlugins.Length; i++) {
				SE.Runtime.LoadAssembly(Assembly.GetAssembly(Midtime.FuncPlugins[i].Classes[0].T));
			}
			
			//����GameAPI�͎��O��`���Ă���
			string DefineGAPIs = "";
			Type[] GAPIs = (Assembly.GetAssembly(typeof(MJHSC.MidtimeEngine.GameAPI.MCore))).GetTypes();
			for (int i = 0; i < GAPIs.Length; i++) {
				if (GAPIs[i].FullName.IndexOf('+') != -1) { continue; }
				DefineGAPIs += (GAPIs[i].Name + " = " + GAPIs[i].FullName.Replace(".", "::")) + '\n';
			}
			SE.Execute(DefineGAPIs, SC);

			try {
				SE.ExecuteFile(ScriptFilePath + ".rb", SC);
			} catch (SyntaxErrorException E) {
				MessageBox.Show(string.Format("�X�N���v�g \"{0}.rb\" �̎��s���ɃG���[���������܂����B\n\n�ꏊ: {2} �s�� {3} ������\n\n{1}"
	, ScriptFilePath, E.Message, E.Line, E.Column)
	, Midtime.METitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
			} catch (MemberAccessException E) {
				MessageBox.Show(string.Format("�X�N���v�g \"{0}.rb\" �̎��s���ɃG���[���������܂����B\n\n����`�̕ϐ����g�p����܂����B\n\n{1}"
, ScriptFilePath, E.Message)
, Midtime.METitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
			} catch (ThreadAbortException) { //�X�N���v�g�̏I��
				return MidtimeResponse.OK;
			} catch (Exception E) {
				MessageBox.Show(string.Format("�X�N���v�g \"{0}.rb\" �̎��s���ɃG���[���������܂����B\n\n{1}", ScriptFilePath, E.Message), Midtime.METitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			return MidtimeResponse.OK;
		}

	}

}