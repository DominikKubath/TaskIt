using NUnit.Framework;
using TaskIt.Classes;
using TaskIt.Interfaces;

namespace TaskIt_Test
{
    [TestFixture]
    public class FileTodoRepository_Test
    {
        ITodoRepository todoRepository;
        ITodoPrinter todoPrinter;

        [OneTimeSetUp]
        public void BeforeAll()
        {
            todoRepository = new FileTodoRepository("test_todos.json");
            todoPrinter = new ConsoleTodoPrinter();
        }

        [OneTimeTearDown]
        public void AfterAll()
        {
            File.Delete("test_todos.json");
        }

        [Test]
        public void AddNewTodo_TodoIsAdded()
        {
            TodoItem newTodo = new TodoItem("Test", "Description");
            
            todoRepository.Add(newTodo);
        
            var todos = todoRepository.GetAll();
            Assert.That(todos.Count(), Is.EqualTo(1));
            Assert.That(todos.ElementAt(0).Name , Is.EqualTo("Test"));
        }

        [Test]
        public void RemoveTodo_TodoIsRemoved() 
        {
            TodoItem newTodo = new TodoItem("Test", "Description");
            todoRepository.Add(newTodo);

            var todos = todoRepository.GetAll();
            todoRepository.Delete(todos.First().ID);

            todos = todoRepository.GetAll();
            Assert.That(todos.Count(), Is.EqualTo(0));
        }

        [Test]
        public void UpdateTodo_TodoIsUpdated()
        {
            TodoItem newTodo = new TodoItem("Test", "Description");
            todoRepository.Add(newTodo);
            var todos = todoRepository.GetAll();
            todos.First().Name = "Changed Title";
            todos.First().Description = "New Desc";


            todoRepository.Update(todos.First());

            todos = todoRepository.GetAll();
            Assert.That(todos.First().Name, Is.EqualTo("Changed Title"));
            Assert.That(todos.First().Description, Is.EqualTo("New Desc"));
        }

    }
}
