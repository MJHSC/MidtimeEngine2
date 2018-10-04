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
	/// BGM や SE 、ビデオ の再生、停止、音量や速度等の設定を行います。
	/// </summary>
	[SmallBasicType]
	public static class MMedia {

		static dynamic GAMediaSS;
		static dynamic GADebug;

		//静的コンストラクタ
		static MMedia() {
			GADebug = new StaticMembersDynamicWrapper(MCore.GameAPI.GetType("MJHSC.MidtimeEngine.GameAPI.MDebug"));
			GAMediaSS = new StaticMembersDynamicWrapper(MCore.GameAPISS.GetType("MJHSC.MidtimeEngine.GameAPI.ScriptSupport.MMedia"));
			GADebug.Write("「MJHSC.MidtimeEngine.Languages.SmallBasic.GameAPI.MMedia」がロードされました。");
		}



		/// <summary>
		/// 指定されたファイルからサウンド（BGM・SE）を読み込み、サウンドIDを取得します。
		/// サウンドは「GameData\Sounds」フォルダに保存されている必要があります。
		/// </summary>
		/// <param name="FileName">ファイル名 （.\GameData\Sounds\）</param>
		/// <returns>サウンドID</returns>
		public static Primitive CreateSound(Primitive FileName) {
			return (int)GAMediaSS.CreateSound((string)FileName);
		}
	
		/// <summary>
		/// 指定されたファイルからサウンド（BGM・SE）を読み込み、ビデオIDを取得します。
		/// サウンドは「GameData\Sounds」フォルダに保存されている必要があります。
		/// </summary>
		/// <param name="FileName">ファイル名 （.\GameData\Videos\）</param>
		/// <returns>ビデオID</returns>
		public static Primitive CreateVideo(Primitive FileName) {
			return (int)GAMediaSS.CreateVideo((string)FileName);
		}

		/// <summary>
		/// すべてのサウンドコントローラを削除してリセットします。
		/// スクリプトが変わる際は自動的に実行されます。
		/// </summary>
		public static Primitive Reset() {
			GAMediaSS.Reset();
			return true;
		}



		/// <summary>
		/// 読み込まれたサウンド・ビデオを一度だけ再生します。
		/// SEやビデオの再生に最適です。
		/// </summary>
		/// <param name="MMediaID">サウンドID または ビデオID (MMedia.CreateSound または MMedia.CreateVideo で取得できます。)</param>
		public static Primitive PlayOnce(Primitive MMediaID) {
			GAMediaSS.PlayOnce((int)MMediaID);
			return true;
		}

		/// <summary>
		/// 読み込まれたサウンド・ビデオを繰り返し再生します。
		/// BGMの再生に最適です。
		/// </summary>
		/// <param name="MMediaID">サウンドID または ビデオID (MMedia.CreateSound または MMedia.CreateVideo で取得できます。)</param>
		public static Primitive PlayLoop(Primitive MMediaID) {
			GAMediaSS.PlayLoop((int)MMediaID);
			return true;
		}

		/// <summary>
		/// 現在読み込まれている、サウンド・ビデオを停止します。
		/// 再度、再生した場合は最初から再生されます。
		/// </summary>
		/// <param name="MMediaID">サウンドID または ビデオID (MMedia.CreateSound または MMedia.CreateVideo で取得できます。)</param>
		public static Primitive Stop(Primitive MMediaID) {
			GAMediaSS.Stop((int)MMediaID);
			return true;
		}

		/// <summary>
		/// 現在読み込まれている、サウンド・ビデオを停止します。
		/// 再度、再生した場合は Pause した位置から再生されます。
		/// </summary>
		/// <param name="MMediaID">サウンドID または ビデオID (MMedia.CreateSound または MMedia.CreateVideo で取得できます。)</param>
		public static Primitive Pause(Primitive MMediaID) {
			GAMediaSS.Pause((int)MMediaID);
			return true;
		}

		/// <summary>
		/// 現在読み込まれている、サウンド・ビデオを閉じて、読み込みを行う前の状態に戻します。
		/// これを行った サウンドコントローラ はもう使用できません。
		/// </summary>
		/// <param name="MMediaID">サウンドID または ビデオID (MMedia.CreateSound または MMedia.CreateVideo で取得できます。)</param>
		public static Primitive Close(Primitive MMediaID) {
			GAMediaSS.Close((int)MMediaID);
			return true;
		}

		/// <summary>
		/// 現在読み込まれている、サウンド・ビデオの「長さ」を取得します。 
		/// 長さはミリ秒(1ms)単位です。（1000ミリ秒で1秒です）
		/// </summary>
		/// <param name="MMediaID">サウンドID または ビデオID (MMedia.CreateSound または MMedia.CreateVideo で取得できます。)</param>
		/// <returns>長さ (ミリ秒)</returns>
		public static Primitive GetLength(Primitive MMediaID) {
			return (int)GAMediaSS.GetLength((int)MMediaID);
		}

		/// <summary>
		/// 現在読み込まれている、サウンド・ビデオの「現在の再生位置」を取得します。 
		/// 長さはミリ秒(1ms)単位です。（1000ミリ秒で1秒です）
		/// </summary>
		/// <param name="MMediaID">サウンドID または ビデオID (MMedia.CreateSound または MMedia.CreateVideo で取得できます。)</param>
		/// <returns>現在の再生位置 (ミリ秒)</returns>
		public static Primitive GetPosition(Primitive MMediaID) {
			return (int)GAMediaSS.GetPosition((int)MMediaID);
		}

		/// <summary>
		/// 現在読み込まれている、指定したサウンド・ビデオの「再生位置」を変更します。 
		/// 長さはミリ秒(1ms)単位です。（1000ミリ秒で1秒です）
		/// </summary>
		/// <param name="MMediaID">サウンドID または ビデオID (MMedia.CreateSound または MMedia.CreateVideo で取得できます。)</param>
		/// <param name="p">新しい再生位置 (ミリ秒)</param>
		public static Primitive SetPosition(Primitive MMediaID, Primitive p) {
			GAMediaSS.SetPosition((int)MMediaID, (int)p);
			return true;
		}

		/// <summary>
		/// 現在読み込まれている、サウンド・ビデオの「音量」を取得します。 
		/// </summary>
		/// <param name="MMediaID">サウンドID または ビデオID (MMedia.CreateSound または MMedia.CreateVideo で取得できます。)</param>
		/// <returns>現在の音量 (0～100)</returns>
		public static Primitive GetVolume(Primitive MMediaID) {
			return (int)GAMediaSS.GetVolume((int)MMediaID);
		}

		/// <summary>
		/// 現在読み込まれている、サウンド・ビデオの「音量」を変更します。 
		/// </summary>
		/// <param name="MMediaID">サウンドID または ビデオID (MMedia.CreateSound または MMedia.CreateVideo で取得できます。)</param>
		/// <param name="v">新しい音量 (0～100)</param>
		public static Primitive SetVolume(Primitive MMediaID, Primitive v) {
			GAMediaSS.SetPosSetVolumeition((int)MMediaID, (int)v);
			return true;
		}

		/// <summary>
		/// 現在読み込まれている、サウンド・ビデオの「速度」を取得します。
		/// 速度は1000で等速（x1.0）です。1200で1.2倍、800で0.8倍になります。
		/// </summary>
		/// <param name="MMediaID">サウンドID または ビデオID (MMedia.CreateSound または MMedia.CreateVideo で取得できます。)</param>
		/// <returns>再生速度 (x1.0 = 1000)</returns>
		public static Primitive GetSpeed(Primitive MMediaID) {
			return (int)GAMediaSS.GetSpeed((int)MMediaID);
		}

		/// <summary>
		/// 現在読み込まれている、サウンド・ビデオの「速度」を変更します。
		/// 速度は1000で等速（x1.0）です。1200で1.2倍、800で0.8倍になります。
		/// </summary>
		/// <param name="MMediaID">サウンドID または ビデオID (MMedia.CreateSound または MMedia.CreateVideo で取得できます。)</param>
		/// <param name="s">再生速度 (x1.0 = 1000)</param>
		public static Primitive SetSpeed(Primitive MMediaID, Primitive s) {
			GAMediaSS.SetSpeed((int)MMediaID, (int)s);
			return true;
		}

	}

}