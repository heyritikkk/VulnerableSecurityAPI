namespace VulnerableSecurityAPI.Services;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VulnerableSecurityAPI.Data;
using VulnerableSecurityAPI.DTOs;
using VulnerableSecurityAPI.Models;

public class UserService : IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        return await _context.Users
            .Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                Role = u.Role
            }).ToListAsync();
    }

    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return null;

        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role
        };
    }

    public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
    {
        var user = new User
        {
            Username = createUserDto.Username,
            Email = createUserDto.Email,
            Password = createUserDto.Password, // TODO: Hash password securely later
            Role = createUserDto.Role
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role
        };
    }

    public async Task<bool> UpdateUserAsync(int id, CreateUserDto updateUserDto)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return false;

        user.Username = updateUserDto.Username;
        user.Email = updateUserDto.Email;
        user.Password = updateUserDto.Password; // TODO: Hash password securely later
        user.Role = updateUserDto.Role;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }
}
