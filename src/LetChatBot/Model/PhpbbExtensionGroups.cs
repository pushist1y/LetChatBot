using System;
using System.Collections.Generic;

namespace LetChatBot.Model
{
    public partial class PhpbbExtensionGroups
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public sbyte CatId { get; set; }
        public sbyte AllowGroup { get; set; }
        public sbyte DownloadMode { get; set; }
        public string UploadIcon { get; set; }
        public int MaxFilesize { get; set; }
        public string AllowedForums { get; set; }
        public sbyte AllowInPm { get; set; }
    }
}
