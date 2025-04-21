using AssistantContract.TgBot.Core.Attribute;
using AssistantContract.TgBot.Core.Extension;
using AssistantContract.TgBot.Core.Field.View;
using AssistantContract.TgBot.Core.Model;
using Telegram.Bot;

namespace AssistantContract.TgBot.Core.Src.View.Implementation
{
    public class BaseBotView : ABotView
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<BaseBotView> _logger;

        public BaseBotView(ITelegramBotClient botClient,
            ILogger<BaseBotView> logger)
        {
            _botClient = botClient;
            _logger = logger;
        }

        [BotView(BaseViewField.Menu)]
        public async Task Intro(UpdateBDto update)
        {
            var text = "Головне меню 📺";
            await _botClient.SendMessage(update.GetUserId(), text);
        }
    }
}
