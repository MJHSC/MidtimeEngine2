//Midtime Engine
//	�{�\�[�X�R�[�h�̃��C�Z���X�ɂ��Ă͕t���� LICENSE.html �����ǂ݂��������B
//	Please read LICENSE.html for detail license information.

using System;
using System.IO;
using System.Reflection;

namespace MJHSC.MidtimeEngine.Plugins {
	public partial class PluginsLoader {

		//����v���O�C�������؂��ǂݍ���
		private LangPluginInfo[] LoadLang(string[] FileList) {

			LangPluginInfo[] LPIs = new LangPluginInfo[FileList.Length];

			for (int i = 0; i < FileList.Length; i++) {
				try {
					Assembly A = this.LoadDLL(FileList[i]);
					if (A == null) { continue; }
					Type[] T = A.GetTypes();
					for (int n = 0; n < T.Length; n++) {

						if (T[n].GetInterface(typeof(IMidtimeLanguage).FullName) != null) {
							LangPluginInfo LPI = new LangPluginInfo();
							LPI.Name = Path.GetFileNameWithoutExtension(FileList[i]);
							LPI.IML = (IMidtimeLanguage)A.CreateInstance(T[n].FullName);
							LPIs[i] = LPI;
							continue;
						}

					}
				} catch (Exception E) {
					ErrorWriter.Write(string.Format("�v���O�C���u{0}�v�̌��؂Ɏ��s���܂����B���Ή��̃v���O�C���̉\��������܂��B({1})", FileList[i], E.GetType().ToString()));
				}
			}
			return LPIs;
		}

		//�֐��v���O�C�������؂��ǂݍ���
		private FuncPluginInfo[] LoadFunc(string[] FileList) {

			FuncPluginInfo[] FPIs = new FuncPluginInfo[FileList.Length];

			for (int i = 0; i < FileList.Length; i++) {
				try {
					Assembly A = this.LoadDLL(FileList[i]);
					Type[] T = A.GetTypes();

					FuncPluginInfo FPI = new FuncPluginInfo();
					FPI.PathForCompilerReferences = FileList[i];

					//�L���ȃN���X�̐����擾
					int ClassesCount = 0;
					for (int n = 0; n < T.Length; n++) {
						//if (T[n].FullName.IndexOf('+') != -1) { continue; }
						if (T[n].GetCustomAttributes(typeof(MidtimeFunction2Attribute), false).Length == 0) { continue; }
						ClassesCount++;
					}

					//�N���X�̓o�^ & �����̌���
					FPI.Classes = new MFClassInfo[ClassesCount];
					ClassesCount = 0;
					for (int n = 0; n < T.Length; n++) {
						//if (T[n].FullName.IndexOf('+') != -1) { continue; }
						var MF2A = (MidtimeFunction2Attribute[])T[n].GetCustomAttributes(typeof(MidtimeFunction2Attribute), false);
						if (MF2A.Length == 0) { continue; }
						//Console.WriteLine("���؂��ꂽ�֐�: {0}, {1}, {2}", FPI.PathForCompilerReferences, ClassesCount, T[n].FullName);

						FPI.Classes[ClassesCount].Info = MF2A[0];
						FPI.Classes[ClassesCount].T = T[n];
						ClassesCount++;
					}

					if (ClassesCount != 0) {
						FPIs[i] = FPI;
					}

				} catch (Exception E) {
					ErrorWriter.Write(string.Format("�v���O�C���u{0}�v�̌��؂Ɏ��s���܂����B���Ή��̃v���O�C���̉\��������܂��B({1})", FileList[i], E.GetType().ToString()));
				}
			}
			return FPIs;
		}


		//�����X�N���v�g�p�֐����d�l�ɏ]���Ă��邩������
		private MFLegacyFuncInfo[] VerifyFuncForLegacy(Type T) {
			MFLegacyFuncInfo[] LFI_Dirty = VerifyFuncForLegacy_Generate(T);

			//�L���Ȃ��݂̂̂𐔂���
			int Count = 0;
			for (int i = 0; i < LFI_Dirty.Length; i++) {
				if (LFI_Dirty[i].Func == null) { continue; }
				Count++;
			}

			//�L���Ȃ��݂̂̂�ԋp����
			MFLegacyFuncInfo[] LFI_Clean = new MFLegacyFuncInfo[Count];
			Count = 0;
			for (int i = 0; i < LFI_Clean.Length; i++) {
				if (LFI_Dirty[i].Func == null) { continue; }
				LFI_Clean[Count++] = LFI_Dirty[i];
			}

			return LFI_Clean;
		}
		private MFLegacyFuncInfo[] VerifyFuncForLegacy_Generate(Type T) {
			

			MethodInfo[] MI = T.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Static);
			MFLegacyFuncInfo[] LFI = new MFLegacyFuncInfo[MI.Length];
			
