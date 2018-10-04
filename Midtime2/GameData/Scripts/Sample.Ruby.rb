

#サウンド読み込み
bgm = MMedia::CreateSound("UCL-GameBGM") #BGM読み込み
bgm::PlayLoop() #BGM再生。1度だけ開始すればいいため、ループ内に入れてはいけない。

#画像読み込み
bg = MImage::CreateImage("GameBG") #背景画像
mousePointer = MImage::CreateImage("mousePointer") #マウス追従テストの画像

charStop = MImage::CreateAnime("UCL-UC-Stop", 3, 6, true) #停止中の画像。アニメ
charMove = MImage::CreateAnime("UCL-UC-Move", 2, 6, true) #移動中の画像。アニメ
charJump = MImage::CreateImage("UCL-UC-Jump") #ジャンプ中の画像。静止画

#ヘルプテキスト表示（文字の表示）
MImage::SetTextColor(0, 0, 0) #黒色
MImage::SetTextSize(16) #16pt
helpText = MImage::CreateText("←→ = 移動、Aボタン (Zキー) = ジャンプ、Bボタン（Xキー） = メニューへ戻る")
helpText::SetPosition(160, 505)

#変数の準備
charX = 20 #キャラクターの初期位置X
charY = 440 #キャラクターの初期位置Y

lastMoveIsBackward = false #後ろに移動したときはtrue、前に移動したときはfalse。
nowMoving = false #キャラが移動中はtrue、停止中はfalse。
while MCore::LoopFPS(30) #30FPSのゲームループ

	#●●● 初期化 ●●●
	nowMoving = false #停止状態に戻す。

	#●●● 入力 ●●●
	if MInput::GetButton(1, MInput::ButtonID.B) #戻る
		MCore::Goto("Startup")
	end
	if MInput::GetButton(1, MInput::ButtonID.Left) #左ボタン
		charX -= 6 #左へ6ピクセル移動
		lastMoveIsBackward = true #後ろ向き
		nowMoving = true #移動中
	end
	if MInput::GetButton(1, MInput::ButtonID.Right) #右ボタン
		charX += 6 #右へ6ピクセル移動
		lastMoveIsBackward = false #前向き
		nowMoving = true #移動中
	end
	if MInput::GetButton(1, MInput::ButtonID.A) #Aボタン: ジャンプ
		charY -= 6 #上へ6ピクセル移動
	else
		charY += 6 #重力: 下へ6ピクセル移動。
	end
end



	#●●● 計算 ●●●
	#キャラ位置の制限
	if charX <= 0
		charX = 0
	end
	if charX >= 920
		charX = 920
	end
	if charY <= 0
		charY = 0
	end
	if charY >= 440
		charY = 440
	end



	#●●● 表示 ●●●
	#背景表示。スクリプトの指示の順で描かれるので、奥から手前に向かって指示を出していく。
	#背景は一番奥なので最初。
	bg::Draw()

	#キャラ表示
	if charY == 440 #地上にいる
		if nowMoving 
			charMove::InvertHorizontal( lastMoveIsBackward ) #最後の移動が後ろ向きなら画像も反転。
			charMove::SetPosition(charX, charY) #計算したキャラ位置を実際に反映
			charMove::Draw() #移動中の画像表示
		else
			charStop::InvertHorizontal( lastMoveIsBackward ) #最後の移動が後ろ向きなら画像も反転。
			charStop::SetPosition(charX, charY) #計算したキャラ位置を実際に反映
			charStop::Draw() #停止中の画像表示
		end
	else #上空にいる
		charJump::InvertHorizontal( lastMoveIsBackward ) #最後の移動が後ろ向きなら画像も反転。
		charJump::SetPosition(charX, charY) #計算したキャラ位置を実際に反映
		charJump::Draw() #ジャンプ中の画像表示
	end

	#マウスポインタ表示。 最後に書いているのでもっとも手前であることと、マウス追従のテスト。
	mousePointer::Rotate(-3) #左に3°回転。
	mousePointer::SetPosition( MInput::GetMouse(MInput::MouseMoveID.X), MInput::GetMouse(MInput::MouseMoveID.Y) ) #現在の位置をマウスカーソルの位置へ。
	mousePointer::Draw() #表示

	#ヘルプテキスト表示
	helpText::Draw()

