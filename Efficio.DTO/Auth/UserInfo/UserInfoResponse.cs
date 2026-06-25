namespace Efficio.DTO.Auth.UserInfo;

public class UserInfoResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; } = default!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public bool IsPlatformAdmin { get; set; }
}