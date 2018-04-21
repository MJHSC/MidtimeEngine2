//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.

using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MJHSC.MidtimeEngine.GameAPI {
	public partial class MInput {

		#region ゲームパッド

		internal static void ReportGamePadStatus() {
			string N = "";
			if (GamePad.GetState(PlayerIndex.One).IsConnected) { N += "1P "; };
			if (GamePad.GetState(PlayerIndex.Two).IsConnected) { N += "2P "; };
			if (GamePad.GetState(PlayerIndex.Three).IsConnected) { N += "3P "; };
			if (GamePad.GetState(PlayerIndex.Four).IsConnected) { N += "4P "; };
			if (N == "") { N = "XInput コントローラが接続されていません。"; }
			MDebug.WriteFormat("コントローラの接続（XInput）「{0}」", N);

			MDebug.WriteFormat("コントローラの接続（DirectInput）「{0}」", "(Midtime v2以降では使用できません)");
		}

		internal static void CheckGamePads() {
			CheckGamePad(1);
			CheckGamePad(2);
			CheckGamePad(3);
			CheckGamePad(4);
		}



		internal static void ApplyGamePadButton(int ControllerID, ButtonState BS, ButtonID BID) {
			//コントローラを使用していない（前回と入力が変わっていない）時に、キーボードの入力を上書きしないようにする。
			if (LastPadButton[ControllerID][(int)BID] == (int)BS) { return; }

			if (BS == ButtonState.Pressed) {
				MVCButton[ControllerID][(int)BID] = true;
			} else {
				MVCButton[ControllerID][(int)BID] = false;
			}

			LastPadButton[ControllerID][(int)BID] = (int)BS;
		}

		internal static void CheckGamePad(int ControllerID) { //将来的にはほかの形式もここで選択できるようにしたい。 「10 == DirectInputの１つ目」のように。
			PlayerIndex PI = PlayerIndex.One;
			if (ControllerID == 1) { PI = PlayerIndex.One; }
			if (ControllerID == 2) { PI = PlayerIndex.Two; }
			if (ControllerID == 3) { PI = PlayerIndex.Three; }
			if (ControllerID == 4) { PI = PlayerIndex.Four; }

			GamePadState GPS = GamePad.GetState(PI);
			if (!GPS.IsConnected) {
				return; //未接続なら何もしないで終了
			}

			//スティック
			MVCStick[ControllerID][(int)StickID.LX] = (int)(GPS.ThumbSticks.Left.X * 0x7F);
			MVCStick[ControllerID][(int)StickID.LY] = - (int)(GPS.ThumbSticks.Left.Y * 0x7F);
			MVCStick[ControllerID][(int)StickID.RX] = (int)(GPS.ThumbSticks.Right.X * 0x7F);
			MVCStick[ControllerID][(int)StickID.RY] = - (int)(GPS.ThumbSticks.Right.Y * 0x7F);

			//トリガー
			MVCTrigger[ControllerID][(int)TriggerID.L] = (int)(GPS.Triggers.Left * 0xFF);
			MVCTrigger[ControllerID][(int)TriggerID.R] = (int)(GPS.Triggers.Right * 0xFF);

			//L, R トリガーをボタンとして取得
			if (MVCTrigger[ControllerID][(int)TriggerID.L] > 0x7F) { MVCButton[ControllerID][(int)ButtonID.L] = true; } else { MVCButton[ControllerID][(int)ButtonID.L] = false; }
			if (MVCTrigger[ControllerID][(int)TriggerID.R] > 0x7F) { MVCButton[ControllerID][(int)ButtonID.R] = true; } else { MVCButton[ControllerID][(int)ButtonID.R] = false; }

			#region デジタルボタン処理
			//十字キー
			ApplyGamePadButton(ControllerID, GPS.DPad.Up, ButtonID.Up);
			ApplyGamePadButton(ControllerID, GPS.DPad.Down, ButtonID.Down);
			ApplyGamePadButton(ControllerID, GPS.DPad.Left, ButtonID.Left);
			ApplyGamePadButton(ControllerID, GPS.DPad.Right, ButtonID.Right);
			ApplyGamePadButton(ControllerID, GPS.Buttons.A, ButtonID.A);
			ApplyGamePadButton(ControllerID, GPS.Buttons.B, ButtonID.B);
			ApplyGamePadButton(ControllerID, GPS.Buttons.X, ButtonID.X);
			ApplyGamePadButton(ControllerID, GPS.Buttons.Y, ButtonID.Y);
			ApplyGamePadButton(ControllerID, GPS.Buttons.Start, ButtonID.Start);
			ApplyGamePadButton(ControllerID, GPS.Buttons.Back, ButtonID.Select);
			ApplyGamePadButton(ControllerID, GPS.Buttons.LeftShoulder, ButtonID.L2);
			ApplyGamePadButton(ControllerID, GPS.Buttons.RightShoulder, ButtonID.R2);
			ApplyGamePadButton(ControllerID, GPS.Buttons.LeftStick, ButtonID.L3);
			ApplyGamePadButton(ControllerID, GPS.Buttons.RightStick, ButtonID.R3);
			#endregion

		}

		#endregion

	}
}