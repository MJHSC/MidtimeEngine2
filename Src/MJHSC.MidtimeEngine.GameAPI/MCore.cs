//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.

using System;
using System.Diagnostics;
using System.Threading;
using MJHSC.MidtimeEngine;
using MJHSC.MidtimeEngine.Plugins;

namespace MJHSC.MidtimeEngine.GameAPI {

	/// <summary>
	/// Midtime スクリプトでの処理結果を表します。
	/// </summary>
	public enum MEResult : int{
		/// <summary>
		/// 処理に失敗しました。
		/// </summary>
		Error = 0,

		/// <summary>
		/// 処理に成功しました。
		/// </summary>
		OK = 1,
	}

	/// <summary>
	/// Midtime自体や、システムに関する操作を行います。
	/// </summary>
	[MidtimeFunction2]
	public class MCore {

		/// <summary>
		/// Midtime を終了します。
		/// 終了ができないプラットフォームでは何も起きません。
		/// </summary>
		public static void Exit() {
			Environment.Exit(0);
		}

		/// <summary>
		/// 別のスクリプトをロードし、実行します。（今のスクリプトはここで終了します。）
		/// </summary>
		/// <param name="SceneName">ロードしたいスクリプトの名前 (例: Startup)</param>>
		public static void Goto(string SceneName) {
			MMedia.Reset();
			MImage.Reset();
			Midtime.Goto(SceneName);
		}

		/// <summary>
		/// 指定した時間の間、動作を停止します。長さはミリ秒(ms)単位です。（1000ミリ秒で1秒です）
		/// </summary>
		/// <param name="TimeInMS">停止する時間（ミリ秒）</param>
		/// <returns></returns>
		public static void Sleep(int TimeInMS) {
			Thread.Sleep(TimeInMS);
		}
		
		/// <summary>
		/// 「while MCore.Loop()」 と使うことで、ゲームループを行います。
		/// プレイする機械によって速度が異なりますが、最速で動作します。
		/// 
		/// 通常は、代わりに「MCore.FPSLoop()」を使います。
		/// </summary>
		/// <returns></returns>
		public static bool Loop() {
			MImage.EndDraw();
			Thread.Sleep(1);
			MImage.StartDraw();
			return true;
		}


		internal static Stopwatch FPSTimer = new Stopwatch();
		/// <summary>
		/// 「while MCore.FPSLoop()」と使うことで、ゲームループを行います。
		/// 速度（FPS）制御を行います
		/// </summary>
		/// <param name="TargetFPS">ゲームの動作FPS。（1～30）
		/// 通常は30を指定してください。
		/// 
		/// ※現在、30を超えるFPSの指定はサポートされていません。</param>
		/// <returns></returns>
		public static bool LoopFPS(int TargetFPS) {
			if (TargetFPS == 0) { TargetFPS = 30; }

			MImage.EndDraw();

			long LastTime = FPSTimer.ElapsedMilliseconds; //前回のFPSLoop()からの時間 （≒処理時間）

			long FPSTime = (1000 / TargetFPS);
			long ExtraTime = FPSTime - LastTime;

			if (ExtraTime > 0) {
				Thread.Sleep((int)ExtraTime);
			} else {
				ExtraTime = 0;
				Thread.Sleep(1);
			}

			FPSTimer.Reset();
			FPSTimer.Start();

			//double CFPS = 1000 / (LastTime + ExtraTime);
			//MDebug.WriteFormat("FPS: {0}, Last: {1}, Extra: {2}, Target: {3}", CFPS, LastTime, ExtraTime, (int) TargetFPS);

			MImage.StartDraw();
			return true;
		}


	}

}

