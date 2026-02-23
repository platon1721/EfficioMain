namespace Base.Contracts;

public interface IClock
{
    DateTime UtcNow { get; }
}