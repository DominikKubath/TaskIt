using TaskIt.Classes;

namespace TaskIt.Interfaces
{
    public interface IBudgetRepository
    {

        IEnumerable<Budget> GetAll();
        IEnumerable<Budget> GetMaxedOutBudgets();
        IEnumerable<Budget> GetNotMaxedBudgets();
        Budget SearchForSimiliarName(string searchedName);
        double GetBudgetAmountLeft(IEnumerable<Transaction> transactions);
        bool BudgetExists(string budgetName);
        void AddBudget(Budget budget);
        void UpdateBudget(Budget budget);
        void DeleteBudget(string budgetName);
    }
}
