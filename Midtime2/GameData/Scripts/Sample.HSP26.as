
#include "MidtimeEngine2.HSPv26.hsp2"

//定義
true = 1
false = 0

//サウンド読み込み
m2f "MMedia.CreateSound", "UCL-GameBGM" //BGM読み込み
BGM = stat
m2f "MMedia.PlayLoop", BGM //BGM再生。1度だけ開始すればいいため、ループ内に入れてはいけない。

//画像読み込み
m2f "MImage.CreateImage", "GameBG" //背景画像
BG = stat

m2f "MImage.CreateImage", "MousePointer" //マウス追従テストの画像
MousePointer = stat

m2f "MImage.CreateAnime", "UCL-UC-Stop", 3, 6, true //停止中の画像。アニメ
CharStop = stat
m2f "MImage.CreateAnime", "UCL-UC-Move", 2, 6, true //移動中の画像。アニメ
CharMove = stat
m2f "MImage.CreateImage", "UCL-UC-Jump" //ジャンプ中の画像。静止画
CharJump = stat

			//ヘルプテキスト表示（文字の表示）
m2f "MImage.SetTextColor", 0, 0, 0 //黒色
m2f "MImage.SetTextSize", 16 //16pt
m2f "MImage.CreateText", "←→ = 移動、Aボタン (Zキー) = ジャンプ、Bボタン（Xキー） = メニューへ戻る"
HelpText = stat
m2f "MImage.SetPosition", HelpText ,160, 505

//変数の準備
CharX = 20	//キャラクターの初期位置X
CharY = 440	 //キャラクターの初期位置Y

LastMoveIsBackward = false //後ろに移動したときはtrue、前に移動したときはfalse。
NowMoving = false //キャラが移動中はtrue、停止中はfalse。
repeat
m2f "MCore.LoopFPS", 30 //30FPSのゲームループ

	//●●● 初期化 ●●●
	NowMoving = false //停止状態に戻す。

	//●●● 入力 ●●●
	m2f "MInput.GetButton", 1, "B"  //戻る
	if (stat) {
		m2f "MCore.Goto", "Startup"
	}
	m2f "MInput.GetButton", 1, "Left"  //左ボタン
	if (stat) {
		CharX = CharX - 6 //左へ6ピクセル移動
		LastMoveIsBackward = true //後ろ向き
		NowMoving = true //移動中
	}
	m2f "MInput.GetButton", 1, "Right"  //右ボタン
	if (stat) {
		CharX = CharX + 6 //右へ6ピクセル移動
		LastMoveIsBackward = false //前向き
		NowMoving = true //移動中
	}
	m2f "MInput.GetButton", 1, "A"  //Aボタン: ジャンプ
	if (stat) {
		CharY = CharY - 6 //上へ6ピクセル移動
	} else {
		CharY = CharY + 6 //重力: 下へ6ピクセル移動。
	}



	//●●● 計算 ●●●
	//キャラ位置の制限
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



	//●●● 表示 ●●●
	//背景表示。スクリプトの指示の順で描かれるので、奥から手前に向かって指示を出していく。
	//背景は一番奥なので最初。
	m2f "MImage.Draw", BG

	//キャラ表示
	if ( CharY == 440) { //地上にいる
		if (NowMoving) {
			m2f "MImage.InvertHorizontal", CharMove, LastMoveIsBackward  //最後の移動が後ろ向きなら画像も反転。
			m2f "MImage.SetPosition", CharMove, CharX, CharY //計算したキャラ位置を実際に反映
			m2f "MImage.Draw", CharMove //移動中の画像表示
		} else {
			m2f "MImage.InvertHorizontal", CharStop, LastMoveIsBackward  //最後の移動が後ろ向きなら画像も反転。
			m2f "MImage.SetPosition", CharStop, CharX, CharY //計算したキャラ位置を実際に反映
			m2f "MImage.Draw", CharStop //停止中の画像表示
		}
	} else { //上空にいる
		m2f "MImage.InvertHorizontal", CharJump, LastMoveIsBackward  //最後の移動が後ろ向きなら画像も反転。
		m2f "MImage.SetPosition", CharJump, CharX, CharY //計算したキャラ位置を実際に反映
		m2f "MImage.Draw", CharJump //ジャンプ中の画像表示
	}

	//マウスポインタ表示。 最後に書いているのでもっとも手前であることと、マウス追従のテスト。
	m2f "MImage.Rotate", MousePointer, -3 //左に3°回転。

	m2f "MInput.GetMouse", "X"
	mx = stat
	m2f "MInput.GetMouse", "Y"
	my = stat
	m2f "MImage.SetPosition", MousePointer, mx, my  //現在の位置をマウスカーソルの位置へ。

	m2f "MImage.Draw", MousePointer //表示

	//ヘルプテキスト表示
	m2f "MImage.Draw", HelpText
loop


