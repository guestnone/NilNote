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

        ////////
        private readonly string NBDetailsCollectionName = "nbDetails";

        private readonly string NBPagesCollectionName = "nbPages";
        ////////

        private void InsertFirstTimeUserPages()
        {
            NoteBookPage page = new NoteBookPage();
            page.BookPath = "";
            page.MarkupType = NoteBookPageMarkupType.PlainText;
            page.DateOfCreation = DateTime.Now;
            page.DateOfLastModification = DateTime.Now;
            page.Text = StaticStuff.GetStartText();
            page.Name = "Your First Page";
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
                if (mDatabase.GetCollectionNames().Contains(NBDetailsCollectionName))
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

            var nbDetailsCollection = mDatabase.GetCollection<NoteBookDetails>(NBDetailsCollectionName);
            nbDetailsCollection.Insert(details);
            InsertFirstTimeUserPages();
        }

        public bool InsertNewPage(NoteBookPage page)
        {
            var collection = mDatabase.GetCollection<NoteBookPage>(NBPagesCollectionName);
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
            var collection = mDatabase.GetCollection<NoteBookPage>(NBPagesCollectionName);
            return collection.FindAll();
        }

        public bool UpdatePage(NoteBookPage page)
        {
            return mDatabase.GetCollection<NoteBookPage>(NBPagesCollectionName).Update(page);
        }

        public int GetPageNumber()
        {
            return mDatabase.GetCollection<NoteBookPage>(NBPagesCollectionName).Count();
        }

        public int GetNoteBookWordCount()
        {
            int wordCount = 0, index = 0;
            var list = mDatabase.GetCollection<NoteBookPage>(NBPagesCollectionName).FindAll();
            foreach (var page in list)
            {
                var text = page.Text;
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

            return wordCount;
        }

        public NoteBookDetails GetDetails()
        {
            var collection = mDatabase.GetCollection<NoteBookDetails>(NBDetailsCollectionName);
            var list = collection.FindAll();
            if (list.Count() != 1)
            {
                // TODO: Handle incorrect number of details
                return new NoteBookDetails();
            }
            return list.ElementAt(0);

        }

    }
}
