//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.

using System;
using System.IO;
using System.Reflection;

namespace MJHSC.MidtimeEngine.Plugins {
	public partial class PluginsLoader {

		//言語プラグインを検証し読み込む
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
					ErrorWriter.Write(string.Format("プラグイン「{0}」の検証に失敗しました。未対応のプラグインの可能性があります。({1})", FileList[i], E.GetType().ToString()));
				}
			}
			return LPIs;
		}

		//関数プラグインを検証し読み込む
		private FuncPluginInfo[] LoadFunc(string[] FileList) {

			FuncPluginInfo[] FPIs = new FuncPluginInfo[FileList.Length];

			for (int i = 0; i < FileList.Length; i++) {
				try {
					Assembly A = this.LoadDLL(FileList[i]);
					Type[] T = A.GetTypes();

					FuncPluginInfo FPI = new FuncPluginInfo();
					FPI.PathForCompilerReferences = FileList[i];

					//有効なクラスの数を取得
					int ClassesCount = 0;
					for (int n = 0; n < T.Length; n++) {
						//if (T[n].FullName.IndexOf('+') != -1) { continue; }
						if (T[n].GetCustomAttributes(typeof(MidtimeFunction2Attribute), false).Length == 0) { continue; }
						ClassesCount++;
					}

					//クラスの登録 & 属性の検証
					FPI.Classes = new MFClassInfo[ClassesCount];
					ClassesCount = 0;
					for (int n = 0; n < T.Length; n++) {
						//if (T[n].FullName.IndexOf('+') != -1) { continue; }
						var MF2A = (MidtimeFunction2Attribute[])T[n].GetCustomAttributes(typeof(MidtimeFunction2Attribute), false);
						if (MF2A.Length == 0) { continue; }
						//Console.WriteLine("検証された関数: {0}, {1}, {2}", FPI.PathForCompilerReferences, ClassesCount, T[n].FullName);

						FPI.Classes[ClassesCount].Info = MF2A[0];
						FPI.Classes[ClassesCount].T = T[n];
						ClassesCount++;
					}

					if (ClassesCount != 0) {
						FPIs[i] = FPI;
					}

				} catch (Exception E) {
					ErrorWriter.Write(string.Format("プラグイン「{0}」の検証に失敗しました。未対応のプラグインの可能性があります。({1})", FileList[i], E.GetType().ToString()));
				}
			}
			return FPIs;
		}


		//旧式スクリプト用関数が仕様に従っているかを検証
		private MFLegacyFuncInfo[] VerifyFuncForLegacy(Type T) {
			MFLegacyFuncInfo[] LFI_Dirty = VerifyFuncForLegacy_Generate(T);

			//有効なもののみを数える
			int Count = 0;
			for (int i = 0; i < LFI_Dirty.Length; i++) {
				if (LFI_Dirty[i].Func == null) { continue; }
				Count++;
			}

			//有効なもののみを返却する
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

				//関数そのもの
				LFI[i].Func = MI[i];
				
				//返値はint,string,bool(int[0/1]に変換される),voidのどれかでなければならない
				Type R = MI[i].ReturnType;
				if (
					(R != typeof(int))
					&& (R != typeof(string))
					&& (R != typeof(bool))
					&& (R != typeof(void))
					) {
						ErrorWriter.Write(string.Format("仕様違反の返値が検出されました。この関数「{1}」は無視されます。(「返値は仕様外の「{2}」です。)", T.FullName, MI[i].Name, R.Name));
						Verified = false;
				}
				LFI[i].ReturnType = R;

				//引数
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
							ErrorWriter.Write(string.Format("仕様違反の引数が検出されました。この関数「{1}」は無視されます。({2}番目の引数「{3}」が仕様外の「{4}」です。)", T.FullName, MI[i].Name, n, PI[n].Name, A.Name));
							Verified = false;
					}
				}

				if (!Verified) {
					LFI[i].Func = null; //仕様違反の関数はなかったことにする
				}
				//ErrorWriter.Write(string.Format("検証された旧式スクリプト用関数「{0}」", LFI[i].ToString()));

			}

			return LFI;
		}


		//旧式スクリプト用関数プラグインを検証し読み込む (v3以降では非.NET言語をサポートしなくなる可能性が0ではないので、LoadFuncとは独立させておく。)
		private FuncPluginForLegacyScriptsInfo[] LoadFuncLegacy(string[] FileList) {
			FuncPluginForLegacyScriptsInfo[] FPLIs = new FuncPluginForLegacyScriptsInfo[FileList.Length];
			
			for (int i = 0; i < FileList.Length; i++) {
				try {
					Assembly A = this.LoadDLL(FileList[i]);
					Type[] T = A.GetTypes();

					FuncPluginForLegacyScriptsInfo FPLI = new FuncPluginForLegacyScriptsInfo();
					FPLI.PathForCompilerReferences = FileList[i];

					//有効なクラスの数を取得
					int ClassesCount = 0;
					for (int n = 0; n < T.Length; n++) {
						//if (T[n].FullName.IndexOf('+') != -1) { continue; }
						if (T[n].GetCustomAttributes(typeof(MidtimeFunction2LegacyAttribute), false).Length == 0) { continue; }
						ClassesCount++;
					}

					//クラスの登録 & 属性の検証
					FPLI.Classes = new MFLegacyClassInfo[ClassesCount];
					ClassesCount = 0;
					
					for (int n = 0; n < T.Length; n++) {
						//if (T[n].FullName.IndexOf('+') != -1) { continue; }
						var MF2LA = (MidtimeFunction2LegacyAttribute[])T[n].GetCustomAttributes(typeof(MidtimeFunction2LegacyAttribute), false);
						if (MF2LA.Length == 0) { continue; }

						MFLegacyFuncInfo[] LFI1 = null;
						MFLegacyFuncInfo[] LFI2 = null;

						//引数・返値の仕様準拠チェック
						LFI1 = VerifyFuncForLegacy(T[n]);
						//if (LFI1 == null) { continue; } //無効なクラス全体でなく該当関数のみスキップするように変更

						//引数・返値の仕様準拠チェック（継承元）
						if (T[n].BaseType != typeof(object)) {
							LFI2 = VerifyFuncForLegacy(T[n].BaseType);
							//if (LFI2 == null) { continue; } //無効なクラス全体でなく該当関数のみスキップするように変更
						}

						//ErrorWriter.Write(string.Format("検証された旧式スクリプト用関数: {0}, {1}, {2}", FPLI.PathForCompilerReferences, ClassesCount, T[n].FullName));
						FPLI.Classes[ClassesCount].Functions = MFLegacyFuncInfo.Merge(LFI1, LFI2);
						FPLI.Classes[ClassesCount].Info = MF2LA[0];
						FPLI.Classes[ClassesCount].T = T[n];
						ClassesCount++;
					}

					if (ClassesCount != 0) {
						FPLIs[i] = FPLI;
					}


				} catch (Exception E) {
					ErrorWriter.Write(string.Format("プラグイン「{0}」の検証に失敗しました。未対応のプラグインの可能性があります。({1})", FileList[i], E.GetType().ToString()));
				}
			}
			return FPLIs;
		}


	}
}