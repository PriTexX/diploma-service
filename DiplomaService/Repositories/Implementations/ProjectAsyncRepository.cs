using DiplomaService.Database;
using DiplomaService.PaginationExtensions;
using DiplomaService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DiplomaService.Repositories.Implementations;

public class ProjectAsyncRepository : BaseAsyncRepository<Project>, IProjectAsyncRepository
{
    public ProjectAsyncRepository(StudentsContext context) : base(context)
    {
    }
    
    public override async Task<List<Project>> GetAll()
    {
        return await Context.Projects
            .Include(p => p.Professor)
            .Include(p => p.Student)
            .ToListAsync();
    }

    public override async Task<PagedList<Project>> GetAll(int page, int pageSize)
    {
        return await Context.Projects
            .Include(p => p.Professor)!
            .Include(p => p.Student)
            .OrderBy(m => m.Guid)
            .ToPagedListAsync(page, pageSize);
    }
    
    public override async Task<Project?> Get(string id)
    {
        return await Context.Projects
            .Include(p => p.Student)!
            .Include(p => p.Professor)
            .FirstOrDefaultAsync(s => s.Guid == id);
    }
}