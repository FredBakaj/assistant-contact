using AssistantContract.Application.Common.Interfaces;
using AssistantContract.Infrastructure.Media;
using AssistantContract.Infrastructure.Utils.RestApiClient;
using AssistantContract.Infrastructure.Utils.RestApiClient.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace AssistantContract.Infrastructure.DI.Manager;

public class ManagerBuild
{
    public static void BuildService(IServiceCollection services)
    {
        services.AddScoped<IConfig, Config.Config>();
        services.AddScoped<IRestApiClient, RestApiClient>();
        services.AddScoped<IVideoManager, VideoManager>();
    }
}
