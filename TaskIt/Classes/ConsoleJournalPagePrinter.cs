using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskIt.Interfaces;

namespace TaskIt.Classes
{
    public class ConsoleJournalPagePrinter : IJournalPagePrinter
    {
        public void PrintAllJournalPages(List<JournalPage> pages)
        {
            foreach (var page in pages)
            {
                Console.WriteLine($"Name: [{page.Name}] | LastChanged: [{page.LastChanged}]");
            }
        }

        public void PrintInstructionPages()
        {
            Console.WriteLine("Arbeit mit Seiten:");
            Console.WriteLine("   -O <Seitenname>        Seite mit dem Namen öffnen");
            Console.WriteLine("   -N <Seitenname>        Neue Seite in Journal hinzufügen");
            Console.WriteLine("   -D <Seitenname>        Eine Seite löschen \n");
            Console.WriteLine("   -Q                     Seite speichern und verlassen \n");

            Console.WriteLine("Spefizikation von Todos auf einer Seite: @[Anweisung]@");
            Console.WriteLine("Beispiel: @Wäsche waschen@ \n");
        }
    }
}
