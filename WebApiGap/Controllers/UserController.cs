using API_GAI.DbServices.SRC.Data.Auth;
using API_GAI.DbServices.SRC.Models;
using API_GAI.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using WebApiGap.DbServices.DefaultCommand.Implements;
using WebApiGap.DbServices.DefaultCommand.Interface;
using WebApiGap.DbServices.PostgresFactory;
using WebApiGap.DbServices.SRC.Queries;
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
        private readonly QueryList _queryList;


        public UserController(Authzorization auth, IUser user, ISessionStorenterface stroe, QueryList queryList )
        {
            _authzorization = auth;
            _user = user;
            _store = stroe;
            _queryList = queryList;
        }

        [HttpPost("test-auth")]
        public async Task<IActionResult> TestConnetion([FromBody] Users user) 
        {
            Console.WriteLine(user.name, user.password);

            var role = await _authzorization.AuthAsync(user.name, user.password);


            if (string.IsNullOrEmpty(role))
            {
                return Unauthorized();
            }

            var session_ID = _store.Create_(user.name, user.password, user.role);

            return Ok(new
            {
                role,
                session_ID
            });

        }


        //добавления роли в БД 
        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser([FromBody] string username)
        {
            var resu = await _queryList.CreateUser(username);

            return Ok(resu);
        }
    }
}
