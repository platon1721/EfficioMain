namespace Base.BLL.Contracts;

public interface IBaseBLL
{
    Task<int> SaveChangesAsync();
}