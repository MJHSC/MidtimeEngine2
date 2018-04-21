//Midtime Engine
//	�{�\�[�X�R�[�h�̃��C�Z���X�ɂ��Ă͕t���� LICENSE.html �����ǂ݂��������B
//	Please read LICENSE.html for detail license information.

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Forms;

namespace MJHSC.MidtimeEngine {
	public class Graphic {

		//�Ӗ��̂Ȃ�new�̖h�~�B
		internal Graphic() { }

		internal class CantInitializeException : System.Exception {
			public override string Message {
				get { return "DirectX9.0 / XNA4.0 �̏������Ɏ��s���܂����B"; }
			}
		}

		internal static bool CheckXNA() {
			GraphicsAdapter GAdapter = GraphicsAdapter.DefaultAdapter;
			return true; //If XNA is not installed, call this function will cause exception.
		}

		internal static bool Initialize() {
			PresentationParameters PParam = new PresentationParameters();
			PParam.DeviceWindowHandle = Midtime.GameWindow.Handle;
			PParam.IsFullScreen = false;
			PParam.BackBufferWidth = Midtime.GameWindow.ScreenWidth;
			PParam.BackBufferHeight = Midtime.GameWindow.ScreenHeight;

			try {
				GraphicsAdapter GAdapter = GraphicsAdapter.DefaultAdapter;
				Midtime.GDevice = new GraphicsDevice(
					GAdapter,
					GraphicsProfile.Reach,
					PParam
					);
			} catch {
				return false;
			}

			Midtime.GDevice.BlendState = BlendState.AlphaBlend;
			Midtime.GDevice.RasterizerState = RasterizerState.CullNone;

			Clear();
			Render();

			Midtime.SB = new SpriteBatch(Midtime.GDevice);

			return true;
		}

		public static void SBStart() {
				Graphic.Clear();
				try {
					Midtime.SB.Begin();
				} catch { }
		}
		public static void SBEnd() {
			try {
				Midtime.SB.End();
			} catch { }
				Graphic.Render();
		}

		internal static void Render() {
			try {
				Midtime.GDevice.Present();
			} catch {
				//Midtime.GameWindow.Hide();
				MessageBox.Show("�\���󂠂�܂���B���g���̃R���s���[�^�ŃG���[�������������߁A������~���܂����B (GPU�̕��A�Ɏ��s���܂���)"
				, (Midtime.METitle), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
				System.Environment.Exit(-1);
			}
			return;
		}

		internal static void Clear() {
			try {
				Midtime.GDevice.Clear(ClearOptions.Target, Microsoft.Xna.Framework.Color.White, 1.0f, 0);
			} catch { }
			return;
		}
	}
}