using API_GAI.DbServices.DefaultCommand.Implements;
using API_GAI.DbServices.SRC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace API_GAI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficerController : ControllerBase
    {
        private readonly DefaultDb<Officer> _Repo;

        public OfficerController(DefaultDb<Officer> Repo) 
        {
            _Repo = Repo;
        }

        [HttpPost("SetOfficer")]
        public async Task<IActionResult> AddAsync([FromBody] Officer officer) 
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await _Repo.AddAsync(officer));
        }

        [HttpPut("UpdateOfficer")]
        public async Task<IActionResult> UpdateAsync([FromBody] Officer officer) 
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await _Repo.UpdateAsync(officer));
        }

        [HttpGet("GetAllOfficer")]
        public async Task<IActionResult> GetAllAsync([FromBody] Officer officer) 
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await _Repo.GetAllAsync());
        }
    }
}
