namespace MVCGAI.Models
{
    public class LoginViewModel
    {
        public string Name { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public LoginViewModel(string name, string password)
        {
            Name = name;
            Password = password;
        }

        public LoginViewModel() { }
    }
}
