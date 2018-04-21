//Midtime Engine
//	�{�\�[�X�R�[�h�̃��C�Z���X�ɂ��Ă͕t���� LICENSE.html �����ǂ݂��������B
//	Please read LICENSE.html for detail license information.

using System;
using System.IO;
using System.Windows.Forms;

namespace MJHSC.MidtimeEngine.GameAPI {
	public partial class MInput {

		/// <summary>
		/// �u�L�[�{�[�h�̃{�^���v��\���܂��B
		/// </summary>
		public enum KeyID : int {		//�uSystem.Windows.Forms.Keys�v����R�s�[�B���ꂻ�ꂪ�ύX���ꂽ�ꍇ�͐��������삵�Ȃ��Ȃ�B
			//�������A�o�[�W�������w�肵�āuSystem.Windows.Forms�v�����[�h���Ă���̂Ńr���h���Ȃ����Ȃ�����i���[�U�[���ŏ���Ɂj���̖�肪�������Ȃ��͂��B

			///<summary>
			///     �L�[�l����C���q�𒊏o����r�b�g �}�X�N�B
			///</summary>
			Modifiers = -65536,

			///<summary>
			///     �L�[���͂Ȃ�
			///</summary>
			None = 0,

			///<summary>
			///     �}�E�X�̍��{�^��
			///</summary>
			LButton = 1,

			///<summary>
			///     �}�E�X�̉E�{�^��
			///</summary>
			RButton = 2,

			///<summary>
			///     Cancel �L�[
			///</summary>
			Cancel = 3,

			///<summary>
			///     �}�E�X�̒����{�^�� (3 �{�^�� �}�E�X�̏ꍇ)
			///</summary>
			MButton = 4,

			///<summary>
			///     x �}�E�X�� 1 �Ԗڂ̃{�^�� (5 �{�^�� �}�E�X�̏ꍇ)
			///</summary>
			XButton1 = 5,

			///<summary>
			///     x �}�E�X�� 2 �Ԗڂ̃{�^�� (5 �{�^�� �}�E�X�̏ꍇ)
			///</summary>
			XButton2 = 6,

			///<summary>
			///     BackSpace �L�[
			///</summary>
			Back = 8,

			///<summary>
			///     TAB �L�[
			///</summary>
			Tab = 9,

			///<summary>
			///     ���C�� �t�B�[�h �L�[
			///</summary>
			LineFeed = 10,

			///<summary>
			///     Clear �L�[
			///</summary>
			Clear = 12,

			///<summary>
			///     Enter �L�[
			///</summary>
			Enter = 13,

			///<summary>
			///     Return �L�[
			///</summary>
			Return = 13,

			///<summary>
			///     Shift �L�[
			///</summary>
			ShiftKey = 16,

			///<summary>
			///     CTRL �L�[
			///</summary>
			ControlKey = 17,

			///<summary>
			///     Alt �L�[
			///</summary>
			Menu = 18,

			///<summary>
			///     Pause �L�[
			///</summary>
			Pause = 19,

			///<summary>
			///     CAPS LOCK �L�[
			///</summary>
			CapsLock = 20,

			///<summary>
			///     CAPS LOCK �L�[
			///</summary>
			Capital = 20,

			///<summary>
			///     IME ���ȃ��[�h �L�[
			///</summary>
			KanaMode = 21,

			///<summary>
			///     IME �n���O�� ���[�h �L�[(�݊�����ۂ��߂ɕێ�����Ă��܂��BHangulMode ���g�p���܂�)
			///</summary>
			HanguelMode = 21,

			///<summary>
			///     IME �n���O�� ���[�h �L�[
			///</summary>
			HangulMode = 21,

			///<summary>
			///     IME Junja ���[�h �L�[
			///</summary>
			JunjaMode = 23,

			///<summary>
			///     IME Final ���[�h �L�[
			///</summary>
			FinalMode = 24,

			///<summary>
			///     IME �������[�h �L�[
			///</summary>
			KanjiMode = 25,

			///<summary>
			///     IME Hanja ���[�h �L�[
			///</summary>
			HanjaMode = 25,

			///<summary>
			///     ESC �L�[
			///</summary>
			Escape = 27,

