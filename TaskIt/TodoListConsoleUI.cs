using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskIt.Interfaces;
using TaskIt.Classes;

namespace TaskIt
{
    public interface ITodoListConsoleUI
    {
        void StartTodoListUI();
    }

    public class TodoListConsoleUI : ITodoListConsoleUI
    { 
        private readonly ITodoRepository _repository;
        private readonly ITodoPrinter _printer;

        public TodoListConsoleUI(ITodoRepository todoRepository, ITodoPrinter todoPrinter)
        {
            _repository = todoRepository;
            _printer = todoPrinter;
        }

        public void StartTodoListUI()
        {
            bool continueExecution = true;

            while (continueExecution)
            {
                _printer.PrintTodo(_repository.GetAll());
                Console.WriteLine("Verfügbare Aktionen: -A (Hinzufügen), -U (Aktualisieren), -D (Löschen), -F (Filtern nach Priorität), -Q (Beenden)");
                string command = Console.ReadLine();
                Console.Clear();
                switch (command.ToUpper())
                {
                    case "-A":
                        Console.WriteLine("Hinzufügen einer Aufgabe...");
                        AddTodoItem();
                        break;
                    case "-U":
                        Console.WriteLine("Aktualisieren einer Aufgabe...");
                        UpdateTodoItem();
                        break;
                    case "-D":
                        Console.WriteLine("Löschen einer Aufgabe...");
                        DeleteTodoItem();
                        break;
                    case "-F":
                        Console.Write("Aufgaben mit welcher Priorität sollen ausgegeben werden?: ");
                        string input = Console.ReadLine();
                        int prio;
                        if (input != null)
                            prio = int.Parse(input);
                        else
                            prio = 5;
                        _printer.PrintTodo(_repository.GetByPriority(prio));
                        break;
                    case "-Q":
                        Console.WriteLine("Schließe Todo-Liste...");
                        continueExecution = false;
                        break;
                    default:
                        Console.WriteLine("Kommando nicht bekannt.");
                        break;
                }
            }
        }

        public void AddTodoItem()
        {
            Console.Write("Name der Aufgabe: ");
            string name = Console.ReadLine();

            Console.Write("Beschreibung der Aufgabe: ");
            string description = Console.ReadLine();

            Console.Write("Priorität der Aufgabe | 1 (Höchste), 2 (Hohe), 3 (Mittlere), 4 (Niedrige), 5 (Keine) | : ");
            string priority = Console.ReadLine();
            int prio;
            try
            {
                prio = int.Parse(priority);
            }
            catch 
            {
                Console.WriteLine("Ungültige Eingabe: Priorität wurde Standartmäßig auf 5 (Keine Priorität) gesetzt");
                prio = 5;
            }

            var newTodo = new TodoItem(name, description, prio);
            newTodo.IsCompleted = false;
            _repository.Add(newTodo);

            Console.WriteLine("Aufgabe hinzugefügt!");
        }

        public void UpdateTodoItem()
        {
            Console.Write("ID der zu aktualisierenden Aufgabe: ");
            if (int.TryParse(Console.ReadLine(), out int todoId))
            {
                var existingTodo = _repository.GetById(todoId);
                if (existingTodo != null)
                {
                    Console.Write("Neuer Name? (J/N): ");
                    string isNewName = Console.ReadLine();
                    if(isNewName.Trim().ToUpper() == "J")
                    {
                        Console.Write("Neuer Name: ");
                        existingTodo.Name = Console.ReadLine();
                    }

                    Console.Write("Neue Beschreibung? (J/N): ");
                    string isNewDescription = Console.ReadLine();
                    if (isNewDescription.Trim().ToUpper() == "J")
                    {
                        Console.Write("Neue Beschreibung: ");
                        existingTodo.Description = Console.ReadLine();
                    }

                    Console.Write("Neue Priorität? (J/N): ");
                    string isNewPrio = Console.ReadLine();
                    if (isNewPrio.Trim().ToUpper() == "J")
                    {
                        Console.WriteLine("Mögliche Prioritäten: \n Höchste => 1\n Hoch => 2\n Mittel => 3\n Niedrig => 4\n Keine => 5\n");
                        Console.Write("Neue Priorität: ");
                        existingTodo.Prio = (TodoItem.Priority)int.Parse(Console.ReadLine());
                    }

                    Console.Write("Ist die Aufgabe abgeschlossen? (J/N): ");
                    string isCompletedInput = Console.ReadLine();
                    existingTodo.IsCompleted = isCompletedInput.Trim().ToUpper() == "J";

                    // Aktualisieren Sie die Aufgabe im Repository.
                    _repository.Update(existingTodo);

                    Console.WriteLine("Aufgabe aktualisiert!");
                }
                else
                {
                    Console.WriteLine($"Aufgabe mit ID {todoId} nicht gefunden.");
                }
            }
            else
            {
                Console.WriteLine("Ungültige ID.");
            }
        }

        public void DeleteTodoItem()
        {
            Console.Write("ID der zu löschenden Aufgabe: ");
            if (int.TryParse(Console.ReadLine(), out int todoId))
            {
                _repository.Delete(todoId);
                Console.WriteLine("Aufgabe gelöscht!");
            }
            else
            {
                Console.WriteLine("Ungültige ID.");
            }
        }

    }
}
