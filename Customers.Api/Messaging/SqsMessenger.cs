using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Customers.Api.Messaging;

public sealed class SqsMessenger(IAmazonSQS sqs, IOptions<QueueSettings> options) : ISqsMessenger
{
    private readonly IAmazonSQS _sqs = sqs;
    private readonly QueueSettings _settings = options.Value;
    private string? _queueUrl;

    public async Task<SendMessageResponse> SendAsync<T>(T message)
    {
        string queueUrl = await GetQueueUrlAsync();
        SendMessageRequest request = new()
        {
            QueueUrl = queueUrl,
            MessageBody = JsonSerializer.Serialize(message),
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

        return await _sqs.SendMessageAsync(request);
    }

    private async ValueTask<string> GetQueueUrlAsync()
    {
        return _queueUrl ??= (await _sqs.GetQueueUrlAsync(_settings.QueueName)).QueueUrl;
    }
}
