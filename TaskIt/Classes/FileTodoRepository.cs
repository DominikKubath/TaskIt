using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TaskIt.Interfaces;
using static TaskIt.Classes.TodoItem;

namespace TaskIt.Classes
{
    public class FileTodoRepository : ITodoRepository
    {
        private readonly string _filePath;
        private List<TodoItem> _tasks;

        public FileTodoRepository(string filePath)
        {
            _filePath = filePath;

            _tasks = File.Exists(_filePath)
            ? JsonConvert.DeserializeObject<List<TodoItem>>(File.ReadAllText(_filePath))
            : new List<TodoItem>();
        }

        public IEnumerable<TodoItem> GetAll() => _tasks;

        public TodoItem GetById(int id) => _tasks.FirstOrDefault(t => t.ID == id);

        public IEnumerable<TodoItem> GetByPriority(int priority)
        {
            Priority priorityEnum = (Priority)priority;

            return _tasks.Where(t => t.Prio == priorityEnum);
        }

        public void Add(TodoItem todoItem)
        {
            todoItem.ID = _tasks.Count + 1;
            _tasks.Add(todoItem);
            SaveTodos();
        }

        public void Update(TodoItem todoItem)
        {
            var existingTodo = _tasks.FirstOrDefault(t => t.ID == todoItem.ID);
            if (existingTodo != null)
            {
                existingTodo.Name = todoItem.Name;
                existingTodo.Description = todoItem.Description;
                existingTodo.Prio = todoItem.Prio;
                existingTodo.IsCompleted = todoItem.IsCompleted;
                SaveTodos();
            }
        }

        public void Delete(int id)
        {
            _tasks.RemoveAll(t => t.ID == id);
            SaveTodos();
        }

        private void SaveTodos()
        {
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(_tasks));
        }

    }
}
