@ECHO OFF
cd /d %~dp0
echo ����ֹͣ����...
SFBR.PubService.Audio.Host.exe stop
echo.
echo ����ֹͣ��ʼж��...
SFBR.PubService.Audio.Host.exe uninstall
echo.
ECHO ж�����
pause