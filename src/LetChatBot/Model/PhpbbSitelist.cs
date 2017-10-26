using System;
using System.Collections.Generic;

namespace LetChatBot.Model
{
    public partial class PhpbbSitelist
    {
        public int SiteId { get; set; }
        public string SiteIp { get; set; }
        public string SiteHostname { get; set; }
        public sbyte IpExclude { get; set; }
    }
}
