using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using TaskIt.Interfaces;
using static TaskIt.Classes.TodoItem;

namespace TaskIt.Classes
{
    public class FileTransactionRepository : ITransactionRepository
    {
        private readonly string _filePath;
        private List<Transaction> _transactions;

        public FileTransactionRepository(string filePath)
        {
            _filePath = filePath;

            _transactions = File.Exists(_filePath)
            ? JsonConvert.DeserializeObject<List<Transaction>>(File.ReadAllText(_filePath))
            : new List<Transaction>();
        }

        public IEnumerable<Transaction> GetAll() => _transactions;

        public Transaction GetById(int id) => _transactions.FirstOrDefault(t => t.ID == id);

        public IEnumerable<Transaction> GetAllIncoming() => _transactions.Where(t => t.Kind == TransactionKind.Incoming);
        public IEnumerable<Transaction> GetAllOutgoing() => _transactions.Where(t => t.Kind == TransactionKind.Outgoing);

        public IEnumerable<Transaction> GetAllOfCategory(string category)
        {
            var transactions = _transactions.Where(t => t.Category?.Name == category).ToList();

            if(transactions.Count == 0)
            {
                return _transactions.Where(t => t.Category.Name.Contains(category));
            }
            else
            {
                return transactions;
            }
        }

        public IEnumerable<Transaction> GetAllOfLastSevenDays()
        {
            var result = new List<Transaction>();
            foreach(Transaction item in _transactions)
            {
                var timespan = item.Date.Date - DateTime.Now;

                if(timespan.Days <= 7)
                    result.Add(item);
            }
            return result;
        }

        public IEnumerable<Transaction> GetAllOfMonth(int month)
        {
            var result = new List<Transaction>();
            foreach (Transaction item in _transactions)
            {
                if (item.Date.Date.Month == month)
                    result.Add(item);
            }
            return result;
        }

        public IEnumerable<Transaction> GetAllOfYear(int year)
        {
            var result = new List<Transaction>();
            foreach (Transaction item in _transactions)
            {
                if (item.Date.Date.Year == year)
                    result.Add(item);
            }
            return result;
        }

        public void Add(Transaction transaction)
        {
            transaction.ID = _transactions.Count + 1;
            _transactions.Add(transaction);
            SaveTransactions();
        }

        public void Update(Transaction transaction)
        {
            var existingTransactions = _transactions.FirstOrDefault(t => t.ID == transaction.ID);
            if (existingTransactions != null)
            {
                existingTransactions.Name = transaction.Name;
                existingTransactions.Date = transaction.Date;
                existingTransactions.Amount = transaction.Amount;
                existingTransactions.Kind = transaction.Kind;
                if(existingTransactions != null)
                {
                    existingTransactions.Category = transaction.Category;
                }

                SaveTransactions();
            }
        }

        public void Delete(Transaction transaction)
        {
            _transactions.RemoveAll(t => t.ID == transaction.ID);
            SaveTransactions();
        }

        public void Delete(int id)
        {
            _transactions.RemoveAll(t => t.ID == id);
            SaveTransactions();
        }

        private void SaveTransactions()
        {
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(_transactions));
        }
    }
}

