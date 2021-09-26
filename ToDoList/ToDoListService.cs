using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList
{
    public class ToDoListService
    {
        private List<Note> _listNotes;

        public void ListAllNotes()
        {
            if (_listNotes != null)
            {
                foreach (var item in _listNotes)
                {
                    
                }
            }
            else
                return;
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
