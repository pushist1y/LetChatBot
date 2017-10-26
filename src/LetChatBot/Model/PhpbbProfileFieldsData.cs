using System;
using System.Collections.Generic;

namespace LetChatBot.Model
{
    public partial class PhpbbProfileFieldsData
    {
        public int UserId { get; set; }
        public string PfCharacters { get; set; }
        public string PfCharacterNames { get; set; }
        public int? PfParty { get; set; }
        public string PfSkype { get; set; }
        public sbyte? PfChatIcq { get; set; }
        public string PfTelegram { get; set; }
    }
}
