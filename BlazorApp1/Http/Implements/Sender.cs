using BlazorApp1.Http.Service;
using BlazorApp1.SettingsApi;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlazorApp1.Http.Implements
{
    public class Sender<T> : ISender<T> where T : class
    {
        private readonly IHttpClientFactory _factory;
        private readonly Key _key;

        public Sender(IHttpClientFactory factory, Key key)
        {
            _factory = factory;
            _key = key;
        }

        public async Task<HttpResponseMessage> SendBaseAsync<T>(T entity) where T : class
        {
            var client = _factory.CreateClient("BlazorClient");

            var request = new HttpRequestMessage(HttpMethod.Post, "api/login");

            var json = JsonSerializer.Serialize(entity);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            return await client.SendAsync(request);
        }

        public async Task<HttpResponseMessage> SendAsync<T>(
            T entity,
            string newUri,
            HttpMethod method) where T : class
        {
            var client = _factory.CreateClient("BlazorClient");

            var request = new HttpRequestMessage(method, newUri);

            var json = JsonSerializer.Serialize(entity);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var sessionId = _key.GetSession().ToString();

            if (!string.IsNullOrEmpty(sessionId))
            {
                request.Headers.Add("X-Session-Id", sessionId);
            }

            return await client.SendAsync(request);
        }
    }


}

