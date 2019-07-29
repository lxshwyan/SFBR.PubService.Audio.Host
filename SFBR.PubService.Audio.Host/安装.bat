@ECHO OFF
cd /d %~dp0
echo 开始安装...
SFBR.PubService.Audio.Host.exe install
ECHO.
ECHO 安装完成正在启动服务...
SFBR.PubService.Audio.Host.exe start
ECHO.
ECHO 安装完成
pause