using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskIt
{
    internal class JournalFunctions
    {
        public JournalFunctions() { }

        public string CreateNewJournal(string journalName)
        {
            string dir = "./Journals";
            if (!Directory.Exists(Path.Combine(dir, journalName)))
            {
                Directory.CreateDirectory(Path.Combine(dir, journalName));
                return $"Journal with the name {journalName} was created. \n";
            }

            return "Journal already exists!";
        }
    }
}
