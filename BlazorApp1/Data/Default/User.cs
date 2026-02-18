using System.ComponentModel.DataAnnotations;

namespace BlazorApp1.Data.Default
{
    public class User
    {
        [Required]
        public string name {  get; set; }

        [Required]
        public string? password { get; set; }

       public User(string name, string? password)
        {
            this.name = name;
            this.password = password;
        }

        public User () { }
    }
}
