using Microsoft.AspNetCore.Mvc;

namespace SchoolRegistry.Controllers;

public class HomeworkController : Controller    
{
    private readonly string _uploadsDirectory;
    private readonly string _downloadsDirectory;

    public HomeworkController(IConfiguration config)
    {
        _uploadsDirectory = config.GetValue<string>("UploadsDirectory");
        _downloadsDirectory = config.GetValue<string>("DownloadsDirectory");
    }

    [HttpPost]
    [Route("homework")]
    public IActionResult SubmitHomework(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("File not selected or empty");
        }

        // Generate unique file name
        var uniqueFileName = $"{Guid.NewGuid().ToString()}-{Path.GetFileName(file.FileName)}";

        // Combine the uploads directory with the unique file name
        var filePath = Path.Combine(_uploadsDirectory, uniqueFileName);

        // Save the file to the server
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            file.CopyTo(stream);
        }

        return Ok(new { message = $"Homework submitted successfully. Name:{uniqueFileName}" });
    }

    [HttpGet]
    [Route("homework/{fileName}")]
    public IActionResult ReadHomework(string fileName)
    {
        // Combine the uploads directory with the file name
        var filePath = Path.Combine(_downloadsDirectory, fileName);

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound();
        }

        // Read the file from the server
        var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

        // Return the file as the response
        return File(fileStream, "application/octet-stream", fileName);
    }

    [HttpDelete]
    [Route("homework/{fileName}")]
    public IActionResult DeleteHomework(string fileName)
    {
        // Combine the uploads directory with the file name
        var filePath = Path.Combine(_uploadsDirectory, fileName);

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound();
        }

        // Delete the file from the server
        System.IO.File.Delete(filePath);

        return Ok(new { message = "Homework deleted successfully" });
    }

}
