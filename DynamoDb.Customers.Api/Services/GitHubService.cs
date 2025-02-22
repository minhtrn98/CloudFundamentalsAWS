using System.Net;
using System.Text.Json.Nodes;

namespace DynamoDb.Customers.Api.Services;

public class GitHubService(IHttpClientFactory httpClientFactory) : IGitHubService
{
    public async Task<bool> IsValidGitHubUser(string username)
    {
        HttpClient client = httpClientFactory.CreateClient("GitHub");
        HttpResponseMessage response = await client.GetAsync($"/users/{username}");
        if (response.StatusCode == HttpStatusCode.Forbidden)
        {
            JsonObject? responseBody = await response.Content.ReadFromJsonAsync<JsonObject>();
            string message = responseBody!["message"]!.ToString();
            throw new HttpRequestException(message);
        }

        return response.StatusCode == HttpStatusCode.OK;
    }
}
