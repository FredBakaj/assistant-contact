using Azure.AI.OpenAI;
using AssistantContract.Application.Manager.Ai;
using AssistantContract.Application.Manager.Ai.Model;
using AssistantContract.Infrastructure.Utils.RestApiClient;
using OpenAI.Audio;
using OpenAI.Chat;
using Microsoft.Extensions.Configuration;


namespace AssistantContract.Infrastructure.Ai;

public class AiManager : IAiManager
{
    private readonly AzureOpenAIClient _azureOpenAiClient;
    private readonly IRestApiClient _restApiClient;
    string _deploymentId = "gpt-4";
    string _deploymentAudioId = "whisper";
    private string _searchUrl = "https://customsearch.googleapis.com/customsearch/v1";
    private readonly ChatClient _chatClient;
    private readonly AudioClient _audioClient;
    private string _googleSearchApiKey;
    private string _googleSearchCx;

    public AiManager(AzureOpenAIClient azureOpenAiClient, IConfiguration configuration, IRestApiClient restApiClient)
    {
        _azureOpenAiClient = azureOpenAiClient;
        _restApiClient = restApiClient;
        _chatClient = _azureOpenAiClient.GetChatClient(_deploymentId);
        _audioClient = _azureOpenAiClient.GetAudioClient(_deploymentAudioId);
        
        _googleSearchApiKey = configuration.GetSection("CommonSettings")["GoogleSearchApiKey"]!;
        _googleSearchCx = configuration.GetSection("CommonSettings")["GoogleSearchCx"]!;
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

    

    public async Task<SearchResponse> QueryToGoogleSearchAsync(string query)
    {
        var header = new Dictionary<string, string>() { };
        var queryParams = new Dictionary<string, string>()
        {
            { "q", query }, { "key", _googleSearchApiKey }, { "dateRestrict", "m3" }, { "cx", _googleSearchCx }
        };
        var newsData = await _restApiClient.GetAsync<SearchResponse>(_searchUrl, header, queryParams);

        return newsData;
    }
    
}
