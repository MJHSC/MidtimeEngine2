//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.

using MJHSC.MidtimeEngine;
using System;
using System.Reflection;
using System.IO;

namespace MJHSC.MidtimeEngine.Plugins {

	//言語プラグイン情報（プラグインの数だけ配列にして使う）
	public struct LangPluginInfo {
		public IMidtimeLanguage IML;
		public string Name;
	}

	//クラス情報（.NET対応スクリプト用）
	public struct MFClassInfo {
		public MidtimeFunction2Attribute Info;
		public Type T;
		public string Name {
			get {
				return T.Name;
			}
		}
		public string FullName {
			get {
				return T.FullName;
			}
		}
	}


	//関数情報（.NET"非"対応スクリプト用）
	public struct MFLegacyFuncInfo {
		public Type ReturnType;
		public MethodInfo Func;
		public ParameterInfo[] Args;
		public string Name {
			get {
				return Func.Name;
			}
		}
		public string ToString() {
			string A = "";
			for (int i = 0; i<this.Args.Length; i++) {
				A += this.Args[i].ParameterType.ToString() + ' ' + this.Args[i].Name + ", ";
			}
			return this.ReturnType.ToString() + ' ' + this.Func.Name + '(' + A + ')';
		}

		public static MFLegacyFuncInfo[] Merge(MFLegacyFuncInfo[] LFI1, MFLegacyFuncInfo[] LFI2){
			if (LFI2 == null) { return LFI1; }
			if (LFI1 == null) { return LFI2; }
			MFLegacyFuncInfo[] LFI = new MFLegacyFuncInfo[LFI1.Length + LFI2.Length];
			int n = 0;
			for (int i = 0; i < LFI1.Length; i++) {
				LFI[n++] = LFI1[i];
			}
			for (int i = 0; i < LFI2.Length; i++) {
				LFI[n++] = LFI2[i];
			}
			return LFI;
		}

	}

	//クラス情報（.NET"非"対応スクリプト用）
	public struct MFLegacyClassInfo {
		public MidtimeFunction2LegacyAttribute Info;
		public Type T;
		public string Name {
			get {
				return T.Name;
			}
		}
		public string FullName {
			get {
				return T.FullName;
			}
		}

		public MFLegacyFuncInfo[] Functions;

	}

	//C#などの.NET対応言語用関数プラグイン情報（プラグインの数だけ配列にして使う）
	public struct FuncPluginInfo {
		public string PathForCompilerReferences;
		public string NameForImports {
			get {
				return Path.GetFileNameWithoutExtension(this.PathForCompilerReferences);
			}
		}
		public MFClassInfo[] Classes;
	}

	//Luaなどの.NET"非"対応言語用関数プラグイン情報（プラグインの数だけ配列にして使う）
	public struct FuncPluginForLegacyScriptsInfo {
		public string PathForCompilerReferences;
		public MFLegacyClassInfo[] Classes;
	}


}