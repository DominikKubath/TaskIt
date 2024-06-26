﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskIt.Interfaces;
using Newtonsoft.Json;

namespace TaskIt.Classes
{
    public class FileJournalPageRepository : IJournalPageRepository
    {
        private readonly string _baseDirectory = "Journals/";
        private readonly IJournalPageRepository _journalPageRepository;

        private readonly ITodoRepository _todoRepository;

        public FileJournalPageRepository(ITodoRepository todoRepository, string filepath)
        {
            _todoRepository = todoRepository;
            _baseDirectory = filepath;
        }

        public List<JournalPage> GetAllPages(string journalName)
        {
            string journalDirectory = Path.Combine(_baseDirectory, journalName);

            Console.WriteLine("Hier ist die Seitenübersicht für das Journal " + journalName);

            if (!Directory.Exists(journalDirectory))
            {
                throw new DirectoryNotFoundException($"Das Verzeichnis für das Journal {journalName} wurde nicht gefunden.");
            }

            var pageFiles = Directory.GetFiles(journalDirectory, "*.json");
            var pages = new List<JournalPage>();
            foreach (var file in pageFiles)
            {
                var pageContent = File.ReadAllText(file);
                var page = JsonConvert.DeserializeObject<JournalPage>(pageContent);
                if (page != null)
                {
                    pages.Add(page);
                }
            }

            return pages;
        }

        public void Add(string journalName, JournalPage page)
        {
            string journalDirectory = Path.Combine(_baseDirectory, journalName);
            if (!Directory.Exists(journalDirectory))
            {
                Directory.CreateDirectory(journalDirectory);
            }

            string pageFile = Path.Combine(journalDirectory, $"{page.Name}.json");
            File.WriteAllText(pageFile, JsonConvert.SerializeObject(page));
        }

        public void Update(string journalName, JournalPage page)
        {
            string journalDirectory = Path.Combine(_baseDirectory, journalName);
            string pageFilePath = Path.Combine(journalDirectory, $"{page.Name}.json");

            if (!Directory.Exists(journalDirectory))
            {
                throw new DirectoryNotFoundException($"Das Verzeichnis für das Journal {journalName} wurde nicht gefunden.");
            }

            if (!File.Exists(pageFilePath))
            {
                throw new FileNotFoundException($"Die Seite \"{page.Name}\" wurde nicht gefunden.");
            }
            JournalPageParser parser = new JournalPageParser();
            if(page.Content != null)
            {
                page.TodoItems = parser.ParseTodos(page.Content);
                foreach (TodoItem item in page.TodoItems)
                {
                    if(!_todoRepository.IsTodoContained(item))
                        _todoRepository.Add(item);
                }
            }
            string updatedPageJson = JsonConvert.SerializeObject(page);

            File.WriteAllText(pageFilePath, updatedPageJson);
        }

        public void Delete(string journalName, string pageName)
        {
            string journalDirectory = Path.Combine(_baseDirectory, journalName);
            var pageFile = Directory.GetFiles(journalDirectory, $"{pageName}.json").FirstOrDefault();
            if (pageFile == null)
            {
                throw new FileNotFoundException("Die angegebene Seite wurde nicht gefunden.");
            }

            File.Delete(pageFile);
        }
    }
}
