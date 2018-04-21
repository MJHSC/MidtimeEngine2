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
	/// Midtime Engine の画面に関する設定を行います。
	/// </summary>
	[SmallBasicType]
	public static class MImage {

		static dynamic GAImage;
		static dynamic GAImageSS;
		static dynamic GADebug;

		//静的コンストラクタ
		static MImage() {
			GADebug = new StaticMembersDynamicWrapper(MCore.GameAPI.GetType("MJHSC.MidtimeEngine.GameAPI.MDebug"));
			GAImage = new StaticMembersDynamicWrapper(MCore.GameAPI.GetType("MJHSC.MidtimeEngine.GameAPI.MImage"));
			GAImageSS = new StaticMembersDynamicWrapper(MCore.GameAPISS.GetType("MJHSC.MidtimeEngine.GameAPI.ScriptSupport.MImage"));
			GADebug.Write("「MJHSC.MidtimeEngine.Languages.SmallBasic.GameAPI.MImage」がロードされました。");
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
		public static Primitive SetTextColor(Primitive R, Primitive G, Primitive B) {
			GAImage.SetTextColor((int)R, (int)G, (int)B);
			return true;
		}

		/// <summary>
		/// 今後、新たに作成する文字の大きさを変更します。
		/// 大きさの単位はWordなどと同じ、「ポイント（pt）」です。
		/// 
		/// 既に作成済みの文字に影響はありません。
		/// </summary>
		/// <param name="Size">文字の大きさ（pt）</param>
		public static Primitive SetTextSize(Primitive Size) {
			GAImage.SetTextSize((int) Size);
			return true;
		}

		/// 今後、新たに作成する文字の書体を変更します。
		/// システムによって使用可能な書体が異なるので、通常使用するべきではありません。
		/// 
		/// 既に作成済みの文字に影響はありません。
		/// <param name="FontName">フォント名</param>
		[Obsolete("システムによって使用可能な書体が異なるので、通常使用するべきではありません。", false)]
		public static Primitive SetTextFont(Primitive FontName) {
			GAImage.SetTextFont((string) FontName);
			return true;
		}

		/// <summary>
		/// 指定した文字を画面表示できるようにします。
		/// </summary>
		/// <param name="DrawText">表示したい文字</param>
		/// <returns>画像ID</returns>
		public static Primitive CreateText(Primitive DrawText) {
			return (int) GAImageSS.CreateText((string) DrawText);
		}

		/// <summary>
		/// 指定した画像ファイルを画面表示できるようにします。
		/// 画像は「GameData\Images\」）に保存されている必要があります。
		/// </summary>
		/// <param name="FileName">表示したい画像ファイル名</param>
		/// <returns>画像ID</returns>
		public static Primitive CreateImage(Primitive FileName) {
			return (int)GAImageSS.CreateImage((string) FileName);
		}



		/// <summary>
		/// すべてのイメージを削除してリセットします。
		/// スクリプトが変わる際は自動的に実行されます。
		/// </summary>
		public static Primitive Reset() {
			GAImage.Reset();
			return true;
		}




		/// <summary>
		/// 画像を「現在の位置から」指定された量だけ移動します。
		/// </summary>
		/// <param name="MImageID">画像ID (MImage.CreateText または MImage.CreateImage で取得できます。)</param>
		/// <param name="X">横方向の移動量。</param>
		/// <param name="Y">縦方向の移動量。</param>
		public static Primitive Move(Primitive MImageID, Primitive X, Primitive Y) {
			GAImageSS.Move((int)MImageID, (int)X, (int)Y);
			return true;
		}

		/// <summary>
		/// 画像を指定された位置に移動します。
		/// </summary>
		/// <param name="MImageID">画像ID (MImage.CreateText または MImage.CreateImage で取得できます。)</param>
		/// <param name="X">横方向の位置。</param>
		/// <param name="Y">縦方向の位置。</param>
		public static Primitive SetPosition(Primitive MImageID, Primitive X, Primitive Y) {
			GAImageSS.SetPosition((int)MImageID, (int)X, (int)Y);
			return true;
		}

		/// <summary>
		/// 画像の透明度を「現在の位置から」指定された量だけ変化させます。
		/// 
		/// その結果、最小値（0）以下の場合は透明になり、それ以下には下がりません。
		/// また、最大値（255）以上の場合は不透明になり、それ以上には上がりません。
		/// </summary>
		/// <param name="MImageID">画像ID (MImage.CreateText または MImage.CreateImage で取得できます。)</param>
		/// <param name="Alpha">透明度。</param>
		public static Primitive Alpha(Primitive MImageID, Primitive Alpha) {
			GAImageSS.Alpha((int)MImageID, (int)Alpha);
			return true;
		}

		/// <summary>
		/// 画像の透明度を指定します。
		/// 
		/// 最小値（0）以下の場合は透明になり、それ以下には下がりません。
		/// 最大値（255）以上の場合は不透明になり、それ以上には上がりません。
		/// </summary>
		/// <param name="MImageID">画像ID (MImage.CreateText または MImage.CreateImage で取得できます。)</param>
		/// <param name="Alpha">透明度。</param>
		public static Primitive SetAlpha(Primitive MImageID, Primitive Alpha) {
			GAImageSS.SetAlpha((int)MImageID, (int)Alpha);
			return true;
		}

		/// <summary>
		/// 画像の角度を「現在の角度から」指定された度数だけ回転させます。
		/// 
		/// 単位は度（°）です。
		/// </summary>
		/// <param name="MImageID">画像ID (MImage.CreateText または MImage.CreateImage で取得できます。)</param>
		/// <param name="Rotate">回転させる度数（-360～360）</param>
		public static Primitive Rotate(Primitive MImageID, Primitive Rotate) {
			GAImageSS.Rotate((int)MImageID, (int)Rotate);
			return true;
		}

		/// 画像の角度を指定された角度にします。
		/// 
		/// 単位は度（°）です。 90で真横、180で真逆になります。
		/// <param name="MImageID">画像ID (MImage.CreateText または MImage.CreateImage で取得できます。)</param>
		public static Primitive SetRotate(Primitive MImageID, Primitive Rotate) {
			GAImageSS.SetRotate((int)MImageID, (int)Rotate);
			return true;
		}

		/// <summary>
		/// 現在までに設定された値を使用して、画像を画面に表示します。
		/// </summary>
		/// <param name="MImageID">画像ID (MImage.CreateText または MImage.CreateImage で取得できます。)</param>
		public static Primitive Draw(Primitive MImageID) {
			GAImageSS.Draw((int)MImageID);
			return true;
		}


		/// <summary>
		/// 現在読み込まれている、画像を閉じて、読み込みを行う前の状態に戻します。
		/// これを行った 画像コントローラ はもう使用できません。
		/// </summary>
		/// <param name="MImageID">画像ID (MImage.CreateText または MImage.CreateImage で取得できます。)</param>
		public static Primitive Close(Primitive MImageID) {
			GAImageSS.Close((int)MImageID);
			return true;
		}
		


	}

}