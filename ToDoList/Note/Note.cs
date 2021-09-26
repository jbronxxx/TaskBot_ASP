using System;

namespace ToDoList
{
    public class Note
    {
        public Guid ID { get; set; }

        public string Name { get; set; }

        public string Discription { get; set; }

        public DateTime DateTime { get; set; }
    }
}
