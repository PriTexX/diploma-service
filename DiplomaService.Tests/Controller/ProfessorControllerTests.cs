using System;
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
using Xunit.Abstractions;

namespace DiplomaService.Tests;

public class ProfessorControllerTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ProfessorControllerTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    
}