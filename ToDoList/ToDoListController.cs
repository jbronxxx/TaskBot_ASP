using System;
using System.Collections.Generic;
using System.Linq;
using ToDoList.Notes;

namespace ToDoList
{
    public class ToDoListController
    {
        private List<Note> _notes { get; set; }

        public void Create(long id, string message)
        {
            _notes = new List<Note>();

            if (id != 0)
            {
                var result = _notes.FirstOrDefault(x => x.ID == id);

                _notes.Add(new Note 
                {
                    ID = id,
                    Name = message,
                    DateTime = DateTime.Now
                });
            }
        }
    }
}
