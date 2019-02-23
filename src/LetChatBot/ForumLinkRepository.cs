using System;
using System.Threading.Tasks;
using LetChatBot.Data;
using LetChatBot.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LetChatBot
{
    public class ForumLinkRepository
    {
        private readonly IServiceProvider _serviceProvider;

        public ForumLinkRepository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<PhpbbUsers> SetUserLink(uint forumUserId, long? telegramUserId)
        {
            var scopeFactory = _serviceProvider.GetRequiredService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ForumContext>();
                var user = await context.PhpbbUsers.FirstOrDefaultAsync(u => u.UserId == forumUserId);
                if (user == null)
                {
                    return null;
                }

                user.UserTelegramId = telegramUserId;
                await context.SaveChangesAsync();

                return user;
            }
        }
    }
}
