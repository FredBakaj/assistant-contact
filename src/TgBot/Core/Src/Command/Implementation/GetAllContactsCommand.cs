using AssistantContract.Application.UseCase.Contact.Queries.AllContacts;
using AssistantContract.TgBot.Core.Extension;
using AssistantContract.TgBot.Core.Field;
using AssistantContract.TgBot.Core.Model;
using MediatR;
using Telegram.Bot;
using System.Text;
using System.Reflection;
using Telegram.Bot.Types;
using File = System.IO.File;

namespace AssistantContract.TgBot.Core.Src.Command.Implementation;

public class GetAllContactsCommand : IBotCommand
{
    private readonly ISender _sender;
    private readonly ITelegramBotClient _telegramBotClient;
    public string GetCommand() => CommandField.GetAllContacts;

    public bool IsMoveNext() => false;

    public GetAllContactsCommand(ISender sender, ITelegramBotClient telegramBotClient)
    {
        _sender = sender;
        _telegramBotClient = telegramBotClient;
    }

    public async Task Exec(UpdateBDto update)
    {
        var contacts = await _sender.Send(new AllContactsQuery { UserId = update.GetUserId() });
        var contactsHtml = HtmlGenerator.GenerateHtmlTable(contacts.Contacts);
        
        var tempFilePath = Path.Combine(Path.GetTempPath(), $"contacts_{Guid.NewGuid()}.html");
        await File.WriteAllTextAsync(tempFilePath, contactsHtml);
        
        await using var stream = File.OpenRead(tempFilePath);
        await _telegramBotClient.SendDocument(
            chatId: update.GetUserId(),
            document: InputFile.FromStream(stream, "contacts.html"),
            caption: "Контакты в виде таблицы"
        );

    }
}

public static class HtmlGenerator
{
    public static string GenerateHtmlTable<T>(IEnumerable<T> items)
    {
        var sb = new StringBuilder();
        var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        sb.AppendLine("<!DOCTYPE html>");
        sb.AppendLine("<html><head><meta charset='UTF-8'><title>Table</title></head><body>");
        sb.AppendLine("<table border='1' cellpadding='5' cellspacing='0'>");

        // Заголовок таблицы
        sb.AppendLine("<tr>");
        foreach (var prop in props)
        {
            sb.AppendLine($"<th>{prop.Name}</th>");
        }

        sb.AppendLine("</tr>");

        // Строки данных
        foreach (var item in items)
        {
            sb.AppendLine("<tr>");
            foreach (var prop in props)
            {
                var value = prop.GetValue(item)?.ToString() ?? "";
                value = string.Concat(value.Take(125));
                sb.AppendLine($"<td>{System.Net.WebUtility.HtmlEncode(value)}</td>");
            }

            sb.AppendLine("</tr>");
        }

        sb.AppendLine("</table></body></html>");
        return sb.ToString();
    }
}
