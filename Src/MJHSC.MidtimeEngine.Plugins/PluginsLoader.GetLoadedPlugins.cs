//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.

using System;
using System.IO;
using System.Reflection;

namespace MJHSC.MidtimeEngine.Plugins {
	public partial class PluginsLoader {

		public FuncPluginInfo[] GetLoadedFuncPlugins() {
			int Count = 0;
			//有効なものを数える
			for (int i = 0; i < this.SystemFuncPlugins.Length; i++) {
				if (this.SystemFuncPlugins[i].Classes == null) { continue; }
				Count++;
			}
			for (int i = 0; i < this.UserFuncPlugins.Length; i++) {
				if (this.UserFuncPlugins[i].Classes == null) { continue; }
				Count++;
			}
			//ErrorWriter.Write(string.Format("有効なDLL: {0}/{1}", Count, this.SystemFuncPlugins.Length + this.UserFuncPlugins.Length));

			//有効なもののみ含まれた配列を返す
			FuncPluginInfo[] R = new FuncPluginInfo[Count];
			Count = 0;
			for (int i = 0; i < this.SystemFuncPlugins.Length; i++) {
				if (this.SystemFuncPlugins[i].Classes == null) { continue; }
				R[Count++] = this.SystemFuncPlugins[i];
			}
			for (int i = 0; i < this.UserFuncPlugins.Length; i++) {
				if (this.UserFuncPlugins[i].Classes == null) { continue; }
				R[Count++] = this.UserFuncPlugins[i];
			}
			return R;
		}

		public FuncPluginForLegacyScriptsInfo[] GetLoadedFuncPluginsLegacy() { // (v3以降では非.NET言語をサポートしなくなる可能性が0ではないので、GetLoadedFuncPluginsとは独立させておく。)
			int Count = 0;
			//有効なものを数える
			for (int i = 0; i < this.SystemFuncLegacyPlugins.Length; i++) {
				if (this.SystemFuncLegacyPlugins[i].Classes == null) { continue; }
				Count++;
			}
			for (int i = 0; i < this.UserFuncLegacyPlugins.Length; i++) {
				if (this.UserFuncLegacyPlugins[i].Classes == null) { continue; }
				Count++;
			}
			//ErrorWriter.Write(string.Format("有効なDLL: {0}/{1}", Count, this.SystemFuncLegacyPlugins.Length + this.UserFuncLegacyPlugins.Length));

			//有効なもののみ含まれた配列を返す
			FuncPluginForLegacyScriptsInfo[] R = new FuncPluginForLegacyScriptsInfo[Count];
			Count = 0;
			for (int i = 0; i < this.SystemFuncLegacyPlugins.Length; i++) {
				if (this.SystemFuncLegacyPlugins[i].Classes == null) { continue; }
				R[Count++] = this.SystemFuncLegacyPlugins[i];
			}
			for (int i = 0; i < this.UserFuncLegacyPlugins.Length; i++) {
				if (this.UserFuncLegacyPlugins[i].Classes == null) { continue; }
				R[Count++] = this.UserFuncLegacyPlugins[i];
			}
			return R;
		}

		public LangPluginInfo[] GetLoadedLangPlugins() {
			int Count = 0;
			for (int i = 0; i < this.SystemLangPlugins.Length; i++) {
				if (this.SystemLangPlugins[i].IML == null) { continue; }
				Count++;
			}
			for (int i = 0; i < this.UserLangPlugins.Length; i++) {
				if (this.SystemLangPlugins[i].IML == null) { continue; }
				Count++;
			}
			//ErrorWriter.Write(string.Format("有効なDLL: {0}/{1}", Count, this.SystemLangPlugins.Length + this.UserLangPlugins.Length);

			//有効なもののみ含まれた配列を返す
			LangPluginInfo[] R = new LangPluginInfo[Count];
			Count = 0;
			for (int i = 0; i < this.SystemLangPlugins.Length; i++) {
				if (this.SystemLangPlugins[i].IML == null) { continue; }
				R[Count++] = this.SystemLangPlugins[i];
			}
			for (int i = 0; i < this.UserLangPlugins.Length; i++) {
				if (this.SystemLangPlugins[i].IML == null) { continue; }
				R[Count++] = this.UserLangPlugins[i];
			}
			return R;
		}

	}
}