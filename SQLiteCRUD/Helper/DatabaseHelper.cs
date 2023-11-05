using System.Data.SQLite;
using System.Data;

namespace SQLiteCRUD.Helper
{
    public class DatabaseHelper : IDisposable
    {
        private readonly string _connectionString;
        private SQLiteConnection _connection;

        public DatabaseHelper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _connection = new SQLiteConnection(_connectionString);
        }

        public void CreateDriverTable()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand("CREATE TABLE IF NOT EXISTS Driver (Id INTEGER PRIMARY KEY AUTOINCREMENT, firstName TEXT, lastName TEXT, email TEXT, phoneNumber TEXT)", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public async Task<int> ExecuteNonQueryAsync(string query)
        {
            try
            {
                await _connection.OpenAsync();
                using (var command = new SQLiteCommand(query, _connection))
                {
                    return await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Failed to execute non-query operation.", ex);
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<DataTable> ExecuteQueryAsync(string query)
        {
            try
            {
                await _connection.OpenAsync();
                using (var command = new SQLiteCommand(query, _connection))
                {
                    var dataTable = new DataTable();
                    using (var adapter = new SQLiteDataAdapter(command))
                    {
                        await Task.Run(() => adapter.Fill(dataTable));
                    }
                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Failed to execute query operation.", ex);
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }
        public void Dispose()
        {
            _connection.Dispose();
        }
    }

    public class DatabaseException : Exception
    {
        public DatabaseException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
