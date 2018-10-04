//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.

using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MJHSC.MidtimeEngine {

	public sealed class Image{
		private Texture2D T;
		private AnimeData AData;

		private Vector2 XYPosition;
		private Vector2 Center;
		private Microsoft.Xna.Framework.Rectangle UVPosition;

		private SpriteEffects Invert = SpriteEffects.None;

		private struct AnimeData {
			public bool Use;
			public bool DirectionIsHorizontal;
			public int FrameSize;
			public int TotalFrames;
			public int Delay;
			public int CurrentFrame;
			public int CurrentDelay;
		}

		private Image(int W, int H) {
			this.T = new Texture2D(Midtime.GDevice, W, H);
			this.Center = new Vector2();
			this.Center.X = W / 2;
			this.Center.Y = H / 2;

			this.XYPosition = new Vector2();

			this.UVPosition = new Microsoft.Xna.Framework.Rectangle();
			this.UVPosition.X = 0;
			this.UVPosition.Y = 0;
			this.UVPosition.Width = W;
			this.UVPosition.Height = H;
		}

		public void Draw(int X, int Y, int Alpha, int Degree) {

			try {
				float FAlpha = (float)Alpha / 255F;

				float Radian = 0;
				if (Degree != 0) { //ほとんどの場合０なので、高速化
					Radian = Degree / 57.295780f;
				}

				if(this.AData.Use) { //Anime
					if (!this.AData.DirectionIsHorizontal) {
						this.UVPosition.X = 0;
						this.UVPosition.Y = (this.AData.FrameSize * this.AData.CurrentFrame);
						this.UVPosition.Width = this.T.Width;
						this.UVPosition.Height = this.AData.FrameSize;
					} else {
						this.UVPosition.X = (this.AData.FrameSize * this.AData.CurrentFrame);
						this.UVPosition.Y = 0;
						this.UVPosition.Width = this.AData.FrameSize;
						this.UVPosition.Height = this.T.Height;
					}
					if(this.AData.CurrentDelay == 0) {
						this.AData.CurrentFrame++;
						this.AData.CurrentDelay = this.AData.Delay;
					} else {
						this.AData.CurrentDelay--;
					}
					if(this.AData.CurrentFrame == this.AData.TotalFrames) { this.AData.CurrentFrame = 0; }

					this.XYPosition.X = X;
					this.XYPosition.Y = Y;
					Midtime.SB.Draw(this.T, this.XYPosition, this.UVPosition, Microsoft.Xna.Framework.Color.White * FAlpha, 0, Vector2.Zero, 1.0f, this.Invert, 0);
				} else {
					this.XYPosition.X = X + this.Center.X;
					this.XYPosition.Y = Y + this.Center.Y;
//					Console.WriteLine("Texture2D Draw: {0}, {1}, {2}, {3}", this.T, this.XYPosition, this.UVPosition, Microsoft.Xna.Framework.Color.White * FAlpha);
					Midtime.SB.Draw(this.T, this.XYPosition, this.UVPosition, Microsoft.Xna.Framework.Color.White * FAlpha, Radian, this.Center, 1.0f, this.Invert, 0);
				}

			} catch(Exception E) { MessageBox.Show(E.ToString()); }
		}

		public void Close() {
			try {
				this.T.Dispose();
			} catch { }
		}

		public void SetInvert(bool NowRevert, bool InvertHorizontal) {
			if (InvertHorizontal) {
				if (NowRevert) {
					this.Invert |= SpriteEffects.FlipHorizontally;
				} else {
					this.Invert &= ~SpriteEffects.FlipHorizontally;
				}
			} else {
				if (NowRevert) {
					this.Invert |= SpriteEffects.FlipVertically;
				} else {
					this.Invert &= ~SpriteEffects.FlipVertically;
				}
			}
		}

		public void SetAnimeData(int FramesCount, int OneFrameInGameFrameRatio, bool CutIsHorizontal) {
			this.AData.Use = true;
			this.AData.DirectionIsHorizontal = CutIsHorizontal;
			this.AData.TotalFrames = FramesCount;
			this.AData.Delay = OneFrameInGameFrameRatio;
			this.AData.CurrentFrame = 0;
			this.AData.CurrentDelay = 0;
			if (CutIsHorizontal) {
				this.AData.FrameSize = this.T.Width / FramesCount;
			} else {
				this.AData.FrameSize = this.T.Height / FramesCount;
			}
		}



		//Static
		private static bool ChangedTextSettings = true;
		private static System.Drawing.Font F;
		private static System.Drawing.Color TColor = System.Drawing.Color.FromArgb(255, 255, 255, 255);

		private static int TSize = 16;
		private static string FontName = "Meiryo";

		public static void SetTextColor(int R, int G, int B) {
			TColor = System.Drawing.Color.FromArgb(255, R, G, B);
		}
		public static void SetTextSize(int P) {
			TSize = P;
			ChangedTextSettings = true;
			return;
		}
		public static void SetFont(string FN) {
			FontName = FN;
			ChangedTextSettings = true;
			return;
		}

		public static Image CreateFromText(string DrawText) {
			if(ChangedTextSettings) {
				F = new System.Drawing.Font(FontName, TSize, FontStyle.Regular, GraphicsUnit.Pixel);
			}
				Size size = TextRenderer.MeasureText(DrawText, F);
				int w = size.Width * 12 / 10;
				Bitmap bitmap = new Bitmap(w, size.Height);
				Graphics G = Graphics.FromImage(bitmap);
				G.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
				//			TextRenderer.DrawText(G, DrawText, F, new System.Drawing.Point(0, 0), TColor);
				G.DrawString(DrawText, F, new SolidBrush(TColor), 0, 0);
				return CreateFromBitmap(bitmap);
		}

		public static Image CreateFromFile(string FileName) {
			try {
				FileStream ImageFile = new FileStream(@".\GameData\Images\" + FileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
				System.Drawing.Bitmap image = (System.Drawing.Bitmap)System.Drawing.Image.FromStream(ImageFile, true);
				return CreateFromBitmap(image);
			} catch {}
			return null;
		}

		private static Image CreateFromBitmap(Bitmap image) {
			try {
				int PixelBytes = 4; // iピクセル辺りの色情報を表すデータの数（RGBAなので4つ
				byte[] pixels = new byte[image.Width * image.Height * PixelBytes];
				System.Drawing.Rectangle lock_rect = new System.Drawing.Rectangle { X = 0, Y = 0, Width = image.Width, Height = image.Height };
				System.Drawing.Imaging.BitmapData BMPData = image.LockBits(lock_rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
				for(int y = 0; y < image.Height; y++) {
					for(int x = 0; x < image.Width; x++) {
						int pixel_target = x * PixelBytes + BMPData.Stride * y;
						int array_index = (y * image.Width + x) * PixelBytes;
						for(int i = 0; i < PixelBytes; i++) {
							pixels[array_index + i] = System.Runtime.InteropServices.Marshal.ReadByte(BMPData.Scan0, pixel_target + PixelBytes - ((1 + i) % PixelBytes) - 1);
						}
					}
				}
				image.UnlockBits(BMPData);
				Image I = new Image(image.Width, image.Height);
				I.T.SetData<byte>(pixels);
				image.Dispose();
				return I;
			} catch { }
			return null;
		}

	}
}