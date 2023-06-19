using Microsoft.AspNetCore.Mvc;
using SchoolRegistry.Models;
using SchoolRegistry.Services;

namespace SchoolRegistry.Controllers
{
    [ApiController]
    [Route("guardian")]
    public class GuardianController : Controller
    {
        private readonly MongoDBService _mongoDBService;

        public GuardianController(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        // create a new guardian
        [HttpPost]
        public async Task<IActionResult> CreateGuardianAsync([FromBody] Guardian guardian)
        {
            try
            {
                await _mongoDBService.CreateGuardianAsync(guardian);
                return Ok(guardian);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // get all guardians
        [HttpGet]
        public async Task<IActionResult> GetGuardiansAsync()
        {
            try
            {
                List<Guardian> guardians = await _mongoDBService.GetGuardiansAsync();
                return Ok(guardians);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // get a single guardian by id
        [HttpGet("{_guardianId}")]
        public async Task<IActionResult> GetGuardianByIdAsync(string _guardianId)
        {
            try
            {
                Guardian guardian = await _mongoDBService.GetGuardianByIdAsync(_guardianId);
                if (guardian == null)
                {
                    return NotFound();
                }
                return Ok(guardian);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
