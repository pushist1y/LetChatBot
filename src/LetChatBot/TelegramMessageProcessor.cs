using System;
using System.Linq;
using System.Text.RegularExpressions;
using LetChatBot.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace LetChatBot
{
    public class TelegramMessageProcessor
    {
        private TelegramBotClient _client;
        private readonly ForumContext _context;
        private readonly IConfigurationRoot _config;
        private readonly int _forumBotUserId;
        private readonly long _defaultGroupId;
        public TelegramMessageProcessor(ForumContext context, IConfigurationRoot config)
        {
            _context = context;
            _config = config;
            _forumBotUserId = Convert.ToInt32(config["ForumBotUserId"]);
            _defaultGroupId = Convert.ToInt64(config["DefaultGroupId"]);
        }

        public void SetClient(TelegramBotClient client)
        {
            if (client == null)
            {
                throw new ArgumentNullException("Telegram client can't be null", nameof(client));
            }
            _client = client;
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
                    TelegramToForum(message.From.FullName(), message.From.Id, message.Text);
                }
            }
        }

        private void TelegramToForum(string telegramName, long telegramId, string text)
        {
                text = text.ConvertToForum();
                var user = _context.PhpbbUsers.AsNoTracking().FirstOrDefault(u => u.UserTelegramId == telegramId);
                if (user == null)
                {
                    user = _context.PhpbbUsers.AsNoTracking().First(u => u.UserId == _forumBotUserId);
                    text = $"T({telegramName}): {text}";
                }

                SendToForum(user, text);
        }

        private void SendToForum(string text, int forumUserId = -1)
        {
            if (forumUserId < 0)
            {
                forumUserId = _forumBotUserId;
            }

            var user = _context.PhpbbUsers.AsNoTracking().FirstOrDefault(u => u.UserId == forumUserId);
            if (user == null)
            {
                user = _context.PhpbbUsers.AsNoTracking().First(u => u.UserId == _forumBotUserId);
            }

            SendToForum(user, text);
        }

        private void SendToForum(PhpbbUsers user, string text)
        {
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

            _context.PhpbbChat.Add(forumMessage);
            _context.SaveChanges();
        }
    }
}