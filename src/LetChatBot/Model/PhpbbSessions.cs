using System;
using System.Collections.Generic;

namespace LetChatBot.Model
{
    public partial class PhpbbSessions
    {
        public string SessionId { get; set; }
        public int SessionUserId { get; set; }
        public int SessionLastVisit { get; set; }
        public int SessionStart { get; set; }
        public int SessionTime { get; set; }
        public string SessionIp { get; set; }
        public string SessionBrowser { get; set; }
        public string SessionForwardedFor { get; set; }
        public string SessionPage { get; set; }
        public sbyte SessionViewonline { get; set; }
        public sbyte SessionAutologin { get; set; }
        public sbyte SessionAdmin { get; set; }
        public int SessionForumId { get; set; }
    }
}
