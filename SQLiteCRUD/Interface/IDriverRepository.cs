using SQLiteCRUD.Models;
namespace SQLiteCRUD.Interface
{
    public interface IDriverRepository
    {
        Task AddDriver(Driver driver);
        void UpdateDriver(int driverId, Driver driver);
        void DeleteDriver(int driverId);
        IEnumerable<Driver> GetAllDrivers();
        Driver GetDriverById(int driverId);
    }
}
