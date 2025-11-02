using Microsoft.EntityFrameworkCore;

namespace CarShopApi.Services;

public class CarService(AppDbContext db)
{
    private readonly AppDbContext _db = db;

    public async Task<List<Car>> GetAllAsync()
    {
        return await _db.Cars.ToListAsync();
    }

    public async Task<Car?> GetByIdAsync(long id)
    {
        return await _db.Cars.Where(e => e.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Car?> CreateOrUpdateAsync(Car car)
    {
        var transact = await _db.Database.BeginTransactionAsync();
        try
        {
            var existingCar = car.Id.HasValue ? await _db.Cars.FirstOrDefaultAsync(e => e.Id == car.Id) : null;
            if (existingCar != null)
            {
                _db.Entry(existingCar).CurrentValues.SetValues(car);
                await _db.SaveChangesAsync();
                await ProcessImages(car);
                await transact.CommitAsync();
                return existingCar;
            }

            await _db.Cars.AddAsync(car);
            await _db.SaveChangesAsync();
            await ProcessImages(car);

            await transact.CommitAsync();
            return car;
        }
        catch
        {
            await transact.RollbackAsync();
            throw;
        }
    }

    private async Task ProcessImages(Car car)
    {
        foreach (var image in car.Images)
        {
            var existingImage = await _db.CarImages.FirstOrDefaultAsync(i => i.Id == image.Id);
            if (existingImage == null)
            {
                image.CarId = car.Id ?? 0;
                await _db.CarImages.AddAsync(image);
                await _db.SaveChangesAsync();
                continue;
            }

            _db.Entry(existingImage).CurrentValues.SetValues(image);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var car = await GetByIdAsync(id);
        if (car == null)
        {
            return false;
        }

        _db.Cars.Remove(car);
        await _db.SaveChangesAsync();
        return true;
    }
}