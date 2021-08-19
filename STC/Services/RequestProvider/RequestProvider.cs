using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using STC.Exceptions;
using STC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
namespace STC.Services.RequestProvider
{
    public class RequestProvider : IRequestProvider
    {
        private readonly JsonSerializerSettings _serializerSettings;

        public RequestProvider()
        {
            _serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Local,
                NullValueHandling = NullValueHandling.Ignore
            };
            _serializerSettings.Converters.Add(new StringEnumConverter());
        }

        public async Task<TResult> GetAsync<TResult>(string uri, string token = "", List<KeyValuePair<string, string>> header = null)
        {
            try
            {
                HttpClient httpClient = CreateHttpClient(token);

                AddHeaderParameter(httpClient, header);

                HttpResponseMessage response = await httpClient.GetAsync(uri);

                //await HandleResponse(response);
                string serialized = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"----------------------------------------------URL :    {uri}");
                Console.WriteLine($"----------------------------------------------Token :    {token}");
                Console.WriteLine($"---------------------------------------------response :    {serialized}");

                TResult result = await Task.Run(() =>
                    JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings));

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Activator.CreateInstance<TResult>();
            }
           
        }

        public async Task<TResult> PostAsync<TResult>(string uri, TResult data, string token = "", List<KeyValuePair<string, string>> header = null)
        {
            try
            {
                HttpClient httpClient = CreateHttpClient(token);

                AddHeaderParameter(httpClient, header);

                var content = new StringContent(JsonConvert.SerializeObject(data));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await httpClient.PostAsync(uri, content);

                //await HandleResponse(response);
                string serialized = await response.Content.ReadAsStringAsync();

                TResult result = await Task.Run(() =>
                    JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings));

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Activator.CreateInstance<TResult>();
            }
           
        }

        public async Task<TResult> PostAsync<TResult>(string uri, string data, string clientId, string clientSecret)
        {
            try
            {
                HttpClient httpClient = CreateHttpClient(string.Empty);

                if (!string.IsNullOrWhiteSpace(clientId) && !string.IsNullOrWhiteSpace(clientSecret))
                {
                    AddBasicAuthenticationHeader(httpClient, clientId, clientSecret);
                }

                var content = new StringContent(data);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                HttpResponseMessage response = await httpClient.PostAsync(uri, content);

                // await HandleResponse(response);
                string serialized = await response.Content.ReadAsStringAsync();

                TResult result = await Task.Run(() =>
                    JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings));

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Activator.CreateInstance<TResult>();
            }
           
        }

        public async Task<TResult> PutAsync<TResult>(string uri, TResult data, string token = "", List<KeyValuePair<string, string>> header = null)
        {
            try
            {
                HttpClient httpClient = CreateHttpClient(token);

                AddHeaderParameter(httpClient, header);

                var content = new StringContent(JsonConvert.SerializeObject(data));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await httpClient.PutAsync(uri, content);

                // await HandleResponse(response);
                string serialized = await response.Content.ReadAsStringAsync();

                TResult result = await Task.Run(() =>
                    JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings));

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Activator.CreateInstance<TResult>();
            }
           
        }

        public async Task DeleteAsync(string uri, string token = "")
        {
            HttpClient httpClient = CreateHttpClient(token);
            await httpClient.DeleteAsync(uri);
        }

        private HttpClient CreateHttpClient(string token = "")
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return httpClient;
        }

        private void AddHeaderParameter(HttpClient httpClient, List<KeyValuePair<string,string>> pairs)
        {
            if (httpClient == null)
                return;

            if (pairs == null)
                return;

            foreach (var item in pairs)
            {
                httpClient.DefaultRequestHeaders.Add(item.Key, item.Value);
            }
           
        }

        private void AddBasicAuthenticationHeader(HttpClient httpClient, string clientId, string clientSecret)
        {
            if (httpClient == null)
                return;

            if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret))
                return;

            httpClient.DefaultRequestHeaders.Authorization = new BasicAuthenticationHeaderValue(clientId, clientSecret);
        }

        private async Task HandleResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.Forbidden ||
                    response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new ServiceAuthenticationException(content);
                }

                throw new HttpRequestExceptionEx(response.StatusCode, content);
            }
        }

        public async Task<Response<T>> PostDataAsync<T>(string uri, object data, string token)
        {
            try
            {
                HttpClient httpClient = CreateHttpClient(token);

                string contentJson = SerializeToJson(data);

                var content = new StringContent(contentJson);

                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await httpClient.PostAsync(uri, content);

                //await HandleResponse(response);
                string serialized = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"----------------------------------------------URL :    {uri}");
                Console.WriteLine($"---------------------------------------------Body :    {contentJson}");
                Console.WriteLine($"---------------------------------------------response :    {serialized}");

                Response<T> result = await Task.Run(() =>
                    JsonConvert.DeserializeObject<Response<T>>(serialized, _serializerSettings));


                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Activator.CreateInstance<Response<T>>();
            }
           
        }

        public string SerializeToJson(object obj)
        {
            string data = "";

            if (obj != null)
            {
                data = JsonConvert.SerializeObject(obj, _serializerSettings);
            }
            return data;
        }

        public async Task<Response<T>> PutDataAsync<T>(string uri, object data, string token)
        {
            try
            {
                HttpClient httpClient = CreateHttpClient(token);

                string contentJson = SerializeToJson(data);

                var content = new StringContent(contentJson);

                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await httpClient.PutAsync(uri, content);

                //await HandleResponse(response);
                string serialized = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"----------------------------------------------URL :    {uri}");
                Console.WriteLine($"---------------------------------------------Body :    {contentJson}");
                Console.WriteLine($"---------------------------------------------response :    {serialized}");

                Response<T> result = await Task.Run(() =>
                    JsonConvert.DeserializeObject<Response<T>>(serialized, _serializerSettings));


                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Activator.CreateInstance<Response<T>>();
            }
           
        }

        public async Task<Response<T>> PostFormDataAsync<T>(string uri, List<KeyValuePair<string, string>> formData, Stream streamData, string fileName, string token)
        {
            try
            {
                MultipartFormDataContent form = new MultipartFormDataContent();

                // FormUrlEncodedContent formUrlEncoded = new FormUrlEncodedContent(formData);

                HttpContent content = new StreamContent(streamData);
                //content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                //{
                //    Name = fileName.Split('.')[0],
                //    FileName = fileName
                //};
                // content.Headers.ContentType.MediaType = "multipart/form-data";
                // form.Add(formUrlEncoded);
                form.Add(content, "File", fileName);


                foreach (var data in formData)
                {
                    HttpContent contentData = new StringContent(data.Value);
                    // contentData.Headers.ContentType.MediaType = "multipart/form-data";
                    form.Add(contentData, data.Key);
                }

                HttpClient httpClient = CreateHttpClient(token);


                form.Headers.ContentType.MediaType = "multipart/form-data";
                HttpResponseMessage response = await httpClient.PostAsync(uri, form);

                //await HandleResponse(response);
                string serialized = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"----------------------------------------------URL :    {uri}");
                // Console.WriteLine($"---------------------------------------------Body :    {contentJson}");
                Console.WriteLine($"---------------------------------------------response :    {serialized}");

                Response<T> result = await Task.Run(() =>
                    JsonConvert.DeserializeObject<Response<T>>(serialized, _serializerSettings));


                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Activator.CreateInstance<Response<T>>();
            }
           
        }
        public async Task<T> PostAsync<T>(string uri, object data, string token)
        {
            try
            {
                HttpClient httpClient = CreateHttpClient(token);

                string contentJson = SerializeToJson(data);

                var content = new StringContent(contentJson);

                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await httpClient.PostAsync(uri, content);

                //await HandleResponse(response);
                string serialized = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"----------------------------------------------URL :    {uri}");
                Console.WriteLine($"---------------------------------------------Body :    {contentJson}");
                Console.WriteLine($"---------------------------------------------response :    {serialized}");

                T result = await Task.Run(() =>
                    JsonConvert.DeserializeObject<T>(serialized, _serializerSettings));


                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Activator.CreateInstance<T>();
            }
           
        }
    }

    /// <summary>
    /// HTTP Basic Authentication authorization header
    /// </summary>
    /// <seealso cref="System.Net.Http.Headers.AuthenticationHeaderValue" />
    public class BasicAuthenticationHeaderValue : AuthenticationHeaderValue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasicAuthenticationHeaderValue"/> class.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        public BasicAuthenticationHeaderValue(string userName, string password)
            : base("Basic", EncodeCredential(userName, password))
        { }

        /// <summary>
        /// Encodes the credential.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">userName</exception>
        public static string EncodeCredential(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException(nameof(userName));
            if (password == null) password = "";

            Encoding encoding = Encoding.UTF8;
            string credential = String.Format("{0}:{1}", userName, password);

            return Convert.ToBase64String(encoding.GetBytes(credential));
        }
    }
}
