using Microsoft.Win32;

namespace API_GAI.DbServices.SRC.Models
{
    public class Users
    {
        public string password { get; set; }
        public string name { get; set; }

        public bool is_conect { get; set; }
        public string? role { get; private set; }


        //POST request for login
        public Users(string name, string password) 
        {
            this.name = name;
            this.password = password;
        }

        //out after login
        public Users(bool is_conect, string role = null)
        {
            this.is_conect = is_conect;
            this.role = role;
        }

        public Users(string name, string password, string role, bool is_conect)
        {
            this.name=name;
            this.password = password;
            this.role = role;
            this.is_conect =is_conect;
        }

        public Users() { }
    }
}
