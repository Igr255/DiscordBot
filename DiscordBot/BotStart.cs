using DiscordBot.BotFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    class BotStart
    {
        private string serverName;

        public void SetServerName(string name){ serverName = name; }
        
        public void Test1()
        {
            //Utils.potkan.SetStartingVoiceChannelID(26);
            //Utils.potkan.SetVoiceChannelCount(3);
            //Utils.potkan.SetWalkSequence(true, 2000);
            //Utils.potkan.SetFollowSequence(true);

            Utils.potkan.LogIn();//TODO osetrit if empty
            Utils.potkan.SwitchChannel(serverName);
        }

    }
}
