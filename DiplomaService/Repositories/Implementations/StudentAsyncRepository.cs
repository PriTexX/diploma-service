using DiplomaService.Database;
using DiplomaService.PaginationExtensions;
using DiplomaService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DiplomaService.Repositories.Implementations;

public class StudentAsyncRepository : BaseAsyncRepository<Student>, IStudentAsyncRepository
{
    public StudentAsyncRepository(StudentsContext context) : base(context)
    {
    }

    public override async Task<List<Student>> GetAll()
    {
        return await Context.Students
            .Include(s => s.Project)
            .ThenInclude(p => p.Professor)
            .ToListAsync();
    }

    public override async Task<PagedList<Student>> GetAll(int page, int pageSize)
    {
        return await Context.Students
            .Include(s => s.Project)
            .ThenInclude(p => p.Professor)
            .OrderBy(m => m.Guid)
            .ToPagedListAsync(page, pageSize);
    }

    public override async Task<Student?> Get(string id)
    {
        return await Context.Students
            .Include(s => s.Project)
            .ThenInclude(p => p.Professor)
            .FirstOrDefaultAsync(s => s.Guid == id);
    }
}