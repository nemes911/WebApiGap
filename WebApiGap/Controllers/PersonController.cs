using API_GAI.DbServices.DefaultCommand.Implements;
using API_GAI.DbServices.DefaultCommand.Interface;
using API_GAI.DbServices.SRC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiGap.DbServices.SRC.DTOModels;
using WebApiGap.DbServices.SRC.Queries;

namespace WebApiGap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IDefaultDB<Person> _Repo;

        private readonly QueryList _queryList;

        private readonly IDefaultDB<HumansVies> _RepoViews;

        public PersonController(IDefaultDB<Person> Repo, QueryList queryList, IDefaultDB<HumansVies> RepoViews)
        {
            _Repo = Repo;
            _queryList = queryList;
            _RepoViews = RepoViews;
        }

        [HttpPost("Set-Peson")]
        public async Task<IActionResult> AddAsync([FromBody] Person person)
        {

            if(person.id_prav == Guid.Empty)
            {

            }

            if(!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(await  _Repo.AddAsync(person));
        }

        [HttpPut("Update-Person")]
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

        [HttpGet("ViewsHumanForSelect")]
        public async Task<IActionResult> GetViewForSelect()
        {
            try
            {
                return Ok(await _RepoViews.GetAllAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }

        }

    }
}
