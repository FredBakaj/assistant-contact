using System.Reflection;
using AssistantContract.Application;
using AssistantContract.Infrastructure;
using AssistantContract.TgBot.Core.Handler.NotificationHandler.Publisher.Implementation;
using AssistantContract.TgBot.Core.Service;
using AssistantContract.TgBot.Core.Service.Implementation;
using AssistantContract.TgBot.Di.Command;
using AssistantContract.TgBot.Di.Controller;
using AssistantContract.TgBot.Di.Factory;
using AssistantContract.TgBot.Di.Handler;
using AssistantContract.TgBot.Di.Manager;
using AssistantContract.TgBot.Di.Middleware;
using AssistantContract.TgBot.Di.View;
using AssistantContract.TgBot.Mapping;
using MediatR;
using Telegram.Bot;

namespace AssistantContract.TgBot.Di
{
    public class ServicesBuild
    {
        public static void BuildService(WebApplicationBuilder builder)
        {
            IServiceCollection services = builder.Services;
            IConfiguration configuration = builder.Configuration;

            services.AddSingleton<ITelegramBotClient, TelegramBotClient>(
                provider => new TelegramBotClient(
                    provider.GetService<IConfiguration>()?.GetSection("CommonSettings")["BotToken"] ??
                    throw new InvalidOperationException()));

            //Background service
            services.AddScoped<IUpdateHandler, UpdateHandler>();
            if (Convert.ToBoolean(configuration.GetSection("CommonSettings")["IsWebHook"]))
            {
                services.AddHostedService<ConfigureWebhook>();
                services.AddHostedService<ResetWebhookService>();
                services.ConfigureTelegramBotMvc();
            }
            else
            {
                services.AddScoped<ReceiverService>();
                services.AddHostedService<PollingService>();
            }

            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);

            //Asp service
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddControllers();
            services.AddAutoMapper(typeof(MapperProfile));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddSingleton<IPublisher, PrioritizedNotificationPublisher>();

            //Custom service
            services.AddScoped<ILogger, Logger<ServicesBuild>>();
            ManagerBuild.BuildService(services);
            MiddlewareBuild.BuildService(services);
            ControllerBuild.BuildService(services);
            ViewBuild.BuildService(services);
            CommandBuild.BuildService(services);
            FactoryBuild.BuildService(services);
            HandlerBuild.BuildService(services);
        }
    }
}
