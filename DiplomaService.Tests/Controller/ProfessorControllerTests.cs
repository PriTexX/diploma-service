using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiplomaService.Controllers;
using DiplomaService.Controllers.Professor;
using DiplomaService.Database;
using DiplomaService.PaginationExtensions;
using DiplomaService.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace DiplomaService.Tests;

public class ProfessorControllerTests
{
    private readonly ProfessorController _controller;
    private readonly Mock<IProfessorAsyncRepository> _mockRepo;
    private readonly Mock<IResponseHandler> _responseMock;

    public ProfessorControllerTests()
    {
        _mockRepo = new Mock<IProfessorAsyncRepository>();
        _responseMock = new Mock<IResponseHandler>();
        _controller = new ProfessorController(_mockRepo.Object, _responseMock.Object);
    }

    [Theory]
    [InlineData(1,2)]
    public async Task GetProfessors_ActionExecutes_ReturnsAllProfessors( int page, int pageSize)
    {
        var testData = GetTestProfessors(page, pageSize);
        _mockRepo.Setup(repo => repo.GetAll(page, pageSize)).Returns(Task.FromResult(testData));
        
        var result = await _controller.GetProfessors(new PaginationQuery{Page = page, PageSize = pageSize});

        var objectResult = Assert.IsType<ObjectResult>(result);
        var professorResponse = Assert.IsAssignableFrom<IEnumerable<ProfessorResponseModel>>(objectResult.Value);
        Assert.Equal(testData.Count, professorResponse.Count());
    }
    
    [Theory]
    [InlineData("1")]
    public async Task GetProfessor_ActionExecutes_ReturnsProfessorById(string guid) 
    {
        var testData = GetTestProfessor(guid);
        _mockRepo.Setup(repo => repo.Get(guid)).Returns(Task.FromResult(testData)!);
    
        var result = await _controller.GetProfessor(guid);
    
        var objectResult = Assert.IsType<ObjectResult>(result);
        var professorResponse = Assert.IsAssignableFrom<ProfessorResponseModel>(objectResult.Value);
        Assert.True(testData == professorResponse);
    }
    
    [Theory]
    [InlineData("1", "2")]
    public async Task GetProfessor_ActionFails_ReturnsNotFound(string realGuid, string fakeGuid) 
    {
        var realTestProfessor = GetTestProfessor(realGuid);
        _mockRepo.Setup(repo => repo.Get(realGuid)).Returns(Task.FromResult(realTestProfessor)!);
    
        var result = await _controller.GetProfessor(fakeGuid);
    
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Theory]
    [InlineData("1")]
    public async Task DeleteProfessor_ActionExecutes_DeletesProfessorById(string guid) 
    {
        var testData = GetTestProfessor(guid);
        _mockRepo.Setup(repo => repo.Delete(guid)).Returns(Task.FromResult(testData)!);
    
        var result = await _controller.DeleteProfessor(guid);
        
        Assert.IsType<OkResult>(result);
    }
    
    [Theory]
    [InlineData("1")]
    public async Task UpdateProfessor_ActionExecutes_UpdatesProfessor(string guid)
    {
        var newProfessor = CreateTestProfessor(guid, "Stan Stan Stan");
        var newUpdateProfessor = new UpdateProfessorRequestModel()
        {
            Guid = newProfessor.Guid,
            FullName = newProfessor.FullName,
        };
        var testData = GetTestProfessor(guid);
        _mockRepo.Setup(repo => repo.Get(guid)).Returns(Task.FromResult(testData)!);
        _mockRepo.Setup(repo => repo.Update(newProfessor)).Returns(Task.FromResult(newProfessor)!);
    
        var updatedData = await _controller.UpdateProfessor(newUpdateProfessor);
        
        Assert.IsType<OkResult>(updatedData);
    }
    
    [Theory]
    [InlineData("1", "2")]
    public async Task UpdateProfessor_ActionFails_ReturnsNotFound(string guid, string fakeGuid)
    {
        var newProfessor = CreateTestProfessor(guid, "Stan Stan Stan");
        var newUpdateProfessor = new UpdateProfessorRequestModel()
        {
            Guid = fakeGuid,
            FullName = newProfessor.FullName,
        };
        _mockRepo.Setup(repo => repo.Update(newProfessor)).Returns(Task.FromResult(newProfessor)!);
    
        var updatedData = await _controller.UpdateProfessor(newUpdateProfessor);
        
        Assert.IsType<NotFoundResult>(updatedData);
    }
    
    [Theory]
    [InlineData("1")]
    public async Task CreateProfessor_ActionExecutes_CreatesProfessor(string guid)
    {
        var newProfessor = CreateTestProfessor(guid, "Stan Stan Stan");
        _mockRepo.Setup(repo => repo.Create(newProfessor)).Returns(Task.FromResult(newProfessor)!);
    
        var result = await _controller.CreateProfessor(newProfessor);
        
        Assert.IsType<CreatedResult>(result);
    }
    
    [Theory]
    [InlineData("1")]
    public async Task CreateProfessor_ActionFails_ReturnsBadRequest(string guid)
    {
        var newProfessor = CreateTestProfessor(guid, "Stan Stan Stan");
        var testData = GetTestProfessor(guid);
        _mockRepo.Setup(repo => repo.Exists(guid)).Returns(Task.FromResult(true));
        _mockRepo.Setup(repo => repo.Create(newProfessor)).Returns(Task.FromResult(newProfessor)!);
        
        var result = await _controller.CreateProfessor(newProfessor);
        
        Assert.IsType<BadRequestObjectResult>(result);
    }

    private Professor CreateTestProfessor(string guid = "", string fullName = "")
    {
        return new Professor()
        {
            FullName = fullName,
            Guid = guid
        };
    }
    private Professor GetTestProfessor(string guid)
    {
        return new Professor()
        {
            FullName = "John",
            Guid = guid
        };
    }
    private PagedList<Professor> GetTestProfessors(int page, int pageSize)
    {
        var source = new List<Professor>
        {
            new ()
            {
                FullName = "John",
                Guid = "1",
            },
            
            new ()
            {
                FullName = "James",
                Guid = "2",
            },
            
            new ()
            {
                FullName = "Sergey",
                Guid = "3",
            },
        };
        
        return PagedList<Professor>.ToPagedList(source.AsQueryable(), page, pageSize);
    } 
}