//Midtime Engine
//	�{�\�[�X�R�[�h�̃��C�Z���X�ɂ��Ă͕t���� LICENSE.html �����ǂ݂��������B
//	Please read LICENSE.html for detail license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

using MJHSC.MidtimeEngine.Plugins;

namespace MJHSC.MidtimeEngine {
	internal class Startup {
		private Startup() {
		}

		[DllImport("winmm.dll")]
		internal static extern uint timeBeginPeriod(uint uMilliseconds);

		[DllImport("winmm.dll")]
		internal static extern uint timeEndPeriod(uint uMilliseconds);

		private static void Main() {
			MessageBox.Show("���C�Z���X�̌��؂Ɏ��s���܂����B\n\n�����`���[�iMidtime2.exe�j���g�p���Ă��������B", "LICENSE_ERROR_NO_LAUNCHER - Midtime", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}

		internal static void LicenseThread() {
			while (true) {
				LicenseServer.Write("Check");
				string text = LicenseServer.Read();
				if (text == "LICENSE_SERVER_EXITED") {
					Midtime.GameWindow.Hide();
					MessageBox.Show("Midtime ���C�Z���X�̌��؂Ɏ��s���܂����B(" + text + ")", "Midtime", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
					Environment.Exit(1);
				}
				Thread.Sleep(3000);
			}
		}

		[STAThread]
		private static void MainEx() {
			try {
				Startup.MainMidtime();
			} catch (Exception E) {
				MessageBox.Show(string.Format("�X�N���v�g�̎��s���ɃG���[���������܂����B\n\n{0}\n\n{1}", E.Message, E.StackTrace), "Midtime", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
			}
		}

		private static void MainMidtime() {
			//������
			Startup.timeBeginPeriod(1u);
			Control.CheckForIllegalCrossThreadCalls = false;

			//DLL���[�_�̗L����
			AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(DLLLoader.LoadDLLHandler);

			//�f�o�b�O�T�[�o�N��
			DebugServer.Initialize();
			DebugServer.Out(string.Format("{0} �o�[�W���� {1} ���J�n���܂����B{2} {3}", Midtime.METitle, Midtime.MEVersion, Midtime.MECopyrigths, Midtime.MEURL));

			//XNA�̃C���X�g�[���`�F�b�N
			try {
				Midtime.CheckXNA();
				DebugServer.Out("XNA Framework 4.0��ǂݍ��݂܂����B");
			} catch {
				DebugServer.Out("XNA Framework 4.0�̓ǂݍ��݂Ɏ��s���܂����B");
				DialogResult dialogResult = MessageBox.Show("���̃Q�[�����N������ɂ́AMicrosoft XNA Framework 4.0 �̃C���X�g�[�����K�v�ł��B\n\n�������C���X�g�[�����ăQ�[�����N�����܂����H", Midtime.METitle, MessageBoxButtons.YesNo, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
				Assembly.GetEntryAssembly();
				if (dialogResult == DialogResult.Yes) {
					try {
						Process XNAInstaller = new Process();
						XNAInstaller.StartInfo.FileName = "msiexec";
						XNAInstaller.StartInfo.Arguments = "/i \"" + Midtime.GetMidtimeRootPath() + "\\MidtimeEngine\\Runtimes\\xnafx40_redist.msi\" /passive";
						XNAInstaller.Start();
						XNAInstaller.WaitForExit();

						Thread.Sleep(1000);

						Process Midtime2 = new Process();
						Midtime2.StartInfo.FileName = Application.ExecutablePath;
						Midtime2.StartInfo.Arguments = "";
						Midtime2.Start();
					} catch {}
				}
				Environment.Exit(0);
			}

			//���C�Z���X�T�[�o�N��
			LicenseServer.Initialize();

			//�E�B���h�E�E�O���t�B�b�N�E�T�E���h��������
			Midtime.GameWindow = new GameWindow();
			Graphic.Initialize();
			MediaServer.Initialize();
			
			//���C�Z���X�`�F�b�J�[�L����
			Thread thread = new Thread(new ThreadStart(Startup.LicenseThread));
			thread.Start();


			//�v���O�C���i�����E�O���j�̃��[�h �����̃N���X�ōs���ƁAMJHSC.MidtimeEngine.Plugins.dll����L��DLL���[�_�ŏ����ł��Ȃ��Ȃ�̂ŁA
			//GameLoop.MEMain�Ń��[�h���s��DLL������x��������B

			//�Q�[�����[�v�J�n�i�X�N���v�g���s�ցj
			GameLoop.MEMain();
		}
	}
}
