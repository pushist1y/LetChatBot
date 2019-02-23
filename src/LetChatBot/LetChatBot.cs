using System;
using System.Threading;
using System.Threading.Tasks;
using LetChatBot.Extensions;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Args;

namespace LetChatBot
{
    public class LetChatBot
    {
        private readonly DatabaseChatPoller _dbPoller;
        private readonly TelegramAccessService _telegramAccessService;
        private readonly TelegramToForumUserLinker _userLinker;
        private readonly TelegramMessageProcessor _messageProcessor;
        private readonly CommandProcessor _commandProcessor;
        private readonly MessagesRepository _messagesRepository;
        private readonly ILogger<LetChatBot> _logger;

        private bool _isRunning;
        private CancellationTokenSource _cancellationTokenSource;

        public LetChatBot(DatabaseChatPoller dbPoller,
            TelegramAccessService telegramAccessService,
                        TelegramToForumUserLinker userLinker,
                        TelegramMessageProcessor messageProcessor,
            CommandProcessor commandProcessor,
            MessagesRepository messagesRepository,
            ILogger<LetChatBot> logger)
        {
            _dbPoller = dbPoller;
            _telegramAccessService = telegramAccessService;
            _userLinker = userLinker;
            _dbPoller.DatabaseMessageReceived += OnDatabaseMessageReceived;
            _telegramAccessService.Client.OnMessage += TelegramAccessServiceOnMessage;
            _telegramAccessService.Client.OnUpdate += ClientOnOnUpdate;
            _messageProcessor = messageProcessor;
            _commandProcessor = commandProcessor;
            _messagesRepository = messagesRepository;
            _logger = logger;
            _logger.LogDebug("Initializing LetChatBot instance");
        }

        private void ClientOnOnUpdate(object sender, UpdateEventArgs e)
        {
            _logger.LogDebug(e.Update.Message.Text);
        }

        private async void TelegramAccessServiceOnMessage(object sender, MessageEventArgs e)
        {
            await _messageProcessor.ProcessMessage(e.Message);
        }

        private async void OnDatabaseMessageReceived(object sender, DatabaseMessageReceivedArgs e)
        {
            Console.WriteLine($"F [{e.Message.Username}]: {e.Message.Message}");

            try
            {
                await _userLinker.ValidateAndLink(e.Message.UserId, e.Message.Message);
                await _telegramAccessService.SendToGroupFromUsernameAsync(e.Message.Message.ConvertToTelegram(), e.Message.Username);
                await _messagesRepository.SetMessageProcessedAsync(e.Message.MessageId);
                await _commandProcessor.ProcessForumCommand(e.Message);
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
