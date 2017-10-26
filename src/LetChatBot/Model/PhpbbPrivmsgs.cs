using System;
using System.Collections.Generic;

namespace LetChatBot.Model
{
    public partial class PhpbbPrivmsgs
    {
        public int MsgId { get; set; }
        public int RootLevel { get; set; }
        public int AuthorId { get; set; }
        public int IconId { get; set; }
        public string AuthorIp { get; set; }
        public int MessageTime { get; set; }
        public sbyte EnableBbcode { get; set; }
        public sbyte EnableSmilies { get; set; }
        public sbyte EnableMagicUrl { get; set; }
        public sbyte EnableSig { get; set; }
        public string MessageSubject { get; set; }
        public string MessageText { get; set; }
        public string MessageEditReason { get; set; }
        public int MessageEditUser { get; set; }
        public sbyte MessageAttachment { get; set; }
        public string BbcodeBitfield { get; set; }
        public string BbcodeUid { get; set; }
        public int MessageEditTime { get; set; }
        public short MessageEditCount { get; set; }
        public string ToAddress { get; set; }
        public string BccAddress { get; set; }
        public sbyte MessageReported { get; set; }
    }
}
