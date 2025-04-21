using AssistantContract.TgBot.Core.Handler.BotStateTreeUserHandler;
using AssistantContract.TgBot.Core.Model;

namespace AssistantContract.TgBot.Core.Src.Middleware.Implementation.Main
{
    /// <summary>
    /// Определяет состояние пользователя
    /// </summary>
    public class StateMiddleware : ABotMiddleware
    {
        private readonly IBotStateTreeUserHandler _botStateTreeUserHandler;


        public StateMiddleware(IBotStateTreeUserHandler botStateTreeUserHandler)
        {
            _botStateTreeUserHandler = botStateTreeUserHandler;
        }

        public override async Task Next(UpdateBDto update)
        {
            if (update.TelegramState == null)
            {
                var userModel = await _botStateTreeUserHandler.GetStateAndActionAsync(update);
                update.TelegramState =
                    new StateTreeBDto() { State = userModel!.State, Action = userModel!.Action };
            }

            await base.Next(update);
        }
    }
}
