@ECHO OFF
cd /d %~dp0
echo 正在停止服务...
SFBR.PubService.Audio.Host.exe stop
echo.
echo 服务停止开始卸载...
SFBR.PubService.Audio.Host.exe uninstall
echo.
ECHO 卸载完成
pause