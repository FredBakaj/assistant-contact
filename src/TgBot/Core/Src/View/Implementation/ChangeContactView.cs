using AssistantContract.TgBot.Core.Attribute;
using AssistantContract.TgBot.Core.Extension;
using AssistantContract.TgBot.Core.Field.Controller;
using AssistantContract.TgBot.Core.Field.View;
using AssistantContract.TgBot.Core.Model;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace AssistantContract.TgBot.Core.Src.View.Implementation;

public class ChangeContactView : ABotView
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<ChangeContactView> _logger;

    public ChangeContactView(ITelegramBotClient botClient, ILogger<ChangeContactView> logger)
    {
        _botClient = botClient;
        _logger = logger;
    }

    [BotView(ChangeContactViewField.ChangeContactInputDescriptionAction)]
    public async Task InputDescriptionAction(UpdateBDto update)
    {
        var text = "Enter new description";
        var keyboard = new ReplyKeyboardMarkup(
            new[] { new KeyboardButton(ChangeContactField.ChangeContactSkipInputDescriptionKeyboard) }
        ) { ResizeKeyboard = true };
        await _botClient.SendMessage(update.GetUserId(), text, replyMarkup: keyboard);
    }

    [BotView(ChangeContactViewField.ChangeContactInputTimeSpan)]
    public async Task InputTimeSpan(UpdateBDto update)
    {
        var text = "Enter new notification time span";
        var keyboard = new InlineKeyboardMarkup(new[]
        {
            new List<InlineKeyboardButton>()
            {
                new InlineKeyboardButton()
                {
                    Text = "3 day", CallbackData = $"{ChangeContactField.InputTimeSpanCallback}:3"
                },
                new InlineKeyboardButton()
                {
                    Text = "1 week", CallbackData = $"{ChangeContactField.InputTimeSpanCallback}:7"
                },
                new InlineKeyboardButton()
                {
                    Text = "2 week", CallbackData = $"{ChangeContactField.InputTimeSpanCallback}:14"
                },
                new InlineKeyboardButton()
                {
                    Text = "1 month", CallbackData = $"{ChangeContactField.InputTimeSpanCallback}:28"
                },
            },
            new List<InlineKeyboardButton>()
            {
                new InlineKeyboardButton()
                {
                    Text = "Skip", CallbackData = $"{ChangeContactField.SkipInputTimeSpanCallback}"
                },
            }
        });
        await _botClient.SendMessage(update.GetUserId(), text, replyMarkup: keyboard);
    }

    [BotView(ChangeContactViewField.ChangeContactSaveChangeContact)]
    public async Task SaveChangeContact(UpdateBDto update)
    {
        var text = "Save the changes?";
        var keyboard = new ReplyKeyboardMarkup(
            new[]
            {
                new KeyboardButton(ChangeContactField.ChangeContactSavedContactKeyboard),
                new KeyboardButton(ChangeContactField.ChangeContactCanceledSaveContactKeyboard)
            }
        ) { ResizeKeyboard = true };
        await _botClient.SendMessage(update.GetUserId(), text, replyMarkup: keyboard);
    }

    [BotView(ChangeContactViewField.ChangeContactSavedContact)]
    public async Task SavedContact(UpdateBDto update)
    {
        var text = "changes have been saved";
        await _botClient.SendMessage(update.GetUserId(), text, replyMarkup: new ReplyKeyboardRemove());
    }

    [BotView(ChangeContactViewField.ChangeContactCanceledSaveContact)]
    public async Task CanceledSaveContact(UpdateBDto update)
    {
        var text = "the changes were not saved";
        await _botClient.SendMessage(update.GetUserId(), text, replyMarkup: new ReplyKeyboardRemove());
    }
}
