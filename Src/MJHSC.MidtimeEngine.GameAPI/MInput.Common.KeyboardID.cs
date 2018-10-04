//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.

using System;
using System.IO;
using System.Windows.Forms;

namespace MJHSC.MidtimeEngine.GameAPI {
	public partial class MInput {

		/// <summary>
		/// 「キーボードのボタン」を表します。
		/// </summary>
		public enum KeyID : int {		//「System.Windows.Forms.Keys」からコピー。万一それが変更された場合は正しく動作しなくなる。
			//ただし、バージョンを指定して「System.Windows.Forms」をロードしているのでビルドしなおさない限り（ユーザー環境で勝手に）この問題が発生しないはず。

			///<summary>
			///     キー値から修飾子を抽出するビット マスク。
			///</summary>
			Modifiers = -65536,

			///<summary>
			///     キー入力なし
			///</summary>
			None = 0,

			///<summary>
			///     マウスの左ボタン
			///</summary>
			LButton = 1,

			///<summary>
			///     マウスの右ボタン
			///</summary>
			RButton = 2,

			///<summary>
			///     Cancel キー
			///</summary>
			Cancel = 3,

			///<summary>
			///     マウスの中央ボタン (3 ボタン マウスの場合)
			///</summary>
			MButton = 4,

			///<summary>
			///     x マウスの 1 番目のボタン (5 ボタン マウスの場合)
			///</summary>
			XButton1 = 5,

			///<summary>
			///     x マウスの 2 番目のボタン (5 ボタン マウスの場合)
			///</summary>
			XButton2 = 6,

			///<summary>
			///     BackSpace キー
			///</summary>
			Back = 8,

			///<summary>
			///     TAB キー
			///</summary>
			Tab = 9,

			///<summary>
			///     ライン フィード キー
			///</summary>
			LineFeed = 10,

			///<summary>
			///     Clear キー
			///</summary>
			Clear = 12,

			///<summary>
			///     Enter キー
			///</summary>
			Enter = 13,

			///<summary>
			///     Return キー
			///</summary>
			Return = 13,

			///<summary>
			///     Shift キー
			///</summary>
			ShiftKey = 16,

			///<summary>
			///     CTRL キー
			///</summary>
			ControlKey = 17,

			///<summary>
			///     Alt キー
			///</summary>
			Menu = 18,

			///<summary>
			///     Pause キー
			///</summary>
			Pause = 19,

			///<summary>
			///     CAPS LOCK キー
			///</summary>
			CapsLock = 20,

			///<summary>
			///     CAPS LOCK キー
			///</summary>
			Capital = 20,

			///<summary>
			///     IME かなモード キー
			///</summary>
			KanaMode = 21,

			///<summary>
			///     IME ハングル モード キー(互換性を保つために保持されています。HangulMode を使用します)
			///</summary>
			HanguelMode = 21,

			///<summary>
			///     IME ハングル モード キー
			///</summary>
			HangulMode = 21,

			///<summary>
			///     IME Junja モード キー
			///</summary>
			JunjaMode = 23,

			///<summary>
			///     IME Final モード キー
			///</summary>
			FinalMode = 24,

			///<summary>
			///     IME 漢字モード キー
			///</summary>
			KanjiMode = 25,

			///<summary>
			///     IME Hanja モード キー
			///</summary>
			HanjaMode = 25,

			///<summary>
			///     ESC キー
			///</summary>
			Escape = 27,

			///<summary>
			///     IME 変換キー
			///</summary>
			IMEConvert = 28,

			///<summary>
			///     IME 無変換キー
			///</summary>
			IMENonconvert = 29,

			///<summary>
			///     IME Accept キー
			///</summary>
			IMEAceept = 30,

			///<summary>
			///     IME Accept キー (System.Windows.Forms.Keys.IMEAceept の代わりに使用します)
			///</summary>
			IMEAccept = 30,

			///<summary>
			///     IME モード変更キー
			///</summary>
			IMEModeChange = 31,

			///<summary>
			///     Space キー
			///</summary>
			Space = 32,

			///<summary>
			///     PageUp キー
			///</summary>
			Prior = 33,

			///<summary>
			///     PageUp キー
			///</summary>
			PageUp = 33,

			///<summary>
			///     PAGE DOWN キー
			///</summary>
			Next = 34,

			///<summary>
			///     PAGE DOWN キー
			///</summary>
			PageDown = 34,

			///<summary>
			///     END キー
			///</summary>
			End = 35,

			///<summary>
			///     HOME キー
			///</summary>
			Home = 36,

			///<summary>
			///     ← キー
			///</summary>
			Left = 37,

