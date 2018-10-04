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
	public static class MWindow {

		static dynamic GAWindow;
		static dynamic GADebug;

		//静的コンストラクタ
		static MWindow() {
			GADebug = new StaticMembersDynamicWrapper(MCore.GameAPI.GetType("MJHSC.MidtimeEngine.GameAPI.MDebug"));
			GAWindow = new StaticMembersDynamicWrapper(MCore.GameAPI.GetType("MJHSC.MidtimeEngine.GameAPI.MWindow"));
			GADebug.Write("「MJHSC.MidtimeEngine.Languages.SmallBasic.GameAPI.MMedia」がロードされました。");
		}



		/// <summary>
		/// 【Windows専用】ゲーム画面のタイトル表記を変更します。通常使用する必要はありません。
		/// </summary>
		/// <param name="Title">新しいタイトル</param>
		[Obsolete("通常、必要としないコードです。本当に必要かよく確認してください。", false)]
		public static Primitive SetTitle(Primitive Title) {
			GAWindow.SetTitle((string)Title);
			return true;
		}

		/// <summary>
		/// 【Windows専用】ゲーム画面の大きさを変更します。 解像度は変わらないため、960x540 以外にした場合は縮小または拡大されます。
		/// 品質の維持のため、「480x270・960x540（1K）・1440x810・1920x1080（2K）・2880x1620・3840x2160（4K）・5760x3240・7680x4320（8K）」以外の数値は推奨しません。
		/// </summary>
		/// <param name="Width">新しい横幅</param>
		/// <param name="Height">新しい縦幅</param>
		public static Primitive SetSize(Primitive Width, Primitive Height) {
			GAWindow.SetSize((int)Width, (int)Height);
			return true;
		}

		/// <summary>
		/// 【Windows専用】画面をフルスクリーン表示にします。既にフルスクリーンの場合、元に戻します。
		/// 「Alt+Enter」を押すと、いつでもプレイヤーは切り替えられることに注意してください。
		/// 　Windows以外では常にフルスクリーンです。
		/// </summary>
		public static Primitive ToggleFullscreen() {
			GAWindow.ToggleFullscreen();
			return true;
		}


	}

}