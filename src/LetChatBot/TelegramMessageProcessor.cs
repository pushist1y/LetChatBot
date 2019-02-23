using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ImageMagick;
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
        private readonly CommandProcessor _commandProcessor;
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
            CommandProcessor commandProcessor,
            ILogger<TelegramMessageProcessor> logger,
            TelegramAccessService telegramAccessService)
        {
            _messagesRepository = messagesRepository;
            _commandProcessor = commandProcessor;
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
                case MessageType.Voice:
                    await ProcessGroupVoiceMessage(message);
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
                using (var f = File.Open(webpPath, FileMode.Create, FileAccess.Write))
                {
                    await _telegramAccessService.Client.DownloadFileAsync(fileInfo.FilePath, f);
                }

                using (var image = new MagickImage(webpPath))
                {
                    image.Format = MagickFormat.Png;
                    image.Write(pngPath);
                }

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
                using (var f = File.Open(filePath, FileMode.Create, FileAccess.Write))
                {
                    await _telegramAccessService.Client.DownloadFileAsync(fileInfo.FilePath, f);
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

                using (var f = File.Open(filePath, FileMode.Create, FileAccess.Write))
                {
                    await _telegramAccessService.Client.DownloadFileAsync(fileInfo.FilePath, f);
                }
            }
            var imageUrl = new Uri(_staticUrl).Append(_filesFolder).Append(message.Video.FileId + ".jpg");
            await TelegramToForum(message.From.FullName(), message.From.Id, $"( {imageUrl} )");
        }

        private async Task ProcessGroupVoiceMessage(Message message)
        {
            var filePath = Path.Combine(_staticPath, _filesFolder, message.Voice.FileId + ".ogg");
            if (!File.Exists(filePath))
            {
                var fileInfo = await _telegramAccessService.Client.GetFileAsync(message.Voice.FileId);

                using (var f = File.Open(filePath, FileMode.Create, FileAccess.Write))
                {
                    await _telegramAccessService.Client.DownloadFileAsync(fileInfo.FilePath, f);
                }
            }
            var fileUrl = new Uri(_staticUrl).Append(_filesFolder).Append(message.Voice.FileId + ".ogg");
            await TelegramToForum(message.From.FullName(), message.From.Id, $"Голосовое сообщение: ( {fileUrl} )");
        }

        private async Task ProcessGroupDocumentMessage(Message message)
        {
            var localFileName = DateTimeOffset.Now.ToString("yyyy-MM-dd-HH-mm-ss-") + message.Document.FileName;
            var filePath = Path.Combine(_staticPath, _filesFolder, localFileName);
            if (!File.Exists(filePath))
            {
                var fileInfo = await _telegramAccessService.Client.GetFileAsync(message.Document.FileId);

                using (var f = File.Open(filePath, FileMode.Create, FileAccess.Write))
                {
                    await _telegramAccessService.Client.DownloadFileAsync(fileInfo.FilePath, f);
                }
            }
            var docUrl = new Uri(_staticUrl).Append(_filesFolder).Append(localFileName);
            await TelegramToForum(message.From.FullName(), message.From.Id, $"( {docUrl} )");
        }

        private async Task ProcessGroupAudioMessage(Message message)
        {
            string fileName;
            if (!string.IsNullOrEmpty(message.Audio.Performer) || !string.IsNullOrEmpty(message.Audio.Title))
            {
                fileName = $"{(string.IsNullOrEmpty(message.Audio.Performer) ? "Unknown artist" : message.Audio.Performer)} - {(string.IsNullOrEmpty(message.Audio.Title) ? "Unknown title" : message.Audio.Title)}";
            }
            else
            {
                fileName = message.Audio.FileId;
            }

            if (message.Audio.MimeType.Contains("mpeg"))
            {
                fileName += ".mp3";
            }
            else
            {
                fileName += ".mp3";
            }

            var filePath = Path.Combine(_staticPath, _filesFolder, fileName);
            if (!File.Exists(filePath))
            {
                var fileInfo = await _telegramAccessService.Client.GetFileAsync(message.Audio.FileId);

                using (var f = File.Open(filePath, FileMode.Create, FileAccess.Write))
                {
                    await _telegramAccessService.Client.DownloadFileAsync(fileInfo.FilePath, f);
                }
            }
            var docUrl = new Uri(_staticUrl).Append(_filesFolder).Append(fileName);
            await TelegramToForum(message.From.FullName(), message.From.Id, $"( {docUrl} )");
        }

        public async Task ProcessMessage(Message message)
        {
            Console.WriteLine($"TG [{message.From.FullName()}]: {message.Text}");

            if (await _commandProcessor.ProcessTelegramCommand(message))
            {
                return;
            }

            if (message.Chat.Id == _defaultGroupId)
            {
                try
                {
                    await ProcessGroupMessage(message);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error on processing tg message");
                }
            }

            await _commandProcessor.ProcessTelegramForumCommand(message);
        }

        private async Task TelegramToForum(string telegramName, long telegramId, string text)
        {

            text = text.ConvertToForum();
            await _messagesRepository.SaveMessageAsync(telegramName, telegramId, text);
        }

    }
}