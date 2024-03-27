using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskIt.Classes;

namespace TaskIt.Interfaces
{
    public interface IBudgetPrinter
    {
        void PrintBudgetCategories(IEnumerable<Budget> budgets);
        void PrintBudgetNames(IEnumerable<Budget> budget);
        void PrintBudgetInfo(Budget budget);
    }
}
