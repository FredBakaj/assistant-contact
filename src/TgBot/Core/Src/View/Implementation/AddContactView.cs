using AssistantContract.TgBot.Core.Attribute;
using AssistantContract.TgBot.Core.Extension;
using AssistantContract.TgBot.Core.Field.Controller;
using AssistantContract.TgBot.Core.Field.View;
using AssistantContract.TgBot.Core.Model;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace AssistantContract.TgBot.Core.Src.View.Implementation;

public class AddContactView : ABotView
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<AddContactView> _logger;

    public AddContactView(ITelegramBotClient botClient, ILogger<AddContactView> logger)
    {
        _botClient = botClient;
        _logger = logger;
    }

    [BotView(AddContactViewField.InputName)]
    public async Task InputName(UpdateBDto update)
    {
        var text = "Enter a name";
        await _botClient.SendMessage(update.GetUserId(), text);
    }


    [BotView(AddContactViewField.InputPersonalInfo)]
    public async Task InputPersonalInfo(UpdateBDto update)
    {
        var text = "🔍 Enter personal info to further identify the contact.\n\n" +
                 "This field is universal - you can enter:\n" +
                 "• Phone number\n" +
                 "• Username\n" +
                 "• Email\n" +
                 "• Or any other identifier";
        var keyboard =
            new ReplyKeyboardMarkup(new KeyboardButton(AddContactField.SkipInputPersonalInfoKeyboard))
            {
                ResizeKeyboard = true
            };
        await _botClient.SendMessage(update.GetUserId(), text, replyMarkup: keyboard);
    }

    [BotView(AddContactViewField.InputDescription)]
    public async Task InputDescription(UpdateBDto update)
    {
        var text = "Enter a description (This description will be used to determine recommendations for the contact)";
        await _botClient.SendMessage(update.GetUserId(), text, replyMarkup: new ReplyKeyboardRemove());
    }

    [BotView(AddContactViewField.SaveContact)]
    public async Task SaveContact(UpdateBDto update)
    {
        var text = "Save a new contact?";
        var keyboard = new ReplyKeyboardMarkup(
            new[]
            {
                new KeyboardButton(AddContactField.SavedContactKeyboard),
                new KeyboardButton(AddContactField.CanceledSaveContactKeyboard)
            }
        ) { ResizeKeyboard = true };
        await _botClient.SendMessage(update.GetUserId(), text, replyMarkup: keyboard);
    }

    [BotView(AddContactViewField.SavedContact)]
    public async Task SavedContact(UpdateBDto update)
    {
        var text = "Contact has been maintained";
        await _botClient.SendMessage(update.GetUserId(), text, replyMarkup: new ReplyKeyboardRemove());
    }

    [BotView(AddContactViewField.CanceledSaveContact)]
    public async Task CanceledSaveContact(UpdateBDto update)
    {
        var text = "Saving a contact has been canceled";
        await _botClient.SendMessage(update.GetUserId(), text, replyMarkup: new ReplyKeyboardRemove());
    }

    [BotView(AddContactViewField.InputTimeSpan)]
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
            }
        });
        await _botClient.SendMessage(update.GetUserId(), text, replyMarkup: keyboard);
    }
}
