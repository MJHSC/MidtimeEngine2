//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.

using System;
using System.IO;
using System.Windows.Forms;
using MJHSC.MidtimeEngine.Plugins;

namespace MJHSC.MidtimeEngine.GameAPI {

	/// <summary>
	/// BGM や SE 、ビデオ の再生、停止、音量や速度等の設定を行います。
	/// </summary>
	[MidtimeFunction2]
	public class MMedia {

		//Static
		private static MMedia[] MediaList = new MMedia[128];

		/// <summary>
		/// ランダムな文字を生成します。 (16文字)
		/// </summary>
		/// <returns>ランダムな文字 (16文字)</returns>
		private static string GetRandom() {
			var RND = new System.Security.Cryptography.RNGCryptoServiceProvider();
			byte[] Data = new byte[16];
			RND.GetBytes(Data);
			return (BitConverter.ToString(Data).ToLower().Replace("-", ""));
		}



		private static MMedia NewWithManagedList() {
			for (int i = 0; i < MediaList.Length; i++) {
				if (MediaList[i] == null) {
					MediaList[i] = new MMedia();
					MediaList[i].SelfListID = i;
					return MediaList[i];
				}
			}

			MDebug.WriteFormat("同時に使用可能な {0} の制限を超えました。 同時に使用できるのは {1}個までです。 不要なものを .Close();するか、 {0}.Reset(); してください。", "MMedia", MediaList.Length);
			return null; //管理領域からはみ出ている。この場合、エラーにさせる。
		}

		[Obsolete("この関数は一部の言語サポートのために内部で使用され、コンテンツでの使用は許可されていません。", false)]
		public static int _GetManagedID(MMedia M) {
			return M.SelfListID;
		}

		[Obsolete("この関数は一部の言語サポートのために内部で使用され、コンテンツでの使用は許可されていません。", false)]
		public static MMedia _GetMMediaByManagedID(int ID) {
			return MediaList[ID];
		}

		/// <summary>
		/// 指定されたファイルからサウンド（BGM・SE）を読み込み、サウンドコントローラを取得します。
		/// サウンドは「GameData\Sounds」フォルダに保存されている必要があります。
		/// </summary>
		/// <param name="FileName">ファイル名 （.\GameData\Sounds\）</param>
		/// <returns>MMedia コントローラ</returns>
		public static MMedia CreateSound(string FileName) {
			FileName = FileName.Replace("..", "__"); //簡易フォルダ参照禁止（この規制を回避してロードした場合、ほかプラットフォームへの移植は不可能に）
			FileName = FileName.Replace(':', '_'); //簡易ドライブ参照禁止（この規制を回避してロードした場合、ほかプラットフォームへの移植は不可能に）
			MMedia S = NewWithManagedList();
			S.LoadSound(FileName);
			return S;
		}

		/// <summary>
		/// 指定されたファイルからビデオを読み込み、ビデオコントローラを取得します。
		/// ビデオは「GameData\Videos」フォルダに保存されている必要があります。
		/// </summary>
		/// <param name="FileName">ファイル名 （.\GameData\Videos\）</param>
		/// <returns>MMedia コントローラ</returns>
		public static MMedia CreateVideo(string FileName) {
			FileName = FileName.Replace("..", "__"); //簡易フォルダ参照禁止（この規制を回避してロードした場合、ほかプラットフォームへの移植は不可能に）
			FileName = FileName.Replace(':', '_'); //簡易ドライブ参照禁止（この規制を回避してロードした場合、ほかプラットフォームへの移植は不可能に）

			MMedia S = NewWithManagedList();
			S.LoadVideo(FileName);
			return S;
		}

		/// <summary>
		/// すべてのサウンドコントローラを削除してリセットします。
		/// スクリプトが変わる際は自動的に実行されます。
		/// </summary>
		public static void Reset() {
			for (int i = 0; i < MediaList.Length; i++ ) {
				if (MediaList[i] != null) {
					MediaList[i].Close();
					MediaList[i] = null;
				}
			}
		}



		//Dynamic
		private string Alias;
		private string FileNameForDebug;
		private bool isVideo = false;
		private int SelfListID = -1;

		// ◆◆　移植元コードの遮蔽: Midtimeでは使用できません　（v2以降はprivate化済み）　◆◆
		// ◆◆　ユーザーによる new MMedia() の防止も含む。　◆◆
		private MMedia() {
			this.Alias = GetRandom();
			return;
		}

