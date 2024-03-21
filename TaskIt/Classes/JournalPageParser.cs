using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TaskIt.Classes
{
    public class JournalPageParser
    {
        public List<TodoItem> ParseTodos(string content)
        {
            var todoItems = new List<TodoItem>();

            string pattern = @"(?<![^\s@])@([^@]+)@(?![^\s@])";
            var matches = Regex.Matches(content, pattern);

            foreach (Match match in matches)
            {
                string todoName = match.Groups[1].Value;

                var todoItem = new TodoItem(todoName, "");
                todoItems.Add(todoItem);
            }

            return todoItems;
        }
    }
}
