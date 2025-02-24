using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace SimpleLambda;

public class Function
{
    public void FunctionHandler(Hello hello, ILambdaContext context)
    {
        context.Logger.LogInformation($"Hello from {hello.World}");
    }
}

public class Hello
{
    public string World { get; set; } = default!;
}
