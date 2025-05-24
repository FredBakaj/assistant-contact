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
            caption: "Your contacts in table format"
        );

    }
}

public static class HtmlGenerator
{
    public static string GenerateHtmlTable<T>(IEnumerable<T> items)
    {
        var sb = new StringBuilder();
        var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.Name != "KeywordDescription" && p.Name != "UserId" && p.Name != "Id");

        sb.AppendLine("<!DOCTYPE html>");
        sb.AppendLine("<html><head>");
        sb.AppendLine("<meta charset='UTF-8'>");
        sb.AppendLine("<title>Your Contacts</title>");
        sb.AppendLine("<style>");
        sb.AppendLine("body { font-family: Arial, sans-serif; margin: 20px; }");
        sb.AppendLine("h1 { color: #2c3e50; }");
        sb.AppendLine("table { border-collapse: collapse; width: 100%; margin-top: 20px; }");
        sb.AppendLine("th, td { border: 1px solid #ddd; padding: 12px; text-align: left; }");
        sb.AppendLine("th { background-color: #f2f2f2; font-weight: bold; }");
        sb.AppendLine("tr:nth-child(even) { background-color: #f9f9f9; }");
        sb.AppendLine("tr:hover { background-color: #f1f1f1; }");
        sb.AppendLine("</style>");
        sb.AppendLine("</head><body>");
        sb.AppendLine("<h1>Your Contacts</h1>");
        sb.AppendLine("<table>");

        // Table header with friendly names
        sb.AppendLine("<tr>");
        foreach (var prop in props)
        {
            string headerName = prop.Name switch
            {
                "ContactNumber" => "#",
                "Name" => "Name",
                "PersonalInfo" => "Contact Info",
                "Description" => "Description",
                "NotificationDayTimeSpan" => "Reminder (days)",
                _ => prop.Name
            };
            sb.AppendLine($"<th>{headerName}</th>");
        }
        sb.AppendLine("</tr>");

        // Table rows
        foreach (var item in items)
        {
            sb.AppendLine("<tr>");
            foreach (var prop in props)
            {
                var value = prop.GetValue(item);
                string displayValue;
                
                if (prop.Name == "NotificationDayTimeSpan" && value is int days)
                {
                    displayValue = days > 0 ? $"{days} day{(days != 1 ? "s" : "")}" : "Off";
                }
                else
                {
                    displayValue = value?.ToString() ?? "";
                    // Truncate long text but keep it readable
                    displayValue = displayValue.Length > 100 ? displayValue.Substring(0, 100) + "..." : displayValue;
                }
                
                sb.AppendLine($"<td>{System.Net.WebUtility.HtmlEncode(displayValue)}</td>");
            }
            sb.AppendLine("</tr>");
        }

        sb.AppendLine("</table></body></html>");
        return sb.ToString();
    }
}
