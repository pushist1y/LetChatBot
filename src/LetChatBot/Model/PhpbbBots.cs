using System;
using System.Collections.Generic;

namespace LetChatBot.Model
{
    public partial class PhpbbBots
    {
        public int BotId { get; set; }
        public sbyte BotActive { get; set; }
        public string BotName { get; set; }
        public int UserId { get; set; }
        public string BotAgent { get; set; }
        public string BotIp { get; set; }
    }
}
