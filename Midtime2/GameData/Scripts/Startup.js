
//背景画像読み込み
var BG = MImage.CreateImage("BG");

//Unity-chan: UCL ライセンス画像読み込み
var UCL = MImage.CreateImage("UCL-LicenseLogo");
UCL.SetPosition( 960 - 140 - 20, 540 - 120 - 20 );

//メニューポインタ読み込み
var MenuPointer = MImage.CreateImage("MenuPointer");


//タイトルテキスト作成
MImage.SetTextColor(0, 0, 0);
MImage.SetTextSize(24);
var TitleText = MImage.CreateText("MidtimeEngine v2 Beta1 スクリプトサンプル");
TitleText.SetPosition(10, 10);

//メニューテキスト作成
MImage.SetTextSize(16);
var MenuText = [
	{File: "Sample.NDP", Image: MImage.CreateText("サンプル .Net バイナリ")}, 
	{File: "Sample.CSharp", Image: MImage.CreateText("サンプル C#")}, 
	{File: "Sample.VisualBasic", Image: MImage.CreateText("サンプル VisualBasic")}, 
	{File: "Sample.JavaScript", Image: MImage.CreateText("サンプル JavaScript")}, 
//	{File: "Sample.VBScript", Image: MImage.CreateText("サンプル VBScript")}, 
	{File: "Sample.Python", Image: MImage.CreateText("サンプル Python")}, 
	{File: "Sample.Ruby", Image: MImage.CreateText("サンプル Ruby")}, 
//	{File: "Sample.SmallBasic", Image: MImage.CreateText("サンプル SmallBasic")}, 
//	{File: "Sample.Native", Image: MImage.CreateText("サンプル ネイティブ DLL")}, 
	{File: "Sample.Lua53", Image: MImage.CreateText("サンプル Lua 5.3")}, 
	{File: "Sample.Lua52", Image: MImage.CreateText("サンプル Lua 5.2")}, 
	{File: "Sample.Lua51_JIT", Image: MImage.CreateText("サンプル Lua 5.1 (LuaJIT)")}, 
	{File: "Sample.HSP3x", Image: MImage.CreateText("サンプル HSP 3.4")}, 
	{File: "Sample.HSP26", Image: MImage.CreateText("サンプル HSP 2.6")}, 
];

//メニューの表示位置設定
var MenuX = 100;
var MenuY = 75;
for(var i=0; i<MenuText.length; i++){
	MenuText[i].Image.SetPosition(MenuX, MenuY);
	MenuY += 32;
}


//ヘルプテキスト
var HelpText = MImage.CreateText("Aボタン / Zキー: 決定、十字キー: 選択");
HelpText.SetPosition(360, 500);


//サウンド読み込み
var BGM = MMedia.CreateSound("UCL-TitleBGM");
var MoveSE = MMedia.CreateSound("UCL-Move");
var OKSE = MMedia.CreateSound("UCL-OK");
BGM.PlayLoop();

//メニューの位置
var MenuPosition = 0;

//ボタンの連続押し防止 → 今後実装予定の「HighlevelAPI」にて対応。（他にもメッセージウィンドウやメニューなどよくゲームで使われる機能を搭載）
var LastButton = null;

//ゲームループ
while(MCore.LoopFPS(30)){ //30FPS

	//入力処理: メニュー
	if(MInput.GetButton(1, MInput.ButtonID.Up)){ //上ボタン
		if(LastButton != MInput.ButtonID.Up){
			LastButton = MInput.ButtonID.Up; //連続押し防止機能設定
			MoveSE.Stop(); //前のSEがまだなっているなら停止
			MoveSE.PlayOnce(); //SE再生
			MenuPosition--; //メニューポインタを下げる
		}
	} else {
		if( LastButton == MInput.ButtonID.Up ){ //連続押し防止機能解除
			LastButton = null;
		}
	}
	if(MInput.GetButton(1, MInput.ButtonID.Down)){ //下ボタン
		if(LastButton != MInput.ButtonID.Down){
			LastButton = MInput.ButtonID.Down; //連続押し防止機能設定
			MoveSE.Stop(); //前のSEがまだなっているなら停止
			MoveSE.PlayOnce(); //SE再生
			MenuPosition++; //メニューポインタを上げる
		}
	} else {
		if( LastButton == MInput.ButtonID.Down ){ //連続押し防止機能解除
			LastButton = null;
		}
	}
	if(MInput.GetButton(1, MInput.ButtonID.A)){ //Aボタン
		if(LastButton != MInput.ButtonID.A){
			LastButton = MInput.ButtonID.A; //連続押し防止機能設定
			OKSE.Stop(); //前のSEがまだなっているなら停止
			OKSE.PlayOnce(); //SE再生
			MCore.Sleep(500); //SE再生待ち
			//選択されたスクリプトを起動
			var ScriptName = MenuText[MenuPosition].File;
			MCore.Goto(ScriptName); //MCore.Gotoを実行すると、直ちに現在のスクリプトが停止され、新しいスクリプトを実行する。

		}
	} else {
		if( LastButton == MInput.ButtonID.A ){ //連続押し防止機能解除
			LastButton = null;
		}
	}
	//メニューポインターの位置を制限
	if(MenuPosition < 0){MenuPosition = 0;}
	if(MenuText.length-1 < MenuPosition){MenuPosition = MenuText.length-1;}


	//背景の表示
	BG.Draw();

	//タイトルテキストの表示
	TitleText.Draw();

	//メニューテキストの表示
	for(var i=0; i<MenuText.length; i++){
		MenuText[i].Image.Draw();
	}

	//メニューポインタの表示
	MenuPointer.SetPosition(50, 75 + (MenuPosition * 32));
	MenuPointer.Draw();

	//ヘルプテキストの表示
	HelpText.Draw();

	//Unity-chan: UCL ライセンス画像の表示
	UCL.Draw();

}

