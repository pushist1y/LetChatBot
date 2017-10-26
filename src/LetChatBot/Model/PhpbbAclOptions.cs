using System;
using System.Collections.Generic;

namespace LetChatBot.Model
{
    public partial class PhpbbAclOptions
    {
        public int AuthOptionId { get; set; }
        public string AuthOption { get; set; }
        public sbyte IsGlobal { get; set; }
        public sbyte IsLocal { get; set; }
        public sbyte FounderOnly { get; set; }
    }
}
