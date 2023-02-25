using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPark_Core.Repositories.Interfaces;
using VPark_Helper.Request;

namespace VPark_Core.Repositories.Implementation
{
    public class HttpServices : IHttpServices
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ILogger<HttpServices> _logger;

        public HttpServices(IHttpClientFactory httpClient, ILogger<HttpServices> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<T> SendPostRequest<T, U>(JsonContentPostRequest<U> request)
        {
            var client = _httpClient.CreateClient();

            HttpRequestMessage message = new HttpRequestMessage();

            message.RequestUri = new Uri(request.Url);
            message.Method = HttpMethod.Post;

            message.Headers.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Clear();

            if (!string.IsNullOrEmpty(request.AccessToken))
            {
                message.Headers.Add("Authorization", $"Bearer {request.AccessToken}");
            }
            var t = JsonConvert.SerializeObject(request.Data);
            message.Content = new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8,
                "application/json");


            HttpResponseMessage response = await client.SendAsync(message);
            var responseContent = await response.Content.ReadAsStringAsync();
            
            _logger.LogError("Paystack API returned a BadRequest response. Response body: {0}", responseContent);
            var linkResponse = JsonConvert.DeserializeObject<T>(responseContent);

            return linkResponse;

        }

        public async Task<T> SendGetRequest<T>(GetRequest request)
        {
            var client = _httpClient.CreateClient();

            HttpRequestMessage message = new HttpRequestMessage();

            message.RequestUri = new Uri(request.Url);
            message.Method = HttpMethod.Get;
            message.Headers.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Clear();

            if (!string.IsNullOrEmpty(request.AccessToken))
            {
                message.Headers.Add("Authorization", $"Bearer {request.AccessToken}");
            }

            HttpResponseMessage response = await client.SendAsync(message);
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseContent);
            var linkResponse = JsonConvert.DeserializeObject<T>(responseContent);

            return linkResponse;

        }

    }

}
