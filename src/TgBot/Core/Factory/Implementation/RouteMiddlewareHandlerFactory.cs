using AssistantContract.TgBot.Core.Handler.MiddlewareHandler;

namespace AssistantContract.TgBot.Core.Factory.Implementation;

public class RouteMiddlewareHandlerFactory: IFactory<IRouteMiddlewareHandler>
{
    private readonly IServiceProvider _serviceProvider;

    public RouteMiddlewareHandlerFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task<IRouteMiddlewareHandler> CreateAsync(object value)
    {
        Type value_ = (value as Type)!;


        // Проверяем, что value_ не null, чтобы избежать исключений
        if (value_ != null)
        {
            return Task.FromResult((IRouteMiddlewareHandler)_serviceProvider.GetRequiredService(value_));
        }

        throw new InvalidOperationException("Value is not a valid type.");
    }
}
