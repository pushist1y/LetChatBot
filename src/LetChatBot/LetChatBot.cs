using System;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;

namespace LetChatBot
{
    public class LetChatBot
    {
        private TelegramBotClient _client;
        private DatabaseChatPoller _dbPoller;

        private readonly string _token;
        public LetChatBot(IConfigurationRoot config, DatabaseChatPoller dbPoller)
        {
            _token = config["TelegramBotToken"];
            _client = new TelegramBotClient(_token);
            _dbPoller = dbPoller;
            _dbPoller.DatabaseMessageReceived += OnDatabaseMessageReceived;
        }

        private void OnDatabaseMessageReceived(object sender, DatabaseMessageReceivedArgs e) 
        {
            Console.WriteLine($"{e.Message.Username}: {e.Message.Message}");
        }

        public void Start()
        {
            _client.StartReceiving();
            _dbPoller.StartPolling();
        }

        public void Stop()
        {
            _client.StopReceiving();
            _dbPoller.StopPolling();
        }

    }
}