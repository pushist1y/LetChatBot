using System;
using System.Collections.Generic;

namespace LetChatBot.Model
{
    public partial class PhpbbReports
    {
        public int ReportId { get; set; }
        public short ReasonId { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public sbyte UserNotify { get; set; }
        public sbyte ReportClosed { get; set; }
        public int ReportTime { get; set; }
        public string ReportText { get; set; }
        public int PmId { get; set; }
    }
}
