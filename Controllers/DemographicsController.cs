using Microsoft.AspNetCore.Mvc;
using DemographicsApi.Services;

namespace DemographicsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DemographicsController : ControllerBase
    {
        private readonly DemographicsService _service;

        public DemographicsController(DemographicsService service)
        {
            _service = service;
        }

        // GET /api/demographics/neha
        [HttpGet("{name}")]
        public async Task<IActionResult> GetDemographics(string name)
        {
            var result = await _service.GetDemographicDataAsync(name);
            return Ok(result);
        }
    }
}
