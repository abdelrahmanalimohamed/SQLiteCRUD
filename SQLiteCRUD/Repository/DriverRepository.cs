using SQLiteCRUD.Helper;
using SQLiteCRUD.Interface;
using SQLiteCRUD.Models;
using System.Data;
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

        public async Task<int> AddDriver(DriverRequest driver)
        {
            string query = $"INSERT INTO Driver (FirstName, LastName, Email, PhoneNumber) " +
                           $"VALUES ('{driver.firstName}', '{driver.lastName}', '{driver.email}', '{driver.phoneNumber}')";

            try
            {
                return  await _databaseHelper.ExecuteNonQueryAsync(query);
            }
            catch (DatabaseException ex)
            {
                // Handle the exception (log, throw, etc.)
                throw new DatabaseException("Failed to add driver to the database.", ex);
            }
        }

        public async Task DeleteDriver(int driverId)
        {
            string query = $"DELETE FROM Driver WHERE Id = "+driverId+"";
           await _databaseHelper.ExecuteNonQueryAsync(query);
        }

        public async Task<Driver> GetDriverById(int driverId)
        {
            string query = $"SELECT  Id , firstName , lastName , email , phoneNumber FROM Driver WHERE Id = "+driverId+"";
            var dataTable = await _databaseHelper.ExecuteQueryAsync(query);

            if (dataTable.Rows.Count > 0)
            {
                var row = dataTable.Rows[0];
                return new Driver
                {
                    Id = Convert.ToInt32(row["Id"]),
                    firstName = row["firstName"].ToString(),
                    lastName = row["lastName"].ToString(),
                    email = row["email"].ToString(),
                    phoneNumber = row["phoneNumber"].ToString()
                };
            }

            return null;
        }

        public async Task<int> UpdateDriver(int driverId, DriverRequest driver)
        {
            string query = $"UPDATE Driver SET firstName = '"+driver.firstName+"', lastName = '"+driver.lastName+"', email = '"+driver.email+"', phoneNumber = '"+driver.phoneNumber+"' WHERE Id = "+driverId+"";
            return await _databaseHelper.ExecuteNonQueryAsync(query);
        }

        public async Task<IEnumerable<Driver>> GetAllDrivers()
        {
            string query = "SELECT Id , firstName , lastName , email , phoneNumber FROM Driver";
            var dataTable = await _databaseHelper.ExecuteQueryAsync(query);
            var drivers = new List<Driver>();

            foreach (DataRow row in dataTable.Rows)
            {
                drivers.Add(new Driver
                {
                    Id = Convert.ToInt32(row["Id"]),
                    firstName = row["firstName"].ToString(),
                    lastName = row["lastName"].ToString(),
                    email = row["email"].ToString(),
                    phoneNumber = row["phoneNumber"].ToString()
                });
            }

            return drivers;
        }


    }
}
