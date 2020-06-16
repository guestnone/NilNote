using System;
using System.Collections.Generic;
using System.Text;

namespace NilNote.Core
{
    public static class StaticStuff
    {
        public static string GetStartText()
        {
            return "## This is your first NilNote Page\r\n\r\nPlease edit me or delete if you don't need me";
        }


        public static string ToIETFCode(Language language)
        {
            switch (language)
            {
                case Language.English:
                    return "en-US";
                case Language.Polish:
                    return "pl-PL";
                case Language.Japanese:
                    return "ja-JP";
                default:
                    throw new ArgumentOutOfRangeException(nameof(language), language, null);
            }
        }

    }
}
