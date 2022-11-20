using DiplomaService.PaginationExtensions;
using DiplomaService.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DiplomaService.Controllers.Professor;

/// <summary>
/// Controller for managing professor entities
/// </summary>
//[Authorize(AuthenticationSchemes = "Asymmetric")]
[ApiController]
[Route("api/[controller]")]
public class ProfessorController : Controller
{
    private readonly IProfessorAsyncRepository _professorAsyncRepository;
    private readonly IResponseHandler _responseHandler;

    public ProfessorController(IProfessorAsyncRepository professorAsyncRepository, IResponseHandler responseHandler)
    {
        _professorAsyncRepository = professorAsyncRepository;
        _responseHandler = responseHandler;
    }

    /// <summary>
    /// Retrieve all professors
    /// </summary>
    /// <returns>List of all professors with their projects and related students</returns>
    /// <response code="200">List of professors</response>
    [HttpGet]
    [Route("all")]
    public async Task<IActionResult> GetProfessors([FromQuery]PaginationQuery query)
    {
        var data = await _professorAsyncRepository.GetAll(query.Page, query.PageSize);
        
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
        
        return new ObjectResult(data.Select(ProfessorResponseModel.ToResponseModel));
    }
    
    /// <summary>
    /// Retrieve professor's info by its guid
    /// </summary>
    /// <param name="guid">Professor's guid</param>
    /// <returns>Professor with related projects and students</returns>
    /// <response code="200">Professor entity</response>
    /// <response code="404">If no professor was found with such guid</response>
    [HttpGet]
    public async Task<IActionResult> GetProfessor(string guid)
    {
        var professor = await _professorAsyncRepository.Get(guid);

        if (professor is null)
        {
            return NotFound();
        }

        return new ObjectResult(ProfessorResponseModel.ToResponseModel(professor));
    }

    /// <summary>
    /// Delete professor
    /// </summary>
    /// <param name="guid">Professor's guid</param>
    /// <returns>Void</returns>
    /// <response code="200">Success</response>
    /// <response code="404">If no professor was found with such guid</response>
    [HttpDelete]
    public async Task<IActionResult> DeleteProfessor(string guid)
    {
        try
        {
            await _professorAsyncRepository.Delete(guid);
        }
        catch (ArgumentException)
        {
            return NotFound();
        }

        return Ok();
    }

    /// <summary>
    /// Update professor
    /// </summary>
    /// <param name="newProfessor"></param>
    /// <response code="200">Success</response>
    /// <response code="400">If there was any model validation errors</response>
    /// <response code="404">If no professor was found with such guid</response>
    [HttpPut]
    public async Task<IActionResult> UpdateProfessor(UpdateProfessorRequestModel newProfessor)
    {
        var professor = await _professorAsyncRepository.Get(newProfessor.Guid);

        if (professor is null)
            return NotFound();
        
        newProfessor.UpdateProfessor(professor);
        
        await _professorAsyncRepository.Update(professor);

        return Ok();
    }

    /// <summary>
    /// Create professor
    /// </summary>
    /// <param name="professor"></param>
    /// <response code="201">Created professor entity</response>
    /// <response code="400">If professor with this guid already exists or there was any model validation errors</response>
    /// <response code="404">If no professor was found with such guid</response>
    [HttpPost]
    public async Task<IActionResult> CreateProfessor(Database.Professor professor)
    {
        await _professorAsyncRepository.Create(professor);

        return new CreatedResult(professor.Guid, ProfessorResponseModel.ToResponseModel(professor));
    }
}