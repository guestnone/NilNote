using LiteDB;
using System;
using System.Collections.Generic;

namespace NilNote.Core
{

    static class NotebookDbNames
    {
        public static string NBDetailsCollectionName = "nbDetails";
        public static string NBPagesCollectionName = "nbPages";
        public static string NBTagsCollectionName = "nbTags";
    }

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

    public enum NoteBookExportFormat
    {
        HTML,
        PlainText
    }

    public enum NoteBookSearchMode
    {
        Content,
        Tags
    }

    public enum NoteBookDateSearchMode
    {
        DateOfCreation,
        DateOfLastEdit
    }


    public struct Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
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

        [BsonRef("nbTags")]
        public List<Tag> Tags { get; set; }
        public string Text { get; set; }
    }

    public class NoteBook
    {
        public int Id { get; set; }
        public NoteBookDetails MDetails { get; set; }
        public List<NoteBookPage> MPages { get; set; }
    }
}
