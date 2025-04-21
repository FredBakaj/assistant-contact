using AssistantContract.TgBot.Core.Field;
using AssistantContract.TgBot.Core.Field.Controller;
using AssistantContract.TgBot.Core.Handler.BotStateTreeUserHandler;
using AssistantContract.TgBot.Core.Model;

namespace AssistantContract.TgBot.Core.Src.Command.Implementation;

public class AddContactCommand : IBotCommand
{
    private readonly IBotStateTreeUserHandler _botStateTreeUserHandler;
    public string GetCommand() => CommandField.AddContact;

    public bool IsMoveNext() => true;

    public AddContactCommand(IBotStateTreeUserHandler botStateTreeUserHandler)
    {
        _botStateTreeUserHandler = botStateTreeUserHandler;
    }

    public async Task Exec(UpdateBDto update)
    {
        await _botStateTreeUserHandler.SetStateAndActionAsync(update, AddContactField.AddContactState,
            AddContactField.AddContactAction);
    }
}
