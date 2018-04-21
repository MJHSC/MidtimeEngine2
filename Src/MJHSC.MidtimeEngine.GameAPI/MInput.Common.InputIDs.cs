//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace MJHSC.MidtimeEngine.GameAPI {

	/// <summary>
	/// コントローラやキーボード、マウスからの入力を取得します。
	/// </summary>
	public partial class MInput {

		/// <summary>
		/// 「Midtime コントローラのボタン」を表します。
		/// </summary>
		public enum ButtonID { //◆◆　v2以降: 移植元との互換性なし　◆◆
			/// <summary>
			/// 無し・不明。通常、使用しません。
			/// </summary>
			None = 0,

			/// <summary>
			/// Aボタン。決定などに使用します。
			/// </summary>
			A = 1,

			/// <summary>
			/// Bボタン。キャンセル・取消などに使用します。
			/// </summary>
			B = 2,

			/// <summary>
			/// Xボタン。
			/// </summary>
			X = 3,

			/// <summary>
			/// Yボタン。
			/// </summary>
			Y = 4,

			/// <summary>
			/// Lボタン。
			/// </summary>
			L = 5,

			/// <summary>
			/// Rボタン。
			/// </summary>
			R = 6,

			/// <summary>
			/// L2ボタン。
			/// </summary>
			L2 = 7,

			/// <summary>
			/// R2ボタン。
			/// </summary>
			R2 = 8,

			/// <summary>
			/// L3ボタン。Lスティックを上から押すと作動します。
			/// </summary>
			L3 = 9,

			/// <summary>
			/// R3ボタン。Rスティックを上から押すと作動します。
			/// </summary>
			R3 = 10,

			/// <summary>
			/// スタートボタン。メニューの表示などに使用します。
			/// </summary>
			Start = 11,

			/// <summary>
			/// セレクトボタン。
			/// </summary>
			Select = 12,

			/// <summary>
			/// 十字キーの「上」
			/// </summary>
			Up = 13,

			/// <summary>
			/// 十字キーの「下」
			/// </summary>
			Down = 14,

			/// <summary>
			/// 十字キーの「左」
			/// </summary>
			Left = 15,

			/// <summary>
			/// 十字キーの「右」
			/// </summary>
			Right = 16,
		}

		/// <summary>
		/// 「Midtime コントローラのスティック」を表します。
		/// </summary>
		public enum StickID { //◆◆　v2以降: 移植元との互換性なし　◆◆
			/// <summary>
			/// 無し・不明。通常、使用しません。
			/// </summary>
			None = 0,

			/// <summary>
			/// 「左スティック」の横方向の移動。
			/// </summary>
			LX = 1,

			/// <summary>
			/// 「左スティック」の縦方向の移動。
			/// </summary>
			LY = 2,

			/// <summary>
			/// 「右スティック」の横方向の移動。
			/// </summary>
			RX = 3,

			/// <summary>
			/// 「右スティック」の縦方向の移動。
			/// </summary>
			RY = 4,
		}

		/// <summary>
		/// 「Midtime コントローラのトリガーレバー」を表します。
		/// </summary>
		public enum TriggerID { //◆◆　v2以降: 移植元との互換性なし　◆◆
			/// <summary>
			/// 無し・不明。通常、使用しません。
			/// </summary>
			None = 0,

			/// <summary>
			/// 左スティック。
			/// </summary>
			L = 1,

			/// <summary>
			/// 右スティック。
			/// </summary>
			R = 2,
		}

		/// <summary>
		/// 「マウスのボタン」を表します。
		/// </summary>
		public enum MouseButtonID { //◆◆　v2以降: 移植元との互換性なし　◆◆
			/// <summary>
			/// 無し・不明。通常、使用しません。
			/// </summary>
			None = 0,

			/// <summary>
			/// 左ボタン。
			/// </summary>
			L = 1,

			/// <summary>
			/// 右ボタン。
			/// </summary>
			R = 2,

			/// <summary>
			/// 中央のホイール
			/// </summary>
			Wheel = 3,
		}

		/// <summary>
		/// 「マウスの移動方向」を表します。
		/// </summary>
		public enum MouseMoveID { //◆◆　v2以降: 移植元との互換性なし　◆◆
			/// <summary>
			/// 無し・不明。通常、使用しません。
			/// </summary>
			None = 0,

			/// <summary>
			/// 横方向の移動。
			/// </summary>
			X = 1,

			/// <summary>
			/// 縦方向の移動。
			/// </summary>
			Y = 2,

			/// <summary>
			/// 中央のホイールの回転。上方向がプラスの数、下方向がマイナスの数です。
			/// </summary>
			Wheel = 3,
		}


	}
}