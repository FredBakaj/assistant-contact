using AssistantContract.Application.Manager.Ai.Model;

namespace AssistantContract.Application.Manager.Ai;

public interface IAiManager
{
    Task<string> QueryToLlmModelAsync(string prompt);
    Task<string> GetAudioTranscriptionAsync(string filePath);
    Task<SearchResponse> QueryToGoogleSearchAsync(string query);
}


