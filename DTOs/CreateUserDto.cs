using System.ComponentModel.DataAnnotations;

namespace VulnerableSecurityAPI.DTOs;

public class CreateUserDto
{
    [Required]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string Password { get; set; } = string.Empty;
    
    public string Role { get; set; } = "User";
}
