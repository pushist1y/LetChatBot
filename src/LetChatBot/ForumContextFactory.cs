using Microsoft.EntityFrameworkCore;

namespace LetChatBot
{
    public class ForumContextFactory
    {
        private readonly DbContextOptions<ForumContext> _options;
        public ForumContextFactory(DbContextOptions<ForumContext> options)
        {
            _options = options;
        }

        public ForumContext NewContext()
        {
            return new ForumContext(_options);
        }
    }
}