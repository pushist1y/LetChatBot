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
        private readonly TelegramToForumUserLinker _userLinker;

        private readonly string _token;
        private readonly int _forumBotUserId;
        private readonly long _defaultGroupId;
        private readonly TelegramMessageProcessor _messageProcessor;
        public LetChatBot(IConfigurationRoot config, DatabaseChatPoller dbPoller, ForumUserStore userStore,
                        ForumMessageStore messageStore, TelegramToForumUserLinker userLinker,
                        TelegramMessageProcessorFactory messageProcessorFactory)
        {
            _token = config["TelegramBotToken"];
            _forumBotUserId = Convert.ToInt32(config["ForumBotUserId"]);
            _defaultGroupId = Convert.ToInt64(config["DefaultGroupId"]);
            _client = new TelegramBotClient(_token);
            _dbPoller = dbPoller;
            _userStore = userStore;
            _userLinker = userLinker;
            _messageStore = messageStore;
            _dbPoller.DatabaseMessageReceived += OnDatabaseMessageReceived;
            _client.OnMessage += OnTelegramMessageReceived;

            _userLinker.UserLinked += OnTelegramUserLinked;
            _userLinker.UserUnlinked += OnTelegramUserUnlinked;
            _messageProcessor = messageProcessorFactory.GetProcessor(_client);
        }

        private void OnTelegramMessageReceived(object sender, MessageEventArgs q)
        {
            _messageProcessor.ProcessMessage(q.Message);
            // var m = q.Message;
            // if(m.Type == MessageType.TextMessage)
            // {
            //     var telegramName = m.From.FirstName;
            //     if(!string.IsNullOrEmpty(m.From.LastName))
            //     {
            //         telegramName += " " + m.From.LastName;
            //     }

            //     var forumId = 0; //find it

            //     var user = _userStore.Users.FirstOrDefault(u => u.UserId == (forumId > 0? forumId:_forumBotUserId));

            //     var forumMessage = new PhpbbChat();
            //     forumMessage.Message = WebUtility.HtmlEncode(m.Text);
            //     forumMessage.UserId = user.UserId;
            //     forumMessage.Username = user.Username;
            //     forumMessage.UserColour = user.UserColour;
            //     forumMessage.BbcodeBitfield = string.Empty;
            //     forumMessage.BbcodeUid = string.Empty;
            //     forumMessage.BbcodeOptions = 7;
            //     forumMessage.TelegramProcessed = 1;
                
            //     forumMessage.Time = (new DateTimeOffset(DateTime.Now)).ToUnixTimeSeconds();
            //     forumMessage.ChatId = 1;

            //     _messageStore.AddMessage(forumMessage);


            // }
            // Console.WriteLine(q.Message.Text);
        }

        private void OnTelegramUserLinked(object sender, TelegramToForumLinkArgumentArgs e) 
        {
            var text = $"Вы успешно связали вашу учётную запись Telegram с аккаунтом [{e.ForumUser.Username}] на форуме";
            _client.SendTextMessageAsync(e.TelegramUserId, text).Wait();
        }

        private void OnTelegramUserUnlinked(object sender, TelegramToForumLinkArgumentArgs e) 
        {
            var text = "Вы успешно отвязали вашу учётную запись Telegram от аккаунта на форуме";
            _client.SendTextMessageAsync(e.TelegramUserId, text).Wait();
        }

        private void OnDatabaseMessageReceived(object sender, DatabaseMessageReceivedArgs e) 
        {
            Console.WriteLine($"{e.Message.Username}: {e.Message.Message}");

            _userLinker.ValidateAndLink(e.Message.UserId, e.Message.Message);
            

            var text = $"{e.Message.Username}: {e.Message.Message.ConvertToTelegram()}";
            var res = _client.SendTextMessageAsync(_defaultGroupId, text).Result;
            e.Message.TelegramProcessed = 1;

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
