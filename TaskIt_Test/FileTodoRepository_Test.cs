using NUnit.Framework;
using TaskIt.Classes;
using TaskIt.Interfaces;

namespace TaskIt_Test
{
    [TestFixture]
    public class FileTodoRepository_Test
    {
        ITodoRepository todoRepository;

        [SetUp]
        public void BeforeEach()
        {
            File.Delete("test_todos.json");
            todoRepository = new FileTodoRepository("test_todos.json");
        }

        [TearDown]
        public void AfterEach()
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

        [Test]
        public void GetTodoById_CorrectTodoIsRetrieved()
        {
            TodoItem newTodo = new TodoItem("Test", "Description");
            todoRepository.Add(newTodo);

            var todo = todoRepository.GetById(1);

            Assert.NotNull(todo);
            Assert.That(todo.Name, Is.EqualTo("Test"));
        }

        [Test]
        public void GetTodosByPrio_CorrectTodosRetrieved()
        {
            TodoItem newTodo = new TodoItem("Test", "Description", 1);
            todoRepository.Add(newTodo);

            var todo = todoRepository.GetByPriority(1);

            Assert.NotNull(todo);
            Assert.That(todo.First().Name, Is.EqualTo("Test"));
        }

        [Test]
        public void GetTodosByPrio_InvalidPrioGiven_TodoHasDefaultPriority()
        {
            TodoItem newTodo = new TodoItem("Test", "Description", -50);
            todoRepository.Add(newTodo);

            var todo = todoRepository.GetAll();

            Assert.NotNull(todo);
            Assert.That(todo.First().Name, Is.EqualTo("Test"));
            Assert.That(Convert.ToInt32(todo.First().Prio), Is.EqualTo(5));
        }

        [Test]
        public void GetTodosCloseToDeadline_RelevantTodosRetrieved()
        {
            TodoItem newTodo1 = new TodoItem("Test1", "Description 1");
            TodoItem newTodo2 = new TodoItem("Test2", "Description 2");
            TodoItem newTodo3 = new TodoItem("Test3", "Description 3");
            DateTime nearDeadline = DateTime.Now + TimeSpan.FromDays(1);
            newTodo1.Deadline = nearDeadline;
            newTodo2.Deadline = nearDeadline;
            newTodo3.Deadline = nearDeadline;
            todoRepository.Add(newTodo1);
            todoRepository.Add(newTodo2);
            todoRepository.Add(newTodo3);

            var todos = todoRepository.GetTodosCloseToDeadline();

            Assert.NotNull(todos);
            Assert.That(todos.Count, Is.EqualTo(3));
        }
    }
}
