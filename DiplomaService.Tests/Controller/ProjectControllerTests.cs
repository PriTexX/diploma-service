using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiplomaService.Controllers;
using DiplomaService.Controllers.Project;
using DiplomaService.Database;
using DiplomaService.PaginationExtensions;
using DiplomaService.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace DiplomaService.Tests.Controller;

public class ProjectControllerTests
{
    private readonly ProjectController _controller;
    private readonly Mock<IProjectAsyncRepository> _mockRepo;
    private readonly Mock<IResponseHandler> _responseMock;

    public ProjectControllerTests()
    {
        _mockRepo = new Mock<IProjectAsyncRepository>();
        _responseMock = new Mock<IResponseHandler>();
        _controller = new ProjectController(_mockRepo.Object, _responseMock.Object);
    }
    
    [Theory]
    [InlineData(1,2)]
    public async Task GetProjects_ActionExecutes_ReturnsAllProjects( int page, int pageSize)
    {
        var testData = GetTestProfessors(page, pageSize);
        _mockRepo.Setup(repo => repo.GetAll(page, pageSize)).Returns(Task.FromResult(testData));
        
        var result = await _controller.GetProjects(new PaginationQuery{Page = page, PageSize = pageSize});

        var objectResult = Assert.IsType<ObjectResult>(result);
        var projectResponse = Assert.IsAssignableFrom<IEnumerable<ProjectResponseModel>>(objectResult.Value);
        Assert.Equal(testData.Count, projectResponse.Count());
    }
    
    [Theory]
    [InlineData("1")]
    public async Task GetProject_ActionExecutes_ReturnsProjectById(string guid) 
    {
        var testData = GetTestProfessor(guid);
        _mockRepo.Setup(repo => repo.Get(guid)).Returns(Task.FromResult(testData)!);
    
        var result = await _controller.GetProject(guid);
    
        var objectResult = Assert.IsType<ObjectResult>(result);
        var projectResponse = Assert.IsAssignableFrom<ProjectResponseModel>(objectResult.Value);
        Assert.True(testData == projectResponse);
    }
    
    [Theory]
    [InlineData("1", "2")]
    public async Task GetProject_ActionFails_ReturnsNotFound(string realGuid, string fakeGuid) 
    {
        var realTestProject = GetTestProfessor(realGuid);
        _mockRepo.Setup(repo => repo.Get(realGuid)).Returns(Task.FromResult(realTestProject)!);
    
        var result = await _controller.GetProject(fakeGuid);
    
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Theory]
    [InlineData("1")]
    public async Task DeleteProject_ActionExecutes_DeletesProjectById(string guid) 
    {
        var testData = GetTestProfessor(guid);
        _mockRepo.Setup(repo => repo.Delete(guid)).Returns(Task.FromResult(testData)!);
    
        var result = await _controller.DeleteProject(guid);
        
        Assert.IsType<OkResult>(result);
    }
    
    [Theory]
    [InlineData("1")]
    public async Task UpdateProject_ActionExecutes_UpdatesProject(string guid)
    {
        var newProject = CreateTestProfessor(guid, "Stan Stan Stan");
        var newUpdateProject = new UpdateProjectRequestModel()
        {
            Guid = newProject.Guid,
            Name = newProject.Name,
        };
        var testData = GetTestProfessor(guid);
        _mockRepo.Setup(repo => repo.Get(guid)).Returns(Task.FromResult(testData)!);
        _mockRepo.Setup(repo => repo.Update(newProject)).Returns(Task.FromResult(newProject)!);
    
        var updatedData = await _controller.UpdateProject(newUpdateProject);
        
        Assert.IsType<OkResult>(updatedData);
    }
    
    [Theory]
    [InlineData("1", "2")]
    public async Task UpdateProject_ActionFails_ReturnsNotFound(string guid, string fakeGuid)
    {
        var newProject = CreateTestProfessor(guid, "ProjectV");
        var newUpdateProject = new UpdateProjectRequestModel()
        {
            Guid = fakeGuid,
            Name = newProject.Name,
        };
        _mockRepo.Setup(repo => repo.Update(newProject)).Returns(Task.FromResult(newProject)!);
    
        var updatedData = await _controller.UpdateProject(newUpdateProject);
        
        Assert.IsType<NotFoundResult>(updatedData);
    }
    
    [Theory]
    [InlineData("1")]
    public async Task CreateProject_ActionExecutes_CreatesProject(string guid)
    {
        var newProject = CreateTestProfessor(guid, "ProjectV");
        _mockRepo.Setup(repo => repo.Create(newProject)).Returns(Task.FromResult(newProject)!);
    
        var result = await _controller.CreateProject(newProject);
        
        Assert.IsType<CreatedResult>(result);
    }
    
    [Theory]
    [InlineData("1")]
    public async Task CreateProject_ActionFails_ReturnsBadRequest(string guid)
    {
        var newProject = CreateTestProfessor(guid, "ProjectV");
        _mockRepo.Setup(repo => repo.Exists(guid)).Returns(Task.FromResult(true));
        _mockRepo.Setup(repo => repo.Create(newProject)).Returns(Task.FromResult(newProject)!);
        
        var result = await _controller.CreateProject(newProject);
        
        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    private Project CreateTestProfessor(string guid = "", string name = "", string professorId = "", string studentId = "")
    {
        return new Project()
        {
            Name = name,
            Guid = guid,
            ProfessorId = professorId,
            StudentId = studentId
        };
    }
    
    private Project GetTestProfessor(string guid)
    {
        return new Project()
        {
            Name = "ProjectX",
            Guid = guid,
            ProfessorId = "2",
            StudentId = "2"
        };
    }
    
    private PagedList<Project> GetTestProfessors(int page, int pageSize)
    {
        var source = new List<Project>
        {
            new ()
            {
                Name = "Project1",
                Guid = "1",
                ProfessorId = "2",
                StudentId = "2"
            },
            
            new ()
            {
                Name = "Project2",
                Guid = "2",
                ProfessorId = "3",
                StudentId = "4"
            },
            
            new ()
            {
                Name = "Project3",
                Guid = "3",
                ProfessorId = "2",
                StudentId = "5"
            },
        };
        
        return PagedList<Project>.ToPagedList(source.AsQueryable(), page, pageSize);
    } 
}