using API_GAI.DbServices.SRC.Data.Auth;
using API_GAI.DbServices.SRC.Models;
using API_GAI.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using WebApiGap.DbServices.DefaultCommand.Interface;
using WebApiGap.DbServices.PostgresFactory;
using WebApiGap.Session.Service;

namespace API_GAI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly string _connection_string;
        private readonly Authzorization _authzorization;
        private readonly IConfiguration _configuration;
        private readonly IUser _user;
        private readonly ISessionStorenterface _store;


        public UserController(Authzorization auth, IUser user, ISessionStorenterface stroe )
        {
            _authzorization = auth;
            _user = user;
            _store = stroe;
        }

        [HttpPost("test-auth")]
        public async Task<IActionResult> TestConnetion(string name, string password) 
        {
            var role = await _authzorization.AuthAsync(name, password);

            if (string.IsNullOrEmpty(role))
            {
                return Unauthorized();
            }

            var session_ID = _store.Create(name);

            return Ok(new
            {
                role,
                session_ID
            });

        }
    }
}
