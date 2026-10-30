using Microsoft.Win32;

namespace API_GAI.DbServices.SRC.Models
{
    public class Users
    {
        public string? Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string? Role { get; set; }

        public bool IsConnected {  get; set; } = false;

        public DateTime? LastLogin { get; set; }

        public Users(string Id, string Name, string Password, string? Rol, bool IsConnected, DateTime? LastLogin)
        {
            this.Id = Id;
            this.Name = Name;
            this.Password = Password;
            this.Role = Rol;
            this.IsConnected = IsConnected;
            this.LastLogin = LastLogin;
        }

        

        public Users() { }
    }
}
