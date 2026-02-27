using BlazorApp1.Data.Dinamic.Datamodel;

namespace BlazorApp1.Data.Dinamic.Service
{
    public class cookieService : ICookie
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public cookieService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

     
        public  string ReadCookie(string key)
        {
            return _httpContextAccessor.HttpContext.Request.Cookies[key];
        }

        public void WriteCookie(string key, string value, int? Expireceday = null)
        {
            var options = new CookieOptions();

            if (Expireceday.HasValue)

                options.Expires = DateTimeOffset.Now.AddDays(Expireceday.Value);

            _httpContextAccessor.HttpContext?.Response.Cookies.Append(key, value, options);
                    
        }
    }
}
