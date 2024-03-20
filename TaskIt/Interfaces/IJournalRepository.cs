using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskIt.Classes;

namespace TaskIt.Interfaces
{
    public interface IJournalRepository
    {
        IEnumerable<Journal> GetAll();

        Journal GetByName(string name);
        Journal GetByLastChangedDate(DateTime date);
        
        void Add(Journal journal);
        void Delete(string name);
        void Select(string name);
    }
}
