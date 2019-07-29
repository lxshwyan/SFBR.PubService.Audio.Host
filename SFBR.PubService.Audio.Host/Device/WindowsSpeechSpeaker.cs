using SFBR.PubService.Audio.Host.Helper;
using SFBR.PubService.Audio.Host.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace SFBR.PubService.Audio.Host.Device
{
    class WindowsSpeechSpeaker : ISpeaker
    {
        public byte[] SpeakContentStream(string content)
        {
            byte[] buffer = CacheHelper.Get(content) as byte[];
            if (buffer != null)
            {
                
                return buffer;
            }
            else
            {
                using (System.Speech.Synthesis.SpeechSynthesizer speechSyn = new System.Speech.Synthesis.SpeechSynthesizer())
                {
                    speechSyn.Volume = 100;
                    speechSyn.Rate = 0;
                    using (var ms = new System.IO.MemoryStream())
                    {
                        speechSyn.SetOutputToWaveStream(ms);
                        speechSyn.Speak(content);
                        speechSyn.SetOutputToNull();
                        buffer = ms.ToArray();
                        //设置平滑过期
                        CacheHelper.Set(content,buffer,TimeSpan.FromSeconds(30));
                        return buffer;
                    }
                }
            }
        }
    }
}
