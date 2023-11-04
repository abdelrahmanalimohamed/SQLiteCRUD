using SQLiteCRUD.Models;
namespace SQLiteCRUD.Interface
{
    public interface IDriverRepository
    {
        Task AddDriver(DriverRequest driver);
        Task UpdateDriver(int driverId, DriverRequest driver);
        Task DeleteDriver(int driverId);
        Task<IEnumerable<Driver>> GetAllDrivers();
        Task<Driver> GetDriverById(int driverId);
     
    }
}
