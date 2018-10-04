//Midtime Engine
//	�{�\�[�X�R�[�h�̃��C�Z���X�ɂ��Ă͕t���� LICENSE.html �����ǂ݂��������B
//	Please read LICENSE.html for detail license information.

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace MJHSC.MidtimeEngine.GameAPI {

	/// <summary>
	/// �R���g���[����L�[�{�[�h�A�}�E�X����̓��͂��擾���܂��B
	/// </summary>
	public partial class MInput {

		/// <summary>
		/// �uMidtime �R���g���[���̃{�^���v��\���܂��B
		/// </summary>
		public enum ButtonID { //�����@v2�ȍ~: �ڐA���Ƃ̌݊����Ȃ��@����
			/// <summary>
			/// �����E�s���B�ʏ�A�g�p���܂���B
			/// </summary>
			None = 0,

			/// <summary>
			/// A�{�^���B����ȂǂɎg�p���܂��B
			/// </summary>
			A = 1,

			/// <summary>
			/// B�{�^���B�L�����Z���E����ȂǂɎg�p���܂��B
			/// </summary>
			B = 2,

			/// <summary>
			/// X�{�^���B
			/// </summary>
			X = 3,

			/// <summary>
			/// Y�{�^���B
			/// </summary>
			Y = 4,

			/// <summary>
			/// L�{�^���B
			/// </summary>
			L = 5,

			/// <summary>
			/// R�{�^���B
			/// </summary>
			R = 6,

			/// <summary>
			/// L2�{�^���B
			/// </summary>
			L2 = 7,

			/// <summary>
			/// R2�{�^���B
			/// </summary>
			R2 = 8,

			/// <summary>
			/// L3�{�^���BL�X�e�B�b�N���ォ�牟���ƍ쓮���܂��B
			/// </summary>
			L3 = 9,

			/// <summary>
			/// R3�{�^���BR�X�e�B�b�N���ォ�牟���ƍ쓮���܂��B
			/// </summary>
			R3 = 10,

			/// <summary>
			/// �X�^�[�g�{�^���B���j���[�̕\���ȂǂɎg�p���܂��B
			/// </summary>
			Start = 11,

			/// <summary>
			/// �Z���N�g�{�^���B
			/// </summary>
			Select = 12,

			/// <summary>
			/// �\���L�[�́u��v
			/// </summary>
			Up = 13,

			/// <summary>
			/// �\���L�[�́u���v
			/// </summary>
			Down = 14,

			/// <summary>
			/// �\���L�[�́u���v
			/// </summary>
			Left = 15,

			/// <summary>
			/// �\���L�[�́u�E�v
			/// </summary>
			Right = 16,
		}

		/// <summary>
		/// �uMidtime �R���g���[���̃X�e�B�b�N�v��\���܂��B
		/// </summary>
		public enum StickID { //�����@v2�ȍ~: �ڐA���Ƃ̌݊����Ȃ��@����
			/// <summary>
			/// �����E�s���B�ʏ�A�g�p���܂���B
			/// </summary>
			None = 0,

			/// <summary>
			/// �u���X�e�B�b�N�v�̉������̈ړ��B
			/// </summary>
			LX = 1,

			/// <summary>
			/// �u���X�e�B�b�N�v�̏c�����̈ړ��B
			/// </summary>
			LY = 2,

			/// <summary>
			/// �u�E�X�e�B�b�N�v�̉������̈ړ��B
			/// </summary>
			RX = 3,

			/// <summary>
			/// �u�E�X�e�B�b�N�v�̏c�����̈ړ��B
			/// </summary>
			RY = 4,
		}

		/// <summary>
		/// �uMidtime �R���g���[���̃g���K�[���o�[�v��\���܂��B
		/// </summary>
		public enum TriggerID { //�����@v2�ȍ~: �ڐA���Ƃ̌݊����Ȃ��@����
			/// <summary>
			/// �����E�s���B�ʏ�A�g�p���܂���B
			/// </summary>
			None = 0,

			/// <summary>
			/// ���X�e�B�b�N�B
			/// </summary>
			L = 1,

			/// <summary>
			/// �E�X�e�B�b�N�B
			/// </summary>
			R = 2,
		}

		/// <summary>
		/// �u�}�E�X�̃{�^���v��\���܂��B
		/// </summary>
		public enum MouseButtonID { //�����@v2�ȍ~: �ڐA���Ƃ̌݊����Ȃ��@����
			/// <summary>
			/// �����E�s���B�ʏ�A�g�p���܂���B
			/// </summary>
			None = 0,

			/// <summary>
			/// ���{�^���B
			/// </summary>
			L = 1,

			/// <summary>
			/// �E�{�^���B
			/// </summary>
			R = 2,

			/// <summary>
			/// �����̃z�C�[��
			/// </summary>
			Wheel = 3,
		}

		/// <summary>
		/// �u�}�E�X�̈ړ������v��\���܂��B
		/// </summary>
		public enum MouseMoveID { //�����@v2�ȍ~: �ڐA���Ƃ̌݊����Ȃ��@����
			/// <summary>
			/// �����E�s���B�ʏ�A�g�p���܂���B
			/// </summary>
			None = 0,

			/// <summary>
			/// �������̈ړ��B
			/// </summary>
			X = 1,

			/// <summary>
			/// �c�����̈ړ��B
			/// </summary>
			Y = 2,

			/// <summary>
			/// �����̃z�C�[���̉�]�B��������v���X�̐��A���������}�C�i�X�̐��ł��B
			/// </summary>
			Wheel = 3,
		}


	}
}