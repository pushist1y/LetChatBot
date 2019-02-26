using System;
using System.Linq;
using System.Threading.Tasks;
using LetChatBot.Data;
using Microsoft.EntityFrameworkCore;

namespace LetChatBot
{
    public class PiuRepository
    {
        private readonly ForumContext _context;

        public PiuRepository(ForumContext context)
        {
            _context = context;
        }

        public async Task<string> GetAnotherUserName()
        {
            var dt = Convert.ToUInt32((DateTimeOffset.Now - TimeSpan.FromDays(10)).ToUnixTimeSeconds());
            var userNames = await _context.PhpbbChat
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

        public async Task<string> GetPiu()
        {
            var count = await _context.IbotPiu.CountAsync();
            var random = new Random();
            var skip = random.Next(0, count);
            var piu = await _context.IbotPiu.OrderBy(p => p.Id).Skip(skip).Take(1).FirstOrDefaultAsync();

            return piu.Text;
        }

        public async Task<string> GetUserName(long telegramId)
        {
            var user = await _context.PhpbbUsers.FirstOrDefaultAsync(u => u.UserTelegramId == telegramId);
            return user?.Username;
        }
    }
}
