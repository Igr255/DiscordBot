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
        BotMain potkan = new BotMain();
        public void Test1()
        {
            potkan.SetStartingVoiceChannelID(26);
            potkan.SetVoiceChannelCount(3);
            potkan.SetWalkSequence(false, 1000);
            potkan.SetFollowSequence(true);

            potkan.LogIn();
            potkan.SwitchChannel("FI Hell");
        }

    }
}
