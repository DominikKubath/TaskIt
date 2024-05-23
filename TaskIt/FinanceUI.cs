using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TaskIt.Classes;
using TaskIt.Interfaces;
using static TaskIt.Classes.TodoItem;

namespace TaskIt
{
    public interface IFinanceUI
    {
        void StartFinanceUI();
    }

    public class FinanceUI : IFinanceUI
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionPrinter _transactionPrinter;

        private readonly IBudgetRepository _budgetRepository;
        private readonly IBudgetPrinter _budgetPrinter;

        public FinanceUI(ITransactionRepository transactionRepository, ITransactionPrinter transactionPrinter, IBudgetRepository budgetRepository, IBudgetPrinter budgetPrinter) 
        {
            _transactionRepository = transactionRepository;
            _transactionPrinter = transactionPrinter;
            _budgetRepository = budgetRepository;
            _budgetPrinter = budgetPrinter;
        }

        public void StartFinanceUI()
        {
            bool continueExecution = true;

            while (continueExecution)
            {
                Console.WriteLine("Willkommen im Finanzbuch. \n Folgende Funktionalitäten sind geboten:");
                Console.WriteLine("- Eingabe, Aktualisieren und Löschen von Einnahmen und Ausgaben\n- Übersicht aller Transaktionen \n- Filtern von Transaktionen");
                Console.WriteLine("- Anlegen von Budgets\n");

                Console.WriteLine("\n\nVerfügbare Aktionen: \n-A (Transaktion Hinzufügen), \n-U (Transaktion Aktualisieren), \n-D (Transaktion Löschen), " +
                    "\n-O (Übersicht aller Transaktionen)\n-F (Filtern), \n-B (Budget Anlegen/Bearbeiten),\n-Q (Beenden)");
                string command = Console.ReadLine();
                Console.Clear();
                switch (command.ToUpper())
                {
                    case "-A":
                        Console.WriteLine("Transaktion Hinzufügen...");
                        AddTransaction();
                        break;
                    case "-U":
                        Console.WriteLine("Transaktion Aktualisieren...");
                        UpdateTransaction();
                        break;
                    case "-D":
                        Console.WriteLine("Transaktion Löschen...");
                        DeleteTransaction();
                        break;
                    case "-O":
                        Console.WriteLine("Übersicht von Transaktionen...");
                        GetTransactionOverview(_transactionRepository.GetAll());
                        break;
                    case "-F":
                        Console.WriteLine("Transaktionen Filtern...");
                        FilterTransactions();
                        break;
                    case "-B":
                        Console.WriteLine("Budget Anlegen/Bearbeiten...");
                        var budgetUI = new BudgetUI(_budgetRepository, _budgetPrinter, _transactionRepository);
                        budgetUI.StartBudgetUI();
                        break;
                    case "-Q":
                        Console.WriteLine("Schließe Finanzbuch...");
                        continueExecution = false;
                        break;
                    default:
                        Console.WriteLine("Aktion nicht bekannt...");
                        break;
                }
            }
        }

        public void AddTransaction()
        {
            var name = GetTransactionName();
            var type = GetTransactionType();
            var amount = GetAmount();
            var date = GetDate();
            var budget = GetBudget();
            Transaction transaction;
            if(budget == null)
            {
                transaction = new Transaction(name, amount, type, date);
            }
            else
            {
                transaction = new Transaction(name, amount, type, date, budget);
            }
            _transactionRepository.Add(transaction);
            Console.WriteLine("Transaktion Hinzugefügt!");
        }

        public void UpdateTransaction()
        {
            Console.Write("Welche ID hat die Transaktion?: ");
            var searchedTransaction = Console.ReadLine();
            try
            {
                int id = int.Parse(searchedTransaction);
                var existingTransaction = _transactionRepository.GetById(id);

                if(existingTransaction != null)
                {
                    Console.Write("Neuer Name? (J/N): ");
                    var hasNewName = Console.ReadLine();
                    if (hasNewName.ToUpper().Equals("J"))
                        existingTransaction.Name = GetTransactionName();

                    Console.Write("Anderer Typ (Eingehend / Ausgehend)? (J/N): ");
                    var hasDifferentType = Console.ReadLine();
                    if (hasDifferentType.ToUpper().Equals("J"))
                        existingTransaction.Kind = GetTransactionType();

                    Console.Write("Anderer Betrag? (J/N): ");
                    var hasNewAmount = Console.ReadLine();
                    if (hasNewAmount.ToUpper().Equals("J"))
                        existingTransaction.Amount = GetAmount();

                    Console.Write("Anderes Datum? (J/N): ");
                    var hasNewDate = Console.ReadLine();
                    if (hasNewDate.ToUpper().Equals("J"))
                        existingTransaction.Date = GetDate();

                    Console.Write("Anderes Budget? (J/N): ");
                    var hasDiferentBudget = Console.ReadLine();
                    if (hasDiferentBudget.ToUpper().Equals("J"))
                        existingTransaction.Category = GetBudget();

                    _transactionRepository.Update(existingTransaction);
                    Console.WriteLine("Transaktion Aktualisiert!");
                }
            }
            catch
            {
                Console.WriteLine("Ungültige Eingabe: ID nicht gefunden");
            }
        }

