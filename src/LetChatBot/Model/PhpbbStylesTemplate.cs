using System;
using System.Collections.Generic;

namespace LetChatBot.Model
{
    public partial class PhpbbStylesTemplate
    {
        public int TemplateId { get; set; }
        public string TemplateName { get; set; }
        public string TemplateCopyright { get; set; }
        public string TemplatePath { get; set; }
        public string BbcodeBitfield { get; set; }
        public sbyte TemplateStoredb { get; set; }
        public int TemplateInheritsId { get; set; }
        public byte[] TemplateInheritPath { get; set; }
    }
}
