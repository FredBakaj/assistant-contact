using AssistantContract.TgBot.Core.Src.Middleware;
using AssistantContract.TgBot.Core.Src.Middleware.Implementation;
using AssistantContract.TgBot.Core.Src.Middleware.Implementation.Group;
using AssistantContract.TgBot.Core.Src.Middleware.Implementation.Main;
using AssistantContract.TgBot.Core.Src.Middleware.Implementation.Private;

namespace AssistantContract.TgBot.Di.Middleware
{
    public class MiddlewareBuild
    {
        public static void BuildService(IServiceCollection services)
        {
            services.AddTransient<IBotMiddleware, SpamBlockerMiddleware>();
            services.AddTransient<IBotMiddleware, InitializationMiddleware>();
            services.AddTransient<IBotMiddleware, CommandMiddleware>();
            services.AddTransient<IBotMiddleware, StateMiddleware>();
            services.AddTransient<IBotMiddleware, RouteChatTypeMiddleware>();
            services.AddTransient<IBotMiddleware, RouteMiddleware>();
            services.AddTransient<IBotMiddleware, GroupProcessingMessageMiddleware>();
        }
    }
}
