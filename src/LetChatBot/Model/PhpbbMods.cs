using System;
using System.Collections.Generic;

namespace LetChatBot.Model
{
    public partial class PhpbbMods
    {
        public int ModId { get; set; }
        public sbyte ModActive { get; set; }
        public int ModTime { get; set; }
        public string ModDependencies { get; set; }
        public string ModName { get; set; }
        public string ModDescription { get; set; }
        public string ModVersion { get; set; }
        public string ModAuthorNotes { get; set; }
        public string ModAuthorName { get; set; }
        public string ModAuthorEmail { get; set; }
        public string ModAuthorUrl { get; set; }
        public string ModActions { get; set; }
        public string ModLanguages { get; set; }
        public string ModTemplate { get; set; }
        public string ModPath { get; set; }
        public string ModContribs { get; set; }
    }
}
