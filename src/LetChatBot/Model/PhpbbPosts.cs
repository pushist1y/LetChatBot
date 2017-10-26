using System;
using System.Collections.Generic;

namespace LetChatBot.Model
{
    public partial class PhpbbPosts
    {
        public int PostId { get; set; }
        public int TopicId { get; set; }
        public int ForumId { get; set; }
        public int PosterId { get; set; }
        public int IconId { get; set; }
        public string PosterIp { get; set; }
        public int PostTime { get; set; }
        public int PostCreated { get; set; }
        public sbyte PostApproved { get; set; }
        public sbyte PostReported { get; set; }
        public sbyte EnableBbcode { get; set; }
        public sbyte EnableSmilies { get; set; }
        public sbyte EnableMagicUrl { get; set; }
        public sbyte EnableSig { get; set; }
        public string PostUsername { get; set; }
        public string PostSubject { get; set; }
        public string PostText { get; set; }
        public string PostChecksum { get; set; }
        public sbyte PostAttachment { get; set; }
        public string BbcodeBitfield { get; set; }
        public string BbcodeUid { get; set; }
        public sbyte PostPostcount { get; set; }
        public int PostEditTime { get; set; }
        public string PostEditReason { get; set; }
        public int PostEditUser { get; set; }
        public short PostEditCount { get; set; }
        public sbyte PostEditLocked { get; set; }
    }
}
