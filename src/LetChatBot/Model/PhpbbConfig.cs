using System;
using System.Collections.Generic;

namespace LetChatBot.Model
{
    public partial class PhpbbConfig
    {
        public string ConfigName { get; set; }
        public string ConfigValue { get; set; }
        public sbyte IsDynamic { get; set; }
    }
}
