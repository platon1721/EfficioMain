// Base.Domain/Identity/BaseRefreshToken.cs
namespace Base.Domain.Identity;

public abstract class BaseRefreshToken<TKey, TUser> : BaseEntity<TKey>
    where TKey : IEquatable<TKey>
    where TUser : class
{
    public TKey UserId { get; set; } = default!;
    public TUser? User { get; set; }
    
    public string Token { get; set; } = Guid.NewGuid().ToString();
    public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddDays(7);
    
    // Token rotation
    public string? PreviousToken { get; set; }
    public DateTime? PreviousExpiresAt { get; set; }
    
    // Revocation
    public bool IsRevoked { get; private set; }
    public DateTime? RevokedAt { get; private set; }
    
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsActive => !IsRevoked && !IsExpired;
    
    /// <summary>
    /// Rotates the token - saves current as previous and generates new one.
    /// </summary>
    public void Rotate(TimeSpan? newTokenLifetime = null)
    {
        PreviousToken = Token;
        PreviousExpiresAt = ExpiresAt;
        
        Token = Guid.NewGuid().ToString();
        ExpiresAt = DateTime.UtcNow.Add(newTokenLifetime ?? TimeSpan.FromDays(7));
    }
    
    /// <summary>
    /// Revokes this token (e.g., on logout or security breach).
    /// </summary>
    public void Revoke()
    {
        IsRevoked = true;
        RevokedAt = DateTime.UtcNow;
    }
    
    /// <summary>
    /// Checks if the given token matches current or previous (for rotation grace period).
    /// </summary>
    public bool MatchesToken(string token)
    {
        if (Token == token) return true;
        
        // Allow previous token within grace period (e.g., race conditions)
        if (PreviousToken == token && PreviousExpiresAt > DateTime.UtcNow)
            return true;
            
        return false;
    }
}

public abstract class BaseRefreshToken<TUser> : BaseRefreshToken<Guid, TUser>
    where TUser : class
{
}