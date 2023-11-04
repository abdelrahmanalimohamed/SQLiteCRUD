using SQLiteCRUD.Interface;
using SQLiteCRUD.Models;

namespace SQLiteCRUD.Utilities
{
    public class UtilitiesServices : IUtilities
    {
        private readonly IDriverRepository _driverRepository;
        public UtilitiesServices(IDriverRepository driverRepository)
        {
            _driverRepository = driverRepository;
        }
        public async void InsertRandomNames(int numberOfNames)
        {
            var random = new Random();
            for (int i = 0; i < numberOfNames; i++)
            {
                var randomFirstName = GenerateRandomString(random, 8);
                var randomLastName = GenerateRandomString(random, 8);
                var randomEmail = $"{randomFirstName.ToLower()}.{randomLastName.ToLower()}@example.com";
                var randomPhoneNumber = GenerateRandomString(random, 10, "0123456789");

                var newDriver = new DriverRequest
                {
                    firstName = randomFirstName,
                    lastName = randomLastName,
                    email = randomEmail,
                    phoneNumber = randomPhoneNumber
                };

                await _driverRepository.AddDriver(newDriver);
            }
        }
        public async Task<List<Driver>> GetUsersAlphabetically()
        {
            var drivers = await _driverRepository.GetAllDrivers();
            var sortedDrivers = drivers.OrderBy(driver => driver.lastName).ThenBy(driver => driver.firstName).ToList();
            return sortedDrivers;

        }

        public async Task<string> GetAlphabetizedName(int driverId)
        {
            var singleDriver = await _driverRepository.GetDriverById(driverId);
            var alphabetizedFirstName = new string(singleDriver.firstName.ToLower().OrderBy(c => c).ToArray());
            var alphabetizedLastName = new string(singleDriver.lastName.ToLower().OrderBy(c => c).ToArray());
            return $"{alphabetizedFirstName} {alphabetizedLastName}";
        }

        private string GenerateRandomString(Random random, int length, string allowedChars = "abcdefghijklmnopqrstuvwxyz")
        {
            var chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = allowedChars[random.Next(allowedChars.Length)];
            }
            return new string(chars);
        }
    }
}
