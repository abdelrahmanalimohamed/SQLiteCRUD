using Microsoft.Extensions.Logging;
using SQLiteCRUD.CustomLogger;
using SQLiteCRUD.Helper;
using SQLiteCRUD.Interface;
using SQLiteCRUD.Repository;
using SQLiteCRUD.Utilities;
using SQLiteCRUD.Validator;

namespace SQLiteCRUD
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.AddProvider(new FileLoggerProvider("app.log"));

            // Add services to the container.
            builder.Services.AddLogging();

            builder.Services.AddControllers();
            builder.Services.AddScoped<IDriverRepository, DriverRepository>();
            builder.Services.AddScoped<IUtilities, UtilitiesServices>();

            // Register the validator
            builder.Services.AddTransient<DriverRequestValidator>();

            builder.Services.AddSingleton<DatabaseHelper>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Create database table
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();
                var databaseHelper = new DatabaseHelper(configuration);
                databaseHelper.CreateDriverTable();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}