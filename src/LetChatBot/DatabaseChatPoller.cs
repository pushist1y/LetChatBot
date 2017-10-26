using System;
using System.Linq;
using System.Threading;

namespace LetChatBot
{
    public class DatabaseChatPoller
    {
        public static TimeSpan ContextRefreshInterval = TimeSpan.FromMinutes(5);
        private readonly ForumContextFactory _contextFactory;
        private DateTime _lastRefresh;
        private ForumContext _context;
        private Thread _runningThread;

        private bool _isRunning = false;
        public DatabaseChatPoller(ForumContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _lastRefresh = DateTime.Now;
            _context = _contextFactory.NewContext();
        }

        public event EventHandler<DatabaseMessageReceivedArgs> DatabaseMessageReceived;

        private void RefreshContext()
        {
            if(DateTime.Now - _lastRefresh > ContextRefreshInterval)
            {
                _context = _contextFactory.NewContext();
            }
        }

        private void GetMessages()
        {
            RefreshContext();

            var messages = _context.PhpbbChat
                            .Where(m => !m.TelegramProcessed)
                            .OrderBy(m => m.MessageId)
                            .ToList();
            
            foreach(var m in messages)
            {
                var args = new DatabaseMessageReceivedArgs(m);
                DatabaseMessageReceived?.Invoke(this, args);
            }
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
}