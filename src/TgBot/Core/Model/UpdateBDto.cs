using Telegram.Bot.Types;

namespace AssistantContract.TgBot.Core.Model
{
    public class UpdateBDto : Update
    {
        //TODO Сделать TelegramState с полем дата и дженериком
        public StateTreeBDto? TelegramState { get; set; }
        public string CallbackData { get; set; } = null!;
    }
}
