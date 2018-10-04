//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.

using System;
namespace MJHSC.MidtimeEngine.Plugins {


	/// <summary>
	/// .NET 対応言語用の関数プラグイン
	/// 「引数」「返値」ともに、全ての型が使用できます。
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class MidtimeFunction2Attribute : Attribute {
		public string Name {
			get;
			private set;
		}
		public string Author = "";

		public MidtimeFunction2Attribute() {
			this.Name = "";
		}
		public MidtimeFunction2Attribute(string Name) {
			this.Name = Name;
		}
	}


	/// <summary>
	/// .NET "非"対応言語用の関数プラグイン
	/// 「引数」「返値」ともにそれぞれ「string」「int」を使用する必要があります。
	/// </summary>
	/// [
	[Obsolete("このタイプのプラグインは推奨されません。また、このタイプの関数プラグインでは「引数」「返値」ともにそれぞれ「string」「int」を使用する必要があります。")]
	public class MidtimeFunction2LegacyAttribute : Attribute {
		public string Name {
			get;
			private set;
		}
		public string Author = "";

		public MidtimeFunction2LegacyAttribute() {
			this.Name = "";
		}
		public MidtimeFunction2LegacyAttribute(string Name) {
			this.Name = Name;
		}
	}

}