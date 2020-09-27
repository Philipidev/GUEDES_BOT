using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using Discord;
using System.Diagnostics;
using Discord.Audio;

namespace MeuBot
{
	// Create a module with no prefix
	public class InfoModule : ModuleBase<SocketCommandContext>
	{
		[Command("help")]
		public Task help()
		{
			//DeleteMessage();
			//string echo = "*!help*\n*!fale* _<mensagem>_\n*!cotacao* _<moeda>_ EX: !cotacao usd\n*!som*";
			var methodsNames = typeof(InfoModule).GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Select(method => "`!" + method.Name + "`");
			//var asd = methodsNames.GetMethods(BindingFlags.Public);//.Select(method => '!' + method.Name);

			IEnumerable<EmbedFieldBuilder> EnumEmbedFieldBuilder = methodsNames.Select(n => new EmbedFieldBuilder()
			{
				Name = n,
				Value = "--------------------------------------------\n",
				IsInline = false,
			});
			
			EmbedFooterBuilder EmbedFooterBuilder = new EmbedFooterBuilder()
			{
				Text = "PAULOOOO GUEDESSS",
			};

			EmbedAuthorBuilder EmbedAuthorBuilder = new EmbedAuthorBuilder()
			{
				Name = "Paulo Guedes",
			};

			EmbedBuilder embedBuilder = new EmbedBuilder()
			{
				Title = "Comandos",
				Fields = EnumEmbedFieldBuilder.ToList(),
				Description = "Lista dos comandos",
				Color = Color.DarkGreen,
				Footer = EmbedFooterBuilder,
				Author = EmbedAuthorBuilder,
				ThumbnailUrl = "https://cdn.discordapp.com/app-icons/532756946013650956/757144fce8ff04689923934efb68fda4.png?size=256",
			};			
			
			return ReplyAsync(null, false, embedBuilder.Build());
		}

		[Command("fale")]
		[Summary("Echoes a message.")]
		public Task fale([Remainder][Summary("The text to echo")] string echo)
        {
			DeleteMessage();
			return ReplyAsync(echo);
		}
		// ReplyAsync is a method on ModuleBase 

		[Command("cotacao")]
		public Task cotacao([Remainder]string moeda)
		{
			try
			{
				HttpClient httpClient = new HttpClient();
				HttpResponseMessage HttpResponseMessage = httpClient.GetAsync("https://economia.awesomeapi.com.br/" + moeda.ToUpper()).GetAwaiter().GetResult();

				string ResponseBody = HttpResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();
				dynamic json = JsonConvert.DeserializeObject<dynamic>(ResponseBody);

				string alta = json[0].high;
				string baixa = json[0].low;

				string echo = $"Alta: {alta}\nBaixa: {baixa}";

				return ReplyAsync(echo);
			}
			catch(Exception ex)
            {
				return ReplyAsync("Moeda inválida");
			}
		}

		// The command's Run Mode MUST be set to RunMode.Async, otherwise, being connected to a voice channel will block the gateway thread.
		[Command("som", RunMode = RunMode.Async)]
		public async Task som()
		{
			// Get the audio channel
			IVoiceChannel channel = (Context.User as IGuildUser)?.VoiceChannel;
			if (channel == null) { await Context.Channel.SendMessageAsync("User must be in a voice channel, or a voice channel must be passed as an argument."); return; }

			// For the next step with transmitting audio, you would want to pass this Audio Client in to a service.
			var audioClient = await channel.ConnectAsync();

			string path = "C:\\Users\\phili\\Music\\bateu.mp3";

			var processo = Process.Start(new ProcessStartInfo
			{
				FileName = "ffmpeg",
				Arguments = $"-hide_banner -loglevel panic -i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
				UseShellExecute = false,
				RedirectStandardOutput = true,
			});

			// Create FFmpeg using the previous example
			using (var ffmpeg = processo)
			using (var output = ffmpeg.StandardOutput.BaseStream)
			//120, 240, 480, 960, 1920, or 2880
			using (var discord = audioClient.CreatePCMStream(AudioApplication.Mixed))
			{
				try { await output.CopyToAsync(discord); }
				finally { await discord.FlushAsync(); }
			}

		}

		private void DeleteMessage()
        {
			Context.Message.DeleteAsync().GetAwaiter().GetResult();
		}
	}

	// Create a module with the 'sample' prefix
	[Group("sample")]
	public class SampleModule : ModuleBase<SocketCommandContext>
	{
		// ~sample square 20 -> 400
		[Command("square")]
		[Summary("Squares a number.")]
		public async Task SquareAsync([Summary("The number to square.")] int num)
		{
			// We can also access the channel from the Command Context.
			await Context.Channel.SendMessageAsync($"{num}^2 = {Math.Pow(num, 2)}");
		}

		// ~sample userinfo --> foxbot#0282
		// ~sample userinfo @Khionu --> Khionu#8708
		// ~sample userinfo Khionu#8708 --> Khionu#8708
		// ~sample userinfo Khionu --> Khionu#8708
		// ~sample userinfo 96642168176807936 --> Khionu#8708
		// ~sample whois 96642168176807936 --> Khionu#8708
		[Command("userinfo")]
		[Summary("Returns info about the current user, or the user parameter, if one passed.")]
		[Alias("user", "whois")]
		public async Task UserInfoAsync(
			[Summary("The (optional) user to get info from")]
		SocketUser user = null)
		{
			var userInfo = user ?? Context.Client.CurrentUser;
			await ReplyAsync($"{userInfo.Username}#{userInfo.Discriminator}");
		}
	}
}
