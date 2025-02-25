using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;


[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace SimpleSqsLambda;

public class Function
{
    public Function()
    {

    }

    public async Task FunctionHandler(SQSEvent evnt, ILambdaContext context)
    {
        foreach(SQSEvent.SQSMessage? message in evnt.Records)
        {
            await ProcessMessageAsync(message, context);
        }
    }

    private async Task ProcessMessageAsync(SQSEvent.SQSMessage message, ILambdaContext context)
    {
        context.Logger.LogInformation($"Processed message {message.Body}");

        // TODO: Do interesting work based on the new message
        await Task.CompletedTask;
    }
}