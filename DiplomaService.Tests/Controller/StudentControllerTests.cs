using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiplomaService.Controllers;
using DiplomaService.Controllers.Student;
using DiplomaService.Database;
using DiplomaService.PaginationExtensions;
using DiplomaService.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace DiplomaService.Tests.Controller;

public class StudentControllerTests
{
    private readonly StudentController _controller;
    private readonly Mock<IStudentAsyncRepository> _mockRepo;
    private readonly Mock<IResponseHandler> _responseMock;

    public StudentControllerTests()
    {
        _mockRepo = new Mock<IStudentAsyncRepository>();
        _responseMock = new Mock<IResponseHandler>();
        _controller = new StudentController(_mockRepo.Object, _responseMock.Object);
    }

    [Theory]
    [InlineData(1,2)]
    public async Task GetStudents_ActionExecutes_ReturnsAllStudents( int page, int pageSize)
    {
        var testData = GetTestStudents(page, pageSize);
        _mockRepo.Setup(repo => repo.GetAll(page, pageSize)).Returns(Task.FromResult(testData));
        
        var result = await _controller.GetStudents(new PaginationQuery{Page = page, PageSize = pageSize});

        var objectResult = Assert.IsType<ObjectResult>(result);
        var studentResponse = Assert.IsAssignableFrom<IEnumerable<StudentResponseModel>>(objectResult.Value);
        Assert.Equal(testData.Count, studentResponse.Count());
    }
    
    [Theory]
    [InlineData("1")]
    public async Task GetStudent_ActionExecutes_ReturnsStudentById(string guid) 
    {
        var testData = GetTestStudent(guid);
        _mockRepo.Setup(repo => repo.Get(guid)).Returns(Task.FromResult(testData)!);

        var result = await _controller.GetStudent(guid);

        var objectResult = Assert.IsType<ObjectResult>(result);
        var studentResponse = Assert.IsAssignableFrom<StudentResponseModel>(objectResult.Value);
        Assert.True(testData == studentResponse);
    }
    
    [Theory]
    [InlineData("1", "2")]
    public async Task GetStudent_ActionFails_ReturnsNotFound(string realGuid, string fakeGuid) 
    {
        var realTestStudent = GetTestStudent(realGuid);
        _mockRepo.Setup(repo => repo.Get(realGuid)).Returns(Task.FromResult(realTestStudent)!);

        var result = await _controller.GetStudent(fakeGuid);

        Assert.IsType<NotFoundResult>(result);
    }
    
    [Theory]
    [InlineData("1")]
    public async Task DeleteStudent_ActionExecutes_DeletesStudentById(string guid) 
    {
        var testData = GetTestStudent(guid);
        _mockRepo.Setup(repo => repo.Delete(guid)).Returns(Task.FromResult(testData)!);

        var result = await _controller.DeleteStudent(guid);
        
        Assert.IsType<OkResult>(result);
    }
    
    [Theory]
    [InlineData("1")]
    public async Task UpdateStudent_ActionExecutes_UpdatesStudent(string guid)
    {
        var newStudent = CreateTestStudent(guid, "211-545", 2, "Stan Stan Stan");
        var newUpdateStudent = new UpdateStudentRequestModel()
        {
            Guid = newStudent.Guid,
            Group = newStudent.Group,
            Course = newStudent.Course,
            FullName = newStudent.FullName,
            ProjectId = "12132"
        };
        var testData = GetTestStudent(guid);
        _mockRepo.Setup(repo => repo.Get(guid)).Returns(Task.FromResult(testData)!);
        _mockRepo.Setup(repo => repo.Update(newStudent)).Returns(Task.FromResult(newStudent)!);

        var updatedData = await _controller.UpdateStudent(newUpdateStudent);
        
        Assert.IsType<OkResult>(updatedData);
    }
    
    [Theory]
    [InlineData("1", "2")]
    public async Task UpdateStudent_ActionFails_ReturnsNotFound(string guid, string fakeGuid)
    {
        var newStudent = CreateTestStudent(guid, "211-545", 2, "Stan Stan Stan");
        var newUpdateStudent = new UpdateStudentRequestModel()
        {
            Guid = fakeGuid,
            Group = newStudent.Group,
            Course = newStudent.Course,
            FullName = newStudent.FullName,
            ProjectId = "12132"
        };
        _mockRepo.Setup(repo => repo.Update(newStudent)).Returns(Task.FromResult(newStudent)!);

        var updatedData = await _controller.UpdateStudent(newUpdateStudent);
        
        Assert.IsType<NotFoundResult>(updatedData);
    }
    
    [Theory]
    [InlineData("1")]
    public async Task CreateStudent_ActionExecutes_CreatesStudent(string guid)
    {
        var newStudent = CreateTestStudent(guid, "211-545", 2, "Stan Stan Stan");
        _mockRepo.Setup(repo => repo.Create(newStudent)).Returns(Task.FromResult(newStudent)!);

        var result = await _controller.CreateStudent(newStudent);
        
        Assert.IsType<CreatedResult>(result);
    }
    
    [Theory]
    [InlineData("1")]
    public async Task CreateStudent_ActionFails_ReturnsBadRequest(string guid)
    {
        var newStudent = CreateTestStudent(guid, "211-545", 2, "Stan Stan Stan");
        _mockRepo.Setup(repo => repo.Exists(guid)).Returns(Task.FromResult(true));
        _mockRepo.Setup(repo => repo.Create(newStudent)).Returns(Task.FromResult(newStudent)!);
        
        var result = await _controller.CreateStudent(newStudent);
        
        Assert.IsType<BadRequestObjectResult>(result);
    }

    private Student CreateTestStudent(string guid = "", string group = "", int course = -1, string fullName = "")
    {
        return new Student()
        {
            FullName = fullName,
            Course = course,
            Group = group,
            Guid = guid
        };
    }
    private Student GetTestStudent(string guid)
    {
        return new Student()
        {
            FullName = "John",
            Course = 1,
            Group = "211-721",
            Guid = guid
        };
    }
    private PagedList<Student> GetTestStudents(int page, int pageSize)
    {
        var source = new List<Student>
        {
            new ()
            {
                FullName = "John",
                Course = 1,
                Group = "211-721",
                Guid = "1",
            },
            
            new ()
            {
                FullName = "James",
                Course = 2,
                Group = "200-341",
                Guid = "2",
            },
            
            new ()
            {
                FullName = "Sergey",
                Course = 3,
                Group = "191-453",
                Guid = "3",
            },
        };
        
        return PagedList<Student>.ToPagedList(source.AsQueryable(), page, pageSize);
    } 
}