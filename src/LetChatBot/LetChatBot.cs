using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Args;

namespace LetChatBot
{
    public class LetChatBot
    {
        private readonly DatabaseChatPollerService _dbPoller;
        private readonly IServiceProvider _serviceProvider;
        private readonly TelegramAccessService _telegramAccessService;
        private readonly ILogger<LetChatBot> _logger;
        private bool _isRunning;
        private CancellationTokenSource _cancellationTokenSource;

        public LetChatBot(DatabaseChatPollerService dbPoller,
            IServiceProvider serviceProvider,
            TelegramAccessService telegramAccessService,
            ILogger<LetChatBot> logger)
        {
            _dbPoller = dbPoller;
            _serviceProvider = serviceProvider;
            _telegramAccessService = telegramAccessService;
            _dbPoller.DatabaseMessageReceived += OnDatabaseMessageReceived;
            _telegramAccessService.Client.OnMessage += TelegramAccessServiceOnMessage;
            _logger = logger;
            _logger.LogDebug("Initializing LetChatBot instance");
        }


        private async void TelegramAccessServiceOnMessage(object sender, MessageEventArgs e)
        {
            try
            {
                var scopeFactory = _serviceProvider.GetRequiredService<IServiceScopeFactory>();
                using (var scope = scopeFactory.CreateScope())
                {
                    var messageProcessor = scope.ServiceProvider.GetRequiredService<MessageProcessorService>();
                    await messageProcessor.ProcessTelegramMessage(e.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on processing telegram message");
            }
        }

        private async void OnDatabaseMessageReceived(object sender, DatabaseMessageReceivedArgs e)
        {
            try
            {
                var scopeFactory = _serviceProvider.GetRequiredService<IServiceScopeFactory>();
                using (var scope = scopeFactory.CreateScope())
                {
                    var messageProcessor = scope.ServiceProvider.GetRequiredService<MessageProcessorService>();
                    await messageProcessor.ProcessForumMessage(e.Message);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error on processing db message");
            }
            

        }

        public async Task Start()
        {
            if (_isRunning)
            {
                return;
            }
            _logger.LogDebug("Starting LetChatBot");
            _isRunning = true;
            _cancellationTokenSource = new CancellationTokenSource();
            _telegramAccessService.Client.StartReceiving();
            var q = await _telegramAccessService.Client.GetMeAsync();
            await _dbPoller.StartAsync(_cancellationTokenSource.Token);
        }

        public async Task Stop()
        {
            _logger.LogDebug("Stopping LetChatBot");
            _telegramAccessService.Client.StopReceiving();
            await _dbPoller.StopAsync(_cancellationTokenSource.Token);
        }

    }
}
