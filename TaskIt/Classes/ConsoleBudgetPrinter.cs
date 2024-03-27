using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskIt.Interfaces;

namespace TaskIt.Classes
{
    public class ConsoleBudgetPrinter : IBudgetPrinter
    {
        const int ColumnWidth = 20;

        public void PrintBudgetCategories(IEnumerable<Budget> budgets)
        {
            Console.WriteLine(new string('-', (ColumnWidth + 3) * 5 - 1));
            Console.WriteLine("| {0} | {1} |",
                "Name".PadRight(ColumnWidth, ' '),
                "Limit".PadRight(ColumnWidth, ' '));

            Console.WriteLine(new string('-', (ColumnWidth + 3) * 5 - 1));
            foreach (var budget in budgets)
            {
                string limit = budget.Limit != null? budget.Limit.ToString() : "No Limit";

                Console.WriteLine("| {0} | {1} |",
                    budget.Name.PadRight(ColumnWidth, ' '),
                    limit.PadRight(ColumnWidth, ' '));
            }
            Console.WriteLine(new string('-', (ColumnWidth + 3) * 5 - 1));
        }

        public void PrintBudgetNames(IEnumerable<Budget> budgets)
        {
            if (budgets.Any())
            {
                foreach (var budget in budgets)
                {
                    Console.WriteLine($"{budget.Name} - ");
                }
            }
            else
            {
                Console.WriteLine("Es Existieren keine Budgets");

            }
        }

        public void PrintBudgetInfo(Budget budget)
        {
            throw new NotImplementedException();
        }

    }
}
