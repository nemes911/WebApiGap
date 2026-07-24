using Microsoft.AspNetCore.Session;
using System.Collections.Concurrent;
using WebApiGap.Log.ServiceLog;
using WebApiGap.Session.Service;

namespace WebApiGap.Session.ServiceSession
{
    public class InMemoryStore : ISessionStorenterface
    {
        private readonly ConcurrentDictionary<string, Session> _sessions = new();

        public void ChangeSession(string sessionId)
        {
            throw new NotImplementedException();
        }

        public string Create(string username, string password)
        {
            throw new NotImplementedException();
        }

        public string Create_(string username, string password, string role)
        {
            string id = Guid.NewGuid().ToString();

            var newSession = new Session(id, username, password, role, DateTime.UtcNow);

            _sessions.TryAdd(id, newSession);

            var log = new ServiceLog(id, username, DateTime.UtcNow, "Login", "Success");

            _ = ServiceLog.CreateLogAsync(log);

            return id;
        }

        public string GetPassword(string password)
        {
            throw new NotImplementedException();
        }

        public Session? GetSession(string sessionId)
        {
            _sessions.TryGetValue(sessionId, out var session);

            return session;
        }

        public string GetUser(string sessionId)
        {
            throw new NotImplementedException();
        }

        public void Remove(string session_id) => _sessions.TryRemove(session_id, out _);
    }
}
