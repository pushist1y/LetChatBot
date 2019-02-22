using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MihaZupan;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace LetChatBot
{
    public class TelegramAccessService
    {
        private readonly TelegramOptions _options;

        public TelegramAccessService(IOptions<TelegramOptions> options)
        {
            _options = options.Value;

            if (!string.IsNullOrEmpty(_options.ProxyAddress))
            {
                var proxy = new HttpToSocks5Proxy(
                    _options.ProxyAddress, _options.ProxyPort, _options.ProxyUsername, _options.ProxyPassword
                );

                proxy.ResolveHostnamesLocally = true;

                Client = new TelegramBotClient(_options.TelegramBotToken, proxy);
            }
            else
            {
                Client = new TelegramBotClient(_options.TelegramBotToken);
            }
        }

        public TelegramBotClient Client { get; }

        public async Task SendToGroupAsync(string message)
        {
            await Client.SendTextMessageAsync(_options.DefaultGroupId, message);
        }

        public async Task SendToGroupFromUsernameAsync(string message, string userName)
        {
            var text = $"{userName}: {message}";
            await Client.SendTextMessageAsync(_options.DefaultGroupId, text);
        }

        public async Task SendToUserAsync(string message, string chatId)
        {
            await Client.SendTextMessageAsync(chatId, message);
        }
    }
}