			///<summary>
			///     ↑ キー
			///</summary>
			Up = 38,

			///<summary>
			///     → キー
			///</summary>
			Right = 39,

			///<summary>
			///     ↓ キー
			///</summary>
			Down = 40,

			///<summary>
			///     Select キー
			///</summary>
			Select = 41,

			///<summary>
			///     Print キー
			///</summary>
			Print = 42,

			///<summary>
			///     Execute キー
			///</summary>
			Execute = 43,

			///<summary>
			///     PrintScreen キー
			///</summary>
			PrintScreen = 44,

			///<summary>
			///     PrintScreen キー
			///</summary>
			Snapshot = 44,

			///<summary>
			///     INS キー
			///</summary>
			Insert = 45,

			///<summary>
			///     DEL キー
			///</summary>
			Delete = 46,

			///<summary>
			///     HELP キー
			///</summary>
			Help = 47,

			///<summary>
			///     0 キー
			///</summary>
			D0 = 48,

			///<summary>
			///     1 キー
			///</summary>
			D1 = 49,

			///<summary>
			///     2 キー
			///</summary>
			D2 = 50,

			///<summary>
			///     3 キー
			///</summary>
			D3 = 51,

			///<summary>
			///     4 キー
			///</summary>
			D4 = 52,

			///<summary>
			///     5 キー
			///</summary>
			D5 = 53,

			///<summary>
			///     6 キー
			///</summary>
			D6 = 54,

			///<summary>
			///     7 キー
			///</summary>
			D7 = 55,

			///<summary>
			///     8 キー
			///</summary>
			D8 = 56,

			///<summary>
			///     9 キー
			///</summary>
			D9 = 57,

			///<summary>
			///     A キー
			///</summary>
			A = 65,

			///<summary>
			///     B キー
			///</summary>
			B = 66,

			///<summary>
			///     C キー
			///</summary>
			C = 67,

			///<summary>
			///     D キー
			///</summary>
			D = 68,

			///<summary>
			///     E キー
			///</summary>
			E = 69,

			///<summary>
			///     F キー
			///</summary>
			F = 70,

			///<summary>
			///     G キー
			///</summary>
			G = 71,

			///<summary>
			///     H キー
			///</summary>
			H = 72,

			///<summary>
			///     I キー
			///</summary>
			I = 73,

			///<summary>
			///     J キー
			///</summary>
			J = 74,

			///<summary>
			///     K キー
			///</summary>
			K = 75,

			///<summary>
			///     L キー
			///</summary>
			L = 76,

			///<summary>
			///     M キー
			///</summary>
			M = 77,

			///<summary>
			///     N キー
			///</summary>
			N = 78,

			///<summary>
			///     O キー
			///</summary>
			O = 79,

			///<summary>
			///     P キー
			///</summary>
			P = 80,

			///<summary>
			///     Q キー
			///</summary>
			Q = 81,

			///<summary>
			///     R キー
			///</summary>
			R = 82,

			///<summary>
			///     S キー
			///</summary>
			S = 83,

			///<summary>
			///     T キー
			///</summary>
			T = 84,

			///<summary>
			///     U キー
			///</summary>
			U = 85,

			///<summary>
			///     V キー
			///</summary>
			V = 86,

			///<summary>
			///     W キー
			///</summary>
			W = 87,

			///<summary>
			///     X キー
			///</summary>
			X = 88,

			///<summary>
			///     Y キー
			///</summary>
			Y = 89,

			///<summary>
			///     Z キー
			///</summary>
			Z = 90,

			///<summary>
			///     左の Windows ロゴ キー (Microsoft Natural Keyboard)
			///</summary>
			LWin = 91,

			///<summary>
			///     右の Windows ロゴ キー (Microsoft Natural Keyboard)
			///</summary>
			RWin = 92,

			///<summary>
			///     アプリケーション キー (Microsoft Natural Keyboard)
			///</summary>
			Apps = 93,

			///<summary>
			///     コンピューターのスリープ キー
			///</summary>
			Sleep = 95,

			///<summary>
			///     数値キーパッドの 0 キー
			///</summary>
			NumPad0 = 96,

			///<summary>
			///     数値キーパッドの 1 キー
			///</summary>
			NumPad1 = 97,

			///<summary>
			///     数値キーパッドの 2 キー
			///</summary>
			NumPad2 = 98,

			///<summary>
			///     数値キーパッドの 3 キー
			///</summary>
			NumPad3 = 99,

			///<summary>
			///     数値キーパッドの 4 キー
			///</summary>
			NumPad4 = 100,

			///<summary>
			///     数値キーパッドの 5 キー
			///</summary>
			NumPad5 = 101,

