using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskBot.Buttons;
using TaskBot_ASP.UserStatement;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using ToDoList;

namespace TaskBot_ASP
{
    public class ButtonsTreatment
    {
        private static Dictionary<long, UserState> _clientState = new Dictionary<long, UserState>();
        private Buttons _buttons;
        private TelegramBotClient _botClient;
        private ToDoListController _toDoList;

        public ButtonsTreatment(TelegramBotClient telegramBotClient)
        {
            _buttons = new Buttons();
            _botClient = telegramBotClient;
            _toDoList = new ToDoListController();
        }
        public async Task ProcessUpdate(Update update)
        {
            switch (update.Type)
            {
                case UpdateType.Message:

                    var text = update.Message.Text;

                    switch (text)
                    {
                        case "/start":
                            _clientState[update.Message.Chat.Id] =
                                new UserState { State = State.MainMenu };
                            var resultStart = _botClient.SendTextMessageAsync(update.Message.Chat.Id, "Сделайте выбор",
                                replyMarkup: _buttons.TaskMenuButtons()).Result;
                            break;

                        case "Список команд":
                            _clientState[update.Message.Chat.Id] =
                                new UserState { State = State.TaskList };
                            var resultCommandList = _botClient.SendTextMessageAsync(update.Message.Chat.Id, "Выберите команду",
                                replyMarkup: _buttons.TaskCommandsButtons()).Result;
                            break;

                        case "Веруться в предыдущее меню":
                            _clientState[update.Message.Chat.Id] =
                                new UserState { State = State.MainMenu };
                            var resultBackToMain = _botClient.SendTextMessageAsync(update.Message.Chat.Id, "Сделайте выбор",
                                replyMarkup: _buttons.TaskMenuButtons()).Result;
                            break;

                        case "Добавить задачу":
                            _clientState[update.Message.Chat.Id] =
                                new UserState { State = State.TaskList };
                            var resultAddTask = _botClient.SendTextMessageAsync(update.Message.Chat.Id, "Введите название задачи",
                                replyMarkup: _buttons.AddNewTaskForm()).Result;
                            break;

                        case "Добавить заголовок":
                            _clientState[update.Message.Chat.Id] =
                                new UserState { State = State.NewTask };
                            var resultNewTask = _botClient.SendTextMessageAsync(update.Message.Chat.Id, "Заголовок сохранен",
                                replyMarkup: _buttons.AddNewTaskForm()).Result;
                            if (update.Message.Text != null)
                            {
                                try
                                {
                                    _toDoList.Create(update.Message.Chat.Id, update.Message.Text.ToString());
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                            break;

                        case "Добавить описание":
                            _clientState[update.Message.Chat.Id] =
                                new UserState { State = State.NewTask };
                            var resultNewTaskDisp = _botClient.SendTextMessageAsync(update.Message.Chat.Id, "Описание сохранено",
                                replyMarkup: _buttons.AddNewTaskForm()).Result;
                            break;

                        default:
                            await _botClient.SendTextMessageAsync(update.Message.Chat.Id, "нет подходящих вариантов");
                            break;
                    }
                    break;

                default:
                    Console.WriteLine(update.Type + " не найдено");
                    break;
            }

        }
    }
}
