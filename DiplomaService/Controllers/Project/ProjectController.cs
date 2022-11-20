using DiplomaService.PaginationExtensions;
using DiplomaService.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DiplomaService.Controllers.Project;

/// <summary>
/// Controller for managing project entities
/// </summary>
//[Authorize(AuthenticationSchemes = "Asymmetric")]
[ApiController]
[Route("api/[controller]")]
public class ProjectController : Controller
{
    private readonly IProjectAsyncRepository _projectAsyncRepository;
    private readonly IResponseHandler _responseHandler;

    public ProjectController(IProjectAsyncRepository projectAsyncRepository, IResponseHandler responseHandler)
    {
        _projectAsyncRepository = projectAsyncRepository;
        _responseHandler = responseHandler;
    }
    
    /// <summary>
    /// Retrieve all projects
    /// </summary>
    /// <returns>List of all projects with related student and professor</returns>
    /// <response code="200">List of projects</response>
    [HttpGet]
    [Route("all")]
    public async Task<IActionResult> GetProjects([FromQuery]PaginationQuery query)
    {
        var data = await _projectAsyncRepository.GetAll(query.Page, query.PageSize);
        
        var metadata = new
        {
            data.TotalCount,
            data.PageSize,
            data.CurrentPage,
            data.TotalPages,
            data.HasNext,
            data.HasPrevious
        };
        
        _responseHandler.AddHeader(Response,"X-Pagination", metadata);
        
        return new ObjectResult(data.Select(ProjectResponseModel.ToResponseModel));
    }

    /// <summary>
    /// Retrieve project's info by its guid
    /// </summary>
    /// <param name="guid">Project's guid</param>
    /// <returns>Project with related student and professor</returns>
    /// <response code="200">Project entity</response>
    /// <response code="404">If no project was found with such guid</response>
    [HttpGet]
    public async Task<IActionResult> GetProject(string guid)
    {
        var project = await _projectAsyncRepository.Get(guid);

        if (project is null)
        {
            return NotFound();
        }

        return new ObjectResult(ProjectResponseModel.ToResponseModel(project));
    }

    /// <summary>
    /// Delete project
    /// </summary>
    /// <param name="guid">Project's guid</param>
    /// <returns>Void</returns>
    /// <response code="200">Success</response>
    /// <response code="404">If no project was found with such guid</response>
    [HttpDelete]
    public async Task<IActionResult> DeleteProject(string guid)
    {
        try
        {
            await _projectAsyncRepository.Delete(guid);
        }
        catch (ArgumentException)
        {
            return NotFound();
        }

        return Ok();
    }

    /// <summary>
    /// Update project
    /// </summary>
    /// <param name="newProject"></param>
    /// <response code="200">Success</response>
    /// <response code="400">If there was any model validation errors</response>
    /// <response code="404">If no project was found with such guid</response>
    [HttpPut]
    public async Task<IActionResult> UpdateProject(UpdateProjectRequestModel newProject)
    {
        var project = await _projectAsyncRepository.Get(newProject.Guid);

        if (project is null)
            return NotFound();
        
        newProject.UpdateProject(project);
        
        await _projectAsyncRepository.Update(project);

        return Ok();
    }

    /// <summary>
    /// Create student
    /// </summary>
    /// <param name="project"></param>
    /// <response code="201">Created project entity</response>
    /// <response code="400">If project with this guid already exists or there was any model validation errors</response>
    /// <response code="404">If no project was found with such guid</response>
    [HttpPost]
    public async Task<IActionResult> CreateProject(Database.Project project)
    {
        await _projectAsyncRepository.Create(project);

        return new CreatedResult("", ProjectResponseModel.ToResponseModel(project));
    }
}