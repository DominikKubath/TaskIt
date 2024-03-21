using NUnit.Framework;
using TaskIt;
using TaskIt.Classes;
using TaskIt.Interfaces;

namespace TaskIt_Test
{
    [TestFixture]
    public class FileJournalRepository_Test
    {
        IJournalRepository journalRepository;
        string journalsTestPath = "Test_Journals";
        string journalsJsonFile = "test_journals.json";
        string filePath;

        [OneTimeSetUp]
        public void BeforeAll()
        {
            filePath = journalsTestPath + "/" + journalsJsonFile;
        }

        [SetUp]
        public void BeforeEach()
        {
            File.Delete(journalsJsonFile);
            journalRepository = new FileJournalRepository(journalsJsonFile);
            
        }

        [TearDown]
        public void AfterEach()
        {
            File.Delete(journalsJsonFile);
        }

        [Test]
        public void GetAllJournals_AllJournalsRetrieved()
        {
            int quantity = 20;
            for(int i = 0; i < quantity; i ++)
            {
                Journal journal = new Journal(filePath, "Test Journal" + i, DateTime.Now);
                journalRepository.Add(journal);
            }

            var journals = journalRepository.GetAll().ToArray();
            Assert.That(journals.Count, Is.EqualTo(quantity));
            for(int i = 0; i < quantity; i++)
            {
                Assert.That(journals.ElementAt(i).Name, Is.EqualTo("Test Journal" + i));
            }
        }

        [Test]
        public void AddJournal_JournalIsAdded()
        {
            Journal journal = new Journal(filePath, "Test Journal", DateTime.Now);

            journalRepository.Add(journal);

            var journals = journalRepository.GetAll();
            Assert.That(journals.Count, Is.EqualTo(1));
            Assert.That(journals.First().Name, Is.EqualTo("Test Journal"));
        }

        [Test]
        public void DeleteJournal_JournalIsDeleted()
        {
            Journal journal = new Journal(filePath, "Test Journal", DateTime.Now);
            journalRepository.Add(journal);

            journalRepository.Delete("Test Journal");

            var journals = journalRepository.GetAll();
            Assert.That(journals.Count, Is.EqualTo(0));
        }

        [Test]
        public void UpdateLastChangedDate_DateIsUpdated()
        {
            DateTime oldDateValue = new DateTime(2000, 1, 1);
            Journal journal = new Journal(filePath, "Test Journal", oldDateValue);
            journalRepository.Add(journal);
            var journals = journalRepository.GetAll();
            Assert.That(journals.Count, Is.EqualTo(1));
            Assert.That(journals.First().LastChanged, Is.EqualTo(oldDateValue));

            journalRepository.UpdateLastChangedDate("Test Journal");

            journals = journalRepository.GetAll();
            Assert.That(journals.Count, Is.EqualTo(1));
            Assert.That(journals.First().LastChanged, Is.Not.EqualTo(oldDateValue));
        }

        [Test]
        public void GetByName_CorrectJournalRetrieved()
        {
            string testedName = "Some Journal";
            Journal journal = new Journal(filePath, "Test Journal", DateTime.Now);
            Journal journal2 = new Journal(filePath, testedName, DateTime.Now);
            Journal journal3 = new Journal(filePath, "Third Journal", DateTime.Now);
            journalRepository.Add(journal);
            journalRepository.Add(journal2);
            journalRepository.Add(journal3);

            var retrievedJournal = journalRepository.GetByName(testedName);

            Assert.NotNull(retrievedJournal);
            Assert.That(retrievedJournal.Name, Is.EqualTo(testedName));
        }

        [Test]
        public void GetByLastChangedDate_CorrectJournalRetrieved()
        {
            DateTime oldestDateValue = new DateTime(2000, 1, 1);
            DateTime oldDateValue = new DateTime(2020, 5, 20);
            DateTime newestDateValue = DateTime.Now;

            Journal journal = new Journal(filePath, "Test Journal", oldestDateValue);
            Journal journal2 = new Journal(filePath, "Some Journal", oldDateValue);
            Journal journal3 = new Journal(filePath, "Third Journal", newestDateValue);
            journalRepository.Add(journal);
            journalRepository.Add(journal2);
            journalRepository.Add(journal3);

            var retrievedJournal = journalRepository.GetByLastChangedDate(newestDateValue);

            Assert.NotNull(retrievedJournal);
            Assert.That(retrievedJournal.Name, Is.EqualTo("Third Journal"));
        }
    }
}
