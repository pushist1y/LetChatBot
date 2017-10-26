using System;
using System.Collections.Generic;

namespace LetChatBot.Model
{
    public partial class PhpbbRanks
    {
        public int RankId { get; set; }
        public string RankTitle { get; set; }
        public int RankMin { get; set; }
        public sbyte RankSpecial { get; set; }
        public string RankImage { get; set; }
    }
}
