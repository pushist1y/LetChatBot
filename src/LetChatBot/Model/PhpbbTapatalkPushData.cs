using System;
using System.Collections.Generic;

namespace LetChatBot.Model
{
    public partial class PhpbbTapatalkPushData
    {
        public int PushId { get; set; }
        public string Author { get; set; }
        public int UserId { get; set; }
        public string DataType { get; set; }
        public string Title { get; set; }
        public int DataId { get; set; }
        public int CreateTime { get; set; }
        public int TopicId { get; set; }
    }
}
