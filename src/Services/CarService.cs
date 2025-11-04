using Microsoft.EntityFrameworkCore;
using CarShopApi.Controllers.Dtos;
using CarShopApi.Models;

namespace CarShopApi.Services;

public class CarService(AppDbContext db)
{
    private readonly AppDbContext _db = db;

    public async Task<List<Car>> GetAllAsync()
    {
        return await _db.Cars.Include(e => e.Images).ToListAsync();
    }

    public async Task<Car?> GetByIdAsync(long id)
    {
        return await _db.Cars.Where(e => e.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Car?> CreateOrUpdateAsync(CreateOrUpdateCarDto carDto)
    {
        var transact = await _db.Database.BeginTransactionAsync();
        try
        {
            Car car;


            if (carDto.Id.HasValue)
            {
                var existingCar = await _db.Cars
                    .Include(c => c.Images)
                    .FirstOrDefaultAsync(e => e.Id == carDto.Id);

                if (existingCar != null)
                {
                    existingCar.New = carDto.New;
                    existingCar.Brand = carDto.Brand;
                    existingCar.Model = carDto.Model;
                    existingCar.Year = carDto.Year;
                    existingCar.Price = carDto.Price;
                    existingCar.Color = carDto.Color;
                    existingCar.Km = carDto.Km;
                    existingCar.Description = carDto.Description;

                    await _db.SaveChangesAsync();
                    await ProcessImages(existingCar, carDto.Images);
                    await transact.CommitAsync();
                    return existingCar;
                }
            }


            car = new Car
            {
                New = carDto.New,
                Brand = carDto.Brand,
                Model = carDto.Model,
                Year = carDto.Year,
                Price = carDto.Price,
                Color = carDto.Color,
                Km = carDto.Km,
                Description = carDto.Description
            };

            await _db.Cars.AddAsync(car);
            await _db.SaveChangesAsync();
            await ProcessImages(car, carDto.Images);

            await transact.CommitAsync();
            return car;
        }
        catch
        {
            await transact.RollbackAsync();
            throw;
        }
    }

    private async Task ProcessImages(Car car, IList<CreateOrUpdateCarImageDto> imageDtos)
    {
        foreach (var imageDto in imageDtos)
        {
            if (imageDto.Id.HasValue)
            {
                var existingImage = await _db.CarImages
                    .FirstOrDefaultAsync(i => i.Id == imageDto.Id && i.CarId == car.Id);

                if (existingImage != null)
                {
                    existingImage.Url = imageDto.Url;
                    await _db.SaveChangesAsync();
                    continue;
                }
            }


            var newImage = new CarImage
            {
                Url = imageDto.Url,
                CarId = car.Id ?? 0
            };

            await _db.CarImages.AddAsync(newImage);
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