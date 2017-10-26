using System;
using System.Collections.Generic;

namespace LetChatBot.Model
{
    public partial class PhpbbModules
    {
        public int ModuleId { get; set; }
        public sbyte ModuleEnabled { get; set; }
        public sbyte ModuleDisplay { get; set; }
        public string ModuleBasename { get; set; }
        public string ModuleClass { get; set; }
        public int ParentId { get; set; }
        public int LeftId { get; set; }
        public int RightId { get; set; }
        public string ModuleLangname { get; set; }
        public string ModuleMode { get; set; }
        public string ModuleAuth { get; set; }
    }
}
