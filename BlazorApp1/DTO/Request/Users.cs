namespace BlazorApp1.DTO.Request;

public class Users
{
    public string password { get; set; } = string.Empty;
    public string name { get; set; } = string.Empty;
    public bool is_conect { get; set; }
    public string? role { get; private set; }

    // POST request for login
    public Users(string name, string password)
    {
        this.name = name;
        this.password = password;
    }

    // out after login
    public Users(bool is_conect, string? role = null)
    {
        this.is_conect = is_conect;
        this.role = role;
    }

    public Users(string name, string password, string role, bool is_conect)
    {
        this.name = name;
        this.password = password;
        this.role = role;
        this.is_conect = is_conect;
    }

    public Users() { }
}

