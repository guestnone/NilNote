using System;
using System.Collections.Generic;
using System.Text;

namespace NilNote.UI
{
    static class Utility
    {
        public static int CountWords(string text)
        {
            if (String.IsNullOrEmpty(text))
            {
                return 0;
            }
            int wordCount = 0, index = 0;

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

            return wordCount;
        }
    }
}
