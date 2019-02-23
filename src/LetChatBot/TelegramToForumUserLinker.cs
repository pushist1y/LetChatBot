using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace LetChatBot
{
    public class TelegramToForumUserLinker
    {
        private readonly ForumLinkRepository _linkRepository;
        private readonly TelegramAccessService _telegramAccessService;
        private readonly int _forumBotUserId;

        public TelegramToForumUserLinker(ForumLinkRepository linkRepository, IConfiguration config, TelegramAccessService telegramAccessService)
        {
            _linkRepository = linkRepository;
            _telegramAccessService = telegramAccessService;
            _forumBotUserId = Convert.ToInt32(config["ForumBotUserId"]);
        }

        private static readonly SemaphoreSlim TokensSemaphore = new SemaphoreSlim(1, 1);
        private static readonly Dictionary<string, TelegramForumLinkTokenInfo> TokensCache = new Dictionary<string, TelegramForumLinkTokenInfo>();

        public async Task<string> IssueToken(long telegramUserId)
        {
            var tokenInfo = new TelegramForumLinkTokenInfo{
                TelegramUserId = telegramUserId, 
                Token = Guid.NewGuid().ToString("n")
            };

            await TokensSemaphore.WaitAsync();
            try
            {
                var existingKeys = TokensCache.Where(kv => kv.Value.TelegramUserId == telegramUserId).Select(kv => kv.Key);
                foreach (var key in existingKeys)
                {
                    TokensCache.Remove(key);
                }

                TokensCache[tokenInfo.Token] = tokenInfo;
                return tokenInfo.Token;
            }
            finally
            {
                TokensSemaphore.Release();
            }
        }

        public async Task<bool> ValidateAndLink(uint forumUserId, string messageText)
        {
            if(forumUserId == _forumBotUserId)
            {
                return false;
            }

            if(TokensCache.Count <= 0)
            {
                return false;
            }

            var token = messageText.Trim();

            if(token.Length != 32)
            {
                return false;
            }

            TelegramForumLinkTokenInfo tokenInfo;
            await TokensSemaphore.WaitAsync();
            try
            {
                if (!TokensCache.TryGetValue(token, out tokenInfo))
                {
                    return false;
                }
            }
            finally
            {
                TokensSemaphore.Release();
            }


            var forumUser = await _linkRepository.SetUserLink(forumUserId, tokenInfo.TelegramUserId);
            if (forumUser == null)
            {
                return false;
            }

            var text = $"�� ������� ������� ���� ������� ������ Telegram � ��������� [{forumUser.Username}] �� ������";
            await _telegramAccessService.SendToUserAsync(text, Convert.ToString(tokenInfo.TelegramUserId));

            return true;
        }

       
    }

    public class TelegramForumLinkTokenInfo
    {
        public long TelegramUserId {get;set;}
        public string Token {get;set;}
    }

}