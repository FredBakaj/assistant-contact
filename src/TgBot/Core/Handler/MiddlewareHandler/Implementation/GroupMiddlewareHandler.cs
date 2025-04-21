using AssistantContract.TgBot.Core.Src.Middleware;
using AssistantContract.TgBot.Core.Src.Middleware.Implementation.Group;
using AssistantContract.TgBot.Extension;

namespace AssistantContract.TgBot.Core.Handler.MiddlewareHandler.Implementation;

public class GroupMiddlewareHandler: ABotMiddlewareHandler, IRouteMiddlewareHandler
{
    public GroupMiddlewareHandler(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    protected override void Bind()
    {
        // Обработчик сообщений в группе
        AddMiddleware(_serviceProvider.GetSpecificService<IBotMiddleware, GroupProcessingMessageMiddleware>());
    }
}
