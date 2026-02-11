namespace WebApiGap.Session.Service
{
    public interface ISessionStorenterface
    {
        string Create(string username, string password);

        string GetUser(string sessionId);

        string GetPassword(string password);
        void Remove(string session_id);
    }
}
