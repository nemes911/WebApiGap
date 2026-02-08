using API_GAI.Settings;
using Microsoft.Extensions.Options;

namespace WebApiGap.DbServices.SRC.Data.UserUpdate
{
    public class MenegUser
    {
        private readonly string _connectionstring;

        public MenegUser(IOptions<AppiSettings> options)
        {
            var setings = options.Value;

            _connectionstring = setings.AppUserConnection;
        }

        
    }
}
