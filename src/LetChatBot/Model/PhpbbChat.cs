﻿using System;
using System.Collections.Generic;

namespace LetChatBot.Model
{
    public partial class PhpbbChat
    {
        public int MessageId { get; set; }
        public int ChatId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string UserColour { get; set; }
        public string Message { get; set; }
        public string BbcodeBitfield { get; set; }
        public string BbcodeUid { get; set; }
        public bool BbcodeOptions { get; set; }
        public int Time { get; set; }
        public int? Processed { get; set; }
        public bool TelegramProcessed { get; set; }
    }
}