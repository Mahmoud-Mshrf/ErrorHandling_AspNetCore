using ErrorHandling_AspNetCore.Dtos;
using ErrorHandling_AspNetCore.Models;

namespace ErrorHandling_AspNetCore.Interfaces
{
    public interface IDriverService
    {
        Task<Driver> Register(Driver driver);
        Task<Driver> Update(Driver driver);
        Task<Driver?> GetDriver(int id);
        Task<IEnumerable<Driver>> GetDrivers();
        Task<bool> DeleteDriver(int id);
    }
}
