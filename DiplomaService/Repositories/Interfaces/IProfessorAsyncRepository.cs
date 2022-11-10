using DiplomaService.Database;
using DiplomaService.PaginationExtensions;

namespace DiplomaService.Repositories.Interfaces;

public interface IProfessorAsyncRepository
{
    public Task<List<Professor>> GetAll();
    public Task<PagedList<Professor>> GetAll(int page, int pageSize);
    public Task<Professor?> Get(string id);
    public Task<Professor> Create(Professor model);
    public Task<Professor> Update(Professor model);
    public Task Delete(string id);
    public Task<bool> Exists(string guid);
}