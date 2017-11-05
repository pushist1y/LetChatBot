using Telegram.Bot.Types;

namespace LetChatBot
{
    public static class TelegramBotExtensions
    {
        public static string FullName(this User user)
        {
                var telegramName = user.FirstName;
                if(!string.IsNullOrEmpty(user.LastName))
                {
                    telegramName += " " + user.LastName;
                }

                return telegramName;
        }
    }
}