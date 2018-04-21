//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.

using System;
using System.IO;
using MJHSC.MidtimeEngine;
using MJHSC.MidtimeEngine.Plugins;

namespace MJHSC.MidtimeEngine.GameAPI {

	/// <summary>
	/// 移動する向きによって変化しない、静止画・アニメを表示します。
	/// </summary>
	[MidtimeFunction2]
	public class MImage {

		//Static
		private static MImage[] ImageList = new MImage[512];

		private static MImage NewWithManagedList() {
			for (int i = 0; i < ImageList.Length; i++) {
				if (ImageList[i] == null) {
					ImageList[i] = new MImage();
					ImageList[i].SelfListID = i;
					return ImageList[i];
				}
			}

			MDebug.WriteFormat("同時に使用可能な {0} の制限を超えました。 同時に使用できるのは {1}個までです。 不要なものを .Close();するか、 {0}.Reset(); してください。", "MImage", ImageList.Length);
			return null; //管理領域からはみ出ている。この場合、エラーにさせる。
		}

		[Obsolete("この関数は一部の言語サポートのために内部で使用され、コンテンツでの使用は許可されていません。", false)]
		public static int _GetManagedID(MImage I) {
			return I.SelfListID;
		}

		[Obsolete("この関数は一部の言語サポートのために内部で使用され、コンテンツでの使用は許可されていません。", false)]
		public static MImage _GetMImageByManagedID(int ID) {
			return ImageList[ID];
		}


		/// <summary>
		/// 今後、新たに作成する文字の色を変更します。
		/// 各色の量は「0～255」の数字で光の三原色として表します。
		/// すべて０の場合は黒、すべて255の場合は白になります。
		/// 
		/// 既に作成済みの文字に影響はありません。
		/// </summary>
		/// <param name="R">赤色（0～255）</param>
		/// <param name="G">緑色（0～255）</param>
		/// <param name="B">青色（0～255）</param>
		public static void SetTextColor(int R, int G, int B) {
			Image.SetTextColor(R, G, B);
		}

		/// <summary>
		/// 今後、新たに作成する文字の大きさを変更します。
		/// 大きさの単位はWordなどと同じ、「ポイント（pt）」です。
		/// 
		/// 既に作成済みの文字に影響はありません。
		/// </summary>
		/// <param name="Size">文字の大きさ（pt）</param>
		public static void SetTextSize(int Size) {
			Image.SetTextSize(Size);
		}

		/// 今後、新たに作成する文字の書体を変更します。
		/// システムによって使用可能な書体が異なるので、通常使用するべきではありません。
		/// 
		/// 既に作成済みの文字に影響はありません。
		/// <param name="FontName">フォント名</param>
		[Obsolete("システムによって使用可能な書体が異なるので、通常使用するべきではありません。", false)]
		public static void SetTextFont(string FontName) {
			Image.SetFont(FontName);
		}

		/// <summary>
		/// 指定した文字を画面表示できるようにします。
		/// </summary>
		/// <param name="DrawText">表示したい文字</param>
		/// <returns>画像コントローラ</returns>
		public static MImage CreateText(string DrawText) {
			MImage I = NewWithManagedList();
			I.MIMG = Image.CreateFromText(DrawText);
			return I;
		}

