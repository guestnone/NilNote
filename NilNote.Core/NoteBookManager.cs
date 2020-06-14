using LiteDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NilNote.Core
{
    public sealed class NoteBookManager
    {
        private static NoteBookManager mInstance = null;

        private string mConfigPath;
        private string mDbPath;
        private bool mHasExistingDb = false;
        private LiteDatabase mDatabase;

        public bool HasExistingDB { get => mHasExistingDb; }

        private void InsertFirstTimeUserPages(Language lang)
        {
            NoteBookPage page = new NoteBookPage();
            page.BookPath = "";
            page.MarkupType = NoteBookPageMarkupType.PlainText;
            page.DateOfCreation = DateTime.Now;
            page.DateOfLastModification = DateTime.Now;
            page.Text = StaticStuff.GetStartText();
            page.Name = "Your First Page";
            page.Language = lang;
            InsertNewPage(page);
        }


        public static NoteBookManager Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new NoteBookManager();
                }
                return mInstance;
            }
        }

        private NoteBookManager()
        {
            mConfigPath = Environment.CurrentDirectory;
            mDbPath = mConfigPath + "/nb_" + Environment.UserName + ".db";
            if (File.Exists(mDbPath))
            {
                mDatabase = new LiteDatabase(mDbPath);
                if (mDatabase.GetCollectionNames().Contains(NotebookDbNames.NBDetailsCollectionName))
                {
                    mHasExistingDb = true;
                    return;
                }
            }
            mHasExistingDb = false;
        }

        public void CreateNewUserDatabase(NoteBookDetails details)
        {
            if (!mHasExistingDb)
            {
                mDatabase = new LiteDatabase(mDbPath);
            }

            var nbDetailsCollection = mDatabase.GetCollection<NoteBookDetails>(NotebookDbNames.NBDetailsCollectionName);
            nbDetailsCollection.Insert(details);
            InsertFirstTimeUserPages(details.DefaultLanguage);
        }

        public bool InsertNewPage(NoteBookPage page)
        {
            var collection = mDatabase.GetCollection<NoteBookPage>(NotebookDbNames.NBPagesCollectionName);
            var ifFound = collection.Query().Where(x => x.Name.StartsWith(page.Name));
            if (ifFound.Count() != 0)
            {
                return false;
            }
            collection.Insert(page);
            return true;

        }

        public IEnumerable<NoteBookPage> GetPages()
        {
            var collection = mDatabase.GetCollection<NoteBookPage>(NotebookDbNames.NBPagesCollectionName);
            return collection.FindAll();
        }

        public bool UpdatePage(NoteBookPage page)
        {
            return mDatabase.GetCollection<NoteBookPage>(NotebookDbNames.NBPagesCollectionName).Update(page);
        }

        public int GetPageNumber()
        {
            return mDatabase.GetCollection<NoteBookPage>(NotebookDbNames.NBPagesCollectionName).Count();
        }

        public int GetNoteBookWordCount()
        {
            int wordCount = 0, index = 0;
            var list = mDatabase.GetCollection<NoteBookPage>(NotebookDbNames.NBPagesCollectionName).FindAll();
            foreach (var page in list)
            {
                var text = page.Text;
                index = 0;
                if (!String.IsNullOrEmpty(text))
                {
                    while (index < text.Length && char.IsWhiteSpace(text[index]))
                        index++;

                    while (index < text.Length)
                    {
                        // check if current char is part of a word
                        while (index < text.Length && !char.IsWhiteSpace(text[index]))
                            index++;

                        wordCount++;

                        // skip whitespace until next word
                        while (index < text.Length && char.IsWhiteSpace(text[index]))
                            index++;
                    }
                }
            }

            return wordCount;
        }

        public NoteBookDetails GetDetails()
        {
            var collection = mDatabase.GetCollection<NoteBookDetails>(NotebookDbNames.NBDetailsCollectionName);
            var list = collection.FindAll();
            if (list.Count() != 1)
            {
                // TODO: Handle incorrect number of details
                return new NoteBookDetails();
            }
            return list.ElementAt(0);

        }

        public IEnumerable<Tag> GetTags()
        {
            var collection = mDatabase.GetCollection<Tag>(NotebookDbNames.NBTagsCollectionName);
            return collection.FindAll();
        }

        public bool TagExist(Tag tag)
        {
            var collection = mDatabase.GetCollection<Tag>(NotebookDbNames.NBTagsCollectionName);
            var ifFound = collection.Query().Where(x => x.Name.StartsWith(tag.Name));
            if (ifFound.Count() != 0)
            {
                return true;
            }
            return false;
        }

        public bool TagExist(string text)
        {
            var collection = mDatabase.GetCollection<Tag>(NotebookDbNames.NBTagsCollectionName);
            var ifFound = collection.Query().Where(x => x.Name.Contains(text));
            if (ifFound.Count() != 0)
            {
                return true;
            }
            return false;
        }

        public bool AddTag(Tag tag)
        {
            var collection = mDatabase.GetCollection<Tag>(NotebookDbNames.NBTagsCollectionName);
            if (TagExist(tag))
            {
                return false;
            }
            collection.Insert(tag);
            return true;
        }

        public bool UpdateTag(Tag tag)
        {
            return mDatabase.GetCollection<Tag>(NotebookDbNames.NBTagsCollectionName).Update(tag);
        }

        public bool RemoveTag(Tag tag)
        {
            if (!TagExist(tag))
            {
                return false;
            }

            // Remove all connections to the tag from pages
            List<NoteBookPage> toUpdate = new List<NoteBookPage>();
            var pages = GetPages();
            foreach (var page in pages)
            {
                if (page.Tags != null)
                {
                    if (page.Tags.Contains(tag))
                    {
                        page.Tags.Remove(tag);
                        toUpdate.Add(page);
                    }
                }
            }
            foreach (var page in toUpdate)
            {
                UpdatePage(page);
            }

            var collection = mDatabase.GetCollection<Tag>(NotebookDbNames.NBTagsCollectionName);
            return collection.Delete(tag.Id);
        }

        public IList<NoteBookPage> TextSearch(string text, NoteBookSearchMode mode)
        {
            var pageCollection = mDatabase.GetCollection<NoteBookPage>(NotebookDbNames.NBPagesCollectionName);
            List<NoteBookPage> result = null;
            switch (mode)
            {
                case NoteBookSearchMode.Content:
                    result = pageCollection.Find(x => x.Text.Contains(text)).ToList();
                    break;
                case NoteBookSearchMode.Tags:
                    var collection = mDatabase.GetCollection<Tag>(NotebookDbNames.NBTagsCollectionName);
                    var tags = collection.Find(x => x.Name.Contains(text));
                    result = new List<NoteBookPage>();
                    foreach (var tag in tags)
                    {
                        var pages = pageCollection.Find(x => x.Tags.Contains(tag));
                        foreach (var page in pages)
                        {
                            if (!result.Contains(page))
                            {
                                result.Add(page);
                            }
                        }
                    }

                    break;
                default:
                    result = new List<NoteBookPage>();
                    break;
            }
            return result;
        }

    }
}
