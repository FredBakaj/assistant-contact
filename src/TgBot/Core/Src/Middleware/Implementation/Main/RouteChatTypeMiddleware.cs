using AssistantContract.TgBot.Core.Extension;
using AssistantContract.TgBot.Core.Factory;
using AssistantContract.TgBot.Core.Handler.MiddlewareHandler;
using AssistantContract.TgBot.Core.Handler.MiddlewareHandler.Implementation;
using AssistantContract.TgBot.Core.Model;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace AssistantContract.TgBot.Core.Src.Middleware.Implementation.Main;

public class RouteChatTypeMiddleware : ABotMiddleware
{
    private readonly IFactory<IRouteMiddlewareHandler> _factoryRoutMiddlewareConstructor;


    public RouteChatTypeMiddleware(IFactory<IRouteMiddlewareHandler> factoryRoutMiddlewareConstructor)
    {
        _factoryRoutMiddlewareConstructor = factoryRoutMiddlewareConstructor;
    }

    public override async Task Next(UpdateBDto update)
    {
        if (update.TryGetMessage(out Message message) && message.Chat != null)
        {
            if (message.Chat.Type == ChatType.Private)
            {
                var middlewareConstructor =
                    await _factoryRoutMiddlewareConstructor.CreateAsync(typeof(PrivateMiddlewareHandler));
                await middlewareConstructor.Run(update);
            }

            if (message.Chat.Type == ChatType.Supergroup || message.Chat.Type == ChatType.Group)
            {
                var middlewareConstructor =
                    await _factoryRoutMiddlewareConstructor.CreateAsync(typeof(GroupMiddlewareHandler));
                await middlewareConstructor.Run(update);
            }
        }

        await base.Next(update);
    }
}
