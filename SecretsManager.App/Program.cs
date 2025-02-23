using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

AmazonSecretsManagerClient managerClient = new();

ListSecretVersionIdsRequest listSecretVersionIds = new()
{
    SecretId = "ApiKey",
    IncludeDeprecated = true
};
ListSecretVersionIdsResponse versionIds = await managerClient.ListSecretVersionIdsAsync(listSecretVersionIds);

GetSecretValueRequest request = new()
{
    SecretId = "ApiKey",
    VersionId = versionIds.Versions.First().VersionId
};

GetSecretValueResponse response = await managerClient.GetSecretValueAsync(request);
Console.WriteLine(response.SecretString);

//DescribeSecretRequest describe = new()
//{
//    SecretId = "ApiKey"
//};

//DescribeSecretResponse describeResponse = await managerClient.DescribeSecretAsync(describe);
//Console.WriteLine(describeResponse.Name);
