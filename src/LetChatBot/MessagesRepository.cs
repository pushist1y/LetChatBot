using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LetChatBot.Data;
using LetChatBot.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LetChatBot
{
    public class MessagesRepository
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly int _forumBotUserId;

        public MessagesRepository(IServiceProvider serviceProvider, IConfiguration config)
        {
            _serviceProvider = serviceProvider;
            _forumBotUserId = Convert.ToInt32(config["ForumBotUserId"]);
        }

        public async Task<IList<PhpbbChat>> GetUnprocessedMessagesAsync(uint? fromId = null)
        {
            var scopeFactory = _serviceProvider.GetRequiredService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ForumContext>();
                var messagesQuery = context.PhpbbChat.Where(m => m.TelegramProcessed <= 0);
                if (fromId.HasValue)
                {
                    messagesQuery = messagesQuery.Where(m => m.MessageId > fromId.Value);
                }

                var messages = await messagesQuery.OrderBy(m => m.MessageId).ToListAsync();

                return messages;
            }
        }

        public async Task SetMessageProcessedAsync(IList<uint> messageIds)
        {
            var scopeFactory = _serviceProvider.GetRequiredService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ForumContext>();
                var messages = await context.PhpbbChat
                    .Where(m => messageIds.Contains(m.MessageId))
                    .ToListAsync();

                foreach (var msg in messages)
                {
                    msg.TelegramProcessed = 1;
                }

                await context.SaveChangesAsync();

            }
        }

        public async Task SetMessageProcessedAsync(uint messageId)
        {
            await SetMessageProcessedAsync(new[] { messageId });
        }

        public async Task SaveMessageAsync(string telegramName, long telegramId, string text)
        {
            var scopeFactory = _serviceProvider.GetRequiredService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ForumContext>();

                var user = context.PhpbbUsers.FirstOrDefault(u => u.UserTelegramId == telegramId);
                if (user == null)
                {
                    user = context.PhpbbUsers.First(u => u.UserId == _forumBotUserId);
                    text = $"T({telegramName}): {text}";
                }

                var message = CreateMessage(user, text);
                await context.PhpbbChat.AddAsync(message);
                await context.SaveChangesAsync();
            }
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

        //private async Task SaveMesageAsync(ForumContext context, PhpbbUsers user, string text)
        //{
        //    var forumMessage = new PhpbbChat();
        //    forumMessage.Message = text;
        //    forumMessage.UserId = user.UserId;
        //    forumMessage.Username = user.Username;
        //    forumMessage.UserColour = user.UserColour;
        //    forumMessage.BbcodeBitfield = string.Empty;
        //    forumMessage.BbcodeUid = string.Empty;
        //    forumMessage.BbcodeOptions = 7;
        //    forumMessage.TelegramProcessed = 1;

        //    forumMessage.Time = (new DateTimeOffset(DateTime.Now)).ToUnixTimeSeconds();
        //    forumMessage.ChatId = 1;

        //    await context.PhpbbChat.AddAsync(forumMessage);
        //    await context.SaveChangesAsync();

        //}


    }
}