			///<summary>
			///     IME �ϊ��L�[
			///</summary>
			IMEConvert = 28,

			///<summary>
			///     IME ���ϊ��L�[
			///</summary>
			IMENonconvert = 29,

			///<summary>
			///     IME Accept �L�[
			///</summary>
			IMEAceept = 30,

			///<summary>
			///     IME Accept �L�[ (System.Windows.Forms.Keys.IMEAceept �̑���Ɏg�p���܂�)
			///</summary>
			IMEAccept = 30,

			///<summary>
			///     IME ���[�h�ύX�L�[
			///</summary>
			IMEModeChange = 31,

			///<summary>
			///     Space �L�[
			///</summary>
			Space = 32,

			///<summary>
			///     PageUp �L�[
			///</summary>
			Prior = 33,

			///<summary>
			///     PageUp �L�[
			///</summary>
			PageUp = 33,

			///<summary>
			///     PAGE DOWN �L�[
			///</summary>
			Next = 34,

			///<summary>
			///     PAGE DOWN �L�[
			///</summary>
			PageDown = 34,

			///<summary>
			///     END �L�[
			///</summary>
			End = 35,

			///<summary>
			///     HOME �L�[
			///</summary>
			Home = 36,

			///<summary>
			///     �� �L�[
			///</summary>
			Left = 37,

			///<summary>
			///     �� �L�[
			///</summary>
			Up = 38,

			///<summary>
			///     �� �L�[
			///</summary>
			Right = 39,

			///<summary>
			///     �� �L�[
			///</summary>
			Down = 40,

			///<summary>
			///     Select �L�[
			///</summary>
			Select = 41,

			///<summary>
			///     Print �L�[
			///</summary>
			Print = 42,

			///<summary>
			///     Execute �L�[
			///</summary>
			Execute = 43,

			///<summary>
			///     PrintScreen �L�[
			///</summary>
			PrintScreen = 44,

			///<summary>
			///     PrintScreen �L�[
			///</summary>
			Snapshot = 44,

			///<summary>
			///     INS �L�[
			///</summary>
			Insert = 45,

			///<summary>
			///     DEL �L�[
			///</summary>
			Delete = 46,

			///<summary>
			///     HELP �L�[
			///</summary>
			Help = 47,

			///<summary>
			///     0 �L�[
			///</summary>
			D0 = 48,

			///<summary>
			///     1 �L�[
			///</summary>
			D1 = 49,

			///<summary>
			///     2 �L�[
			///</summary>
			D2 = 50,

			///<summary>
			///     3 �L�[
			///</summary>
			D3 = 51,

			///<summary>
			///     4 �L�[
			///</summary>
			D4 = 52,

			///<summary>
			///     5 �L�[
			///</summary>
			D5 = 53,

			///<summary>
			///     6 �L�[
			///</summary>
			D6 = 54,

			///<summary>
			///     7 �L�[
			///</summary>
			D7 = 55,

			///<summary>
			///     8 �L�[
			///</summary>
			D8 = 56,

			///<summary>
			///     9 �L�[
			///</summary>
			D9 = 57,

			///<summary>
			///     A �L�[
			///</summary>
			A = 65,

			///<summary>
			///     B �L�[
			///</summary>
			B = 66,

			///<summary>
			///     C �L�[
			///</summary>
			C = 67,

			///<summary>
			///     D �L�[
			///</summary>
			D = 68,

			///<summary>
			///     E �L�[
			///</summary>
			E = 69,

			///<summary>
			///     F �L�[
			///</summary>
			F = 70,

			///<summary>
			///     G �L�[
			///</summary>
			G = 71,

			///<summary>
			///     H �L�[
			///</summary>
			H = 72,

			///<summary>
			///     I �L�[
			///</summary>
			I = 73,

			///<summary>
			///     J �L�[
			///</summary>
			J = 74,

			///<summary>
			///     K �L�[
			///</summary>
			K = 75,

			///<summary>
			///     L �L�[
			///</summary>
			L = 76,

			///<summary>
			///     M �L�[
			///</summary>
			M = 77,

			///<summary>
			///     N �L�[
			///</summary>
			N = 78,

			///<summary>
			///     O �L�[
			///</summary>
			O = 79,

