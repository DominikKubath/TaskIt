using TaskIt.Classes;

namespace TaskIt.Interfaces
{
    public interface IBudgetRepository
    {

        IEnumerable<Budget> GetAll();
        IEnumerable<Budget> GetMaxedOutBudgets(IEnumerable<Transaction> transactions);
        IEnumerable<Budget> GetNotMaxedBudgets(IEnumerable<Transaction> transactions);
        Budget SearchForSimiliarName(string searchedName);
        double GetBudgetAmountLeft(IEnumerable<Transaction> transactions, Budget budget);
        bool BudgetExists(string budgetName);
        void AddBudget(Budget budget);
        void UpdateBudget(Budget budget);
        void DeleteBudget(string budgetName);
    }
}
