@ECHO OFF
cd /d %~dp0
echo ��ʼ��װ...
SFBR.PubService.Audio.Host.exe install
ECHO.
ECHO ��װ���������������...
SFBR.PubService.Audio.Host.exe start
ECHO.
ECHO ��װ���
pause