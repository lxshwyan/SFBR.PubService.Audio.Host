using SFBR.PubService.Audio.Host.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SFBR.PubService.Audio.Host.Device
{
    public class SpeakerConstructor : ISpeakerConstructor
    {
        static string speechModel = ConfigurationManager.AppSettings["SpeechMode"].ToString();

        public ISpeaker CreateSpeakerInstance()
        {
            string strNamespace = Assembly.GetExecutingAssembly().GetName().Name;
            string fullName = $"{strNamespace}.Device.{speechModel}";
            return (ISpeaker)Assembly.Load(strNamespace).CreateInstance(fullName);
        }
    }
}
