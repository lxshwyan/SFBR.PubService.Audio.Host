using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace SFBR.PubService.Audio.Host
{
    class MainService
    {


       private static readonly string path = System.Configuration.ConfigurationManager.AppSettings["apiPath"] ?? "/tool/voice";
        private static readonly string url = System.Configuration.ConfigurationManager.AppSettings["Url"] ?? "http://+:7000";
        IDisposable server = null;

        public void Start()
        {
            server = WebApp.Start<Startup>(url);
            Console.WriteLine("接口地址:{0}?context=内容", url.Replace("+", "*") + path);
        }
        public void Stop()
        {
            if (server != null)
            {
                server.Dispose();
            }

        }


    }
}
