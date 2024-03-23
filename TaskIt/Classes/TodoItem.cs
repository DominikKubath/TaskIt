using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskIt.Classes
{
    public class TodoItem
    {
        public enum Priority
        {
            Highest = 1,
            High = 2,
            Medium = 3,
            Low = 4,
            None = 5
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public Priority Prio { get; set; }      
        public DateTime? Deadline { get; set; }
        public TodoItem(string name, string description, int priority = 5) 
        {
            Name = name; 
            Description = description;
            Prio = (Priority)priority;
        }
    }
}
