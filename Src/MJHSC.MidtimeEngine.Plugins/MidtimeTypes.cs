//Midtime Engine
//	�{�\�[�X�R�[�h�̃��C�Z���X�ɂ��Ă͕t���� LICENSE.html �����ǂ݂��������B
//	Please read LICENSE.html for detail license information.


//Midtime Engine�ƃv���O�C�������ʂŎg�p����^�̒�`
using System;

namespace MJHSC.MidtimeEngine.Plugins {

	public enum MidtimeResponse {
		Error = 0, 
		OK = 1,
	}
	
	public delegate MidtimeResponse LanguageVMEntry(string ScriptFilePath);

}

