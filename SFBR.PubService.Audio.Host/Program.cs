using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace SFBR.PubService.Audio.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                Console.WriteLine(e);
            };
            HostFactory.Run(config =>
            {
                config.Service<MainService>(s =>
                {
                    s.ConstructUsing(() => new MainService());
                    s.WhenStarted(ws => ws.Start());
                    s.WhenStopped(ws => ws.Stop());
                });
                config.SetDescription("四方博瑞设备语音播报服务");
                config.SetDisplayName("四方博瑞设备语音播报服务");
                config.SetServiceName("SFBR.PubService.Audio.Host");
                config.StartAutomatically();
                config.RunAsLocalSystem();
                config.EnableServiceRecovery(reStart =>
                {
                    reStart.RestartService(1);
                    reStart.OnCrashOnly();
                    reStart.SetResetPeriod(1);
                });
            });
        }
    }
}
