using DiplomaService.Database;
using DiplomaService.PaginationExtensions;

namespace DiplomaService.Repositories.Interfaces;

public interface IProjectAsyncRepository
{
    public Task<List<Project>> GetAll();
    public Task<PagedList<Project>> GetAll(int page, int pageSize);
    public Task<Project?> Get(string id);
    public Task<Project> Create(Project model);
    public Task<Project> Update(Project model);
    public Task Delete(string id);
    public Task<bool> Exists(string guid);
}