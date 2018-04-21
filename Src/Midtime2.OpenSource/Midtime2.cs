//Midtime Engine
//	�{�\�[�X�R�[�h�̃��C�Z���X�ɂ��Ă͕t���� LICENSE.html �����ǂ݂��������B
//	Please read LICENSE.html for detail license information.

using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace MJHSC.MidtimeEngine.Loader {

	class Startup {


		public static Assembly LoadDLL(string FilePath){
			try {
				FileStream FS = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read | FileShare.Write | FileShare.Delete);
				byte[] ME = new byte[FS.Length];
				FS.Read(ME, 0, ME.Length);
				FS.Close();
				return Assembly.Load(ME);
			} catch { }
			return null;
		}

		static void Main() {

			try {
				//Midtime2.exe ���p�X�̊�_�ɂ���B
				Directory.SetCurrentDirectory(Application.StartupPath);

				//��Ɉˑ�����Plugins.dll�����[�h���Ă���
				LoadDLL(".\\MidtimeEngine\\Binaries\\MJHSC.MidtimeEngine.Plugins.dll");

				//���ꂩ��{�̂����[�h
				Assembly A = LoadDLL(".\\MidtimeEngine\\Binaries\\MJHSC.MidtimeEngine.exe");

				//�^��Main��T��
				Type T = A.GetType("MJHSC.MidtimeEngine.Startup");
				Object V = A.CreateInstance("MJHSC.MidtimeEngine.Startup", false, BindingFlags.NonPublic | BindingFlags.Instance, null, null, null, null);
				MethodInfo MI = T.GetMethod("MainEx", BindingFlags.NonPublic | BindingFlags.Static);

				//�^��Main���N��
				MI.Invoke(V, null);
			} catch (Exception E) {
				MessageBox.Show(string.Format("�Q�[���f�[�^���j�����Ă��邽�߁A�N���Ɏ��s���܂����B\n�Q�[���̍ăC���X�g�[�������������������B\n\n�G���[�R�[�h: {0}\n\n{1}", E.GetType().ToString(), E.Message)
					, "Midtime Engine", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
			}
			Environment.Exit(0);
		}

	}

}