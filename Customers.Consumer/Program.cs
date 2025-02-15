using Amazon.SQS;
using Customers.Consumer;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<QueueSettings>(builder.Configuration.GetSection("QueueSettings"));
builder.Services.AddSingleton<IAmazonSQS, AmazonSQSClient>();
builder.Services.AddHostedService<QueueConsumerService>();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<Program>();
});

WebApplication app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
