using AciPlatform.Application.DTOs;
using AciPlatform.Domain.Entities;
using System.Security.Claims;

namespace AciPlatform.Application.Interfaces;

public interface IWebAuthService
{
    Task<WebCustomerV2Model?> Authenticate(string username, string password);
    Task<WebCustomerV2Model> RegisterAccountSocial(AuthenticateSocialModel model);
    Task<WebCustomerV2Model> Register(WebCustomerV2Model model);
    Task UpdateMail(WebCustomerV2Model model);
    Task<WebCustomerV2Model?> GetCustomerAsync(int id);
    Task UpdateCustomerAsync(WebCustomerUpdateModel model);
    Task ChangePassWordCustomerAsync(int id, string password);
    string GenerateToken(List<Claim> authClaims);
}
