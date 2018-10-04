//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.

using System;
using System.IO;
using System.Runtime.InteropServices;
using MJHSC.MidtimeEngine.GameAPI;
using MJHSC.MidtimeEngine.Languages.Lua;
using MJHSC.MidtimeEngine.Plugins;

namespace MJHSC.MidtimeEngine.Languages.Lua.HostingV52 {
	public class Language : IMidtimeLanguage {


		internal class LuaDLL {
			//System
			[DllImport(@".\MidtimeEngine\Binaries\Lua.v52.VM.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			internal static extern IntPtr luaL_newstate();

			[DllImport(@".\MidtimeEngine\Binaries\Lua.v52.VM.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			internal static extern int luaL_loadfilex(IntPtr lua_State, string ScriptFileName, IntPtr Mode);

			[DllImport(@".\MidtimeEngine\Binaries\Lua.v52.VM.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			internal static extern int lua_pcallk(IntPtr lua_State, int nargs, int nresults, int msgh, IntPtr k);

			[DllImport(@".\MidtimeEngine\Binaries\Lua.v52.VM.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			internal static extern void lua_pushcclosure(IntPtr lua_State, LuaType.lua_CFunction fn, int n);

			[DllImport(@".\MidtimeEngine\Binaries\Lua.v52.VM.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			internal static extern void luaL_setfuncs(IntPtr lua_State, LuaType.luaL_Reg[] l, int nup);

			[DllImport(@".\MidtimeEngine\Binaries\Lua.v52.VM.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			internal static extern void lua_setglobal(IntPtr lua_State, string name);

			[DllImport(@".\MidtimeEngine\Binaries\Lua.v52.VM.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			internal static extern int lua_gettop(IntPtr lua_State);

			//Table
			[DllImport(@".\MidtimeEngine\Binaries\Lua.v52.VM.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			internal static extern void lua_createtable(IntPtr lua_State, int narray, int nrec);

			//Push
			[DllImport(@".\MidtimeEngine\Binaries\Lua.v52.VM.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			internal static extern IntPtr lua_pushinteger(IntPtr lua_State, long n);

			[DllImport(@".\MidtimeEngine\Binaries\Lua.v52.VM.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			internal static extern IntPtr lua_pushlstring(IntPtr lua_State, string s, int len);

			[DllImport(@".\MidtimeEngine\Binaries\Lua.v52.VM.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			internal static extern void lua_pushlightuserdata(IntPtr lua_State, IntPtr p);

			[DllImport(@".\MidtimeEngine\Binaries\Lua.v52.VM.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			internal static extern void lua_pushboolean(IntPtr lua_State, bool b);

			//Pop
			[DllImport(@".\MidtimeEngine\Binaries\Lua.v52.VM.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			internal static extern IntPtr lua_tolstring(IntPtr lua_State, int idx, int len);

			[DllImport(@".\MidtimeEngine\Binaries\Lua.v52.VM.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			internal static extern int lua_tointegerx(IntPtr lua_State, int idx, ref int pisnum);

			[DllImport(@".\MidtimeEngine\Binaries\Lua.v52.VM.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			internal static extern IntPtr lua_touserdata(IntPtr lua_State, int idx);

			[DllImport(@".\MidtimeEngine\Binaries\Lua.v52.VM.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			internal static extern bool lua_toboolean(IntPtr lua_State, int idx);
		}


		internal class LuaAccess52 : ILuaAccess{
			private IntPtr lua_State;

			public LuaAccess52(IntPtr lua_State) {
				this.lua_State = lua_State;
			}

			public bool RegisterFunction(LuaType.luaL_Reg[] LR, string Name) {
				LuaDLL.lua_createtable(this.lua_State, 0, LR.Length);
				LuaDLL.lua_pushlightuserdata(this.lua_State, IntPtr.Zero);
				LuaDLL.luaL_setfuncs(this.lua_State, LR, 1);
				LuaDLL.lua_setglobal(this.lua_State, Name);
				return true;
			}

			public long GetNextArgAsInt(int Index) {
				int Result = 0;
				int Value = LuaDLL.lua_tointegerx(this.lua_State, Index, ref Result);
				return Value;
			}

			public string GetNextArgAsString(int Index) {
				return Marshal.PtrToStringAnsi(LuaDLL.lua_tolstring(this.lua_State, Index, 0));
			}

			public bool GetNextArgAsBoolean(int Index) {
				return LuaDLL.lua_toboolean(this.lua_State, Index);
			}

			public int GetNextArgAsProtectedInt(int Index) {
				IntPtr ProtectedValue = LuaDLL.lua_touserdata(this.lua_State, Index);
				return (int) ProtectedValue;
			}

			public bool SetReturnValue(long Value) {
				LuaDLL.lua_pushinteger(lua_State, Value);
				return true;
			}

			public bool SetReturnValue(string Value) {
				LuaDLL.lua_pushlstring(lua_State, Value, Value.Length);
				return true;
			}

			public bool SetReturnValue(bool Value) {
				LuaDLL.lua_pushboolean(lua_State, Value);
				return true;
			}

			public bool SetReturnProtectedValue(int ProtectedValue) {
				LuaDLL.lua_pushlightuserdata(lua_State, (IntPtr)ProtectedValue);
				return true;
			}
		}



	
		IntPtr LuaVM;


		public MidtimeResponse CanRunScript(string ScriptFilePath) {
			if (File.Exists(ScriptFilePath + ".lua52")) {
				return MidtimeResponse.OK;
			}

			return MidtimeResponse.Error;
		}

		public LanguageVMEntry StartVM(string ScriptFilePath) {
			this.LuaVM = LuaDLL.luaL_newstate();
			return VMMain;
		}

		public MidtimeResponse EndVM(string ScriptFilePath) {
			return MidtimeResponse.OK;
		}

		public MidtimeResponse VMMain(string ScriptFilePath) {
			int R = -1;

			LuaAccess52 LA52 = new LuaAccess52(this.LuaVM);
			LuaLibrary.Register(LA52);


			if (File.Exists(ScriptFilePath + ".lua52")) {
				R = LuaDLL.luaL_loadfilex(LuaVM, ScriptFilePath + ".lua52", IntPtr.Zero);
			}
			R = LuaDLL.lua_pcallk(LuaVM, 0, 0, 0, IntPtr.Zero);

			if (R != 0) {
				MDebug.WriteFormat("Lua エラー 「{0}」", LA52.GetNextArgAsString(-1));
			}
			MDebug.WriteFormat("LUA エラーコード 「{0}」", R);
	
			return MidtimeResponse.OK;
		}
		



	}

}