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
        private readonly TelegramToForumUserLinker _userLinker;
        private readonly ForumContext _context;
        private readonly string _token;
        private readonly int _forumBotUserId;
        private readonly long _defaultGroupId;
        private readonly TelegramMessageProcessor _messageProcessor;
        public LetChatBot(IConfigurationRoot config, DatabaseChatPoller dbPoller, ForumContext context,
                        TelegramToForumUserLinker userLinker,
                        TelegramMessageProcessor messageProcessor)
        {
            _token = config["TelegramBotToken"];
            _forumBotUserId = Convert.ToInt32(config["ForumBotUserId"]);
            _defaultGroupId = Convert.ToInt64(config["DefaultGroupId"]);
            _client = new TelegramBotClient(_token);
            _dbPoller = dbPoller;
            _context = context;
            _userLinker = userLinker;
            _dbPoller.DatabaseMessageReceived += OnDatabaseMessageReceived;
            _client.OnMessage += OnTelegramMessageReceived;

            _userLinker.UserLinked += OnTelegramUserLinked;
            _userLinker.UserUnlinked += OnTelegramUserUnlinked;
            _messageProcessor = messageProcessor;
            _messageProcessor.SetClient(_client);
        }

        private void OnTelegramMessageReceived(object sender, MessageEventArgs q)
        {
            _messageProcessor.ProcessMessage(q.Message);
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
