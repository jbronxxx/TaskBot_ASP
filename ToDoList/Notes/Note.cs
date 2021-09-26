using System;

namespace ToDoList.Notes
{
    class Note
    {
        public long ID { get; set; }

        public string Name { get; set; }

        public string Discription { get; set; }

        public DateTime DateTime { get; set; }

        public NotState NoteState { get; set; }
    }
}
