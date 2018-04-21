//Midtime Engine
//	�{�\�[�X�R�[�h�̃��C�Z���X�ɂ��Ă͕t���� LICENSE.html �����ǂ݂��������B
//	Please read LICENSE.html for detail license information.

using System;
using System.IO;
using System.Reflection;

namespace MJHSC.MidtimeEngine.Plugins {
	public partial class ErrorWriter {

		public delegate void ErrorOut(string S);

		public static ErrorOut ErrorOutFunction;
		public static void Write(string S){
			if (ErrorOutFunction != null) {
				ErrorOutFunction(S);
			}
		}
	}
}