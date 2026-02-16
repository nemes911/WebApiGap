namespace BlazorApp1.Http.Service
{
    public interface ISender<T> where T : class
    {
        Task<HttpResponseMessage> SendBaseAsync<T>(T entity) where T : class;
        Task<HttpResponseMessage> SendAsync<T>(T entity, string newuri,HttpMethod method)where T : class;
    }
}
