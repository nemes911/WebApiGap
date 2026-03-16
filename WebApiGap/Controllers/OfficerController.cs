using API_GAI.DbServices.DefaultCommand.Implements;
using API_GAI.DbServices.DefaultCommand.Interface;
using API_GAI.DbServices.SRC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebApiGap.DbServices.Join.Inner;

namespace API_GAI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficerController : ControllerBase
    {
        private readonly IDefaultDB<Officer> _Repo;
        private readonly JDb _JDb;

        public OfficerController(IDefaultDB<Officer> Repo, JDb jDb) 
        {
            _Repo = Repo;
            _JDb = jDb;
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

        /// <summary>Симметричное внутреннее соединение с условием по офицеру</summary>
        [HttpGet("IncidentsByOfficer/{officerId}")]
        public IActionResult GetIncidentsByOfficer(Guid officerId)
        {
            var officer = new Officer { Id = officerId };
            return Ok(_JDb.GetIncidentsByOfficer(officer));
        }

        /// <summary>Симметричное внутреннее соединение по званию</summary>
        [HttpGet("OfficerByRanks/{rankId}")]
        public IActionResult GetOfficerByRanks(int rankId)
        {
            var rank = new Rank { Id = rankId };
            return Ok(_JDb.GetOfficerByRanks(rank));
        }

        /// <summary>Симметричное внутреннее соединение без условия — все офицеры + станция + ранг</summary>
        [HttpGet("AllOfficersWithStationsAndRanks")]
        public IActionResult GetAllOfficerWithStationsAndRanks()
        {
            return Ok(_JDb.GetAllOfficerWithStationsAndRanks());
        }

        /// <summary>Правое внешнее соединение</summary>
        [HttpGet("RightJoinOfficersWithIncidents")]
        public IActionResult GetRightJoinOfficersWithIncidents()
        {
            return Ok(_JDb.GetRightJoinOfficersWithIncidents());
        }
    }
}
