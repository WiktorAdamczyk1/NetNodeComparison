using Microsoft.AspNetCore.Mvc;

namespace SchoolRegistry.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : Controller
    {
        public class GradesModel
        {
            public List<double> Grades { get; set; }
        }

        [HttpPost]
        [Route("gpa")]
        public IActionResult CalculateGPA([FromBody] GradesModel gradesModel)
        {
            // Perform GPA calculation
            double sum = 0.0;
            foreach (double grade in gradesModel.Grades)
            {
                sum += grade;
            }
            double gpa = sum / gradesModel.Grades.Count;

            // Return result as JSON
            return Ok(new { gpa = gpa });
        }
    }
}
