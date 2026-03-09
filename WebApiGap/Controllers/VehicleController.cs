using API_GAI.DbServices.DefaultCommand.Implements;
using API_GAI.DbServices.DefaultCommand.Interface;
using API_GAI.DbServices.SRC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.Xml;
using WebApiGap.DbServices.Join.Inner;
using WebApiGap.DbServices.SRC.Models;

namespace WebApiGap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IDefaultDB<Vehicle> _Repo;

        private readonly JDb _jDb;

        public VehicleController(IDefaultDB<Vehicle> repo, JDb jDb)
        {
            _Repo = repo;
            _jDb = jDb;
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] Vehicle vehicle)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState); 
            return Ok(await _Repo.AddAsync(vehicle));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _Repo.GetAllAsync());
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] Vehicle vehicle)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await _Repo.UpdateAsync(vehicle));
        }

        [HttpGet("get-prava")]
        public async Task<IActionResult> GetUnAccsesPrava()
        {
            List<JoinPrava> prav;
            return Ok(prav = _jDb.GetOnPrava());
        }
    }
}
