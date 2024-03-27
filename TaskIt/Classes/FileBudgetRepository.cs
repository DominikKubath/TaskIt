using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TaskIt.Interfaces;

namespace TaskIt.Classes
{
    public class FileBudgetRepository : IBudgetRepository
    {
        private readonly string _filePath;
        private List<Budget> _budgets;

        public FileBudgetRepository(string filePath)
        {
            _filePath = filePath;

            _budgets = File.Exists(_filePath)
            ? JsonConvert.DeserializeObject<List<Budget>>(File.ReadAllText(_filePath))
            : new List<Budget>();
        }

        public IEnumerable<Budget> GetAll()
        {
            throw new NotImplementedException();
        }

        public double GetBudgetAmountLeft(IEnumerable<Transaction> transactions)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Budget> GetMaxedOutBudgets()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Budget> GetNotMaxedBudgets()
        {
            throw new NotImplementedException();
        }

        public Budget SearchForSimiliarName(string searchedName) => _budgets.FirstOrDefault(b => b.Name.Contains(searchedName));

        public void AddBudget(Budget budget)
        {
            throw new NotImplementedException();
        }

        public bool BudgetExists(string budgetName)
        {
            throw new NotImplementedException();
        }

        public void DeleteBudget(string budgetName)
        {
            throw new NotImplementedException();
        }

        public void UpdateBudget(Budget budget)
        {
            throw new NotImplementedException();
        }
    }
}
