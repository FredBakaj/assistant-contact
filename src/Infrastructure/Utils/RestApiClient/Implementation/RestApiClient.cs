using System.Text;
using Newtonsoft.Json;

namespace AssistantContract.Infrastructure.Utils.RestApiClient.Implementation;

public class RestApiClient : IRestApiClient
{
    public async Task<TOut> PostAsync<TOut>(string apiUrl, Dictionary<string, string> headers, object body)
    {
        using (HttpClient httpClient = new HttpClient())
        {
            foreach (var headKey in headers.Keys)
            {
                httpClient.DefaultRequestHeaders.Add(headKey, headers[headKey]);
            }

            string jsonBody = JsonConvert.SerializeObject(body);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                TOut result =
                    JsonConvert.DeserializeObject<TOut>(responseBody)!;
                return result;
            }

            response.EnsureSuccessStatusCode();
        }

        throw new InvalidOperationException("post request has problem");
    }

    public async Task<TOut> GetAsync<TOut>(string apiUrl, Dictionary<string, string> headers)
    {
        using (HttpClient httpClient = new HttpClient())
        {
            foreach (var headKey in headers.Keys)
            {
                httpClient.DefaultRequestHeaders.Add(headKey, headers[headKey]);
            }

            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(responseBody))
                    return default!;

                TOut result = JsonConvert.DeserializeObject<TOut>(responseBody)!;
                return result;
            }

            response.EnsureSuccessStatusCode();
        }

        throw new InvalidOperationException("GET request has problem");
    }

    public async Task<TOut> GetAsync<TOut>(string apiUrl, Dictionary<string, string> headers,
        Dictionary<string, string> queryParams)
    {
        using (HttpClient httpClient = new HttpClient())
        {
            foreach (var headKey in headers.Keys)
            {
                httpClient.DefaultRequestHeaders.Add(headKey, headers[headKey]);
            }

            // Добавляем параметры в URL
            if (queryParams != null && queryParams.Any())
            {
                var queryString = string.Join("&",
                    queryParams
                        .Where(kvp => kvp.Key != null && kvp.Value != null)
                        .Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));

                if (!string.IsNullOrEmpty(queryString))
                {
                    apiUrl += (apiUrl.Contains("?") ? "&" : "?") + queryString;
                }
            }


            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(responseBody))
                    return default!;

                TOut result = JsonConvert.DeserializeObject<TOut>(responseBody)!;
                return result;
            }

            response.EnsureSuccessStatusCode();
        }

        throw new InvalidOperationException("GET request with params has problem");
    }
}
