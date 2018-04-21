//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.

using System;
using System.IO;
using System.Windows.Forms;

namespace MJHSC.MidtimeEngine.GameAPI {
	public partial class MInput {

		#region フレーム処理
		internal static void Process() {
			CheckGamePads();
		}
		#endregion

		#region Midtime 仮想コントローラ
		internal static bool[][] MVCButton;
		internal static int[][] MVCStick;
		internal static int[][] MVCTrigger;
		internal static int[][] LastPadButton; //コントローラを使用していない時に、キーボードの入力を上書きしないようにする。「Input.X360.cs/ApplyGamePadButton()」で使用。

		internal static void Initialize_VController() {
			int MaxControllers = Enum.GetNames(typeof(Microsoft.Xna.Framework.PlayerIndex)).Length + 1; //0は使用しない

			MVCButton = new bool[MaxControllers][];
			MVCStick = new int[MaxControllers][];
			MVCTrigger = new int[MaxControllers][];

			LastPadButton = new int[MaxControllers][];

			for (int i = 0; i < MaxControllers; i++) {
				MVCButton[i] = new bool[Enum.GetNames(typeof(ButtonID)).Length];
				MVCStick[i] = new int[Enum.GetNames(typeof(StickID)).Length];
				MVCTrigger[i] = new int[Enum.GetNames(typeof(TriggerID)).Length];
				LastPadButton[i] = new int[Enum.GetNames(typeof(ButtonID)).Length];
			}
		}

		/// <summary>
		/// 指定されたプレイヤーのコントローラのボタンの状態を調べます。
		/// </summary>
		/// <param name="ControllerID">コントローラの番号（1～4）</param>
		/// <param name="StickID">調べたいボタンの「ButtonID」</param>
		/// <returns>押されている場合、trueまたは1。
		/// 押されていない場合、falseまたは0。/// </returns>
		public static bool GetButton(int ControllerID, ButtonID ButtonID) {
			if (ControllerID < 1 || MVCStick.Length <= ControllerID) { MDebug.WriteFormat("存在しないコントローラ番号「{0}」が指定されました", ControllerID); return false; }
			try {
				return MVCButton[ControllerID][(int)ButtonID];
			} catch { MDebug.WriteFormat("存在しないコントローラボタン「{0}」が指定されました", ButtonID); }
			return false;
		}

		/// <summary>
		/// 指定されたプレイヤーのコントローラのスティックの状態を調べます。
		/// </summary>
		/// <param name="ControllerID">コントローラの番号（1～4）</param>
		/// <param name="StickID">調べたいスティックの「TriggerID」</param>
		/// <returns>スティックの傾き。（-127～127）</returns>
		public static int GetStick(int ControllerID, StickID StickID) {
			if (ControllerID < 1 || MVCStick.Length <= ControllerID) { MDebug.WriteFormat("存在しないコントローラ番号「{0}」が指定されました", ControllerID); return 0; }
			try {
				return MVCStick[ControllerID][(int)StickID];
			} catch { MDebug.WriteFormat("存在しないスティック「{0}」が指定されました", StickID); }
			return 0;
		}

		/// <summary>
		/// 指定されたプレイヤーのコントローラのトリガーの状態を調べます。
		/// </summary>
		/// <param name="ControllerID">コントローラの番号（1～4）</param>
		/// <param name="TriggerID">調べたいトリガーの「TriggerID」</param>
		/// <returns>トリガーの押し込まれた量。（0～255）</returns>
		public static int GetTrigger(int ControllerID, TriggerID TriggerID) {
			if (ControllerID < 1 || MVCTrigger.Length <= ControllerID) { MDebug.WriteFormat("存在しないコントローラ番号「{0}」が指定されました", ControllerID); return 0; }
			try {
				return MVCTrigger[ControllerID][(int)TriggerID];
			} catch { MDebug.WriteFormat("存在しないトリガー「{0}」が指定されました", TriggerID); }
			return 0;
		}

		#endregion
		
		#region 物理コントローラ (DirectInput) 
		//→ Midtime v2～「Microsoft.DirectX.DirectInput.dll」が 標準で.NET Framework 4しかない環境 (Windows 8.1、10)で読み込めないので一旦削除。 いい方法があれば再実装。
		//代わりに新しい規格「XInput」を実装。「Input.X360.cs」。DirectInputの対応は凍結。
		#endregion

		#region 物理マウス
		internal static int MouseX = 0; //Set by mouse handler
		internal static int MouseY = 0; //Set by mouse handler
		internal static int MouseW = 0; //Set by mouse handler

		/// <summary>
		/// 指定された方向のマウスの位置を調べます。
		/// </summary>
		/// <param name="MouseMoveID">調べたい方向を表す「MouseMoveID」</param>
		/// <returns>マウスの位置座標（画面左上が0）</returns>
		public static int GetMouse(MouseMoveID MouseMoveID) {
			if (MouseMoveID == MouseMoveID.X) { return MouseX; }
			if (MouseMoveID == MouseMoveID.Y) { return MouseY; }
			if (MouseMoveID == MouseMoveID.Wheel) { return MouseW; }
			MDebug.WriteFormat("存在しないマウス位置「{0}」が指定されました", MouseMoveID);
			return 0;
		}
		#endregion

		#region 物理キーボード
		internal static bool[] KeyData = new bool[256];
		internal static KeyID LastKey; //set by key handler

		/// <summary>
		/// 最後に押されたキーボードのボタンを調べます。
		/// 
		/// 現在も押されているか、すでに離されているか等は関係ありません。
		/// 文字入力を受けるときなどに便利です。
		/// </summary>
		/// <returns>最後に押されたボタンの「KeyID」</returns>
		public static KeyID GetLastKey() {
			KeyID KID = (KeyID)LastKey;
			LastKey = 0;
			return KID;
		}

		/// <summary>
		/// 指定されたキーボードのボタンが現在押されているかを確認します。
		/// </summary>
		/// <param name="KeyID">調べたいボタンの「KeyID」</param>
		/// <returns>押されている場合、trueまたは1。
		/// 押されていない場合、falseまたは0。/// </returns>
		public static bool GetKey(KeyID KeyID) {
			try {
				return KeyData[(int)KeyID];
			} catch { MDebug.WriteFormat("存在しないキー「{0}」が指定されました", KeyID); }
			return false;
		}
		#endregion

	}
}