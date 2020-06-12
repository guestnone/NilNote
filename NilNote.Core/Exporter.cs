using Markdig;
using SpoiledCat.NiceIO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace NilNote.Core
{
    public interface Exporter
    {
        void Start(string path);
        void ExportPage(NoteBookPage page);

        void ExportPages(List<NoteBookPage> pages);
        void End();
        void ExportPages(IList selectedItems);
    }

    public class HTMLExporter : Exporter
    {
        public void End()
        {
            // Does nothing
        }

        public void ExportPage(NoteBookPage page)
        {
            NPath file;
            if (mPath.FileExists(page.Name + ".html"))
            {
                file = new NPath(mStringPath + "/" + page.Name + ".html");
            }
            else
            {
                file = mPath.CreateFile(page.Name + ".html");
            }
            string output = "<html>\n";
            switch (page.MarkupType)
            {
                case NoteBookPageMarkupType.Markdown:
                    var result = Markdown.ToHtml(page.Text);
                    output += result;
                    break;
                case NoteBookPageMarkupType.PlainText:
                    output += page.Text;
                    break;
            }
            output += "\n</html>";
            file.WriteAllText(output);
        }

        public void ExportPages(List<NoteBookPage> pages)
        {
            foreach(var page in pages)
            {
                ExportPage(page);
            }
        }

        public void Start(string path)
        {
            mPath = new NPath(path);
            mStringPath = path;
            if (!mPath.DirectoryExists())
            {
                mPath.CreateDirectory();
            }
            mPath.DeleteContents();
        }

        public void ExportPages(IList selectedItems)
        {
            ExportPages(selectedItems.Cast<NoteBookPage>().ToList());
        }

        private NPath mPath;
        private string mStringPath;
    }

    public class PlainTextExporter : Exporter
    {
        public void End()
        {
            // Does nothing
        }

        public void ExportPage(NoteBookPage page)
        {
            NPath file;
            if (mPath.FileExists(page.Name + ".html"))
            {
                file = new NPath(mStringPath + "/" + page.Name + ".txt");
            }
            else
            {
                file = mPath.CreateFile(page.Name + ".txt");
            }
            file.WriteAllText(page.Text);
        }

        public void ExportPages(List<NoteBookPage> pages)
        {
            foreach (var page in pages)
            {
                ExportPage(page);
            }
        }

        public void Start(string path)
        {
            mPath = new NPath(path);
            mStringPath = path;
            if (!mPath.DirectoryExists())
            {
                mPath.CreateDirectory();
            }
            mPath.DeleteContents();
        }

        public void ExportPages(IList selectedItems)
        {
            ExportPages(selectedItems.Cast<NoteBookPage>().ToList());
        }

        private NPath mPath;
        private string mStringPath;
    }
}
