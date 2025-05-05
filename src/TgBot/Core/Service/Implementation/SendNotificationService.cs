using AssistantContract.Application.UseCase.User.Commands.SendNotification;
using MediatR;
using Telegram.Bot;

namespace AssistantContract.TgBot.Core.Service.Implementation;

public class SendNotificationService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ISender _sender;
    private readonly ITelegramBotClient _telegramBotClient;

    public SendNotificationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        var scope = _serviceProvider.CreateScope();
        _sender = scope.ServiceProvider.GetRequiredService<ISender>();
        _telegramBotClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await _sender.Send(new SendNotificationCommand() { Action = SendToTelegram }, stoppingToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }

    private async Task SendToTelegram(long userId, int contactNumber, string contactName)
    {
        var text = $"Time to email your contact \nContact number: {contactNumber} \nName: {contactName}";
        await _telegramBotClient.SendMessage(userId, text);
    }
}
