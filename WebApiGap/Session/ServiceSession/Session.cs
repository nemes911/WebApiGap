using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using System.Diagnostics.CodeAnalysis;
using WebApiGap.DbServices.DefaultCommand.Interface;
using WebApiGap.Session.Service;

namespace WebApiGap.Session.ServiceSession
{
    public class Session : Service.ISession
    {
        public string Id_Session { get; set; }
        public string name { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsAuthenticated { get; set; } 

        public Session(string id_Session, string name, DateTime createdAt,  bool isAuthenticated = true)
        {
            Id_Session = id_Session;
            this.name = name;
            CreatedAt = createdAt;
            IsAuthenticated = isAuthenticated;
        }

        public Session() { }

    }


}
