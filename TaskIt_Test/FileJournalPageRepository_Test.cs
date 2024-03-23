using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskIt;
using TaskIt.Classes;
using TaskIt.Interfaces;

namespace TaskIt_Test
{
    [TestFixture]
    public class FileJournalPageRepository_Test
    {
        IJournalPageRepository journalPageRepository;
        IJournalRepository journalRepository;
        ITodoRepository todoRepository;
        private const string testTodoPath = "test_todos.json";
        private const string journalsTestPath = "Test_Journals";
        private const string journalsJsonFile = "test_journals.json";
        private string filePath;

        Journal testJournal;

        [OneTimeSetUp]
        public void BeforeAll()
        {
            filePath = journalsTestPath + "/" + journalsJsonFile;
            if(File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            if(Directory.Exists(journalsTestPath))
            {
                Directory.Delete(journalsTestPath, true);
            }
            journalRepository = new FileJournalRepository(journalsJsonFile);
            todoRepository = new FileTodoRepository(testTodoPath);
            journalPageRepository = new FileJournalPageRepository(todoRepository, journalsTestPath);
            testJournal = new Journal(filePath, "Test Journal", DateTime.Now);
        }

        [TearDown]
        public void AfterEach()
        {
            File.Delete(testTodoPath);
            File.Delete(journalsJsonFile);
            if(Directory.Exists(journalsTestPath))
            {
                Directory.Delete(journalsTestPath, true);
            }
        }

        [Test]
        public void GetAllPagesOfJournal_AllPagesReceived()
        {
            JournalPage page1 = new JournalPage("Test Page 1", "Test Content", DateTime.Now);
            JournalPage page2 = new JournalPage("Page 2", "Example Content", DateTime.Now);
            JournalPage page3 = new JournalPage("Test 3", "Test Something", DateTime.Now);
            journalPageRepository.Add(testJournal.Name, page1);
            journalPageRepository.Add(testJournal.Name, page2);
            journalPageRepository.Add(testJournal.Name, page3);

            var journalPages = journalPageRepository.GetAllPages(testJournal.Name);

            Assert.That(journalPages.Count, Is.EqualTo(3));
        }

        [Test]
        public void AddPageToJournal_PageIsAdded()
        {
            JournalPage page1 = new JournalPage("Test Page 1", "Test Content", DateTime.Now);
            journalPageRepository.Add(testJournal.Name, page1);

            var journalPage = journalPageRepository.GetAllPages(testJournal.Name);

            Assert.That(journalPage.Count, Is.EqualTo(1));
            Assert.That(journalPage.First().Name, Is.EqualTo("Test Page 1"));
        }

        [Test]
        public void UpdatePage_PageIsUpdated()
        {
            JournalPage page1 = new JournalPage("Test Page 1", "Test Content", DateTime.Now);
            journalPageRepository.Add(testJournal.Name, page1);
            var journalPage = journalPageRepository.GetAllPages(testJournal.Name);
            Assert.That(journalPage.Count, Is.EqualTo(1));

            string updatedContent = "Some Test Content @Task@ to do";
            page1.Content = updatedContent;
            journalPageRepository.Update(testJournal.Name, page1);
            journalPage = journalPageRepository.GetAllPages(testJournal.Name);

            Assert.That(journalPage.Count, Is.EqualTo(1));
            Assert.That(journalPage.First().Content, Is.EqualTo(updatedContent));
            Assert.That(journalPage.First().TodoItems.Count, Is.EqualTo(1));
            Assert.That(journalPage.First().TodoItems.First().Name, Is.EqualTo("Task"));
        }

        [Test]
        public void DeletePage_PageIsDeleted()
        {
            JournalPage page1 = new JournalPage("Test Page 1", "Test Content", DateTime.Now);
            journalPageRepository.Add(testJournal.Name, page1);
            var journalPage = journalPageRepository.GetAllPages(testJournal.Name);
            Assert.That(journalPage.Count, Is.EqualTo(1));

            journalPageRepository.Delete(testJournal.Name, "Test Page 1");
            journalPage = journalPageRepository.GetAllPages(testJournal.Name);

            Assert.That(journalPage.Count, Is.EqualTo(0));
        }

    }
}
