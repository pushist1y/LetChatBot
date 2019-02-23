using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetChatBot.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LetChatBot
{
    public class PiuRepository
    {
        private readonly IServiceProvider _serviceProvider;

        public PiuRepository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<string> GetAnotherUserName()
        {
            var scopeFactory = _serviceProvider.GetRequiredService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var dt = Convert.ToUInt32((DateTimeOffset.Now - TimeSpan.FromDays(10)).ToUnixTimeSeconds());
                var context = scope.ServiceProvider.GetRequiredService<ForumContext>();
                var userNames = await context.PhpbbChat
                    .AsNoTracking()
                    .Where(m => m.Time > dt)
                    .OrderByDescending(m => m.MessageId)
                    .Select(m => m.Username)
                    .Distinct()
                    .ToListAsync();

                var random = new Random();
                var index = random.Next(0, userNames.Count);
                var userName = userNames[index];

                return userName;

            }
        }

        public async Task<string> GetPiu()
        {
            var scopeFactory = _serviceProvider.GetRequiredService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ForumContext>();
                var count = await context.IbotPiu.CountAsync();
                var random = new Random();
                var skip = random.Next(0, count);
                var piu = await context.IbotPiu.OrderBy(p => p.Id).Skip(skip).Take(1).FirstOrDefaultAsync();

                return piu.Text;

            }
        }

        public async Task<string> GetUserName(long telegramId)
        {
            var scopeFactory = _serviceProvider.GetRequiredService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ForumContext>();

                var user = await context.PhpbbUsers.FirstOrDefaultAsync(u => u.UserTelegramId == telegramId);

                return user?.Username;
            }
        }
    }
}
