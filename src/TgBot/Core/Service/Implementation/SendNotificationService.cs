using AssistantContract.Application.UseCase.User.Commands.SendNotification;
using MediatR;
using System.Text;
using AssistantContract.TgBot.Core.Field;
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

    private async Task SendToTelegram(long userId, int contactNumber, string contactName, string personalInfo)
    {
        var message = new StringBuilder();
        message.AppendLine("🔔 *Contact Reminder*\n");
        
        // Contact info
        message.AppendLine($"📛 *Name:* {contactName}");

        if (!string.IsNullOrEmpty(personalInfo))
        {
            message.AppendLine($"📱 *Contact Info:* {personalInfo}");
        }
        
        // Quick action
        message.AppendLine("\n💬 *Quick Action:*");
        message.AppendLine($"`/{CommandField.GetRecommendation} {contactNumber}`");
        
        await _telegramBotClient.SendMessage(
            chatId: userId,
            text: message.ToString(),
            parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
    }
}
