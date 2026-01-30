using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace AciPlatform.Application.DTOs;

public class ObjectReturn
{
    public int status { get; set; }
    public string message { get; set; } = string.Empty;
    public object? data { get; set; }
}

public class AuthenticateModel
{
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? CompanyCode { get; set; }
}

public class ResetPasswordModel
{
    public string? Username { get; set; }
}

public class RegisterModel
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? UserRoleIds { get; set; }
}

public class PasswordModel
{
    public int Id { get; set; }
    public string OldPassword { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty; // New password
}

public class UserAuthDto
{
    public byte[] PasswordHash { get; set; } = null!;
    public byte[] PasswordSalt { get; set; } = null!;
    public string FullName { get; set; } = string.Empty;
    public int Id { get; set; }
    public DateTime? LastLogin { get; set; }
    public string Avatar { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public int Status { get; set; }
    public string UserRoleIds { get; set; } = string.Empty;
    public bool Timekeeper { get; set; }
    public int TargetId { get; set; }
    public string Language { get; set; } = string.Empty;
}

public class SelectTypePayModel
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}

public class ForgotPasswordRequest
{
    public string? Username { get; set; }
}

public class ChangePasswordRequest
{
    public int Id { get; set; }
    public string OldPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}

public class RegisterRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? UserRoleIds { get; set; }
    public string? CompanyCode { get; set; }
}

public class RefreshRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}

public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class CreateUserRequest
{
    public string Username { get; set; } = string.Empty;
    public string? Password { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? UserRoleIds { get; set; }
    [JsonPropertyName("departmentId")]
    [JsonProperty("departmentId")]
    public int? DepartmentId { get; set; }

    [JsonPropertyName("positionDetailId")]
    [JsonProperty("positionDetailId")]
    public int? PositionDetailId { get; set; }

    [JsonPropertyName("gender")]
    [JsonProperty("gender")]
    public int? Gender { get; set; }

    [JsonPropertyName("birthDay")]
    [JsonProperty("birthDay")]
    public DateTime? BirthDay { get; set; }

    [JsonPropertyName("address")]
    [JsonProperty("address")]
    public string? Address { get; set; }
}


public class UserMenuAssignDto
{
    public int MenuId { get; set; }
    public string? MenuCode { get; set; }
    public bool View { get; set; }
    public bool Add { get; set; }
    public bool Edit { get; set; }
    public bool Delete { get; set; }
}

