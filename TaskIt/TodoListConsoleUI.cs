using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskIt.Interfaces;
using TaskIt.Classes;
using System.Globalization;

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
                var closeToDeadline = _repository.GetTodosCloseToDeadline();
                if(closeToDeadline.Any())
                {
                    Console.WriteLine("\n\n\nFolgende Todo's sind nah an der Deadline: ");
                    _printer.PrintTodo(closeToDeadline);
                }
                Console.WriteLine("\n\nVerfügbare Aktionen: -A (Hinzufügen), -U (Aktualisieren), -D (Löschen), -F (Filtern nach Priorität), -Q (Beenden)");
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
            var name = GetName();

            var description = GetDescription();

            var prio = GetPriority();


            var newTodo = new TodoItem(name, description, prio);
            newTodo.IsCompleted = false;
            Console.Write("Besitzt die Aufgabe eine Deadline? (J/N): ");
            string hasDeadline = Console.ReadLine();
            if (hasDeadline.Trim().ToUpper() == "J")
            {
                newTodo.Deadline = GetDeadline();
            }

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
                        existingTodo.Name = GetName();
                    }

                    Console.Write("Neue Beschreibung? (J/N): ");
                    string isNewDescription = Console.ReadLine();
                    if (isNewDescription.Trim().ToUpper() == "J")
                    {
                        existingTodo.Description = GetDescription();
                    }

                    Console.Write("Neue Priorität? (J/N): ");
                    string isNewPrio = Console.ReadLine();
                    if (isNewPrio.Trim().ToUpper() == "J")
                    {
                        existingTodo.Prio = (TodoItem.Priority)GetPriority();
                    }

                    Console.Write("Neue Deadline? (J/N): ");
                    string hasDeadline = Console.ReadLine();
                    if (hasDeadline.Trim().ToUpper() == "J")
                    {

                        existingTodo.Deadline = GetDeadline();
                    }

                    Console.Write("Ist die Aufgabe abgeschlossen? (J/N): ");
                    string isCompletedInput = Console.ReadLine();
                    existingTodo.IsCompleted = isCompletedInput.Trim().ToUpper() == "J";

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


        public string GetName()
        {
            Console.Write("Name der Aufgabe: ");
            string name = Console.ReadLine();
            return name;
        }

        public string GetDescription()
        {
            Console.Write("Beschreibung der Aufgabe: ");
            string description = Console.ReadLine();
            return description;
        }

        public DateTime? GetDeadline()
        {
            Console.WriteLine("Gebe eine Deadline im folgenden Format an: dd.MM.yyyy (z.B., 01.06.2024)");
            Console.Write("Deadline ist am: ");
            string deadlineInput = Console.ReadLine();

            DateTime deadline;
            if (DateTime.TryParseExact(deadlineInput, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out deadline))
            {
                return deadline;
            }
            else
            {
                Console.WriteLine("Ungültiges Datumformat für die Deadline. Die Deadline wurde nicht gesetzt.");
                return null;
            }
        }

        public int GetPriority()
        {
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
            return prio;
        }

    }
}
