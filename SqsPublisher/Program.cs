using Amazon.SQS;
using Amazon.SQS.Model;
using SqsPublisher;
using System.Text.Json;

AmazonSQSClient sqsClient = new ();

CustomerCreated customerCreated = new()
{
    Id = Guid.NewGuid(),
    Email = "minhtrn98@gmail.com",
    FullName = "Minh Tran",
    GitHubUsername = "minhtrn98",
    DateOfBirth = new DateTime(1998, 03, 25)
};

GetQueueUrlResponse queueUrlResponse = await sqsClient.GetQueueUrlAsync("customers");

SendMessageRequest sendMessageRequest = new()
{
    QueueUrl = queueUrlResponse.QueueUrl,
    MessageBody = JsonSerializer.Serialize(customerCreated),
    MessageAttributes = new Dictionary<string, MessageAttributeValue>
    {
        {
            "Type",
            new MessageAttributeValue
            {
                DataType = "String",
                StringValue = nameof(CustomerCreated)
            }
        }
    }
};

SendMessageResponse response = await sqsClient.SendMessageAsync(sendMessageRequest);

Console.WriteLine();
