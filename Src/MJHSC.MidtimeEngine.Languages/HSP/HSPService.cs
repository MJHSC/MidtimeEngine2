//Midtime Engine
//	�{�\�[�X�R�[�h�̃��C�Z���X�ɂ��Ă͕t���� LICENSE.html �����ǂ݂��������B
//	Please read LICENSE.html for detail license information.

using System;
using System.Runtime.InteropServices;
using MJHSC.MidtimeEngine.GameAPI;
using MJHSC.MidtimeEngine.Languages.HSP.HSPPlugin;

namespace MJHSC.MidtimeEngine.Languages.HSP {

	public interface IHSPAccess { 
		int GetNextArgAsInt();
		string GetNextArgAsString();
		bool GetNextArgAsBoolean();

		bool SetReturnValue(int Value);
		bool SetReturnValue(string Value);
		bool SetReturnValue(bool Value);
	}


	//v26, v3x�œ���Proxy dll�Ȃ̂ł����Œ�`���Ă��܂��B
	//v4�ȍ~��Proxy dll���������ꍇ��Lua�Ɠ����悤�ɂ��ꂼ���Hosting (���ۂ̖��O��axv**)�Ɏ�������
	public class HSPAccessAll : IHSPAccess { 

		public string GetNextArgAsString() {
			return HSPProxy.GetNextArgAsString();
		}

		public int GetNextArgAsInt() {
			return HSPProxy.GetNextArgAsInt();
		}

		public bool GetNextArgAsBoolean() {
			return ((HSPProxy.GetNextArgAsInt()) != 0);
		}

		public bool SetReturnValue(int R) {
			HSPProxy.SetReturnValue(R);
			return true;
		}

		public bool SetReturnValue(bool R) {
			if (R) {
				HSPProxy.SetReturnValue(1);
			} else {
				HSPProxy.SetReturnValue(0);
			}
			return true;
		}

		public bool SetReturnValue(string R) {
			HSPProxy.SetReturnValue(R);
			return true;
		}

	}

}
