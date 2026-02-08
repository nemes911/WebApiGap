using API_GAI.DbServices.SRC.Models;
using API_GAI.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApiGap.DbServices.DefaultCommand.Interface;

namespace WebApiGap.DbServices.PostgresFactory
{
    public class PostgresContextFactory
    {
        private readonly AppiSettings _settings;

        private readonly IUser _user;

        public PostgresContextFactory(
            IOptions<AppiSettings> options,
            IUser user)
        {
            _settings = options.Value;
            _user = user;
        }

        public PostgresContext Create()
        {
            var connString =
                $"{_settings.PostgresBase};Username={_user.name};Password={_user.password}";

            var options = new DbContextOptionsBuilder<PostgresContext>()
                .UseNpgsql(connString)
                .Options;

            return new PostgresContext(options);

        }
    }
}
