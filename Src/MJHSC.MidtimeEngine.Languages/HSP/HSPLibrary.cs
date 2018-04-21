//Midtime Engine
//	�{�\�[�X�R�[�h�̃��C�Z���X�ɂ��Ă͕t���� LICENSE.html �����ǂ݂��������B
//	Please read LICENSE.html for detail license information.

using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using MJHSC.MidtimeEngine.GameAPI;
using MJHSC.MidtimeEngine.Languages.HSP.HSPPlugin;
using MJHSC.MidtimeEngine.Plugins;

namespace MJHSC.MidtimeEngine.Languages.HSP {

	public class HSPLibrary {

		private static Hashtable CacheDB = new Hashtable();

		private static IHSPAccess HSPAccess = new HSPAccessAll();

		public static void Register() {
			CacheDB = new Hashtable();
			HSPProxy.cmdfunc = cmdfunc;
		}


		private static bool SearchFunction(string FuncName) {
			if (FuncName.IndexOf('.') == -1) { return false; } //�s���Ȗ��O
			string NS = Path.GetFileNameWithoutExtension(FuncName); //���O��ԂƃN���X��
			string FN = Path.GetExtension(FuncName).Substring(1); //�֐���

			DebugServer.Write("�֐��u{0}�v���������ēo�^��...", FuncName);

			for (int i = 0; i < Midtime.FuncPluginsLegacy.Length; i++) {
				for (int n = 0; n < Midtime.FuncPluginsLegacy[i].Classes.Length; n++) {
					if (
						(Midtime.FuncPluginsLegacy[i].Classes[n].FullName == NS) //�t���֐�������v (��: MJHSC.MidtimeEngine.GameAPI.ScriptSupport.MCore.LoopFPS)
						|| (Midtime.FuncPluginsLegacy[i].Classes[n].Name == NS) //�֐�������v (��: MCore.LoopFPS)
					) {
						for (int m = 0; m < Midtime.FuncPluginsLegacy[i].Classes[n].Functions.Length; m++) {
							if (Midtime.FuncPluginsLegacy[i].Classes[n].Functions[m].Name == FN) {
								//����ȍ~�T���Ȃ��čςނ悤�ɃL���b�V���ɕۑ��i�������j
								CacheDB[FuncName] = Midtime.FuncPluginsLegacy[i].Classes[n].Functions[m];
								return true;
							}
						}
					}

				}
			}
			return false;
		}


		public static int cmdfunc(int p1) {

			if (p1 == 0x0000) {
				string FuncName = HSPAccess.GetNextArgAsString();

				//�֐����L���b�V���ɂȂ���ΒT��
				if (!CacheDB.ContainsKey(FuncName)) {					
					if (!SearchFunction(FuncName)) {//�T���Ă��A�܂��Ȃ��Ȃ炻�̊֐��͑��݂��Ȃ�
						//Error
						DebugServer.Write("�֐��u{0}�v��������܂���", FuncName);
						return 0;
					}
				}

				MFLegacyFuncInfo LFI = (MFLegacyFuncInfo)CacheDB[FuncName];
				//�����擾�E����
				object[] Args = new object[LFI.Args.Length];
				for (int i = 0; i < LFI.Args.Length; i++) {
					if (LFI.Args[i].ParameterType == typeof(int)) { Args[i] = HSPAccess.GetNextArgAsInt(); }
					if (LFI.Args[i].ParameterType == typeof(bool)) { Args[i] = ((HSPAccess.GetNextArgAsInt()) == 1); }
					if (LFI.Args[i].ParameterType == typeof(string)) { Args[i] = HSPAccess.GetNextArgAsString(); }
				}

				object R = LFI.Func.Invoke(null, Args);

				//�Ԓl
				if (LFI.ReturnType == typeof(int)) { HSPAccess.SetReturnValue((int)R); }
				if (LFI.ReturnType == typeof(bool)) { HSPAccess.SetReturnValue((bool)R); }
				if (LFI.ReturnType == typeof(string)) { HSPAccess.SetReturnValue((string)R); }

			}

			return 0;
		}

	}

}
