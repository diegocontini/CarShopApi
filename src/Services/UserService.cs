using CarShopApi.Controllers.Dtos;
using CarShopApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CarShopApi.Services;

public class UserService(AppDbContext dbContext)
{
    private readonly AppDbContext _db = dbContext;
    
    public async Task<User> CreateAsync(User user)
    {
        var existingUser = await GetAsync(user.Username);
        if (existingUser != null)
        {
            throw new Exception("Um usuário com esse login já existe.");
        }
        
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }

    public async Task<User?> UpdateAsync(long id, User user)
    {
        var existingUser = await GetAsync(id);
        if (existingUser == null)
        {
            return null;
        }

        existingUser.Username = user.Username;
        existingUser.Password = user.Password;
        existingUser.Email = user.Email;
        existingUser.ComissionPerSaleInPercent = user.ComissionPerSaleInPercent;
        existingUser.Role = user.Role;
        
        await _db.SaveChangesAsync();
        return existingUser;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var user = await GetAsync(id);
        if (user == null)
        {
            return false;
        }

        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<User?> AuthenticateLogin(LoginDto dto)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Username == dto.Username && u.Password == dto.Password);
        return user;
    }
    
    public async Task<IEnumerable<User>> Get()
    {
        return await _db.Users.ToListAsync();
    }
    
    public async Task<User?> GetAsync(long id)
    {
        return await _db.Users.FindAsync(id);
    }
    
    public async Task<User?> GetAsync(string userName)
    {
        return await _db.Users.Where(e => e.Username == userName).FirstOrDefaultAsync();
    }
}