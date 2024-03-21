using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TaskIt.Interfaces;

namespace TaskIt.Classes
{
    internal class FileJournalRepository : IJournalRepository
    {
        private readonly string _filePath;
        private List<Journal> _journals;

        public FileJournalRepository(string filepath) 
        {
            _filePath = filepath;

            _journals = File.Exists(_filePath)
            ? JsonConvert.DeserializeObject<List<Journal>>(File.ReadAllText(_filePath))
            : new List<Journal>();
        }

        public IEnumerable<Journal> GetAll() => _journals;

        public Journal GetByLastChangedDate(DateTime date)
        {
            return _journals.FirstOrDefault(j => j.LastChanged.Date == date.Date);
        }

        public Journal GetByName(string name)
        {
            var journal = _journals.FirstOrDefault(j => j.Name == name);
            if (journal == null)
            {
                throw new KeyNotFoundException("Das angegebene Journal wurde nicht gefunden.");
            }

            return journal;
        }

        public void UpdateLastChangedDate(string journalName)
        {
            GetByName(journalName).LastChanged = DateTime.Now;
        }

        public void Add(Journal journal)
        {
            _journals.Add(journal);
            SaveJournals();
        }

        public void Delete(string name)
        {
            var journal = GetByName(name);
            if (journal == null)
            {
                Console.WriteLine("Das Journal mit dem Namen " + name + " existiert nicht und kann daher nicht gelöscht werden.");
                return;
            }

            _journals.Remove(journal);
            SaveJournals();
        }

        private void SaveJournals()
        {
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(_journals));
        }
    }
}
