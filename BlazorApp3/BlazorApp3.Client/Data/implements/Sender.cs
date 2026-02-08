using BlazorApp3.Client.Data.Inteface;
using System.Net.Http.Headers;
using System.Text.Json;

namespace BlazorApp3.Client.Data.implements
{
    public sealed class Sender<T> : ISend<T> where T : class
    {
        private readonly IHttpClientFactory _httpclientfactory = null;

        private readonly IConfiguration _configuration;

        private readonly string _url;

        
        public Sender(IHttpClientFactory httpclientfactory, IConfiguration configuration, string clientname)
            => (_httpclientfactory, _configuration, _url) =
            (httpclientfactory, configuration, configuration["TestedUri:Default"]);

        public string serialize<T>(T obj)
        {
            if(obj == null)
            {
                return null;
            }

            string json = JsonSerializer.Serialize(obj);
            return json;
        }

        public async Task<T> SendGetAsync(T message)
        {
            throw new NotImplementedException();
        }

        public async Task<T> SendPostAsync(T message)
        {
            try { 

            string? httpclientname = _configuration["JsonPlaceholder"];

            HttpClient client = _httpclientfactory.CreateClient(httpclientname ?? "");

            using var msg = new HttpRequestMessage(HttpMethod.Post, new Uri(_url));

            var content = new StringContent(serialize(message));

            msg.Content = content;

                using var response = await client.SendAsync(msg);

                return message;
            }
            catch(Exception exp)
            {
                Console.WriteLine(exp.Message);
                return message;
            }

           
        }

        public async Task<T> SendPutAsync(T message)
        {
            throw new NotImplementedException();
        }

        T ISend<T>.SendPostAsync(T message)
        {
            throw new NotImplementedException();
        }

        T ISend<T>.SendGetAsync(T message)
        {
            throw new NotImplementedException();
        }

        T ISend<T>.SendPutAsync(T message)
        {
            throw new NotImplementedException();
        }
    }
}
