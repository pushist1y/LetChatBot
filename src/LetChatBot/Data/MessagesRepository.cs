using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LetChatBot.Data;
using LetChatBot.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LetChatBot
{
    public class MessagesRepository
    {
        private readonly ForumContext _context;
        private readonly int _forumBotUserId;

        public MessagesRepository(ForumContext context, IConfiguration config)
        {
            _context = context;
            _forumBotUserId = Convert.ToInt32(config["ForumBotUserId"]);
        }

        public async Task<IList<PhpbbChat>> GetUnprocessedMessagesAsync(uint? fromId = null)
        {
            var messagesQuery = _context.PhpbbChat.Where(m => m.TelegramProcessed <= 0);
            if (fromId.HasValue)
            {
                messagesQuery = messagesQuery.Where(m => m.MessageId > fromId.Value);
            }

            var messages = await messagesQuery.OrderBy(m => m.MessageId).ToListAsync();

            return messages;
        }

        public async Task SetMessageProcessedAsync(IList<uint> messageIds)
        {
            var messages = await _context.PhpbbChat
                .Where(m => messageIds.Contains(m.MessageId))
                .ToListAsync();

            foreach (var msg in messages)
            {
                msg.TelegramProcessed = 1;
            }

            await _context.SaveChangesAsync();
        }

        public async Task SetMessageProcessedAsync(uint messageId)
        {
            await SetMessageProcessedAsync(new[] { messageId });
        }

        public async Task SaveMessageAsync(string text)
        {
            var user = _context.PhpbbUsers.FirstOrDefault(u => u.UserId == _forumBotUserId);
            var message = CreateMessage(user, text);
            await _context.PhpbbChat.AddAsync(message);
            await _context.SaveChangesAsync();
        }

        public async Task SaveMessageAsync(string telegramName, long telegramId, string text)
        {

            var user = _context.PhpbbUsers.FirstOrDefault(u => u.UserTelegramId == telegramId);
            if (user == null)
            {
                user = _context.PhpbbUsers.First(u => u.UserId == _forumBotUserId);
                text = $"T({telegramName}): {text}";
            }

            var message = CreateMessage(user, text);
            await _context.PhpbbChat.AddAsync(message);
            await _context.SaveChangesAsync();
        }

        private PhpbbChat CreateMessage(PhpbbUsers user, string text)
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

            forumMessage.Time = Convert.ToUInt32(new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds());
            forumMessage.ChatId = 1;

            return forumMessage;
        }

    }
}
