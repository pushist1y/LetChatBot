using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using LetChatBot.Model;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace LetChatBot
{
    public class LetChatBot
    {
        private TelegramBotClient _client;
        private DatabaseChatPoller _dbPoller;
        private readonly ForumUserStore _userStore;
        private readonly ForumMessageStore _messageStore;

        private readonly string _token;
        private readonly int _forumBotUserId;
        private readonly long _defaultGroupId;
        public LetChatBot(IConfigurationRoot config, DatabaseChatPoller dbPoller, ForumUserStore userStore,
                        ForumMessageStore messageStore)
        {
            _token = config["TelegramBotToken"];
            _forumBotUserId = Convert.ToInt32(config["ForumBotUserId"]);
            _defaultGroupId = Convert.ToInt64(config["DefaultGroupId"]);
            _client = new TelegramBotClient(_token);
            _dbPoller = dbPoller;
            _userStore = userStore;
            _messageStore = messageStore;
            _dbPoller.DatabaseMessageReceived += OnDatabaseMessageReceived;
            _client.OnMessage += OnTelegramMessageReceived;
        }

        private void OnTelegramMessageReceived(object sender, MessageEventArgs q)
        {
            var m = q.Message;
            if(m.Type == MessageType.TextMessage)
            {
                var telegramName = m.From.FirstName;
                if(!string.IsNullOrEmpty(m.From.LastName))
                {
                    telegramName += " " + m.From.LastName;
                }

                var forumId = 0; //find it

                var user = _userStore.Users.FirstOrDefault(u => u.UserId == (forumId > 0? forumId:_forumBotUserId));

                var forumMessage = new PhpbbChat();
                forumMessage.Message = WebUtility.HtmlEncode(m.Text);
                forumMessage.UserId = user.UserId;
                forumMessage.Username = user.Username;
                forumMessage.UserColour = user.UserColour;
                forumMessage.BbcodeBitfield = string.Empty;
                forumMessage.BbcodeUid = string.Empty;
                forumMessage.BbcodeOptions = true;
                forumMessage.TelegramProcessed = true;
                
                forumMessage.Time = (new DateTimeOffset(DateTime.Now)).ToUnixTimeSeconds();
                forumMessage.ChatId = 1;

                _messageStore.AddMessage(forumMessage);


            }
            Console.WriteLine(q.Message.Text);
        }

        private void OnDatabaseMessageReceived(object sender, DatabaseMessageReceivedArgs e) 
        {
            Console.WriteLine($"{e.Message.Username}: {e.Message.Message}");

            e.Message.FilteredText = Regex.Replace(e.Message.Message, "<a.*?href=\\\"(.*?)\\\".*?>(.*?)<\\/a>", "$1");
            e.Message.FilteredText = Regex.Replace(e.Message.FilteredText, "<!--[^>]*>[^<]*<img[^>]*alt=\\\"(.*?)\\\"[^>]*>[^>]*-->", "$1");
            e.Message.FilteredText = WebUtility.HtmlDecode(Regex.Replace(e.Message.FilteredText, "<[^>]*(>|$)", string.Empty));
            if(e.Message.UserId != _forumBotUserId)
            {
                //TODO: link users logic
            }

            var text = $"{e.Message.Username}: {e.Message.FilteredText}";
            var res = _client.SendTextMessageAsync(_defaultGroupId, text).Result;
            e.Message.TelegramProcessed = true;

        }

        public void Start()
        {
            _client.StartReceiving();
            _dbPoller.StartPolling();
        }

        public void Stop()
        {
            _client.StopReceiving();
            _dbPoller.StopPolling();
        }

    }
}
