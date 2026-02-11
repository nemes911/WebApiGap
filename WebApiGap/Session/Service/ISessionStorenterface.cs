namespace WebApiGap.Session.Service
{
    public interface ISessionStorenterface
    {
        string Create(string username);

        string GetUser(string sessionId);

        void Remove(string session_id);
    }
}
