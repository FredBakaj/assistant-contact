using System.ClientModel;
using System.Text;
using System.Text.Json;
using Azure.AI.OpenAI;
using AssistantContract.Application.Manager.Ai;
using AssistantContract.Infrastructure.Utils.RestApiClient;
using OpenAI.Audio;
using OpenAI.Chat;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;


namespace AssistantContract.Infrastructure.Ai;

public class AiManager : IAiManager
{
    private readonly AzureOpenAIClient _azureOpenAiClient;
    private readonly IRestApiClient _restApiClient;
    string _deploymentId = "gpt-4";
    string _deploymentAudioId = "whisper";
    string url = $"https://newsapi.org/v2/everything";
    private readonly ChatClient _chatClient;
    private readonly AudioClient _audioClient;
    private string _newsApiKey;

    public AiManager(AzureOpenAIClient azureOpenAiClient, IConfiguration configuration, IRestApiClient restApiClient)
    {
        _azureOpenAiClient = azureOpenAiClient;
        _restApiClient = restApiClient;
        _chatClient = _azureOpenAiClient.GetChatClient(_deploymentId);
        _audioClient = _azureOpenAiClient.GetAudioClient(_deploymentAudioId);

        _newsApiKey = configuration.GetSection("CommonSettings")["NewsApiKey"]!;
    }

    public async Task<string> QueryToLlmModelAsync(string prompt)
    {
        var messages = new List<ChatMessage> { new SystemChatMessage("Ai agent"), new UserChatMessage(prompt) };

        var options = new ChatCompletionOptions
        {
            Temperature = (float)0.7, MaxOutputTokenCount = 1200, FrequencyPenalty = 0, PresencePenalty = 0,
        };
        var completion = (await _chatClient.CompleteChatAsync(messages, options)).Value;

        if (completion.Content != null && completion.Content.Count > 0)
        {
            string responseText = completion.Content[0].Text;
            return responseText;
        }

        throw new InvalidOperationException("Ai model return empty text");
    }

    public async Task<string> GetAudioTranscriptionAsync(string filePath)
    {
        var response = await _audioClient.TranscribeAudioAsync(filePath);
        return response.Value.Text;
    }

    public async Task<List<string>> QueryToNewsApiAsync(string keyword)
    {
        var yesterday = DateTime.Now.AddDays(-1);
        var header = new Dictionary<string, string>() { { "User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)" } };
        var queryParams = new Dictionary<string, string>()
        {
            { "q", keyword },
            { "from", yesterday.ToString("yyyy-MM-dd") },
            { "sortBy", "popularity" },
            { "apiKey", _newsApiKey }
        };
        var newsData = await _restApiClient.GetAsync<NewsResponse>(url, header, queryParams);
        List<string> textFromSites = new List<string>();

        var rangeNewsData = newsData!.Articles;
        foreach (var article in rangeNewsData)
        {
            var text = await GetLongTextsAsync(article.Url);
            if (!string.IsNullOrEmpty(text))
            {
                textFromSites.Add(string.Concat(text.Take(2000)));
            }

            if (textFromSites.Count >= 5)
            {
                break;
            }
        }

        return textFromSites;
    }


    public class NewsResponse
    {
        public Article[] Articles { get; set; } = null!;
    }

    public class Article
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Url { get; set; } = null!;
    }


    public async Task<string> GetLongTextsAsync(string url)
    {
        try
        {
            var client = new HttpClient();
            var html = await client.GetStringAsync(url);

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var longTexts = new List<string>();
            var hashText = new List<int>();

            // Ищем теги, которые могут содержать текст
            var tags = new[] { "h1", "h2", "h3", "h4", "h5", "h6", "p", "div", "span", "article", "section" };

            foreach (var tag in tags)
            {
                var nodes = doc.DocumentNode.SelectNodes($"//{tag}");
                if (nodes == null)
                    continue;

                foreach (var node in nodes)
                {
                    var text = node.InnerText.Trim();
                    var cleanText = CleanText(text);

                    var hash = cleanText.GetHashCode();
                    if (cleanText.Length > 150 && !hashText.Any(x => x == hash))
                    {
                        longTexts.Add(cleanText);
                        hashText.Add(hash);
                    }
                }
            }

            return string.Join("\n", longTexts);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return "";
        }
    }

    private string CleanText(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;

        // Убираем переносы строк и табуляцию
        text = text.Replace("\r", " ").Replace("\n", " ").Replace("\t", " ");

        // Заменяем несколько пробелов на один
        text = System.Text.RegularExpressions.Regex.Replace(text, @"\s+", " ");

        return text.Trim();
    }
}
