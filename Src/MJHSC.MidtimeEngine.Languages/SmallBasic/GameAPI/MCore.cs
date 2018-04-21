//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.SmallBasic.Library;
using MJHSC.MidtimeEngine;

namespace MJHSC.MidtimeEngine.Languages.SmallBasic.GameAPI {


	/// <summary>
	/// Midtime自体や、システムに関する操作を行います。
	/// </summary>
	[SmallBasicType]
	public static class MCore {

		static dynamic GACore;
		static dynamic GADebug;

		//静的コンストラクタ（SmallBasicに関係ない本体部分）
		private static void MCoreInitialize() {
			GADebug = new StaticMembersDynamicWrapper(MCore.GameAPI.GetType("MJHSC.MidtimeEngine.GameAPI.MDebug"));
			GACore = new StaticMembersDynamicWrapper(MCore.GameAPI.GetType("MJHSC.MidtimeEngine.GameAPI.MCore"));
			GADebug.Write("「MJHSC.MidtimeEngine.Languages.SmallBasic.GameAPI.MCore」がロードされました。");
		}

		/// <summary>
		/// Midtime を終了します。
		/// 終了ができないプラットフォームでは何も起きません。
		/// </summary>
		public static Primitive Exit() {
			GACore.Exit();
			return false;
		}

		/// <summary>
		/// 別のスクリプトをロードし、実行します。（今のスクリプトはここで終了します。）
		/// </summary>
		/// <param name="ScriptName">ロードしたいスクリプトの名前 (例: Startup)</param>>
		public static Primitive Goto(Primitive SceneName) {
			return GACore.Goto((string)SceneName);
		}

		/// <summary>
		/// 指定した時間の間、動作を停止します。長さはミリ秒(ms)単位です。（1000ミリ秒で1秒です）
		/// </summary>
		/// <param name="TimeInMS">停止する時間（ミリ秒）</param>
		/// <returns></returns>
		public static Primitive Sleep(Primitive TimeInMS) {
			GACore.Sleep((int) TimeInMS);
			return true;
		}


		/// <summary>
		/// 「while MCore.Loop()」 と使うことで、ゲームループを行います。
		/// プレイする機械によって速度が異なりますが、最速で動作します。
		/// 
		/// 通常は、代わりに「MCore.FPSLoop()」を使います。
		/// </summary>
		/// <returns></returns>
		public static Primitive Loop() {
			return GACore.Loop();
		}

		/// <summary>
		/// 「while MCore.FPSLoop()」と使うことで、ゲームループを行います。
		/// 速度（FPS）制御を行います
		/// </summary>
		/// <param name="TargetFPS">ゲームの動作FPS。（1～30）
		/// 通常は30を指定してください。
		/// 
		/// ※現在、30を超えるFPSの指定はサポートされていません。</param>
		/// <returns></returns>
		public static Primitive LoopFPS(Primitive TargetFPS) {
			return GACore.LoopFPS((int) TargetFPS);
		}






#region システム内部の関数
		internal struct ObjectManager {
			public bool Using;
			public dynamic Obj;
		}

		internal static int SearchUnusedOM(ObjectManager[] DOs) {
			for (int i = 1; i < DOs.Length; i++) {
				if (!DOs[i].Using) {
					DOs[i].Using = true;
					return i;
				}
			}
			return -1;
		}


		//MJHSC.MidtimeEngine.GameAPI の 間接的なロード
		internal static Assembly GameAPI;
		internal static Assembly GameAPISS;

		//MCore 静的コンストラクタ: SmallBasic、Midtimeどちらかから起動されたのかを判別。GameAPIはMidtimeからしか使えない。
		static MCore() {
			//SmallBasicから直接起動なのか、Midtimeからの起動なのかを確認
			Assembly[] A = AppDomain.CurrentDomain.GetAssemblies();

			bool MidtimeFlag = false;
			for (int i = 0; i < A.Length; i++) {
				if (A[i].GetName().Name == Assembly.GetExecutingAssembly().GetName().Name) {
					continue; //自身は別
				}
				if (A[i].GetName().Name.IndexOf("MJHSC.Midtime") == 0) {
					MidtimeFlag = true; //MJHSC.Midtimeから始まるdllがあればそれはきっとMidtime
				}
			}

			if (!MidtimeFlag) {
				//SmallBasic のデバッガーからの起動
				Process P = new Process();

				//SmallBasicから起動しているので、ExecutingAssemblyはGameData\Scriptsフォルダ。
				P.StartInfo.FileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\..\..\Midtime2.exe";
				P.Start();
				P.WaitForExit();
				Environment.Exit(0);
			}

			//Midtime からの起動。SmallBasicから起動している場合は、GameAPIがロードできないため
			//Midtimeであるからとわかってから動的にロードする必要がある。
			MCore.GameAPI = LoadCLRLibrary(".\\MidtimeEngine\\Binaries\\MJHSC.MidtimeEngine.GameAPI.dll");
			MCore.GameAPISS = LoadCLRLibrary(".\\MidtimeEngine\\Binaries\\MJHSC.MidtimeEngine.GameAPI.ScriptSupport.dll");
			MCoreInitialize();

		}

		internal static Assembly LoadCLRLibrary(string FilePath) { //Midtime v2 alpha引継時のコードそのまま
			if (System.IO.File.Exists(FilePath)) {
				FileStream FileLib = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				byte[] MemLib = new byte[FileLib.Length];
				FileLib.Read(MemLib, 0, (int)FileLib.Length);
				Assembly dll = null;
				try {
					dll = Assembly.Load(MemLib);
				} catch { }
				FileLib.Close();
				MemLib = null;
				GC.Collect(0);
				return dll;
			}
			return null;
		}

		private static void Main() { }	//古いVS内蔵のC#コンパイラだとSmallBasic （.NET 4.5）が拡張プラグインとして認識しないDLLができるのでVSが出力したファイルは使わない（ダミーのEXEを出させる）
										//	VisualStudio 2010 だと問題発生、2010 SP1だと問題起きない可能性。

#endregion
		
	}

}