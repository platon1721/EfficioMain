using Base.Contracts;
namespace Base.Domain;

public class SystemClock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}