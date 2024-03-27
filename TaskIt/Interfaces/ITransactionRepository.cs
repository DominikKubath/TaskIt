using TaskIt.Classes;

namespace TaskIt.Interfaces
{
    public interface ITransactionRepository
    {
        IEnumerable<Transaction> GetAll();
        Transaction GetById(int id);
        IEnumerable<Transaction> GetAllOutgoing();
        IEnumerable<Transaction> GetAllIncoming();
        IEnumerable<Transaction> GetAllOfLastSevenDays();
        IEnumerable<Transaction> GetAllOfCategory(string category);
        IEnumerable<Transaction> GetAllOfMonth(int month);
        IEnumerable<Transaction> GetAllOfYear(int year);
        void Add(Transaction transaction);
        void Update(Transaction transaction);
        void Delete(Transaction transaction);
        void Delete(int id);
    }
}
