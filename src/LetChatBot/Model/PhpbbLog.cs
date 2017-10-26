﻿using System;
using System.Collections.Generic;

namespace LetChatBot.Model
{
    public partial class PhpbbLog
    {
        public int LogId { get; set; }
        public sbyte LogType { get; set; }
        public int UserId { get; set; }
        public int ForumId { get; set; }
        public int TopicId { get; set; }
        public int ReporteeId { get; set; }
        public string LogIp { get; set; }
        public int LogTime { get; set; }
        public string LogOperation { get; set; }
        public string LogData { get; set; }
    }
}
