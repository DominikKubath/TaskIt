using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TaskIt.Classes;
using TaskIt.Interfaces;

namespace TaskIt
{
    public class BudgetUI
    {
        private readonly IBudgetRepository _budgetRepository;
        private readonly IBudgetPrinter _budgetPrinter;

        private readonly ITransactionRepository _transactionRepository;

        public BudgetUI(IBudgetRepository budgetRepository, IBudgetPrinter budgetPrinter, ITransactionRepository transactionRepository) 
        {
            _budgetRepository = budgetRepository;
            _budgetPrinter = budgetPrinter;

            _transactionRepository = transactionRepository;
        }

        public void StartBudgetUI()
        {
            bool continueExecution = true;

            while (continueExecution)
            {
                Console.WriteLine("Management von Budgets. \n Folgende Funktionalitäten sind geboten:");
                Console.WriteLine("- Eingabe, Aktualisieren und Löschen von Budgets\n- Übersicht aller Budgets\n- Filtern von Budgets");
                Console.WriteLine("- Anlegen von Budgets\n");

                Console.WriteLine("\n\nVerfügbare Aktionen: \n-A (Budget Hinzufügen), \n-U (Budget Aktualisieren), \n-D (Budget Löschen), " +
                    "\n-O (Übersicht aller Budgets)\n-F (Filtern), \n-Q (Beenden)");
                string command = Console.ReadLine();
                Console.Clear();
                switch (command.ToUpper())
                {
                    case "-A":
                        Console.WriteLine("Budget Hinzufügen...");
                        AddBudget();
                        break;
                    case "-U":
                        Console.WriteLine("Budget Aktualisieren...");
                        UpdateBudget();
                        break;
                    case "-D":
                        Console.WriteLine("Budget Löschen...");
                        RemoveBudget();
                        break;
                    case "-O":
                        Console.WriteLine("Übersicht von Budgets...");
                        GetBudgetOverview();
                        break;
                    case "-F":
                        Console.WriteLine("Filtere Budgets...");
                        SelectBudgetFilter();
                        break;
                    case "-Q":
                        Console.WriteLine("Schließe Budgetverwaltung...");
                        continueExecution = false;
                        break;
                    default:
                        Console.WriteLine("Aktion nicht bekannt...");
                        break;
                }
            }
        }

        public void AddBudget()
        {
            var name = GetBudgetName();
            var limit = GetLimit();
            Budget budget; 
            if(limit == null)
            {
                budget = new Budget(name);
            }
            else
            {
                budget = new Budget(name, (double)limit);
            }
            _budgetRepository.AddBudget(budget);
            Console.WriteLine("Neues Budget Hinzugefügt!");
        }

        public void UpdateBudget()
        {
            Console.Write("Welchen Namen hat das Budget?: ");
            var searchedBudget = Console.Read();
            try
            {
                var existingBudget = _budgetRepository.SearchForSimiliarName(searchedBudget.ToString());
                Console.Write("Willst du ein neues Limit für das Budget definieren? (J/N): ");
                var isNewLimit = Console.Read();
                if (isNewLimit.ToString().Equals("J"))
                    existingBudget.Limit = GetLimit();

                _budgetRepository.UpdateBudget(existingBudget);
                Console.WriteLine("Budget wurde Aktualisiert!");
            }
            catch
            {
                Console.WriteLine("Fehler: Ungültige Eingabe oder Budget konnte nicht gefunden werden");
            }
        }

        public void RemoveBudget()
        {
            Console.Write("Welchen Namen hat das Budget?: ");
            var searchedBudget = Console.Read();
            try
            {
                var existingBudget = _budgetRepository.SearchForSimiliarName(searchedBudget.ToString());
                if (existingBudget != null)
                {
                    _budgetRepository.DeleteBudget(searchedBudget.ToString());
                    Console.WriteLine("Budget Gelöscht!");
                }

            }
            catch
            {
                Console.WriteLine("Ungültige Eingabe: Budget mit diesem Namen wurde nicht gefunden");
            }
        }

        public void GetBudgetOverview()
        {
            _budgetPrinter.PrintBudgetCategories(_budgetRepository.GetAll());
        }

        public void SelectBudgetFilter()
        {
            Console.WriteLine("\n\nVerfügbare Budget Filter: \n-M (Ausgeschöpfte Budgets in diesem Monat), \n-N (Noch nicht ausgeschöpfte Budgets in diesem Monat), \n-Q (Beenden)");
            string command = Console.ReadLine();
            Console.Clear();
            switch (command.ToUpper())
            {
                case "-M":
                    _budgetPrinter.PrintBudgetCategories(_budgetRepository.GetMaxedOutBudgets(_transactionRepository.GetAllOfMonth(DateTime.Now.Month)));
                    break;
                case "-N":
                    _budgetPrinter.PrintBudgetCategories(_budgetRepository.GetNotMaxedBudgets(_transactionRepository.GetAllOfMonth(DateTime.Now.Month)));
                    break;
                case "-Q":
                    break;
                default:
                    break;
            }
        }

        private string GetBudgetName()
        {
            Console.Write("Name des Budgets: ");
            string name = Console.ReadLine();
            if (string.IsNullOrEmpty(name))
            {
                name = "Kein Name";
            }
            return name;
        }

        private double? GetLimit()
        {
            Console.Write("Betrag des Budget Limits(Nur Positivie Zahlen) \n Falls kein Limit für ein Budget Existiert, drücke nur 'Enter': ");
            string amountText = Console.ReadLine();
            if (string.IsNullOrEmpty(amountText))
                return null;

            if (amountText.StartsWith('-'))
            {
                amountText = amountText.Substring(1);
            }
            double amount = Convert.ToDouble(amountText);
            return amount;
        }

    }
}
