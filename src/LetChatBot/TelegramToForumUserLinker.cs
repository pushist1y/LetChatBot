using System;
using System.Collections.Generic;
using System.Linq;
using LetChatBot.Model;
using Microsoft.Extensions.Configuration;

namespace LetChatBot
{
    public class TelegramToForumUserLinker
    {
        private readonly ForumUserStore _userStore;
        private readonly IConfigurationRoot _config;
        private readonly int _forumBotUserId;
        public TelegramToForumUserLinker(ForumUserStore userStore, IConfigurationRoot config)
        {
            _userStore = userStore;
            _config = config;
            _forumBotUserId = Convert.ToInt32(config["ForumBotUserId"]);
        }
        private static List<TelegramForumLinkTokenInfo> TokensCache = new List<TelegramForumLinkTokenInfo>();

        public event EventHandler<TelegramToForumLinkArgumentArgs> UserLinked;
        public event EventHandler<TelegramToForumLinkArgumentArgs> UserUnlinked;
        public string IssueToken(long telegramUserId)
        {
            var tokenInfo = new TelegramForumLinkTokenInfo{
                TelegramUserId = telegramUserId, 
                Token = Guid.NewGuid().ToString("n")
            };
            TokensCache.RemoveAll(t => t.TelegramUserId == telegramUserId);
            TokensCache.Add(tokenInfo);
            return tokenInfo.Token;
        }

        public bool ValidateAndLink(int forumUserId, string messageText)
        {
            if(forumUserId == _forumBotUserId)
            {
                return false;
            }

            if(TokensCache.Count <= 0)
            {
                return false;
            }

            if(messageText.Trim().Length != 32)
            {
                return false;
            }

            var tokenInfo = TokensCache.FirstOrDefault(t => t.Token.Equals(messageText.Trim(), StringComparison.OrdinalIgnoreCase));
            if(tokenInfo == null)
            {
                return false;
            }

            var user = _userStore.Users.FirstOrDefault(u => u.UserId == forumUserId);
            if(user == null)
            {
                return false;
            }

            user.UserTelegramId = tokenInfo.TelegramUserId;
            _userStore.UpdateUser(user);
            TokensCache.Remove(tokenInfo);

            UserLinked?.Invoke(this, new TelegramToForumLinkArgumentArgs{TelegramUserId = tokenInfo.TelegramUserId, ForumUser = user});

            return true;
        }

        public bool Unlink(long telegramUserId)
        {
            var user = _userStore.Users.FirstOrDefault(u => u.UserTelegramId == telegramUserId);
            if(user == null)
            {
                return false;
            }

            user.UserTelegramId = null;
            _userStore.UpdateUser(user);
            UserUnlinked?.Invoke(this, new TelegramToForumLinkArgumentArgs{TelegramUserId = telegramUserId, ForumUser = user});

            return true;
        }
    }

    public class TelegramForumLinkTokenInfo
    {
        public long TelegramUserId {get;set;}
        public string Token {get;set;}
    }

    public class TelegramToForumLinkArgumentArgs
    {
        public long TelegramUserId {get;set;}
        public PhpbbUsers ForumUser {get;set;}
    }
}