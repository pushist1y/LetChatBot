using System;
using System.Collections.Generic;

namespace LetChatBot.Model
{
    public partial class PhpbbTopicsPosted
    {
        public int UserId { get; set; }
        public int TopicId { get; set; }
        public sbyte TopicPosted { get; set; }
    }
}
