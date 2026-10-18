using API_GAI.DbServices.DefaultCommand.Implements;
using API_GAI.DbServices.DefaultCommand.Interface;
using API_GAI.DbServices.SRC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiGap.DbServices.SRC.Queries;

namespace WebApiGap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistrictController : ControllerBase
    {
        private readonly IDefaultDB<District> _Repo;

        private readonly QueryList _queryList;

        public DistrictController(IDefaultDB<District> Repo, QueryList queryList)
        {
            _Repo = Repo;
            _queryList = queryList;
        }

        [HttpPost("set-district")]
        public async Task<IActionResult> AddAsync([FromBody] District district)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(await _Repo.AddAsync(district));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] District district)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(await _Repo.UpdateAsync(district));
        }

        [HttpGet("get-all-district")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _Repo.GetAllAsync());
        }

        [HttpGet("GetStatsByDistrict/{id}")]
        public async Task<IActionResult> GetJoin( int id)
        {
            var result = await _queryList.JoinDistrictIncidents(id);
            return Ok(result);
        }
    }
}
