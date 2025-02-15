using Amazon.SQS;
using Amazon.SQS.Model;

CancellationTokenSource cts = new();
AmazonSQSClient sqsClient = new();

GetQueueUrlResponse queueUrlResponse = await sqsClient.GetQueueUrlAsync("customers");

ReceiveMessageRequest receiveMessageRequest = new()
{
    QueueUrl = queueUrlResponse.QueueUrl,
    AttributeNames = ["All"],
    MessageAttributeNames = ["All"]
};

while (!cts.IsCancellationRequested)
{
    ReceiveMessageResponse response = await sqsClient.ReceiveMessageAsync(receiveMessageRequest, cts.Token);
    foreach (Message message in response.Messages)
    {
        Console.WriteLine($"Message Id: {message.MessageId}");
        Console.WriteLine($"Message Body: {message.Body}");

        await sqsClient.DeleteMessageAsync(queueUrlResponse.QueueUrl, message.ReceiptHandle);
    }
    await Task.Delay(3000);
}

