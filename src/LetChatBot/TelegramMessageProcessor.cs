using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LetChatBot.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using File = System.IO.File;

namespace LetChatBot
{
    public class TelegramMessageProcessor
    {
        private readonly MessagesRepository _messagesRepository;
        private readonly ILogger<TelegramMessageProcessor> _logger;
        private readonly TelegramAccessService _telegramAccessService;
        private readonly long _defaultGroupId;
        private readonly string _staticPath;
        private readonly string _staticUrl;
        private readonly string _stickersFolder;
        private readonly string _imagesFolder;
        private readonly string _filesFolder;

        public TelegramMessageProcessor(IConfiguration config, 
            MessagesRepository messagesRepository, 
            ILogger<TelegramMessageProcessor> logger,
            TelegramAccessService telegramAccessService)
        {
            _messagesRepository = messagesRepository;
            _logger = logger;
            _telegramAccessService = telegramAccessService;
            _defaultGroupId = Convert.ToInt64(config["DefaultGroupId"]);

            _staticPath = config["StaticFolderPath"];
            _staticUrl = config["StaticDataUrl"];
            _stickersFolder = config["StickersFolder"];
            _imagesFolder = config["ImagesFolder"];
            _filesFolder = config["FilesFolder"];

        }

        private async Task ProcessGroupMessage(Message message)
        {
            switch (message.Type)
            {
                case MessageType.Text:
                    await ProcessGroupTextMessage(message);
                    break;
                case MessageType.Sticker:
                    await ProcessGroupStickerMessage(message);
                    break;
                case MessageType.Photo:
                    await ProcessGroupImageMessage(message);
                    break;
                case MessageType.Video:
                    await ProcessGroupVideoMessage(message);
                    break;
                case MessageType.Audio:
                    await ProcessGroupAudioMessage(message);
                    break;
                case MessageType.Document:
                    await ProcessGroupDocumentMessage(message);
                    break;

            }
        }

        private async Task ProcessGroupTextMessage(Message message)
        {
            await TelegramToForum(message.From.FullName(), message.From.Id, message.Text);
            _logger.LogInformation($"Telegram TEXT message from [{message.From.FullName()}]: {message.Text}");
        }

        private async Task ProcessGroupStickerMessage(Message message)
        {
            var webpPath = Path.Combine(_staticPath, _stickersFolder, message.Sticker.FileId + ".webp");
            var pngPath = Regex.Replace(webpPath, @"\.webp$", ".png");
            if (!File.Exists(pngPath))
            {
                var fileInfo = await _telegramAccessService.Client.GetFileAsync(message.Sticker.FileId);

                using (var webpFile = File.Create(webpPath))
                {
                    File.Open(fileInfo.FilePath, FileMode.Open, FileAccess.Read).CopyTo(webpFile);
                }
                $"dwebp \"{webpPath}\" -o \"{pngPath}\" ".Bash();
                File.Delete(webpPath);
            }

            var stickerUrl = new Uri(_staticUrl).Append(_stickersFolder).Append(message.Sticker.FileId + ".png");

            var text = $"(Стикер) {stickerUrl.AbsoluteUri}";

            await TelegramToForum(message.From.FullName(), message.From.Id, text);
        }

        private async Task ProcessGroupImageMessage(Message message)
        {
            var imageInfo = message.Photo.Last();
            var filePath = Path.Combine(_staticPath, _imagesFolder, imageInfo.FileId + ".jpg");
            if (!File.Exists(filePath))
            {
                var fileInfo = await _telegramAccessService.Client.GetFileAsync(imageInfo.FileId);

                using (var jpgFile = File.Create(filePath))
                {
                    File.Open(fileInfo.FilePath, FileMode.Open, FileAccess.Read).CopyTo(jpgFile);
                }
            }

            var imageUrl = new Uri(_staticUrl).Append(_imagesFolder).Append(imageInfo.FileId + ".jpg");

            var text = $"( {imageUrl.AbsoluteUri} )";
            if (!string.IsNullOrEmpty(message.Caption))
            {
                text = $"{message.Caption}: {text}";
            }

            await TelegramToForum(message.From.FullName(), message.From.Id, text);
        }

