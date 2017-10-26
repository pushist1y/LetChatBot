using System;
using System.Collections.Generic;

namespace LetChatBot.Model
{
    public partial class PhpbbProfileFieldsLang
    {
        public int FieldId { get; set; }
        public int LangId { get; set; }
        public int OptionId { get; set; }
        public sbyte FieldType { get; set; }
        public string LangValue { get; set; }
    }
}
