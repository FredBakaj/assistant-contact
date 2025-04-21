using AssistantContract.TgBot.Core.Factory;
using AssistantContract.TgBot.Core.Factory.Implementation;
using AssistantContract.TgBot.Core.Handler.MiddlewareHandler;
using AssistantContract.TgBot.Core.Src.Command;
using AssistantContract.TgBot.Core.Src.Controller;

namespace AssistantContract.TgBot.Di.Factory;

public class FactoryBuild
{
    public static void BuildService(IServiceCollection services)
    {
        services.AddTransient<IFactory<IBotController>, ControllerFactory>();
        services.AddTransient<IFactory<IBotCommand>, CommandFactory>();
        services.AddTransient<IFactory<IRouteMiddlewareHandler>, RouteMiddlewareHandlerFactory>();
    }
}
