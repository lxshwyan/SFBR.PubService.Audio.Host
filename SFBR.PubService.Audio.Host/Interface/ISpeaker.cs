using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFBR.PubService.Audio.Host.Interface
{
    public interface ISpeaker
    {
        byte[] SpeakContentStream(string content);
    }
}
