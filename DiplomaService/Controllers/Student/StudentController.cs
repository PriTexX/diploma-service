using DiplomaService.PaginationExtensions;
using DiplomaService.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DiplomaService.Controllers.Student;

/// <summary>
/// Controller for managing student entities
/// </summary>
//[Authorize(AuthenticationSchemes = "Asymmetric")]
[ApiController]
[Route("api/[controller]")]
public class StudentController : Controller
{
    private readonly IStudentAsyncRepository _studentAsyncRepository;
    private readonly ILogger<StudentController> _logger;

    public StudentController(IStudentAsyncRepository studentAsyncRepository, ILogger<StudentController> logger)
    {
        _studentAsyncRepository = studentAsyncRepository;
        _logger = logger;
        
        logger.LogDebug("Логер инициализирован в контроллере {Name}", nameof(StudentController));
    }
    
    /// <summary>
    /// Retrieve all students
    /// </summary>
    /// <returns>List of all students with their project and professor related to the project</returns>
    /// <response code="200">List of students</response>
    [HttpGet]
    [Route("all")]
    public async Task<IActionResult> GetStudents([FromQuery]PaginationQuery query)
    {
        var data = await _studentAsyncRepository.GetAll(query.Page, query.PageSize);
        
        var metadata = new
        {
            data.TotalCount,
            data.PageSize,
            data.CurrentPage,
            data.TotalPages,
            data.HasNext,
            data.HasPrevious
        };
        
        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
        
        return new ObjectResult(data.Select(StudentResponseModel.ToResponseModel));
    }

    /// <summary>
    /// Retrieve student's info by its guid
    /// </summary>
    /// <param name="guid">Student's guid</param>
    /// <returns>Student with his project and professor related to the project</returns>
    /// <response code="200">Student entity</response>
    /// <response code="404">If no student was found with such guid</response>
    [HttpGet]
    public async Task<IActionResult> GetStudent(string guid)
    {
        var student = await _studentAsyncRepository.Get(guid);

        if (student is null)
        {
            return NotFound();
        }

        return new ObjectResult(StudentResponseModel.ToResponseModel(student));
    }

    /// <summary>
    /// Delete student
    /// </summary>
    /// <param name="guid">Student's guid</param>
    /// <returns>Void</returns>
    /// <response code="200">Success</response>
    /// <response code="404">If no student was found with such guid</response>
    [HttpDelete]
    public async Task<IActionResult> DeleteStudent(string guid)
    {
        try
        {
            await _studentAsyncRepository.Delete(guid);
        }
        catch (ArgumentException e)
        {
            _logger.LogError(e, e.Message);
            return NotFound();
        }

        return Ok();
    }

    /// <summary>
    /// Update student
    /// </summary>
    /// <param name="newStudent"></param>
    /// <response code="200">Success</response>
    /// <response code="400">If there was any model validation errors</response>
    /// <response code="404">If no student was found with such guid</response>
    [HttpPut]
    public async Task<IActionResult> UpdateStudent(UpdateStudentRequestModel newStudent)
    {
        var student = await _studentAsyncRepository.Get(newStudent.Guid);

        if (student is null)
            return NotFound();
        
        newStudent.UpdateStudent(student);
        
        await _studentAsyncRepository.Update(student);

        return Ok();
    }

    /// <summary>
    /// Create student
    /// </summary>
    /// <param name="student"></param>
    /// <response code="201">Created student entity</response>
    /// <response code="400">If student with this guid already exists or there was any model validation errors</response>
    /// <response code="404">If no student was found with such guid</response>
    [HttpPost]
    public async Task<IActionResult> CreateStudent(Database.Student student)
    {
        if (await _studentAsyncRepository.Exists(student.Guid))
            return BadRequest("Student with such guid already exists");
        
        await _studentAsyncRepository.Create(student);

        return new CreatedResult("", StudentResponseModel.ToResponseModel(student));
    }
}