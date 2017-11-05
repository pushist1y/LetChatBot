using System;
using System.Linq;
using System.Threading;
using LetChatBot.Model;
using Microsoft.EntityFrameworkCore;

namespace LetChatBot
{
    public class DatabaseChatPoller
    {
        private Thread _runningThread;
        private readonly ForumContext _context;
        private bool _isRunning = false;
        public DatabaseChatPoller(ForumContext context)
        {
            _context = context;
        }

        public event EventHandler<DatabaseMessageReceivedArgs> DatabaseMessageReceived;

        private void GetMessages()
        {
            var messages = _context.PhpbbChat
                            .Where(m => m.TelegramProcessed <= 0)
                            .OrderBy(m => m.MessageId);
            
            foreach(var m in messages)
            {
                var args = new DatabaseMessageReceivedArgs(m);
                DatabaseMessageReceived?.Invoke(this, args);
            }

            _context.SaveChanges();
        }

        private void RunningLoop()
        {
            while(_isRunning)
            {
                Thread.Sleep(1000);
                GetMessages();
            }
        }

        public void StartPolling()
        {
            if(_isRunning)
            {
                return;
            }

            _isRunning = true;
            var threadStart = new ThreadStart(this.RunningLoop);
            _runningThread = new Thread(threadStart);
            _runningThread.Start();
        }

        public void StopPolling()
        {
            if(!_isRunning)
            {
                return;
            }

            _isRunning = false;
            _runningThread.Join();
            _runningThread = null;
        }


    }

    public class DatabaseMessageReceivedArgs
    {
        public DatabaseMessageReceivedArgs(PhpbbChat message)
        {
            Message = message;
        }
        public PhpbbChat Message {get;set;}

        
    }
}