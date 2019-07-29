using Microsoft.Owin;
using SFBR.PubService.Audio.Host.Device;
using System;
using System.Threading.Tasks;

namespace SFBR.PubService.Audio.Host.Middleware
{
    public class AudioMiddleware : OwinMiddleware
    {
       private static readonly string path = System.Configuration.ConfigurationManager.AppSettings["apiPath"] ?? "/tool/voice";
        public AudioMiddleware(OwinMiddleware next) : base(next) { }

        public override async Task Invoke(IOwinContext context)
        {
            if (Next != null && context.Request.Path.Value.StartsWith(path, StringComparison.InvariantCultureIgnoreCase))
            {
                string method = context.Request.Method;
                string content = string.Empty;
                if (method.Equals("POST",StringComparison.InvariantCultureIgnoreCase))
                {
                    var getDicTask = Helper.GetBodyParameterHelper.GetBodyParameters(context.Request);
                    var parameterDic = getDicTask.Result;
                    content = parameterDic["content"];
                }
                else if (method.Equals("GET", StringComparison.InvariantCultureIgnoreCase))
                {
                    content = context.Request.Query["content"];
                }
                if (!string.IsNullOrEmpty(content))
                {
                    try
                    {
                        SpeakerConstructor speakerConstructor = new SpeakerConstructor();
                        var speaker = speakerConstructor.CreateSpeakerInstance();
                        //必须进行缓存，一次警报推送会有很多警报客户端发起请求
                        var buffer = speaker.SpeakContentStream(content);
                        context.Response.ContentType = "audio/mpeg";
                        context.Response.ContentLength = buffer.Length;
                        await context.Response.WriteAsync(buffer);
                        return;
                    }
                    catch
                    {
                    }
                    finally { await Next.Invoke(context); }
                }
            }
        }





    }
}
