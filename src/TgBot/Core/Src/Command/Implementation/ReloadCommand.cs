using AssistantContract.TgBot.Core.Extension;
using AssistantContract.TgBot.Core.Field;
using AssistantContract.TgBot.Core.Field.Controller;
using AssistantContract.TgBot.Core.Handler.BotStateTreeUserHandler;
using AssistantContract.TgBot.Core.Handler.TaskProcessingHandler;
using AssistantContract.TgBot.Core.Model;
using Telegram.Bot;

namespace AssistantContract.TgBot.Core.Src.Command.Implementation;

public class ReloadCommand : IBotCommand
{
    private readonly ITelegramBotClient _client;
    private readonly IBotStateTreeUserHandler _botStateTreeUserHandler;
    private readonly IBackgroundTaskHandler _backgroundTaskHandler;
    public string GetCommand() => CommandField.Reload;

    public bool IsMoveNext() => true;

    public ReloadCommand(ITelegramBotClient client,
        IBotStateTreeUserHandler botStateTreeUserHandler,
        IBackgroundTaskHandler backgroundTaskHandler)
    {
        _client = client;
        _botStateTreeUserHandler = botStateTreeUserHandler;
        _backgroundTaskHandler = backgroundTaskHandler;
    }

    public async Task Exec(UpdateBDto update)
    {
        // TODO Вынести в отдельный сервис, для закрытия который будет подписываться на события
        if (await _backgroundTaskHandler.IsProcessRunningAsync(update.GetUserId(),
                TaskProcessingField.SearchNewUserMessage))
            await _backgroundTaskHandler.StopProcessAsync(update.GetUserId(),
                TaskProcessingField.SearchNewUserMessage);
        
        if (await _backgroundTaskHandler.IsProcessRunningAsync(update.GetUserId(),
                TaskProcessingField.GenerateReplyToUserMessage))
            await _backgroundTaskHandler.StopProcessAsync(update.GetUserId(),
                TaskProcessingField.GenerateReplyToUserMessage);

        await _botStateTreeUserHandler.SetStateAndActionAsync(update, BaseField.BaseState, BaseField.ReloadAction,
            CancellationToken.None);
        await _client.SendMessage(update.GetUserId(), "Бот перезавантажений 🏗");
    }
}
