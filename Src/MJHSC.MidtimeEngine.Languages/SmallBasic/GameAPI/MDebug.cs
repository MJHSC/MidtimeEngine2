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
	/// Midtime自体や、システムに関する操作を行います。
	/// </summary>
	[SmallBasicType]
	public static class MDebug {

		static dynamic GADebug;

		//静的コンストラクタ（SmallBasicに関係ない本体部分）
		static MDebug() {
			GADebug = new StaticMembersDynamicWrapper(MCore.GameAPI.GetType("MJHSC.MidtimeEngine.GameAPI.MDebug"));
			GADebug.Write("「MJHSC.MidtimeEngine.Languages.SmallBasic.GameAPI.MDebug」がロードされました。");
		}

		/// <summary>
		/// デバッグログに文字を書き込みます。
		/// </summary>
		/// <param name="Text">デバッグログに書き込みたい文字</param>
		public static Primitive Write(Primitive Text) {
			GADebug.Write( (string) Text);
			return false;
		}


	}

}