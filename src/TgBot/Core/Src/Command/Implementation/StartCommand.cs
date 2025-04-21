using AssistantContract.TgBot.Core.Field;
using AssistantContract.TgBot.Core.Field.Controller;
using AssistantContract.TgBot.Core.Handler.BotStateTreeUserHandler;
using AssistantContract.TgBot.Core.Model;
using MediatR;

namespace AssistantContract.TgBot.Core.Src.Command.Implementation;

public class StartCommand : IBotCommand
{
    private readonly ISender _sender;
    private readonly IBotStateTreeUserHandler _botStateTreeUserHandler;
    public string GetCommand() => CommandField.Start;
    public bool IsMoveNext() => true;

    public StartCommand(ISender sender, IBotStateTreeUserHandler botStateTreeUserHandler)
    {
        _sender = sender;
        _botStateTreeUserHandler = botStateTreeUserHandler;
    }

    public async Task Exec(UpdateBDto telegramUpdate)
    {
        await _botStateTreeUserHandler.SetStateAndActionAsync(telegramUpdate, StartField.StartState,
            StartField.StartAction);
    }
}
