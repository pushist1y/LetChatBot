using System;
using System.Collections.Generic;

namespace LetChatBot.Model
{
    public partial class PhpbbConfirm
    {
        public string ConfirmId { get; set; }
        public string SessionId { get; set; }
        public sbyte ConfirmType { get; set; }
        public string Code { get; set; }
        public int Seed { get; set; }
        public int Attempts { get; set; }
    }
}
