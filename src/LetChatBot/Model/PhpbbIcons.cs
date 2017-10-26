using System;
using System.Collections.Generic;

namespace LetChatBot.Model
{
    public partial class PhpbbIcons
    {
        public int IconsId { get; set; }
        public string IconsUrl { get; set; }
        public sbyte IconsWidth { get; set; }
        public sbyte IconsHeight { get; set; }
        public int IconsOrder { get; set; }
        public sbyte DisplayOnPosting { get; set; }
    }
}
