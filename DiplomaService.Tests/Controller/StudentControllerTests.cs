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
using Xunit.Abstractions;

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
    [InlineData(1)]
    public async Task GetStudent_ActionExecutes_ReturnsStudentById( int id)
    {
        
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