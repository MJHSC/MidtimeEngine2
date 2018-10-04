Imports System
Imports MJHSC.MidtimeEngine.GameAPI

Namespace MidtimeScript
	Class Startup 
		Shared Sub Main()
			'サウンド読み込み
			Dim BGM As MMedia = MMedia.CreateSound("UCL-GameBGM") 'BGM読み込み
			BGM.PlayLoop() 'BGM再生。1度だけ開始すればいいため、ループ内に入れてはいけない。

			'画像読み込み
			Dim BG As MImage  = MImage.CreateImage("GameBG") '背景画像
			Dim MousePointer As MImage  = MImage.CreateImage("MousePointer") 'マウス追従テストの画像

			Dim CharStop As MImage = MImage.CreateAnime("UCL-UC-Stop", 3, 6, true) '停止中の画像。アニメ
			Dim CharMove As MImage  = MImage.CreateAnime("UCL-UC-Move", 2, 6, true) '移動中の画像。アニメ
			Dim CharJump As MImage  = MImage.CreateImage("UCL-UC-Jump") 'ジャンプ中の画像。静止画

			'ヘルプテキスト表示（文字の表示）
			MImage.SetTextColor(0, 0, 0) '黒色
			MImage.SetTextSize(16) '16pt
			Dim HelpText As MImage  = MImage.CreateText("←→ = 移動、Aボタン (Zキー) = ジャンプ、Bボタン（Xキー） = メニューへ戻る")
			HelpText.SetPosition(160, 505)

			'変数の準備
			Dim CharX As Integer = 20	'キャラクターの初期位置X
			Dim CharY As Integer = 440 'キャラクターの初期位置Y

			Dim LastMoveIsBackward As Boolean = false '後ろに移動したときはtrue、前に移動したときはfalse。
			Dim NowMoving As Boolean = false 'キャラが移動中はtrue、停止中はfalse。
			While MCore.LoopFPS(30) '30FPSのゲームループ

				'●●● 初期化 ●●●
				NowMoving = false '停止状態に戻す。

				'●●● 入力 ●●●
				If (MInput.GetButton(1, MInput.ButtonID.B)) Then '戻る
					MCore.Goto("Startup")
				End If
				If (MInput.GetButton(1, MInput.ButtonID.Left)) Then '左ボタン
					CharX -= 6 '左へ6ピクセル移動
					LastMoveIsBackward = true '後ろ向き
					NowMoving = true '移動中
				End If
				If (MInput.GetButton(1, MInput.ButtonID.Right)) Then '右ボタン
					CharX += 6 '右へ6ピクセル移動
					LastMoveIsBackward = false '前向き
					NowMoving = true '移動中
				End If
				If (MInput.GetButton(1, MInput.ButtonID.A)) Then 'Aボタン: ジャンプ
					CharY -= 6 '上へ6ピクセル移動
				Else
					CharY += 6 '重力: 下へ6ピクセル移動。
				End If



				'●●● 計算 ●●●
				'キャラ位置の制限
				If (CharX <= 0) Then
					CharX = 0 
				End If
				If (CharX >= 920) Then
					CharX = 920 
				End If
				If (CharY <= 0) Then
					CharY = 0 
				End If
				If (CharY >= 440) Then
					CharY = 440 
				End If



				'●●● 表示 ●●●
				'背景表示。スクリプトの指示の順で描かれるので、奥から手前に向かって指示を出していく。
				'背景は一番奥なので最初。
				BG.Draw()

				'キャラ表示
				If (CharY = 440) Then '地上にいる
					If(NowMoving)
						CharMove.InvertHorizontal( LastMoveIsBackward ) '最後の移動が後ろ向きなら画像も反転。
						CharMove.SetPosition(CharX, CharY) '計算したキャラ位置を実際に反映
						CharMove.Draw() '移動中の画像表示
					Else
						CharStop.InvertHorizontal( LastMoveIsBackward ) '最後の移動が後ろ向きなら画像も反転。
						CharStop.SetPosition(CharX, CharY) '計算したキャラ位置を実際に反映
						CharStop.Draw() '停止中の画像表示
					End If
				Else '上空にいる
					CharJump.InvertHorizontal( LastMoveIsBackward ) '最後の移動が後ろ向きなら画像も反転。
					CharJump.SetPosition(CharX, CharY) '計算したキャラ位置を実際に反映
					CharJump.Draw() 'ジャンプ中の画像表示
				End If

				'マウスポインタ表示。 最後に書いているのでもっとも手前であることと、マウス追従のテスト。
				MousePointer.Rotate(-3) '左に3°回転。
				MousePointer.SetPosition( MInput.GetMouse(MInput.MouseMoveID.X), MInput.GetMouse(MInput.MouseMoveID.Y) ) '現在の位置をマウスカーソルの位置へ。
				MousePointer.Draw() '表示

				'ヘルプテキスト表示
				HelpText.Draw()
			End While

		End Sub
	End Class
End Namespace

