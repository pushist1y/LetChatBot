using System;
using System.Linq;
using System.Threading;
using LetChatBot.Model;

namespace LetChatBot
{
    public class DatabaseChatPoller
    {
        private readonly ForumMessageStore _messageStore;
        private Thread _runningThread;

        private bool _isRunning = false;
        public DatabaseChatPoller(ForumMessageStore messageStore)
        {
            _messageStore = messageStore;
        }

        public event EventHandler<DatabaseMessageReceivedArgs> DatabaseMessageReceived;

        private void GetMessages()
        {
            var messages = _messageStore.UnprocessedMessages;
            
            foreach(var m in messages)
            {
                var args = new DatabaseMessageReceivedArgs(m);
                DatabaseMessageReceived?.Invoke(this, args);
            }

            _messageStore.UpdateMessages(messages);
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