using AssistantContract.TgBot.Core.Attribute;
using AssistantContract.TgBot.Core.Extension;
using AssistantContract.TgBot.Core.Field.Controller;
using AssistantContract.TgBot.Core.Field.View;
using AssistantContract.TgBot.Core.Model;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace AssistantContract.TgBot.Core.Src.View.Implementation;

public class StartBotView : ABotView
{
    private readonly ITelegramBotClient _botClient;

    public StartBotView(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    [BotView(StartViewField.Start)]
    public async Task StartAsync(UpdateBDto update)
    {
        var text = "Привет";

        var replyMarkup =
            new ReplyKeyboardMarkup(new[] { new KeyboardButton[] { StartField.CompleteStartButton, } })
            {
                ResizeKeyboard = true
            };
        await _botClient.SendTextMessageMarkdown2Async(update.GetUserId(), text, replyMarkup: replyMarkup);
    }
}
