using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace TaskBot.Buttons
{
    class Buttons
    {
        private const string BUTTONS_START = "/start";
        private const string BUTTONS_MENU = "Список дел";
        private const string BUTTONS_COMMANDS = "Список команд";
        private const string BUTTON_BACK_TO_MAIN_MENU = "Веруться в предыдущее меню";

        private const string BUTTON_ADD_TASK = "Добавить задачу";
        private const string BUTTON_DELETE_TASK = "Удалить задачу";
        private const string BUTTON_EDIT_TASK = "Редактировать задачу";

        private const string BUTTON_ADD_TASK_TITLE = "Добавить заголовок";
        private const string BUTTON_ADD_TASK_DISCRIPTION = "Добавить описание";

        public IReplyMarkup TaskMenuButtons()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton> { new KeyboardButton { Text = "Список дел" }, },
                    new List<KeyboardButton> { new KeyboardButton { Text = "Список команд" }, }
                },

                ResizeKeyboard = true
            };
        }

        public IReplyMarkup TaskCommandsButtons()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton>{new KeyboardButton { Text = "Добавить задачу" }, },
                    new List<KeyboardButton>{new KeyboardButton { Text = "Редактировать задачу" }, },
                    new List<KeyboardButton>{new KeyboardButton { Text = "Удалить задачу" }, },
                    new List<KeyboardButton>{new KeyboardButton { Text = "Веруться в предыдущее меню" }, },
                },

                ResizeKeyboard = true
            };
        }

        public IReplyMarkup AddNewTaskForm()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton>{new KeyboardButton { Text = "Добавить заголовок"}, },
                    new List<KeyboardButton>{new KeyboardButton { Text = "Добавить описание"}, },
                },
                ResizeKeyboard = true
            };
        }
    }
}
