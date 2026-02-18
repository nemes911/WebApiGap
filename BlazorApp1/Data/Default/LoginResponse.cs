namespace BlazorApp1.Data.Default
{
    public class LoginResponse
    {
        public string Role { get; set; }

        public string Session_Id { get; set;}

        public LoginResponse(string role, string sessionId)
        {
            Role = role;
            this.Session_Id = sessionId;
        }

        public LoginResponse() { }
    }
}
