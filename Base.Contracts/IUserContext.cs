namespace Base.Contracts;

public interface IUserContext
{
    Guid UserId { get; }
    Guid? TenantRootDepartmentId { get; }  
    Guid? DepartmentId { get; }            
    bool IsAuthenticated { get; }
    bool IsPlatformAdmin { get; }          
}