namespace AssistantContract.Infrastructure.Utils.RestApiClient;

public interface IRestApiClient
{
    Task<TOut> PostAsync<TOut>(string apiUrl, Dictionary<string, string> headers, object body);

    Task<TOut> GetAsync<TOut>(string apiUrl, Dictionary<string, string> headers,
        Dictionary<string, string> queryParams);

    Task<TOut> GetAsync<TOut>(string apiUrl, Dictionary<string, string> headers);
}
