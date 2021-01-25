using GetRequiredSectionSample.Configurations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GetRequiredSectionSample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly SecondOptions _options2;
        private readonly SampleOptions _sampleOptions;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IOptions<SampleOptions> options, IOptions<SecondOptions> options2)
        {
            _logger = logger;
            _options2 = options2.Value;
            _sampleOptions = options.Value;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)],
                AdditionalDataFromConfig = _sampleOptions.SampleProperty,
                AdditionalDataFromConfig2 = _options2.SampleProperty
            })
            .ToArray();
        }
    }
}
