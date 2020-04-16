using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace TestHoge
{
    class Program
    {
        private DiscordSocketClient _client;
        public static CommandService _commands;
        public static IServiceProvider _services;

        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info
            });
            _client.Log += Log;
            _commands = new CommandService();
            _services = new ServiceCollection().BuildServiceProvider();
            _client.MessageReceived += CommandRecieved;
            ///DiscordTokenを指定する。
            Console.Write("Please enter token : ");
            string token = Console.ReadLine();
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        /// <summary>
        /// メッセージの受信
        /// </summary>
        /// <returns></returns>
        private async Task CommandRecieved(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;

            //デバッグ用メッセージを出力
            Console.WriteLine("{0} {1}:{2}", message.Channel.Name, message.Author.Username, message);
            //メッセージがnullの場合
            if (message == null)
                return;

            //発言者がBotの場合無視する
            if (message.Author.IsBot)
                return;


            var context = new CommandContext(_client, message);

            //ここから記述--------------------------------------------------------------------------
            var CommandContext = message.Content;

            // コマンド("ping")かどうか判定
            if (0 <= CommandContext.IndexOf("ping"))
            {
                await message.Channel.SendMessageAsync("pong");
            }


        }

        private Task Log(LogMessage message)
        {
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }
    }
}