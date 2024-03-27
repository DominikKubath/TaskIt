namespace TaskIt.Classes
{
    public enum TransactionKind
    {
        Outgoing,
        Incoming
    }

    public class Transaction
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public TransactionKind Kind { get; set; }
        public DateTime Date { get; set; }

        //Category is the Budget to which the Transaction belongs to
        public Budget? Category {  get; set; }
        
        public Transaction(string name, double amount, TransactionKind kind, DateTime dateOfTransaction)
        {
            Name = name;
            Amount = amount;
            Kind = kind;
            Date = dateOfTransaction;
        }

        public Transaction()
        {
        }

        public Transaction(string name, double amount, TransactionKind kind, DateTime dateOfTransaction, Budget category) 
        {
            Name = name;
            Amount = amount;
            Kind = kind;
            Date = dateOfTransaction;
            Category = category;
        }
    }
}
