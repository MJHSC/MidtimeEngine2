//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.


//Midtime Engineとプラグインが共通で使用する型の定義
using System;

namespace MJHSC.MidtimeEngine.Plugins {

	public enum MidtimeResponse {
		Error = 0, 
		OK = 1,
	}
	
	public delegate MidtimeResponse LanguageVMEntry(string ScriptFilePath);

}

