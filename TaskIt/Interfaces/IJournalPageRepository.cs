using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskIt.Classes;

namespace TaskIt.Interfaces
{
    public interface IJournalPageRepository
    {
        List<JournalPage> GetAllPages(string journalName);
        void Add(string journalName, JournalPage page);
        void Delete(string journalName, string pageName);

    }
}
