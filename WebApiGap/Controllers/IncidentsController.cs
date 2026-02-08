using API_GAI.DbServices.SRC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API_GAI.DbServices.DefaultCommand.Interface;
using API_GAI.DbServices.DefaultCommand.Implements;

namespace API_GAI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentsController : ControllerBase
    {
        private readonly DefaultDb<Incident> _Repo;

        public IncidentsController(DefaultDb<Incident> repo, PostgresContext context)
        {
            _Repo = repo;
        }

        [HttpPost("SetIncident")]
        public async Task<IActionResult> AddAsync([FromBody] Incident incident) 
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await _Repo.AddAsync(incident));
        }

        [HttpGet("AllIncidents")]
        public async Task<IActionResult> GetAllAsync() 
        {
            return Ok(await _Repo.GetAllAsync());
        }
       
        [HttpPut("UpdateIncide")]
        public async Task<IActionResult> UpdateAsync(Incident incident) 
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await _Repo.UpdateAsync(incident));
        }
    }
}
