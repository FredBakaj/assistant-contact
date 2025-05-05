using AssistantContract.TgBot.Core.Extension;
using AssistantContract.TgBot.Core.Field;
using AssistantContract.TgBot.Core.Field.Controller;
using AssistantContract.TgBot.Core.Handler.BotStateTreeUserHandler;
using AssistantContract.TgBot.Core.Model;
using Telegram.Bot;

namespace AssistantContract.TgBot.Core.Src.Command.Implementation;

public class ChangeContactCommand : IBotCommand
{
    private readonly IBotStateTreeUserHandler _botStateTreeUserHandler;
    private readonly ITelegramBotClient _telegramBotClient;
    public string GetCommand() => CommandField.ChangeContact;

    public bool IsMoveNext() => true;

    public ChangeContactCommand(IBotStateTreeUserHandler botStateTreeUserHandler, ITelegramBotClient telegramBotClient)
    {
        _botStateTreeUserHandler = botStateTreeUserHandler;
        _telegramBotClient = telegramBotClient;
    }

    public async Task Exec(UpdateBDto update)
    {
        var splitMessage = update.Message!.Text!.Split(" ");
        if (splitMessage.Length < 2)
        {
            var text = "Enter the number of the contact";
            await _telegramBotClient.SendMessage(update.GetUserId(), text);
        }
        else
        {
            await _botStateTreeUserHandler.SetStateAndActionAsync(update, ChangeContactField.ChangeContactState,
                ChangeContactField.ChangeContactAction);
        }
    }
}
