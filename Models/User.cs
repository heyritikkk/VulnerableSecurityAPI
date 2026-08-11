namespace VulnerableSecurityAPI.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    
    // TODO: This password field is currently a simple plain text string. 
    // It will be intentionally made vulnerable to demonstrate security issues later, 
    // and then securely hashed and salted during the remediation phase.
    public string Password { get; set; } = string.Empty;
    
    public string Role { get; set; } = "User";
}
