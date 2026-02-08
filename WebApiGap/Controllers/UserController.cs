using API_GAI.DbServices.SRC.Data.Auth;
using API_GAI.DbServices.SRC.Models;
using API_GAI.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using WebApiGap.Settings.Audinthification;

namespace API_GAI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly string _connection_string;
        private readonly Authzorization _authzorization;
        private readonly IConfiguration _configuration;

        public UserController(IOptions<AppiSettings> options, Authzorization authzorization, IConfiguration configuration) 
        {
            var setting = options.Value;

            _connection_string = setting.AppConnection;

            _authzorization = authzorization;

            _configuration = configuration;
        }

        [HttpPost("test-auth")]
        public async Task<IActionResult> TestConnetion(string name, string passord) 
        {

            string role = await _authzorization.AuthAsync(name, passord);

            if (string.IsNullOrEmpty(role))
            {
                return Unauthorized();
            }
            return Ok(role);



            /*
                var role = await _authzorization.AuthAsync(user.name, user.password);

                if (role == null)
                {
                    return Unauthorized("dsfdgf");
                }

               return Ok(role);
            */
        }
    }
}
