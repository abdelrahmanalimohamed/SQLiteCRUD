using Microsoft.Extensions.Configuration;
using SQLiteCRUD.Helper;
using System.Data;
using System.Data.SQLite;
using Xunit;

namespace SQLiteCRUD.UnitTestings.Tests
{
    public class DatabaseHelperTests
    {
        private readonly IConfiguration _configuration;

        public DatabaseHelperTests()
        {
            // Create IConfiguration instance for testing
            _configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
        }

           [Fact]
            public void CreateDriverTable_ShouldCreateTable()
            {
                // Arrange
            var databaseHelper = new DatabaseHelper(_configuration);

            using (var connection = new SQLiteConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    // Act
                    databaseHelper.CreateDriverTable();

                    // Assert
                    using (var command = new SQLiteCommand("PRAGMA table_info(Driver)", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            Assert.True(reader.HasRows);
                        }
                    }
                }
            }

        [Fact]
        public async Task ExecuteNonQueryAsync_ShouldReturnAffectedRowsCount()
        {
            // Arrange

            var databaseHelper = new DatabaseHelper(_configuration);

            // Act
            int affectedRows = await databaseHelper.ExecuteNonQueryAsync("INSERT INTO Driver (firstName, lastName) VALUES ('John', 'Doe')");

            // Assert
            Assert.Equal(1, affectedRows);
        }

        [Fact]
        public async Task ExecuteQueryAsync_ShouldReturnDataTable()
        {
            // Arrange

            var databaseHelper = new DatabaseHelper(_configuration);

            // Act
            DataTable dataTable = await databaseHelper.ExecuteQueryAsync("SELECT * FROM Driver");

            // Assert
            Assert.NotNull(dataTable);
            Assert.NotEmpty(dataTable.Rows);
        }

    }
}