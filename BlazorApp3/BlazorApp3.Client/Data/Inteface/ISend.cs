namespace BlazorApp3.Client.Data.Inteface
{
    public interface ISend<T> where T : class
    {
        T SendPostAsync(T message);

        T SendGetAsync(T message);

        T SendPutAsync(T message);
    }
}
