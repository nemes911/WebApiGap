using API_GAI.DbServices.DefaultCommand.Interface;
using API_GAI.DbServices.SRC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WebApiGap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PoliceStationController : ControllerBase
    {
        private readonly IDefaultDB<PoliceStation> _Repo;
        
        public PoliceStationController(IDefaultDB<PoliceStation> repo)
        {
            _Repo = repo;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] PoliceStation policeStation)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(await  _Repo.UpdateAsync(policeStation));
        }

        [HttpGet("get-all-policestation")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _Repo.GetAllAsync());
        }

    }
}
