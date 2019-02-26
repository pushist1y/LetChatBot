using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LetChatBot.Models;
using Microsoft.Extensions.Configuration;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace LetChatBot
{
    public class CommandProcessorService
    {
        private readonly TelegramAccessService _telegramAccessService;
        private readonly TelegramToForumLinkService _forumUserLinker;
        private readonly PiuRepository _piuRepository;
        private readonly MessagesRepository _messagesRepository;
        private readonly long _defaultGroupId;

        public CommandProcessorService(IConfiguration config, TelegramAccessService telegramAccessService, TelegramToForumLinkService forumUserLinker, PiuRepository piuRepository, MessagesRepository messagesRepository)
        {
            _telegramAccessService = telegramAccessService;
            _forumUserLinker = forumUserLinker;
            _piuRepository = piuRepository;
            _messagesRepository = messagesRepository;
            _defaultGroupId = Convert.ToInt64(config["DefaultGroupId"]);
        }

        public async Task ProcessForumCommand(PhpbbChat message)
        {
            if (message.Message.StartsWith("!пиу", true, CultureInfo.InvariantCulture) ||
                message.Message.StartsWith("!piu", true, CultureInfo.InvariantCulture) ||
                message.Message.StartsWith("!gbe", true, CultureInfo.InvariantCulture) ||
                message.Message.StartsWith("!зшг", true, CultureInfo.InvariantCulture))
            {
                await CommandPiu(message.Username);
            }
        }

        private async Task CommandPiu(string userName)
        {
            var piuMessage = await _piuRepository.GetPiu();


            piuMessage = Regex.Replace(piuMessage, @"%username%", userName);
            piuMessage = Regex.Replace(piuMessage, @"%nickname%", userName);

            if (piuMessage.Contains("%anotheruser%", StringComparison.InvariantCultureIgnoreCase))
            {
                var anotherUserName = await _piuRepository.GetAnotherUserName();
                piuMessage = Regex.Replace(piuMessage, @"%anotheruser%", anotherUserName);
            }

            await _messagesRepository.SaveMessageAsync(piuMessage);
            await _telegramAccessService.SendToGroupAsync(piuMessage);

        }

        public async Task ProcessTelegramForumCommand(Message message)
        {
            if (message.Type != MessageType.Text)
            {
                return;
            }

            if (message.Chat.Id == _defaultGroupId)
            {
                if (message.Text.StartsWith("!пиу", true, CultureInfo.InvariantCulture) ||
                    message.Text.StartsWith("!piu", true, CultureInfo.InvariantCulture) ||
                    message.Text.StartsWith("!gbe", true, CultureInfo.InvariantCulture) ||
                    message.Text.StartsWith("!зшг", true, CultureInfo.InvariantCulture))
                {
                    var username = await _piuRepository.GetUserName(message.From.Id) ?? message.From.FullName();
                    await CommandPiu(username);
                    return;
                }
            }
        }

        public async Task<bool> ProcessTelegramCommand(Message message)
        {
            if (message.Type != MessageType.Text)
            {
                return false;
            }

            var match = Regex.Match(message.Text, @"^(?:\/([A-Za-z0-9_]+)(@[A-Za-z0-9_]+)?(?:\s+(.*))?)$");
            if (match.Success)
            {
                var command = match.Groups[1].Value;
                var owner = match.Groups[2].Value;
                var commandParams = match.Groups[3].Value;

                if (message.Chat.Type != ChatType.Private && (owner == "@LET_iBot" || owner == "@LetTestBot"))
                {
                    await _telegramAccessService.SendToUserAsync("Команды принимаются только в приват", message.Chat.Id.ToString());
                    return true;
                }
                else if (message.Chat.Type == ChatType.Private)
                {
                    if (command == "echo")
                    {
                        return await ProcessEcho(message, commandParams);
                    }
                    else if (command == "link")
                    {
                        return await ProcessLink(message, commandParams);
                    }
                    else if (command == "help")
                    {
                        return await ProcessHelp(message, commandParams);
                    }
                }

            }

            return false;
        }

        private async Task<bool> ProcessEcho(Message message, string arguments)
        {
            await _telegramAccessService.Client.SendTextMessageAsync(message.From.Id, arguments);
            return true;
        }

        private async Task<bool> ProcessLink(Message message, string arguments)
        {
            var token = await _forumUserLinker.IssueToken(message.From.Id);

            var text =
                $@"Для привязки к учётной записи форума отправьте в чат форума (в браузере) следующее сообщение (вместе с фигурными скобками):
        {{{token}}}";

            await _telegramAccessService.Client.SendTextMessageAsync(message.From.Id, text);

            return true;
        }

        private async Task<bool> ProcessHelp(Message message, string arguments)
        {
            var text =
                @"Привет! Я бот, который транслирует сообщения из чата на форуме http://terribles.ru в Telegram и обратно
                Для участия в чате присоединяйся к группе: https://telegram.me/joinchat/BCU5_QEoyCHyB1xB41aAuA
                Регистрация на форуме не обязательна, но если ты зарегистрирован(а), то можешь связать свою учётную запись в Telegram с учётной записью на форуме с помощью команды /link.";

            await _telegramAccessService.Client.SendTextMessageAsync(message.From.Id, text);

            return true;
        }
    }
}
