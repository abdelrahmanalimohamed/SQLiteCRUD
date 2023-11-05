using Microsoft.Extensions.Configuration;
using SQLiteCRUD.Interface;
using SQLiteCRUD.Models;
using SQLiteCRUD.Repository;

namespace SQLiteCRUD.UnitTestings.Tests
{
    public class DriverRepositoryTests
    {
        private readonly IConfiguration _configuration;

        private readonly IDriverRepository _driverRepository;

        public DriverRepositoryTests()
        {
            // Create IConfiguration instance for testing
            _configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            _driverRepository = new DriverRepository(_configuration);
        }


        [Fact]
        public async Task AddDriverAsync_ShouldAddDriverToDatabase()
        {
            // Arrange
            var driverRequest = new DriverRequest { firstName = "John", lastName = "Doe" };

            // Act
               var added = await _driverRepository.AddDriver(driverRequest);

            // Assert
            Assert.Equal(1, added);
        }

        [Fact]
        public async Task UpdateDriver_ShouldUpdateDriverInDatabase()
        {
            // Arrange
            int driverId = 5;
            var updatedDriverRequest = new DriverRequest
            {
                // Initialize updated driver request properties
                firstName = "UpdatedFirstName",
                lastName = "UpdatedLastName",
                email = "updated@example.com",
                phoneNumber = "555-555-5555"
            };

            // Act
            var updated = await _driverRepository.UpdateDriver(driverId, updatedDriverRequest);

            // Assert
            Assert.Equal(1, updated);
          
        }

        [Fact]
        public async Task GetAllDrivers_ShouldReturnAllDriversFromDatabase()
        {
            // Act
            var drivers = await _driverRepository.GetAllDrivers();

            // Assert
            Assert.NotNull(drivers);
            Assert.NotEmpty(drivers);
        }

        [Fact]
        public async Task GetDriverById_ShouldReturnDriverFromDatabase()
        {
            // Arrange
            int driverId = 3;

            // Act
            var driver = await _driverRepository.GetDriverById(driverId);

            // Assert
            Assert.NotNull(driver);
            Assert.Equal(driverId, driver.Id);
        }

        [Fact]
        public async Task DeleteDriver_ShouldDeleteDriverFromDatabase()
        {
            // Arrange
            int driverId = 9;

            // Act
            await _driverRepository.DeleteDriver(driverId);

            // Assert
            var deletedDriver = await _driverRepository.GetDriverById(driverId);
            Assert.Null(deletedDriver);
        }


    }
}