			///<summary>
			///     数値キーパッドの 6 キー
			///</summary>
			NumPad6 = 102,

			///<summary>
			///     数値キーパッドの 7 キー
			///</summary>
			NumPad7 = 103,

			///<summary>
			///     数値キーパッドの 8 キー
			///</summary>
			NumPad8 = 104,

			///<summary>
			///     数値キーパッドの 9 キー
			///</summary>
			NumPad9 = 105,

			///<summary>
			///     乗算記号 (*) キー
			///</summary>
			Multiply = 106,

			///<summary>
			///     Add キー
			///</summary>
			Add = 107,

			///<summary>
			///     区切り記号キー
			///</summary>
			Separator = 108,

			///<summary>
			///     減算記号 (-) キー
			///</summary>
			Subtract = 109,

			///<summary>
			///     小数点キー
			///</summary>
			Decimal = 110,

			///<summary>
			///     除算記号 (/) キー
			///</summary>
			Divide = 111,

			///<summary>
			///     F1 キー
			///</summary>
			F1 = 112,

			///<summary>
			///     F2 キー
			///</summary>
			F2 = 113,

			///<summary>
			///     F3 キー
			///</summary>
			F3 = 114,

			///<summary>
			///     F4 キー
			///</summary>
			F4 = 115,

			///<summary>
			///     F5 キー
			///</summary>
			F5 = 116,

			///<summary>
			///     F6 キー
			///</summary>
			F6 = 117,

			///<summary>
			///     F7 キー
			///</summary>
			F7 = 118,

			///<summary>
			///     F8 キー
			///</summary>
			F8 = 119,

			///<summary>
			///     F9 キー
			///</summary>
			F9 = 120,

			///<summary>
			///     F10 キー
			///</summary>
			F10 = 121,

			///<summary>
			///     F11 キー
			///</summary>
			F11 = 122,

			///<summary>
			///     F12 キー
			///</summary>
			F12 = 123,

			///<summary>
			///     F13 キー
			///</summary>
			F13 = 124,

			///<summary>
			///     F14 キー
			///</summary>
			F14 = 125,

			///<summary>
			///     F15 キー
			///</summary>
			F15 = 126,

			///<summary>
			///     F16 キー
			///</summary>
			F16 = 127,

			///<summary>
			///     F17 キー
			///</summary>
			F17 = 128,

			///<summary>
			///     F18 キー
			///</summary>
			F18 = 129,

			///<summary>
			///     F19 キー
			///</summary>
			F19 = 130,

			///<summary>
			///     F20 キー
			///</summary>
			F20 = 131,

			///<summary>
			///     F21 キー
			///</summary>
			F21 = 132,

			///<summary>
			///     F22 キー
			///</summary>
			F22 = 133,

			///<summary>
			///     F23 キー
			///</summary>
			F23 = 134,

			///<summary>
			///     F24 キー
			///</summary>
			F24 = 135,

			///<summary>
			///     NUM LOCK キー
			///</summary>
			NumLock = 144,

			///<summary>
			///     ScrollLock キー
			///</summary>
			Scroll = 145,

			///<summary>
			///     左の Shift キー
			///</summary>
			LShiftKey = 160,

			///<summary>
			///     右の Shift キー
			///</summary>
			RShiftKey = 161,

			///<summary>
			///     左の Ctrl キー
			///</summary>
			LControlKey = 162,

			///<summary>
			///     右の Ctrl キー
			///</summary>
			RControlKey = 163,

			///<summary>
			///     左の Alt キー
			///</summary>
			LMenu = 164,

			///<summary>
			///     右の Alt キー
			///</summary>
			RMenu = 165,

			///<summary>
			///     戻るキー
			///</summary>
			BrowserBack = 166,

			///<summary>
			///     進むキー
			///</summary>
			BrowserForward = 167,

			///<summary>
			///     更新キー
			///</summary>
			BrowserRefresh = 168,

			///<summary>
			///     中止キー
			///</summary>
			BrowserStop = 169,

			///<summary>
			///     検索キー
			///</summary>
			BrowserSearch = 170,

			///<summary>
			///     お気に入りキー
			///</summary>
			BrowserFavorites = 171,

			///<summary>
			///     ホーム キー
			///</summary>
			BrowserHome = 172,

			///<summary>
			///     ミュート キー
			///</summary>
			VolumeMute = 173,

			///<summary>
			///     音量 - キー
			///</summary>
			VolumeDown = 174,

			///<summary>
			///     音量 + キー
			///</summary>
			VolumeUp = 175,

			///<summary>
			///     次のトラック キー
			///</summary>
			MediaNextTrack = 176,

