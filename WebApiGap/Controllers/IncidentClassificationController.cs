using API_GAI.DbServices.DefaultCommand.Interface;
using API_GAI.DbServices.SRC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApiGap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentClassificationController : ControllerBase
    {
        private readonly IDefaultDB<IncidentClassification> _repo;

        public IncidentClassificationController(IDefaultDB<IncidentClassification> repo)
        {
            _repo = repo;
        }


        [HttpGet("Get-Classification")]
        public async Task<IActionResult> GetClassification()
        {
            var incidentclassification = await _repo.GetAllAsync();

            return Ok(incidentclassification);
        }
    }
}
