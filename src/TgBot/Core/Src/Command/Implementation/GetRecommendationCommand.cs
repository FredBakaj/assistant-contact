using AssistantContract.Application.UseCase.Contact.Queries.GetRecommendation;
using AssistantContract.TgBot.Core.Extension;
using AssistantContract.TgBot.Core.Field;
using AssistantContract.TgBot.Core.Model;
using MediatR;
using System.Globalization;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

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
        var splitMessage = update.Message!.Text!.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        
        // Check if contact number is provided
        if (splitMessage.Length < 2)
        {
            await _telegramBotClient.SendMessage(
                chatId: update.GetUserId(),
                text: "❌ Please provide a contact number.\n\n" +
                      $"<i>Example:</i> /{CommandField.GetRecommendation} 1",
                parseMode: ParseMode.Html);
            return;
        }

        // Validate that the parameter is a valid number
        if (!int.TryParse(splitMessage[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out int contactNumber))
        {
            await _telegramBotClient.SendMessage(
                chatId: update.GetUserId(),
                text: "❌ Invalid contact number. Please provide a valid number.\n\n" +
                      $"<i>Example:</i> /{CommandField.GetRecommendation} 1",
                parseMode: ParseMode.Html);
            return;
        }

        try
        {
            var recommendation = await _sender.Send(new RecommendationQuery()
            {
                UserId = update.GetUserId(), 
                ContactNumber = contactNumber
            });

            if (recommendation.SearchResponse?.Items?.Any() == true)
            {
                var recommendationLinks = recommendation.SearchResponse.Items
                    .Select(x => $"<a href=\"{x.Link}\">{x.Title}</a>");
                var recommendationText = string.Join("\n\n➡️", recommendationLinks);
                var text = $"🔍 Recommended resources for your contact:\n\n➡️{recommendationText}";
                await _telegramBotClient.SendMessage(update.GetUserId(), text, parseMode: ParseMode.Html);
            }
            else
            {
                await _telegramBotClient.SendMessage(
                    chatId: update.GetUserId(),
                    text: "ℹ️ No recommendations found for this contact.",
                    parseMode: ParseMode.Html);
            }
        }
        catch (Exception ex)
        {
            var errorMessage = ex.Message.Contains("does not exist", StringComparison.OrdinalIgnoreCase)
                ? $"❌ {ex.Message}\n\n📋 To view your contacts, use: /{CommandField.GetAllContacts}"
                : $"❌ Failed to get recommendations. {ex.Message}";
            
            await _telegramBotClient.SendMessage(
                chatId: update.GetUserId(),
                text: errorMessage,
                parseMode: ParseMode.Html);
        }
    }
}
