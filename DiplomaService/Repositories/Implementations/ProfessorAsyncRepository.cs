using DiplomaService.Database;
using DiplomaService.PaginationExtensions;
using DiplomaService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DiplomaService.Repositories.Implementations;

public class ProfessorAsyncRepository : BaseAsyncRepository<Professor>, IProfessorAsyncRepository
{
    public ProfessorAsyncRepository(StudentsContext context) : base(context)
    {
    }

    public override async Task<List<Professor>> GetAll()
    {
        return await Context.Professors
            .Include(p => p.Projects)!
            .ThenInclude(p => p.Student)
            .ToListAsync();
    }

    public override async Task<PagedList<Professor>> GetAll(int page, int pageSize)
    {
        return await Context.Professors
            .Include(p => p.Projects)!
            .ThenInclude(p => p.Student)
            .OrderBy(m => m.Guid)
            .ToPagedListAsync(page, pageSize);
    }
    
    public override async Task<Professor?> Get(string id)
    {
        return await Context.Professors
            .Include(p => p.Projects)!
            .ThenInclude(p => p.Student)
            .FirstOrDefaultAsync(s => s.Guid == id);
    }
}