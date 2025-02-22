using Amazon.SQS;
using Amazon.SQS.Model;
using Customers.Consumer.Messagers;
using MediatR;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Customers.Consumer;

public sealed class QueueConsumerService(IAmazonSQS sqs, IOptions<QueueSettings> options, IMediator mediator, ILogger<QueueConsumerService> logger) : BackgroundService
{
    private readonly QueueSettings _settings = options.Value;
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        GetQueueUrlResponse queueUrlResponse = await sqs.GetQueueUrlAsync(_settings.QueueName);
        ReceiveMessageRequest request = new()
        {
            QueueUrl = queueUrlResponse.QueueUrl,
            AttributeNames = ["All"],
            MessageAttributeNames = ["All"],
            MaxNumberOfMessages = 1
        };

        while (!stoppingToken.IsCancellationRequested)
        {
            ReceiveMessageResponse response = await sqs.ReceiveMessageAsync(request, stoppingToken);
            foreach (Message message in response.Messages)
            {
                string messageType = message.MessageAttributes["MessageType"].StringValue;
                Type? type = Type.GetType($"Customers.Consumer.Messagers.{messageType}");

                if (type is null)
                {
                    logger.LogWarning("Message type handler for {messageType} not found.", messageType);
                    continue;
                }


                try
                {
                    ISqsMessage typedMessage = (ISqsMessage)JsonSerializer.Deserialize(message.Body, type, _options)!;
                    await mediator.Send(typedMessage, stoppingToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error processing message {messageId}.", message.MessageId);
                    continue;
                }

                await sqs.DeleteMessageAsync(queueUrlResponse.QueueUrl, message.ReceiptHandle, stoppingToken);
            }
            await Task.Delay(1000, stoppingToken);
        }
    }
}
