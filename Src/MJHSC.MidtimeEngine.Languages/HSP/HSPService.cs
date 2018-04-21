//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.

using System;
using System.Runtime.InteropServices;
using MJHSC.MidtimeEngine.GameAPI;
using MJHSC.MidtimeEngine.Languages.HSP.HSPPlugin;

namespace MJHSC.MidtimeEngine.Languages.HSP {

	public interface IHSPAccess { 
		int GetNextArgAsInt();
		string GetNextArgAsString();
		bool GetNextArgAsBoolean();

		bool SetReturnValue(int Value);
		bool SetReturnValue(string Value);
		bool SetReturnValue(bool Value);
	}


	//v26, v3xで同じProxy dllなのでここで定義してしまう。
	//v4以降でProxy dllが分かれる場合はLuaと同じようにそれぞれのHosting (実際の名前はaxv**)に実装する
	public class HSPAccessAll : IHSPAccess { 

		public string GetNextArgAsString() {
			return HSPProxy.GetNextArgAsString();
		}

		public int GetNextArgAsInt() {
			return HSPProxy.GetNextArgAsInt();
		}

		public bool GetNextArgAsBoolean() {
			return ((HSPProxy.GetNextArgAsInt()) != 0);
		}

		public bool SetReturnValue(int R) {
			HSPProxy.SetReturnValue(R);
			return true;
		}

		public bool SetReturnValue(bool R) {
			if (R) {
				HSPProxy.SetReturnValue(1);
			} else {
				HSPProxy.SetReturnValue(0);
			}
			return true;
		}

		public bool SetReturnValue(string R) {
			HSPProxy.SetReturnValue(R);
			return true;
		}

	}

}
