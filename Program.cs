using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;


namespace MeuBot
{
    class Program
    {
        // Program entry point
        static void Main(string[] args)
        {
            // Call the Program constructor, followed by the 
            // MainAsync method and wait until it finishes (which should be never).
            //DiscordBot discordBot = new DiscordBot();
            new DiscordBot().MainAsync().GetAwaiter().GetResult();
        }
    }
}
