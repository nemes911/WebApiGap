namespace BlazorApp1.Data.Default
{
    public class LoginResponse
    {
        public string Role { get; set; }

        public string SessionId { get; set;}

        public LoginResponse(string role, string sessionId)
        {
            Role = role;
            SessionId = sessionId;
        }

        public LoginResponse() { }
    }
}
