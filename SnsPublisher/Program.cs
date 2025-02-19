using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using SnsPublisher;
using System.Text.Json;

CustomerCreated customerCreated = new()
{
    Id = Guid.NewGuid(),
    Email = "minhtrn98@gmail.com",
    FullName = "Minh Tran",
    GitHubUsername = "minhtrn98",
    DateOfBirth = new DateTime(1998, 03, 25)
};

AmazonSimpleNotificationServiceClient snsClient = new();

Topic topicArn = await snsClient.FindTopicAsync("customers");

PublishRequest publishRequest = new()
{
    TopicArn = topicArn.TopicArn,
    Message = JsonSerializer.Serialize(customerCreated),
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

PublishResponse response = await snsClient.PublishAsync(publishRequest);
