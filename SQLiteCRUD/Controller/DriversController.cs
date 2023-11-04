using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SQLiteCRUD.Interface;
using SQLiteCRUD.Models;
using SQLiteCRUD.Validator;

namespace SQLiteCRUD.Controller
{
    [Route("api/drivers")]
    [ApiController]
    public class DriversController : ControllerBase
    {
        private readonly IDriverRepository _driverRepository;
        private readonly ILogger<DriversController> _logger;
        private readonly DriverRequestValidator _validator;

        public DriversController(IDriverRepository driverRepository, ILogger<DriversController> logger, DriverRequestValidator validator)
        {
            _driverRepository = driverRepository;
            _logger = logger;
            _validator = validator;
        }

        [HttpPost("random")]
        public IActionResult InsertRandomNames()
        {
            _driverRepository.InsertRandomNames(10);
            return Ok("Random names inserted successfully.");
        }

        [HttpGet("alphabetical")]
        public async Task<IActionResult> GetUsersAlphabetically()
        {
            var sortedDrivers = await _driverRepository.GetUsersAlphabetically();
            return Ok(sortedDrivers);
        }

        [HttpGet("alphabetized")]
        public async Task<IActionResult> GetAlphabetizedName(int driverId)
        {
            var alphabetizedName = await _driverRepository.GetAlphabetizedName(driverId);
            return Ok(alphabetizedName);
        }

        [HttpPost]
        public async Task<IActionResult> AddDriver([FromBody] DriverRequest driverRequest)
        {

            ValidationResult validationResult = _validator.Validate(driverRequest);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
            }

            try
            {
                // Add the driver using the repository
                await _driverRepository.AddDriver(driverRequest);

                return Ok("Driver added successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Error occurred while adding a driver.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDriver(int id)
        {
            try
            {
                // Check if the driver with the specified id exists
                var existingDriver = await _driverRepository.GetDriverById(id);

                if (existingDriver == null)
                {
                    return NotFound("Driver not found.");
                }

                // Delete the driver using the repository
                await _driverRepository.DeleteDriver(id);

                return Ok("Driver deleted successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Error occurred while deleting a driver.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDriver(int id, [FromBody] DriverRequest driverRequest)
        {
            ValidationResult validationResult = _validator.Validate(driverRequest);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
            }

            try
            {
                // Check if the driver with the specified id exists
                var existingDriver = await _driverRepository.GetDriverById(id);

                if (existingDriver == null)
                {
                    return NotFound("Driver not found.");
                }

                // Update the driver using the repository
                await _driverRepository.UpdateDriver(id, driverRequest);

                return Ok("Driver updated successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Error occurred while updating a driver.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDriver(int id)
        {
            try
            {
                // Retrieve the driver by id using the repository
                var driver = await _driverRepository.GetDriverById(id);

                if (driver == null)
                {
                    return NotFound("Driver not found.");
                }

                return Ok(driver);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Error occurred while retrieving a driver.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDrivers()
        {
            try
            {
                var drivers = await _driverRepository.GetAllDrivers();
                return Ok(drivers);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Error occurred while retrieving all drivers.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
