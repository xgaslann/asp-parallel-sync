using Microsoft.AspNetCore.Mvc;
using ParallelSync.Services;

namespace ParallelSync.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{

    private readonly WeatherService _weatherService;
    
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, WeatherService weatherService)
    {
        _logger = logger;
        _weatherService = weatherService;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<List<IEnumerable<Services.WeatherForecast>>> Get()
    {
        // Console.WriteLine($"Sync Start:{DateTime.UtcNow}");
        // var london = await _weatherService.GetWeather("London");
        // var paris = await _weatherService.GetWeather("Paris");
        // var newYork = await _weatherService.GetWeather("New York");
        // var ankara = await _weatherService.GetWeather("Ankara");
        //
        // var cities = new List<IEnumerable<WeatherForecast>?>();
        // cities.Add(london as IEnumerable<WeatherForecast>);
        // cities.Add(paris as IEnumerable<WeatherForecast>);
        // cities.Add(newYork as IEnumerable<WeatherForecast>);
        // cities.Add(ankara as IEnumerable<WeatherForecast>);
        // Console.WriteLine($"Sync End:{DateTime.UtcNow}");
        
        Console.WriteLine($"Sync Parallel Start:{DateTime.UtcNow}");
        
        var fourCitiesWeatherTasks = new[]
        {
            _weatherService.GetWeather("London"),
            _weatherService.GetWeather("Paris"),
            _weatherService.GetWeather("New York"),
            _weatherService.GetWeather("Ankara")
        };

        var fourCitiesWeatherResults = await CustomTaskExtensions.GetAwaiter(fourCitiesWeatherTasks);
        
        var twoCitiesWeatherTasks = new[]
        {
            _weatherService.GetWeather("London"),
            _weatherService.GetWeather("Paris")
        };

        var twoCitiesWeatherResults = await CustomTaskExtensions.GetAwaiter(twoCitiesWeatherTasks);
        
        var cities2 = await CustomTaskExtensions.GetAwaiter((_weatherService.GetWeather("London"),
            _weatherService.GetWeather("Paris"), _weatherService.GetWeather("New York"),
            _weatherService.GetWeather("Ankara")));

        Console.WriteLine($"Sync Parallel End:{DateTime.UtcNow}");
        return fourCitiesWeatherResults.ToList();
    }
}