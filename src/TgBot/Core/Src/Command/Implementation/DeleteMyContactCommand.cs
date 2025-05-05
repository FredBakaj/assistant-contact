using AssistantContract.Application.UseCase.Contact.Commands.DeleteContact;
using AssistantContract.TgBot.Core.Extension;
using AssistantContract.TgBot.Core.Field;
using AssistantContract.TgBot.Core.Model;
using MediatR;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace AssistantContract.TgBot.Core.Src.Command.Implementation;

public class DeleteMyContactCommand : IBotCommand
{
    private readonly ISender _sender;
    private readonly ITelegramBotClient _telegramBotClient;
    public string GetCommand() => CommandField.DeleteMyContact;

    public bool IsMoveNext() => false;

    public DeleteMyContactCommand(ISender sender, ITelegramBotClient telegramBotClient)
    {
        _sender = sender;
        _telegramBotClient = telegramBotClient;
    }

    public async Task Exec(UpdateBDto update)
    {
        var splitMessage = update.Message!.Text!.Split(" ");
        var contactNumber = int.Parse(splitMessage[1]);

        await _sender.Send(new DeleteContactCommand() { UserId = update.GetUserId(), ContactNumber = contactNumber });

        var text = $"the contact has been deleted";
        await _telegramBotClient.SendMessage(update.GetUserId(), text, parseMode: ParseMode.Html);
    }
}
