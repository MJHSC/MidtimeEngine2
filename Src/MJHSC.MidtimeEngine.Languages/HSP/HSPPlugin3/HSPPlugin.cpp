//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.

#include <Windows.h>
#include <msclr/marshal.h> 
#include "Include/hsp3struct.h"

#using <System.Windows.Forms.dll>
using namespace msclr::interop; 
using namespace System;
using namespace System::Reflection;


//疑似malloc/free
#undef malloc
#undef free
#define malloc(s) HeapAlloc(GetProcessHeap(), HEAP_ZERO_MEMORY, s)
#define free(s) HeapFree(GetProcessHeap(), 0, s)

int CopyString(char* To, const char* From, int Size){
	if(To == 0 || From == 0){return 0;}
	int i = 0;
	for(i = 0; i<Size; i++){
		if(
			(*(To++) = *(From++))
			==
			0x00
		){
			return i;
		}
	}
	*(To-1) = 0x00; //Safe
	return i;
}



#include <stdio.h>	//******************************************************************

//共通変数
int refstrsize = 0;
int *stat;
int statV26 = 0;
HSPEXINFO* hei;
int Initialized = false;
bool EnvChecked = false;

char** RefstrPointer;
bool ExtendRefstrSupported = false; //v26 = false, v3x = true

//C#
namespace MJHSC {				//1
namespace MidtimeEngine {		//2
namespace Languages {			//3
namespace HSP {					//4
namespace HSPPlugin {			//5

	public ref class HSPProxy{
		public:

			delegate int Dcmdfunc(int p1);
			delegate IntPtr Dreffunc(IntPtr p1, int p2);
			delegate int Dtermfunc(int p1);
			delegate int Dmsgfunc(int p1, int p2, int p3);
			delegate int Deventfunc(int p1, int p2, int p3, IntPtr^ p4);

			static Dcmdfunc^ cmdfunc = nullptr;
			static Dreffunc^ reffunc = nullptr;
			static Dtermfunc^ termfunc = nullptr;
			static Dmsgfunc^ msgfunc = nullptr;
			static Deventfunc^ eventfunc = nullptr;

			static int GetNextArgAsInt(){
				return hei->HspFunc_prm_geti();
			}

			static String^ GetNextArgAsString(){
				char* c = hei->HspFunc_prm_gets();
				return marshal_as<String^>(c);
			}

			static void SetReturnValue(int R){
				*stat = R;
			}

			static void SetReturnValue(String^ R){
				marshal_context ^ mc = gcnew marshal_context();
				const char* TEMP = mc->marshal_as<const char*>(R);

				if(ExtendRefstrSupported){ //Midtime v1互換の拡張refstr
					int newsize = strlen(TEMP); //遅くなるので有効化時のみ数える
					if(refstrsize = newsize){
						refstrsize = newsize + 1;
						free(*(RefstrPointer));
						*(RefstrPointer) = (char*)malloc(refstrsize);
					}
				}

				CopyString(*(RefstrPointer), TEMP, refstrsize);
			}

			static void ResetRefstrSize(){
					refstrsize = 65536;
					free(*(RefstrPointer));
					*(RefstrPointer) = (char*)malloc(refstrsize);
			}
	};
}}}}} //Namespaces: 5



int cmdfuncV3(int p1){
	hei->HspFunc_prm_next();
	if(MJHSC::MidtimeEngine::Languages::HSP::HSPPlugin::HSPProxy::cmdfunc != nullptr){
		return MJHSC::MidtimeEngine::Languages::HSP::HSPPlugin::HSPProxy::cmdfunc(p1);
	}
	hei->HspFunc_puterror( HSPERR_UNSUPPORTED_FUNCTION );
	return RUNMODE_RUN;
}

static void* R; // 返値のための変数
void* reffuncV3(int *p1, int p2){
	if ( *(hei->nptype) != TYPE_MARK ) hei->HspFunc_puterror( HSPERR_INVALID_FUNCPARAM );
	if ( *(hei->npval) != '(' ) hei->HspFunc_puterror( HSPERR_INVALID_FUNCPARAM );
	hei->HspFunc_prm_next();
	R = 0;
	if(MJHSC::MidtimeEngine::Languages::HSP::HSPPlugin::HSPProxy::reffunc != nullptr){
		IntPtr RIP = MJHSC::MidtimeEngine::Languages::HSP::HSPPlugin::HSPProxy::reffunc( IntPtr(p1) , p2);
		R = (void*) RIP.ToPointer();
	} else {
		hei->HspFunc_puterror( HSPERR_UNSUPPORTED_FUNCTION );
	}
	if ( *(hei->nptype) != TYPE_MARK ) hei->HspFunc_puterror( HSPERR_INVALID_FUNCPARAM );
	if ( *(hei->npval) != ')' ) hei->HspFunc_puterror( HSPERR_INVALID_FUNCPARAM );
	hei->HspFunc_prm_next();
	return &R;
}

int termfuncV3(int p1){
	if(MJHSC::MidtimeEngine::Languages::HSP::HSPPlugin::HSPProxy::termfunc != nullptr){
		return MJHSC::MidtimeEngine::Languages::HSP::HSPPlugin::HSPProxy::termfunc(p1);
	}
	return 0;
}

