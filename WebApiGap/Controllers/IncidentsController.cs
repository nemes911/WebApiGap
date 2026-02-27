using API_GAI.DbServices.SRC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API_GAI.DbServices.DefaultCommand.Interface;
using API_GAI.DbServices.DefaultCommand.Implements;
using WebApiGap.DbServices.SRC.Models;
using WebApiGap.DbServices.Join.Inner;

namespace API_GAI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentsController : ControllerBase
    {
        private readonly IDefaultDB<Incident> _Repo;

        private readonly JDb _JDb;

        public IncidentsController(IDefaultDB<Incident> repo, JDb jDb)
        {
            _Repo = repo;
            _JDb = jDb;
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

        //get count incidents
        [HttpGet("CountIncident")]
        public async Task<IActionResult> GetCoutntByDistrict()
        {
            List<DistricStat> stat;
            return Ok(stat = _JDb.GetIncidentGroupDistrict());
        }

        //views
        [HttpPost("GetFullIncident")]

        public async Task<IActionResult> GetViewIncedent(ViewIncidents incident)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            List<ViewIncidents> list;
            return Ok(list = _JDb.GetIncidents(incident));
        }
    }
}
