@echo off
"robocopy" /r:0 /mir /xj "%~dp0\MJHSC.MidtimeEngine.Resources\Binaries" "%~dp0\..\Midtime2\MidtimeEngine\Binaries"
exit /b 0

