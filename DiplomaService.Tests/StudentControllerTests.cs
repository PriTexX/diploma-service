using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DiplomaService.Controllers.Student;
using DiplomaService.Database;
using DiplomaService.PaginationExtensions;
using DiplomaService.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace DiplomaService.Tests;

public class StudentControllerTests
{
    [Fact]
    public async Task GetAllStudentsReturnsAllStudents()
    {
        const int page = 1;
        const int pageSize = 2;
        
        var mock = new Mock<IStudentAsyncRepository>();
        mock.Setup(repo => repo.GetAll(page, pageSize)).Returns(Task.FromResult(GetTestStudents(page, pageSize)));
        
        var controller = new StudentController(mock.Object);
        controller.Response = ;
        
        var data = await mock.Object.GetAll(page, pageSize);

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