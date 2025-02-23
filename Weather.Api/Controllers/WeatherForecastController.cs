using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Weather.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController(IOptionsMonitor<OpenApiSetting> openApiSetting) : ControllerBase
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return [.. Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)],
            ApiKey = openApiSetting.CurrentValue.ApiKey
        })];
    }
}
