using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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

        public IEnumerable<Budget> GetAll() => _budgets;


        //Get the current month and calculate all budgets from the transactions in a month
        //Passed transactions in Parameters are previously filtered for their month
        public double GetBudgetAmountLeft(IEnumerable<Transaction> transactions, Budget budget)
        {
            if (budget.Limit != null)
            {
                var amountLeft = budget.Limit;

                var categoriesTransactions = transactions.Where(t => t.Category.Name == budget.Name && t.Kind == TransactionKind.Outgoing);
                foreach(Transaction ta in categoriesTransactions)
                {
                    amountLeft -= ta.Amount;
                }
                categoriesTransactions = transactions.Where(t => t.Category.Name == budget.Name && t.Kind == TransactionKind.Incoming);
                foreach (Transaction ta in categoriesTransactions)
                {
                    amountLeft += ta.Amount;
                }

                return (double)amountLeft;
            }
            else
                return 0;

        }

        private bool BudgetAlreadyExists(string budgetName) => _budgets.Where(b => b.Name == budgetName).Any();

        public IEnumerable<Budget> GetMaxedOutBudgets(IEnumerable<Transaction> transactions)
        {
            List<Budget> budgets = new List<Budget>();
            foreach(Budget budget in _budgets)
            {
                if(GetBudgetAmountLeft(transactions, budget) <= 0)
                    budgets.Add(budget);
            }

            return budgets.AsEnumerable();
        }

        public IEnumerable<Budget> GetNotMaxedBudgets(IEnumerable<Transaction> transactions)
        {
            var budgets = GetMaxedOutBudgets(transactions);
            return _budgets.Where(b => !budgets.Contains(b));
        }

        public Budget SearchForSimiliarName(string searchedName) => _budgets.FirstOrDefault(b => b.Name.Contains(searchedName));

        public bool BudgetExists(string budgetName) => _budgets.Where(b => b.Name == budgetName).Any();

        public void AddBudget(Budget budget)
        {
            if(!BudgetAlreadyExists(budget.Name))
            {
                _budgets.Add(budget);
                SaveBudgets();
            }
        }

        public void DeleteBudget(string budgetName)
        {
            var foundBudget = SearchForSimiliarName(budgetName);
            if (foundBudget != null)
            {
                _budgets.Remove(foundBudget);
            }
        }

        public void UpdateBudget(Budget budget)
        {
            var foundbudget = _budgets.FirstOrDefault(b => b.Name == budget.Name);
            if(foundbudget != null)
            {
                foundbudget.Limit = budget.Limit;
                SaveBudgets();
            }
        }

        private void SaveBudgets()
        {
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(_budgets));
        }
    }
}
