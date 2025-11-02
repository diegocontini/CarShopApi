namespace CarShopApi.Models;

public class User
{
    public long? Id { get; set; }
    
    public byte? ComissionPerSaleInPercent { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }   
    public UserRole Role { get; set; } 
    
}

public enum UserRole
{
    Admin = 0,
    Vendor = 1
}