namespace AssistantContract.Application.Manager.Ai;

public interface IAiManager
{
    Task<string> QueryToLlmModelAsync(string prompt);
    Task<string> GetAudioTranscriptionAsync(string filePath);
    Task<List<string>> QueryToNewsApiAsync(string keyword);
}
