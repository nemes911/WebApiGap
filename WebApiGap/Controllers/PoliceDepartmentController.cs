using API_GAI.DbServices.DefaultCommand.Implements;
using API_GAI.DbServices.DefaultCommand.Interface;
using API_GAI.DbServices.SRC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApiGap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PoliceDepartmentController : ControllerBase
    {
        private readonly IDefaultDB<PoliceDepartment> _Repo;

        public PoliceDepartmentController(IDefaultDB<PoliceDepartment> Repo)
        {
            _Repo = Repo;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] PoliceDepartment police_department)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(await _Repo.UpdateAsync(police_department));
        }
    }
}