			for (int i = 0; i < MI.Length; i++) {
				bool Verified = true;

				//�֐����̂���
				LFI[i].Func = MI[i];
				
				//�Ԓl��int,string,bool(int[0/1]�ɕϊ������),void�̂ǂꂩ�łȂ���΂Ȃ�Ȃ�
				Type R = MI[i].ReturnType;
				if (
					(R != typeof(int))
					&& (R != typeof(string))
					&& (R != typeof(bool))
					&& (R != typeof(void))
					) {
						ErrorWriter.Write(string.Format("�d�l�ᔽ�̕Ԓl�����o����܂����B���̊֐��u{1}�v�͖�������܂��B(�u�Ԓl�͎d�l�O�́u{2}�v�ł��B)", T.FullName, MI[i].Name, R.Name));
						Verified = false;
				}
				LFI[i].ReturnType = R;

				//����
				ParameterInfo[] PI = MI[i].GetParameters();
				LFI[i].Args = PI;
				for (int n = 0; n < PI.Length; n++) {
					Type A = PI[n].ParameterType;
					if (
						(A != typeof(int))
						&& (A != typeof(string))
						&& (A != typeof(bool))
						&& (A != typeof(void))
						) {
							ErrorWriter.Write(string.Format("�d�l�ᔽ�̈��������o����܂����B���̊֐��u{1}�v�͖�������܂��B({2}�Ԗڂ̈����u{3}�v���d�l�O�́u{4}�v�ł��B)", T.FullName, MI[i].Name, n, PI[n].Name, A.Name));
							Verified = false;
					}
				}

				if (!Verified) {
					LFI[i].Func = null; //�d�l�ᔽ�̊֐��͂Ȃ��������Ƃɂ���
				}
				//ErrorWriter.Write(string.Format("���؂��ꂽ�����X�N���v�g�p�֐��u{0}�v", LFI[i].ToString()));

			}

			return LFI;
		}


		//�����X�N���v�g�p�֐��v���O�C�������؂��ǂݍ��� (v3�ȍ~�ł͔�.NET������T�|�[�g���Ȃ��Ȃ�\����0�ł͂Ȃ��̂ŁALoadFunc�Ƃ͓Ɨ������Ă����B)
		private FuncPluginForLegacyScriptsInfo[] LoadFuncLegacy(string[] FileList) {
			FuncPluginForLegacyScriptsInfo[] FPLIs = new FuncPluginForLegacyScriptsInfo[FileList.Length];
			
			for (int i = 0; i < FileList.Length; i++) {
				try {
					Assembly A = this.LoadDLL(FileList[i]);
					Type[] T = A.GetTypes();

					FuncPluginForLegacyScriptsInfo FPLI = new FuncPluginForLegacyScriptsInfo();
					FPLI.PathForCompilerReferences = FileList[i];

					//�L���ȃN���X�̐����擾
					int ClassesCount = 0;
					for (int n = 0; n < T.Length; n++) {
						//if (T[n].FullName.IndexOf('+') != -1) { continue; }
						if (T[n].GetCustomAttributes(typeof(MidtimeFunction2LegacyAttribute), false).Length == 0) { continue; }
						ClassesCount++;
					}

					//�N���X�̓o�^ & �����̌���
					FPLI.Classes = new MFLegacyClassInfo[ClassesCount];
					ClassesCount = 0;
					
					for (int n = 0; n < T.Length; n++) {
						//if (T[n].FullName.IndexOf('+') != -1) { continue; }
						var MF2LA = (MidtimeFunction2LegacyAttribute[])T[n].GetCustomAttributes(typeof(MidtimeFunction2LegacyAttribute), false);
						if (MF2LA.Length == 0) { continue; }

						MFLegacyFuncInfo[] LFI1 = null;
						MFLegacyFuncInfo[] LFI2 = null;

						//�����E�Ԓl�̎d�l�����`�F�b�N
						LFI1 = VerifyFuncForLegacy(T[n]);
						//if (LFI1 == null) { continue; } //�����ȃN���X�S�̂łȂ��Y���֐��̂݃X�L�b�v����悤�ɕύX

						//�����E�Ԓl�̎d�l�����`�F�b�N�i�p�����j
						if (T[n].BaseType != typeof(object)) {
							LFI2 = VerifyFuncForLegacy(T[n].BaseType);
							//if (LFI2 == null) { continue; } //�����ȃN���X�S�̂łȂ��Y���֐��̂݃X�L�b�v����悤�ɕύX
						}

						//ErrorWriter.Write(string.Format("���؂��ꂽ�����X�N���v�g�p�֐�: {0}, {1}, {2}", FPLI.PathForCompilerReferences, ClassesCount, T[n].FullName));
						FPLI.Classes[ClassesCount].Functions = MFLegacyFuncInfo.Merge(LFI1, LFI2);
						FPLI.Classes[ClassesCount].Info = MF2LA[0];
						FPLI.Classes[ClassesCount].T = T[n];
						ClassesCount++;
					}

					if (ClassesCount != 0) {
						FPLIs[i] = FPLI;
					}


				} catch (Exception E) {
					ErrorWriter.Write(string.Format("�v���O�C���u{0}�v�̌��؂Ɏ��s���܂����B���Ή��̃v���O�C���̉\��������܂��B({1})", FileList[i], E.GetType().ToString()));
				}
			}
			return FPLIs;
		}


	}
}