﻿using System;
using System.Collections.Generic;

namespace LetChatBot.Model
{
    public partial class PhpbbBanlist
    {
        public int BanId { get; set; }
        public int BanUserid { get; set; }
        public string BanIp { get; set; }
        public string BanEmail { get; set; }
        public int BanStart { get; set; }
        public int BanEnd { get; set; }
        public sbyte BanExclude { get; set; }
        public string BanReason { get; set; }
        public string BanGiveReason { get; set; }
    }
}
