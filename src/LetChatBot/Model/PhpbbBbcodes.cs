﻿using System;
using System.Collections.Generic;

namespace LetChatBot.Model
{
    public partial class PhpbbBbcodes
    {
        public short BbcodeId { get; set; }
        public string BbcodeTag { get; set; }
        public string BbcodeHelpline { get; set; }
        public sbyte DisplayOnPosting { get; set; }
        public string BbcodeMatch { get; set; }
        public string BbcodeTpl { get; set; }
        public string FirstPassMatch { get; set; }
        public string FirstPassReplace { get; set; }
        public string SecondPassMatch { get; set; }
        public string SecondPassReplace { get; set; }
    }
}
