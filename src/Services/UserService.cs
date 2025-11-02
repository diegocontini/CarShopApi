using CarShopApi.Controllers.Dtos;
using CarShopApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CarShopApi.Services;

public class UserService(AppDbContext dbContext)
{
    private readonly AppDbContext _db = dbContext;
    
    public async Task<User?> CreateOrUpdateAsync(User user)
    {
        var existingUser = user.Id.HasValue ? await GetAsync(user.Id ?? -1) : null;
        if (existingUser != null)
        {
            _db.Entry(existingUser).CurrentValues.SetValues(user);
            await _db.SaveChangesAsync();
            return existingUser;
        }

        existingUser = await GetAsync(user.Username);
        if (existingUser != null)
        {
            throw new Exception("Um usuário com esse login já existe.");
        }
        
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }

    public async Task<User?> AuthenticateLogin(LoginDto dto)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Username == dto.Username && u.Password == dto.Password);
        return user;
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