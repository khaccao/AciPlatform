using AciPlatform.Application.DTOs;
using AciPlatform.Application.Interfaces;
using AciPlatform.Application.Helpers;
using AciPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace AciPlatform.Application.Services;

public class WebAuthService : IWebAuthService
{
    private readonly IApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public WebAuthService(IApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<WebCustomerV2Model?> Authenticate(string username, string password)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(x => (x.Phone == username || x.Email == username) && !x.IsDeleted);

        if (customer == null || customer.PasswordHash == null || customer.PasswordSalt == null)
            return null;

        if (!VerifyPasswordHash(password, customer.PasswordHash, customer.PasswordSalt))
            return null;

        return new WebCustomerV2Model
        {
            Id = customer.Id,
            Code = customer.Code,
            Name = customer.Name,
            Avatar = customer.Avatar,
            Phone = customer.Phone,
            Email = customer.Email,
            Address = customer.Address,
            ProvinceId = customer.ProvinceId,
            DistrictId = customer.DistrictId,
            WardId = customer.WardId
        };
    }

    public async Task<WebCustomerV2Model> RegisterAccountSocial(AuthenticateSocialModel model)
    {
        var existing = await _context.Customers
            .FirstOrDefaultAsync(x => x.ProviderId == model.ProviderId && x.Provider == model.Provider);

        if (existing != null)
        {
            return new WebCustomerV2Model
            {
                Id = existing.Id,
                Code = existing.Code,
                Name = existing.Name,
                Avatar = existing.Avatar,
                Phone = existing.Phone,
                Email = existing.Email
            };
        }

        var customer = new Customer
        {
            Name = model.Name,
            Email = model.Email,
            Avatar = model.Avarta,
            Provider = model.Provider,
            ProviderId = model.ProviderId,
            Gender = model.Gender,
            Code = $"CUS{DateTime.Now:yyyyMMddHHmmss}",
            CreatedDate = DateTime.Now
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        return new WebCustomerV2Model
        {
            Id = customer.Id,
            Code = customer.Code,
            Name = customer.Name,
            Avatar = customer.Avatar,
            Phone = customer.Phone,
            Email = customer.Email
        };
    }

    public async Task<WebCustomerV2Model> Register(WebCustomerV2Model model)
    {
        if (await _context.Customers.AnyAsync(x => x.Phone == model.Phone))
            throw new Exception("Phone number already exists");

        byte[] passwordHash, passwordSalt;
        CreatePasswordHash(model.Password ?? "123456", out passwordHash, out passwordSalt);

        var customer = new Customer
        {
            Code = model.Code ?? $"CUS{DateTime.Now:yyyyMMddHHmmss}",
            Name = model.Name,
            Avatar = model.Avatar,
            Phone = model.Phone,
            Email = model.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Address = model.Address,
            ProvinceId = model.ProvinceId,
            DistrictId = model.DistrictId,
            WardId = model.WardId,
            CreatedDate = DateTime.Now
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        model.Id = customer.Id;
        model.Code = customer.Code;
        return model;
    }

    public async Task UpdateMail(WebCustomerV2Model model)
    {
        var customer = await _context.Customers.FindAsync(model.Id);
        if (customer != null)
        {
            customer.Email = model.Email;
            customer.UpdatedDate = DateTime.Now;
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<WebCustomerV2Model?> GetCustomerAsync(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null) return null;

        return new WebCustomerV2Model
        {
            Id = customer.Id,
            Code = customer.Code,
            Name = customer.Name,
            Avatar = customer.Avatar,
            Phone = customer.Phone,
            Email = customer.Email,
            Address = customer.Address,
            ProvinceId = customer.ProvinceId,
            DistrictId = customer.DistrictId,
            WardId = customer.WardId
        };
    }

    public async Task UpdateCustomerAsync(WebCustomerUpdateModel model)
    {
        var customer = await _context.Customers.FindAsync(model.Id);
        if (customer == null)
            throw new Exception("Customer not found");

        customer.Name = model.Name;
        customer.Avatar = model.Avatar;
        customer.Phone = model.Phone;
        customer.Email = model.Email;
        customer.Address = model.Address;
        customer.ProvinceId = model.ProvinceId;
        customer.DistrictId = model.DistrictId;
        customer.WardId = model.WardId;
        customer.UpdatedDate = DateTime.Now;

        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
    }

    public async Task ChangePassWordCustomerAsync(int id, string password)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null)
            throw new Exception("Customer not found");

        byte[] passwordHash, passwordSalt;
        CreatePasswordHash(password, out passwordHash, out passwordSalt);

        customer.PasswordHash = passwordHash;
        customer.PasswordSalt = passwordSalt;
        customer.UpdatedDate = DateTime.Now;

        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
    }

    public string GenerateToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"] ?? "your-secret-key"));

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddHours(8),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

    private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
        {
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != storedHash[i]) return false;
            }
        }
        return true;
    }
}

