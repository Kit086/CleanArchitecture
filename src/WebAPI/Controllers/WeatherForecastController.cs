using CleanArchitecture.Application.WeatherForecasts.Queries.GetWeatherForecasts;
using CleanArchitecture.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebUI.Controllers;

public class WeatherForecastController : ApiControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        return await Mediator.Send(new GetWeatherForecastsQuery());
    }
}
