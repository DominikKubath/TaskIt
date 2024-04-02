using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskIt.Interfaces;

namespace TaskIt.Classes
{
    public class ConsoleTransactionPrinter : ITransactionPrinter
    {
        const int ColumnWidth = 20;

        public void PrintHeader()
        {
            Console.WriteLine(new string('-', (ColumnWidth + 3) * 6 - 1));
            Console.WriteLine("| {0} | {1} | {2} | {3} | {4} | {5} |",
                                "ID".PadRight(ColumnWidth, ' '),
                                "Date".PadRight(ColumnWidth, ' '),
                                "Name".PadRight(ColumnWidth, ' '),
                                "Amount".PadRight(ColumnWidth, ' '),
                                "Kind".PadRight(ColumnWidth, ' '),
                                "Category".PadRight(ColumnWidth, ' '));
            Console.WriteLine(new string('-', (ColumnWidth + 3) * 6 - 1));
        }

        public void PrintItems(IEnumerable<Transaction> items)
        {
            foreach (Transaction item in items)
            {
                string budgetCategory = item.Category != null? item.Category.Name : "Keine Kategorie";

                Console.WriteLine("| {0} | {1} | {2} | {3} | {4} | {5} |", 
                    item.ID.ToString().PadRight(ColumnWidth, ' '),
                    item.Date.ToString().PadRight(ColumnWidth, ' '),
                    item.Name.PadRight(ColumnWidth, ' '), 
                    item.Amount.ToString().PadRight(ColumnWidth, ' '),
                    item.Kind.ToString().PadRight(ColumnWidth, ' '),
                    budgetCategory.PadRight(ColumnWidth, ' '));
            }
        }

        public void PrintFooter()
        {
            Console.WriteLine(new string('-', (ColumnWidth + 3) * 6 - 1));
        }

        public void PrintFilterInstructions()
        {
            Console.WriteLine("Nach welchen Kriterien möchtest du die Transaktionen gefiltert ausgeben?");
            Console.WriteLine("Folgende Optionen sind möglich:");
            Console.WriteLine("- Alle eingehenden Transaktionen ('-E')");
            Console.WriteLine("- Alle ausgehenden Transaktionen ('-A')");
            Console.WriteLine("- Alle Transaktionen eines Budgets ('-B BudgetName')");
            Console.WriteLine("- Alle Transaktionen der letzten 7 Tage ('-W')");
            Console.WriteLine("- Alle Transaktionen aus einem Monat ('-M MonatsZahl') -- Monatszahlen: 1 - 12");
            Console.WriteLine("- Alle Transaktionen aus einem Jahr ('-J JahresZahl') -- Jahreszahlen: Bsp. 2022");
        }
    }
}
