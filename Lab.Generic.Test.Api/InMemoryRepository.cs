using Lab.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab.Generic.Test.Api
{
    public class InMemoryRepository
    {
        private IList<WeatherForecast> _weatherForecastsDetails;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public InMemoryRepository()
        {
            _weatherForecastsDetails = WeatherForecastDetails();
        }
        public IEnumerable<WeatherForecast> GetAll()
        {
            return _weatherForecastsDetails;
        }

        public void Add(WeatherForecast weatherForecast)
        {
            _weatherForecastsDetails.Add(weatherForecast);
        }

        private static IList<WeatherForecast> WeatherForecastDetails()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }).ToList();
        }
    }
}