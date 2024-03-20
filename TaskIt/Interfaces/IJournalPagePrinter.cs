using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskIt.Classes;

namespace TaskIt.Interfaces
{
    internal interface IJournalPagePrinter
    {
        void PrintAllJournalPages(List<JournalPage> pages);
        void PrintInstructionPages();
    }
}
