using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using LetChatBot.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using File = System.IO.File;

namespace LetChatBot
{
    public class TelegramMessageProcessor
    {
        private TelegramBotClient _client;
        private readonly ForumContext _context;
        private readonly IConfigurationRoot _config;
        private readonly int _forumBotUserId;
        private readonly long _defaultGroupId;
        private readonly string _token;
        private readonly string _staticPath;
        private readonly string _staticUrl;
        private readonly string _stickersFolder;
        private readonly string _imagesFolder;
        private readonly string _filesFolder;

        public TelegramMessageProcessor(ForumContext context, IConfigurationRoot config)
        {
            _context = context;
            _config = config;
            _forumBotUserId = Convert.ToInt32(config["ForumBotUserId"]);
            _defaultGroupId = Convert.ToInt64(config["DefaultGroupId"]);
            _token = config["TelegramBotToken"];

            _staticPath = config["StaticFolderPath"];
            _staticUrl = config["StaticDataUrl"];
            _stickersFolder = config["StickersFolder"];
            _imagesFolder = config["ImagesFolder"];
            _filesFolder = config["FilesFolder"];

        }

        public void SetClient(TelegramBotClient client)
        {
            if (client == null)
            {
                throw new ArgumentNullException("Telegram client can't be null", nameof(client));
            }
            _client = client;
        }

        private void ProcessGroupMessage(Message message)
        {
            switch (message.Type)
            {
                case MessageType.Text:
                    ProcessGroupTextMessage(message);
                    break;
                case MessageType.Sticker:
                    ProcessGroupStickerMessage(message);
                    break;
                case MessageType.Photo:
                    ProcessGroupImageMessage(message);
                    break;

            }
        }

        private void ProcessGroupTextMessage(Message message)
        {
            TelegramToForum(message.From.FullName(), message.From.Id, message.Text);
        }

        private void ProcessGroupStickerMessage(Message message)
        {
            var webpPath = Path.Combine(_staticPath, _stickersFolder, message.Sticker.FileId + ".webp");
            var pngPath = Regex.Replace(webpPath, @"\.webp$", ".png");
            if (!System.IO.File.Exists(pngPath))
            {
                var fileInfo = _client.GetFileAsync(message.Sticker.FileId).Result;

                using (var webpFile = System.IO.File.Create(webpPath))
                {
                    File.Open(fileInfo.FilePath, FileMode.Open, FileAccess.Read).CopyTo(webpFile);
                }
                $"dwebp \"{webpPath}\" -o \"{pngPath}\" ".Bash();
                System.IO.File.Delete(webpPath);
            }

            var stickerUrl = new Uri(_staticUrl).Append(_stickersFolder).Append(message.Sticker.FileId + ".png");

            var text = $"(Стикер) {stickerUrl.AbsoluteUri}";

            TelegramToForum(message.From.FullName(), message.From.Id, text);
        }

        private void ProcessGroupImageMessage(Message message)
        {
            var imageInfo = message.Photo.Last();
            var filePath = Path.Combine(_staticPath, _imagesFolder, imageInfo.FileId + ".jpg");
            if (!System.IO.File.Exists(filePath))
            {
                var fileInfo = _client.GetFileAsync(imageInfo.FileId).Result;

                using (var jpgFile = System.IO.File.Create(filePath))
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

            TelegramToForum(message.From.FullName(), message.From.Id, text);
        }

        private void ProcessGroupVideoMessage(Message message)
        {
            var filePath = Path.Combine(_staticPath, _filesFolder, message.Video.FileId + ".mp4");
            if (!System.IO.File.Exists(filePath))
            {
                var fileInfo = _client.GetFileAsync(message.Video.FileId).Result;

                using (var videoFile = System.IO.File.Create(filePath))
                {
                    File.Open(fileInfo.FilePath, FileMode.Open, FileAccess.Read).CopyTo(videoFile);
                }
            }
            var imageUrl = new Uri(_staticUrl).Append(_filesFolder).Append(message.Video.FileId + ".jpg");
            TelegramToForum(message.From.FullName(), message.From.Id, $"( {imageUrl} )");
        }

        private void ProcessGroupDocumentMessage(Message message)
        {
            var filePath = Path.Combine(_staticPath, _filesFolder, message.Audio.FileId + ".mp3");
            if (!System.IO.File.Exists(filePath))
            {
                var fileInfo = _client.GetFileAsync(message.Document.FileId).Result;

                using (var docFile = System.IO.File.Create(filePath))
                {
                    File.Open(fileInfo.FilePath, FileMode.Open, FileAccess.Read).CopyTo(docFile);
                }
            }
            var docUrl = new Uri(_staticUrl).Append(_filesFolder).Append(message.Audio.FileId + ".mp3");
            TelegramToForum(message.From.FullName(), message.From.Id, $"( {docUrl} )");
        }

        private void ProcessGroupAudioMessage(Message message)
        {
            if(message.Document.FileSize > 1024 * 1024 * 20)
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
            if (!System.IO.File.Exists(filePath))
            {
                var fileInfo = _client.GetFileAsync(message.Document.FileId).Result;

                using (var docFile = System.IO.File.Create(filePath))
                {
                    File.Open(fileInfo.FilePath, FileMode.Open, FileAccess.Read).CopyTo(docFile);
                }
            }
            var docUrl = new Uri(_staticUrl).Append(_filesFolder).Append(fileName);
            TelegramToForum(message.From.FullName(), message.From.Id, $"( {docUrl} )");
        }

        public void ProcessMessage(Message message)
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
                ProcessGroupMessage(message);
            }
        }

        private void TelegramToForum(string telegramName, long telegramId, string text, bool convert = true)
        {
            if (convert)
            {
                text = text.ConvertToForum();
            }
            var user = _context.PhpbbUsers.FirstOrDefault(u => u.UserTelegramId == telegramId);
            if (user == null)
            {
                user = _context.PhpbbUsers.First(u => u.UserId == _forumBotUserId);
                text = $"T({telegramName}): {text}";
            }

            SendToForum(user, text);
        }

        private void SendToForum(string text, int forumUserId = -1)
        {
            if (forumUserId < 0)
            {
                forumUserId = _forumBotUserId;
            }

            var user = _context.PhpbbUsers.FirstOrDefault(u => u.UserId == forumUserId);
            if (user == null)
            {
                user = _context.PhpbbUsers.First(u => u.UserId == _forumBotUserId);
            }

            SendToForum(user, text);
        }

        private void SendToForum(PhpbbUsers user, string text)
        {
            var forumMessage = new PhpbbChat();
            forumMessage.Message = text;
            forumMessage.UserId = user.UserId;
            forumMessage.Username = user.Username;
            forumMessage.UserColour = user.UserColour;
            forumMessage.BbcodeBitfield = string.Empty;
            forumMessage.BbcodeUid = string.Empty;
            forumMessage.BbcodeOptions = 7;
            forumMessage.TelegramProcessed = 1;

            forumMessage.Time = (new DateTimeOffset(DateTime.Now)).ToUnixTimeSeconds();
            forumMessage.ChatId = 1;

            _context.PhpbbChat.Add(forumMessage);
            _context.SaveChanges();
        }
    }
}