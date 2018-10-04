@echo off
setlocal
pushd "%~dp0"

taskkill /im SB.exe /f
set SBMODE=%ErrorLevel%

set NAME=MJHSC.MidtimeEngine.Languages.SmallBasic.GameAPI
set OUT=%NAME%.dll
set R=/r:..\..\..\MJHSC.MidtimeEngine.Resources\Binaries\SmallBasicLibrary.dll 
"%~1\csc.exe" /nologo /target:library /win32res:..\..\..\MJHSC.MidtimeEngine.Resources\Win32\MJHSC.res /doc:"%NAME%.xml" %R% /out:%OUT% *.cs

copy /y "%OUT%" "..\..\..\..\Midtime2\MidtimeEngine\Binaries\"

set SBPATH=..\..\..\..\SrcPrivate\SmallBasicIDE
IF EXIST "%SBPATH%" (

	copy /y "%OUT%" "%SBPATH%\Lib\"
	copy /y "%NAME%.xml" "%SBPATH%\Lib\"

	IF EXIST "..\..\..\..\Midtime2\GameData\Scripts\%OUT%" copy /y "%OUT%"  "..\..\..\..\Midtime2\GameData\Scripts\%OUT%" 
)

popd
endlocal
