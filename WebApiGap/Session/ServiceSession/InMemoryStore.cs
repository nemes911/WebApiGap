using Microsoft.AspNetCore.Session;
using System.Collections.Concurrent;
using WebApiGap.Session.Service;

namespace WebApiGap.Session.ServiceSession
{
    public class InMemoryStore : ISessionStorenterface
    {
        private readonly ConcurrentDictionary<string, Session> _sessions = new();

        public string Create(string username)
        {
            var session = new Session
            {
                Id_Session = Guid.NewGuid().ToString(),
                name = username,
                CreatedAt = DateTime.UtcNow,
                IsAuthenticated = true
            };

            _sessions[session.Id_Session] = session;

            return session.Id_Session;
        }

        public string GetUser(string sessionId)
        {
            return _sessions.TryGetValue(sessionId, out var session) && session.IsAuthenticated
                ? session.name
                : null;
        }


        public void Remove(string session_id) => _sessions.TryRemove(session_id, out _);
    }
}
