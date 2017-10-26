using System;
using System.Collections.Generic;

namespace LetChatBot.Model
{
    public partial class PhpbbMycalendar
    {
        public int CalId { get; set; }
        public int TopicId { get; set; }
        public DateTime CalDate { get; set; }
        public sbyte CalInterval { get; set; }
        public sbyte CalRepeat { get; set; }
        public int ForumId { get; set; }
    }
}
