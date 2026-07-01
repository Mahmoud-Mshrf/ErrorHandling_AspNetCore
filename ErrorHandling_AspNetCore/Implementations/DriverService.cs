using ErrorHandling_AspNetCore.Data;
using ErrorHandling_AspNetCore.Dtos;
using ErrorHandling_AspNetCore.Interfaces;
using ErrorHandling_AspNetCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ErrorHandling_AspNetCore.Implementations
{
    public class DriverService : IDriverService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<Driver> _hasher;
        public DriverService(AppDbContext context, IPasswordHasher<Driver> hasher)
        {
            _context = context;
            _hasher = hasher;
        }

        public async Task<bool> DeleteDriver(int id)
        {
            var driver =await _context.Drivers.FindAsync(id);
            if (driver==null)
            {
                return false;
            }
            _context.Remove(driver);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Driver?> GetDriver(int id)
        {
            return await _context.Drivers.FindAsync(id);
        }

        public async Task<IEnumerable<Driver>> GetDrivers()
        {
            return await _context.Drivers.ToListAsync();
        }

        public async Task<Driver> Register(Driver dto)
        {
            //bool existedBefore = await _context.Drivers.AnyAsync(x => x.DriverNumber == dto.DriverNumber);
            //if (existedBefore)
            //{
            //    return new ResultDto { Errors = [$"User with DriverNumber : {dto.DriverNumber} is already existed "] };
            //}
            var driver = new Driver { FullName = dto.FullName, DriverNumber=dto.DriverNumber};
            await _context.Drivers.AddAsync(driver);
            await _context.SaveChangesAsync();
            return driver;
            //return new ResultDto { IsSuccess = true, Message = $"User added successfully with id : {user.Id}" };
        }

        public async Task<Driver> Update(Driver dto)
        {
            //var driver = await _context.Drivers.FindAsync(dto.Id);
            var result = _context.Drivers.Update(dto);
            await _context.SaveChangesAsync();
            return result.Entity;
        }
    }
}
