using System.Text;

namespace API_GAI.Settings
{
    public class AppiSettings
    {
        public string AppConnection { get; set; } = "Host=localhost;Database=postgres;Username=app_user;Password=;Port=5432";
        public string SystemConnection { get; set; } = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=1243;";

        public string AppUserConnection { get; set; } = "Host=localhost;Port=5432;Database=postgres;Username=app_user;Password=1234";

        public string PostgresBase { get; set; } = "Host=localhost;Port=5432;Database=postgres;";
    }
}
