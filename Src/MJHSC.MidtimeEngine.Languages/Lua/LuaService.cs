//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.

using System;
using System.Runtime.InteropServices;
using MJHSC.MidtimeEngine.GameAPI;

namespace MJHSC.MidtimeEngine.Languages.Lua {

	public interface ILuaAccess {
		bool RegisterFunction(LuaType.luaL_Reg[] LR, string Name);

		long GetNextArgAsInt(int Index);
		string GetNextArgAsString(int Index);
		bool GetNextArgAsBoolean(int Index);
		int GetNextArgAsProtectedInt(int Index);
		
		bool SetReturnValue(long Value);
		bool SetReturnValue(string Value);
		bool SetReturnValue(bool Value);
		bool SetReturnProtectedValue(int ProtectedInt);
	}



	public class LuaType {

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int lua_CFunction(IntPtr lua_State);

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
		public struct luaL_Reg {
			public string Name;
			public lua_CFunction Function;
		}

	}

}
