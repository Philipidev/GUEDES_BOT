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

namespace MeuBot
{
	// Create a module with no prefix
	public class InfoModule : ModuleBase<SocketCommandContext>
	{
		// ~say hello world -> hello world
		[Command("fale")]
		[Summary("Echoes a message.")]
		public Task SayAsync([Remainder][Summary("The text to echo")] string echo)
			=> ReplyAsync(echo);
		// ReplyAsync is a method on ModuleBase 

		[Command("cotacao")]
		[Summary("Echoes a message.")]
		public Task AtivarDolar([Remainder]string moeda)
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
	}

	// Create a module with the 'sample' prefix
	[Group("sample")]
	public class SampleModule : ModuleBase<SocketCommandContext>
	{
		// ~sample square 20 -> 400
		[Command("square")]
		[Summary("Squares a number.")]
		public async Task SquareAsync(
			[Summary("The number to square.")]
		int num)
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
		[Summary
		("Returns info about the current user, or the user parameter, if one passed.")]
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
