using System;
using System.Reflection;
using System.Threading;
using MJHSC.MidtimeEngine.Plugins;
using System.IO;
using System.Windows.Forms;

namespace MJHSC.MidtimeEngine {

	internal class GameLoop {


		internal struct ScriptThreadArgs {
			public LanguageVMEntry LVME;
			public string ScriptName;
		}

		internal static void ScriptThread(Object Arg) {
			ScriptThreadArgs Args = (ScriptThreadArgs)Arg; 
			DebugServer.Out("�X�N���v�g�u" + Args.ScriptName + "�v���J�n���܂����B");
			Args.LVME(Args.ScriptName);
			DebugServer.Out("�X�N���v�g�u" + Args.ScriptName + "�v���\�������I�����܂����B�V�X�e�����I�����܂��B");
			Thread.Sleep(1000 * 10);
			Environment.Exit(0);
		}


				

		internal static void MEMain() {
			
			//MJHSC.MidtimeEngine.Plugins �̃G���[�o�͂����_�C���N�g
			ErrorWriter.ErrorOutFunction = DebugServer.Write;

			//�v���O�C���i�����E�O���j�̃��[�h
			PluginsLoader PL = new PluginsLoader(Midtime.GetMidtimeRootPath(), DLLLoader.LoadDLL);
			PL.OpenAll();
			PL.Load();
			Midtime.LangPlugins = PL.GetLoadedLangPlugins();
			Midtime.FuncPlugins = PL.GetLoadedFuncPlugins();
			Midtime.FuncPluginsLegacy = PL.GetLoadedFuncPluginsLegacy();

			Midtime.GameWindow.Show();

			if (RunScene("Startup") != MidtimeResponse.OK) {
				//Midtime.GameWindow.Hide();
				MessageBox.Show("�X�^�[�g�A�b�v �X�N���v�g �̊J�n�Ɏ��s���܂����B", Midtime.METitle, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
				Environment.Exit(0);
			}

			while (true) {
				System.Windows.Forms.Application.DoEvents();
				Thread.Sleep(33);

				if (Midtime.ReloadSceneRequired) {
					Midtime.ReloadSceneRequired = false;
					if (RunScene(Midtime.NextSceneName) != MidtimeResponse.OK) {
						MessageBox.Show("�X�N���v�g�u"+Midtime.NextSceneName+"�v�̊J�n�Ɏ��s���܂����B", Midtime.METitle, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
						Environment.Exit(0);
					}
				}

				//���̑�����
				if (Midtime.GameWindow.IsDisposed) {
					break;
				}
			}

			DebugServer.Out("�E�B���h�E������ꂽ���߁A�I�����܂��B");
			Environment.Exit(0);
		}



		internal static MidtimeResponse RunScene(string ScriptName) {

			string ScriptDir = @".\GameData\Scripts\";

			DebugServer.Out("�X�N���v�g�u" + ScriptName + "�v���J�n���܂��B");

			LanguageVMEntry LVME = LoadScene(ScriptDir + ScriptName);
			if (LVME == null) {
				DebugServer.Out("�u" + ScriptName + "�v�������ł��錾��v���O�C����������܂���ł����B");
				return MidtimeResponse.Error; 
			}


			ScriptThreadArgs SA = new ScriptThreadArgs();
			{
				SA.LVME = LVME;
				SA.ScriptName = ScriptDir + ScriptName;
			}
			Midtime.SceneThread = new Thread(new ParameterizedThreadStart(ScriptThread));
			Midtime.SceneThread.SetApartmentState(ApartmentState.STA);

			Graphic.SBStart();
			Midtime.SceneThread.Start(SA);

			return MidtimeResponse.OK;
		}

		internal static LanguageVMEntry LoadScene(string ScriptName) {
			MidtimeResponse MR;
			LanguageVMEntry LVME = null;

			
			for (int i = 0; i < Midtime.LangPlugins.Length; i++) {
				MR = Midtime.LangPlugins[i].IML.CanRunScript(ScriptName);
				if (MR == MidtimeResponse.OK) {
					LVME = Midtime.LangPlugins[i].IML.StartVM(ScriptName);
					if (LVME != null) {
						DebugServer.Out(string.Format("�u{0}�v�͌���v���O�C���u{1}�v�ɂ���ď�������܂��B", ScriptName, Midtime.LangPlugins[i].Name));
						return LVME;
					}
				}
			}
			

			return null;
		}



	}
}