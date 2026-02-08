

using System.Net;

namespace WebApiGap.Settings
{
    public class Host
    {
        IPAddress address;   
        
        public Host(IPAddress iP)
        {
            address = iP;
        }

        private bool CheckIpNetworke(IPAddress ip)
        {
            //проверять лог файл подключений и  30 дней с момента входа
            return false;
        }


    }
}
