using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Sns.Customers.Api.Messaging;

public sealed class SnsMessenger(IAmazonSimpleNotificationService sns, IOptions<TopicSettings> options) : ISnsMessenger
{
    private readonly IAmazonSimpleNotificationService _sns = sns;
    private readonly TopicSettings _settings = options.Value;
    private string? _queueUrl;

    public async Task<PublishResponse> PublishAsync<T>(T message)
    {
        string topicArn = await GetTopicArnAsync();
        PublishRequest request = new()
        {
            TopicArn = topicArn,
            Message = JsonSerializer.Serialize(message),
            MessageAttributes = new Dictionary<string, MessageAttributeValue>
            {
                {
                    "MessageType", new MessageAttributeValue
                    {
                        DataType = "String",
                        StringValue = typeof(T).Name
                    }
                }
            }
        };

        return await _sns.PublishAsync(request);
    }

    private async ValueTask<string> GetTopicArnAsync()
    {
        return _queueUrl ??= (await _sns.FindTopicAsync(_settings.TopicName)).TopicArn;
    }
}
