using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Specialized;
using BlazorApp1.Http.Service;

namespace BlazorApp1.SettingsApi
{
    public class Key
    {
        IMemoryCache chache;

        public Key(IMemoryCache chache)
        {
            this.chache = chache;
        }

        public async  Task SetSession(SessionService session)
        {
            chache.Set<SessionService>("Session", session, new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromHours(8)));

        }

        public virtual async Task<string?> GetSession()
        {
            var result = chache.TryGetValue("Session", out SessionService? key);

            if(key.id != null && key.name != null && key.role != null)
            {
                return key.role + key.name;
            }
            return null;


           
        }

        public  async Task<string?> GetSessionId()
        {
            var result = chache.TryGetValue("Session", out SessionService? key);

            if(key.id != null)
            {
                return key.id;
            }
            return null;
        } 
        
    }
}
