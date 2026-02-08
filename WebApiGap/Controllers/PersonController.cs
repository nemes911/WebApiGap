using API_GAI.DbServices.DefaultCommand.Implements;
using API_GAI.DbServices.DefaultCommand.Interface;
using API_GAI.DbServices.SRC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApiGap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IDefaultDB<Person> _Repo;

        public PersonController(DefaultDb<Person> Repo)
        {
            _Repo = Repo;
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] Person person)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(await  _Repo.AddAsync(person));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] Person person)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(await _Repo.UpdateAsync(person));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _Repo.GetAllAsync());
        }
    }
}
