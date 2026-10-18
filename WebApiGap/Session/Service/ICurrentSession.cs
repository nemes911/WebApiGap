using Microsoft.AspNetCore.Components.Web;

namespace WebApiGap.Session.Service
{
    public interface ICurrentSession
    {
        string? SessionId { get; set; }

        Session.ServiceSession.Session? Session { get; set; }

        bool IsAuthentificated { get; set; }
    }
}
