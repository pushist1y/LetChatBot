using System.Threading.Tasks;
using LetChatBot.Data;
using LetChatBot.Models;
using Microsoft.EntityFrameworkCore;

namespace LetChatBot
{
    public class ForumLinkRepository
    {
        private readonly ForumContext _context;

        public ForumLinkRepository(ForumContext context)
        {
            _context = context;
        }

        public async Task<PhpbbUsers> SetUserLink(uint forumUserId, long? telegramUserId)
        {
            var user = await _context.PhpbbUsers.FirstOrDefaultAsync(u => u.UserId == forumUserId);
            if (user == null)
            {
                return null;
            }

            user.UserTelegramId = telegramUserId;
            await _context.SaveChangesAsync();

            return user;
        }
    }
}
