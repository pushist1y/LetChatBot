using System;
using System.Collections.Generic;

namespace LetChatBot.Model
{
    public partial class PhpbbGroups
    {
        public int GroupId { get; set; }
        public sbyte GroupType { get; set; }
        public sbyte GroupFounderManage { get; set; }
        public string GroupName { get; set; }
        public string GroupDesc { get; set; }
        public string GroupDescBitfield { get; set; }
        public int GroupDescOptions { get; set; }
        public string GroupDescUid { get; set; }
        public sbyte GroupDisplay { get; set; }
        public string GroupAvatar { get; set; }
        public sbyte GroupAvatarType { get; set; }
        public short GroupAvatarWidth { get; set; }
        public short GroupAvatarHeight { get; set; }
        public int GroupRank { get; set; }
        public string GroupColour { get; set; }
        public int GroupSigChars { get; set; }
        public sbyte GroupReceivePm { get; set; }
        public int GroupMessageLimit { get; set; }
        public sbyte GroupLegend { get; set; }
        public int GroupMaxRecipients { get; set; }
        public sbyte GroupSkipAuth { get; set; }
    }
}
