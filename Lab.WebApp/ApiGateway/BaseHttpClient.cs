using Lab.WebApp.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Lab.WebApp.ApiGateway
{
    public class BaseHttpClient : IBaseHttpClient
    {
        private readonly ILogger<BaseHttpClient> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        public BaseHttpClient(IHttpClientFactory httpClientFactory, ILogger<BaseHttpClient> logger)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<ApiResponse<T>> GetAsync<T>(string requestUrl, string bearerToken, IDictionary<string, string> headers = null)
        {
            return await CreateResponseAsync<T>(requestUrl, HttpMethod.Get, null, bearerToken, headers);
        }

        public async Task<ApiResponse<T>> PostAsync<T>(string requestUrl, object jsonObject, string bearerToken, IDictionary<string, string> headers = null)
        {
            return await CreateResponseAsync<T>(requestUrl, HttpMethod.Post, jsonObject, bearerToken, headers);
        }

        private async Task<ApiResponse<T>> CreateResponseAsync<T>(string requestUrl, HttpMethod httpMethod, object jsonObject = null, string bearerToken = null, IDictionary<string, string> headers = null)
        {
            ApiResponse<T> response = new ApiResponse<T>();
            string responseString = null;
            try
            {
                Task<HttpResponseMessage> task = null;
                HttpResponseMessage httpResponseMessage = null;

                var client = CreateClientWithHeaders(bearerToken, headers);
                task = HttpCall(requestUrl, httpMethod, jsonObject, client);

                if (task == null)
                {
                    throw new NotImplementedException(httpMethod + " Method has not been implemented");
                }

                httpResponseMessage = await task;

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    responseString = await ExtractDataToStringAsync(httpResponseMessage);
                    response.Data = JsonSerializer.Deserialize<T>(responseString);
                    response.WasSuccessful = true;
                }
                else
                {
                    // log the error response
                    _logger.LogTrace(await ExtractDataToStringAsync(httpResponseMessage));
                    response.ErrorMessages.Add(new ErrorMessage { Description = httpResponseMessage.ReasonPhrase.ToString(), Source = "HTTP Request" });
                    response.WasSuccessful = false;
                }
                response.ResponseCode = httpResponseMessage.StatusCode.ToString();
                response.ResponseReason = httpResponseMessage.ReasonPhrase.ToString();
            }
            catch (Exception exp)
            {
                _logger.LogTrace($"Raw response from data bridge: {responseString}");
                response.ErrorMessages.Add(new ErrorMessage { Description = exp.Message.ToString(), Source = "HTTP Request" });
                response.WasSuccessful = false;
                response.ResponseCode = HttpStatusCode.InternalServerError.ToString();
                response.ResponseReason = HttpStatusCode.InternalServerError.ToString();
            }

            return response;
        }

        private static Task<HttpResponseMessage> HttpCall(string requestUrl, HttpMethod httpMethod, object jsonObject, HttpClient client)
        {
            Task<HttpResponseMessage> task;
            if (httpMethod == HttpMethod.Get)
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
                task = client.SendAsync(request);
            }
            else if (httpMethod == HttpMethod.Post)
            {
                var content = new StringContent(JsonSerializer.Serialize(jsonObject), Encoding.UTF8, Application.Json);
                task = client.PostAsync(requestUrl, content);
            }
            else
            {
                task = null;
            }

            return task;
        }

        private HttpClient CreateClientWithHeaders(string bearerToken, IDictionary<string, string> headers = null)
        {
            var client = _httpClientFactory.CreateClient();
            if (bearerToken != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            }

            if (headers != null && headers.Any())
            {
                foreach (KeyValuePair<string, string> kvp in headers)
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation(kvp.Key, kvp.Value);
                }
            }
            return client;
        }

        private static async Task<string> ExtractDataToStringAsync(HttpResponseMessage httpResponseMessage)
        {
            string responseContentString = await httpResponseMessage.Content.ReadAsStringAsync();
            return responseContentString;
        }
    }
}
