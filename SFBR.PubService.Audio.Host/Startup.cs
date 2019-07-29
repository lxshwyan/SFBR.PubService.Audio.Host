using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net.Http.Formatting;
using Microsoft.Owin;
using Swashbuckle.Application;
using System.IO;

namespace SFBR.PubService.Audio.Host
{
    internal class Startup
    {

        private readonly static ObjectCache cacheVoices = MemoryCache.Default;
        private readonly static object lockobj = new object();
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            var options = new Microsoft.Owin.StaticFiles.FileServerOptions
            {
                EnableDefaultFiles = true
            };
            options.DefaultFilesOptions.DefaultFileNames = new List<string> { "index.html" };
            app.UseFileServer(options);
            app.Use(HealthCheck);
            app.UseAudio();
        }
        private static async Task HealthCheck(IOwinContext context, Func<Task> next)
        {
            if ("/healthcheck".Equals(context.Request.Path.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                //TODO:可以添加数据库，缓存等各种资源的检查
                context.Response.StatusCode = 204;
                await context.Response.WriteAsync("");
                return;
            }
            await next();
        }
    }

}
