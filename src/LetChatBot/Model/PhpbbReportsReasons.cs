using System;
using System.Collections.Generic;

namespace LetChatBot.Model
{
    public partial class PhpbbReportsReasons
    {
        public short ReasonId { get; set; }
        public string ReasonTitle { get; set; }
        public string ReasonDescription { get; set; }
        public short ReasonOrder { get; set; }
    }
}