			///<summary>
			///     P �L�[
			///</summary>
			P = 80,

			///<summary>
			///     Q �L�[
			///</summary>
			Q = 81,

			///<summary>
			///     R �L�[
			///</summary>
			R = 82,

			///<summary>
			///     S �L�[
			///</summary>
			S = 83,

			///<summary>
			///     T �L�[
			///</summary>
			T = 84,

			///<summary>
			///     U �L�[
			///</summary>
			U = 85,

			///<summary>
			///     V �L�[
			///</summary>
			V = 86,

			///<summary>
			///     W �L�[
			///</summary>
			W = 87,

			///<summary>
			///     X �L�[
			///</summary>
			X = 88,

			///<summary>
			///     Y �L�[
			///</summary>
			Y = 89,

			///<summary>
			///     Z �L�[
			///</summary>
			Z = 90,

			///<summary>
			///     ���� Windows ���S �L�[ (Microsoft Natural Keyboard)
			///</summary>
			LWin = 91,

			///<summary>
			///     �E�� Windows ���S �L�[ (Microsoft Natural Keyboard)
			///</summary>
			RWin = 92,

			///<summary>
			///     �A�v���P�[�V���� �L�[ (Microsoft Natural Keyboard)
			///</summary>
			Apps = 93,

			///<summary>
			///     �R���s���[�^�[�̃X���[�v �L�[
			///</summary>
			Sleep = 95,

			///<summary>
			///     ���l�L�[�p�b�h�� 0 �L�[
			///</summary>
			NumPad0 = 96,

			///<summary>
			///     ���l�L�[�p�b�h�� 1 �L�[
			///</summary>
			NumPad1 = 97,

			///<summary>
			///     ���l�L�[�p�b�h�� 2 �L�[
			///</summary>
			NumPad2 = 98,

			///<summary>
			///     ���l�L�[�p�b�h�� 3 �L�[
			///</summary>
			NumPad3 = 99,

			///<summary>
			///     ���l�L�[�p�b�h�� 4 �L�[
			///</summary>
			NumPad4 = 100,

			///<summary>
			///     ���l�L�[�p�b�h�� 5 �L�[
			///</summary>
			NumPad5 = 101,

			///<summary>
			///     ���l�L�[�p�b�h�� 6 �L�[
			///</summary>
			NumPad6 = 102,

			///<summary>
			///     ���l�L�[�p�b�h�� 7 �L�[
			///</summary>
			NumPad7 = 103,

			///<summary>
			///     ���l�L�[�p�b�h�� 8 �L�[
			///</summary>
			NumPad8 = 104,

			///<summary>
			///     ���l�L�[�p�b�h�� 9 �L�[
			///</summary>
			NumPad9 = 105,

			///<summary>
			///     ��Z�L�� (*) �L�[
			///</summary>
			Multiply = 106,

			///<summary>
			///     Add �L�[
			///</summary>
			Add = 107,

			///<summary>
			///     ��؂�L���L�[
			///</summary>
			Separator = 108,

			///<summary>
			///     ���Z�L�� (-) �L�[
			///</summary>
			Subtract = 109,

			///<summary>
			///     �����_�L�[
			///</summary>
			Decimal = 110,

			///<summary>
			///     ���Z�L�� (/) �L�[
			///</summary>
			Divide = 111,

			///<summary>
			///     F1 �L�[
			///</summary>
			F1 = 112,

			///<summary>
			///     F2 �L�[
			///</summary>
			F2 = 113,

			///<summary>
			///     F3 �L�[
			///</summary>
			F3 = 114,

			///<summary>
			///     F4 �L�[
			///</summary>
			F4 = 115,

			///<summary>
			///     F5 �L�[
			///</summary>
			F5 = 116,

			///<summary>
			///     F6 �L�[
			///</summary>
			F6 = 117,

			///<summary>
			///     F7 �L�[
			///</summary>
			F7 = 118,

			///<summary>
			///     F8 �L�[
			///</summary>
			F8 = 119,

			///<summary>
			///     F9 �L�[
			///</summary>
			F9 = 120,

			///<summary>
			///     F10 �L�[
			///</summary>
			F10 = 121,

			///<summary>
			///     F11 �L�[
			///</summary>
			F11 = 122,

