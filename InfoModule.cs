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
using System.Threading;

namespace MeuBot
{
	// Create a module with no prefix
	public class InfoModule : ModuleBase<SocketCommandContext>
	{
		[Command("help")]
		public Task help()
		{
			EmbedBuilder builder = new EmbedBuilder()
				.WithTitle("COMANDOS DO GUEDESSS")
				.WithDescription("[̲̅$̲̅(̲̅ιοο̲̅)̲̅$̲̅] [̲̅$̲̅(̲̅ιοο̲̅)̲̅$̲̅] [̲̅$̲̅(̲̅ιοο̲̅)̲̅$̲̅] \n-----------------------------------")
				.WithUrl("https://discordapp.com")
				.WithColor(new Color(0xFF0E))
				.WithFooter(footer => {
					footer
						.WithText("by Paulinho (lolo) Guedes")
						.WithIconUrl("https://cdn.discordapp.com/app-icons/532756946013650956/757144fce8ff04689923934efb68fda4.png");
				})
				.WithThumbnailUrl("https://cdn.discordapp.com/app-icons/532756946013650956/757144fce8ff04689923934efb68fda4.png")
				.WithAuthor(author => {
					author
						.WithName("Paulinho (lolo) Guedes")
						.WithUrl("https://muquiranas.com/wp-content/uploads/2019/03/DlZUNuGXgAAKk_g-696x391.jpg")
						.WithIconUrl("https://cdn.discordapp.com/app-icons/532756946013650956/757144fce8ff04689923934efb68fda4.png");
				})
				.AddField("!help", "Lista de comandos\n\n")
				.AddField("!fale", "!fale <texto> paulo guedes vai dizer algo\n\n")
				.AddField("!cotacao", "!cotacao <moeda> paulo guedes vai falar a cotacão\n");

			var embed = builder.Build();
			return ReplyAsync(null, false, embed);

			//DeleteMessage();
			//string echo = "*!help*\n*!fale* _<mensagem>_\n*!cotacao* _<moeda>_ EX: !cotacao usd\n*!som*";
			//var methodsNames = typeof(InfoModule).GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Select(method => "`!" + method.Name + "`");
			//var asd = methodsNames.GetMethods(BindingFlags.Public);//.Select(method => '!' + method.Name);

			//IEnumerable<EmbedFieldBuilder> EnumEmbedFieldBuilder = methodsNames.Select(n => new EmbedFieldBuilder()
			//{
			//	Name = n,
			//	Value = "--------------------------------------------\n",
			//	IsInline = false,
			//});

			//EmbedFooterBuilder EmbedFooterBuilder = new EmbedFooterBuilder()
			//{
			//	Text = "PAULOOOO GUEDESSS",
			//};

			//EmbedAuthorBuilder EmbedAuthorBuilder = new EmbedAuthorBuilder()
			//{
			//	Name = "Paulo Guedes",
			//};

			//EmbedBuilder embedBuilder = new EmbedBuilder()
			//{
			//	Title = "Comandos",
			//	Fields = EnumEmbedFieldBuilder.ToList(),
			//	Description = "Lista dos comandos",
			//	Color = Color.DarkGreen,
			//	Footer = EmbedFooterBuilder,
			//	Author = EmbedAuthorBuilder,
			//	ThumbnailUrl = "https://cdn.discordapp.com/app-icons/532756946013650956/757144fce8ff04689923934efb68fda4.png?size=256",
			//};			

			//return ReplyAsync(null, false, embedBuilder.Build());
		}

		[Command("fale")]
		[Summary("Echoes a message.")]
		public Task fale([Remainder][Summary("The text to echo")] string echo)
        {
			DeleteMessage();
			if (echo.ToLower().Trim().Contains("sou gay") || echo.ToLower().Trim().Contains("eu sou gay") || echo.ToLower().Trim().Contains("é gay") || echo.ToLower().Trim().Contains("e gay"))
				return ReplyAsync("<@" + Context.User.Id + "> tu gosta de dar o cu, não é?");
			else
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

		//[Command("ativar")]
		//public Task ativar()
		//{
		//	HttpClient httpClient = new HttpClient();
		//	try
		//	{
		//		while (true) 
		//		{
		//			HttpResponseMessage HttpResponseMessage = httpClient.GetAsync("https://economia.awesomeapi.com.br/usd").GetAwaiter().GetResult();

		//			string ResponseBody = HttpResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();
		//			dynamic json = JsonConvert.DeserializeObject<dynamic>(ResponseBody);

		//			string alta = json[0].high;
		//			string baixa = json[0].low;

		//			string echo = $"Alta: {alta}\nBaixa: {baixa}";
		//			ReplyAsync(echo);
		//			Task.Delay(3000);
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		return ReplyAsync("Moeda inválida");
		//	}
		//}

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
			string pathSite = "https://www.youtube.com/watch?v=IpMLYWb1G1w";

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
}
