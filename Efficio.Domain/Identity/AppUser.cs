using Base.Domain.Identity;

namespace Efficio.Domain.Identity;

public class AppUser : BaseUser<Guid>
{
    public bool IsPlatformAdmin { get; set; }
}