        public void DeleteTransaction()
        {
            Console.Write("Welche ID hat die Transaktion?: ");
            var searchedTransaction = Console.ReadLine();
            try
            {
                int id = int.Parse(searchedTransaction);
                var existingTransaction = _transactionRepository.GetById(id);
                if (existingTransaction != null)
                {
                    _transactionRepository.Delete(id);
                    Console.WriteLine("Transaktion Gelöscht!");
                }

            }
            catch
            {
                Console.WriteLine("Ungültige Eingabe: ID nicht gefunden");
            }
        }

        public void GetTransactionOverview(IEnumerable<Transaction> transactions)
        {
            _transactionPrinter.PrintHeader();
            _transactionPrinter.PrintItems(transactions);
            _transactionPrinter.PrintFooter();
        }

        public void FilterTransactions()
        {
            _transactionPrinter.PrintFilterInstructions();
            Console.Write("\n\n Gebe ein Kommando ein: ");
            string command = Console.ReadLine();
            Console.Clear();

            var splitCommand = command.Split(new[] { ' ' }, 2);
            var action = splitCommand[0];
            var name = splitCommand.Length > 1 ? splitCommand[1] : string.Empty;
            try
            {
                switch(action.ToUpper())
                {
                    case "-E":
                        GetTransactionOverview(_transactionRepository.GetAllIncoming());
                        break;
                    case "-A":
                        GetTransactionOverview(_transactionRepository.GetAllOutgoing());
                        break;
                    case "-B":
                        GetTransactionOverview(_transactionRepository.GetAllOfCategory(name.ToString()));
                        break;
                    case "-W":
                        GetTransactionOverview(_transactionRepository.GetAllOfLastSevenDays());
                        break;
                    case "-M":
                        GetTransactionOverview(_transactionRepository.GetAllOfMonth(int.Parse(name.ToString())));
                        break;
                    case "-J":
                        GetTransactionOverview(_transactionRepository.GetAllOfYear(int.Parse(name.ToString())));
                        break;
                    default:
                        Console.WriteLine("Kommando nicht bekannt...");
                        break;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine("Kommando nicht bekannt. Abbruch...");
            }
        }

        private TransactionKind GetTypeFromString(string type)
        {
            if (string.IsNullOrEmpty(type) || type.ToUpper() != "-A" && type.ToUpper() != "-E")
            {
                type = "-A";
                Console.WriteLine("Kein Typ angegeben - Typ wurde standardmäßig als 'Ausgabe' definiert");
            }
            TransactionKind finalType;
            switch (type.ToUpper())
            {
                case "-A":
                    finalType = TransactionKind.Outgoing; 
                    break;
                case "-E":
                    finalType = TransactionKind.Incoming;
                    break;
                default:
                    finalType= TransactionKind.Outgoing;
                    break;
            }

            return finalType;
        }

        private string GetTransactionName()
        {
            Console.Write("Name der Transaktion: ");
            string name = Console.ReadLine();
            if (string.IsNullOrEmpty(name))
            {
                name = "Kein Name";
            }
            return name;
        }

        private TransactionKind GetTransactionType()
        {
            Console.Write(" \nKommando für Einnahmen => -E \nKommando für Ausgaben => -A\n Typ der Transaktion (Einnahme / Ausgabe): ");
            string type = Console.ReadLine();
            TransactionKind finalType = GetTypeFromString(type);
            return finalType;
        }

        private DateTime GetDate()
        {
            Console.WriteLine("Gebe das Datum im folgenden Format an: dd.MM.yyyy (z.B., 01.06.2024)");
            Console.Write("Datum der Transaktion: ");
            string transactionDate = Console.ReadLine();
            DateTime date;
            DateTime.TryParseExact(transactionDate, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
            return date;
        }

        private double GetAmount()
        {
            Console.Write("Betrag der Transaktion (Nur Positivie Zahlen): ");
            string amountText = Console.ReadLine();
            if (amountText.StartsWith('-'))
            {
                amountText = amountText.Substring(1);
            }
            double amount = Convert.ToDouble(amountText);
            return amount;
        }

        private Budget GetBudget()
        {
            Console.Write("Gehört diese Transaktion einem Budget an? (J/N): ");
            var hasBudget = Console.ReadLine();
            if(hasBudget.ToUpper() == "N")
            {
                return null;
            }
            else if(hasBudget.ToUpper() == "J") 
            {
                Console.WriteLine("Zu welcher der folgenden Budgets gehört die Transaktion: ");
                _budgetPrinter.PrintBudgetNames(_budgetRepository.GetAll());
                Console.Write("\nGebe den Namen des Budgets an: ");
                var budgetName = Console.ReadLine();
                var budgetResult = _budgetRepository.SearchForSimiliarName(budgetName);
                return budgetResult;
            }
            else
            {
                Console.WriteLine("Unbekannte Antwort\n Budget wird standardmäßig auf 'Kein Budget' gesetzt");
                return null;
            }
        }

    }
}
