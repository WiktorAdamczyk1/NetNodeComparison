using Microsoft.AspNetCore.Mvc;
using SchoolRegistry.Models;
using SchoolRegistry.Services;

namespace SchoolRegistry.Controllers
{
    [ApiController]
    [Route("teacher")]
    public class TeacherController : Controller
    {
        private readonly MongoDBService _mongoDBService;

        public TeacherController(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeacherAsync([FromBody] Teacher teacher)
        {
            await _mongoDBService.CreateTeacherAsync(teacher);
            return Ok(teacher);
        }

        [HttpGet]
        public async Task<IActionResult> GetTeachersAsync()
        {
            var teachers = await _mongoDBService.GetTeachersAsync();
            return Ok(teachers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeacherAsync(string id)
        {
            var teacher = await _mongoDBService.GetTeacherAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }
            return Ok(teacher);
        }
    }
}
