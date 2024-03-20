using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskIt.Interfaces;

namespace TaskIt.Classes
{
    public class ConsoleJournalPrinter : IJournalPrinter
    {
        public void PrintTableOfContents(IEnumerable<Journal> journals)
        {
            foreach (var journal in journals)
            {
                Console.WriteLine($"Name: [{journal.Name}] | LastChanged: [{journal.LastChanged}]");
            }
        }

        public void PrintInstructionJournal()
        {
            Console.WriteLine("Arbeit mit dem Journal-Tool:");
            Console.WriteLine("   -N <Journalname>       Neuen Journal mit dem Namen kreieren");
            Console.WriteLine("   -J <Journalname>       Journal mit den Seitenauswahl öffnen");
            Console.WriteLine("   -D <Journalname>       Journal mit dem Namen löschen");
            Console.WriteLine("   -Q                     Journal-Tab wieder verlassen \n");
        }
    }
}
