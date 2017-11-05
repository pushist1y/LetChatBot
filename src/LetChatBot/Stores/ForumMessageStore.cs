using System.Collections.Generic;
using System.Linq;
using LetChatBot.Model;
using Microsoft.EntityFrameworkCore;

namespace LetChatBot
{
    public class ForumMessageStore
    {
        
        private readonly ForumContext _context;
        public ForumMessageStore(ForumContext context)
        {
            _context = context;
        }

        public IQueryable<PhpbbChat> UnprocessedMessages => _context.PhpbbChat
                            .AsNoTracking()        
                            .Where(m => m.TelegramProcessed <= 0)
                            .OrderBy(m => m.MessageId);

        public void UpdateMessages(IEnumerable<PhpbbChat> messages)
        {
            _context.UpdateRange(messages);
            _context.SaveChanges();
        }
                            
        public void AddMessage(PhpbbChat message)
        {
            _context.PhpbbChat.Add(message);
            _context.SaveChanges();
        }

    }
}