//Midtime Engine
//	�{�\�[�X�R�[�h�̃��C�Z���X�ɂ��Ă͕t���� LICENSE.html �����ǂ݂��������B
//	Please read LICENSE.html for detail license information.

using System;
namespace MJHSC.MidtimeEngine.Plugins {


	/// <summary>
	/// .NET �Ή�����p�̊֐��v���O�C��
	/// �u�����v�u�Ԓl�v�Ƃ��ɁA�S�Ă̌^���g�p�ł��܂��B
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class MidtimeFunction2Attribute : Attribute {
		public string Name {
			get;
			private set;
		}
		public string Author = "";

		public MidtimeFunction2Attribute() {
			this.Name = "";
		}
		public MidtimeFunction2Attribute(string Name) {
			this.Name = Name;
		}
	}


	/// <summary>
	/// .NET "��"�Ή�����p�̊֐��v���O�C��
	/// �u�����v�u�Ԓl�v�Ƃ��ɂ��ꂼ��ustring�v�uint�v���g�p����K�v������܂��B
	/// </summary>
	/// [
	[Obsolete("���̃^�C�v�̃v���O�C���͐�������܂���B�܂��A���̃^�C�v�̊֐��v���O�C���ł́u�����v�u�Ԓl�v�Ƃ��ɂ��ꂼ��ustring�v�uint�v���g�p����K�v������܂��B")]
	public class MidtimeFunction2LegacyAttribute : Attribute {
		public string Name {
			get;
			private set;
		}
		public string Author = "";

		public MidtimeFunction2LegacyAttribute() {
			this.Name = "";
		}
		public MidtimeFunction2LegacyAttribute(string Name) {
			this.Name = Name;
		}
	}

}