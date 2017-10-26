using System;
using System.Collections.Generic;

namespace LetChatBot.Model
{
    public partial class PhpbbStylesTheme
    {
        public int ThemeId { get; set; }
        public string ThemeName { get; set; }
        public string ThemeCopyright { get; set; }
        public string ThemePath { get; set; }
        public sbyte ThemeStoredb { get; set; }
        public int ThemeMtime { get; set; }
        public string ThemeData { get; set; }
    }
}
