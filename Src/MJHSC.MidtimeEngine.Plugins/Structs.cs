//Midtime Engine
//	�{�\�[�X�R�[�h�̃��C�Z���X�ɂ��Ă͕t���� LICENSE.html �����ǂ݂��������B
//	Please read LICENSE.html for detail license information.

using MJHSC.MidtimeEngine;
using System;
using System.Reflection;
using System.IO;

namespace MJHSC.MidtimeEngine.Plugins {

	//����v���O�C�����i�v���O�C���̐������z��ɂ��Ďg���j
	public struct LangPluginInfo {
		public IMidtimeLanguage IML;
		public string Name;
	}

	//�N���X���i.NET�Ή��X�N���v�g�p�j
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


	//�֐����i.NET"��"�Ή��X�N���v�g�p�j
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

	//�N���X���i.NET"��"�Ή��X�N���v�g�p�j
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

	//C#�Ȃǂ�.NET�Ή�����p�֐��v���O�C�����i�v���O�C���̐������z��ɂ��Ďg���j
	public struct FuncPluginInfo {
		public string PathForCompilerReferences;
		public string NameForImports {
			get {
				return Path.GetFileNameWithoutExtension(this.PathForCompilerReferences);
			}
		}
		public MFClassInfo[] Classes;
	}

	//Lua�Ȃǂ�.NET"��"�Ή�����p�֐��v���O�C�����i�v���O�C���̐������z��ɂ��Ďg���j
	public struct FuncPluginForLegacyScriptsInfo {
		public string PathForCompilerReferences;
		public MFLegacyClassInfo[] Classes;
	}


}