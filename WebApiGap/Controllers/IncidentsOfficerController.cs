using API_GAI.DbServices.DefaultCommand.Implements;
using API_GAI.DbServices.SRC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_GAI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentsOfficerController : ControllerBase
    {
        private readonly DefaultDb<IncidentOfficer> _Repo;

        public IncidentsOfficerController(DefaultDb<IncidentOfficer> Repo) 
        {
            _Repo = Repo;
        }

        [HttpPost("SetIncidentOfficer")]
        public async Task<IActionResult> AddAsync([FromBody] IncidentOfficer incidentofficer) 
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await _Repo.AddAsync(incidentofficer));
        }

        [HttpPut("UpdateIncidentOfficer")]
        public async Task<IActionResult> UpdateAsync([FromBody] IncidentOfficer incidentofficer) 
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await _Repo.UpdateAsync(incidentofficer));
        }

        [HttpGet("GetIncidentOfficer")]
        public async Task<IActionResult> GetAll() 
        {
            return Ok(await _Repo.GetAllAsync());
        }
    }
}
