using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskIt.Classes
{
    public class JournalPage
    {
        public string Name { get; set; }
        public string? Content { get; set; }
        public DateTime LastChanged { get; set; }

        public JournalPage(string name, string? content, DateTime lastChanged) 
        {
            Name = name;
            Content = content;
            LastChanged = lastChanged;
        }
    }
}
