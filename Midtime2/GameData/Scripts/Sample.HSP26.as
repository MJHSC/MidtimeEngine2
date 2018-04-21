
#include "MidtimeEngine2.HSPv26.hsp2"

//��`
true = 1
false = 0

//�T�E���h�ǂݍ���
m2f "MMedia.CreateSound", "UCL-GameBGM" //BGM�ǂݍ���
BGM = stat
m2f "MMedia.PlayLoop", BGM //BGM�Đ��B1�x�����J�n����΂������߁A���[�v���ɓ���Ă͂����Ȃ��B

//�摜�ǂݍ���
m2f "MImage.CreateImage", "GameBG" //�w�i�摜
BG = stat

m2f "MImage.CreateImage", "MousePointer" //�}�E�X�Ǐ]�e�X�g�̉摜
MousePointer = stat

m2f "MImage.CreateAnime", "UCL-UC-Stop", 3, 6, true //��~���̉摜�B�A�j��
CharStop = stat
m2f "MImage.CreateAnime", "UCL-UC-Move", 2, 6, true //�ړ����̉摜�B�A�j��
CharMove = stat
m2f "MImage.CreateImage", "UCL-UC-Jump" //�W�����v���̉摜�B�Î~��
CharJump = stat

			//�w���v�e�L�X�g�\���i�����̕\���j
m2f "MImage.SetTextColor", 0, 0, 0 //���F
m2f "MImage.SetTextSize", 16 //16pt
m2f "MImage.CreateText", "���� = �ړ��AA�{�^�� (Z�L�[) = �W�����v�AB�{�^���iX�L�[�j = ���j���[�֖߂�"
HelpText = stat
m2f "MImage.SetPosition", HelpText ,160, 505

//�ϐ��̏���
CharX = 20	//�L�����N�^�[�̏����ʒuX
CharY = 440	 //�L�����N�^�[�̏����ʒuY

LastMoveIsBackward = false //���Ɉړ������Ƃ���true�A�O�Ɉړ������Ƃ���false�B
NowMoving = false //�L�������ړ�����true�A��~����false�B
repeat
m2f "MCore.LoopFPS", 30 //30FPS�̃Q�[�����[�v

	//������ ������ ������
	NowMoving = false //��~��Ԃɖ߂��B

	//������ ���� ������
	m2f "MInput.GetButton", 1, "B"  //�߂�
	if (stat) {
		m2f "MCore.Goto", "Startup"
	}
	m2f "MInput.GetButton", 1, "Left"  //���{�^��
	if (stat) {
		CharX = CharX - 6 //����6�s�N�Z���ړ�
		LastMoveIsBackward = true //������
		NowMoving = true //�ړ���
	}
	m2f "MInput.GetButton", 1, "Right"  //�E�{�^��
	if (stat) {
		CharX = CharX + 6 //�E��6�s�N�Z���ړ�
		LastMoveIsBackward = false //�O����
		NowMoving = true //�ړ���
	}
	m2f "MInput.GetButton", 1, "A"  //A�{�^��: �W�����v
	if (stat) {
		CharY = CharY - 6 //���6�s�N�Z���ړ�
	} else {
		CharY = CharY + 6 //�d��: ����6�s�N�Z���ړ��B
	}



	//������ �v�Z ������
	//�L�����ʒu�̐���
	if (CharX <= 0) {
		CharX = 0
	}
	if (CharX >= 920) {
		CharX = 920
	}
	if (CharY <= 0) {
		CharY = 0
	}
	if (CharY >= 440) {
		CharY = 440
	}



	//������ �\�� ������
	//�w�i�\���B�X�N���v�g�̎w���̏��ŕ`�����̂ŁA�������O�Ɍ������Ďw�����o���Ă����B
	//�w�i�͈�ԉ��Ȃ̂ōŏ��B
	m2f "MImage.Draw", BG

	//�L�����\��
	if ( CharY == 440) { //�n��ɂ���
		if (NowMoving) {
			m2f "MImage.InvertHorizontal", CharMove, LastMoveIsBackward  //�Ō�̈ړ����������Ȃ�摜�����]�B
			m2f "MImage.SetPosition", CharMove, CharX, CharY //�v�Z�����L�����ʒu�����ۂɔ��f
			m2f "MImage.Draw", CharMove //�ړ����̉摜�\��
		} else {
			m2f "MImage.InvertHorizontal", CharStop, LastMoveIsBackward  //�Ō�̈ړ����������Ȃ�摜�����]�B
			m2f "MImage.SetPosition", CharStop, CharX, CharY //�v�Z�����L�����ʒu�����ۂɔ��f
			m2f "MImage.Draw", CharStop //��~���̉摜�\��
		}
	} else { //���ɂ���
		m2f "MImage.InvertHorizontal", CharJump, LastMoveIsBackward  //�Ō�̈ړ����������Ȃ�摜�����]�B
		m2f "MImage.SetPosition", CharJump, CharX, CharY //�v�Z�����L�����ʒu�����ۂɔ��f
		m2f "MImage.Draw", CharJump //�W�����v���̉摜�\��
	}

	//�}�E�X�|�C���^�\���B �Ō�ɏ����Ă���̂ł����Ƃ���O�ł��邱�ƂƁA�}�E�X�Ǐ]�̃e�X�g�B
	m2f "MImage.Rotate", MousePointer, -3 //����3����]�B

	m2f "MInput.GetMouse", "X"
	mx = stat
	m2f "MInput.GetMouse", "Y"
	my = stat
	m2f "MImage.SetPosition", MousePointer, mx, my  //���݂̈ʒu���}�E�X�J�[�\���̈ʒu�ցB

	m2f "MImage.Draw", MousePointer //�\��

	//�w���v�e�L�X�g�\��
	m2f "MImage.Draw", HelpText
loop


