using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Specialized;

namespace BlazorApp1.SettingsApi
{
    public class Key
    {
        IMemoryCache chache;

        public Key(IMemoryCache chache)
        {
            this.chache = chache;
        }

        public async  void SetSession(string id)
        {
            chache.Set("Session_id", id, new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromHours(8)));

        }

        public async Task<string?> GetSession()
        {
            var result = chache.TryGetValue("Session_id", out string? key);

            if(key != null)
            {
                return key;
            }
            return null;
           
        }
    }
}