		/// <summary>
		/// 指定した画像ファイルを画面表示できるようにします。
		/// 画像は「GameData\Images\」）に保存されている必要があります。
		/// .png、.bmp、.jpgのような拡張子は必要ありません。
		/// </summary>
		/// <param name="FileName">表示したい画像ファイル名</param>
		/// <returns>画像コントローラ</returns>
		public static MImage CreateImage(string FileName) {
			MImage I = NewWithManagedList();

			if (File.Exists(@".\GameData\Images\" + FileName + ".png")) { FileName += ".png"; }
			if (File.Exists(@".\GameData\Images\" + FileName + ".bmp")) { FileName += ".bmp"; }
			if (File.Exists(@".\GameData\Images\" + FileName + ".jpg")) { FileName += ".jpg"; }

			MDebug.WriteFormat("MImageを使用して、画像「{0}」を読み込みます。", FileName);

			I.MIMG = Image.CreateFromFile(FileName);
			return I;
		}

		/// <summary>
		/// 指定した画像ファイルを画面表示できるようにします。
		/// 画像は「GameData\Images\」）に保存されている必要があります。
		/// .png、.bmp、.jpgのような拡張子は必要ありません。
		/// </summary>
		/// <param name="FileName">表示したい画像ファイル名</param>
		/// <param name="FramesCount">アニメのコマ数</param>
		/// <param name="OneFrameInGameFrameRatio">アニメーション速度。ゲームのフレーム(MCore.LoopFPS)に対してどれだけ遅くするかを指定します。1が最速で、MCore.LoopFPS(30)の場合、30で1秒に1コマで再生します。</param>
		/// <param name="CutIsHorizontal">アニメーション方向。通常はfalseを指定し縦方向にアニメーションさせてください。 横方向にアニメーションさせたい場合はtrueを指定してください。</param>
		/// <returns>画像コントローラ</returns>
		public static MImage CreateAnime(string FileName, int FramesCount, int OneFrameInGameFrameRatio, bool CutIsHorizontal) {
			MImage I = CreateImage(FileName);
			if (I.MIMG != null) {
				I.MIMG.SetAnimeData(FramesCount, OneFrameInGameFrameRatio, CutIsHorizontal);
			}
			return I;
		}


		/// <summary>
		/// すべてのイメージを削除してリセットします。
		/// スクリプトが変わる際は自動的に実行されます。
		/// </summary>
		public static void Reset() {
			MImage.EndDraw(); //MImageを削除する前に描画処理を終了する必要がある。
			for (int i = 0; i < ImageList.Length; i++) {
				if (ImageList[i] != null) {
					ImageList[i].Close();
					ImageList[i] = null;
				}
			}
			MImage.StartDraw();
		}






		//Dynamic
		private Image MIMG;
		private int SelfListID = -1;
		private string FileNameForDebug;

		//描画パラメータ
		private int X = 0; //X
		private int Y = 0; //Y
		private int A = 255; //透明度
		private int R = 0; //角度

		/// <summary>
		/// 画像を「現在の位置から」指定された量だけ移動します。
		/// </summary>
		/// <param name="X">横方向の移動量。</param>
		/// <param name="Y">縦方向の移動量。</param>
		public void Move(int X, int Y) {
			this.X += X;
			this.Y += Y;
		}

		/// <summary>
		/// 画像を指定された位置に移動します。
		/// </summary>
		/// <param name="X">横方向の位置。</param>
		/// <param name="Y">縦方向の位置。</param>
		public void SetPosition(int X, int Y) {
			this.X = X;
			this.Y = Y;
		}

		/// <summary>
		/// 画像の透明度を「現在の位置から」指定された量だけ変化させます。
		/// 
		/// その結果、最小値（0）以下の場合は透明になり、それ以下には下がりません。
		/// また、最大値（255）以上の場合は不透明になり、それ以上には上がりません。
		/// </summary>
		/// <param name="Alpha">透明度。</param>
		public void Alpha(int Alpha) {
			this.A += Alpha;
			if (this.A < 0) { this.A = 0; }
			if (this.A > 256) { this.A = 255; }
		}

		/// <summary>
		/// 画像の透明度を指定します。
		/// 
		/// 最小値（0）以下の場合は透明になり、それ以下には下がりません。
		/// 最大値（255）以上の場合は不透明になり、それ以上には上がりません。
		/// </summary>
		/// <param name="Alpha">透明度。</param>
		public void SetAlpha(int Alpha) {
			this.A = Alpha;
			if (this.A < 0) { this.A = 0; }
			if (this.A > 256) { this.A = 255; }
		}

		/// <summary>
		/// 画像の角度を「現在の角度から」指定された度数だけ回転させます。
		/// 
		/// 単位は度（°）です。
		/// </summary>
		/// <param name="Rotate">回転させる度数（-360～360）</param>
		public void Rotate(int Rotate) {
			this.R += Rotate;
			if (this.R < 0) { this.R = this.R - 360; }
			if (this.R > 360) { this.R = this.R % 360; }
		}

		/// 画像の角度を指定された角度にします。
		/// 
		/// 単位は度（°）です。 90で真横、180で真逆になります。
		public void SetRotate(int Rotate) {
			this.R = Rotate;
			if (this.R < 0) { this.R = this.R - 360; }
			if (this.R > 360) { this.R = this.R % 360; }
		}

		/// <summary>
		/// 現在までに設定された値を使用して、画像を画面に表示します。
		/// </summary>
		public void Draw() {
			if (this.MIMG != null) {
				try {
					this.MIMG.Draw(this. X, this.Y, this.A, this.R);
				} catch { }
			}
		}

		/// <summary>
		/// 画像を水平（横）方向に反転させるかを設定します。
		/// </summary>
		/// <param name="Invert">true = 反転する。false = 反転しない。</param>
		public void InvertHorizontal(bool Invert) {
			if (this.MIMG != null) {
				this.MIMG.SetInvert(Invert, true);
			}
		}

		/// <summary>
		/// 画像を垂直（縦）方向に反転させるかを設定します。
		/// </summary>
		/// <param name="Invert">true = 反転する。false = 反転しない。</param>
		public void InvertVertical(bool Invert) {
			if (this.MIMG != null) {
				this.MIMG.SetInvert(Invert, false);
			}
		}


		/// <summary>
		/// 現在読み込まれている、画像を閉じて、読み込みを行う前の状態に戻します。
		/// これを行った 画像コントローラ はもう使用できません。
		/// </summary>
		public void Close() {
			if (this.MIMG != null) {
				this.MIMG.Close();
				this.MIMG = null;
			}
		}



		[Obsolete("通常は、MCore.LoopFPS(); または MCore.Loop(); を使用してください。", false)]
		public static void StartDraw() {
			try {
				Graphic.SBStart();
			} catch { }
		}

		[Obsolete("通常は、MCore.LoopFPS(); または MCore.Loop(); を使用してください。", false)]
		public static void EndDraw() {
			try {
				Graphic.SBEnd();
				MInput.Process();
			} catch { }
		}

	}

}

