using AssistantContract.TgBot.Core.Field.Controller;
using AssistantContract.TgBot.Core.Field.View;
using AssistantContract.TgBot.Core.Handler.BotStateTreeHandler;
using AssistantContract.TgBot.Core.Handler.BotStateTreeUserHandler;
using AssistantContract.TgBot.Core.Handler.BotViewHandler;
using AssistantContract.TgBot.Core.Model;
using MediatR;

namespace AssistantContract.TgBot.Core.Src.Controller.Implementation;

public class StartController : IBotController
{
    private readonly IBotStateTreeHandler _botStateTreeHandler;
    private readonly IBotStateTreeUserHandler _botStateTreeUserHandler;
    private readonly IBotViewHandler _botViewHandler;
    private readonly ISender _sender;
    public string Name() => StartField.StartState;

    public StartController(
        IBotStateTreeHandler botStateTreeHandler,
        IBotStateTreeUserHandler botStateTreeUserHandler,
        IBotViewHandler botViewHandler,
        ISender sender)
    {
        _botStateTreeHandler = botStateTreeHandler;
        _botStateTreeUserHandler = botStateTreeUserHandler;
        _botViewHandler = botViewHandler;
        _sender = sender;

        Initialize();
    }

    private void Initialize()
    {
        _botStateTreeHandler.AddAction(StartField.StartAction, StartActionAsync);
    }

    public async Task Exec(UpdateBDto update)
    {
        await _botStateTreeHandler.ExecuteRoute(update);
    }

    private async Task StartActionAsync(UpdateBDto update)
    {
        await _botStateTreeUserHandler.SetStateAndActionAsync(update, BaseField.BaseState,
            BaseField.BaseAction);
        await _botViewHandler.SendAsync(StartViewField.Start, update);
    }
}
