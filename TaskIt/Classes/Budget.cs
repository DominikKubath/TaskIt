namespace TaskIt.Classes
{
    public class Budget
    {
        public string Name { get; set; }
        public double? Limit { get; set; }
        public Budget() 
        { 
        }

        public Budget(string name) 
        {
            Name = name;
        }
        public Budget(string name, double limit)
        {
            Name = name;
            Limit = limit;
        }
    }
}
