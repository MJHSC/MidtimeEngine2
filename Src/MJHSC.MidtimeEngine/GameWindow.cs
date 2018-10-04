//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.

using System;
using System.Windows.Forms;
using System.Drawing;

namespace MJHSC.MidtimeEngine {

	public class GameWindow : Form {

		private delegate void InputProc(Message m);
		private InputProc IOProc = null;

		private void DefaultIOProc(Message m) { }

		private int _ScreenWidth = 960;
		private int _ScreenHeight = 540;

		public int ScreenWidth {
			get { return this._ScreenWidth;  }
			private set { this._ScreenWidth = value; }
		}
		public int ScreenHeight {
			get { return this._ScreenHeight;  }
			private set { this._ScreenWidth = value; }
		}

		internal GameWindow() {
			IOProc = DefaultIOProc; ChangeTitle("");
			this.ShowIcon = true;
			this.Icon = new Icon(Midtime.GetMidtimeRootPath() + "GameData\\Midtime\\Midtime.ico", 16, 16);

			this.FormBorderStyle = FormBorderStyle.Fixed3D;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.StartPosition = FormStartPosition.CenterScreen;
			this.ClientSize = new System.Drawing.Size(this._ScreenWidth, this._ScreenHeight);
		}

		protected override void OnLoad(EventArgs e) {
			base.OnLoad(e);
		}

		public void ChangeTitle(string Title) {
			this.Text = string.Format("{0} - {1} (Version {2}) - {3} - {4}", Title, Midtime.METitle, Midtime.MEVersion, Midtime.MEBy, Midtime.MEURL);
		}

		public void SetSize(int Width, int Height) {
			this._ScreenWidth = Width;
			this._ScreenHeight = Height;
			this.ClientSize = new System.Drawing.Size(this._ScreenWidth, this._ScreenHeight);
			MoveWindowToCenter();
		}

		internal void MoveWindowToCenter() {
			this.Left = (Screen.PrimaryScreen.Bounds.Width - this._ScreenWidth) / 2;
			this.Top = (Screen.PrimaryScreen.Bounds.Height - this._ScreenHeight) / 2;
		}

		public void ToggleFullscreen() {
			if(this.FormBorderStyle == FormBorderStyle.None){
				this.FormBorderStyle = FormBorderStyle.Fixed3D;
				this.ClientSize = new Size(this._ScreenWidth, this._ScreenHeight);
				this.MoveWindowToCenter();
			} else {
				this.FormBorderStyle = FormBorderStyle.None;
				this.Left = 0;
				this.Top = 0;
				this.ClientSize = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
			}

		}

		public IntPtr GetHWND() {
			return this.Handle;
		}

		protected override void OnPaintBackground(PaintEventArgs pea) { }
		protected override void WndProc(ref Message m) {
			if(m.Msg == 0xFF) { //WM_INPUT
				this.IOProc(m);
			} else {
				base.WndProc(ref m);
			}
		}

	}
}