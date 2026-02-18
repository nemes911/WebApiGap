namespace BlazorApp1.Http.Service
{
    public class SessionService
    {
        public string id { get; private set; }

        public string name { get; private set; }

        public string role { get; private set; }

        public void Set(string id, string name, string role)
        {
           this.id = id ?? throw new ArgumentNullException("id"); 
            this.name = name ?? throw new ArgumentNullException(
                "name");
            this.role = role ?? throw new ArgumentNullException("role");
        }

        public bool IsAuthenticated => !string.IsNullOrEmpty(id);
    }
}
