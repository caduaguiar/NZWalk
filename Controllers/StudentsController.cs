using Microsoft.AspNetCore.Mvc;

namespace NZWalk.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllStudents()
    {
        string[] studentsNames = new string[] { "Cadu", "Lucio" };

        return Ok(studentsNames);
    }
}