using System.Runtime.CompilerServices;
using TaskIt.Interfaces;
using TaskIt.Classes;
using TaskIt;


/*ITodoRepository todoRepository = new FileTodoRepository("todos.json");
ITodoPrinter todoPrinter = new ConsoleTodoPrinter();

todoPrinter.PrintInstructions();

var todoListUI = new TodoListConsoleUI(todoRepository, todoPrinter);
todoListUI.StartTodoListUI();*/

StartApp();

static void StartApp()
{
    ITodoRepository todoRepository = new FileTodoRepository("todos.json");
    ITodoPrinter todoPrinter = new ConsoleTodoPrinter();

    IJournalRepository journalRepository = new FileJournalRepository("journal.json");
    IJournalPrinter journalPrinter = new ConsoleJournalPrinter();

    IJournalPageRepository journalPageRepository = new FileJournalPageRepository("Journals/");
    IJournalPagePrinter journalPagePrinter = new ConsoleJournalPagePrinter();

    while (true)
    {
        Console.WriteLine("Journale öffnen: -J \nTodo Liste öffnen: -T \nFinanzbuch öffnen: -F \nSchließen der Anwendung: -Q \nGebe ein Kommando ein: ");
        string command = Console.ReadLine();
        Console.Clear();

        switch (command.ToUpper())
        {
            case "-J":
                Console.WriteLine("Öffne Journale...\n");
                var journalsUI = new JournalUI(journalRepository, journalPrinter, journalPageRepository, journalPagePrinter);
                journalsUI.StartJournalUI();
                //ProcessJournalCommands(journalRepository, journalPrinter);
                break;
            case "-T":
                Console.WriteLine("Öffne Todo Liste...");
                var todoListUI = new TodoListConsoleUI(todoRepository, todoPrinter);
                todoListUI.StartTodoListUI();
                break;
            case "-F":
                Console.WriteLine("Öffne Finanzbuch...");
                // Implementiere die Logik für das Finanzbuch
                break;
            case "-Q":
                Console.WriteLine("Schließe Anwendung...");
                return;
            default:
                Console.WriteLine("Kommando nicht bekannt.");
                break;
        }
    }
}