using DiplomaService.Database;
using DiplomaService.PaginationExtensions;
using Microsoft.EntityFrameworkCore;

namespace DiplomaService.Repositories.Interfaces;

public abstract class BaseAsyncRepository<TDbModel> where TDbModel : BaseModel
{
    protected readonly StudentsContext Context;

    protected BaseAsyncRepository(StudentsContext context)
    {
        Context = context;
    }

    public virtual async Task<List<TDbModel>> GetAll()
    {
        return await Context.Set<TDbModel>().ToListAsync();
    }

    public virtual async Task<PagedList<TDbModel>> GetAll(int page, int pageSize)
    {
        return await Context.Set<TDbModel>().OrderBy(m => m.Guid).ToPagedListAsync(page, pageSize);
    }

    public virtual async Task<TDbModel?> Get(string id)
    {
        return await Context.Set<TDbModel>().FirstOrDefaultAsync(m => m.Guid == id);
    }

    public virtual async Task<TDbModel> Create(TDbModel model)
    {
        await Context.Set<TDbModel>().AddAsync(model);
        await Context.SaveChangesAsync();
        return model;
    }

    public virtual async Task<TDbModel> Update(TDbModel model)
    {
        Context.Set<TDbModel>().Update(model);
        await Context.SaveChangesAsync();
        
        return model;
    }

    public virtual async Task Delete(string id)
    {
        var toDelete = await Context.Set<TDbModel>().FirstOrDefaultAsync(m => m.Guid == id);

        if (toDelete is null)
        {
            throw new ArgumentException($"Model with guid: {id} doesn't exist");
        } 
        
        Context.Set<TDbModel>().Remove(toDelete);
        await Context.SaveChangesAsync();
    }

    public virtual async Task<bool> Exists(string guid)
    {
        var exists = await Context.Set<TDbModel>()
            .AsNoTracking()
            .Select(d => d.Guid)
            .FirstOrDefaultAsync(d => d == guid);
        
        return exists is null;
    }
}