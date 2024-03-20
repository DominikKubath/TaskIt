using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskIt.Classes;
using TaskIt.Interfaces;

namespace TaskIt
{
    public class JournalPageUI
    {
        private readonly IJournalPageRepository _pageRepository;
        private readonly IJournalPagePrinter _pagePrinter;
        private readonly string _selectedJournal;

        internal JournalPageUI(IJournalPageRepository pageRepository, IJournalPagePrinter pagePrinter, string selectedJournal)
        {
            _pageRepository = pageRepository;
            _pagePrinter = pagePrinter;
            _selectedJournal = selectedJournal;
        }

        public void StartJournalPageUI()
        {
            bool continueExecution = true;

            while (continueExecution)
            {
                _pagePrinter.PrintInstructionPages();

                string command = Console.ReadLine();
                Console.Clear();
                _pagePrinter.PrintAllJournalPages(_pageRepository.GetAllPages(_selectedJournal));
                // Extrahiere Befehl und optionalen Parameter
                var splitCommand = command.Split(new[] { ' ' }, 2);
                var action = splitCommand[0];
                var name = splitCommand.Length > 1 ? splitCommand[1] : string.Empty;
                switch (action.ToUpper())
                {
                    case "-N":
                        CreatePage(name);
                        break;
                    case "-O":
                        OpenPage(name);
                        break;
                    case "-D":
                        DeletePage(name);
                        break;
                    case "-Q":
                        Console.WriteLine("Schließe Seitenansicht...");
                        // continueExecution = false;
                        break;
                    default:
                        Console.WriteLine("Kommando nicht bekannt.");
                        break;
                }
            }
        }


        public void CreatePage(string pageName)
        {
            if (string.IsNullOrEmpty(pageName))
            {
                Console.WriteLine("Wie soll deine Seite heißen?");
                pageName = Console.ReadLine();
            }

            JournalPage newPage = new JournalPage(pageName, null, DateTime.Now);
            _pageRepository.Add(_selectedJournal, newPage);
        }

        public void DeletePage(string pageName)
        {
            if (string.IsNullOrEmpty(pageName))
            {
                Console.WriteLine("Name der zu löschenden Seite: ");
                pageName = Console.ReadLine();
            }

            _pageRepository.Delete(_selectedJournal, pageName);
            Console.WriteLine("Seite gelöscht!");
        }

        public void OpenPage(string pageName)
        {
            // Hier implementierst du die Logik, um eine Seite zu öffnen
            Console.WriteLine("Öffne Seite: " + pageName);
        }
    }
}
