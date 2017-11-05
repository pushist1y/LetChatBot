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
            switch(message.Type)
            {
                case MessageType.TextMessage:
                    ProcessGroupTextMessage(message);
                    break;
                case MessageType.StickerMessage:
                    ProcessGroupStickerMessage(message);
                    break;
                case MessageType.PhotoMessage:
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
                        fileInfo.FileStream.CopyTo(webpFile);
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
                        fileInfo.FileStream.CopyTo(jpgFile);
                    }
                }

                var imageUrl = new Uri(_staticUrl).Append(_imagesFolder).Append(imageInfo.FileId + ".jpg");

                var text = $"( {imageUrl.AbsoluteUri} )";
                if(!string.IsNullOrEmpty(message.Caption))
                {
                    text = $"{message.Caption}: {text}";
                }

                TelegramToForum(message.From.FullName(), message.From.Id, text);
        }

        public void ProcessMessage(Message message)
        {
            if (message.Type == MessageType.TextMessage)
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
            var user = _context.PhpbbUsers.AsNoTracking().FirstOrDefault(u => u.UserTelegramId == telegramId);
            if (user == null)
            {
                user = _context.PhpbbUsers.AsNoTracking().First(u => u.UserId == _forumBotUserId);
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

            var user = _context.PhpbbUsers.AsNoTracking().FirstOrDefault(u => u.UserId == forumUserId);
            if (user == null)
            {
                user = _context.PhpbbUsers.AsNoTracking().First(u => u.UserId == _forumBotUserId);
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