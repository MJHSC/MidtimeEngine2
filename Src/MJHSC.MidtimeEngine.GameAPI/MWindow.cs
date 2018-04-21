//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.

using System;
using System.IO;
using System.Windows.Forms;
using MJHSC.MidtimeEngine.Plugins;

namespace MJHSC.MidtimeEngine.GameAPI {

	/// <summary>
	/// Midtime Engine の画面に関する設定を行います。
	/// </summary>
	[MidtimeFunction2]
	public class MWindow {

		/// <summary>
		/// 【Windows専用】ゲーム画面のタイトル表記を変更します。通常使用する必要はありません。
		/// </summary>
		/// <param name="Title">新しいタイトル</param>
		[Obsolete("通常、必要としないコードです。本当に必要かよく確認してください。", false)]
		public static void SetTitle(string Title) {
			Midtime.GameWindow.ChangeTitle(Title);
		}

		/// <summary>
		/// 【Windows専用】ゲーム画面の大きさを変更します。 解像度は変わらないため、960x540 以外にした場合は縮小または拡大されます。
		/// 品質の維持のため、「480x270・960x540（1K）・1440x810・1920x1080（2K）・2880x1620・3840x2160（4K）・5760x3240・7680x4320（8K）」以外の数値は推奨しません。
		/// </summary>
		/// <param name="Width">新しい横幅</param>
		/// <param name="Height">新しい縦幅</param>
		public static void SetSize(int Width, int Height) {
			Midtime.SetWindowSize(Width, Height);
		}

		/// <summary>
		/// 【Windows専用】画面をフルスクリーン表示にします。既にフルスクリーンの場合、元に戻します。
		/// 「Alt+Enter」を押すと、いつでもプレイヤーは切り替えられることに注意してください。
		/// 　Windows以外では常にフルスクリーンです。
		/// </summary>
		public static void ToggleFullscreen() {
			Midtime.GameWindow.ToggleFullscreen();
		}

	}

}