int msgfuncV3(int p1, int p2, int p3){
	if(MJHSC::MidtimeEngine::Languages::HSP::HSPPlugin::HSPProxy::msgfunc != nullptr){
		return MJHSC::MidtimeEngine::Languages::HSP::HSPPlugin::HSPProxy::msgfunc(p1, p2, p3);
	}
	return 0;
}

int eventfuncV3(int p1, int p2, int p3, void *p4){
	if(MJHSC::MidtimeEngine::Languages::HSP::HSPPlugin::HSPProxy::eventfunc != nullptr){
		return MJHSC::MidtimeEngine::Languages::HSP::HSPPlugin::HSPProxy::eventfunc(p1, p2, p3, IntPtr(p4) );
	}
	return 0;
}




void __stdcall Midtime2FunctionCommonInit( HSPEXINFO *exinfo ){
	//登録
	hei = exinfo;

	//HSP、Midtimeどちらかから起動されたのかを判別。GameAPIはMidtimeからしか使えない。
	if(!EnvChecked){ //重いので起動直後の１度だけ行う
		array<Assembly^>^ A = AppDomain::CurrentDomain->GetAssemblies();

		bool MidtimeFlag = false;
		for (int i = 0; i < A->Length; i++) {
			if (A[i]->GetName()->Name == Assembly::GetExecutingAssembly()->GetName()->Name) {
				continue; //自身は別
			}
			if (A[i]->GetName()->Name->IndexOf("MJHSC.Midtime") == 0) {
				MidtimeFlag = true; //MJHSC.Midtimeから始まるdllがあればそれはきっとMidtime
			}
		}
		if (!MidtimeFlag) {		
			bool FoundMidtime = false;
			System::Windows::Forms::MessageBox::Show("このスクリプトはMidtime Engineを使用して実行する必要があります。\n\n(.ax / .ax3 / .ax2 ファイルの代わりに、.hsp / .hsp3 / .hsp2 / .as ファイルを配置することを推奨します。)", "Midtime Engine", System::Windows::Forms::MessageBoxButtons::OK, System::Windows::Forms::MessageBoxIcon::Error);
			ExitProcess(-1);
		}
		EnvChecked = true;
	} //if(!EnvChecked)
}




void __stdcall Midtime2FunctionV3x ( HSP3TYPEINFO *h3ti ){
	//v3 native statの使用
	stat = &(h3ti->hspctx->stat);

	//Midtime v1 互換: 拡張refstrの使用設定
	//	公式配布版ではオフになっています。（検証が完了していないため）
	//	「ExtendRefstrSupported = true;」に変更することで有効化できますが、
	//	・正しく動作しなくなる可能性があります。
	//	・動的に確保するための処理によりフレームごとの速度が同じにならなくなります。（拡大が必要なときは遅い、文字数チェックが遅い、等）
	//	Midtime v1 プラグインで本機能が必要な場合やその他にどうしても必要な場合にのみ有効化してください。
	ExtendRefstrSupported = false;
	refstrsize = 4096; //デフォルトは4KB
	RefstrPointer = &(h3ti->hspctx->refstr); //HSP2.6と同じhspexinfo->refstrには値が入っていないので使えない。
	if(ExtendRefstrSupported){
		*(RefstrPointer) = (char*)malloc(refstrsize);
		refstrsize = 65536;
	}

	//HSP v3x 拡張プラグインとしての初期化
	h3ti->cmdfunc = cmdfuncV3;
	h3ti->reffunc = reffuncV3;
	h3ti->termfunc = termfuncV3;
	h3ti->msgfunc = msgfuncV3;
	h3ti->eventfunc = eventfuncV3;

	//共通処理（v3xではこの関数自体が1度しか呼び出されない）
	Midtime2FunctionCommonInit(h3ti->hspexinfo);
}


int __stdcall Midtime2FunctionV26 ( HSPEXINFO *exinfo, int p1, int p2, int p3 ){
	//v26 emulated statの使用
	statV26 = 0;
	stat = &statV26;
	refstrsize = 4096; //デフォルトは4KB

	//初期化（各HSP26セッションの初回のみ）
	if(!Initialized){
		//Midtime v1 互換: 拡張refstr はMidtime 2以降でのHSP2実行方式では構造上使用できません。
		//(HSP側が持っている別のポインタ変数をここからは操作できない。全体がHSPのMidtime v1の動作を完全再現は不可能)
		ExtendRefstrSupported = false;
		RefstrPointer = &(exinfo->refstr);

		Midtime2FunctionCommonInit(exinfo);
		Initialized = true;
	}

	//この関数自体がv3でのcmdfuncのようなものなので直接処理する。
	int R = 0; //Rが0（エラー無し）なら、emulated statの値を返す。
	if(MJHSC::MidtimeEngine::Languages::HSP::HSPPlugin::HSPProxy::cmdfunc != nullptr){
		R = MJHSC::MidtimeEngine::Languages::HSP::HSPPlugin::HSPProxy::cmdfunc( 0x0000 ); //cmdfunc ID: 0x0000
		if(R == 0){
			return -(*stat);
		}
		return R;
	}
	return 2;
}


#pragma unmanaged
BOOL WINAPI DllMain(HINSTANCE hinstDll, DWORD dwReason, LPVOID lpReserved){
	if(dwReason == DLL_PROCESS_ATTACH){ //新しいHSPセッション
		Initialized = false;
	}
	return 1;
}
#pragma managed