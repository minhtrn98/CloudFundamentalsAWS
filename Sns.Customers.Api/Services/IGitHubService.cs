namespace Sns.Customers.Api.Services;

public interface IGitHubService
{
    Task<bool> IsValidGitHubUser(string username);
}
