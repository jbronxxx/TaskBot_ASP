using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot_Buttons;

namespace TaskBot_ASP.Services 
{
    public class HandleUpdateService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly Buttons _buttons;

        public HandleUpdateService(ITelegramBotClient botClient)
        {
            _buttons = new Buttons(botClient);
            _botClient = botClient;
        }

        public async Task EchoAsync(Update update)
        {
            var handler = update.Type switch
            {
                UpdateType.Message                  => _buttons.BotOnMessageReceived(update.Message),
                UpdateType.EditedMessage            => _buttons.BotOnMessageReceived(update.EditedMessage),
                UpdateType.CallbackQuery            => _buttons.BotOnCallbackQueryReceived(update.CallbackQuery),
                UpdateType.InlineQuery              => _buttons.BotOnInlineQueryReceived(update.InlineQuery),
                UpdateType.ChosenInlineResult       => _buttons.BotOnChosenInlineResultReceived(update.ChosenInlineResult),
                _                                   => _buttons.UnknownUpdateHandlerAsync(update)
            };

            try
            {
                await handler;
            }
            catch (Exception exception)
            {
                await _buttons.HandleErrorAsync(exception);
            }
        }
    }
}