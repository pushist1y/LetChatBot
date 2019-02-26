using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LetChatBot.Extensions;
using LetChatBot.Models;
using Telegram.Bot.Types;

namespace LetChatBot
{
    public class MessageProcessorService
    {
        private readonly TelegramAccessService _telegramAccessService;
        private readonly TelegramToForumLinkService _userLinker;
        private readonly TelegramMessageProcessorService _telegramMessageProcessor;
        private readonly CommandProcessorService _commandProcessor;
        private readonly MessagesRepository _messagesRepository;

        public MessageProcessorService(TelegramAccessService telegramAccessService,
            TelegramToForumLinkService userLinker,
            TelegramMessageProcessorService telegramMessageProcessor,
            CommandProcessorService commandProcessor,
            MessagesRepository messagesRepository)
        {
            _telegramAccessService = telegramAccessService;
            _userLinker = userLinker;
            _telegramMessageProcessor = telegramMessageProcessor;
            _commandProcessor = commandProcessor;
            _messagesRepository = messagesRepository;
        }

        public async Task ProcessForumMessage(PhpbbChat message)
        {
            Console.WriteLine($"F [{message.Username}]: {message.Message}");
            await _userLinker.ValidateAndLink(message.UserId, message.Message);
            await _telegramAccessService.SendToGroupFromUsernameAsync(message.Message.ConvertToTelegram(), message.Username);
            await _messagesRepository.SetMessageProcessedAsync(message.MessageId);
            await _commandProcessor.ProcessForumCommand(message);
        }

        public async Task ProcessTelegramMessage(Message message)
        {
            await _telegramMessageProcessor.ProcessMessage(message);
        }
    }
}
