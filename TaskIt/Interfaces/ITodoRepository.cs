using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskIt.Classes;

namespace TaskIt.Interfaces
{
    public interface ITodoRepository
    {
        IEnumerable<TodoItem> GetAll();
        TodoItem GetById(int id);
        IEnumerable<TodoItem> GetByPriority(int priority);
        void Add(TodoItem todoItem);
        void Update(TodoItem todoItem);
        void Delete(int id);
    }
}
