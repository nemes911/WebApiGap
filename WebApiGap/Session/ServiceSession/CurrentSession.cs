using System.Diagnostics.CodeAnalysis;
using WebApiGap.Session.Service;

namespace WebApiGap.Session.ServiceSession
{
    public class CurrentSession : ICurrentSession
    {
        public string? SessionId {  get; set; }

        public Session? Session { get; set;  }

        public bool IsAuthentificated { get; set; }


        public CurrentSession(string? sessionId, Session? session, bool isAuthentificated)
        {
            SessionId = sessionId;
            Session = session;
            IsAuthentificated = isAuthentificated;
        }

        public CurrentSession() { }
    }
}
