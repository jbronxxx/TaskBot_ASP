using System;
using TaskBot_ASP;
using Telegram.Bot;

namespace TaskBot_Logger
{
    class Program
    {
        static void Main(string[] args)
        {
            BotConfiguration _botConfig = new BotConfiguration();
            TelegramBotClient botClient = new TelegramBotClient(_botConfig.BotToken);
            var bot = botClient.GetUpdatesAsync();

            Console.WriteLine(bot);
        }
    }
}
