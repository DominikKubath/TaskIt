using TaskIt.Classes;
using TaskIt.Interfaces;
using TaskIt;
using System.Xml.Linq;

namespace TaskIt
{
    public interface IJournalUI
    {
        void StartJournalUI();
    }

    public class JournalUI : IJournalUI
    {
        private readonly IJournalRepository _journalRepository;
        private readonly IJournalPrinter _journalPrinter;

        private readonly IJournalPageRepository _pageRepository;
        private readonly IJournalPagePrinter _pagePrinter;
        private string _selectedJournal;

        internal JournalUI(IJournalRepository journalRepository, IJournalPrinter journalPrinter, IJournalPageRepository pageRepository, IJournalPagePrinter pagePrinter)
        {
            _journalRepository = journalRepository;
            _journalPrinter = journalPrinter;
            _pageRepository = pageRepository;
            _pagePrinter = pagePrinter;
        }

        public void StartJournalUI()
        {
            bool continueExecution = true;

            while (continueExecution)
            {
                _journalPrinter.PrintInstructionJournal();                

                string command = Console.ReadLine();
                Console.Clear();
                _journalPrinter.PrintTableOfContents(_journalRepository.GetAll());
                // Extrahiere Befehl und optionalen Parameter
                var splitCommand = command.Split(new[] { ' ' }, 2);
                var action = splitCommand[0];
                var name = splitCommand.Length > 1 ? splitCommand[1] : string.Empty;
                switch (action.ToUpper())
                {
                    case "-N":
                        CreateJournal(name);
                        break;
                    case "-J":
                        OpenJournal(name);
                        break;
                    case "-D":
                        DeleteJournal(name);
                        break;
                    /*
                    case "-O":
                        OpenPage(name);
                        break;
                    */
                    case "-Q":
                        Console.WriteLine("Schließe Anwendung...");
                        continueExecution = false;
                        break;
                    default:
                        Console.WriteLine("Kommando nicht bekannt.");
                        break;
                }
            }
        }

  

        public void CreateJournal(string journalName)
        {
            if (string.IsNullOrEmpty(journalName))
            {
                Console.WriteLine("Wie soll dein Journal heißen?");
                journalName = Console.ReadLine();
            }

            JournalFunctions journalFunctions = new JournalFunctions();
            string result = journalFunctions.CreateNewJournal(journalName);
            Journal newJournal = new Journal("/Journals/" + journalName, journalName, DateTime.Now);
            _journalRepository.Add(newJournal);
        }

        public void DeleteJournal(string journalName)
        {
            if (string.IsNullOrEmpty(journalName))
            {
                Console.WriteLine("Name des zu löschenden Journals: ");
                journalName = Console.ReadLine();
            }

            _journalRepository.Delete(journalName);
            Console.WriteLine("Journal gelöscht!");
        }

        public void OpenJournal(string journalName)
        {
            if (string.IsNullOrEmpty(journalName))
            {
                Console.WriteLine("Welches Journal möchtest du öffnen? Gib den Namen ein: ");
                journalName = Console.ReadLine();
            }

            _selectedJournal = journalName;
            var journalPageUI = new JournalPageUI(_pageRepository, _pagePrinter, _selectedJournal);
            Console.WriteLine("Dein Journal ist: " + journalName);
            journalPageUI.StartJournalPageUI();
        }

    }

}
