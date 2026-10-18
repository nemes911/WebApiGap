using API_GAI.DbServices.DefaultCommand.Interface;
using API_GAI.DbServices.SRC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiGap.DbServices.SRC.Models;

namespace WebApiGap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PravaController : ControllerBase
    {
        private readonly IDefaultDB<Prava> _Repo;

        public PravaController(IDefaultDB<Prava> repo)
        {
            _Repo = repo;
        }

        [HttpPost("NewPrava")]
        public async Task<IActionResult> SetPrava([FromBody] Prava prava)
        {
            try
            {
                var res = await _Repo.AddAsync(prava);
                return Ok(res.Id);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
