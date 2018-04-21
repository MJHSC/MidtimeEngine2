using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using MJHSC.MidtimeEngine;
using MJHSC.MidtimeEngine.GameAPI;
using MJHSC.MidtimeEngine.Plugins;

namespace MJHSC.MidtimeEngine.GameAPI.ScriptSupport {

	[Obsolete("この関数は一部の言語サポートのために内部で使用され、コンテンツでの使用は許可されていません。", false)]
	[MidtimeFunction2Legacy]
	public static class MImage {

		public static void SetTextColor(int R, int G, int B) {
			MJHSC.MidtimeEngine.GameAPI.MImage.SetTextColor(R, G, B);
		}

		public static void SetTextSize(int Size) {
			MJHSC.MidtimeEngine.GameAPI.MImage.SetTextSize(Size);
		}

		public static void SetTextFont(string FontName) {
			MJHSC.MidtimeEngine.GameAPI.MImage.SetTextFont(FontName);
		}

		public static int CreateText(string DrawText) {
			MJHSC.MidtimeEngine.GameAPI.MImage I = MJHSC.MidtimeEngine.GameAPI.MImage.CreateText(DrawText);
			return MJHSC.MidtimeEngine.GameAPI.MImage._GetManagedID(I);
		}

		public static int CreateImage(string FileName) {
			MJHSC.MidtimeEngine.GameAPI.MImage I = MJHSC.MidtimeEngine.GameAPI.MImage.CreateImage(FileName);
			return MJHSC.MidtimeEngine.GameAPI.MImage._GetManagedID(I);
		}

		public static int CreateAnime(string FileName, int FramesCount, int OneFrameInGameFrameRatio, bool CutIsHorizontal) {
			MJHSC.MidtimeEngine.GameAPI.MImage I = MJHSC.MidtimeEngine.GameAPI.MImage.CreateAnime(FileName, FramesCount, OneFrameInGameFrameRatio, CutIsHorizontal);
			return MJHSC.MidtimeEngine.GameAPI.MImage._GetManagedID(I);
		}

		public static bool Move(int MImageID, int X, int Y) {
			MJHSC.MidtimeEngine.GameAPI.MImage I = MJHSC.MidtimeEngine.GameAPI.MImage._GetMImageByManagedID((int)MImageID);
			if (I != null) {
				I.Move((int)X, (int)Y);
				return true;
			}
			return false;
		}

		public static bool SetPosition(int MImageID, int X, int Y) {
			MJHSC.MidtimeEngine.GameAPI.MImage I = MJHSC.MidtimeEngine.GameAPI.MImage._GetMImageByManagedID((int)MImageID);
			if (I != null) {
				I.SetPosition((int)X, (int)Y);
				return true;
			}
			return false;
		}

		public static bool Alpha(int MImageID, int Alpha) {
			MJHSC.MidtimeEngine.GameAPI.MImage I = MJHSC.MidtimeEngine.GameAPI.MImage._GetMImageByManagedID((int)MImageID);
			if (I != null) {
				I.Alpha((int)Alpha);
				return true;
			}
			return false;
		}

		public static bool SetAlpha(int MImageID, int Alpha) {
			MJHSC.MidtimeEngine.GameAPI.MImage I = MJHSC.MidtimeEngine.GameAPI.MImage._GetMImageByManagedID((int)MImageID);
			if (I != null) {
				I.SetAlpha((int)Alpha);
				return true;
			}
			return false;
		}

		public static bool Rotate(int MImageID, int Rotate) {
			MJHSC.MidtimeEngine.GameAPI.MImage I = MJHSC.MidtimeEngine.GameAPI.MImage._GetMImageByManagedID((int)MImageID);
			if (I != null) {
				I.Rotate((int)Rotate);
				return true;
			}
			return false;
		}

		public static bool SetRotate(int MImageID, int Rotate) {
			MJHSC.MidtimeEngine.GameAPI.MImage I = MJHSC.MidtimeEngine.GameAPI.MImage._GetMImageByManagedID((int)MImageID);
			if (I != null) {
				I.SetRotate((int)Rotate);
				return true;
			}
			return false;
		}

		public static bool Draw(int MImageID) {
			MJHSC.MidtimeEngine.GameAPI.MImage I = MJHSC.MidtimeEngine.GameAPI.MImage._GetMImageByManagedID((int)MImageID);
			if (I != null) {
				I.Draw();
				return true;
			}
			return false;
		}

		public static bool InvertHorizontal(int MImageID, bool Invert) {
			MJHSC.MidtimeEngine.GameAPI.MImage I = MJHSC.MidtimeEngine.GameAPI.MImage._GetMImageByManagedID((int)MImageID);
			if (I != null) {
				I.InvertHorizontal(Invert);
				return true;
			}
			return false;
		}

		public static bool InvertVertical(int MImageID, bool Invert) {
			MJHSC.MidtimeEngine.GameAPI.MImage I = MJHSC.MidtimeEngine.GameAPI.MImage._GetMImageByManagedID((int)MImageID);
			if (I != null) {
				I.InvertVertical(Invert);
				return true;
			}
			return false;
		}

		public static bool Close(int MImageID) {
			MJHSC.MidtimeEngine.GameAPI.MImage I = MJHSC.MidtimeEngine.GameAPI.MImage._GetMImageByManagedID((int)MImageID);
			if (I != null) {
				I.Close();
				return true;
			}
			return false;
		}
	}
}