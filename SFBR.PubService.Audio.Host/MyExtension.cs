using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFBR.PubService.Audio.Host
{
    public static class MyExtension
    {
        public static IAppBuilder UseAudio(this IAppBuilder builder)
        {
            return builder.Use<Middleware.AudioMiddleware>();
        }
    }
}