		// ◆◆　移植元コードの遮蔽: Midtimeでは使用できません　（v2以降はprivate化済み）　◆◆
		private void LoadSound(string FileName) {
//			this.Close();
			this.FileNameForDebug = FileName;
			this.isVideo = false;
			if (File.Exists(@".\GameData\Sounds\" + FileName + ".wav")) { FileName += ".wav"; }
			if (File.Exists(@".\GameData\Sounds\" + FileName + ".mp3")) { FileName += ".mp3"; }
			if (File.Exists(@".\GameData\Sounds\" + FileName + ".m4a")) { FileName += ".m4a"; }
			if (File.Exists(@".\GameData\Sounds\" + FileName + ".ogg")) { FileName += ".ogg"; }

			MDebug.WriteFormat("MMediaを使用して、サウンド「{0}」を読み込みます。", FileName);

			MediaServer.Write(string.Format("Open|{0}{1}|{2}", @".\GameData\Sounds\", FileName, this.Alias));
		}

		// ◆◆　移植元コードの遮蔽: Midtimeでは使用できません　（v2以降はprivate化済み）　◆◆
		private void LoadVideo(string FileName) {
//			this.Close();
			this.FileNameForDebug = FileName;
			this.isVideo = true;

			if (File.Exists(@".\GameData\Sounds\" + FileName + ".mpg")) { FileName += ".mpg"; }
			if (File.Exists(@".\GameData\Sounds\" + FileName + ".mpeg")) { FileName += ".mpeg"; }
			if (File.Exists(@".\GameData\Sounds\" + FileName + ".mp2")) { FileName += ".mp2"; }
			if (File.Exists(@".\GameData\Sounds\" + FileName + ".mp4")) { FileName += ".mp4"; }
			if (File.Exists(@".\GameData\Sounds\" + FileName + ".m4v")) { FileName += ".m4v"; }
			if (File.Exists(@".\GameData\Sounds\" + FileName + ".avi")) { FileName += ".avi"; }
			if (File.Exists(@".\GameData\Sounds\" + FileName + ".ogv")) { FileName += ".ogv"; }
	
			MDebug.WriteFormat("MMediaを使用して、ビデオ「{0}」を読み込みます。", FileName);
			MediaServer.Write(string.Format("Open|{0}{1}|{2}", @".\GameData\Videos\", FileName, this.Alias));
		}

		/// <summary>
		/// 読み込まれたサウンド・ビデオを一度だけ再生します。
		/// SEやビデオの再生に最適です。
		/// </summary>
		public void PlayOnce() {
			MediaServer.Write(string.Format("Play|{0}", this.Alias));
		}

		/// <summary>
		/// 読み込まれたサウンド・ビデオを繰り返し再生します。
		/// BGMの再生に最適です。
		/// </summary>
		public void PlayLoop() {
			MediaServer.Write(string.Format("Loop|{0}", this.Alias));
		}

		/// <summary>
		/// 現在読み込まれている、サウンド・ビデオを停止します。
		/// 再度、再生した場合は最初から再生されます。
		/// </summary>
		public void Stop() {
			MediaServer.Write(string.Format("Stop|{0}", this.Alias));			
		}

		/// <summary>
		/// 現在読み込まれている、サウンド・ビデオを停止します。
		/// 再度、再生した場合は Pause した位置から再生されます。
		/// </summary>
		public void Pause() {
			MediaServer.Write(string.Format("Pause|{0}", this.Alias));			
		}

		/// <summary>
		/// 現在読み込まれている、サウンド・ビデオを閉じて、読み込みを行う前の状態に戻します。
		/// これを行った サウンドコントローラ はもう使用できません。
		/// </summary>
		public void Close() {
			MediaServer.Write(string.Format("Close|{0}", this.Alias));
			MediaList[this.SelfListID] = null;
		}

		/// <summary>
		/// 現在読み込まれている、サウンド・ビデオの「長さ」を取得します。 
		/// 長さはミリ秒(1ms)単位です。（1000ミリ秒で1秒です）
		/// </summary>
		/// <returns>長さ (ミリ秒)</returns>
		public int GetLength() {
			MediaServer.Write(string.Format("GetLength|{0}", this.Alias));
			int R = 0;
			int.TryParse(MediaServer.Read(), out R);
			return R;
		}

		/// <summary>
		/// 現在読み込まれている、サウンド・ビデオの「現在の再生位置」を取得します。 
		/// 長さはミリ秒(1ms)単位です。（1000ミリ秒で1秒です）
		/// </summary>
		/// <returns>現在の再生位置 (ミリ秒)</returns>
		public int GetPosition() {
			MediaServer.Write(string.Format("GetPosition|{0}", this.Alias));
			int R = 0;
			int.TryParse(MediaServer.Read(), out R);
			return R;
		}

		/// <summary>
		/// 現在読み込まれている、指定したサウンド・ビデオの「再生位置」を変更します。 
		/// 長さはミリ秒(1ms)単位です。（1000ミリ秒で1秒です）
		/// </summary>
		/// <param name="p">新しい再生位置 (ミリ秒)</param>
		public void SetPosition(int p) {
			MediaServer.Write(string.Format("SetPosition|{0}|{1}", this.Alias, p));	
		}

		/// <summary>
		/// 現在読み込まれている、サウンド・ビデオの「音量」を取得します。 
		/// </summary>
		/// <returns>現在の音量 (0～100)</returns>
		public int GetVolume() {
			MediaServer.Write(string.Format("GetVolume|{0}", this.Alias));
			int R = 0;
			int.TryParse(MediaServer.Read(), out R);
			return R/10;
		}
		
		/// <summary>
		/// 現在読み込まれている、サウンド・ビデオの「音量」を変更します。 
		/// </summary>
		/// <param name="v">新しい音量 (0～100)</param>
		public void SetVolume(int v) {
			MediaServer.Write(string.Format("SetVolume|{0}|{1}", this.Alias, v*10));
		}

		/// <summary>
		/// 現在読み込まれている、サウンド・ビデオの「速度」を取得します。
		/// 速度は1000で等速（x1.0）です。1200で1.2倍、800で0.8倍になります。
		/// </summary>
		/// <returns>再生速度 (x1.0 = 1000)</returns>
		public int GetSpeed() {
			MediaServer.Write(string.Format("GetSpeed|{0}", this.Alias));
			int R = 0;
			int.TryParse(MediaServer.Read(), out R);
			return R;
		}

		/// <summary>
		/// 現在読み込まれている、サウンド・ビデオの「速度」を変更します。
		/// 速度は1000で等速（x1.0）です。1200で1.2倍、800で0.8倍になります。
		/// </summary>
		/// <param name="s">再生速度 (x1.0 = 1000)</param>
		public void SetSpeed(int s) {
			MediaServer.Write(string.Format("SetSpeed|{0}|{1}", this.Alias, s));
		}
	}

}

