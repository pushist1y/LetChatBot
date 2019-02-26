using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LetChatBot.Models;
using Microsoft.Extensions.Logging;

namespace LetChatBot
{
    public class DatabaseChatPollerService: HostedService
    {
        private readonly MessagesRepository _messagesRepository;
        private readonly ILogger<DatabaseChatPollerService> _logger;
        public event EventHandler<DatabaseMessageReceivedArgs> DatabaseMessageReceived;

        public DatabaseChatPollerService(MessagesRepository messagesRepository, ILogger<DatabaseChatPollerService> logger)
        {
            _messagesRepository = messagesRepository;
            _logger = logger;
        }

        private uint? _lastId;

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var messages = await _messagesRepository.GetUnprocessedMessagesAsync(_lastId);
                    if (messages.Any())
                    {
                        _lastId = messages.Max(m => m.MessageId);
                    }
            
                    foreach(var m in messages)
                    {
                        var args = new DatabaseMessageReceivedArgs(m);
                        DatabaseMessageReceived?.Invoke(this, args);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error on polling database for new messages");
                }
                finally
                {
                    await Task.Delay(1000, cancellationToken);
                }
            }
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