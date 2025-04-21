using AssistantContract.TgBot.Core.Src.Middleware;
using AssistantContract.TgBot.Core.Src.Middleware.Implementation.Main;
using AssistantContract.TgBot.Extension;

namespace AssistantContract.TgBot.Core.Handler.MiddlewareHandler.Implementation
{
    public class MainMiddlewareHandler : ABotMiddlewareHandler
    {

        public MainMiddlewareHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            
        }

     

        protected override void Bind()
        {
            //Последовательность вызовов AddMiddleware важна!
            //Спам блокер 
            //AddMiddleware(_serviceProvider.GetSpecificService<IBotMiddleware, SpamBlockerMiddleware>());
            //Реализует первичную инициализацию телеграмм аккаунта при запуске бота
            AddMiddleware(_serviceProvider.GetSpecificService<IBotMiddleware, InitializationMiddleware>());
            // Обработчик команд (пример: /start)
            AddMiddleware(_serviceProvider.GetSpecificService<IBotMiddleware, CommandMiddleware>());
            // Определяет в каком сейчас стейте находиться пользователь
            AddMiddleware(_serviceProvider.GetSpecificService<IBotMiddleware, StateMiddleware>());
            // Определяет в какой обект передать обработку сообщения от пользователя
            // в зависимости от состояния
            // Нужен для редиректа на контролеры для обработчика чата
            // AddMiddleware(_serviceProvider.GetSpecificService<IBotMiddleware, RouteMiddleware>());
            AddMiddleware(_serviceProvider.GetSpecificService<IBotMiddleware, RouteChatTypeMiddleware>());
        }
    }
}
