//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.

using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MJHSC.MidtimeEngine.Plugins;
using System.Reflection;
using System.IO;

namespace MJHSC.MidtimeEngine {

	public static partial class Midtime {

		public static readonly string MEVersion = "2.00";
		public static readonly string MEBy = "MJHSC!";
		public static readonly string METitle = "Midtime";
		public static readonly string MECopyrigths = "(C)2016-2017 MJHSC!";
		public static readonly string MEURL = "http://mjhsc.nl/";

		private static string PathMidtimeRoot;
		private static string PathSaveData;
		private static string PathGameData;

		public static GameWindow GameWindow { get; internal set; }
		public static GraphicsDevice GDevice { get; internal set; }
		public static SpriteBatch SB { get; internal set; }
		
		public static bool ReloadSceneRequired = false;
		public static string NextSceneName = "";

		public static Thread SceneThread;

		public static LangPluginInfo[] LangPlugins;
		public static FuncPluginInfo[] FuncPlugins;
		public static FuncPluginForLegacyScriptsInfo[] FuncPluginsLegacy;

	}
	

}