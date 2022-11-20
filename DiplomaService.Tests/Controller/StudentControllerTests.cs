using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly Mock<IStudentAsyncRepository> _mockRepo;

    public StudentControllerTests(ITestOutputHelper testOutputHelper, Mock<IStudentAsyncRepository> mockRepo)
    {
        _testOutputHelper = testOutputHelper;
        _mockRepo = mockRepo;
    }

    [Theory]
    [InlineData(1,2)]
    public async Task GetStudents_GetsAllStudents_ReturnsAllStudents( int page, int pageSize)
    {
        _mockRepo.Setup(repo => repo.GetAll(page, pageSize)).Returns(Task.FromResult(GetTestStudents(page, pageSize)));
        
        var controller = new StudentController(_mockRepo.Object);

        var data = await _mockRepo.Object.GetAll(page, pageSize);

        var result = await controller.GetStudents(new PaginationQuery{Page = page, PageSize = pageSize});

        var objectResult = Assert.IsType<ObjectResult>(result);
        var studentResponse = Assert.IsAssignableFrom<IEnumerable<StudentResponseModel>>(objectResult.Value);
        Assert.Equal(GetTestStudents(page, pageSize).Count, studentResponse.Count());
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