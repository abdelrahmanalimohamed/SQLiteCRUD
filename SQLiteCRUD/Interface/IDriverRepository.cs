using SQLiteCRUD.Models;
namespace SQLiteCRUD.Interface
{
    public interface IDriverRepository
    {
        Task<int> AddDriver(DriverRequest driver);
        Task<int> UpdateDriver(int driverId, DriverRequest driver);
        Task<int> DeleteDriver(int driverId);
        Task<IEnumerable<Driver>> GetAllDrivers();
        Task<Driver> GetDriverById(int driverId);
     
    }
}
