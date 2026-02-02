using Microsoft.AspNetCore.Identity;

namespace Base.Domain.Identity;

public abstract class BaseUser<TKey> : IdentityUser<TKey> where TKey : IEquatable<TKey>
{
    
}