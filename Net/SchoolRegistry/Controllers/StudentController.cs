using Microsoft.AspNetCore.Mvc;
using SchoolRegistry.Services;
using SchoolRegistry.Models;

namespace SchoolRegistry.Controllers;

[Controller]
public class StudentController : Controller
{

    private readonly MongoDBService _mongoDBService;

    public StudentController(MongoDBService mongoDBService)
    {
        _mongoDBService = mongoDBService;
    }

    [HttpGet("studentclass/{_classId}/students")]
    public async Task<List<Student>> GetByClassId(string _classId)
    {
        return await _mongoDBService.GetStudentsByClassIdAsync(_classId);
    }

    [HttpGet("studentclass/{_classId}/students/{id}")]
    public async Task<Student> GetByClassIdStudentId(string _classId, string id)
    {
        return await _mongoDBService.GetStudentByClassIdStudentIdAsync(_classId, id);
    }

    [HttpPost("studentclass/{_classId}/students")]
    public async Task<IActionResult> Post([FromBody] Student student, string _classId)
    {
        student._classId = _classId;
        await _mongoDBService.CreateStudentAsync(student);
        return CreatedAtAction(nameof(GetByClassId), new { 
            _classId = _classId, 
            Id = student.Id, 
            name = student.name, 
            lastName = student.lastName, 
            phone = student.phone, 
            email = student.email, 
            dateOfBirth = student.dateOfBirth, 
            _guardianId = student._guardianId }, student);
    }

    [HttpDelete("studentclass/{_classId}/students/{id}")]
    public async Task<IActionResult> Delete(string _classId, string id)
    {
        await _mongoDBService.DeleteStudentAsync(_classId, id);
        return NoContent();
    }

}
