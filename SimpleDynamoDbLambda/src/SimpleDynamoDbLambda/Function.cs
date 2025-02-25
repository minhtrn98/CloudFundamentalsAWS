using Amazon.Lambda.Core;
using Amazon.Lambda.DynamoDBEvents;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace SimpleDynamoDbLambda;

public class Function
{
    public void FunctionHandler(DynamoDBEvent dynamoEvent, ILambdaContext context)
    {
        context.Logger.LogInformation($"Beginning to process {dynamoEvent.Records.Count} records...");

        foreach (DynamoDBEvent.DynamodbStreamRecord? record in dynamoEvent.Records)
        {
            context.Logger.LogInformation($"Event ID: {record.EventID}");
            context.Logger.LogInformation($"Event Name: {record.EventName}");

            if (record.Dynamodb.OldImage != null)
            {
                context.Logger.LogInformation($"Old Document: {record.Dynamodb.OldImage.ToJsonPretty()}");
            }

            context.Logger.LogInformation($"New Document: {record.Dynamodb.NewImage.ToJsonPretty()}");
        }

        context.Logger.LogInformation("Stream processing complete.");
    }
}