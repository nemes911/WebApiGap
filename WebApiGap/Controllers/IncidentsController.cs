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
        private readonly IDefaultDB<Incident> _Repo;

        public IncidentsController(IDefaultDB<Incident> repo)
        {
            _Repo = repo;
        }

        [HttpPost("SetIncident")]
        public async Task<IActionResult> AddAsync([FromBody] Incident incident) 
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            incident.PoliceStation = null;
            incident.IncidentClass = null;
            incident.IncidentVehicles = new List<IncidentVehicle>();

            return Ok(await _Repo.AddAsync(incident));
        }

        [HttpGet("AllIncidents")]
        public async Task<IActionResult> GetAllAsync() 
        {
            var incidents = await _Repo.GetAllAsync(
                i => i.IncidentClass,
                i => i.PoliceStation
                );

            return Ok(incidents);
        }
       
        [HttpPut("UpdateIncident")]
        public async Task<IActionResult> UpdateAsync(Incident incident) 
        {

            incident.PoliceStation = null!;
            incident.IncidentClass = null!;
            incident.IncidentVehicles = new List<IncidentVehicle>();

            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await _Repo.UpdateAsync(incident));
        }
    }
}
