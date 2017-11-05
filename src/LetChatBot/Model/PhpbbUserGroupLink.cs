using System;
using System.Collections.Generic;

namespace LetChatBot.Model
{
    public partial class PhpbbUserGroupLink
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public sbyte GroupLeader { get; set; }
        public sbyte UserPending { get; set; }
    }
}
