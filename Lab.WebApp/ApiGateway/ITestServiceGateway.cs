using Lab.Dtos;
using Lab.WebApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lab.WebApp.ApiGateway
{
    public interface ITestServiceGateway
    {
        public Task<IEnumerable<WeatherForecast>> GetWeatherForecasts();

        public Task<bool> AddtWeatherForecasts(WeatherForecast weatherForecast);
    }
}