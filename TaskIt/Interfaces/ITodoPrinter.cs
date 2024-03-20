using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskIt.Classes;

namespace TaskIt.Interfaces
{
    //IDEA: Ui should be flexible with the implementations -> it should be possible to
    // use web ui without much more work. Dependencies should be next to none.
    // Thats why this Adapter exists. So the Program doesnt need to care about how the info 
    // is displayed. Functionality stays more or less the same, only the way we take inputs and make outputs 
    // need to be flexible
    public interface ITodoPrinter
    {
        /*public void StartUI();
        public void PrintInstructions();
        public void AwaitCommand();
        public void CreateObject(string name);

        public void OpenObject(string name);
        
        public void DeleteObject(string name);*/
        void PrintInstructions();
        void PrintTodo(IEnumerable<TodoItem> todoItem);
    }

}
