using AssistantContract.TgBot.Core.Handler.BotStateTreeHandler;
using AssistantContract.TgBot.Core.Handler.BotStateTreeHandler.Implementation;
using AssistantContract.TgBot.Core.Handler.BotStateTreeUserHandler;
using AssistantContract.TgBot.Core.Handler.BotStateTreeUserHandler.Implementation;
using AssistantContract.TgBot.Core.Handler.BotViewHandler;
using AssistantContract.TgBot.Core.Handler.BotViewHandler.Implementation;
using AssistantContract.TgBot.Core.Handler.MiddlewareHandler;
using AssistantContract.TgBot.Core.Handler.MiddlewareHandler.Implementation;
using AssistantContract.TgBot.Core.Handler.TaskProcessingHandler;
using AssistantContract.TgBot.Core.Handler.TaskProcessingHandler.Implementation;
using AssistantContract.TgBot.Extension;

namespace AssistantContract.TgBot.Di.Handler;

public class HandlerBuild
{
    public static void BuildService(IServiceCollection services)
    {
        services.AddScopedWithAlias<IBotStateTreeHandler, BotStateTreeHandler>();

        services.AddScopedWithAlias<IBotMiddlewareHandler, MainMiddlewareHandler>();
        services.AddScopedWithAlias<IRouteMiddlewareHandler, GroupMiddlewareHandler>();
        services.AddScopedWithAlias<IRouteMiddlewareHandler, PrivateMiddlewareHandler>();
        services.AddScopedWithAlias<IBotViewHandler, BotViewHandler>();
        services.AddScopedWithAlias<IBotStateTreeUserHandler, BotStateTreeUserHandler>();

        services.AddSingleton<IBackgroundTaskHandler, BackgroundTaskHandler>();
    }
}