			///<summary>
			///     前のトラック キー
			///</summary>
			MediaPreviousTrack = 177,

			///<summary>
			///     停止キー
			///</summary>
			MediaStop = 178,

			///<summary>
			///     再生/一時停止キー
			///</summary>
			MediaPlayPause = 179,

			///<summary>
			///     メール ホット キー
			///</summary>
			LaunchMail = 180,

			///<summary>
			///     メディア キー
			///</summary>
			SelectMedia = 181,

			///<summary>
			///     カスタム ホット キー 1
			///</summary>
			LaunchApplication1 = 182,

			///<summary>
			///     カスタム ホット キー 2
			///</summary>
			LaunchApplication2 = 183,

			///<summary>
			///     OEM 1 キー
			///</summary>
			Oem1 = 186,

			///<summary>
			///     米国標準キーボード上の OEM セミコロン キー
			///</summary>
			OemSemicolon = 186,

			///<summary>
			///     国または地域別キーボード上の OEM プラス キー
			///</summary>
			Oemplus = 187,

			///<summary>
			///     国または地域別キーボード上の OEM コンマ キー
			///</summary>
			Oemcomma = 188,

			///<summary>
			///     国または地域別キーボード上の OEM マイナス キー
			///</summary>
			OemMinus = 189,

			///<summary>
			///     国または地域別キーボード上の OEM ピリオド キー
			///</summary>
			OemPeriod = 190,

			///<summary>
			///     米国標準キーボード上の OEM 疑問符キー
			///</summary>
			OemQuestion = 191,

			///<summary>
			///     OEM 2 キー
			///</summary>
			Oem2 = 191,

			///<summary>
			///     米国標準キーボード上の OEM ティルダ キー
			///</summary>
			Oemtilde = 192,

			///<summary>
			///     OEM 3 キー
			///</summary>
			Oem3 = 192,

			///<summary>
			///     OEM 4 キー
			///</summary>
			Oem4 = 219,

			///<summary>
			///     米国標準キーボード上の OEM 左角かっこキー
			///</summary>
			OemOpenBrackets = 219,

			///<summary>
			///     米国標準キーボード上の OEM Pipe キー
			///</summary>
			OemPipe = 220,

			///<summary>
			///     OEM 5 キー
			///</summary>
			Oem5 = 220,

			///<summary>
			///     OEM 6 キー
			///</summary>
			Oem6 = 221,

			///<summary>
			///     米国標準キーボード上の OEM 右角かっこキー
			///</summary>
			OemCloseBrackets = 221,

			///<summary>
			///     OEM 7 キー
			///</summary>
			Oem7 = 222,

			///<summary>
			///     米国標準キーボード上の OEM 一重/二重引用符キー
			///</summary>
			OemQuotes = 222,

			///<summary>
			///     OEM 8 キー
			///</summary>
			Oem8 = 223,

			///<summary>
			///     OEM 102 キー
			///</summary>
			Oem102 = 226,

			///<summary>
			///     RT 102 キーのキーボード上の OEM 山かっこキーまたは円記号キー
			///</summary>
			OemBackslash = 226,

			///<summary>
			///     ProcessKey キー
			///</summary>
			ProcessKey = 229,

			///<summary>
			///     Unicode 文字がキーストロークであるかのように渡されます。Packet のキー値は、キーボード以外の入力手段に使用される 32 ビット仮想キー値の下位ワードです。
			///</summary>
			Packet = 231,

			///<summary>
			///     ATTN キー
			///</summary>
			Attn = 246,

			///<summary>
			///     Crsel キー
			///</summary>
			Crsel = 247,

			///<summary>
			///     Exsel キー
			///</summary>
			Exsel = 248,

			///<summary>
			///     EraseEof キー
			///</summary>
			EraseEof = 249,

			///<summary>
			///     PLAY キー
			///</summary>
			Play = 250,

			///<summary>
			///     ZOOM キー
			///</summary>
			Zoom = 251,

			///<summary>
			///     今後使用するために予約されている定数
			///</summary>
			NoName = 252,

			///<summary>
			///     PA1 キー
			///</summary>
			Pa1 = 253,

			///<summary>
			///     Clear キー
			///</summary>
			OemClear = 254,

			///<summary>
			///     キー値からキー コードを抽出するビット マスク。
			///</summary>
			KeyCode = 65535,

			///<summary>
			///     Shift 修飾子キー
			///</summary>
			Shift = 65536,

			///<summary>
			///     Ctrl 修飾子キー
			///</summary>
			Control = 131072,

			///<summary>
			///     Alt 修飾子キー
			///</summary>
			Alt = 262144,



		}

	}
}