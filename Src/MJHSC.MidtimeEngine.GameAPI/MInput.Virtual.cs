//Midtime Engine
//	�{�\�[�X�R�[�h�̃��C�Z���X�ɂ��Ă͕t���� LICENSE.html �����ǂ݂��������B
//	Please read LICENSE.html for detail license information.

using System;
using System.IO;
using System.Windows.Forms;

namespace MJHSC.MidtimeEngine.GameAPI {
	public partial class MInput {

		#region �t���[������
		internal static void Process() {
			CheckGamePads();
		}
		#endregion

		#region Midtime ���z�R���g���[��
		internal static bool[][] MVCButton;
		internal static int[][] MVCStick;
		internal static int[][] MVCTrigger;
		internal static int[][] LastPadButton; //�R���g���[�����g�p���Ă��Ȃ����ɁA�L�[�{�[�h�̓��͂��㏑�����Ȃ��悤�ɂ���B�uInput.X360.cs/ApplyGamePadButton()�v�Ŏg�p�B

		internal static void Initialize_VController() {
			int MaxControllers = Enum.GetNames(typeof(Microsoft.Xna.Framework.PlayerIndex)).Length + 1; //0�͎g�p���Ȃ�

			MVCButton = new bool[MaxControllers][];
			MVCStick = new int[MaxControllers][];
			MVCTrigger = new int[MaxControllers][];

			LastPadButton = new int[MaxControllers][];

			for (int i = 0; i < MaxControllers; i++) {
				MVCButton[i] = new bool[Enum.GetNames(typeof(ButtonID)).Length];
				MVCStick[i] = new int[Enum.GetNames(typeof(StickID)).Length];
				MVCTrigger[i] = new int[Enum.GetNames(typeof(TriggerID)).Length];
				LastPadButton[i] = new int[Enum.GetNames(typeof(ButtonID)).Length];
			}
		}

		/// <summary>
		/// �w�肳�ꂽ�v���C���[�̃R���g���[���̃{�^���̏�Ԃ𒲂ׂ܂��B
		/// </summary>
		/// <param name="ControllerID">�R���g���[���̔ԍ��i1�`4�j</param>
		/// <param name="StickID">���ׂ����{�^���́uButtonID�v</param>
		/// <returns>������Ă���ꍇ�Atrue�܂���1�B
		/// ������Ă��Ȃ��ꍇ�Afalse�܂���0�B/// </returns>
		public static bool GetButton(int ControllerID, ButtonID ButtonID) {
			if (ControllerID < 1 || MVCStick.Length <= ControllerID) { MDebug.WriteFormat("���݂��Ȃ��R���g���[���ԍ��u{0}�v���w�肳��܂���", ControllerID); return false; }
			try {
				return MVCButton[ControllerID][(int)ButtonID];
			} catch { MDebug.WriteFormat("���݂��Ȃ��R���g���[���{�^���u{0}�v���w�肳��܂���", ButtonID); }
			return false;
		}

		/// <summary>
		/// �w�肳�ꂽ�v���C���[�̃R���g���[���̃X�e�B�b�N�̏�Ԃ𒲂ׂ܂��B
		/// </summary>
		/// <param name="ControllerID">�R���g���[���̔ԍ��i1�`4�j</param>
		/// <param name="StickID">���ׂ����X�e�B�b�N�́uTriggerID�v</param>
		/// <returns>�X�e�B�b�N�̌X���B�i-127�`127�j</returns>
		public static int GetStick(int ControllerID, StickID StickID) {
			if (ControllerID < 1 || MVCStick.Length <= ControllerID) { MDebug.WriteFormat("���݂��Ȃ��R���g���[���ԍ��u{0}�v���w�肳��܂���", ControllerID); return 0; }
			try {
				return MVCStick[ControllerID][(int)StickID];
			} catch { MDebug.WriteFormat("���݂��Ȃ��X�e�B�b�N�u{0}�v���w�肳��܂���", StickID); }
			return 0;
		}

