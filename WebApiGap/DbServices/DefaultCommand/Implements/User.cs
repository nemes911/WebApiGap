using WebApiGap.DbServices.DefaultCommand.Interface;

namespace WebApiGap.DbServices.DefaultCommand.Implements
{
    public class User : IUser
    {
        public string name { get; set;  } = "";

        public string password { get; set; } = "";
    }
}
