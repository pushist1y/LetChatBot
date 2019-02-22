using System;
using System.Collections.Generic;
using System.Text;

namespace LetChatBot
{
    public class TelegramOptions
    {
        public string TelegramBotToken { get; set; }
        public string DefaultGroupId { get; set; }

        public string ProxyAddress { get; set; }
        public int ProxyPort { get; set; }
        public string ProxyUsername { get; set; }
        public string ProxyPassword { get; set; }
    }
}
