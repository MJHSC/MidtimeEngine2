//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.

using System;
using System.IO;
using System.Runtime.InteropServices;
using MJHSC.MidtimeEngine.GameAPI;
using MJHSC.MidtimeEngine.Languages.Lua;
using MJHSC.MidtimeEngine.Plugins;

namespace MJHSC.MidtimeEngine.Languages.Lua.HostingV51 {
	public class Language : IMidtimeLanguage {


		internal class LuaDLL {
			//System
			[DllImport(@".\MidtimeEngine\Binaries\Lua.v51.VM.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			internal static extern IntPtr luaL_newstate();

			[DllImport(@".\MidtimeEngine\Binaries\Lua.v51.VM.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			internal static extern int luaL_loadfilex(IntPtr lua_State, string ScriptFileName, IntPtr Mode);

			[DllImport(@".\MidtimeEngine\Binaries\Lua.v51.VM.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			internal static extern int lua_pcall(IntPtr lua_State, int nargs, int nresults, IntPtr errfunc);

			[DllImport(@".\MidtimeEngine\Binaries\Lua.v51.VM.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			internal static extern void lua_pushcclosure(IntPtr lua_State, LuaType.lua_CFunction fn, int n);

			[DllImport(@".\MidtimeEngine\Binaries\Lua.v51.VM.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			internal static extern void luaL_register(IntPtr lua_State, string libname, LuaType.luaL_Reg[] l);

			[DllImport(@".\MidtimeEngine\Binaries\Lua.v51.VM.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			internal static extern void lua_setglobal(IntPtr lua_State, string name);

			[DllImport(@".\MidtimeEngine\Binaries\Lua.v51.VM.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			internal static extern int lua_gettop(IntPtr lua_State);

			//Table
			[DllImport(@".\MidtimeEngine\Binaries\Lua.v51.VM.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			internal static extern void lua_createtable(IntPtr lua_State, int narray, int nrec);

			//Push
			[DllImport(@".\MidtimeEngine\Binaries\Lua.v51.VM.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			internal static extern IntPtr lua_pushinteger(IntPtr lua_State, long n);

			[DllImport(@".\MidtimeEngine\Binaries\Lua.v51.VM.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			internal static extern IntPtr lua_pushlstring(IntPtr lua_State, string s, int len);

			[DllImport(@".\MidtimeEngine\Binaries\Lua.v51.VM.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			internal static extern void lua_pushlightuserdata(IntPtr lua_State, IntPtr p);

			[DllImport(@".\MidtimeEngine\Binaries\Lua.v51.VM.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			internal static extern void lua_pushboolean(IntPtr lua_State, bool b);

			//Pop
			[DllImport(@".\MidtimeEngine\Binaries\Lua.v51.VM.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			internal static extern IntPtr lua_tolstring(IntPtr lua_State, int idx, int len);

			[DllImport(@".\MidtimeEngine\Binaries\Lua.v51.VM.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			internal static extern int lua_tointeger(IntPtr lua_State, int idx);

			[DllImport(@".\MidtimeEngine\Binaries\Lua.v51.VM.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			internal static extern IntPtr lua_touserdata(IntPtr lua_State, int idx);

			[DllImport(@".\MidtimeEngine\Binaries\Lua.v51.VM.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			internal static extern bool lua_toboolean(IntPtr lua_State, int idx);
		}


		internal class LuaAccess51 : ILuaAccess{
			private IntPtr lua_State;

			public LuaAccess51(IntPtr lua_State) {
				this.lua_State = lua_State;
			}

			public bool RegisterFunction(LuaType.luaL_Reg[] LR, string Name) {
				LuaDLL.lua_createtable(this.lua_State, 0, LR.Length);
				LuaDLL.lua_pushlightuserdata(this.lua_State, IntPtr.Zero);
				LuaDLL.luaL_register(this.lua_State, Name, LR);
				return true;
			}

			public long GetNextArgAsInt(int Index) {
				int Value = LuaDLL.lua_tointeger(this.lua_State, Index);
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
			if (File.Exists(ScriptFilePath + ".lua51")) {
				return MidtimeResponse.OK;
			}
			if (File.Exists(ScriptFilePath + ".luajit")) {
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

			LuaAccess51 LA51 = new LuaAccess51(this.LuaVM);
			LuaLibrary.Register(LA51);

			int R = -1;

			if (File.Exists(ScriptFilePath + ".lua51")) {
				R = LuaDLL.luaL_loadfilex(LuaVM, ScriptFilePath + ".lua51", IntPtr.Zero);
			} else if (File.Exists(ScriptFilePath + ".luajit")) {
				R = LuaDLL.luaL_loadfilex(LuaVM, ScriptFilePath + ".luajit", IntPtr.Zero);
			}
			R = LuaDLL.lua_pcall(LuaVM, 0, 0, IntPtr.Zero);

			if (R != 0) {
				MDebug.WriteFormat("Lua エラー 「{0}」", LA51.GetNextArgAsString(-1));
			}
			MDebug.WriteFormat("LUA エラーコード 「{0}」", R);
	
			return MidtimeResponse.OK;
		}
		



	}

}