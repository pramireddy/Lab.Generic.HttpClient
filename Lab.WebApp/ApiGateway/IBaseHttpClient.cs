using Lab.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab.WebApp.ApiGateway
{
    public interface IBaseHttpClient
    {
        Task<ApiResponse<T>> GetAsync<T>(string requestUrl, string bearerToken, IDictionary<string, string> headers = null);
        Task<ApiResponse<T>> PostAsync<T>(string requestUrl, object jsonObject, string bearerToken, IDictionary<string, string> headers = null);
    }
}
