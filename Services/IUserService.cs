namespace VulnerableSecurityAPI.Services;

using System.Collections.Generic;
using System.Threading.Tasks;
using VulnerableSecurityAPI.DTOs;
using VulnerableSecurityAPI.Models;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<UserDto?> GetUserByIdAsync(int id);
    Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);
    Task<bool> UpdateUserAsync(int id, CreateUserDto updateUserDto);
    Task<bool> DeleteUserAsync(int id);
}
