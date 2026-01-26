using System.ComponentModel.DataAnnotations;

namespace AciPlatform.Domain.Entities.Auth;

public class RefreshToken
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }
    [Required]
    [MaxLength(200)]
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? RevokedAt { get; set; }
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsRevoked => RevokedAt.HasValue;
}
