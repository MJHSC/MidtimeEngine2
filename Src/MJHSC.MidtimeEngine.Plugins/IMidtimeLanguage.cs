//Midtime Engine
//	�{�\�[�X�R�[�h�̃��C�Z���X�ɂ��Ă͕t���� LICENSE.html �����ǂ݂��������B
//	Please read LICENSE.html for detail license information.


namespace MJHSC.MidtimeEngine.Plugins {

	/// <summary>
	/// Midtime�ƌ���v���O�C���Ƃ̐ڑ�
	/// </summary>
	public interface IMidtimeLanguage {

		/// <summary>
		/// �w�肳�ꂽ�X�N���v�g�����̌���v���O�C���ŏ����\�����m�F���A���̌��ʂ�ԋp���܂��B
		/// �����̏ꍇ�AMidtimeResponse.OK�����X�N���v�g�͎��̎菇�ł��̌���v���O�C�����������邱�ƂɂȂ�܂��B
		/// �ł������g���q�̈�v�����łȂ��A���ۂɏ����\�Ȃ��̂��m�F���Ă��������B
		/// </summary>
		/// <param name="ScriptFileName">�e�X�g����X�N���v�g�t�@�C���̃p�X�i�g���q�Ȃ��j</param>
		/// <returns>�����\�ȏꍇ: MidtimeResponse.OK
		/// �����ł��Ȃ��ꍇ: MidtimeResponse.Error</returns>
		MidtimeResponse CanRunScript(string ScriptFileName);

		/// <summary>
		/// �V�����X�N���v�g�����[�h�����Ƃ��ɌĂяo����܂��B
		/// �K�v�ɉ����ĐV�����C���X�^���X���쐬���A���ۂɃX�N���v�g�����s����֐���ԋp���܂��B
		/// Midtime Engine�͂����ŕԋp���ꂽ�֐����X�N���v�g���s�X���b�h�Ŏ��s���܂��B
		/// </summary>
		/// <param name="ScriptFileName"></param>
		/// <returns></returns>
		LanguageVMEntry StartVM(string ScriptFileName);

		/// <summary>
		/// ���̃V�[���ɐ؂�ւ�钼�O�ɌĂяo����܂��B���\�[�X�̊J���ȂǂɎg�p���܂��B
		/// ������Sleep��UI�̕\���Ȃǒx�����������铮����s���Ƌ����I�������ꍇ������̂ŁA
		/// �K�v�ȉ�������݂̂��s���A����return���Ă��������B
		/// </summary>
		/// <param name="ScriptFileName">���s���̃X�N���v�g�t�@�C���̃p�X�i�g���q�Ȃ��j</param>
		/// <returns></returns>
		MidtimeResponse EndVM(string ScriptFileName);
		
	}
	
}