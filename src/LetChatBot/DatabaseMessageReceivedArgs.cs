using LetChatBot.Model;

namespace LetChatBot
{
    public class DatabaseMessageReceivedArgs
    {
        public DatabaseMessageReceivedArgs(PhpbbChat message)
        {
            Message = message;
        }
        public PhpbbChat Message {get;set;}

        
    }
}