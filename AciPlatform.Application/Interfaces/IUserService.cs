using AciPlatform.Application.DTOs;
using AciPlatform.Domain.Entities;

namespace AciPlatform.Application.Interfaces;

public interface IUserService
{
    Task<UserAuthDto?> Authenticate(string username, string password);
    Task<User?> GetByUserName(string username);
    Task<User?> GetById(int id);
    Task<IEnumerable<User>> GetAll();
    Task<object> GetPaging(UserFilterParams filterParams);
    Task<User> Create(User user, string password);
    Task Update(User user, string? password = null);
    Task Delete(int id);
    Task UpdateLastLogin(int userId);
    Task<bool> CheckPassword(int id, string oldPassword);
    Task UpdatePassword(int id, string newPassword);
    Task<bool> RequestPasswordReset(string username);
    Task ResetPasswordForMultipleUsers(List<int> ids, string newPassword);
    Task<int> GetTotalResetPass();
    Task<List<string>> GetAllUserName();
    Task<IEnumerable<User>> GetAllUserActive(List<string> roles, int userId);
    Task<IEnumerable<User>> GetAllUserNotRole();
    Task UpdateCurrentYear(int year, int userId);
}
