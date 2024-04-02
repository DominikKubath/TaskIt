using TaskIt.Classes;

namespace TaskIt.Interfaces
{
    public interface ITransactionPrinter
    {
        void PrintHeader();
        void PrintItems(IEnumerable<Transaction> items);
        void PrintFooter();
        void PrintFilterInstructions();
    }
}
