//Midtime Engine
//	�{�\�[�X�R�[�h�̃��C�Z���X�ɂ��Ă͕t���� LICENSE.html �����ǂ݂��������B
//	Please read LICENSE.html for detail license information.

using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MJHSC.MidtimeEngine.Plugins;
using System.Reflection;
using System.IO;

namespace MJHSC.MidtimeEngine {

	public static partial class Midtime {

		//C#
		[DllImport("kernel32.dll")]
		internal static extern int GetPrivateProfileString(string lpApplicationName, string lpKeyName, string lpDefault, StringBuilder lpReturnedstring, int nSize, string lpFileName);

		static Midtime() {
			SetMidtimePath(Application.StartupPath);
		}

		//�V�X�e��
		public static void SetWindowSize(int Width, int Height) {
			GameWindow.SetSize(Width, Height);
			Graphic.Initialize();
			GameWindow.MoveWindowToCenter();
		}
		internal static bool CheckXNA() {
			GraphicsAdapter GAdapter = GraphicsAdapter.DefaultAdapter;
			return true; //XNA���C���X�g�[������Ă��Ȃ��ꍇ�A"���̊֐����Ăяo�����u��"�ɗ�O����������B 
			//��LGraphicsAdapter���Ăяo�������ł͂Ȃ��̂ŁACheckXNA()�̌Ăяo�����̂�try catch����K�v����B
		}

		//�p�X
		internal static void SetMidtimePath(String MidtimeRoot) {
			PathMidtimeRoot = MidtimeRoot + '\\';
			PathSaveData = PathMidtimeRoot + "SaveData\\";
			PathGameData = PathMidtimeRoot + "GameData\\";
		}
		public static string GetMidtimeRootPath() { //�I�[�v���\�[�X��Midtime2.exe(C#)�ALightness��Midtime2.exe�i.NET�C���X�g�[���`�F�b�N����j�̗����ɑΉ��B
			return PathMidtimeRoot;
		}
		public static string GetSaveDataPath() { //AppData\Roadming�ւ̃f�[�^�ۑ��ɑΉ�����\��B�K�����̊֐����g���B
			return PathSaveData;
		}
		public static string GetGameDataPath() {
			return PathGameData;
		}

		//�X�N���v�g
		public static void Goto(string SceneName) {
			NextSceneName = SceneName;
			ReloadSceneRequired = true;
			Thread.CurrentThread.Abort(); //�����ŌĂяo�������g���I������B
		}

		//�ݒ�擾	
		internal static string GetConfig(string Key) {
			StringBuilder SB = new StringBuilder(1024);
			GetPrivateProfileString("Midtime", Key, string.Empty, SB, SB.Capacity, GetMidtimeRootPath() + "GameData\\Midtime\\Midtime.ini");
			return SB.ToString();
		}



	}
}