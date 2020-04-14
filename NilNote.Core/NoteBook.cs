using System;
using System.Collections.Generic;

namespace NilNote.Core
{
    public enum NoteBookPageMarkupType
    {
        PlainText,
        Markdown
    }

    public struct NoteBookDetails
    {
        public string Name { get; set; }
        public string Creator { get; set; }
        public DateTime DateOfCreation { get; set; }
        public DateTime DateOfLastOpen { get; set; }
    }

    public struct NoteBookPage
    { 
        public string Name { get; set; }
        public string BookPath { get; set; }
        public DateTime DateOfCreation { get; set; }
        public DateTime DateOfLastModification { get; set; }
        public NoteBookPageMarkupType MarkupType { get; set; }
        public string Text { get; set; }
    }

    public class NoteBook
    {

        public Guid MUuid { get; set; }
        public NoteBookDetails MDetails { get; set; }
        public List<NoteBookPage> MPages { get; set; }
    }
}
