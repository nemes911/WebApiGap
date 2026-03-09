using API_GAI.DbServices.DefaultCommand.Implements;
using API_GAI.DbServices.DefaultCommand.Interface;
using API_GAI.DbServices.SRC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_GAI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentVehiclesController : ControllerBase
    {
        private readonly IDefaultDB<IncidentVehicle> _Repo;

        public IncidentVehiclesController(IDefaultDB<IncidentVehicle> Repo) 
        {
            _Repo = Repo;
        }

        [HttpPost("SetIncidentVehicle")]
        public async Task<IActionResult> AddAsync([FromBody]IncidentVehicle incidentvehicle) 
        {
            if (!ModelState.IsValid) return BadRequest();
            return Ok(await _Repo.AddAsync(incidentvehicle));
        }

        [HttpPut("UpdateIncident")]
        public async Task<IActionResult> UpdateAsync([FromBody] IncidentVehicle incidentvehicle) 
        {
            if (!ModelState.IsValid) return BadRequest();
            return Ok(await _Repo.UpdateAsync(incidentvehicle));
        }
        [HttpGet("GetAllIncidentVehicle")]
        public async Task<IActionResult> GetAllAsync() 
        {
            return Ok(await _Repo.GetAllAsync());
        }
    }
}
