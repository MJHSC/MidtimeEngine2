@echo off

set SBPATH=%~dp0\..\..\..\..\SrcPrivate\SmallBasicIDE
pushd "%SBPATH%"
start "" SB.exe ..\..\Midtime2\GameData\Scripts\Startup.sb
popd
	)
