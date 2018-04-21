//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.

using System;
using System.IO;
using System.Reflection;

namespace MJHSC.MidtimeEngine.Plugins {
	public partial class PluginsLoader {

		public delegate Assembly DLLLoader(string FilePath);

		private string RootPath = "";
		private DLLLoader LoadDLL;

		private string[] SystemLangList = new string[0];
		private string[] UserLangList = new string[0];
		private string[] SystemFuncList = new string[0];
		private string[] UserFuncList = new string[0];

		private FuncPluginInfo[] SystemFuncPlugins;
		private FuncPluginForLegacyScriptsInfo[] SystemFuncLegacyPlugins;
		private LangPluginInfo[] SystemLangPlugins;

		private FuncPluginInfo[] UserFuncPlugins;
		private FuncPluginForLegacyScriptsInfo[] UserFuncLegacyPlugins;
		private LangPluginInfo[] UserLangPlugins;


		public PluginsLoader(string RootPath, DLLLoader DLLLoader) {
			this.RootPath = RootPath;
			this.LoadDLL = DLLLoader;
		}

		//内蔵関数プラグインを開き、ロードできるようにする
		public void OpenSystemFunc() {
			this.SystemFuncList = Directory.GetFiles(this.RootPath + "\\MidtimeEngine\\Binaries", "MJHSC.MidtimeEngine.*GameAPI*.dll", SearchOption.AllDirectories);
		}

		//外部関数プラグインを開き、ロードできるようにする
		public void OpenUserFunc() {
			this.UserFuncList = Directory.GetFiles(this.RootPath + "\\GameData\\Plugins\\Functions", "*", SearchOption.AllDirectories);
		}

		//内部言語プラグインを開き、ロードできるようにする
		public void OpenSystemLang() {
			this.SystemLangList = Directory.GetFiles(this.RootPath + "\\MidtimeEngine\\Binaries", "MJHSC.MidtimeEngine.Languages.*.dll", SearchOption.AllDirectories);
		}

		//外部言語プラグインを開き、ロードできるようにする
		public void OpenUserLang() {
			this.UserLangList = Directory.GetFiles(this.RootPath + "\\GameData\\Plugins\\Languages", "*", SearchOption.AllDirectories);
		}

		//全てを開く
		public void OpenAll() {
			this.OpenSystemLang();
			this.OpenSystemFunc();
			this.OpenUserLang();
			this.OpenUserFunc();
		}
		
		//Open***で開かれたプラグインをロードする。
		public void Load() {
			this.SystemFuncPlugins = LoadFunc(this.SystemFuncList);
			this.SystemFuncLegacyPlugins = LoadFuncLegacy(this.SystemFuncList);
			this.SystemLangPlugins = LoadLang(this.SystemLangList);

			this.UserFuncPlugins = LoadFunc(this.UserFuncList);
			this.UserFuncLegacyPlugins = LoadFuncLegacy(this.UserFuncList);
			this.UserLangPlugins = LoadLang(this.UserLangList);

		}
	}
}