		/// <summary>
		/// �w�肳�ꂽ�v���C���[�̃R���g���[���̃g���K�[�̏�Ԃ𒲂ׂ܂��B
		/// </summary>
		/// <param name="ControllerID">�R���g���[���̔ԍ��i1�`4�j</param>
		/// <param name="TriggerID">���ׂ����g���K�[�́uTriggerID�v</param>
		/// <returns>�g���K�[�̉������܂ꂽ�ʁB�i0�`255�j</returns>
		public static int GetTrigger(int ControllerID, TriggerID TriggerID) {
			if (ControllerID < 1 || MVCTrigger.Length <= ControllerID) { MDebug.WriteFormat("���݂��Ȃ��R���g���[���ԍ��u{0}�v���w�肳��܂���", ControllerID); return 0; }
			try {
				return MVCTrigger[ControllerID][(int)TriggerID];
			} catch { MDebug.WriteFormat("���݂��Ȃ��g���K�[�u{0}�v���w�肳��܂���", TriggerID); }
			return 0;
		}

		#endregion
		
		#region �����R���g���[�� (DirectInput) 
		//�� Midtime v2�`�uMicrosoft.DirectX.DirectInput.dll�v�� �W����.NET Framework 4�����Ȃ��� (Windows 8.1�A10)�œǂݍ��߂Ȃ��̂ň�U�폜�B �������@������΍Ď����B
		//����ɐV�����K�i�uXInput�v�������B�uInput.X360.cs�v�BDirectInput�̑Ή��͓����B
		#endregion

		#region �����}�E�X
		internal static int MouseX = 0; //Set by mouse handler
		internal static int MouseY = 0; //Set by mouse handler
		internal static int MouseW = 0; //Set by mouse handler

		/// <summary>
		/// �w�肳�ꂽ�����̃}�E�X�̈ʒu�𒲂ׂ܂��B
		/// </summary>
		/// <param name="MouseMoveID">���ׂ���������\���uMouseMoveID�v</param>
		/// <returns>�}�E�X�̈ʒu���W�i��ʍ��オ0�j</returns>
		public static int GetMouse(MouseMoveID MouseMoveID) {
			if (MouseMoveID == MouseMoveID.X) { return MouseX; }
			if (MouseMoveID == MouseMoveID.Y) { return MouseY; }
			if (MouseMoveID == MouseMoveID.Wheel) { return MouseW; }
			MDebug.WriteFormat("���݂��Ȃ��}�E�X�ʒu�u{0}�v���w�肳��܂���", MouseMoveID);
			return 0;
		}
		#endregion

		#region �����L�[�{�[�h
		internal static bool[] KeyData = new bool[256];
		internal static KeyID LastKey; //set by key handler

		/// <summary>
		/// �Ō�ɉ����ꂽ�L�[�{�[�h�̃{�^���𒲂ׂ܂��B
		/// 
		/// ���݂�������Ă��邩�A���łɗ�����Ă��邩���͊֌W����܂���B
		/// �������͂��󂯂�Ƃ��Ȃǂɕ֗��ł��B
		/// </summary>
		/// <returns>�Ō�ɉ����ꂽ�{�^���́uKeyID�v</returns>
		public static KeyID GetLastKey() {
			KeyID KID = (KeyID)LastKey;
			LastKey = 0;
			return KID;
		}

		/// <summary>
		/// �w�肳�ꂽ�L�[�{�[�h�̃{�^�������݉�����Ă��邩���m�F���܂��B
		/// </summary>
		/// <param name="KeyID">���ׂ����{�^���́uKeyID�v</param>
		/// <returns>������Ă���ꍇ�Atrue�܂���1�B
		/// ������Ă��Ȃ��ꍇ�Afalse�܂���0�B/// </returns>
		public static bool GetKey(KeyID KeyID) {
			try {
				return KeyData[(int)KeyID];
			} catch { MDebug.WriteFormat("���݂��Ȃ��L�[�u{0}�v���w�肳��܂���", KeyID); }
			return false;
		}
		#endregion

	}
}