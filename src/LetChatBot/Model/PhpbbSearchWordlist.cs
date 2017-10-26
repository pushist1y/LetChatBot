using System;
using System.Collections.Generic;

namespace LetChatBot.Model
{
    public partial class PhpbbSearchWordlist
    {
        public int WordId { get; set; }
        public string WordText { get; set; }
        public sbyte WordCommon { get; set; }
        public int WordCount { get; set; }
    }
}
