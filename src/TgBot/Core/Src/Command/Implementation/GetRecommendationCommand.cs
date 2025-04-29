using AssistantContract.Application.UseCase.Contact.Queries.GetRecommendation;
using AssistantContract.TgBot.Core.Extension;
using AssistantContract.TgBot.Core.Field;
using AssistantContract.TgBot.Core.Model;
using MediatR;
using Telegram.Bot;

namespace AssistantContract.TgBot.Core.Src.Command.Implementation;

public class GetRecommendationCommand : IBotCommand
{
    private readonly ISender _sender;
    private readonly ITelegramBotClient _telegramBotClient;
    public string GetCommand() => CommandField.GetRecommendation;

    public bool IsMoveNext() => false;


    public GetRecommendationCommand(ISender sender, ITelegramBotClient telegramBotClient)
    {
        _sender = sender;
        _telegramBotClient = telegramBotClient;
    }

    public async Task Exec(UpdateBDto update)
    {
        var splitMessage = update.Message!.Text!.Split(" ");
        var contactNumber = int.Parse(splitMessage[1]);
        var recommendation =
            await _sender.Send(new RecommendationQuery()
            {
                UserId = update.GetUserId(), ContactNumber = contactNumber
            });
        await _telegramBotClient.SendMessage(update.GetUserId(), recommendation.RecommendationText);
    }
}
