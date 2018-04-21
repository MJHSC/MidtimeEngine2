//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.

using MJHSC.MidtimeEngine;
using MJHSC.MidtimeEngine.Plugins;

namespace MJHSC.MidtimeEngine.GameAPI {

	/// <summary>
	/// デバッグ
	/// </summary>
	[MidtimeFunction2]
	public class MDebug {

		/// <summary>
		/// デバッグログに文字を書き込みます。
		/// </summary>
		/// <param name="Text">デバッグログに書き込みたい文字</param>
		public static void Write(string Text) {
			DebugServer.Write(Text);
			return;
		}

		/// <summary>
		/// 【上級者向け】デバッグログに文字を書き込みます。 string.Format(); と同じ形式が使用できます。
		/// 最低１つ以上、「追加の変数」が必要です。 それが必要でない場合は「MDebug.Write()」を使用してください。
		/// </summary>
		/// <param name="Text">デバッグログに書き込みたい文字</param>
		/// <param name="Objects">追加の変数</param>
		public static void WriteFormat(string Text, params object[] Objects) {
			DebugServer.Write(string.Format(Text, Objects));
			return;
		}

	}

}

