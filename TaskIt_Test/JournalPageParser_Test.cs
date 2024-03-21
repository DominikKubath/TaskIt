using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskIt.Classes;
using TaskIt.Interfaces;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace TaskIt_Test
{
    [TestFixture]
    public class JournalPageParser_Test
    {
        [Test]
        public void ParseContent_NewTodosAdded_TodosAreAdded()
        {
            string testContent = "Test @Aufgabe1@ Lorem Ipsum @Aufgabe2@ @Aufgabe3@ bla blub";

            JournalPageParser parser = new JournalPageParser();
            List<TodoItem> foundItems = parser.ParseTodos(testContent);

            Assert.That(foundItems.Count, Is.EqualTo(3));
            Assert.That(foundItems.ElementAt(0).Name, Is.EqualTo("Aufgabe1"));
        }

        [Test]
        public void ParseContentWithEmails_EmailsAreIgnored()
        {
            string testContent = "This is an email: test@mail.com and another one: jon@gmail.com @Task1@ is recognized";

            JournalPageParser parser = new JournalPageParser();
            List<TodoItem> foundItems = parser.ParseTodos(testContent);

            Assert.That(foundItems.Count, Is.EqualTo(1));
            Assert.That(foundItems.First().Name, Is.EqualTo("Task1"));
        }
    }
}
