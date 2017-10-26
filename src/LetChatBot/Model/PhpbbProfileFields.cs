using System;
using System.Collections.Generic;

namespace LetChatBot.Model
{
    public partial class PhpbbProfileFields
    {
        public int FieldId { get; set; }
        public string FieldName { get; set; }
        public sbyte FieldType { get; set; }
        public string FieldIdent { get; set; }
        public string FieldLength { get; set; }
        public string FieldMinlen { get; set; }
        public string FieldMaxlen { get; set; }
        public string FieldNovalue { get; set; }
        public string FieldDefaultValue { get; set; }
        public string FieldValidation { get; set; }
        public sbyte FieldRequired { get; set; }
        public sbyte FieldShowOnReg { get; set; }
        public sbyte FieldHide { get; set; }
        public sbyte FieldNoView { get; set; }
        public sbyte FieldActive { get; set; }
        public int FieldOrder { get; set; }
        public sbyte FieldShowProfile { get; set; }
        public sbyte FieldShowOnVt { get; set; }
        public sbyte FieldShowNovalue { get; set; }
    }
}
