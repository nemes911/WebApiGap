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

        [HttpGet("AllIncidents")]//вот запрос сюда 
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
        [HttpGet("by-date")]
        public async Task<IActionResult> GetByDate(DateOnly date)
        {
            List<Incident> incident = await _Repo.GetByAsync<DateOnly>(x => x.IncidentDate, date);
            if (incident == null) return BadRequest();
            return Ok(incident);
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

        [HttpGet("GetFullInfoIncidents")]
        public IActionResult GetFullInfoIncident([FromBody] PoliceStation station)
        {
            return Ok(_JDb.GetFullInfoIncident(station));
        }

        /// <summary>
        /// Симметричное внутреннее соединение с условием по датам
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        [HttpGet("IncidentsByDate")]
        public IActionResult GetIncidentsByDate([FromQuery] DateOnly dateFrom, [FromQuery] DateOnly dateTo)
        {
            return Ok(_JDb.GetIncidentsByDateRange(dateFrom, dateTo));
        }

        /// <summary>
        /// Симметричное внутреннее соединение с условием по автомобилю
        /// </summary>
        /// <param name="vehicleID"></param>
        /// <returns></returns>
        [HttpGet("IncidentsByVehicle/{vehicleid}")]
        public IActionResult GetIncidentsByVehicle(Guid vehicleID)
        {
            var vehicle = new Vehicle { Id = vehicleID };
            return Ok(_JDb.GetIncidentByVehicle(vehicle));
        }

        /// <summary>
        /// Левое внешнее соединение
        /// </summary>
        /// <returns></returns>
        [HttpGet("LeftJoinIncidentsWithOfficers")]
        public IActionResult GetLeftJoinIncidentsWithOfficers()
        {
            return Ok(_JDb.GetLeftJoinIncidentsWithOfficers());
        }

        /// <summary>
        /// Запрос на запросе по принципу левого соединения
        /// </summary>
        /// <param name="OfficerId"></param>
        /// <returns></returns>
        [HttpGet("SubqueryLeftJoinStyle/{officerId}")]
        public IActionResult GetSubqueryLeftJoinStyle([FromQuery] Guid OfficerId)
        {
            var officer = new Officer { Id = OfficerId };
            return Ok(_JDb.GetSubqueryLeftJoinStyle(officer));
        }


        [HttpGet("AggregateNoCondition")]
        public IActionResult GetAggregateNoCondition() => Ok(_JDb.GetAggregateNoCondition());

        [HttpGet("AggregateWithDataCondition")]
        public IActionResult GetAggregateWithDataCondition([FromQuery] DateOnly dateFrom)
            => Ok(_JDb.GetAggregateWithDataCondition(dateFrom));

        [HttpGet("AggregateWithGroupCondition")]
        public IActionResult GetAggregateWithGroupCondition([FromQuery] int minIncidents)
            => Ok(_JDb.GetAggregateWithGroupCondition(minIncidents));

        [HttpGet("AggregateWithBothConditions")]
        public IActionResult GetAggregateWithBothConditions([FromQuery] DateOnly from, [FromQuery] DateOnly to, [FromQuery] decimal minDamage)
            => Ok(_JDb.GetAggregateWithBothConditions(from, to, minDamage));

        [HttpGet("SubqueryAggregate")]
        public IActionResult GetSubqueryAggregate() => Ok(_JDb.GetSubqueryAggregate());

        [HttpGet("AggregateWithSubquery")]
        public IActionResult GetAggregateWithSubquery() => Ok(_JDb.GetAggregateWithSubquery());
    }
}
