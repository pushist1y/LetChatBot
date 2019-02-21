namespace LetChatBot
{
    public interface ITelegramCommandProcessor
    {
        TelegramCommandProcessResult Process();
    }

    public class TelegramCommandProcessResult
    {
        public bool Processed {get;set;}
    }
}