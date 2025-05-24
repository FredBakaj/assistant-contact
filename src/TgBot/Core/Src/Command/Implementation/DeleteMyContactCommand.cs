using AssistantContract.Application.UseCase.Contact.Commands.DeleteContact;
using AssistantContract.TgBot.Core.Extension;
using AssistantContract.TgBot.Core.Field;
using AssistantContract.TgBot.Core.Model;
using MediatR;
using System.Globalization;
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
        var splitMessage = update.Message!.Text!.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        // Check if contact number is provided
        if (splitMessage.Length < 2)
        {
            await _telegramBotClient.SendMessage(
                chatId: update.GetUserId(),
                text: "❌ Please provide a contact number.\n\n" +
                      $"<i>Example:</i> /{CommandField.DeleteMyContact} 1",
                parseMode: ParseMode.Html);
            return;
        }

        // Validate that the parameter is a valid number
        if (!int.TryParse(splitMessage[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out int contactNumber))
        {
            await _telegramBotClient.SendMessage(
                chatId: update.GetUserId(),
                text: "❌ Invalid contact number. Please provide a valid number.\n\n" +
                      $"<i>Example:</i> /{CommandField.DeleteMyContact} 1",
                parseMode: ParseMode.Html);
            return;
        }

        try
        {
            await _sender.Send(
                new DeleteContactCommand() { UserId = update.GetUserId(), ContactNumber = contactNumber });

            await _telegramBotClient.SendMessage(
                chatId: update.GetUserId(),
                text: "✅ The contact has been deleted.",
                parseMode: ParseMode.Html);
        }
        catch (Exception ex)
        {
            await _telegramBotClient.SendMessage(
                chatId: update.GetUserId(),
                text: $"❌ Failed to delete contact. {ex.Message}",
                parseMode: ParseMode.Html);
        }
    }
}