        private async Task ProcessGroupVideoMessage(Message message)
        {
            var filePath = Path.Combine(_staticPath, _filesFolder, message.Video.FileId + ".mp4");
            if (!File.Exists(filePath))
            {
                var fileInfo = await _telegramAccessService.Client.GetFileAsync(message.Video.FileId);

                using (var videoFile = File.Create(filePath))
                {
                    File.Open(fileInfo.FilePath, FileMode.Open, FileAccess.Read).CopyTo(videoFile);
                }
            }
            var imageUrl = new Uri(_staticUrl).Append(_filesFolder).Append(message.Video.FileId + ".jpg");
            await TelegramToForum(message.From.FullName(), message.From.Id, $"( {imageUrl} )");
        }

        private async Task ProcessGroupDocumentMessage(Message message)
        {
            var filePath = Path.Combine(_staticPath, _filesFolder, message.Audio.FileId + ".mp3");
            if (!File.Exists(filePath))
            {
                var fileInfo = await _telegramAccessService.Client.GetFileAsync(message.Document.FileId);

                using (var docFile = File.Create(filePath))
                {
                    File.Open(fileInfo.FilePath, FileMode.Open, FileAccess.Read).CopyTo(docFile);
                }
            }
            var docUrl = new Uri(_staticUrl).Append(_filesFolder).Append(message.Audio.FileId + ".mp3");
            await TelegramToForum(message.From.FullName(), message.From.Id, $"( {docUrl} )");
        }

        private async Task ProcessGroupAudioMessage(Message message)
        {
            if (message.Document.FileSize > 1024 * 1024 * 20)
            {
                return;
            }
            string fileName;
            if (message.Document.FileName == null)
            {
                fileName = message.Document.FileId;
                if (message.Document.MimeType == "video/mp4")
                {
                    fileName += ".mp4";
                }
            }
            else
            {
                fileName = message.Document.FileId + "_" + message.Document.FileName;
            }

            var filePath = Path.Combine(_staticPath, _filesFolder, fileName);
            if (!File.Exists(filePath))
            {
                var fileInfo = await _telegramAccessService.Client.GetFileAsync(message.Document.FileId);

                using (var docFile = File.Create(filePath))
                {
                    File.Open(fileInfo.FilePath, FileMode.Open, FileAccess.Read).CopyTo(docFile);
                }
            }
            var docUrl = new Uri(_staticUrl).Append(_filesFolder).Append(fileName);
            await TelegramToForum(message.From.FullName(), message.From.Id, $"( {docUrl} )");
        }

        public async Task ProcessMessage(Message message)
        {
            if (message.Type == MessageType.Text)
            {
                var match = Regex.Match(message.Text, "^(?:\\/([a-z0-9_]+)(@[a-z0-9_]+)?(?:\\s+(.*))?)$");
                if (match.Success)
                {
                    var command = match.Groups[1].Value;
                    var owner = match.Groups[2].Value;
                    var commandParams = match.Groups[3].Value;
                    //telegram command
                    return;
                }
            }

            if (message.Chat.Id == _defaultGroupId)
            {
                await ProcessGroupMessage(message);
            }
        }

        private async Task TelegramToForum(string telegramName, long telegramId, string text)
        {

            text = text.ConvertToForum();
            await _messagesRepository.SaveMessageAsync(telegramName, telegramId, text);
        }

        //private void SendToForum(string text, int forumUserId = -1)
        //{
        //    if (forumUserId < 0)
        //    {
        //        forumUserId = _forumBotUserId;
        //    }

        //    var user = _context.PhpbbUsers.FirstOrDefault(u => u.UserId == forumUserId);
        //    if (user == null)
        //    {
        //        user = _context.PhpbbUsers.First(u => u.UserId == _forumBotUserId);
        //    }

        //    SendToForum(user, text);
        //}

        //private void SendToForum(PhpbbUsers user, string text)
        //{
        //    var forumMessage = new PhpbbChat();
        //    forumMessage.Message = text;
        //    forumMessage.UserId = user.UserId;
        //    forumMessage.Username = user.Username;
        //    forumMessage.UserColour = user.UserColour;
        //    forumMessage.BbcodeBitfield = string.Empty;
        //    forumMessage.BbcodeUid = string.Empty;
        //    forumMessage.BbcodeOptions = 7;
        //    forumMessage.TelegramProcessed = 1;

        //    forumMessage.Time = (new DateTimeOffset(DateTime.Now)).ToUnixTimeSeconds();
        //    forumMessage.ChatId = 1;

        //    _context.PhpbbChat.Add(forumMessage);
        //    _context.SaveChanges();
        //}
    }
}