using System;
using System.Linq;
using System.Text.RegularExpressions;
using LetChatBot.Model;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace LetChatBot
{
    public class TelegramMessageProcessor
    {
        private readonly TelegramBotClient _client;
        private readonly ForumUserStore _userStore;
        private readonly ForumMessageStore _messageStore;
        private readonly IConfigurationRoot _config;
        private readonly int _forumBotUserId;
        private readonly long _defaultGroupId;
        public TelegramMessageProcessor(TelegramBotClient client, ForumUserStore userStore, ForumMessageStore messageStore, IConfigurationRoot config)
        {
            _client = client;
            _messageStore = messageStore;
            _userStore = userStore;
            _config = config;
            _forumBotUserId = Convert.ToInt32(config["ForumBotUserId"]);
            _defaultGroupId = Convert.ToInt64(config["DefaultGroupId"]);
        }

        public void ProcessMessage(Message message)
        {
            if (message.Type == MessageType.TextMessage)
            {
                var match = Regex.Match(message.Text, "^(?:\\/([a-z0-9_]+)(@[a-z0-9_]+)?(?:\\s+(.*))?)$");
                if (match.Success)
                {
                    var command = match.Groups[1].Value;
                    var owner = match.Groups[2].Value;
                    var commandParams = match.Groups[3].Value;
                    //telegram command
                    return;
                }
                if (message.Chat.Id == _defaultGroupId)
                {
                    SendToForum(message.From.FullName(), message.From.Id, message.Text);
                }
            }
        }

        private void SendToForum(string telegramName, long telegramId, string text)
        {
            text = text.ConvertToForum();
            var user = _userStore.Users.FirstOrDefault(u => u.UserTelegramId == telegramId);
            if (user == null)
            {
                user = _userStore.Users.First(u => u.UserId == _forumBotUserId);
                text = $"T({telegramName}): {text}";
            }

            var forumMessage = new PhpbbChat();
            forumMessage.Message = text;
            forumMessage.UserId = user.UserId;
            forumMessage.Username = user.Username;
            forumMessage.UserColour = user.UserColour;
            forumMessage.BbcodeBitfield = string.Empty;
            forumMessage.BbcodeUid = string.Empty;
            forumMessage.BbcodeOptions = 7;
            forumMessage.TelegramProcessed = 1;

            forumMessage.Time = (new DateTimeOffset(DateTime.Now)).ToUnixTimeSeconds();
            forumMessage.ChatId = 1;

            _messageStore.AddMessage(forumMessage);
        }
    }

    public class TelegramMessageProcessorFactory
    {
        private readonly ForumUserStore _userStore;
        private readonly ForumMessageStore _messageStore;
        private readonly IConfigurationRoot _config;

        public TelegramMessageProcessorFactory(ForumUserStore userStore, ForumMessageStore messageStore, IConfigurationRoot config)
        {
            _messageStore = messageStore;
            _userStore = userStore;
            _config = config;
        }



        public TelegramMessageProcessor GetProcessor(TelegramBotClient client)
        {
            return new TelegramMessageProcessor(client, _userStore, _messageStore, _config);
        }
    }
}