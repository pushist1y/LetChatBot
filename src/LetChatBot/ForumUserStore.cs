using System.Collections.Generic;
using System.Linq;
using LetChatBot.Model;

namespace LetChatBot
{
    public class ForumUserStore
    {
        
        private readonly ForumContext _context;
        public ForumUserStore(ForumContext context)
        {
            _context = context;
        }

        public IQueryable<PhpbbUsers> Users => _context.PhpbbUsers;

    }
}