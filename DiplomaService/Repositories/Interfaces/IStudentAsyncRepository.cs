using DiplomaService.Database;
using DiplomaService.PaginationExtensions;

namespace DiplomaService.Repositories.Interfaces;

public interface IStudentAsyncRepository
{
    public Task<List<Student>> GetAll();
    public Task<PagedList<Student>> GetAll(int page, int pageSize);
    public Task<Student?> Get(string id);
    public Task<Student> Create(Student model);
    public Task<Student> Update(Student model);
    public Task Delete(string id);
    public Task<bool> Exists(string guid);
}