using API_GAI.DbServices.DefaultCommand.Implements;
using API_GAI.DbServices.DefaultCommand.Interface;
using API_GAI.DbServices.SRC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApiGap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistrictController : ControllerBase
    {
        private readonly IDefaultDB<District> _Repo;

        public DistrictController(DefaultDb<District> Repo)
        {
            _Repo = Repo;
        }

        [HttpPost]
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

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _Repo.GetAllAsync());
        }
    }
}
