using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using STC.Models;

namespace STC.Services.RequestProvider
{
    public interface IRequestProvider
    {
        Task<TResult> GetAsync<TResult>(string uri, string token = "", List<KeyValuePair<string, string>> header = null);

        Task<TResult> PostAsync<TResult>(string uri, TResult data, string token = "", List<KeyValuePair<string, string>> header = null);

        Task<Response<T>> PostDataAsync<T>(string uri, object data, string token);
        Task<T> PostAsync<T>(string uri, object data, string token);
        Task<Response<T>> PostFormDataAsync<T>(string uri, List<KeyValuePair<string, string>> formData, Stream streamData, string fileName, string token);
        Task<Response<T>> PutDataAsync<T>(string uri, object data, string token);

        Task<TResult> PostAsync<TResult>(string uri, string data, string clientId, string clientSecret);

        Task<TResult> PutAsync<TResult>(string uri, TResult data, string token = "", List<KeyValuePair<string, string>> header = null);

        Task DeleteAsync(string uri, string token = "");
    }
}
