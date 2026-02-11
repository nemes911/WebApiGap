using Microsoft.AspNetCore.Session;
using System.Collections.Concurrent;
using WebApiGap.Session.Service;

namespace WebApiGap.Session.ServiceSession
{
    public class InMemoryStore : ISessionStorenterface
    {
        private readonly ConcurrentDictionary<string, (string user, string pass)> _sessions =
            new ConcurrentDictionary<string, (string user, string pass)>();

        public string Create(string username, string password)
        {
            string id = Guid.NewGuid().ToString();
            _sessions.TryAdd(id, (user: username, pass: password));

            return id;
        }
        public string GetUser(string sessionId)
        {
            //return _sessions.TryGetValue(sessionId, out var session) && session.IsAuthenticated
            // ? session.name
            // : null;

            return _sessions.TryGetValue(sessionId, out var session) ? session.user : null;
        }

        public string GetPassword(string sessionId)
        {
            //return _sessions.TryGetValue(sessionId, out var session) && session.IsAuthenticated
            // ? session.password
            // : null;

            return _sessions.TryGetValue(sessionId, out var session) ? session.pass : null;
        }

        public void Remove(string session_id) => _sessions.TryRemove(session_id, out _);
    }
}
