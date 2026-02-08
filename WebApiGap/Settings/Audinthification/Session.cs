using API_GAI.DbServices.SRC.Models;

namespace WebApiGap.Settings.Audinthification
{
    public class Session
    {
        private bool _auth = false;

        public Session(bool auth) => (_auth) = (auth);

        public void Succses(Session session)
        {
            session._auth = true;
        }
    }
}
