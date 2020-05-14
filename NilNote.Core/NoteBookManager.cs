using LiteDB;
using System;
using System.Collections.Generic;
using System.IO;
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
            mConfigPath = FileUtils.GetSaveLocationInAppDataFolder();
            mDbPath = mConfigPath + "/nb_" + Environment.UserName + ".db";
            if (File.Exists(mDbPath))
            {
                mDatabase = new LiteDatabase(mDbPath);
                mHasExistingDb = true;
            }
            mHasExistingDb = false;
        }

        private void SetNewUserDatabase(NoteBookDetails details)
        {
            mDatabase = new LiteDatabase(mDbPath);

        }
        

    }
}
