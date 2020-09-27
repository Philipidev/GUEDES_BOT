using Discord;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeuBot
{
    public class BotActivity : IActivity
    {
        public string Name => "!help";

        public ActivityType Type => ActivityType.Playing;

        public ActivityProperties Flags => ActivityProperties.None;

        public string Details => null;
    }
}
