using API_GAI.DbServices.SRC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API_GAI.DbServices.DefaultCommand.Interface;
using API_GAI.DbServices.DefaultCommand.Implements;
using WebApiGap.DbServices.SRC.Queries;

namespace API_GAI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentsController : ControllerBase
    {
        private readonly IDefaultDB<Incident> _Repo;

        private readonly QueryList _queryList;

        public IncidentsController(IDefaultDB<Incident> repo, QueryList queryList)
        {
            _Repo = repo;
            _queryList = queryList;
        }

        [HttpPost("SetIncident")]
        public async Task<IActionResult> AddAsync([FromBody] Incident incident)
        {
            incident.Id = null;
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
        [HttpPost("GetStatByDate")]
        public async Task<IActionResult> GetStatDate([FromBody]DateTime date_start, DateTime date_end)
        {

            return Ok(await _queryList.EquelseDTPPerMothAsync(date_start, date_end));
        }

    }
}
