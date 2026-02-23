using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace Efficio.Domain.Security;

public class Role: BaseSoftDeleteDepartmentEntity
{

    [MaxLength(128)]
    public string Name { get; set; } = default!;
    [MaxLength(500)]
    public string? Description { get; set; }
}