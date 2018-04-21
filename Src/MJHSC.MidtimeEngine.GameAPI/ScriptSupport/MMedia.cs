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
	public static class MMedia {

		public static int CreateSound(string FileName) {
			MJHSC.MidtimeEngine.GameAPI.MMedia M = MJHSC.MidtimeEngine.GameAPI.MMedia.CreateSound(FileName);
			return MJHSC.MidtimeEngine.GameAPI.MMedia._GetManagedID(M);
		}

		public static int CreateVideo(string FileName) {
			MJHSC.MidtimeEngine.GameAPI.MMedia M = MJHSC.MidtimeEngine.GameAPI.MMedia.CreateVideo(FileName);
			return MJHSC.MidtimeEngine.GameAPI.MMedia._GetManagedID(M);
		}

		public static void Reset() {
			MJHSC.MidtimeEngine.GameAPI.MMedia.Reset();
		}

		public static void PlayOnce(int MMediaID) {
			MJHSC.MidtimeEngine.GameAPI.MMedia M = MJHSC.MidtimeEngine.GameAPI.MMedia._GetMMediaByManagedID((int)MMediaID);
			if (M != null) {
				M.PlayOnce();
			}
		}

		public static void PlayLoop(int MMediaID) {
			MJHSC.MidtimeEngine.GameAPI.MMedia M = MJHSC.MidtimeEngine.GameAPI.MMedia._GetMMediaByManagedID((int)MMediaID);
			if (M != null) {
				M.PlayLoop();
			}
		}

		public static void Stop(int MMediaID) {
			MJHSC.MidtimeEngine.GameAPI.MMedia M = MJHSC.MidtimeEngine.GameAPI.MMedia._GetMMediaByManagedID((int)MMediaID);
			if (M != null) {
				M.Stop();
			}
		}

		public static void Pause(int MMediaID) {
			MJHSC.MidtimeEngine.GameAPI.MMedia M = MJHSC.MidtimeEngine.GameAPI.MMedia._GetMMediaByManagedID((int)MMediaID);
			if (M != null) {
				M.Pause();
			}
		}

		public static void Close(int MMediaID) {
			MJHSC.MidtimeEngine.GameAPI.MMedia M = MJHSC.MidtimeEngine.GameAPI.MMedia._GetMMediaByManagedID((int)MMediaID);
			if (M != null) {
				M.Close();
			}
		}

		public static int GetLength(int MMediaID) {
			MJHSC.MidtimeEngine.GameAPI.MMedia M = MJHSC.MidtimeEngine.GameAPI.MMedia._GetMMediaByManagedID((int)MMediaID);
			if (M != null) {
				return M.GetLength();
			}
			return 0;
		}

		public static int GetPosition(int MMediaID) {
			MJHSC.MidtimeEngine.GameAPI.MMedia M = MJHSC.MidtimeEngine.GameAPI.MMedia._GetMMediaByManagedID((int)MMediaID);
			if (M != null) {
				return M.GetPosition();
			}
			return 0;
		}

		public static void SetPosition(int MMediaID, int p) {
			MJHSC.MidtimeEngine.GameAPI.MMedia M = MJHSC.MidtimeEngine.GameAPI.MMedia._GetMMediaByManagedID((int)MMediaID);
			if (M != null) {
				M.SetPosition((int)p);
			}
		}

		public static int GetVolume(int MMediaID) {
			MJHSC.MidtimeEngine.GameAPI.MMedia M = MJHSC.MidtimeEngine.GameAPI.MMedia._GetMMediaByManagedID((int)MMediaID);
			if (M != null) {
				return M.GetVolume();
			}
			return 0;
		}

		public static void SetVolume(int MMediaID, int v) {
			MJHSC.MidtimeEngine.GameAPI.MMedia M = MJHSC.MidtimeEngine.GameAPI.MMedia._GetMMediaByManagedID((int)MMediaID);
			if (M != null) {
				M.SetVolume((int)v);
			}
		}

		public static int GetSpeed(int MMediaID) {
			MJHSC.MidtimeEngine.GameAPI.MMedia M = MJHSC.MidtimeEngine.GameAPI.MMedia._GetMMediaByManagedID((int)MMediaID);
			if (M != null) {
				return M.GetSpeed();
			}
			return 0;
		}

		public static void SetSpeed(int MMediaID, int s) {
			MJHSC.MidtimeEngine.GameAPI.MMedia M = MJHSC.MidtimeEngine.GameAPI.MMedia._GetMMediaByManagedID((int)MMediaID);
			if (M != null) {
				M.SetSpeed((int)s);
			}
		}

	}

}