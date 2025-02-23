using Weather.Api;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
string envName = builder.Environment.EnvironmentName;
string appName = builder.Environment.ApplicationName;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddSecretsManager(configurator: options =>
{
    options.SecretFilter = entry => entry.Name.StartsWith($"{envName}_{appName}");
    options.KeyGenerator = (_, s) => s
        .Replace($"{envName}_{appName}_", string.Empty)
        .Replace("__", ":");
    options.PollingInterval = TimeSpan.FromSeconds(10);
});

builder.Services.Configure<OpenApiSetting>(builder.Configuration.GetSection("OpenApi"));

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
