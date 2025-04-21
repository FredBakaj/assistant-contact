using System.ClientModel;
using Azure.AI.OpenAI;
using AssistantContract.Application.Manager.Ai;
using OpenAI.Audio;
using OpenAI.Chat;

namespace AssistantContract.Infrastructure.Ai;

public class AiManager : IAiManager
{
    private readonly AzureOpenAIClient _azureOpenAiClient;
    string _deploymentId = "gpt-4";
    string _deploymentAudioId = "whisper";
    private readonly ChatClient _chatClient;
    private readonly AudioClient _audioClient;

    public AiManager(AzureOpenAIClient azureOpenAiClient)
    {
        _azureOpenAiClient = azureOpenAiClient;
        _chatClient = _azureOpenAiClient.GetChatClient(_deploymentId);
        _audioClient = _azureOpenAiClient.GetAudioClient(_deploymentAudioId);
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
}
