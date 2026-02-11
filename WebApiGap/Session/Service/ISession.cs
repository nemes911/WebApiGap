using WebApiGap.DbServices.DefaultCommand.Interface;

namespace WebApiGap.Session.Service
{
    public interface ISession
    {
        string Id_Session { get; set; }

        string name { get; set; }

        DateTime CreatedAt { get; set; }

        bool IsAuthenticated { get; set; } 


    }
}
