using AssistantContract.TgBot.Core.Src.Middleware;
using AssistantContract.TgBot.Core.Src.Middleware.Implementation.Group;
using AssistantContract.TgBot.Core.Src.Middleware.Implementation.Private;
using AssistantContract.TgBot.Extension;

namespace AssistantContract.TgBot.Core.Handler.MiddlewareHandler.Implementation;

public class PrivateMiddlewareHandler : ABotMiddlewareHandler, IRouteMiddlewareHandler
{
    public PrivateMiddlewareHandler(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    protected override void Bind()
    {
        // Обработчик сообщений в группе
        AddMiddleware(_serviceProvider.GetSpecificService<IBotMiddleware, RouteMiddleware>());
    }
}
