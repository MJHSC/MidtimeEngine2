//Midtime Engine
//	�{�\�[�X�R�[�h�̃��C�Z���X�ɂ��Ă͕t���� LICENSE.html �����ǂ݂��������B
//	Please read LICENSE.html for detail license information.

using System;
using System.Reflection;
using System.Runtime.InteropServices;
using MJHSC.MidtimeEngine.GameAPI;
using MJHSC.MidtimeEngine.Languages.Lua;
using MJHSC.MidtimeEngine.Plugins;

namespace MJHSC.MidtimeEngine.Languages.Lua {

	public class LuaLibrary {

		internal static ILuaAccess ILA;

		public static bool Register(ILuaAccess ILAccess) {
			ILA = ILAccess;

			DebugServer.Write("Lua��GameAPI��o�^���Ă��܂��B");
			for (int i = 0; i < Midtime.FuncPluginsLegacy.Length; i++) {
				for (int n = 0; n < Midtime.FuncPluginsLegacy[i].Classes.Length; n++) {
					//DebugServer.Write(string.Format("{0}, {1}: {2}", i, n, Midtime.FuncPluginsLegacy[i].Classes[n].T));
					ILA.RegisterFunction(AutoRegister(Midtime.FuncPluginsLegacy[i].Classes[n]), Midtime.FuncPluginsLegacy[i].Classes[n].Name);
				}
			}
			return true;
		}



		internal static LuaType.luaL_Reg[] AutoRegister(MFLegacyClassInfo LCI) {
			LuaType.luaL_Reg[] LR = new LuaType.luaL_Reg[LCI.Functions.Length + 1];
			for (int i = 0; i < LCI.Functions.Length; i++) {

				//�u���I�֐����g�p���āv���폜�B GameAPI.ScriptSupport�ɂ���Ď蓮�Ή��̕K�v�Ȋ֐��͂Ȃ��͂��B
				DebugServer.Write("Lua�֐��Ƃ���GameAPI�u{0}.{1}�v��o�^���Ă��܂��B", LCI.T, LCI.Functions[i].Name);

				LR[i].Name = LCI.Functions[i].Name;
				MFLegacyFuncInfo[] Closure_LFI = LCI.Functions;
				int Closure_I = i;

				LuaType.lua_CFunction F = delegate(IntPtr L) { //���I�Ȋ֐�

					ParameterInfo[] PI = Closure_LFI[Closure_I].Args;
					Object[] Args = new Object[PI.Length];
					for (int n = 0; n < PI.Length; n++) {

						//���炩���߁A�֐��v���O�C���̈�����Midtime�ɂ���āuint�v�ustring�v�ubool�v�̂ǂꂩ�ł��邱�Ƃ��ۏ؂���Ă���B�i���̑��͍l���Ȃ��Ă悢�j

						if (PI[n].ParameterType == typeof(int)) {
							Args[n] = (int)LuaLibrary.ILA.GetNextArgAsInt(n + 1);
							continue;
						}
						if (PI[n].ParameterType == typeof(string)) {
							Args[n] = LuaLibrary.ILA.GetNextArgAsString(n + 1);
							continue;
						}
						if (PI[n].ParameterType == typeof(bool)) {
							Args[n] = (LuaLibrary.ILA.GetNextArgAsBoolean(n + 1));
							continue;
						}

						//���Ή��̌^(����)
						Args[n] = 0;
					}

					Object R = Closure_LFI[Closure_I].Func.Invoke(LCI.T, Args);

					//���炩���߁A�֐��v���O�C���̕Ԓl��Midtime�ɂ���āuint�v�ustring�v�ubool�v�uvoid�v�̂ǂꂩ�ł��邱�Ƃ��ۏ؂���Ă���B�i���̑��͍l���Ȃ��Ă悢�j

					if (Closure_LFI[Closure_I].ReturnType == typeof(int)) {
						ILA.SetReturnValue((int)R);
						return 1;
					}
					if (Closure_LFI[Closure_I].ReturnType == typeof(string)) {
						ILA.SetReturnValue((string)R);
						return 1;
					}
					if (Closure_LFI[Closure_I].ReturnType == typeof(bool)) {
						ILA.SetReturnValue((bool)R);
						return 1;
					}
					if (Closure_LFI[Closure_I].ReturnType == typeof(void)) {
						return 1;
					}

					//���Ή��̌^(����)
					ILA.SetReturnValue(0);
					return 1;
				}; //delegate
				GCHandle.Alloc(F);

				LR[i].Function = F;
			}
			return LR;
		}



	}

}
