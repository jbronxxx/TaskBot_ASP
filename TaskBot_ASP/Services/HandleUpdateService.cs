using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TaskBot_ASP.Services
{
    public class HandleUpdateService
    {
        private readonly ITelegramBotClient _botClient;

        public HandleUpdateService(ITelegramBotClient telegramBotClient)
        {
            _botClient = telegramBotClient;
        }

        public async Task EchoAsync(Update update)
        {
            var me = _botClient.GetMeAsync().Result;

            if (me != null && !string.IsNullOrEmpty(me.Username))
            {
                int offset = 0;

                while (true)
                {
                    try
                    {
                        var updates = _botClient.GetUpdatesAsync(offset).Result;

                        if (updates != null && updates.Count() > 0)
                        {
                            foreach (var item in updates)
                            {

                                offset = update.Id + 1;
                            }
                        }
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }

                    Thread.Sleep(1000);
                }
            }
        }
    }
}