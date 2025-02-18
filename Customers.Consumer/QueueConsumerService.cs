using Amazon.SQS;
using Amazon.SQS.Model;
using Customers.Consumer.Messagers;
using MediatR;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Customers.Consumer;

public sealed class QueueConsumerService : BackgroundService
{
    private readonly IAmazonSQS _sqs;
    private readonly QueueSettings _settings;
    private readonly IMediator _mediator;
    private readonly ILogger<QueueConsumerService> _logger;
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public QueueConsumerService(IAmazonSQS sqs, IOptions<QueueSettings> options, IMediator mediator, ILogger<QueueConsumerService> logger)
    {
        _sqs = sqs;
        _settings = options.Value;
        _mediator = mediator;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        GetQueueUrlResponse queueUrlResponse = await _sqs.GetQueueUrlAsync(_settings.QueueName);
        ReceiveMessageRequest request = new()
        {
            QueueUrl = queueUrlResponse.QueueUrl,
            AttributeNames = ["All"],
            MessageAttributeNames = ["All"],
            MaxNumberOfMessages = 1
        };

        while (!stoppingToken.IsCancellationRequested)
        {
            ReceiveMessageResponse response = await _sqs.ReceiveMessageAsync(request, stoppingToken);
            foreach (Message message in response.Messages)
            {
                string messageType = message.MessageAttributes["MessageType"].StringValue;
                Type? type = Type.GetType($"Customers.Consumer.Messagers.{messageType}");

                if (type is null)
                {
                    _logger.LogWarning("Message type handler for {messageType} not found.", messageType);
                    continue;
                }


                try
                {
                    ISqsMessage typedMessage = (ISqsMessage)JsonSerializer.Deserialize(message.Body, type, _options)!;
                    await _mediator.Send(typedMessage, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message {messageId}.", message.MessageId);
                    continue;
                }

                await _sqs.DeleteMessageAsync(queueUrlResponse.QueueUrl, message.ReceiptHandle, stoppingToken);
            }
            await Task.Delay(1000, stoppingToken);
        }
    }
}
