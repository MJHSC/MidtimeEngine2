//Midtime Engine
//	�{�\�[�X�R�[�h�̃��C�Z���X�ɂ��Ă͕t���� LICENSE.html �����ǂ݂��������B
//	Please read LICENSE.html for detail license information.

using System;
using System.IO;
using System.Windows.Forms;

namespace MJHSC.MidtimeEngine.GameAPI {
	public partial class MInput {

		internal static ButtonID[] KB2VConvertTable = new ButtonID[Enum.GetNames(typeof(KeyID)).Length];
		internal static ButtonID[] KB2VConvertTable_Default = new ButtonID[Enum.GetNames(typeof(KeyID)).Length];

		#region Midtime ���z�R���g���[���̃L�[�R���t�B�O�T�|�[�g
		internal static void Initialize_KB2VConverter() {
			KB2VConvertTable_Default[(int)KeyID.Z] = ButtonID.A;
			KB2VConvertTable_Default[(int)KeyID.X] = ButtonID.B;
			KB2VConvertTable_Default[(int)KeyID.C] = ButtonID.X;
			KB2VConvertTable_Default[(int)KeyID.V] = ButtonID.Y;
			KB2VConvertTable_Default[(int)KeyID.A] = ButtonID.L;
			KB2VConvertTable_Default[(int)KeyID.S] = ButtonID.R;
			KB2VConvertTable_Default[(int)KeyID.None] = ButtonID.L2;
			KB2VConvertTable_Default[(int)KeyID.None] = ButtonID.R2;
			KB2VConvertTable_Default[(int)KeyID.None] = ButtonID.L3;
			KB2VConvertTable_Default[(int)KeyID.None] = ButtonID.R3;
			KB2VConvertTable_Default[(int)KeyID.Enter] = ButtonID.Start;
			KB2VConvertTable_Default[(int)KeyID.Back] = ButtonID.Select;
			KB2VConvertTable_Default[(int)KeyID.Up] = ButtonID.Up;
			KB2VConvertTable_Default[(int)KeyID.Down] = ButtonID.Down;
			KB2VConvertTable_Default[(int)KeyID.Left] = ButtonID.Left;
			KB2VConvertTable_Default[(int)KeyID.Right] = ButtonID.Right;
			KB2VConvertTable_Default.CopyTo(KB2VConvertTable, 0); ;
		}
		#endregion

	}
}