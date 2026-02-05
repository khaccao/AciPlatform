using AciPlatform.Application.DTOs;
using AciPlatform.Application.Helpers;
using System.Security.Cryptography;
using AciPlatform.Application.Interfaces;
using AciPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AciPlatform.Application.Services;

public class UserService : IUserService
{
    private readonly IApplicationDbContext _context;

    public UserService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserAuthDto?> Authenticate(string username, string password)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            return null;

        var user = await _context.Users.SingleOrDefaultAsync(x => x.Username == username && !x.IsDeleted);

        if (user == null)
            return null;

        if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            return null;

        return new UserAuthDto
        {
            Id = user.Id,
            Username = user.Username,
            FullName = user.FullName ?? "",
            PasswordHash = user.PasswordHash,
            PasswordSalt = user.PasswordSalt,
            LastLogin = user.LastLogin,
            Avatar = user.Avatar ?? "",
            Status = user.Status,
            UserRoleIds = user.UserRoleIds ?? "",
            Timekeeper = user.Timekeeper ?? false,
            TargetId = user.TargetId ?? 0,
            Language = user.Language ?? ""
        };
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        return await _context.Users.Where(x => !x.IsDeleted).ToListAsync();
    }

    public async Task<User?> GetById(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User?> GetByUserName(string username)
    {
        return await _context.Users.SingleOrDefaultAsync(x => x.Username == username);
    }

    public async Task<User> Create(User user, string password)
    {
        if (string.IsNullOrWhiteSpace(password))
           throw new Exception("Password is required");

        if (await _context.Users.AnyAsync(x => x.Username == user.Username))
            throw new Exception("Username \"" + user.Username + "\" is already taken");

        byte[] passwordHash, passwordSalt;
        CreatePasswordHash(password, out passwordHash, out passwordSalt);

        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task Update(User userParam, string? password = null)
    {
        var user = await _context.Users.FindAsync(userParam.Id);

        if (user == null)
             throw new Exception("User not found");

        if (userParam.Username != user.Username)
        {
            if (await _context.Users.AnyAsync(x => x.Username == userParam.Username))
                throw new Exception("Username " + userParam.Username + " is already taken");
        }

        user.FullName = userParam.FullName;
        user.Username = userParam.Username;
        user.Email = userParam.Email;
        user.Phone = userParam.Phone;
        user.UserRoleIds = userParam.UserRoleIds;
        user.BranchId = userParam.BranchId;
        user.DepartmentId = userParam.DepartmentId;
        user.PositionDetailId = userParam.PositionDetailId;
        user.Gender = userParam.Gender;
        user.BirthDay = userParam.BirthDay;
        user.Address = userParam.Address;
        user.RequestPassword = userParam.RequestPassword;
        user.UpdatedDate = DateTime.Now;

        if (!string.IsNullOrWhiteSpace(password))
        {
             byte[] passwordHash, passwordSalt;
             CreatePasswordHash(password, out passwordHash, out passwordSalt);
             user.PasswordHash = passwordHash;
             user.PasswordSalt = passwordSalt;
        }

        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            user.IsDeleted = true; 
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateLastLogin(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user != null)
        {
            user.LastLogin = DateTime.Now;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> CheckPassword(int id, string oldPassword)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return false;
        return VerifyPasswordHash(oldPassword, user.PasswordHash, user.PasswordSalt);
    }

    public async Task UpdatePassword(int id, string newPassword)
    {
        if (string.IsNullOrWhiteSpace(newPassword))
             throw new Exception("Password is required");

        var user = await _context.Users.FindAsync(id);
        if (user == null)
             throw new Exception("User not found");

        byte[] passwordHash, passwordSalt;
        CreatePasswordHash(newPassword, out passwordHash, out passwordSalt);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        user.RequestPassword = false;
        user.UpdatedDate = DateTime.Now;

        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> RequestPasswordReset(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            return false;
        var user = await _context.Users.SingleOrDefaultAsync(x => x.Username == username && !x.IsDeleted);
        if (user == null)
            return false;

        user.RequestPassword = true;
        user.UpdatedDate = DateTime.Now;
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task ResetPasswordForMultipleUsers(List<int> ids, string newPassword)
    {
         if (ids == null || ids.Count == 0) return;
         if (string.IsNullOrWhiteSpace(newPassword)) throw new Exception("Password is required");

         var users = await _context.Users.Where(x => ids.Contains(x.Id)).ToListAsync();
         if (!users.Any()) return;

         byte[] passwordHash, passwordSalt;
         CreatePasswordHash(newPassword, out passwordHash, out passwordSalt);

         foreach(var user in users)
         {
             user.PasswordHash = passwordHash;
             user.PasswordSalt = passwordSalt;
             user.RequestPassword = false;
             user.UpdatedDate = DateTime.Now;
         }
         _context.Users.UpdateRange(users);
         await _context.SaveChangesAsync();
    }

    public async Task<object> GetPaging(UserFilterParams filterParams)
    {
        var query = _context.Users.Where(x => !x.IsDeleted);

        if (!string.IsNullOrEmpty(filterParams.Keyword))
            query = query.Where(x => x.Username.Contains(filterParams.Keyword) || (x.FullName != null && x.FullName.Contains(filterParams.Keyword)));

        // Multi-tenancy filtering
        if (!filterParams.roles.Contains("SuperAdmin"))
        {
            // If not SuperAdmin, must filter by CompanyCode
            var companyCode = filterParams.CompanyCode;
            if (string.IsNullOrEmpty(companyCode))
            {
                // Safety: if no company code in token for non-superadmin, return empty
                return new { TotalItems = 0, Data = new List<object>(), PageSize = filterParams.PageSize, CurrentPage = filterParams.CurrentPage };
            }
            
            // Join with UserCompanies to filter
            query = from u in query
                    join uc in _context.UserCompanies on u.Id equals uc.UserId
                    where uc.CompanyCode == companyCode
                    select u;
        }
        else if (!string.IsNullOrEmpty(filterParams.Keyword) || !string.IsNullOrEmpty(filterParams.CompanyCode))
        {
            // For SuperAdmin, if they provide a CompanyCode (e.g. from a filter UI), we filter.
            // But we don't force it from their token.
            if (!string.IsNullOrEmpty(filterParams.CompanyCode))
            {
                query = from u in query
                        join uc in _context.UserCompanies on u.Id equals uc.UserId
                        where uc.CompanyCode == filterParams.CompanyCode
                        select u;
            }
        }

        if (filterParams.Gender.HasValue)
            query = query.Where(x => x.Gender == filterParams.Gender);

        if (filterParams.RequestPassword.HasValue)
            query = query.Where(x => x.RequestPassword == filterParams.RequestPassword);

        if (filterParams.Ids != null && filterParams.Ids.Any())
            query = query.Where(x => filterParams.Ids.Contains(x.Id));

        var totalItems = await query.CountAsync();
        
        var users = await (from u in query
                          join d in _context.Departments on u.DepartmentId equals d.Id into deptJoin
                          from dept in deptJoin.DefaultIfEmpty()
                          join p in _context.PositionDetails on u.PositionDetailId equals p.Id into posJoin
                          from pos in posJoin.DefaultIfEmpty()
                          select new {
                              u.Id,
                              u.Username,
                              u.FullName,
                              u.Email,
                              u.Phone,
                              u.UserRoleIds,
                              u.DepartmentId,
                              u.PositionDetailId,
                              u.BirthDay,
                              u.Gender,
                              u.Address,
                              Avatar = u.Avatar,
                              u.Status,
                              u.CreatedDate,
                              DepartmentName = dept != null ? dept.Name : string.Empty,
                              PositionName = pos != null ? pos.Name : string.Empty,
                              CompanyCode = (from uc in _context.UserCompanies 
                                            where uc.UserId == u.Id 
                                            select uc.CompanyCode).FirstOrDefault()
                          })
            .OrderByDescending(x => x.CreatedDate)
            .Skip((filterParams.CurrentPage - 1) * filterParams.PageSize)
            .Take(filterParams.PageSize)
            .ToListAsync();

        return new
        {
            TotalItems = totalItems,
            Data = users,
            PageSize = filterParams.PageSize,
            CurrentPage = filterParams.CurrentPage
        };
    }

    public async Task<int> GetTotalResetPass()
    {
        return await _context.Users.CountAsync(x => !x.IsDeleted && x.RequestPassword == true);
    }

    public async Task<List<string>> GetAllUserName()
    {
        return await _context.Users
            .Where(x => !x.IsDeleted)
            .Select(x => x.Username)
            .ToListAsync();
    }

    public async Task<IEnumerable<User>> GetAllUserActive(List<string> roles, int userId)
    {
        // Return active users (non-deleted, status = 1)
        return await _context.Users
            .Where(x => !x.IsDeleted && x.Status == 1)
            .ToListAsync();
    }

    public async Task<IEnumerable<User>> GetAllUserNotRole()
    {
        // Return users without roles
        return await _context.Users
            .Where(x => !x.IsDeleted && string.IsNullOrEmpty(x.UserRoleIds))
            .ToListAsync();
    }

    public async Task UpdateCurrentYear(int year, int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user != null)
        {
            user.YearCurrent = year;
            user.UpdatedDate = DateTime.Now;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }

    private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        if (password == null) throw new ArgumentNullException("password");
        if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

        using (var hmac = new System.Security.Cryptography.HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

    private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
    {
        if (password == null) throw new ArgumentNullException("password");
        if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
        if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
        if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

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
