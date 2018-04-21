

//サウンド読み込み
var BGM = MMedia.CreateSound("UCL-GameBGM"); //BGM読み込み
BGM.PlayLoop(); //BGM再生。1度だけ開始すればいいため、ループ内に入れてはいけない。

//画像読み込み
var BG = MImage.CreateImage("GameBG"); //背景画像
var MousePointer = MImage.CreateImage("MousePointer"); //マウス追従テストの画像

var CharStop = MImage.CreateAnime("UCL-UC-Stop", 3, 6, true); //停止中の画像。アニメ
var CharMove = MImage.CreateAnime("UCL-UC-Move", 2, 6, true); //移動中の画像。アニメ
var CharJump = MImage.CreateImage("UCL-UC-Jump"); //ジャンプ中の画像。静止画

			//ヘルプテキスト表示（文字の表示）
MImage.SetTextColor(0, 0, 0); //黒色
MImage.SetTextSize(16); //16pt
var HelpText = MImage.CreateText("←→ = 移動、Aボタン (Zキー) = ジャンプ、Bボタン（Xキー） = メニューへ戻る");
HelpText.SetPosition(160, 505);

//変数の準備
var CharX = 20;	//キャラクターの初期位置X
var CharY = 440; //キャラクターの初期位置Y

var LastMoveIsBackward = false; //後ろに移動したときはtrue、前に移動したときはfalse。
var NowMoving = false; //キャラが移動中はtrue、停止中はfalse。
while(MCore.LoopFPS(30)){ //30FPSのゲームループ

	//●●● 初期化 ●●●
	NowMoving = false; //停止状態に戻す。

	//●●● 入力 ●●●
	if(MInput.GetButton(1, MInput.ButtonID.B)){ //戻る
		MCore.Goto("Startup");
	};
	if(MInput.GetButton(1, MInput.ButtonID.Left)){ //左ボタン
		CharX -= 6; //左へ6ピクセル移動
		LastMoveIsBackward = true; //後ろ向き
		NowMoving = true; //移動中
	}
	if(MInput.GetButton(1, MInput.ButtonID.Right)){ //右ボタン
		CharX += 6; //右へ6ピクセル移動
		LastMoveIsBackward = false; //前向き
		NowMoving = true; //移動中
	}
	if(MInput.GetButton(1, MInput.ButtonID.A)){ //Aボタン: ジャンプ
		CharY -= 6; //上へ6ピクセル移動
	} else {
		CharY += 6; //重力: 下へ6ピクセル移動。
	}



	//●●● 計算 ●●●
	//キャラ位置の制限
	if(CharX <= 0){ CharX = 0; }
	if(CharX >= 920){ CharX = 920; }
	if(CharY <= 0){ CharY = 0; }
	if(CharY >= 440){ CharY = 440; }



	//●●● 表示 ●●●
	//背景表示。スクリプトの指示の順で描かれるので、奥から手前に向かって指示を出していく。
	//背景は一番奥なので最初。
	BG.Draw();

	//キャラ表示
	if( CharY == 440){ //地上にいる
		if(NowMoving){
			CharMove.InvertHorizontal( LastMoveIsBackward ); //最後の移動が後ろ向きなら画像も反転。
			CharMove.SetPosition(CharX, CharY); //計算したキャラ位置を実際に反映
			CharMove.Draw(); //移動中の画像表示
		} else {
			CharStop.InvertHorizontal( LastMoveIsBackward ); //最後の移動が後ろ向きなら画像も反転。
			CharStop.SetPosition(CharX, CharY); //計算したキャラ位置を実際に反映
			CharStop.Draw(); //停止中の画像表示
		}
	} else { //上空にいる
		CharJump.InvertHorizontal( LastMoveIsBackward ); //最後の移動が後ろ向きなら画像も反転。
		CharJump.SetPosition(CharX, CharY); //計算したキャラ位置を実際に反映
		CharJump.Draw(); //ジャンプ中の画像表示
	}

	//マウスポインタ表示。 最後に書いているのでもっとも手前であることと、マウス追従のテスト。
	MousePointer.Rotate(-3); //左に3°回転。
	MousePointer.SetPosition( MInput.GetMouse(MInput.MouseMoveID.X), MInput.GetMouse(MInput.MouseMoveID.Y) ); //現在の位置をマウスカーソルの位置へ。
	MousePointer.Draw(); //表示

	//ヘルプテキスト表示
	HelpText.Draw();

}
