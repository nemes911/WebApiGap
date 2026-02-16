namespace BlazorApp1.Http.Service
{
    public class SessionService
    {
        public string id { get; private set; }

        public string name { get; private set; }

        public string role { get; private set; }

        public void Set(string id, string name, string role)
        {
           id = id ?? throw new ArgumentNullException("id");
            name = name ?? throw new ArgumentNullException(
                "name");
            role = role ?? throw new ArgumentNullException("role");
        }

        public bool IsAuthenticated => !string.IsNullOrEmpty(id);
    }
}
