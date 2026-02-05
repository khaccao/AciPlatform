namespace AciPlatform.Application.DTOs;

public class IdentityUser
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string CompanyCode { get; set; } = string.Empty;
}
