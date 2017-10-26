using System;
using System.Collections.Generic;

namespace LetChatBot.Model
{
    public partial class PhpbbChatSessions
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string UserColour { get; set; }
        public int UserLogin { get; set; }
        public int UserFirstpost { get; set; }
        public int UserLastpost { get; set; }
        public int UserLastupdate { get; set; }
    }
}
