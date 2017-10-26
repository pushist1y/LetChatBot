using System;
using System.Collections.Generic;

namespace LetChatBot.Model
{
    public partial class PhpbbZebra
    {
        public int UserId { get; set; }
        public int ZebraId { get; set; }
        public sbyte Friend { get; set; }
        public sbyte Foe { get; set; }
    }
}