			///<summary>
			///     F12 �L�[
			///</summary>
			F12 = 123,

			///<summary>
			///     F13 �L�[
			///</summary>
			F13 = 124,

			///<summary>
			///     F14 �L�[
			///</summary>
			F14 = 125,

			///<summary>
			///     F15 �L�[
			///</summary>
			F15 = 126,

			///<summary>
			///     F16 �L�[
			///</summary>
			F16 = 127,

			///<summary>
			///     F17 �L�[
			///</summary>
			F17 = 128,

			///<summary>
			///     F18 �L�[
			///</summary>
			F18 = 129,

			///<summary>
			///     F19 �L�[
			///</summary>
			F19 = 130,

			///<summary>
			///     F20 �L�[
			///</summary>
			F20 = 131,

			///<summary>
			///     F21 �L�[
			///</summary>
			F21 = 132,

			///<summary>
			///     F22 �L�[
			///</summary>
			F22 = 133,

			///<summary>
			///     F23 �L�[
			///</summary>
			F23 = 134,

			///<summary>
			///     F24 �L�[
			///</summary>
			F24 = 135,

			///<summary>
			///     NUM LOCK �L�[
			///</summary>
			NumLock = 144,

			///<summary>
			///     ScrollLock �L�[
			///</summary>
			Scroll = 145,

			///<summary>
			///     ���� Shift �L�[
			///</summary>
			LShiftKey = 160,

			///<summary>
			///     �E�� Shift �L�[
			///</summary>
			RShiftKey = 161,

			///<summary>
			///     ���� Ctrl �L�[
			///</summary>
			LControlKey = 162,

			///<summary>
			///     �E�� Ctrl �L�[
			///</summary>
			RControlKey = 163,

			///<summary>
			///     ���� Alt �L�[
			///</summary>
			LMenu = 164,

			///<summary>
			///     �E�� Alt �L�[
			///</summary>
			RMenu = 165,

			///<summary>
			///     �߂�L�[
			///</summary>
			BrowserBack = 166,

			///<summary>
			///     �i�ރL�[
			///</summary>
			BrowserForward = 167,

			///<summary>
			///     �X�V�L�[
			///</summary>
			BrowserRefresh = 168,

			///<summary>
			///     ���~�L�[
			///</summary>
			BrowserStop = 169,

			///<summary>
			///     �����L�[
			///</summary>
			BrowserSearch = 170,

			///<summary>
			///     ���C�ɓ���L�[
			///</summary>
			BrowserFavorites = 171,

			///<summary>
			///     �z�[�� �L�[
			///</summary>
			BrowserHome = 172,

			///<summary>
			///     �~���[�g �L�[
			///</summary>
			VolumeMute = 173,

			///<summary>
			///     ���� - �L�[
			///</summary>
			VolumeDown = 174,

			///<summary>
			///     ���� + �L�[
			///</summary>
			VolumeUp = 175,

			///<summary>
			///     ���̃g���b�N �L�[
			///</summary>
			MediaNextTrack = 176,

			///<summary>
			///     �O�̃g���b�N �L�[
			///</summary>
			MediaPreviousTrack = 177,

			///<summary>
			///     ��~�L�[
			///</summary>
			MediaStop = 178,

			///<summary>
			///     �Đ�/�ꎞ��~�L�[
			///</summary>
			MediaPlayPause = 179,

			///<summary>
			///     ���[�� �z�b�g �L�[
			///</summary>
			LaunchMail = 180,

			///<summary>
			///     ���f�B�A �L�[
			///</summary>
			SelectMedia = 181,

			///<summary>
			///     �J�X�^�� �z�b�g �L�[ 1
			///</summary>
			LaunchApplication1 = 182,

			///<summary>
			///     �J�X�^�� �z�b�g �L�[ 2
			///</summary>
			LaunchApplication2 = 183,

			///<summary>
			///     OEM 1 �L�[
			///</summary>
			Oem1 = 186,

			///<summary>
			///     �č��W���L�[�{�[�h��� OEM �Z�~�R���� �L�[
			///</summary>
			OemSemicolon = 186,

			///<summary>
			///     ���܂��͒n��ʃL�[�{�[�h��� OEM �v���X �L�[
			///</summary>
			Oemplus = 187,

