using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskIt.Interfaces;

namespace TaskIt.Classes
{
    public class ConsoleTodoPrinter : ITodoPrinter
    {
        public void PrintTodo(IEnumerable<TodoItem> items)
        {
            foreach (var item in items)
            {
                string deadline;
                if (item.Deadline == null)
                    deadline = "Keine Deadline";
                else
                    deadline = item.Deadline.ToString().Substring(0,10);
                Console.WriteLine($"ID: [{item.ID}] | Name: {item.Name} | Description: {item.Description} | Priority: {item.Prio} | Deadline: {deadline} -- {(item.IsCompleted ? "Completed" : "Not Completed")}");
            }
        }

        public void PrintInstructions()
        {
            Console.WriteLine("\r\n ______                  __      ______   __      \r\n/\\__  _\\                /\\ \\    /\\__  _\\ /\\ \\__   \r\n\\/_/\\ \\/    __      ____\\ \\ \\/'\\\\/_/\\ \\/ \\ \\ ,_\\  \r\n   \\ \\ \\  /'__`\\   /',__\\\\ \\ , <   \\ \\ \\  \\ \\ \\/  \r\n    \\ \\ \\/\\ \\L\\.\\_/\\__, `\\\\ \\ \\\\`\\  \\_\\ \\__\\ \\ \\_ \r\n     \\ \\_\\ \\__/.\\_\\/\\____/ \\ \\_\\ \\_\\/\\_____\\\\ \\__\\\r\n      \\/_/\\/__/\\/_/\\/___/   \\/_/\\/_/\\/_____/ \\/__/\r\n                                                  \r\n                                                  \r\n");
            Console.WriteLine("Willkommen bei TaskIt!");
            Console.WriteLine("Diese Anwendung ermöglicht es dir ein Journal zu führen");
            Console.WriteLine("und daraus automatisch deine Todo's rauslesen!");
            Console.WriteLine("Keine Sorge, du kannst auch manuell weitere Todo's hinzufügen, falls");
            Console.WriteLine("dir spontan etwas einfallen sollte ;D \n");
        }
    }

}
