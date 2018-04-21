//Midtime Engine
//	�{�\�[�X�R�[�h�̃��C�Z���X�ɂ��Ă͕t���� LICENSE.html �����ǂ݂��������B
//	Please read LICENSE.html for detail license information.

#include <Windows.h>
#include <msclr/marshal.h> 
#include "Include/hsp3struct.h"

#using <System.Windows.Forms.dll>
using namespace msclr::interop; 
using namespace System;
using namespace System::Reflection;


//�^��malloc/free
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

//���ʕϐ�
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

				if(ExtendRefstrSupported){ //Midtime v1�݊��̊g��refstr
					int newsize = strlen(TEMP); //�x���Ȃ�̂ŗL�������̂ݐ�����
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

static void* R; // �Ԓl�̂��߂̕ϐ�
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
	//�o�^
	hei = exinfo;

	//HSP�AMidtime�ǂ��炩����N�����ꂽ�̂��𔻕ʁBGameAPI��Midtime���炵���g���Ȃ��B
	if(!EnvChecked){ //�d���̂ŋN������̂P�x�����s��
		array<Assembly^>^ A = AppDomain::CurrentDomain->GetAssemblies();

		bool MidtimeFlag = false;
		for (int i = 0; i < A->Length; i++) {
			if (A[i]->GetName()->Name == Assembly::GetExecutingAssembly()->GetName()->Name) {
				continue; //���g�͕�
			}
			if (A[i]->GetName()->Name->IndexOf("MJHSC.Midtime") == 0) {
				MidtimeFlag = true; //MJHSC.Midtime����n�܂�dll������΂���͂�����Midtime
			}
		}
		if (!MidtimeFlag) {		
			bool FoundMidtime = false;
			System::Windows::Forms::MessageBox::Show("���̃X�N���v�g��Midtime Engine���g�p���Ď��s����K�v������܂��B\n\n(.ax / .ax3 / .ax2 �t�@�C���̑���ɁA.hsp / .hsp3 / .hsp2 / .as �t�@�C����z�u���邱�Ƃ𐄏����܂��B)", "Midtime Engine", System::Windows::Forms::MessageBoxButtons::OK, System::Windows::Forms::MessageBoxIcon::Error);
			ExitProcess(-1);
		}
		EnvChecked = true;
	} //if(!EnvChecked)
}




void __stdcall Midtime2FunctionV3x ( HSP3TYPEINFO *h3ti ){
	//v3 native stat�̎g�p
	stat = &(h3ti->hspctx->stat);

	//Midtime v1 �݊�: �g��refstr�̎g�p�ݒ�
	//	�����z�z�łł̓I�t�ɂȂ��Ă��܂��B�i���؂��������Ă��Ȃ����߁j
	//	�uExtendRefstrSupported = true;�v�ɕύX���邱�ƂŗL�����ł��܂����A
	//	�E���������삵�Ȃ��Ȃ�\��������܂��B
	//	�E���I�Ɋm�ۂ��邽�߂̏����ɂ��t���[�����Ƃ̑��x�������ɂȂ�Ȃ��Ȃ�܂��B�i�g�傪�K�v�ȂƂ��͒x���A�������`�F�b�N���x���A���j
	//	Midtime v1 �v���O�C���Ŗ{�@�\���K�v�ȏꍇ�₻�̑��ɂǂ����Ă��K�v�ȏꍇ�ɂ̂ݗL�������Ă��������B
	ExtendRefstrSupported = false;
	refstrsize = 4096; //�f�t�H���g��4KB
	RefstrPointer = &(h3ti->hspctx->refstr); //HSP2.6�Ɠ���hspexinfo->refstr�ɂ͒l�������Ă��Ȃ��̂Ŏg���Ȃ��B
	if(ExtendRefstrSupported){
		*(RefstrPointer) = (char*)malloc(refstrsize);
		refstrsize = 65536;
	}

	//HSP v3x �g���v���O�C���Ƃ��Ă̏�����
	h3ti->cmdfunc = cmdfuncV3;
	h3ti->reffunc = reffuncV3;
	h3ti->termfunc = termfuncV3;
	h3ti->msgfunc = msgfuncV3;
	h3ti->eventfunc = eventfuncV3;

	//���ʏ����iv3x�ł͂��̊֐����̂�1�x�����Ăяo����Ȃ��j
	Midtime2FunctionCommonInit(h3ti->hspexinfo);
}


int __stdcall Midtime2FunctionV26 ( HSPEXINFO *exinfo, int p1, int p2, int p3 ){
	//v26 emulated stat�̎g�p
	statV26 = 0;
	stat = &statV26;
	refstrsize = 4096; //�f�t�H���g��4KB

	//�������i�eHSP26�Z�b�V�����̏���̂݁j
	if(!Initialized){
		//Midtime v1 �݊�: �g��refstr ��Midtime 2�ȍ~�ł�HSP2���s�����ł͍\����g�p�ł��܂���B
		//(HSP���������Ă���ʂ̃|�C���^�ϐ�����������͑���ł��Ȃ��B�S�̂�HSP��Midtime v1�̓�������S�Č��͕s�\)
		ExtendRefstrSupported = false;
		RefstrPointer = &(exinfo->refstr);

		Midtime2FunctionCommonInit(exinfo);
		Initialized = true;
	}

	//���̊֐����̂�v3�ł�cmdfunc�̂悤�Ȃ��̂Ȃ̂Œ��ڏ�������B
	int R = 0; //R��0�i�G���[�����j�Ȃ�Aemulated stat�̒l��Ԃ��B
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
	if(dwReason == DLL_PROCESS_ATTACH){ //�V����HSP�Z�b�V����
		Initialized = false;
	}
	return 1;
}
#pragma managed