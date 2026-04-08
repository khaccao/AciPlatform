using AciPlatform.Application.DTOs;
using System.Collections.Generic;

namespace AciPlatform.Application.Interfaces;

public interface ITwoFactorService
{
    TwoFactorSetupResponse GenerateSetupCode(string email, string? secretKey = null);
    bool ValidateTwoFactorCode(string secretKey, string code);
    string GenerateSecretKey();
    List<string> GenerateRecoveryCodes(int count = 8);
    string GenerateQrCodeUrl(string appName, string email, string secretKey);
}

public class TwoFactorSetupResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public string? SecretKey { get; set; }
    public string? QrCodeImageUrl { get; set; }
    public List<string>? RecoveryCodes { get; set; }
}

public class TwoFactorRequest
{
    public int UserId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string? SessionId { get; set; }
}
