﻿using System;
using System.Collections.Generic;

namespace NilNote.Core
{

	public enum Language
	{
        English,
        Polish,
        Japanese
    };

	public enum NoteBookPageMarkupType
    {
        PlainText,
        Markdown
    }

    public struct NoteBookDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Creator { get; set; }
        public Language DefaultLanguage { get; set; }
        public NoteBookPageMarkupType DefaultMarkupType { get; set; }
        public DateTime DateOfCreation { get; set; }
        public DateTime DateOfLastOpen { get; set; }
    }

    public struct NoteBookPage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BookPath { get; set; }
        public DateTime DateOfCreation { get; set; }
        public DateTime DateOfLastModification { get; set; }
        public NoteBookPageMarkupType MarkupType { get; set; }
        public Language Language { get; set; }
        public string Text { get; set; }
    }

    public class NoteBook
    {
        public int Id { get; set; }
        public NoteBookDetails MDetails { get; set; }
        public List<NoteBookPage> MPages { get; set; }
    }
}
