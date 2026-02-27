namespace BlazorApp1.Data.Dinamic.Datamodel
{
    public interface ICookie
    {
        public void WriteCookie(string key, string value, int? Expireceday = null);

        public string ReadCookie(string key);

    }
}
