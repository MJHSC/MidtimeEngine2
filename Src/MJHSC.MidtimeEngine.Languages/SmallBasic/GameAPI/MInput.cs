//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.SmallBasic.Library;
using MJHSC.MidtimeEngine;

namespace MJHSC.MidtimeEngine.Languages.SmallBasic.GameAPI {


	/// <summary>
	/// Midtime Engine の画面に関する設定を行います。
	/// </summary>
	[SmallBasicType]
	public static class MInput {

		static dynamic GAInputSS;
		static dynamic GADebug;

		//静的コンストラクタ
		static MInput() {
			GADebug = new StaticMembersDynamicWrapper(MCore.GameAPI.GetType("MJHSC.MidtimeEngine.GameAPI.MDebug"));
			GAInputSS = new StaticMembersDynamicWrapper(MCore.GameAPISS.GetType("MJHSC.MidtimeEngine.GameAPI.ScriptSupport.MInput"));
			GADebug.Write("「MJHSC.MidtimeEngine.Languages.SmallBasic.GameAPI.MInput」がロードされました。");
		}

		/// <summary>
		/// 指定した時間の間、動作を停止します。長さはミリ秒(ms)単位です。（1000ミリ秒で1秒です）
		/// </summary>
		/// <param name="TimeInMS">停止する時間（ミリ秒）</param>
		/// <returns></returns>
		public static Primitive GetButton(Primitive PlayerID, Primitive ButtonID){
			if (
				GAInputSS.GetButton((int)PlayerID, (string)ButtonID)
			) {
				return 1;
			}
			return 0;
		}
		public static Primitive GetKey(Primitive KeyID){
			if (
				GAInputSS.GetKey((string)KeyID)
			) {
				return 1;
			}
			return 0;
		}
		public static Primitive GetLastKey(){
			return GAInputSS.GetLastKey();
		}
		public static Primitive GetMouse(Primitive MouseMoveID){
			return GAInputSS.GetMouse((string)MouseMoveID);
		}
		public static Primitive GetStick(Primitive PlayerID, Primitive StickID){
			return GAInputSS.GetStick((int)PlayerID, (string)StickID);
		}
		public static Primitive GetTrigger(Primitive PlayerID, Primitive TriggerID){
			return GAInputSS.GetTrigger((int)PlayerID, (string)TriggerID);
		}




	}

}