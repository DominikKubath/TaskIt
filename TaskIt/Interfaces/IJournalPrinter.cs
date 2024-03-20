using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskIt.Classes;

namespace TaskIt.Interfaces
{
    public interface IJournalPrinter
    {
        void PrintTableOfContents(IEnumerable<Journal> journals);
        void PrintInstructionJournal();
    }
}
