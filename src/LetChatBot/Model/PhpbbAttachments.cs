using System;
using System.Collections.Generic;

namespace LetChatBot.Model
{
    public partial class PhpbbAttachments
    {
        public int AttachId { get; set; }
        public int PostMsgId { get; set; }
        public int TopicId { get; set; }
        public sbyte InMessage { get; set; }
        public int PosterId { get; set; }
        public sbyte IsOrphan { get; set; }
        public string PhysicalFilename { get; set; }
        public string RealFilename { get; set; }
        public int DownloadCount { get; set; }
        public string AttachComment { get; set; }
        public string Extension { get; set; }
        public string Mimetype { get; set; }
        public int Filesize { get; set; }
        public int Filetime { get; set; }
        public sbyte Thumbnail { get; set; }
    }
}
