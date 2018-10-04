//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.

using System;
using System.IO;
using System.Reflection;
using MJHSC.MidtimeEngine.Plugins;

namespace MJHSC.MidtimeEngine {

	public class DLLLoader {
		internal static Assembly LoadDLL(string FilePath) { //Midtime v2 alpha引継時のコードほぼそのまま
			string FName = Path.GetFileNameWithoutExtension(FilePath);
			DebugServer.Write("システムファイルを読み込み中: {0}", FName);

			if (File.Exists(FilePath)) {

				MidtimeFile FileLib = new MidtimeFile(FilePath);
				byte[] MemLib = new byte[FileLib.Length];
				FileLib.Read(MemLib, 0, (int)FileLib.Length);
				Assembly dll = null;
				try {
					dll = Assembly.Load(MemLib);
				} catch (Exception E) {
					DebugServer.Write("　システムファイルの読み込みに失敗「{0}: {1}」", E.GetType(), E.Message);
				}
				FileLib.Close();
				MemLib = null;
				GC.Collect(0);
				return dll;
			} else {
				DebugServer.Write("　システムファイルが見つかりませんでした。");
			}
			return null;
		}

		public static Assembly LoadDLLHandler(object AD, ResolveEventArgs e) {
			string AName = e.Name.Split(',')[0];
			//DebugServer.Write(string.Format("システムファイルを読み込み中*: {0}", AName));
			
			string DLLPath;
			string FilePath;

			//Midtime2.exeから起動されたMJHSC.MidtimeEngine.exeはシステム側にはMidtime2.exeで指定された別として認識されているので、
			//これをロードするdllがあった時に、新しくMJHSC.MidtimeEngine.exeをロードしてしまう。（GameAPIなど）
			//その場合、static等の空間が分離されてしまい、想定通りに動作しない。
			//それを回避するために、後続のロード機構を使用せず、自身をそのまま使用させる。
			if (AName == "MJHSC.MidtimeEngine") {
				return Assembly.GetAssembly(typeof(MJHSC.MidtimeEngine.Startup));
			}

			//読み込み済みのものにはキャッシュを返す 
			//(使用するMidtime2.exeがLightness版の場合は以下の方法ではうまく動作しないので、別途上記のロード機構が必要)
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