using Microsoft.AspNetCore.Mvc;
using SchoolRegistry.Services;
using SchoolRegistry.Models;

namespace SchoolRegistry.Controllers;

[Controller]
[Route("studentclass")]
public class StudentClassController : Controller
{

    private readonly MongoDBService _mongoDBService;

    public StudentClassController(MongoDBService mongoDBService)
    {
        _mongoDBService = mongoDBService;
    }

    [HttpGet]
    public async Task<List<StudentClass>> Get()
    {
        return await _mongoDBService.GetAsync();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateClassAsync(string id, [FromBody] string className, [FromBody] int numberOfStudents, [FromBody] string _teacherId)
    {
        await _mongoDBService.UpdateClassAsync(id, className, numberOfStudents, _teacherId);
        return NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] StudentClass studentClass)
    {
        await _mongoDBService.CreateAsync(studentClass);
        return CreatedAtAction(nameof(Get), new { id = studentClass.Id, _teacherId = studentClass._teacherId, numberOfStudents = studentClass.numberOfStudents, className = studentClass.className, }, studentClass);
    }

    [HttpGet("{id}")]
    public async Task<StudentClass> GetById(string id)
    {
        return await _mongoDBService.GetByIdAsync(id);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _mongoDBService.DeleteAsync(id);
        return NoContent();
    }

}   