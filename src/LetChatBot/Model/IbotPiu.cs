using System;
using System.Collections.Generic;

namespace LetChatBot.Model
{
    public partial class IbotPiu
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Author { get; set; }
        public string DateCreate { get; set; }
        public int? U { get; set; }
        public int? N { get; set; }
    }
}