			///<summary>
			///     ���܂��͒n��ʃL�[�{�[�h��� OEM �R���} �L�[
			///</summary>
			Oemcomma = 188,

			///<summary>
			///     ���܂��͒n��ʃL�[�{�[�h��� OEM �}�C�i�X �L�[
			///</summary>
			OemMinus = 189,

			///<summary>
			///     ���܂��͒n��ʃL�[�{�[�h��� OEM �s���I�h �L�[
			///</summary>
			OemPeriod = 190,

			///<summary>
			///     �č��W���L�[�{�[�h��� OEM �^�╄�L�[
			///</summary>
			OemQuestion = 191,

			///<summary>
			///     OEM 2 �L�[
			///</summary>
			Oem2 = 191,

			///<summary>
			///     �č��W���L�[�{�[�h��� OEM �e�B���_ �L�[
			///</summary>
			Oemtilde = 192,

			///<summary>
			///     OEM 3 �L�[
			///</summary>
			Oem3 = 192,

			///<summary>
			///     OEM 4 �L�[
			///</summary>
			Oem4 = 219,

			///<summary>
			///     �č��W���L�[�{�[�h��� OEM ���p�������L�[
			///</summary>
			OemOpenBrackets = 219,

			///<summary>
			///     �č��W���L�[�{�[�h��� OEM Pipe �L�[
			///</summary>
			OemPipe = 220,

			///<summary>
			///     OEM 5 �L�[
			///</summary>
			Oem5 = 220,

			///<summary>
			///     OEM 6 �L�[
			///</summary>
			Oem6 = 221,

			///<summary>
			///     �č��W���L�[�{�[�h��� OEM �E�p�������L�[
			///</summary>
			OemCloseBrackets = 221,

			///<summary>
			///     OEM 7 �L�[
			///</summary>
			Oem7 = 222,

			///<summary>
			///     �č��W���L�[�{�[�h��� OEM ��d/��d���p���L�[
			///</summary>
			OemQuotes = 222,

			///<summary>
			///     OEM 8 �L�[
			///</summary>
			Oem8 = 223,

			///<summary>
			///     OEM 102 �L�[
			///</summary>
			Oem102 = 226,

			///<summary>
			///     RT 102 �L�[�̃L�[�{�[�h��� OEM �R�������L�[�܂��͉~�L���L�[
			///</summary>
			OemBackslash = 226,

			///<summary>
			///     ProcessKey �L�[
			///</summary>
			ProcessKey = 229,

			///<summary>
			///     Unicode �������L�[�X�g���[�N�ł��邩�̂悤�ɓn����܂��BPacket �̃L�[�l�́A�L�[�{�[�h�ȊO�̓��͎�i�Ɏg�p����� 32 �r�b�g���z�L�[�l�̉��ʃ��[�h�ł��B
			///</summary>
			Packet = 231,

			///<summary>
			///     ATTN �L�[
			///</summary>
			Attn = 246,

			///<summary>
			///     Crsel �L�[
			///</summary>
			Crsel = 247,

			///<summary>
			///     Exsel �L�[
			///</summary>
			Exsel = 248,

			///<summary>
			///     EraseEof �L�[
			///</summary>
			EraseEof = 249,

			///<summary>
			///     PLAY �L�[
			///</summary>
			Play = 250,

			///<summary>
			///     ZOOM �L�[
			///</summary>
			Zoom = 251,

			///<summary>
			///     ����g�p���邽�߂ɗ\�񂳂�Ă���萔
			///</summary>
			NoName = 252,

			///<summary>
			///     PA1 �L�[
			///</summary>
			Pa1 = 253,

			///<summary>
			///     Clear �L�[
			///</summary>
			OemClear = 254,

			///<summary>
			///     �L�[�l����L�[ �R�[�h�𒊏o����r�b�g �}�X�N�B
			///</summary>
			KeyCode = 65535,

			///<summary>
			///     Shift �C���q�L�[
			///</summary>
			Shift = 65536,

			///<summary>
			///     Ctrl �C���q�L�[
			///</summary>
			Control = 131072,

			///<summary>
			///     Alt �C���q�L�[
			///</summary>
			Alt = 262144,



		}

	}
}