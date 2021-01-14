using Lab.Dtos;
using Lab.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lab.WebApp.ApiGateway
{
    public class TestServiceGateway : ITestServiceGateway
    {
        private IBaseHttpClient _baseHttpClient;

        public TestServiceGateway(IBaseHttpClient baseHttpClient)
        {
            _baseHttpClient = baseHttpClient;
        }

        public async Task<bool> AddtWeatherForecasts(WeatherForecast weatherForecast)
        {
            string uri = new Uri($"https://localhost:44391/WeatherForecast").ToString();

            ApiResponse<bool> response = await _baseHttpClient.PostAsync<bool>(uri, weatherForecast, null);

            return response.Data;
        }

        public async Task<IEnumerable<WeatherForecast>> GetWeatherForecasts()
        {
            string uri = new Uri($"https://localhost:44391/WeatherForecast").ToString();

            ApiResponse<IEnumerable<WeatherForecast>> response = await _baseHttpClient.GetAsync<IEnumerable<WeatherForecast>>(uri, null);

            return response.Data;
        }
    }
}