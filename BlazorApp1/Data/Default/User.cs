namespace BlazorApp1.Data.Default
{
    public class User
    {
        public string name {  get; set; }

        public string? password { get; set; }

       public User(string name, string? password)
        {
            this.name = name;
            this.password = password;
        }

        public User () { }
    }
}
