using AciPlatform.Application.DTOs;

namespace AciPlatform.Application.Interfaces;

public interface IUserService
{
    Task<UserDto?> GetUserByIdAsync(int id);
    Task<UserDto?> GetUserByEmailAsync(string email);
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<int> CreateUserAsync(string firstName, string lastName, string email);
    Task UpdateUserAsync(int id, string firstName, string lastName);
    Task DeleteUserAsync(int id);
}
