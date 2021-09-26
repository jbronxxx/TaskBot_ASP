using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using ToDoList;

namespace TelegramBot_Buttons
{
    public class Buttons
    {
        private readonly ITelegramBotClient _botClient;

        private const string BUTTONS_START = "/start";
        private const string BUTTONS_MENU = "Список дел";
        private const string BUTTONS_COMMANDS = "Список команд";
        private const string BUTTON_BACK_TO_MAIN_MENU = "Веруться в главное меню";

        private const string BUTTON_ADD_TASK = "Добавить задачу";
        private const string BUTTON_EDIT_TASK = "Редактировать задачу";
        private const string BUTTON_DELETE_TASK = "Удалить задачу";
        
        private const string BUTTON_ADD_TASK_TITLE = "Добавить заголовок";
        private const string BUTTON_ADD_TASK_DISCRIPTION = "Добавить описание";

        public Buttons(ITelegramBotClient botClient)
        {
            _botClient = botClient;
        }

        #region Кастомные кнопки
        // Кнопки главного меню 
        public static async Task<Message> TaskMenuButtons(ITelegramBotClient botClient, Message message)
        {
            var taskMenuKeyboard =  new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton> { new KeyboardButton { Text = "Список дел" }, },
                    new List<KeyboardButton> { new KeyboardButton { Text = "Список команд" }, }
                },

                ResizeKeyboard = true
            };

            return await botClient.SendTextMessageAsync(message.Chat.Id,
                                                        "Сделайте выбор", 
                                                        replyMarkup: taskMenuKeyboard);
        }

        // Кнопки редактирования задачи
        public static async Task<Message> TaskCommandsButtons(ITelegramBotClient botClient, Message message)
        {
            var taskCommandButtons =  new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton>{new KeyboardButton { Text = "Добавить задачу" }, },
                    new List<KeyboardButton>{new KeyboardButton { Text = "Редактировать задачу" }, },
                    new List<KeyboardButton>{new KeyboardButton { Text = "Удалить задачу" }, },
                    new List<KeyboardButton>{new KeyboardButton { Text = "Веруться в главное меню" }, },
                },

                ResizeKeyboard = true
            };

            return await botClient.SendTextMessageAsync(message.Chat.Id,
                                                        "Добавь или отредактируй задачу",
                                                        replyMarkup: taskCommandButtons);
        }

        // Кнопки добавдения Задачи
        public static async Task<Message> AddNewTaskForm(ITelegramBotClient botClient, Message message)
        {
            var addNewTaskFormKeyboard =  new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton>{new KeyboardButton { Text = "Добавить заголовок"}, }
                },
                ResizeKeyboard = true
            };

            return await botClient.SendTextMessageAsync(message.Chat.Id,
                                                        "Добавь или отредактируй задачу",
                                                        replyMarkup: addNewTaskFormKeyboard);
        }

        #endregion

        #region Обработка текстового сообщения
        // Обработка текстового сообщения 
        public async Task BotOnMessageReceived(Message message)
        {
            if (message.Type != MessageType.Text)
                return;

            static async Task<Message> Usage(ITelegramBotClient bot, Message message)
            {
                const string usage = "Выбери команду:\n" +
                                     "/start    - начать\n";

                return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                      text: usage,
                                                      replyMarkup: new ReplyKeyboardRemove());
            }

            var action = message.Text switch
            {
                "/start"                                => TaskMenuButtons(_botClient, message),
                "Список команд"                         => TaskCommandsButtons(_botClient, message),
                "Веруться в главное меню"               => TaskMenuButtons(_botClient, message),
                "Список дел"                            => ToDoListService.ListAllNotes(_botClient, message),
                "Добавить задачу"                       => ToDoListService.CreateListNotes(_botClient, message),
                //"Редактировать задачу" => ,
                //"Удалить задачу" => ,
                //"Добавить заголовок" => ,
                //"Добавить описание" => ,

                "/inline"                               => SendInlineKeyboard(_botClient, message),
                "/remove"                               => RemoveKeyboard(_botClient, message),
                "/photo"                                => SendFile(_botClient, message),
                "/request"                              => RequestContactAndLocation(_botClient, message),
                _                                       => Usage(_botClient, message)
            };

            var sentMessage = await action;

            // Send inline keyboard
            // You can process responses in BotOnCallbackQueryReceived handler
            static async Task<Message> SendInlineKeyboard(ITelegramBotClient bot, Message message)
            {
                await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);

                // Simulate longer running task
                await Task.Delay(500);

                var inlineKeyboard = new InlineKeyboardMarkup(new[]
                {
                    // first row
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("1.1", "11"),
                        InlineKeyboardButton.WithCallbackData("1.2", "12"),
                    },
                    // second row
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("2.1", "21"),
                        InlineKeyboardButton.WithCallbackData("2.2", "22"),
                    },
                });

                return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                      text: "Choose",
                                                      replyMarkup: inlineKeyboard);
            }

            static async Task<Message> RemoveKeyboard(ITelegramBotClient bot, Message message)
            {
                return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                      text: "Removing keyboard",
                                                      replyMarkup: new ReplyKeyboardRemove());
            }

            static async Task<Message> SendFile(ITelegramBotClient bot, Message message)
            {
                await bot.SendChatActionAsync(message.Chat.Id, ChatAction.UploadPhoto);

                const string filePath = @"Files/tux.png";
                using FileStream fileStream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                var fileName = filePath.Split(Path.DirectorySeparatorChar).Last();

                return await bot.SendPhotoAsync(chatId: message.Chat.Id,
                                                photo: new InputOnlineFile(fileStream, fileName),
                                                caption: "Nice Picture");
            }

            static async Task<Message> RequestContactAndLocation(ITelegramBotClient bot, Message message)
            {
                var RequestReplyKeyboard = new ReplyKeyboardMarkup(new[]
                {
                    KeyboardButton.WithRequestLocation("Location"),
                    KeyboardButton.WithRequestContact("Contact"),
                });

                return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                      text: "Who or Where are you?",
                                                      replyMarkup: RequestReplyKeyboard);
            }

            
        }

        #endregion

        #region Обработка данных вызова встроенной клавиатуры
        // Обработка данных вызова встроенной клавиатуры
        public async Task BotOnCallbackQueryReceived(CallbackQuery callbackQuery)
        {
            await _botClient.AnswerCallbackQueryAsync(
                callbackQueryId: callbackQuery.Id,
                text: $"Received {callbackQuery.Data}");

            await _botClient.SendTextMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                text: $"Received {callbackQuery.Data}");
        }

        #endregion

        #region Встроенный режим

        public async Task BotOnInlineQueryReceived(InlineQuery inlineQuery)
        {
            InlineQueryResultBase[] results = {
                // displayed result
                new InlineQueryResultArticle(
                    id: "3",
                    title: "TgBots",
                    inputMessageContent: new InputTextMessageContent(
                        "hello"
                    )
                )
            };

            await _botClient.AnswerInlineQueryAsync(inlineQueryId: inlineQuery.Id,
                                                    results: results,
                                                    isPersonal: true,
                                                    cacheTime: 0);
        }

        public Task BotOnChosenInlineResultReceived(ChosenInlineResult chosenInlineResult)
        {
            return Task.CompletedTask;
        }

        #endregion

        #region Обработка ощибок и неизвестного ввода
        public Task UnknownUpdateHandlerAsync(Update update)
        {
            return Task.CompletedTask;
        }

        public Task HandleErrorAsync(Exception exception)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            return Task.CompletedTask;
        }

        #endregion
    }
}
