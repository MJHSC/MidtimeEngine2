//Midtime Engine
//	�{�\�[�X�R�[�h�̃��C�Z���X�ɂ��Ă͕t���� LICENSE.html �����ǂ݂��������B
//	Please read LICENSE.html for detail license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.SmallBasic.Library;
using MJHSC.MidtimeEngine;

namespace MJHSC.MidtimeEngine.Languages.SmallBasic.GameAPI {


	/// <summary>
	/// Midtime���̂�A�V�X�e���Ɋւ��鑀����s���܂��B
	/// </summary>
	[SmallBasicType]
	public static class MCore {

		static dynamic GACore;
		static dynamic GADebug;

		//�ÓI�R���X�g���N�^�iSmallBasic�Ɋ֌W�Ȃ��{�̕����j
		private static void MCoreInitialize() {
			GADebug = new StaticMembersDynamicWrapper(MCore.GameAPI.GetType("MJHSC.MidtimeEngine.GameAPI.MDebug"));
			GACore = new StaticMembersDynamicWrapper(MCore.GameAPI.GetType("MJHSC.MidtimeEngine.GameAPI.MCore"));
			GADebug.Write("�uMJHSC.MidtimeEngine.Languages.SmallBasic.GameAPI.MCore�v�����[�h����܂����B");
		}

		/// <summary>
		/// Midtime ���I�����܂��B
		/// �I�����ł��Ȃ��v���b�g�t�H�[���ł͉����N���܂���B
		/// </summary>
		public static Primitive Exit() {
			GACore.Exit();
			return false;
		}

		/// <summary>
		/// �ʂ̃X�N���v�g�����[�h���A���s���܂��B�i���̃X�N���v�g�͂����ŏI�����܂��B�j
		/// </summary>
		/// <param name="ScriptName">���[�h�������X�N���v�g�̖��O (��: Startup)</param>>
		public static Primitive Goto(Primitive SceneName) {
			return GACore.Goto((string)SceneName);
		}

		/// <summary>
		/// �w�肵�����Ԃ̊ԁA������~���܂��B�����̓~���b(ms)�P�ʂł��B�i1000�~���b��1�b�ł��j
		/// </summary>
		/// <param name="TimeInMS">��~���鎞�ԁi�~���b�j</param>
		/// <returns></returns>
		public static Primitive Sleep(Primitive TimeInMS) {
			GACore.Sleep((int) TimeInMS);
			return true;
		}


		/// <summary>
		/// �uwhile MCore.Loop()�v �Ǝg�����ƂŁA�Q�[�����[�v���s���܂��B
		/// �v���C����@�B�ɂ���đ��x���قȂ�܂����A�ő��œ��삵�܂��B
		/// 
		/// �ʏ�́A����ɁuMCore.FPSLoop()�v���g���܂��B
		/// </summary>
		/// <returns></returns>
		public static Primitive Loop() {
			return GACore.Loop();
		}

		/// <summary>
		/// �uwhile MCore.FPSLoop()�v�Ǝg�����ƂŁA�Q�[�����[�v���s���܂��B
		/// ���x�iFPS�j������s���܂�
		/// </summary>
		/// <param name="TargetFPS">�Q�[���̓���FPS�B�i1�`30�j
		/// �ʏ��30���w�肵�Ă��������B
		/// 
		/// �����݁A30�𒴂���FPS�̎w��̓T�|�[�g����Ă��܂���B</param>
		/// <returns></returns>
		public static Primitive LoopFPS(Primitive TargetFPS) {
			return GACore.LoopFPS((int) TargetFPS);
		}






#region �V�X�e�������̊֐�
		internal struct ObjectManager {
			public bool Using;
			public dynamic Obj;
		}

		internal static int SearchUnusedOM(ObjectManager[] DOs) {
			for (int i = 1; i < DOs.Length; i++) {
				if (!DOs[i].Using) {
					DOs[i].Using = true;
					return i;
				}
			}
			return -1;
		}


		//MJHSC.MidtimeEngine.GameAPI �� �ԐړI�ȃ��[�h
		internal static Assembly GameAPI;
		internal static Assembly GameAPISS;

		//MCore �ÓI�R���X�g���N�^: SmallBasic�AMidtime�ǂ��炩����N�����ꂽ�̂��𔻕ʁBGameAPI��Midtime���炵���g���Ȃ��B
		static MCore() {
			//SmallBasic���璼�ڋN���Ȃ̂��AMidtime����̋N���Ȃ̂����m�F
			Assembly[] A = AppDomain.CurrentDomain.GetAssemblies();

			bool MidtimeFlag = false;
			for (int i = 0; i < A.Length; i++) {
				if (A[i].GetName().Name == Assembly.GetExecutingAssembly().GetName().Name) {
					continue; //���g�͕�
				}
				if (A[i].GetName().Name.IndexOf("MJHSC.Midtime") == 0) {
					MidtimeFlag = true; //MJHSC.Midtime����n�܂�dll������΂���͂�����Midtime
				}
			}

			if (!MidtimeFlag) {
				//SmallBasic �̃f�o�b�K�[����̋N��
				Process P = new Process();

				//SmallBasic����N�����Ă���̂ŁAExecutingAssembly��GameData\Scripts�t�H���_�B
				P.StartInfo.FileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\..\..\Midtime2.exe";
				P.Start();
				P.WaitForExit();
				Environment.Exit(0);
			}

			//Midtime ����̋N���BSmallBasic����N�����Ă���ꍇ�́AGameAPI�����[�h�ł��Ȃ�����
			//Midtime�ł��邩��Ƃ킩���Ă��瓮�I�Ƀ��[�h����K�v������B
			MCore.GameAPI = LoadCLRLibrary(".\\MidtimeEngine\\Binaries\\MJHSC.MidtimeEngine.GameAPI.dll");
			MCore.GameAPISS = LoadCLRLibrary(".\\MidtimeEngine\\Binaries\\MJHSC.MidtimeEngine.GameAPI.ScriptSupport.dll");
			MCoreInitialize();

		}

		internal static Assembly LoadCLRLibrary(string FilePath) { //Midtime v2 alpha���p���̃R�[�h���̂܂�
			if (System.IO.File.Exists(FilePath)) {
				FileStream FileLib = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				byte[] MemLib = new byte[FileLib.Length];
				FileLib.Read(MemLib, 0, (int)FileLib.Length);
				Assembly dll = null;
				try {
					dll = Assembly.Load(MemLib);
				} catch { }
				FileLib.Close();
				MemLib = null;
				GC.Collect(0);
				return dll;
			}
			return null;
		}

		private static void Main() { }	//�Â�VS������C#�R���p�C������SmallBasic �i.NET 4.5�j���g���v���O�C���Ƃ��ĔF�����Ȃ�DLL���ł���̂�VS���o�͂����t�@�C���͎g��Ȃ��i�_�~�[��EXE���o������j
										//	VisualStudio 2010 ���Ɩ�蔭���A2010 SP1���Ɩ��N���Ȃ��\���B

#endregion
		
	}

}