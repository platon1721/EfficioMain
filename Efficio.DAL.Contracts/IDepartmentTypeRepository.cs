using Base.DAL.Contracts;
using Efficio.DAL.DTO.Departments;

namespace Efficio.DAL.Contracts;

public interface IDepartmentTypeRepository : IBaseRepository<DepartmentType>, IDepartmentTypeRepositoryCustom
{
}

public interface IDepartmentTypeRepositoryCustom
{
    Task<DepartmentType?> FindByNameAsync(string name);
    Task<bool> NameExistsAsync(string name);
}