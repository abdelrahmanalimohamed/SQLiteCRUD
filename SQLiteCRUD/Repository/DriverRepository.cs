using SQLiteCRUD.Helper;
using SQLiteCRUD.Interface;
using SQLiteCRUD.Models;
using System.Data.SQLite;

namespace SQLiteCRUD.Repository
{
    public class DriverRepository : IDriverRepository
    {
        private readonly DatabaseHelper _databaseHelper;

        public DriverRepository(IConfiguration configuration)
        {
            _databaseHelper = new DatabaseHelper(configuration);
        }

        public async Task AddDriver(Driver driver)
        {
            string query = $"INSERT INTO Driver (FirstName, LastName, Email, PhoneNumber) " +
                           $"VALUES ('{driver.firstName}', '{driver.lastName}', '{driver.email}', '{driver.phoneNumber}')";

            try
            {
                await _databaseHelper.ExecuteNonQueryAsync(query);
            }
            catch (DatabaseException ex)
            {
                // Handle the exception (log, throw, etc.)
                throw new DatabaseException("Failed to add driver to the database.", ex);
            }
        }

        public void DeleteDriver(int driverId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Driver> GetAllDrivers()
        {
            throw new NotImplementedException();
        }

        public Driver GetDriverById(int driverId)
        {
            throw new NotImplementedException();
        }

        public void UpdateDriver(int driverId, Driver driver)
        {
            throw new NotImplementedException();
        }
    }
}
