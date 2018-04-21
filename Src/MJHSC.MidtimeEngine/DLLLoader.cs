//Midtime Engine
//	�{�\�[�X�R�[�h�̃��C�Z���X�ɂ��Ă͕t���� LICENSE.html �����ǂ݂��������B
//	Please read LICENSE.html for detail license information.

using System;
using System.IO;
using System.Reflection;
using MJHSC.MidtimeEngine.Plugins;

namespace MJHSC.MidtimeEngine {

	public class DLLLoader {
		internal static Assembly LoadDLL(string FilePath) { //Midtime v2 alpha���p���̃R�[�h�قڂ��̂܂�
			string FName = Path.GetFileNameWithoutExtension(FilePath);
			DebugServer.Write("�V�X�e���t�@�C����ǂݍ��ݒ�: {0}", FName);

			if (File.Exists(FilePath)) {

				MidtimeFile FileLib = new MidtimeFile(FilePath);
				byte[] MemLib = new byte[FileLib.Length];
				FileLib.Read(MemLib, 0, (int)FileLib.Length);
				Assembly dll = null;
				try {
					dll = Assembly.Load(MemLib);
				} catch (Exception E) {
					DebugServer.Write("�@�V�X�e���t�@�C���̓ǂݍ��݂Ɏ��s�u{0}: {1}�v", E.GetType(), E.Message);
				}
				FileLib.Close();
				MemLib = null;
				GC.Collect(0);
				return dll;
			} else {
				DebugServer.Write("�@�V�X�e���t�@�C����������܂���ł����B");
			}
			return null;
		}

		public static Assembly LoadDLLHandler(object AD, ResolveEventArgs e) {
			string AName = e.Name.Split(',')[0];
			//DebugServer.Write(string.Format("�V�X�e���t�@�C����ǂݍ��ݒ�*: {0}", AName));
			
			string DLLPath;
			string FilePath;

			//Midtime2.exe����N�����ꂽMJHSC.MidtimeEngine.exe�̓V�X�e�����ɂ�Midtime2.exe�Ŏw�肳�ꂽ�ʂƂ��ĔF������Ă���̂ŁA
			//��������[�h����dll�����������ɁA�V����MJHSC.MidtimeEngine.exe�����[�h���Ă��܂��B�iGameAPI�Ȃǁj
			//���̏ꍇ�Astatic���̋�Ԃ���������Ă��܂��A�z��ʂ�ɓ��삵�Ȃ��B
			//�����������邽�߂ɁA�㑱�̃��[�h�@�\���g�p�����A���g�����̂܂܎g�p������B
			if (AName == "MJHSC.MidtimeEngine") {
				return Assembly.GetAssembly(typeof(MJHSC.MidtimeEngine.Startup));
			}

			//�ǂݍ��ݍς݂̂��̂ɂ̓L���b�V����Ԃ� 
			//(�g�p����Midtime2.exe��Lightness�ł̏ꍇ�͈ȉ��̕��@�ł͂��܂����삵�Ȃ��̂ŁA�ʓr��L�̃��[�h�@�\���K�v)
			Assembly[] Already = AppDomain.CurrentDomain.GetAssemblies();
			for (int i = 0; i < Already.Length; i++) {
				if (Already[i].FullName.Split(',')[0] == AName) {
					return Already[i];
				}
			}


			DLLPath = Midtime.GetMidtimeRootPath() + "MidtimeEngine\\Binaries\\";

			if (AName == "MJHSC.MidtimeEngine.Languages.HSP.HSPPlugin") {
				return Assembly.LoadFile(DLLPath + AName + ".dll");
			}

			FilePath = DLLPath + AName + ".mep";
			if (File.Exists(FilePath)) {
				return LoadDLL(FilePath);
			}
			FilePath = DLLPath + AName + ".dll";
			if (File.Exists(FilePath)) {
				return LoadDLL(FilePath);
			}
			FilePath = DLLPath + AName + ".exe";
			if (File.Exists(FilePath)) {
				return LoadDLL(FilePath);
			}

			DLLPath = Midtime.GetMidtimeRootPath() + "GameData\\Plugins\\Binaries\\";
			FilePath = DLLPath + AName + ".mep";
			if (File.Exists(FilePath)) {
				return LoadDLL(FilePath);
			}
			FilePath = DLLPath + AName + ".dll";
			if (File.Exists(FilePath)) {
				return LoadDLL(FilePath);
			}
			FilePath = DLLPath + AName + ".exe";
			if (File.Exists(FilePath)) {
				return LoadDLL(FilePath);
			}

			return null;
		}

	}
}