##0 ".\GameData\Scripts\Sample.HSP26.as"

##0 "MidtimeEngine2.HSPv26.hsp2"




#uselib "*m2f*"
#func midtime2function Midtime2FunctionV26 $202
#func m2f Midtime2FunctionV26 $202

##2 ".\GameData\Scripts\Sample.HSP26.as"


true = 1
false = 0


m2f "MMedia.CreateSound", "UCL-GameBGM" 
bgm = stat@hsp
m2f "MMedia.PlayLoop", bgm 


m2f "MImage.CreateImage", "GameBG" 
bg = stat@hsp

m2f "MImage.CreateImage", "MousePointer" 
mousepointer = stat@hsp

m2f "MImage.CreateAnime", "UCL-UC-Stop", 3, 6, true 
charstop = stat@hsp
m2f "MImage.CreateAnime", "UCL-UC-Move", 2, 6, true 
charmove = stat@hsp
m2f "MImage.CreateImage", "UCL-UC-Jump" 
charjump = stat@hsp


m2f "MImage.SetTextColor", 0, 0, 0 
m2f "MImage.SetTextSize", 16 
m2f "MImage.CreateText", "←→ = 移動、Aボタン (Zキー) = ジャンプ、Bボタン（Xキー） = メニューへ戻る"
helptext = stat@hsp
m2f "MImage.SetPosition", helptext ,160, 505


charx = 20	
chary = 440	 

lastmoveisbackward = false 
nowmoving = false 
repeat@hsp
m2f "MCore.LoopFPS", 30 


nowmoving = false 


m2f "MInput.GetButton", 1, "B"  
if@hsp (stat@hsp) {
m2f "MCore.Goto", "Startup"
}
m2f "MInput.GetButton", 1, "Left"  
if@hsp (stat@hsp) {
charx = charx - 6 
lastmoveisbackward = true 
nowmoving = true 
}
m2f "MInput.GetButton", 1, "Right"  
if@hsp (stat@hsp) {
charx = charx + 6 
lastmoveisbackward = false 
nowmoving = true 
}
m2f "MInput.GetButton", 1, "A"  
if@hsp (stat@hsp) {
chary = chary - 6 
} else@hsp {
chary = chary + 6 
}





if@hsp (charx <= 0) {
charx = 0
}
if@hsp (charx >= 920) {
charx = 920
}
if@hsp (chary <= 0) {
chary = 0
}
if@hsp (chary >= 440) {
chary = 440
}






m2f "MImage.Draw", bg


if@hsp ( chary == 440) { 
if@hsp (nowmoving) {
m2f "MImage.InvertHorizontal", charmove, lastmoveisbackward  
m2f "MImage.SetPosition", charmove, charx, chary 
m2f "MImage.Draw", charmove 
} else@hsp {
m2f "MImage.InvertHorizontal", charstop, lastmoveisbackward  
m2f "MImage.SetPosition", charstop, charx, chary 
m2f "MImage.Draw", charstop 
}
} else@hsp { 
m2f "MImage.InvertHorizontal", charjump, lastmoveisbackward  
m2f "MImage.SetPosition", charjump, charx, chary 
m2f "MImage.Draw", charjump 
}


m2f "MImage.Rotate", mousepointer, -3 

m2f "MInput.GetMouse", "X"
mx = stat@hsp
m2f "MInput.GetMouse", "Y"
my = stat@hsp
m2f "MImage.SetPosition", mousepointer, mx, my  

m2f "MImage.Draw", mousepointer 


m2f "MImage.Draw", helptext
loop@hsp


