using API_GAI.DbServices.SRC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApiGap.Settings.Audinthification;


namespace WebApiGap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {


        [HttpPost("Check-Session")]
        public async Task<IActionResult> Check([FromBody] Users user)
        {
            if(user.session == null)
            {
                return RedirectToRoute("http://localhost7011/test-auth/");
            }
            else
            {
                return Ok(user);
            }
        }
    }
}
