namespace WebApiGap.Session.Service
{
    public interface ISessionStorenterface
    {
        string Create(string username, string password);

        string Create_(string username, string password, string role);

        ServiceSession.Session? GetSession(string id);

        string GetUser(string sessionId);

        string GetPassword(string password);

        void ChangeSession(string sessionId);
        void Remove(string session_id);
    }
}
