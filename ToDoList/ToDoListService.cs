using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ToDoList
{
    public class ToDoListService
    {
        public static List<Note> _listNotes;

        public static async Task<Message> ListAllNotes(ITelegramBotClient botClient, Message message)
        {
            if (_listNotes != null)
            {
                foreach (var item in _listNotes)
                {
                    return await botClient.SendTextMessageAsync(message.Chat.Id,
                        item.Name);
                }
            }
            else
            {
                return await botClient.SendTextMessageAsync(message.Chat.Id,
                    "Список дел пуст. Создай новую задачу");
            }
            return await botClient.SendTextMessageAsync(message.Chat.Id, 
                "Твой список дел");
        }

        public List<Note> CreateListNotes(string title, string discription)
        {
            if (_listNotes == null)
            {
                _listNotes = new List<Note>();

                _listNotes.Add(new Note
                {
                    ID = new Guid(),
                    Name = title,
                    Discription = discription,
                    DateTime = DateTime.Now
                });
            }
            else
            {
                _listNotes.Add(new Note
                {
                    ID = new Guid(),
                    Name = title,
                    Discription = discription,
                    DateTime = DateTime.Now
                });
            }

            return _listNotes;
        }
    }
}
